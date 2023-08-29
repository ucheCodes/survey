using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DBreeze;
using DBreeze.DataTypes;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Database
{
    public static class database
    {
        static string dir = Directory.GetCurrentDirectory();
        static string path = Path.Combine(dir,("Database\\data"));
        // static string dir2 = AppDomain.CurrentDomain.BaseDirectory;
        // static string path2 = Path.Combine(dir2,("Database\\data"));
        //static string path = Path.Combine((string)AppDomain.CurrentDomain.GetData("ContentRootPath"),("Database\\data"));
        private static DBreezeEngine engine = new DBreezeEngine(path);
        public static bool Create(string tablename, string serializedKey, string serializedValue)
        {
            using (var trans = engine.GetTransaction())
            {
                try
                {
                    trans.Insert<string,string>(tablename,serializedKey, serializedValue);
                    trans.Commit();
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }

        public static KeyValuePair<string,string> Read(string tablename, string key)
        {
            using (var trans = engine.GetTransaction())
            {
                try
                {
                    string _key = JsonConvert.SerializeObject(key);
                    var data = trans.Select<string, string>(tablename,_key);
                    if (data.Exists)
                    {
                        return new KeyValuePair<string, string>(data.Key, data.Value);
                    }
                }
                catch (System.Exception)
                {
                    return new KeyValuePair<string, string>("", "");
                }
            }
            return new KeyValuePair<string,string>("","");
        }

        public static Dictionary<Tkey,Tvalue> ReadAll<Tkey,Tvalue>(string tablename)
        {
            var list = new Dictionary<Tkey, Tvalue>();
            using (var trans = engine.GetTransaction())
            {
                try
                {
                    var data = trans.SelectForward<string,string>(tablename);
                    if (data != null && data.Count() > 0)
                    {
                       foreach (var d in data)
                       {

                            var key = JsonConvert.DeserializeObject<Tkey>(d.Key);
                            var value = JsonConvert.DeserializeObject<Tvalue>(d.Value);
                            if (key != null && value != null)
                            {
                                list.Add(key, value);
                            }
                       } 
                       return list;
                    }
                }
                catch (System.Exception)
                {
                  return list;
                }
            }
            return list;
        }

        public static bool Delete(string tablename, string key)
        {
            using (var trans = engine.GetTransaction())
            {
                try
                {
                    string _key = JsonConvert.SerializeObject(key);
                    if (trans.Select<string,string>(tablename, _key).Exists)
                    {
                        trans.RemoveKey<string>(tablename,_key);
                        trans.Commit();
                        return true;
                    }
                }
                catch (System.Exception)
                {
                   return false;
                }
            }
            return false;
        }

        public static bool Exists(string tablename, string key)
        {
            using (var trans = engine.GetTransaction())
            {
                try
                {
                    string _key = JsonConvert.SerializeObject(key);
                    if (trans.Select<string,string>(tablename, _key).Exists)
                    {
                        return true;
                    }
                }
                catch (System.Exception)
                {
                   return false;
                }
            }
            return false;
        }
        
        public static bool DeleteAll(string tablename, bool recreateFile)
        {
            using (var trans = engine.GetTransaction())
            {
                try
                {
                    trans.RemoveAllKeys(tablename, recreateFile);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }
}