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
        SQLiteMethods sqlitemethode = new SQLiteMethods();
        List<RSSItem> listItems = new List<RSSItem>();
        string keyword;

        public ShowItemsFromKeyword()
        {
            this.InitializeComponent();
            keyword = (string)ApplicationData.Current.LocalSettings.Values["keyword"];
            //listItems = GetItemsList(keyword);
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
            
            listItems = sqlitemethode.GetItemsList(keyword);
            listbox2.ItemsSource = listItems;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //BACK BUTTON
        {
            this.Frame.Navigate(typeof(ShowKeywords));
        }

        private async void AppBarButton1_Click(object sender, RoutedEventArgs e) //DETAILS BUTTON
        {
            int selectedIndex = listbox2.SelectedIndex;
            //RSSItem item = new RSSItem;
            var selectedItemLink = listItems[selectedIndex].Link;
            
            await Launcher.LaunchUriAsync(selectedItemLink);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            RSSItem selectedItem = (RSSItem)listbox2.SelectedItem;
            Debug.WriteLine(" ***   SELECTION= {0}", selectedItem.Title);
            //DeleteSelectedItem(selection);
        }
    }
}
