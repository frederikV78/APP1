using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace BTask
{
    public class FeedMethods
    {
        SQLiteMethods sqlitemethode = new SQLiteMethods();


        public async Task loadRSSFeed()
        {
            try
            {
                string newPubDate = "";
                string oldPubDate;
                bool newFeed;

                HttpClient client = new HttpClient();
                string rssText = await client.GetStringAsync(new Uri("http://www.hln.be/rss.xml", UriKind.Absolute));
                XElement rssElements = XElement.Parse(rssText);
                newPubDate = rssElements.Element("channel").Element("pubDate").Value;
                List<string> keywords = sqlitemethode.GetKeywordsList();

                foreach (var itemElement in rssElements.Element("channel").Elements("item"))
                {
                    var titleElement = itemElement.Element("title");
                    string title = itemElement.Element("title").Value.ToLower();

                    foreach (string keyword in keywords)
                    {
                        keyword.ToLower();
                        if (title.Contains(keyword))
                        {
                            sqlitemethode.SaveItemElement(itemElement, keyword);
                            //Debug.WriteLine("*** *** ***   HIT   = {0}: {1}", keyword, title);
                        }
                    }
                }

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










    }
}
