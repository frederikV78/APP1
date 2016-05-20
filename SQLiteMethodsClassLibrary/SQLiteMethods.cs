using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLiteMethodsClassLibrary
{
    public class SQLiteMethods
    {
        public List<string> GetKeywordsList()
        {
            try
            {
                string query = "SELECT * FROM Keywords ORDER BY Name;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        List<string> LijstKeywords = new List<string>();

                        int i = 0;
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            i++;
                            string keyword = (string)statement[1];
                            LijstKeywords.Add(keyword);
                        }
                        if (i == 0)
                        {
                            LijstKeywords.Add("Nothing to show!");
                        }
                        else
                        {
                            //Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
                        }
                        return LijstKeywords;
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }

        }

        public void SaveItemElement(XElement itemElement, string keyword)
        {
            //(ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, KId INTEGER NOT NULL, Keyword varchar(30), Title varchar(30) UNIQUE, Link varchar(100), description varchar(1024), PubDate varchar(30))

            // CHECK IF ALLREADY EXISTS
            bool allreadyExists = false;
            string title = "";
            try
            {
                string query = "SELECT ItemId,Title,Link,Description,PubDate FROM Items WHERE Title=@title ORDER BY ItemId ASC;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        statement.Bind("@title", itemElement.Element("title").Value);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            title = (string)statement[1];
                            allreadyExists = true;
                        }
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
            // SAVE THE ITEM
            if (!allreadyExists)
            {
                try
                {
                    string query = "INSERT INTO Items (KId,Keyword,Title,Link,Description,PubDate) VALUES (7,@keyword,@title,@link,@description,@pubdate);";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (var statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", keyword);
                            statement.Bind("@title", itemElement.Element("title").Value);
                            statement.Bind("@link", itemElement.Element("link").Value);
                            statement.Bind("@description", itemElement.Element("description").Value);
                            statement.Bind("@pubdate", itemElement.Element("pubDate").Value);
                            statement.Step();
                            statement.Reset();
                        }
                        //Debug.WriteLine(" *** *** *** {0} *** *** *** added in Items db!", itemElement.Element("title").Value);
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    throw;
                }
            }
            else
            {
                Debug.WriteLine(" ***   {0}   *** ALLREADY IN ITEMS TABLE, SKIP ADD!", title);
            }
        }











    }
}
