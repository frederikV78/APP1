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
    public sealed partial class ShowKeywords : Page
    {
        
        public ShowKeywords()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Load the list of keywords from the Keywords table
            SQLiteMethods sqlitemethode = new SQLiteMethods();
            listbox1.ItemsSource = sqlitemethode.GetKeywordsListWithAmount();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //BACK BUTTON
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton1_Click(object sender, RoutedEventArgs e) //DETAILS BUTTON
        {
            //go to another page and load the Items table for the pressed/selected keyword
            //view in a listbox with delete button from selection 
            int index = listbox1.SelectedIndex;

            if (index >= 0)
            {
                Keyword item = new Keyword();
                item = (Keyword)listbox1.SelectedItem;
                string selection = item.LabelKeyword.ToString();

                selection.ToLower();

                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("keyword"))
                {
                    ApplicationData.Current.LocalSettings.Values.Remove("keyword");
                }
                ApplicationData.Current.LocalSettings.Values.Add("keyword", selection);

                this.Frame.Navigate(typeof(ShowItemsFromKeyword));
            }

        }







        // // // // // //
        //// METHODS ////
        // // // // // //
        //public List<string> GetKeywordsList()
        //{
        //    try
        //    {
        //        string query = "SELECT * FROM Keywords ORDER BY Name;";
        //        using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
        //        {
        //            using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
        //            {
        //                List<string> LijstKeywords = new List<string>();

        //                int i = 0;
        //                Debug.WriteLine("   *** START db items ***   ");
        //                while (statement.Step() == SQLiteResult.ROW)
        //                {
        //                    i++;
        //                    string keyword = (string)statement[1];
        //                    LijstKeywords.Add(keyword);
        //                }
        //                if (i == 0)
        //                {
        //                    LijstKeywords.Add("Nothing to show!");
        //                }
        //                else
        //                {
        //                    Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
        //                }
        //                return LijstKeywords;
        //            };
        //        };
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
        //        throw;
        //    }
            
        //}






    }




    }

