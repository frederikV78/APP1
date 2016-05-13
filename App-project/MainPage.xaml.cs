using SQLitePCL;
using System;
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
            //DeleteTable("Items");

            //TODO: Prepare page for display here.

            // TODO: Prepare page for display here.

            CreateTablesIfNotExists();
            CountTablesItems();
            //tekst();


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
                AddKeyword(keyword);
            }
            keyword = "";
            CountTablesItems();


        }

        private void btnOnDelKeyword_Click(object sender, RoutedEventArgs e)
        {
            string keyword = keywordTextBox.Text;

            if (keyword != "")
            {
                DeleteKeyword(keyword);
            }
            else
            {
                this.Frame.Navigate(typeof(DeleteKeyword));
            }

            CountTablesItems();
            keyword = "";
        }

        private void btnOnUpdateRssfeed_Click(object sender, RoutedEventArgs e)
        {
            loadRSSFeed();
        }








        // // // // // //
        //// METHODS ////
        // // // // // //
        private async void loadRSSFeed()  
        {
            //try
            //{
            //    using (HttpClient client = new HttpClient())
            //    {

            //        Uri url = new Uri("http://deredactie.be/cm/vrtnieuws?mode=atom");

            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        HttpResponseMessage response = await client.GetAsync(url);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            var data = response.Content.ReadAsStringAsync();
            //            var weatherdata =  JsonConvert.DeserializeObject<RSSItem>(data.Result.ToString());



            //        }



            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;

            //}

            try
            {
                string newPubDate = "";
                string oldPubDate;
                bool newFeed;

                HttpClient client = new HttpClient();
                string rssText = await client.GetStringAsync(new Uri("http://www.hln.be/rss.xml", UriKind.Absolute));
                XElement rssElements = XElement.Parse(rssText);
                newPubDate = rssElements.Element("channel").Element("pubDate").Value;
                //Stream rssText = await client.GetStreamAsync(new Uri("http://deredactie.be/cm/vrtnieuws?mode=atom"));

                //XmlReader reader = XmlReader.Create(rssText);

                //SyndicationFeed feed = new SyndicationFeed();
                //////feed.Load(reader.ToString());

                //////newPubDate = feed.LastUpdatedTime.ToUniversalTime().ToString();

                //////reader.Dispose();

                //var url = @"http://www.hln.be/rss.xml";
                //XDocument rss = XDocument.Load(url);


                //XElement rssElements = XElement.Parse(rss.ToString());
                //XElement test = rssElements.Descendants("updated").FirstOrDefault();

                //newPubDate = rssElements.Element("updated").Value;

                ////foreach (var item in feed.Items)
                ////{
                ////    newPubDate = item.LastUpdatedTime.ToString();
                ////}


                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("pubDate"))
                {
                    oldPubDate = (string)ApplicationData.Current.LocalSettings.Values["pubDate"];
                    newFeed = ComparePubDate(newPubDate, oldPubDate);
                }
                else
                {
                    newFeed = true;
                }

                if (newFeed)
                {
                    //save newPubDate
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("pubDate"))
                    {
                        ApplicationData.Current.LocalSettings.Values.Remove("pubDate");
                    }
                    ApplicationData.Current.LocalSettings.Values.Add("pubDate", newPubDate);

                    ////save xml
                    //StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("news_feed.xml", CreationCollisionOption.ReplaceExisting);
                    //await FileIO.WriteTextAsync(file, rssText.ToString());

                    //StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("news_feed.xml");
                    //string readFileRSS = await FileIO.ReadTextAsync(file2);

                    //Debug.WriteLine(" ***   FEED: {0}", readFileRSS);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ComparePubDate(string newPubDate, string oldPubDate)
        {
            DateTime newDate, oldDate;

            if (!DateTime.TryParse(newPubDate, out newDate))
            {
                return false;
            }

            if (!DateTime.TryParse(oldPubDate, out oldDate))
            {
                return false;
            }

            // >= om te testen, moet in werkelijkheid > zijn
            return DateTime.Compare(newDate, oldDate) >= 0;
        }

        private async Task RegisterTask()
        {
            string taskName = "NewsReader task";
            bool isTaskRegisterd = BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == taskName);
            if (!isTaskRegisterd)
            {
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = taskName;
                builder.TaskEntryPoint = "BTask.BackgroundTask";
                builder.SetTrigger(new TimeTrigger(30, false));
                builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status != BackgroundAccessStatus.Denied)
                {
                    BackgroundTaskRegistration task = builder.Register();
                }
            }
        }

        private async void tekst()
        {
            RSSFeed rssFeed = (RSSFeed)App.Current.Resources["rssFeed"];
            if (rssFeed != null)
            {
                await rssFeed.GetFeedAsync("http://deredactie.be/cm/vrtnieuws?mode=atom");
                //this.defaultViewModel["Feed"] = rssFeed;
            }

            await RegisterTask();
        }


        private void CreateTablesIfNotExists()
        {
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
        }

        private void DeleteKeyword(string keyword)
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
            //try
            //{
            //    string query = "DELETE FROM Items WHERE Word=@keyword;";
            //    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            //    {
            //        using (ISQLiteStatement statement = conn.Prepare(query))
            //        {
            //            statement.Bind("@keyword", keyword);
            //            statement.Step();
            //            statement.Reset();
            //        }
            //        Debug.WriteLine(" ***   Rows with Word={0} deleted in Items db!", keyword);
            //        keywordTextBox.Text = "";
            //        keywordTextBox.PlaceholderText = "Enter a keyword";
            //    };
            //}
            //catch (SQLiteException ex)
            //{
            //    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
            //    throw;
            //}
        }

        private void AddKeyword(string keyword)
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

            //try
            //{
            //    string query = "INSERT INTO Items (Word) VALUES (@keyword);";
            //    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            //    {
            //        using (var statement = conn.Prepare(query))
            //        {
            //            statement.Bind("@keyword", keyword);
            //            statement.Step();
            //            statement.Reset();
            //        }
            //        Debug.WriteLine(" ***   {0} added in Items db!", keyword);
            //        keywordTextBox.Text = "";
            //        keywordTextBox.PlaceholderText = "Enter a keyword";
            //    };
            //}
            //catch (SQLiteException ex)
            //{
            //    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
            //    //throw;
            //}
        }

        private void DeleteTable(string table)
        {
            //string query5 = "IF EXISTS (SELECT 1 FROM sqlite_master WHERE table_name='KEYWORDS') DROP TABLE KEYWORDS;";
            string query5 = "DROP TABLE IF EXISTS @table;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query5))
                    {
                        statement.Bind("@table", table);
                        statement.Step();
                        statement.Reset();
                        Debug.WriteLine(" ***   Items table deleted");
                    }
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
        }

        private void CountTablesItems()
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
