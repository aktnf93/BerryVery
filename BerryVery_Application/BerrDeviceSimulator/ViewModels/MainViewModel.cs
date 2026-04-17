using BerrDeviceSimulator.CommServices;
using BerrDeviceSimulator.Models;
using BerryDevice.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;

namespace BerrDeviceSimulator.ViewModels
{
    public class MainViewModel
    {
        private DatabaseCommService _db = new DatabaseCommService();
        private SerialPortCommService _com = new SerialPortCommService();

        public ObservableCollection<DeviceMonitoring> DeviceMonitoringList { get; set; }
            = new ObservableCollection<DeviceMonitoring>();

        public ICollectionView DeviceMonitoringView { get; set; }

        public MainViewModel()
        {
            this.DeviceMonitoringView = CollectionViewSource.GetDefaultView(this.DeviceMonitoringList);
        }

        private void OnLoaded()
        {
            this.SetupView();
        }

        private void SetupView()
        {
            var list = this.DeviceMonitoringList;
            list.Clear();

            string sql = @"SELECT * FROM v_device WHERE 1 = 1;";
            var tb = this._db.GetTable(sql);

            foreach (var row in tb.Select())
            {
                var device = new DeviceMonitoring();

                device.Port = new DevicePort()
                {
                    PortId      = row.Field<uint>("port_id"),
                    PortName    = row.Field<string>("port_name") ?? string.Empty,
                    PortType    = row.Field<uint>("port_type"),
                    PortAddress = row.Field<string>("port_address") ?? string.Empty,
                    PortStatus  = row.Field<uint>("port_status")
                };


                list.Add(device);
            }

            this.DeviceMonitoringView.Refresh();
        }
    }
}
