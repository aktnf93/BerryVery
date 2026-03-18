using System.Net;
using System.Net.Sockets;
using Timer = System.Timers.Timer;
using IPAddress = System.Net.IPAddress;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;

namespace BerryDevice.CommServices
{
    public class SocketCommService : BackgroundService
    {
        public class SocketObj
        {
            public const int BUFFER_SIZE = 8192;
            public Socket Client { get; set; }
            public byte[] Buffer { get; set; }

            /// <summary>
            /// 서버에서 클라이언트 등록 시 주소
            /// </summary>
            public string RemoteEndPoint { get; set; }

            // 클라이언트에서 서버로 재접속 시 주소
            public string Host { get; set; }
            public int Port { get; set; }
            public bool IsReconnecting { get; set; } = false;

            public SocketObj(Socket client, int bufferSize)
            {
                this.Client = client;
                this.Buffer = new byte[bufferSize];
            }
        }

        public class MessageQueue
        {
            public SocketObj Target { get; set; }
            public byte[] Packet { get; set; }
            public int SendCount { get; set; } = 1;
        }

        public event EventHandler<string, byte[]> DataReceivedEvent;
        public event EventHandler<string, string> ExceptionEvent;

        // __________________________________________________________
        // 서버로 운용 시
        private Timer _listenTimer;
        private TcpListener _listener;
        private ConcurrentDictionary<string, SocketObj> _listenerClients = new ConcurrentDictionary<string, SocketObj>();

        // __________________________________________________________
        // 클라로 운용 시
        private SocketObj _client;
        private Timer _clientTimer;

        // __________________________________________________________
        private Thread _dataSendThread;
        private ConcurrentQueue<MessageQueue> _dataSendQueue = new ConcurrentQueue<MessageQueue>();
        private AutoResetEvent _dataSendWait;
        private const int DATA_SEND_CHECK_TIME = 10000;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                this.ListenerStart(8500);

