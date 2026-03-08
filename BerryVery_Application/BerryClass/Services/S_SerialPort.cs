using System.IO.Ports;
using Timer = System.Timers.Timer;

namespace BerryClass.Services
{
    public class S_SerialPort : ICommService
    {
        public event EventHandler<string, bool> ConnectionChanged;
        public event EventHandler<string, byte[]> DataReceived;
        public event EventHandler<string, Exception> Error;

        private readonly SerialPort _comport;
        private readonly Timer _comportChecker;

        public bool IsConnected { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits">0=None, 1=One(1), 2=Two(2), 3=OnePointFive(1.5)</param>
        /// <param name="parity">0=None, 1=Odd(홀수), 2=Even(짝수), 3=Mark(패리티1), 4=Space(패리티0)</param>
        public S_SerialPort(string portName = "COM1", int baudRate = 19200, int dataBits = 8, int stopBits = 1, int parity = 0)
        {
            this._comport = new SerialPort()
            {
                PortName = portName,
                BaudRate = baudRate,
                DataBits = dataBits,
                StopBits = (StopBits)stopBits,
                Parity   = (Parity)parity,

                ReceivedBytesThreshold = 1,

                ReadTimeout  = 50,
                WriteTimeout = 50
            };

            this._comport.DataReceived += OnDataReceived;

            this._comportChecker = new Timer(1000);
            this._comportChecker.Elapsed += (sender, e) =>
            {
                bool oldStatus = this.IsConnected;

                if (this._comport.IsOpen)
                {
                    this.IsConnected = true;
                }
                else
                {
                    try
                    {
                        if (SerialPort.GetPortNames().Any(x => x.Equals(this._comport.PortName)))
                        {
                            this._comport.Open();
                            this.IsConnected = true;
                        }
                        else throw new Exception("NonePort");
                    }
                    catch (Exception ex)
                    {
                        this.Error?.Invoke(this._comport.PortName, ex);
                        this.IsConnected = false;
                    }
                }

                if (oldStatus != this.IsConnected)
                    this.ConnectionChanged?.Invoke(this._comport.PortName, this.IsConnected);
            };
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is SerialPort port)
            {
                try
                {
                    int length = port.BytesToRead;
                    if (length > 0)
                    {
                        var buffer = new byte[length];

                        int readLength = port.Read(buffer, 0, buffer.Length);
                        if (readLength < length)
                            Array.Resize(ref buffer, readLength);

                        this.DataReceived?.Invoke(port.PortName, buffer);
                    }
                }
                catch (Exception ex)
                {
                    this.Error?.Invoke(port.PortName, ex);
                }
            }
        }

        public void Connect()
        {
            this._comportChecker?.Start();
        }

        public void Disconnect()
        {
            this._comportChecker?.Stop();
            this._comportChecker?.Dispose();

            this._comport?.Close();
            this._comport?.Dispose();

            this.IsConnected = false;
        }

        public bool DataSend(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                    throw new ArgumentException("Data cannot be null or empty");

                if (!this._comport.IsOpen)
                    throw new InvalidOperationException("Port is not open");

                this._comport.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                this.Error?.Invoke(this._comport.PortName, ex);
                return false;
            }
        }
    }
}
