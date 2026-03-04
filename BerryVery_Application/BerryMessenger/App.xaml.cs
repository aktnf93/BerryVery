using BerryMessenger.ViewModels;
using BerryMessenger.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BerryMessenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var vm = new VM_MainWindow();
            var window = new V_MainWindow
            {
                DataContext = vm
            };

            Converters.Packet.ToData();

            window.Show();
        }
    }

    public class Converters
    {
        public static ConvPacket Packet => new ConvPacket();
    }

    public class ConvPacket
    {
        public object ToData()
        {
            return new object();
        }
    }

}
