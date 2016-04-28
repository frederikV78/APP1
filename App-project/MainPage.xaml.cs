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

            //string query5 = "DROP TABLE IF EXISTS Keywords;";
            //try
            //{
            //    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            //    {
            //        using (ISQLiteStatement statement = conn.Prepare(query5))
            //        {
            //            statement.Step();
            //            statement.Reset();
            //            Debug.WriteLine(" ***   Keywords table deleted");
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
            string query1 = "CREATE TABLE IF NOT EXISTS Items (ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,KId INTEGER UNIQUE, Item varchar(500));";
            using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            {
                using (ISQLiteStatement statement = conn.Prepare(query1))
                {
                    statement.Step();
                    statement.Reset();
                    Debug.WriteLine(" ***   Items table created!");
                }
            };
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
                        Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
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
                        Debug.WriteLine("AMOUNT OF ITEMS in Items:{0}", i);
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }




            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void btnOnToonKernwoorden_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT * FROM Keywords WHERE Name='Keyword-Test';";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query))
                    {
                        int i = 0;
                        Debug.WriteLine("   *** START db items ***   ");
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            i++;
                            //int KId = (int)statement[0];
                            string keyword = (string)statement[1];
                            //Debug.WriteLine("KEYWORD: {0}, Id: {1}", keyword, KId);
                            Debug.WriteLine("KEYWORD: {0}", keyword);
                        }
                        Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
                        Debug.WriteLine("   *** EINDE db items ***   ");
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }


        }

        private void btnOnAddKeyword_Click(object sender, RoutedEventArgs e)
        {
            string keyword = "KeywordTest";
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
                    Debug.WriteLine(" ***   {0} added in Keywords db!",keyword);
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                //throw;
            }



        }

        private void btnOnDelKeyword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM Keywords WHERE Name='KeywordTest';";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query))
                    {
                        statement.Step();
                        statement.Reset();
                    }
                    Debug.WriteLine(" ***   Row KeywordTest deleted in Keywords db!");
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
