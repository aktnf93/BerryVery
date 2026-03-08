
namespace BerryClass.Services
{
    public interface ICommService
    {
        event EventHandler<string, bool> ConnectionChanged;
        event EventHandler<string, byte[]> DataReceived;
        event EventHandler<string, Exception> Error;

        bool IsConnected { get; }

        void Connect();

        void Disconnect();

        bool DataSend(byte[] data);

    }
}
