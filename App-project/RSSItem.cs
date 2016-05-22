using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_project
{
    public class RSSItem
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
                }
            }
        }

 



    }
}
