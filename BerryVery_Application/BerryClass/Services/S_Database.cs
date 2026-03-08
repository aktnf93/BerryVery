using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BerryClass.Services
{
    public class S_Database
    {
        string query = "select * from tb_device_ctrl where 1 = 1;";
        string connStr = "Server=localhost;UserId=root;Password=1111;Database=berry_very;";

        public DataTable GetTable(string query)
        {
            DataTable dt = new DataTable();

            using (var adapter = new MySqlDataAdapter(query, connStr))
            {
                adapter.Fill(dt);
            }

            return dt;
        }
    }
}
