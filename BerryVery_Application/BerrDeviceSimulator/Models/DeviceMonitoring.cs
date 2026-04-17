using System;
using System.Collections.Generic;
using System.Text;

namespace BerrDeviceSimulator.Models
{
    public class DeviceMonitoring
    {
        public DevicePort Port { get; set; }
        public DeviceGate Gate { get; set; }
        public DeviceCtrl Ctrl { get; set; }

        public List<DeviceSub> SubList { get; set; } = new List<DeviceSub>();
    }
}
