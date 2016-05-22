using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace App_project
{
    public class SQLiteMethods
    {
        public List<RSSItem> GetItemsList(string keyword)
        {
            //(ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,KId INTEGER NOT NULL, Keyword varchar(30), Title varchar(30) UNIQUE, Link varchar(100), Description varchar(1024), PubDate varchar(30))
            try
            {       //ItemId,Title,Link, Description, PubDate
                string query = "SELECT * FROM Items WHERE Keyword=@keyword ORDER BY PubDate DESC;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        List<RSSItem> LijstItems = new List<RSSItem>();
                        RSSItem item;

                        int i = 0;
                        statement.Bind("@keyword", keyword);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            item = new RSSItem();
                            i++;
                            item.UniqueId = statement[0].ToString();
                            item.Title = (string)statement[3];
                            item.Link = new Uri(statement[4].ToString(), UriKind.Absolute);
                            item.Description = (string)statement[5];
                            item.PubDate = DateTime.Parse(statement[6].ToString());

                            LijstItems.Add(item);
                        }
                        if (i == 0)
                        {
                            item = new RSSItem();
                            item.Title = "Nothing to show!";
                            LijstItems.Add(item);
                        }
                        else
                        {
                            //Debug.WriteLine("AMOUNT OF ITEMS in Items:{0}", i);
                        }
                        return LijstItems;
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }

        }

        public List<Keyword> GetKeywordsListWithAmount()
        {
            try
            {
                string query1 = "SELECT COUNT(Keyword) FROM Items WHERE Keyword=@keyword";
                using (SQLiteConnection conn1 = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement1 = conn1.Prepare(query1))
                    {
                        string query = "SELECT * FROM Keywords ORDER BY Name;";
                        using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                        {
                            using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                            {
                                List<Keyword> LijstKeywords = new List<Keyword>();
                                int i = 0;
                                while (statement.Step() == SQLiteResult.ROW)
                                {
                                    i++;
                                    Keyword item = new Keyword();
                                    string keyword = (string)statement[1];

                                    statement1.Bind("@keyword", keyword);
                                    statement1.Step();

                                    item.LabelKeyword = keyword;
                                    item.LabelAmount = "[" + statement1[0].ToString() + "]";
                                    LijstKeywords.Add(item);

                                    statement1.Reset();
                                }
                                if (i == 0)
                                {
                                    Keyword item = new Keyword();
                                    item.LabelKeyword = "Nothing to show!";
                                    item.LabelAmount = "0";
                                    LijstKeywords.Add(item);
                                }
                                else
                                {
                                    //Debug.WriteLine("AMOUNT OF ITEMS in Keywords:{0}", i);
                                }
                                return LijstKeywords;
                            };
                        };
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
        }

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

        public void CreateTablesIfNotExists()
        {
            //CREATE Keywords TABLE IF NOT EXISTS
            string query = "CREATE TABLE IF NOT EXISTS Keywords (KId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name varchar(30) UNIQUE);";
            using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            {
                using (ISQLiteStatement statement = conn.Prepare(query))
                {
                    statement.Step();
                    statement.Reset();
                }
            };
            //CREATE Items TABLE IF NOT EXISTS
            string query1 = "CREATE TABLE IF NOT EXISTS Items (ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,KId INTEGER NOT NULL, Keyword varchar(30), Title varchar(30) UNIQUE, Link varchar(100), Description varchar(1024), PubDate varchar(30));";
            using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            {
                using (ISQLiteStatement statement = conn.Prepare(query1))
                {
                    statement.Step();
                    statement.Reset();
                }
            };
            //CREATE Deleted TABLE IF NOT EXISTS
            string query2 = "CREATE TABLE IF NOT EXISTS Deleted (DeletedId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Title varchar(30) UNIQUE);";
            using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
            {
                using (ISQLiteStatement statement = conn.Prepare(query2))
                {
                    statement.Step();
                    statement.Reset();
                }
            };
        }

        public void DeleteKeyword(string keyword)
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
                };
            }
            catch (SQLiteException ex)
            {
                //Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                //throw;
            }
        }

        public void AddKeyword(string keyword)
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
                };
            }
            catch (SQLiteException ex)
            {
                //Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                //throw;
            }
        }

        public string CountTableKeywords() //CHECKING HOW MANY KEYWORDS IN THE Keywords TABLE
        {
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
                        string amountOfKeywords = i.ToString();

                        return amountOfKeywords;
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
        }
        public string CountTableItems() //CHECKING HOW MANY ITEMS IN THE Items TABLE
        {
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
                        string amountOfItems = i.ToString();

                        return amountOfItems;
                    };
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
        }

        public bool SaveItemElement(XElement itemElement, string keyword)
        {
            //(ItemId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, KId INTEGER NOT NULL, Keyword varchar(30), Title varchar(30) UNIQUE, Link varchar(100), description varchar(1024), PubDate varchar(30))

            // CHECK IF ALLREADY EXISTS
            bool succes = false;
            bool allreadyExists = false;
            string title ="";
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
            // CHECK IF DELETED BEFORE
            try
            {
                string query = "SELECT DeletedId,Title FROM Deleted WHERE Title=@title;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (SQLitePCL.ISQLiteStatement statement = conn.Prepare(query))
                    {
                        statement.Bind("@title", itemElement.Element("title").Value);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            title = (string)statement[1];
                            bool allreadyDeleted = true;
                            allreadyExists = allreadyDeleted;
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
                        succes = true;
                        Debug.WriteLine(" *** *** *** {0} *** *** *** added in Items db!", itemElement.Element("title").Value);
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
                //Debug.WriteLine(" ***   {0}   *** ALLREADY IN ITEMS TABLE, SKIP ADD!", title);
            }
            return succes;
        }

        public void DeleteItems(string keyword)
        {
            try
            {
                string query = "DELETE FROM Items WHERE keyword=@keyword;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query))
                    {
                        statement.Bind("@keyword", keyword);
                        statement.Step();
                        statement.Reset();
                    }
                    //Debug.WriteLine(" ***   Rows with Word={0} deleted in Items db!", keyword);
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                throw;
            }
        }

        public void DeleteSelectedItem(string title)
        {
            try  //Delete from Items TABLE
            {
                string query = "DELETE FROM Items WHERE Title=@title;";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (ISQLiteStatement statement = conn.Prepare(query))
                    {
                        statement.Bind("@title", title);
                        statement.Step();
                        statement.Reset();
                    }
                    //Debug.WriteLine(" ***   Rows with Word={0} deleted in Items db!", keyword);
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                //throw;
            }

            //CREATE TABLE IF NOT EXISTS Deleted (DeletedId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Title varchar(30) UNIQUE);

            //Add to Deleted TABLE (to remember the deletion)
            try
            {
                string query = "INSERT INTO Deleted (Title) VALUES (@title);";
                using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                {
                    using (var statement = conn.Prepare(query))
                    {
                        statement.Bind("@title", title);
                        statement.Step();
                        statement.Reset();
                    }
                };
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                //throw;
            }
        }

        public void DeleteSelectedKeyword(string selection)
        {
            if (selection != "Nothing to show!" && selection != "")
            {
                try
                {
                    string query = "DELETE FROM Keywords WHERE Name=@keyword;";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (ISQLiteStatement statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", selection);
                            statement.Step();
                            statement.Reset();
                        }
                        //Debug.WriteLine(" ***   Row {0} deleted in Keywords db!", selection);
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    //throw;
                }

                try
                {
                    string query = "DELETE FROM Items WHERE keyword=@keyword;";
                    using (SQLiteConnection conn = new SQLiteConnection("Keywords.db"))
                    {
                        using (ISQLiteStatement statement = conn.Prepare(query))
                        {
                            statement.Bind("@keyword", selection);
                            statement.Step();
                            statement.Reset();
                        }
                        //Debug.WriteLine(" ***   Rows with Word={0} deleted in Items db!", selection);
                    };
                }
                catch (SQLiteException ex)
                {
                    Debug.WriteLine(" ***   Exeption: {0}", ex.Message);
                    //throw;
                }

            }
        }





    }
}