                // while (!stoppingToken.IsCancellationRequested)
                // {
                // 
                // }
            });
        }

        public SocketCommService()
        {
            this.Init();
        }

        private void Init()
        {
            this._listenTimer = new Timer(3000);
            this._listenTimer.Elapsed += (sender, e) =>
            {
                try
                {
                    var s = this._listener.Server;

                    Console.WriteLine("{0} Socket Monitor > " +
                        "Available={1}, Blocking={2}, IsBound={3}, Connected={4}, LingerState.Enabled={5} " +
                        "Poll Write={6}, Poll Read={7}, Poll Error={8}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        s.Available, s.Blocking, s.IsBound, s.Connected, s.LingerState.Enabled,
                        s.Poll(300, SelectMode.SelectWrite), s.Poll(300, SelectMode.SelectRead), s.Poll(300, SelectMode.SelectError));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Socket Monitor Error > {0}", ex.Message);
                }
            };

            this._clientTimer = new Timer(3000);
            this._clientTimer.Elapsed += (sender, e) =>
            {
                try
                {
                    var s = this._client.Client;

                    Console.WriteLine("{0} Socket Monitor > " +
                        "Available={1}, Blocking={2}, IsBound={3}, Connected={4}, LingerState.Enabled={5} " +
                        "Poll Write={6}, Poll Read={7}, Poll Error={8}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        s.Available, s.Blocking, s.IsBound, s.Connected, s.LingerState.Enabled,
                        s.Poll(300, SelectMode.SelectWrite), s.Poll(300, SelectMode.SelectRead), s.Poll(300, SelectMode.SelectError));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Socket Monitor Error > {0}", ex.Message);
                }
            };

            this._dataSendWait = new AutoResetEvent(false);
            this._dataSendThread = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("송신 큐 체크");

                    while (this._dataSendQueue.TryDequeue(out MessageQueue message))
                    {
                        if (message.Target.Client.Connected)
                        {
                            for (int i = 0; i < message.SendCount; i++)
                            {
                                try
                                {
                                    Console.WriteLine("소켓 송신 > {0}", BitConverter.ToString(message.Packet).Replace('-', ' '));
                                    message.Target.Client.Send(message.Packet);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("송신 오류 > {0}", ex.Message);
                                    this.ExceptionEvent?.Invoke("송신 오류", ex.Message);
                                }
                            }
                        }
                    }

                    Console.WriteLine("송신 대기");
                    this._dataSendWait.WaitOne(DATA_SEND_CHECK_TIME);
                }

                Console.WriteLine("송신 큐 오류");
            });
            this._dataSendThread.Start();
        }

        public void ListenerStart(int port)
        {
            this._listenerClients.Any(x => {

                Console.WriteLine("disconnect {0}", x.Value.Client?.RemoteEndPoint);
                x.Value.Client?.Disconnect(false);

                return false;
            });
            this._listenerClients.Clear();

            this._listener?.Dispose();
            this._listener = new TcpListener(IPAddress.Any, port);
            this._listener.Start();
            this._listener.BeginAcceptSocket(this.ListenerAcceptSocket, this._listener);

            this._listenTimer.Start();
            Console.WriteLine("{0} > server listen ip={1}, port={2}", DateTime.Now, IPAddress.Any, port);
        }

        public void ListenerStop()
        {
            this._listener?.Dispose();

            this._listenTimer.Stop();
        }

        public void ListenerConnect(string hostname, int port)
        {
            this._client = new SocketObj(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), SocketObj.BUFFER_SIZE);
            this._client.Host = hostname;
            this._client.Port = port;
            this._client.IsReconnecting = true;

            this._client.Client.BeginConnect(hostname, port, this.ClientConnectSocket, this._client);

            this._clientTimer.Start();
        }

        public void ListenerDisconnect()
        {
            this._client.Client?.Disconnect(false);

            this._clientTimer?.Stop();
        }

        public void SendToClient(byte[] packet, string host = "")
        {
            if (string.IsNullOrEmpty(host))
            {
                foreach (var c in this._listenerClients)
                {
                    this._dataSendQueue.Enqueue(new MessageQueue()
                    {
                        Target = c.Value,
                        Packet = packet,
                        SendCount = 1
                    });
                }
            }
            else if (this._listenerClients.TryGetValue(host, out SocketObj c))
            {
                this._dataSendQueue.Enqueue(new MessageQueue()
                {
                    Target = c,
                    Packet = packet,
                    SendCount = 1
                });
            }

            this._dataSendWait.Set();
        }

        public void SendToListener(byte[] packet)
        {
            this._dataSendQueue.Enqueue(new MessageQueue()
            {
                Target = this._client,
                Packet = packet, 
                SendCount = 1
            });

            this._dataSendWait.Set();
        }

        private void ListenerAcceptSocket(IAsyncResult ar)
        {
            string address = string.Empty;
            string clientAddress = string.Empty;

            if (ar.AsyncState is TcpListener listener)
            {
                try
                {
                    address = listener.LocalEndpoint.ToString();

                    var socket = new SocketObj(listener.EndAcceptSocket(ar), SocketObj.BUFFER_SIZE);
                    clientAddress = socket.Client.RemoteEndPoint.ToString();

                    // connected event
                    if(this._listenerClients.TryAdd(clientAddress, socket))
                    {
                    }
                    else
                    {
                    }

                    socket.Client.BeginReceive(socket.Buffer, 0, socket.Buffer.Length, SocketFlags.None, this.ClientDataReceived, socket);

                    listener.BeginAcceptSocket(this.ListenerAcceptSocket, listener);
                }
                catch (Exception ex)
                {
                    this.ExceptionEvent?.Invoke(address, ex.Message);
                }
            }
        }

        private void ClientConnectSocket(IAsyncResult ar)
        {
            Task.Delay(1000).Wait();
            string address = string.Empty;

            if (ar.AsyncState is SocketObj socket)
            {
                try
                {
                    socket.Client.EndConnect(ar);

                    socket.Client.BeginReceive(socket.Buffer, 0, socket.Buffer.Length, SocketFlags.None, this.ClientDataReceived, socket);
                }
                catch (Exception ex)
                {
                    this.ExceptionEvent?.Invoke(address, ex.Message);

                    this.Reconnecting(socket);
                }
            }
        }

        private void Reconnecting(SocketObj socket)
        {
            if (socket.Client.Poll(300, SelectMode.SelectRead))
            {
                socket.Client.Disconnect(true);
                socket.Client.Dispose();
                socket.Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            socket.Client.BeginConnect(socket.Host, socket.Port, this.ClientConnectSocket, socket);
        }

        private void ClientDataReceived(IAsyncResult ar)
        {
            string address = string.Empty;

            if (ar.AsyncState is SocketObj socket)
            {
                try
                {
                    int length = socket.Client.EndReceive(ar);
                    address = socket.Client.RemoteEndPoint.ToString();

                    if (length > 0)
                    {
                        byte[] data = new byte[length];
                        Array.Copy(socket.Buffer, data, length);

                        // data received event
                        this.DataReceivedEvent?.Invoke(address, data);

                        Array.Clear(socket.Buffer, 0, length);
                        socket.Client.BeginReceive(socket.Buffer, 0, socket.Buffer.Length, SocketFlags.None, this.ClientDataReceived, socket);
                    }
                }
                catch (Exception ex)
                {
                    this.ExceptionEvent?.Invoke(address, ex.Message);

                    if (socket.IsReconnecting)
                    {
                        this.Reconnecting(socket);
                    }
                    else  if (this._listenerClients.TryRemove(socket.RemoteEndPoint, out SocketObj obj))
                    {
                        obj.Client?.Disconnect(false);
                    }
                }
            }
        }
    }
}
