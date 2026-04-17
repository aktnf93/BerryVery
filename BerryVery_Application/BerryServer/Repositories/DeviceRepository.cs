using BerryServer.Connections;
using BerryServer.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;

namespace BerryServer.Repositories
{
    public class DeviceRepository : RepositoryBaseEx<DeviceRepository>
    {
        public DeviceRepository(ILogger<DeviceRepository> logger, DatabaseConnection db) : base(logger, db)
        {
        }

        public List<DevicePort> GetDevicePort()
        {
            var sql = "SELECT * FROM tb_device_port WHERE 1 = 1";

            var result = base.Db.GetCommand<DevicePort>(sql, (r =>
                {
                    try
                    {
                        var data = r.GetInt32("test");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    return new DevicePort()
                    {
                        Id = Convert.ToUInt32(r["port_id"]),
                        Name = Convert.ToString(r["port_name"]) ?? string.Empty,
                        Type = Convert.ToUInt32(r["port_type"]),
                        Address = Convert.ToString(r["port_address"]) ?? string.Empty,
                        Status = Convert.ToUInt32(r["port_status"])
                    };
                }));

            return result;
        }

        public int PortAdd(DevicePort para)
        {
            var sql = @"
                INSERT INTO tb_device_port 
                SET name = @name, 
                    type = @type, 
                    address = @address, 
                    status = @status";
            var param = new Dictionary<string, object>
            {
                { "@name", para.Name },
                { "@type", para.Type },
                { "@address", para.Address },
                { "@status", para.Status }
            };

            base.Logger.LogInformation(Regex.Replace(sql, @"\s+", " ").Trim());

            var resultId = base.Db.SetCommand(sql, param);

            return resultId;
        }

        public int PortUpdate(object obj)
        {
            return 1;
        }

        public int PortDelete(int id)
        {
            return 1;
        }
    }
}
