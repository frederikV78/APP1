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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace App_project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //string query5 = "IF EXISTS (SELECT 1 FROM sqlite_master WHERE table_name='KEYWORDS') DROP TABLE KEYWORDS;";


            // TODO: Prepare page for display here.

            //string query5 = "DROP TABLE IF EXISTS Items;";
            //try
            //{
            //    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            //    {
            //        using (ISQLiteStatement statement = conn.Prepare(query5))
            //        {
            //            statement.Step();
            //            statement.Reset();
            //            Debug.WriteLine(" ***   Items table deleted");
            //        }
            //    };
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
            //    throw;
            //}






            // TODO: Prepare page for display here.

            //CREATE Keywords TABLE IF NOT EXISTS
            string query = "CREATE TABLE IF NOT EXISTS Keywords (KId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name varchar(30) UNIQUE);";
            using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            {
                using (ISQLiteStatement statement = conn.Prepare(query))
                {
                    statement.Step();
                    statement.Reset();
                    Debug.WriteLine(" ***   Keywords table created!");
                }
            };
            //CREATE Items TABLE IF NOT EXISTS
            string query1 = "CREATE TABLE IF NOT EXISTS Items (ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,KId INTEGER UNIQUE, Word varchar(30), Item varchar(500));";
            using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            {
                using (ISQLiteStatement statement = conn.Prepare(query1))
                {
                    statement.Step();
                    statement.Reset();
                    Debug.WriteLine(" ***   Items table created!");
                }
            };


            CheckTablesItems();


            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void btnOnToonKernwoorden_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowKeywords));



        }

        private void btnOnAddKeyword_Click(object sender, RoutedEventArgs e)
        {
            string keyword = keywordTextBox.Text;
            if (keyword != "")
            {
                try
                {
                    string query = "INSERT INTO Keywords (Name) VALUES (@keyword);";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (var statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", keyword);
                            statement.Step();
                            statement.Reset();
                        }
                        Debug.WriteLine(" ***   {0} added in Keywords db!", keyword);
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    //throw;
                }

                try
                {
                    string query = "INSERT INTO Items (Word) VALUES (@keyword);";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (var statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", keyword);
                            statement.Step();
                            statement.Reset();
                        }
                        Debug.WriteLine(" ***   {0} added in Items db!", keyword);
                        keywordTextBox.Text = "";
                        keywordTextBox.PlaceholderText = "Enter a keyword";
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    //throw;
                }
            }
            keyword = "";
            CheckTablesItems();


        }

        private void btnOnDelKeyword_Click(object sender, RoutedEventArgs e)
        {
            string keyword = keywordTextBox.Text;

            if (keyword != "")
            {
                try
                {
                    string query = "DELETE FROM Keywords WHERE Name=@keyword;";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (ISQLiteStatement statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", keyword);
                            statement.Step();
                            statement.Reset();
                        }
                        Debug.WriteLine(" ***   Row {0} deleted in Keywords db!", keyword);

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
                            statement.Bind("@keyword", keyword);
                            statement.Step();
                            statement.Reset();
                        }
                        Debug.WriteLine(" ***   Rows with Word={0} deleted in Items db!", keyword);
                        keywordTextBox.Text = "";
                        keywordTextBox.PlaceholderText = "Enter a keyword";
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    throw;
                }
            }
            else
            {
                this.Frame.Navigate(typeof(DeleteKeyword));
            }

            CheckTablesItems();
            keyword = "";
        }


        private void CheckTablesItems()
        {
            //CHECKING HOW MANY KEYWORDS IN THE Keywords TABLE
            try
            {
                string query2 = "SELECT * FROM Keywords;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query2))
                    {
                        int i = 0;
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            i++;
                        }
                        LabelAmountOfKeywords.Text = i.ToString();
                        //Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
            //CHECKING HOW MANY ITEMS IN THE Items TABLE
            try
            {
                string query3 = "SELECT * FROM Items;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query3))
                    {
                        int i = 0;
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            i++;
                        }
                        LabelAmountOfItems.Text = i.ToString();
                        //Debug.WriteLine("AMOUNT OF ITEMS in Items:{0}", i);
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
