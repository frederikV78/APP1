using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App_project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteKeyword : Page
    {
        public DeleteKeyword()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Load the list of keywords from the Keywords table
            try
            {
                string query = "SELECT * FROM Keywords ORDER BY Name;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        List<string> LijstKeywords = new List<string>();

                        int i = 0;
                        Debug.WriteLine("   *** START db items ***   ");
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            i++;
                            string keyword = (string)statement[1];
                            LijstKeywords.Add(keyword);
                        }
                        if (i == 0)
                        {
                            LijstKeywords.Add("Nothing to show!");
                        }
                        else
                        {
                            Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
                        }
                        listbox1.ItemsSource = LijstKeywords;
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //BACK BUTTON
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton1_Click(object sender, RoutedEventArgs e) //DELETE BUTTON
        {
            string selection = listbox1.SelectedItem.ToString();

            if (selection != "Nothing to show!" && selection != "")
            {
                Debug.WriteLine(" ***   Selection to delete: {0}", listbox1.SelectedItem.ToString());

                try
                {
                    string query = "DELETE FROM Keywords WHERE Name=@keyword;";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (ISQLiteStatement statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", selection);
                            statement.Step();
                            statement.Reset();
                        }
                        Debug.WriteLine(" ***   Row {0} deleted in Keywords db!", selection);

                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    throw;
                }
                try
                {
                    string query = "DELETE FROM Items WHERE Word=@keyword;";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (ISQLiteStatement statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", selection);
                            statement.Step();
                            statement.Reset();
                        }
                        Debug.WriteLine(" ***   Rows with Word={0} deleted in Items db!", selection);
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    throw;
                }

                this.Frame.Navigate(typeof(DeleteKeyword));
            }
        }







    }
}
