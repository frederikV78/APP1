using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ShowItemsFromKeyword : Page
    {
        public ShowItemsFromKeyword()
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
            string keyword = "nokeywordavailable";
            keyword = (string)ApplicationData.Current.LocalSettings.Values["keyword"];

            Debug.WriteLine("*** *** *** keyword: {0}  SELECTED",keyword);

            listbox1.ItemsSource = GetItemsList(keyword);

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //BACK BUTTON
        {
            this.Frame.Navigate(typeof(ShowKeywords));
        }

        private void AppBarButton1_Click(object sender, RoutedEventArgs e) //DETAILS BUTTON
        {
            


        }









        // // // // // //
        //// METHODS ////
        // // // // // //
        public List<string> GetItemsList(string keyword)
        {
            //(ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,KId INTEGER NOT NULL, Keyword varchar(30), Title varchar(30) UNIQUE, Link varchar(100), Description varchar(1024), PubDate varchar(30))
            try
            {
                string query = "SELECT ItemId,Title,Link, Description, PubDate FROM Items WHERE Keyword=@keyword ORDER BY ItemId DESC;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        List<string> LijstItems = new List<string>();

                        int i = 0;
                        Debug.WriteLine("   *** START db items ***   ");
                        statement.Bind("@keyword", keyword);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            i++;
                            string title = (string)statement[1];
                            LijstItems.Add(title);
                        }
                        if (i == 0)
                        {
                            LijstItems.Add("Nothing to show!");
                        }
                        else
                        {
                            Debug.WriteLine("AMOUNT OF ITEMS in Items:{0}", i);
                        }
                        return LijstItems;
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }

        }








    }
}
