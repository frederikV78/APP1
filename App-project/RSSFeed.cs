using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Web.Syndication;

namespace App_project
{
    class RSSFeed : INotifyPropertyChanged
    {
        public RSSFeed()
        {
            this.Items = new ObservableCollection<RSSItem>();
        }

        public ObservableCollection<RSSItem> Items { get; private set; }



        public async Task DownloadFeedAsync(string feedUriString)
        {
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = new Uri(feedUriString);

            try
            {
                SyndicationFeed feed = await client.RetrieveFeedAsync(feedUri);

                if (feed.Title != null && feed.Title.Text != null)
                {
                    this.Title = feed.Title.Text;
                }

                if (feed.Items != null && feed.Items.Count > 0)
                {
                    this.Items.Clear();
                    int i = 0;

                    foreach (SyndicationItem item in feed.Items)
                    {
                        RSSItem rssItem = new RSSItem();

                        if (item.Title != null && item.Title.Text != null)
                        {
                            rssItem.Title = item.Title.Text;
                        }

                        if (item.Links != null && item.Links.Count > 0)
                        {
                            rssItem.Link = item.Links[0].Uri;
                        }

                        if (item.Summary != null && item.Summary.Text != null)
                        {
                            rssItem.Description = item.Summary.Text;
                        }

                        if (item.PublishedDate != null)
                        {
                            rssItem.PubDate = item.PublishedDate.DateTime;
                        }

                        rssItem.UniqueId = Convert.ToString(i);
                        i++;

                        this.Items.Add(rssItem);
                    }
                }
            }
            catch
            {
            }

            //try
            //{
            //    SyndicationFeed feed = await client.RetrieveFeedAsync(feedUri);

            //    if (feed.Title != null && feed.Title.Text != null)
            //    {
            //        this.Title = feed.Title.Text;
            //    }

            //    if (feed.Items != null && feed.Items.Count > 0)
            //    {
            //        this.Items.Clear();
            //        int i = 0;

            //        foreach (SyndicationItem item in feed.Items)
            //        {
            //            RSSItem rssItem = new RSSItem();

            //            if (item.Title != null && item.Title.Text != null)
            //            {
            //                rssItem.Title1 = item.Title.Text;
            //            }

            //            if (item.Links != null && item.Links.Count > 0)
            //            {
            //                rssItem.Url1 = item.Links[0].Uri;
            //            }

            //            if (item.Summary != null && item.Summary.Text != null)
            //            {
            //                rssItem.Summary1 = item.Summary.Text;
            //            }

            //            if (item.PublishedDate != null)
            //            {
            //                rssItem.Published1 = item.PublishedDate.DateTime;
            //            }
            //            if (item.LastUpdatedTime != null)
            //            {
            //                rssItem.Updated1 = item.LastUpdatedTime.DateTime;
            //            }

            //            rssItem.UniqueId1 = Convert.ToString(i);
            //            i++;

            //            this.Items.Add(rssItem);
            //        }
            //    }
            //}
            //catch
            //{
            //}
        }

        public async Task GetFeedAsync(string feedUriString)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("pubDate") == false)
            {
                ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
                NetworkConnectivityLevel connectivityLevel = profile.GetNetworkConnectivityLevel();
                if (connectivityLevel == NetworkConnectivityLevel.InternetAccess)
                {
                    await DownloadFeedAsync(feedUriString);
                }
            }
            else
            {
                await ReadFeedAsync("news_feed.xml");
            }
        }

        public async Task ReadFeedAsync(string filename)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            string rssText = await FileIO.ReadTextAsync(file);

            XElement rssElements = XElement.Parse(rssText);

            this.Title = rssElements.Element("channel").Element("title").Value;

            var itemList = from item in rssElements.Elements("channel").Elements("item")
                           select new RSSItem
                           {
                               Title = item.Element("title").Value,
                               Link = new Uri(item.Element("link").Value),
                               Description = item.Element("description").Value,
                               PubDate = Convert.ToDateTime(item.Element("pubDate").Value),
                           };

            this.Items.Clear();
            int i = 0;
            foreach (var item in itemList)
            {
                item.UniqueId = Convert.ToString(i);
                i++;
                this.Items.Add(item);
            }
        }

        //StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
        //string rssText = await FileIO.ReadTextAsync(file);

        //XElement rssElements = XElement.Parse(rssText);

        //this.Title = rssElements.Element("author").Element("name").Value;

        //var itemList = from item in rssElements.Elements("feed").Elements("entry")
        //               select new RSSItem
        //               {
        //                   Title1 = item.Element("title").Value,
        //                   Summary1 = item.Element("description").Value,
        //                   Url1 = new Uri(item.Element("id").Value),
        //                   Published1 = Convert.ToDateTime(item.Element("published").Value),
        //                   Updated1 = Convert.ToDateTime(item.Element("updated").Value),
        //               };

        //this.Items.Clear();
        //int i = 0;
        //foreach (var item in itemList)
        //{
        //    item.UniqueId1 = Convert.ToString(i);
        //    i++;
        //    this.Items.Add(item);
        //}
    //}

    private string title;

    public string Title
    {
        get { return title; }
        set
        {
            if (value != title)
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }
    }

    //private string title;

    //public string Title
    //{
    //    get { return title; }
    //    set
    //    {
    //        if (value != title)
    //        {
    //            title = value;
    //            NotifyPropertyChanged("Title");
    //        }
    //    }
    //}




    public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
