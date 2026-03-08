using BerryClass.Services;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BerryTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        S_SerialPort port;

        public MainWindow()
        {
            InitializeComponent();

            void log(object sender, object e, [CallerLineNumber] int call = 0)
            {
                Console.WriteLine("{0:HH:mm:ss.fff}\t{1}, {2}, {3}",
                    DateTime.Now, call, sender, e);
            }

            port = new S_SerialPort("COM2");
            port.ConnectionChanged += (sender, e) => log(sender, e);
            port.DataReceived += (sender, e) => log(sender, e);
            port.Error += (sender, e) => log(sender, e);
            port.Connect();
        }
    }
}