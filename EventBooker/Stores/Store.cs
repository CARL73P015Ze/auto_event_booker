using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.Data.SQLite.Generic;
namespace EventBooker.Stores
{
    class Store
    {
        public void Initialize()
        {
            string datapath = System.IO.Path.Combine(Environment.CurrentDirectory, "EventBooker.db");

            string connStr = string.Format("Data Source={0}", datapath);
            SQLiteConnection conn = new SQLiteConnection(connStr);
            conn.Open();
            //conn.Commit();

        
                
            
           // conn.Database = "";
        }
    }
}
