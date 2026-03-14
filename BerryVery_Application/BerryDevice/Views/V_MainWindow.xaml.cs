using BerryDevice.Services;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace BerryDevice.Views
{
    public partial class V_MainWindow : Window
    {
        S_SerialPort port;

        public V_MainWindow()
        {
            InitializeComponent();


            void log(object sender, object e, [CallerLineNumber] int call = 0)
            {
                Console.WriteLine("{0:HH:mm:ss.fff}\t{1}, {2}, {3}", 
                    DateTime.Now, call, sender, e);
            }

            port = new S_SerialPort("COM2");
            port.ConnectionChanged += (sender, e) => log(sender, e);
            port.DataReceived      += (sender, e) => log(sender, e);
            port.Error             += (sender, e) => log(sender, e);
            port.Connect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = "02aabbccddeeff03";
            var arr = Encoding.UTF8.GetBytes(data);

            port.DataSend(arr);
        }
    }
}