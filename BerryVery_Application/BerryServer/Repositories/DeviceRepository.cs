using BerryServer.Connections;
using BerryServer.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BerryServer.Repositories
{
    public class DeviceRepository : RepositoryBaseEx<DeviceRepository>
    {
        public DeviceRepository(ILogger<DeviceRepository> logger, DatabaseConnection db) : base(logger, db)
        {
        }

        public List<DevicePort> GetDevicePort(DevicePort para = null)
        {
            var sql = "SELECT * FROM tb_device_port WHERE 1 = 1";
            var param = new Dictionary<string, object>();

            if (para is not null)
            {
                if (para.Id > 0)
                {
                    sql += " AND id = @id";
                    param.Add("@id", para.Id);
                }
            }

            var tb = base._db.GetTable(sql, param);
            var list = new List<DevicePort>();

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                var row = tb.Rows[i];

                list.Add(new DevicePort()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = Convert.ToString(row["name"]),
                    Type = Convert.ToInt32(row["type"]),
                    Address = Convert.ToString(row["address"]),
                    Status = Convert.ToInt32(row["status"])
                });
            }

            return list;
        }

        public int PortAdd(DevicePort para)
        {
            var sql = @"
                INSERT INTO tb_device_port 
                SET name = @name, 
                    type = @type, 
                    address = @address, 
                    status = @status";
            var param = new Dictionary<string, object>();
            param.Add("@name", para.Name);
            param.Add("@type", para.Type);
            param.Add("@address", para.Address);
            param.Add("@status", para.Status);

            base._logger.LogInformation(Regex.Replace(sql, @"\s+", " ").Trim());

            var resultId = base._db.SetCommand(sql, param);

            return resultId;
        }

        public object PortUpdate(object obj)
        {
            return "";
        }

        public object PortDelete(int id)
        {
            return "";
        }
    }
}
