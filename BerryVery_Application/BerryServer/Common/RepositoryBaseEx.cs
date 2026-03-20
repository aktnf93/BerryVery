using BerryServer.CommServices;

namespace BerryServer.Common
{
    public class RepositoryBaseEx<Repository>
    {
        public class QueryResult
        {
            public string Sql { get; set; }
            public Dictionary<string, object> Parameters { get; set; }
        }

        public class Condition
        {
            /// <summary>
            /// 컬럼명
            /// </summary>
            public string Column { get; set; }
            /// <summary>
            /// =, >, <, LIKE 등
            /// </summary>
            public string Operator { get; set; }
            /// <summary>
            /// 값
            /// </summary>
            public object Value { get; set; }
            /// <summary>
            /// AND / OR
            /// </summary>
            public string Logical { get; set; } = "AND";
        }

        protected readonly ILogger<Repository> _logger;
        protected readonly DatabaseCommService _db;

        public RepositoryBaseEx(ILogger<Repository> logger, DatabaseCommService db)
        {
            // Type type = typeof(T);
            // var stack = new StackTrace(true);
            // var frame = stack.GetFrame(1);
            // Console.WriteLine("RepositoryBase(logger, db) > {0}, {1}:{2}", type.Name, frame.GetFileLineNumber(), frame.GetMethod().Name);

            this._logger = logger;
            this._db = db;
        }


        /// <summary>
        /// <example>
        /// This shows how to increment an integer.
        /// <code>
        /// var result = BuildInsert("Device", new Dictionary&lt;string, object&gt; { ["Name"] = "Test", ["Port"] = 1 });
        /// </code>
        /// </example>
        /// SQL : INSERT INTO Device (Name, Port) VALUES (@Name, @Port)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected QueryResult BuildInsert(string table, Dictionary<string, object> values)
        {
            var columns = string.Join(", ", values.Keys);
            var paramNames = values.Keys.Select(k => "@" + k);

            var sql = $"INSERT INTO {table} ({columns}) VALUES ({string.Join(", ", paramNames)})";

            return new QueryResult
            {
                Sql = sql,
                Parameters = values
            };
        }

        /// <summary>
        /// <example>
        /// <code>
        /// var result = BuildUpdate("Device",
        ///     new Dictionary&lt;string, object&gt; { ["Name"] = "Updated" },
        ///     new Dictionary&lt;string, object&gt; { ["Id"] = 1 });
        /// </code>
        /// SQL : UPDATE Device SET Name = @set_Name WHERE Id = @where_Id
        /// </example>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="setValues"></param>
        /// <param name="whereValues"></param>
        /// <returns></returns>
        protected QueryResult BuildUpdate(string table, Dictionary<string, object> setValues, Dictionary<string, object> whereValues)
        {
            var setClause = string.Join(", ",
                setValues.Keys.Select(k => $"{k} = @set_{k}"));

            var whereClause = string.Join(" AND ",
                whereValues.Keys.Select(k => $"{k} = @where_{k}"));

            var sql = $"UPDATE {table} SET {setClause} WHERE {whereClause}";

            var parameters = new Dictionary<string, object>();

            foreach (var kv in setValues)
                parameters["set_" + kv.Key] = kv.Value;

            foreach (var kv in whereValues)
                parameters["where_" + kv.Key] = kv.Value;

            return new QueryResult
            {
                Sql = sql,
                Parameters = parameters
            };
        }

        /*
        var result = BuildDelete("Device", new Dictionary<string, object> { ["Id"] = 1 });
        DELETE FROM Device WHERE Id = @where_Id
         */
        protected QueryResult BuildDelete(string table, Dictionary<string, object> whereValues)
        {
            if (whereValues == null || !whereValues.Any())
                throw new Exception("WHERE 조건 없음");

            var whereClause = string.Join(" AND ",
                whereValues.Keys.Select(k => $"{k} = @where_{k}"));

            var sql = $"DELETE FROM {table} WHERE {whereClause}";

            var parameters = whereValues.ToDictionary(
                kv => "where_" + kv.Key,
                kv => kv.Value);

            return new QueryResult
            {
                Sql = sql,
                Parameters = parameters
            };
        }



        /*
        var conditions = new List<Condition>
        {
            new Condition { Column = "Id", Operator = "=", Value = 1 }, // 첫 조건
            new Condition { Column = "Id", Operator = ">", Value = 10, Logical = "OR" }
        };
        WHERE Id = @w_Id_0 OR Id > @w_Id_1


        new List<Condition>
        {
            new Condition { Column = "Name", Operator = "=", Value = "A" },
            new Condition { Column = "Age", Operator = ">", Value = 20 },
            new Condition { Column = "Score", Operator = "<", Value = 50, Logical = "OR" }
        }
        WHERE Name = @w_Name_0 AND Age > @w_Age_1 OR Score < @w_Score_2
         */

        private (string whereSql, Dictionary<string, object> parameters)BuildWhere(List<Condition> conditions)
        {
            var whereParts = new List<string>();
            var parameters = new Dictionary<string, object>();

            for (int i = 0; i < conditions.Count; i++)
            {
                var c = conditions[i];
                var paramName = $"@w_{c.Column}_{i}";

                var prefix = i == 0 ? "" : $" {c.Logical} ";

                whereParts.Add($"{prefix}{c.Column} {c.Operator} {paramName}");

                parameters[paramName.TrimStart('@')] = c.Value;
            }

            var whereSql = whereParts.Any()
                ? "WHERE " + string.Join("", whereParts)
                : "";

            return (whereSql, parameters);
        }

        protected QueryResult BuildUpdate(string table, Dictionary<string, object> setValues, List<Condition> conditions)
        {
            var setClause = string.Join(", ",
                setValues.Keys.Select(k => $"{k} = @set_{k}"));

            var (whereSql, whereParams) = BuildWhere(conditions);

            var sql = $"UPDATE {table} SET {setClause} {whereSql}";

            var parameters = new Dictionary<string, object>();

            foreach (var kv in setValues)
                parameters["set_" + kv.Key] = kv.Value;

            foreach (var kv in whereParams)
                parameters[kv.Key] = kv.Value;

            return new QueryResult
            {
                Sql = sql,
                Parameters = parameters
            };
        }

        protected QueryResult BuildDelete(string table, List<Condition> conditions)
        {
            var (whereSql, whereParams) = BuildWhere(conditions);

            if (string.IsNullOrWhiteSpace(whereSql))
                throw new Exception("WHERE 조건 없음");

            return new QueryResult
            {
                Sql = $"DELETE FROM {table} {whereSql}",
                Parameters = whereParams
            };
        }
    }
}
