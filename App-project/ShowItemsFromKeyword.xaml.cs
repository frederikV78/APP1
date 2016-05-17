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
using App_project;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App_project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowItemsFromKeyword : Page
    {

        List<RSSItem> listItems = new List<RSSItem>();
        string keyword;

        public ShowItemsFromKeyword()
        {
            this.InitializeComponent();
            keyword = (string)ApplicationData.Current.LocalSettings.Values["keyword"];
            listItems = GetItemsList(keyword);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            keyword = "nokeywordavailable";
            keyword = (string)ApplicationData.Current.LocalSettings.Values["keyword"];

            Debug.WriteLine("*** *** *** keyword: {0}  SELECTED",keyword);
            
            listItems = GetItemsList(keyword);
            listbox1.ItemsSource = listItems;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //BACK BUTTON
        {
            this.Frame.Navigate(typeof(ShowKeywords));
        }

        private async void AppBarButton1_Click(object sender, RoutedEventArgs e) //DETAILS BUTTON
        {
            int selectedIndex = listbox1.SelectedIndex;
            //RSSItem item = new RSSItem;
            var selectedItemLink = listItems[selectedIndex].Link;
            
            await Launcher.LaunchUriAsync(selectedItemLink);


        }









        // // // // // //
        //// METHODS ////
        // // // // // //
        public List<RSSItem> GetItemsList(string keyword)
        {
            //(ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,KId INTEGER NOT NULL, Keyword varchar(30), Title varchar(30) UNIQUE, Link varchar(100), Description varchar(1024), PubDate varchar(30))
            try
            {       //ItemId,Title,Link, Description, PubDate
                string query = "SELECT * FROM Items WHERE Keyword=@keyword ORDER BY ItemId DESC;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        List<RSSItem> LijstItems = new List<RSSItem>();
                        RSSItem item;

                        int i = 0;
                        Debug.WriteLine("   *** START db items ***   ");
                        statement.Bind("@keyword", keyword);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            item = new RSSItem();
                            i++;
                            item.UniqueId = statement[0].ToString();
                            item.Title = (string)statement[3];
                            item.Link = new Uri(statement[4].ToString(), UriKind.Absolute);
                            item.Description = (string)statement[5];
                            item.PubDate = DateTime.Parse(statement[6].ToString());

                            LijstItems.Add(item);
                        }
                        if (i == 0)
                        {
                            item = new RSSItem();
                            item.Title = "Nothing to show!";
                            LijstItems.Add(item);
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
