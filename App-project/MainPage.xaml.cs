using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace App_project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SQLiteMethods sqlitemethode = new SQLiteMethods();
        FeedMethods feedmethode = new FeedMethods();
        public DispatcherTimer dispatcherTimer1;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Tick += DispatcherTimer1_Tick;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 10); // als demo staat dit op 10 seconden
            //10min = 64-80kb per feed (piekwaarde met marge) * 30 dagen * 24 uur * 6 updates/uur = ~270-340 MB/maand
            dispatcherTimer1.Start();

        }

        private void DispatcherTimer1_Tick(object sender, object e)
        {
            feedmethode.Timer1_Tick();
            LabelAmountOfItems.Text = sqlitemethode.CountTableItems();
            LabelAmountOfKeywords.Text = sqlitemethode.CountTableKeywords();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            sqlitemethode.CreateTablesIfNotExists();
            LabelAmountOfItems.Text = sqlitemethode.CountTableItems();
            LabelAmountOfKeywords.Text = sqlitemethode.CountTableKeywords();
           
        }

        private void btnOnToonKernwoorden_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowKeywords));
            keywordTextBox.Text = "";
        }

        private async void btnOnAddKeyword_Click(object sender, RoutedEventArgs e)
        {
            string keyword = keywordTextBox.Text;

            if (keyword != "")
            {
               sqlitemethode.AddKeyword(keyword);
            }
            await feedmethode.LoadRSSFeed();
            LabelAmountOfItems.Text = sqlitemethode.CountTableItems();
            LabelAmountOfKeywords.Text = sqlitemethode.CountTableKeywords();
            keyword = "";
        }

        private void btnOnDelKeyword_Click(object sender, RoutedEventArgs e)
        {
            string keyword = keywordTextBox.Text;

            if (keyword != "")
            {
                sqlitemethode.DeleteItems(keyword);
                sqlitemethode.DeleteKeyword(keyword);
            }
            else
            {
                this.Frame.Navigate(typeof(DeleteKeyword));
            }
            LabelAmountOfKeywords.Text = sqlitemethode.CountTableKeywords();
            LabelAmountOfItems.Text = sqlitemethode.CountTableItems();
            keyword = "";
            keywordTextBox.Text = keyword;
        }

        private async void btnOnUpdateRssfeed_Click(object sender, RoutedEventArgs e)
        {
            await feedmethode.LoadRSSFeed();
            LabelAmountOfItems.Text = sqlitemethode.CountTableItems();
            keywordTextBox.Text = "";
        }








    }
}
