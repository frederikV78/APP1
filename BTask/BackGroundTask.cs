using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BTask
{
    public sealed class BackGroundTask : IBackgroundTask
    {
        //SQLiteMethods sqlitemethode = new SQLiteMethods();
        FeedMethods feedmethode = new FeedMethods();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            //code to run in background

            await feedmethode.loadRSSFeed();
            //SendToast();
            UpdateTile("New Update Available");

            deferral.Complete();
        }

        private void SendToast()
        {
            XmlDocument template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList texts = template.GetElementsByTagName("text");
            texts[0].AppendChild(template.CreateTextNode("Follow up on HLN newsfeed"));
            texts[1].AppendChild(template.CreateTextNode("Updated news items are available"));

            ToastNotification notification = new ToastNotification(template);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }

        private void UpdateTile(string pubDate) //om de templates aan elkaar te koppelen en de pubdate door te geven voor de view
        {
            XmlDocument template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text04);
            XmlNodeList texts = template.GetElementsByTagName("text");
            texts[0].InnerText = pubDate;

            XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text03);
            XmlNodeList wideTileTexts = wideTileXml.GetElementsByTagName("text");
            wideTileTexts[0].InnerText = pubDate;

            IXmlNode node = template.ImportNode(wideTileXml.GetElementsByTagName("binding").Item(0), true);
            template.GetElementsByTagName("visual").Item(0).AppendChild(node);

            TileNotification notification = new TileNotification(template);
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(notification);
        }










    }
}
