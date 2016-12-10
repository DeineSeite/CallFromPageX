using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using SQLite.Net.Interop;
using System.Linq;
using SQLite;

namespace CallFromPageApp.Droid
{
    public class SqliteService
    {
        public const string DB_NAME = "callfrompage.db3";

        private string path;
        ISQLitePlatform platform;

        public SqliteService(ISQLitePlatform platform, string path)
        {
            this.platform = platform;
            this.path = Path.Combine(path, DB_NAME);
        }

        private SQLiteConnection GetSQLiteConnetion()
        {
            var conn = new SQLiteConnection(platform, path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache | SQLiteOpenFlags.FullMutex, true);
            return conn;
        }

        public bool TableExists<T>()
        {
            try
            {
                using (var conn = GetSQLiteConnetion())
                {
                    string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                    var cmd = conn.CreateCommand(cmdText, typeof(T).Name);
                    return cmd.ExecuteScalar<string>() != null;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                return false;
            }
        }

        public bool CreateTables()
        {
            try
            {
                using (var conn = GetSQLiteConnetion())
                {
                    conn.CreateTable<ContactNotificationItem>();
                }

                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                return false;
            }
        }

        public int getMaxId()
        {
            try
            {
                using (var conn = GetSQLiteConnetion())
                {
                    var item = conn.Table<ContactNotificationItem>().Max(x => x.Id);
                    return item;
                    //return 1;
                }
            }
            catch (System.Exception e)
            {
                return 0;
            }
        }

        public bool DeleteRows()
        {
            try
            {
                using (var conn = GetSQLiteConnetion())
                {
                    conn.DeleteAll<ContactNotificationItem>();
                }

                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public string InsertUpdateProduct<T>(T data)
        {
            using (var conn = GetSQLiteConnetion())
            {
                try
                {
                    conn.InsertOrReplaceWithChildren(data, true);
                }
                catch (Exception e)
                {
                }
            }
            return "Single data file inserted or updated";
        }
        public string InsertUpdateProductList<T>(List<T> data)
        {
            using (var conn = GetSQLiteConnetion())
            {
                conn.InsertOrReplaceAllWithChildren(data, true);
                return "Single data file inserted or updated";
            }
        }
        public List<T> GetDataList<T>() where T : class
        {
            List<T> result = default(List<T>);

            using (var conn = GetSQLiteConnetion())
            {
                result = conn.GetAllWithChildren<T>(null, true).ToList();
                return result;
            }
        }
        public List<T> GetDataList<T>(int count) where T : class
        {
            List<T> result = default(List<T>);

            using (var conn = GetSQLiteConnetion())
            {
                result = conn.GetAllWithChildren<T>(null, true).Take(count).ToList();
                return result;
            }
        }
        public List<T> GetDataList<T>(Func<T, bool> where) where T : class
        {
            using (var conn = GetSQLiteConnetion())
            {
                var result = conn.GetAllWithChildren<T>(null, true).Where(where).ToList();
                return result;
            }
        }

    }
}
