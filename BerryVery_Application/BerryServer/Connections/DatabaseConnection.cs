using BerryServer.Route.Api.Device.Entities;
using MySql.Data.MySqlClient;
using System.Data;

namespace BerryServer.Connections
{
<<<<<<< HEAD:BerryVery_Application/BerryServer/CommServices/DatabaseCommService.cs

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

    public class DatabaseCommService
=======
    public class DatabaseConnection
>>>>>>> origin/main:BerryVery_Application/BerryServer/Connections/DatabaseConnection.cs
    {
        private string _connStr;

<<<<<<< HEAD:BerryVery_Application/BerryServer/CommServices/DatabaseCommService.cs
        public DatabaseCommService(IConfiguration config)
=======
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

        public DatabaseConnection()
>>>>>>> origin/main:BerryVery_Application/BerryServer/Connections/DatabaseConnection.cs
        {
            _connStr = config.GetConnectionString("Default") ?? string.Empty;

            Console.WriteLine("DatabaseCommService > {0}", _connStr);
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

            using (var conn = new MySqlConnection(this._connStr))
            {
                conn.Open();

                using (var cmd = CreateCommand(conn, query, param))
                {
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public List<T> GetCommand<T>(string query, Func<MySqlDataReader, T> map, Dictionary<string, object> param = null)
        {
            var result = new List<T>();

            using (var conn = new MySqlConnection(this._connStr))
            {
                conn.Open();

                using (var cmd = CreateCommand(conn, query, param))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(map(reader));
                        }
                    }
                }
            }

            return result;
        }

        private DataTable GetTable(string query, Dictionary<string, object> param = null)
        {
            DataTable dt = new DataTable();

            using (var conn = new MySqlConnection(this._connStr))
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