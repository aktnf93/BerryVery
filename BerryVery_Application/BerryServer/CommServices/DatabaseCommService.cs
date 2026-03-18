using MySql.Data.MySqlClient;
using System.Data;

namespace BerryServer.CommServices
{
    public class DatabaseCommService
    {
        public string connStr { private get; set; }
            = "Server=localhost;UserId=root;Password=1111;Database=berry_very;";

        /* INSERT
         *  var db = new S_Database();
         *  string query = "INSERT INTO users (id, name) VALUES (@id, @name)";
         *  var param = new Dictionary<string, object>() { {"@id", "admin"}, {"@name", "관리자"} };
         *  db.SetCommand(query, param);
         * 
         */

        /* SELECT
         *  var db = new S_Database();
         *  string query = "SELECT * FROM users WHERE id = @id";
         *  var param = new Dictionary<string, object>() { {"@id", "admin"} };
         *  DataTable dt = db.GetTable(query, param);
         */

        public DatabaseCommService()
        {
            Console.WriteLine("DatabaseCommService");
        }


        private MySqlCommand CreateCommand(MySqlConnection conn, string query, Dictionary<string, object> param = null)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);

            if (param != null)
            {
                foreach (var p in param)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                }
            }

            return cmd;
        }

        public int SetCommand(string query, Dictionary<string, object> param = null)
        {
            int result = 0;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                using (var cmd = CreateCommand(conn, query, param))
                {
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public DataTable GetTable(string query, Dictionary<string, object> param = null)
        {
            DataTable dt = new DataTable();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                using (var cmd = CreateCommand(conn, query, param))
                {
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}