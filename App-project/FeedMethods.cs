using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace App_project
{
    public class FeedMethods
    {
        SQLiteMethods sqlitemethode = new SQLiteMethods();

        public async Task LoadRSSFeed()
        {
            try
            {
                string newPubDate = "";
                string oldPubDate;
                bool newFeed;
                int hits;

                HttpClient client = new HttpClient();
                string rssText = await client.GetStringAsync(new Uri("http://www.hln.be/rss.xml", UriKind.Absolute));
                XElement rssElements = XElement.Parse(rssText);
                newPubDate = rssElements.Element("channel").Element("pubDate").Value;
                List<string> keywords = sqlitemethode.GetKeywordsList();

                
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

                    //save keyword hits in title of items
                    hits = 0;
                    foreach (var itemElement in rssElements.Element("channel").Elements("item"))
                    {
                        var titleElement = itemElement.Element("title");
                        string title = itemElement.Element("title").Value.ToLower();

                        foreach (string keyword in keywords)
                        {
                            keyword.ToLower();
                            if (title.Contains(keyword))
                            {
                                bool succes = false;
                                succes = sqlitemethode.SaveItemElement(itemElement, keyword);
                                if (succes)
                                {
                                    hits++;
                                    //Debug.WriteLine("*** *** ***   HIT   = {0}: {1}", keyword, title);
                                }
                            }
                        }
                    }
                    if (hits>0)
                    {
                        SendToast(hits);
                    }
                    string amountOfItems = sqlitemethode.CountTableItems();
                    string notification = "Total Items : " + amountOfItems + "| New Items: " + hits + " (" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + ")";
                    UpdateTile(notification);
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
            return DateTime.Compare(newDate, oldDate) >= 0;  // DEMO SITUATIE, >= om te testen, moet in werkelijkheid > zijn
        }                                                       // GEVOLG: elke feed is een nieuwe als DEMO

        public async void Timer1_Tick()
        {
            await LoadRSSFeed();
        }

        public void UpdateTile(string tile) 
        {
            XmlDocument template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text04);
            XmlNodeList texts = template.GetElementsByTagName("text");
            texts[0].InnerText = tile;

            XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text03);
            XmlNodeList wideTileTexts = wideTileXml.GetElementsByTagName("text");
            wideTileTexts[0].InnerText = tile;

            IXmlNode node = template.ImportNode(wideTileXml.GetElementsByTagName("binding").Item(0), true);
            template.GetElementsByTagName("visual").Item(0).AppendChild(node);

            TileNotification notification = new TileNotification(template);
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(notification);
        }

        private void SendToast(int hits)
        {
            XmlDocument template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList texts = template.GetElementsByTagName("text");
            texts[0].AppendChild(template.CreateTextNode("HLN news by keyword"));
            texts[1].AppendChild(template.CreateTextNode("Feed updated: "+hits+" new articles"));

            ToastNotification notification = new ToastNotification(template);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }









    }
}
