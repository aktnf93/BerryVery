using BerryServer.Application.Repositories;
using BerryServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace BerryServer.Infrastructure.Data
{
    public class DatabaseConnection
    {
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

        private readonly ILogger<DatabaseConnection> _logger;
        private string _connStr;

        public DatabaseConnection(ILogger<DatabaseConnection> logger, IConfiguration config)
        {
            this._logger = logger;
            this._connStr = config.GetConnectionString("Default") ?? string.Empty;

            _logger.LogInformation("{ConnectionString}", _connStr);
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

                using (var cmd = this.CreateCommand(conn, query, param))
                {
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public List<T> GetCommand<T>(string query, Func<MySqlDataReader, T> map, Dictionary<string, object> param = null)
        {
            List<T> result = new List<T>();

            using (var conn = new MySqlConnection(this._connStr))
            {
                conn.Open();

                using (var cmd = this.CreateCommand(conn, query, param))
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

                using (var cmd = this.CreateCommand(conn, query, param))
                {
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// 지정한 SQL 쿼리를 비동기적으로 실행하여 결과 행을 T로 매핑해 스트리밍합니다.
        /// </summary>
        /// <remarks>
        /// 지연 실행됩니다. 열거를 완료하거나 취소해야 연결과 리더가 해제됩니다. 전체 결과를 메모리에 버퍼링하지 않으므로 순차적 반복 중 예외가 발생할 수 있습니다.
        /// </remarks>
        /// <typeparam name="T">DbDataReader의 행을 T 인스턴스로 매핑하도록 IRowMapper<T>를 구현한 타입.</typeparam>
        /// <param name="query">실행할 SQL 쿼리 문자열.</param>
        /// <param name="param">매개변수 이름과 값을 담은 사전(선택 사항). null이면 매개변수 없음.</param>
        /// <param name="cancellationToken">열거 및 비동기 작업의 취소를 제어하는 토큰; [EnumeratorCancellation]로 스트리밍 취소와 동기화됩니다.</param>
        /// <returns>데이터베이스 결과를 비동기적으로 스트리밍하는 IAsyncEnumerable<T>. 열거는 연결과 리더를 사용하며 순차적으로 레코드를 반환합니다.</returns>
        public async IAsyncEnumerable<T> GetCommandAsync<T>(
            string query,
            Dictionary<string, object> param = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class, IMapper<T>
        {
            await using (var conn = new MySqlConnection(this._connStr))
            {
                await conn.OpenAsync(cancellationToken);

                await using (var cmd = this.CreateCommand(conn, query, param))
                {
                    await using (DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            yield return T.Map(reader);
                        }
                    }
                }
            }
        }
    }
}