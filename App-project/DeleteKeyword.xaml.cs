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

        SQLiteMethods sqlitemethode = new SQLiteMethods();

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            listbox1.ItemsSource = sqlitemethode.GetKeywordsList();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //BACK BUTTON
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton1_Click(object sender, RoutedEventArgs e) //DELETE BUTTON
        {
            string selection = listbox1.SelectedItem.ToString();

            sqlitemethode.DeleteSelectedKeyword(selection);

            if (selection != "Nothing to show!" && selection != "")
            { 
                this.Frame.Navigate(typeof(DeleteKeyword));
            }
        }







    }
}
