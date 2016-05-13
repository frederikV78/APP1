using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_project
{
    public class RSSItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string uniqueId;

        public string UniqueId
        {
            get { return uniqueId; }
            set
            {
                if (value != uniqueId)
                {
                    uniqueId = value;
                    NotifyPropertyChanged("UniqueId");
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
                    NotifyPropertyChanged("Title");
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
                    NotifyPropertyChanged("Link");
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
                    NotifyPropertyChanged("Description");
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
                    NotifyPropertyChanged("PubDate");
                }
            }
        }

        //private string UniqueId;
        //private string Title;
        //private string Summary;
        //private Uri Url;
        //private DateTime Published;
        //private DateTime Updated;

        //public string Title1
        //{
        //    get
        //    {
        //        return Title;
        //    }
        //    set
        //    {
        //        if (value != Title)
        //        {
        //            Title = value;
        //            NotifyPropertyChanged("Title");
        //        }
        //    }
        //}

        //public string Summary1
        //{
        //    get
        //    {
        //        return Summary;
        //    }
        //    set
        //    {
        //        if (value != Summary)
        //        {
        //            Summary = value;
        //            NotifyPropertyChanged("Summary");
        //        }
        //    }
        //}

        //public Uri Url1
        //{
        //    get
        //    {
        //        return Url;
        //    }
        //    set
        //    {
        //        if (value != Url)
        //        {
        //            Url = value;
        //            NotifyPropertyChanged("Url");
        //        }
        //    }
        //}

        //public DateTime Published1
        //{
        //    get
        //    {
        //        return Published;
        //    }
        //    set
        //    {
        //        if (value != Published)
        //        {
        //            Published = value;
        //            NotifyPropertyChanged("Published");
        //        }
        //    }
        //}

        //public DateTime Updated1
        //{
        //    get
        //    {
        //        return Updated;
        //    }
        //    set
        //    {
        //        if (value != Updated)
        //        {
        //            Updated = value;
        //            NotifyPropertyChanged("Updated");
        //        }
        //    }
        //}

        //public string UniqueId1
        //{
        //    get
        //    {
        //        return UniqueId;
        //    }
        //    set
        //    {
        //        if (value != UniqueId)
        //        {
        //            UniqueId = value;
        //            NotifyPropertyChanged("UniqueId");
        //        }
        //    }
        //}
    }
}
