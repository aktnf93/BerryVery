using System.Net;
using System.Net.Sockets;
using Timer = System.Timers.Timer;
using IPAddress = System.Net.IPAddress;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;

namespace BerryServer.CommServices
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
            public string Host { get; set; }
            public int Port { get; set; }
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
        public Thread _dataSendThread;
        private ConcurrentQueue<MessageQueue> _dataSendQueue = new ConcurrentQueue<MessageQueue>();

        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                ListenerStart(8500);

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
                    var s = this._listener?.Server;
                    string msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} > " +
                        "Available={0}, IsBound={1}, Connected={2}, LingerState.Enabled={3} " +
                        "Poll Write={4}, Poll Read={5}, Poll Error={6}",
                        DateTime.Now,
                        s.Available,
                        s.IsBound,
                        s.Connected,
                        s.LingerState.Enabled,
                        s.Poll(300, SelectMode.SelectWrite),
                        s.Poll(300, SelectMode.SelectRead),
                        s.Poll(300, SelectMode.SelectError));

                    Console.WriteLine("Socket Listen Status > {0}", msg);


                    // this._listener.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Socket Listen Timer Error > {0}", ex.Message);
                }
            };

            this._clientTimer = new Timer(3000);
            this._clientTimer.Elapsed += (sender, e) =>
            {
                try
                {
                    if (this._client is not null)
                    {
                        var s = this._client?.Client;
                        string msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} > " +
                            "Available={0}, IsBound={1}, Connected={2}, LingerState.Enabled={3} " +
                            "Poll Write={4}, Poll Read={5}, Poll Error={6}",
                            DateTime.Now,
                            s.Available,
                            s.IsBound,
                            s.Connected,
                            s.LingerState.Enabled,
                            s.Poll(300, SelectMode.SelectWrite),
                            s.Poll(300, SelectMode.SelectRead),
                            s.Poll(300, SelectMode.SelectError));

                        Console.WriteLine("Socket Client Status > {0}", msg);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Socket Client Timer Error > {0}", ex.Message);
                }
            };
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

        public void SendToClient(string address, byte[] packet)
        {
            if (string.IsNullOrEmpty(address))
            {
                foreach (var c in this._listenerClients)
                {
                    c.Value.Client?.Send(packet);
                }
            }
            else
            {
                if (this._listenerClients.TryGetValue(address, out SocketObj c))
                {
                    c.Client?.Send(packet);
                }
            }
        }

        public void SendToListener(byte[] packet)
        {
            if (this._client is not null && this._client.Client is not null)
            {
                if (this._client.Client.Connected)
                {
                    this._client.Client.Send(packet);
                }
                else Console.WriteLine("서버로 데이터를 보낼 수 없습니다. > 연결 끊김.");
            }
            else Console.WriteLine("서버로 데이터를 보낼 수 없습니다. > 객체 없음.");
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
                    if (this._listenerClients.TryAdd(clientAddress, socket))
                    {
                        Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff} > client add server={1}, client={2}", DateTime.Now, address, clientAddress);
                    }
                    else
                    {
                        Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff} > client add fail server={1}, client={2}", DateTime.Now, address, clientAddress);
                    }

                    socket.Client.BeginReceive(socket.Buffer, 0, socket.Buffer.Length, SocketFlags.None, this.ClientDataReceived, socket);

                    listener.BeginAcceptSocket(this.ListenerAcceptSocket, listener);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("클라 접속 오류 > {0}, {1}, {2}", address, clientAddress, ex.Message);
                    this.ExceptionEvent?.Invoke(address, ex.Message);
                }
            }
        }

        private void ClientConnectSocket(IAsyncResult ar)
        {
            string address = string.Empty;

            if (ar.AsyncState is SocketObj socket)
            {
                try
                {
                    socket.Client.EndConnect(ar);

                    Console.WriteLine("서버 접속 성공 !");

                    socket.Client.BeginReceive(socket.Buffer, 0, socket.Buffer.Length, SocketFlags.None, this.ClientDataReceived, socket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("서버 접속 오류 > {0}, {1}", address, ex.Message);
                    this.ExceptionEvent?.Invoke(address, ex.Message);

                    Console.WriteLine("접속 오류 > 서버 접속 재시도 > {0}, {1}:{2}", address, socket.Host, socket.Port);
                    socket.Client.BeginConnect(socket.Host, socket.Port, this.ClientConnectSocket, socket);
                }
            }
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
                        Console.WriteLine("소켓 데이터 수신 > {0}, {1}", address, BitConverter.ToString(data).Replace("-", " "));
                        this.DataReceivedEvent?.Invoke(address, data);

                        Array.Clear(socket.Buffer, 0, length);
                        socket.Client.BeginReceive(socket.Buffer, 0, socket.Buffer.Length, SocketFlags.None, this.ClientDataReceived, socket);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("소켓 데이터 수신 오류 > {0}, {1}", address, ex.Message);
                    this.ExceptionEvent?.Invoke(address, ex.Message);

                    if (socket.IsReconnecting)
                    {
                        Console.WriteLine("수신 오류 > 서버 접속 재시도 > {0}, {1}:{2}", address, socket.Host, socket.Port);
                        socket.Client.BeginConnect(socket.Host, socket.Port, this.ClientConnectSocket, socket);
                    }
                    else
                    {
                        Console.WriteLine("수신 오류 > 클라이언트 객체 삭제 > {0}, {1}:{2}", address, socket.Host, socket.Port);
                        if (this._listenerClients.TryRemove(socket.RemoteEndPoint, out SocketObj obj))
                        {
                            obj.Client?.Disconnect(false);
                        }
                    }
                }
            }
        }
    }
}
