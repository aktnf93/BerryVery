using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerrDeviceSimulator.Common
{
    /// <summary>
    /// <para> EnvConfig.Load(); // 프로그램 시작 시 한번만 호출 </para>
    /// <para> string apiUrl = EnvConfig.Get("API_URL"); </para>
    /// <para> string dbConn = EnvConfig.Get("DB_CONN"); </para>
    /// </summary>
    public static class EnvConfig
    {
        private static Dictionary<string, string> _values = new Dictionary<string, string>();

        // 파일 읽기
        public static void Load(string path = "env")
        {
            if (!File.Exists(path)) return;

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('='); // key=value
                if (parts.Length == 2)
                {
                    _values[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        // 값 가져오기
        public static string Get(string key, string defaultValue = "")
        {
            return _values.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
