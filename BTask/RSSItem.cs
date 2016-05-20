using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTask
{
    class RSSItem
    {






        private string uniqueId;

        public string UniqueId
        {
            get { return uniqueId; }
            set
            {
                if (value != uniqueId)
                {
                    uniqueId = value;
                    //NotifyPropertyChanged("UniqueId");
                }
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    //NotifyPropertyChanged("Title");
                }
            }
        }
        private Uri link;

        public Uri Link
        {
            get { return link; }
            set
            {
                if (value != link)
                {
                    link = value;
                    //NotifyPropertyChanged("Link");
                }
            }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    //NotifyPropertyChanged("Description");
                }
            }
        }

        private DateTime pubDate;



        public DateTime PubDate
        {
            get { return pubDate; }
            set
            {
                if (value != pubDate)
                {
                    pubDate = value;
                    //NotifyPropertyChanged("PubDate");
                }
            }
        }





    }
}
