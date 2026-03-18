using BerryServer.CommServices;
using BerryServer.Entities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BerryServer.Repositories
{
    public class DeviceRepository
    {
        private readonly ILogger<DeviceRepository> _logger;
        private DatabaseCommService _db;

        public DeviceRepository(ILogger<DeviceRepository> logger, DatabaseCommService db)
        {
            this._logger = logger;
            this._db = db;
        }

        public List<PortEntity> GetDevicePort(PortEntity para = null)
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

            var tb = this._db.GetTable(sql, param);
            var list = new List<PortEntity>();

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                var row = tb.Rows[i];

                list.Add(new PortEntity()
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

        public int PortAdd(PortEntity para)
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

            _logger.LogInformation(Regex.Replace(sql, @"\s+", " ").Trim());

            var resultId = this._db.SetCommand(sql, param);

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
