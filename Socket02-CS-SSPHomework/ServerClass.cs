using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Socket02_CS_SSPHomework
{

    public static class Global
    {
        public static string sendData;
    }
    class ServerClass
    {
        public string ServerClassTest()
        {
            string retVal = "Hello Client, This is Server!";
            Console.WriteLine(retVal);
            return retVal;

            //서버 입장 소켓 생성 코드
            // 1. 소켓 생성 //클리이언트던 서버던 소켓 생성은 동일
            // 2. 바인더 : 서비스할 주소와 포트를 소켓에 연결 / 서버는 서비스 주소, 서비스는 서버 주소 / 서버 클라이언트 둘다 형식은 동일
            //서버 소켓 무한 루프
            // 3. 서버 : Listening /대기중
            // 4. Accept
            //여기까지가 establish 과정
            //Accept지점이 분기점 되어 동시 접속 처리 가능하다. - 스레드로 동작.
            //구조체 생성
            //스레드로
            //서버 소켓 무한 루프
            //차일드 소켓 무한 루프
            // 5. 메시지 수신
            // 6. 메시지 송신 (에코로 설정한다.)
            //차일드 소켓 무한 루프
            // 7. 접송 종료 // 열었던 것의 반대로 닫아준다. 1 2 >> 2 1 순서로 닫기



            //서버쪽 대기 시작하는코드
            //Server server = new Server();
            //server.Start();


            //클라이언트쪽 연결시도하는 코드
            //Client client = new Client();
            //client.Connect();

            //연결이 완료됐다면 전송하는 코드
            //server.Send(msg);
            //client.Send(msg);

        }


        public void ServerClassStart()
        {
            // (1) 소켓 객체 생성 (TCP 소켓)
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // (2) 포트에 바인드
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 12345);
            sock.Bind(serverEP);

            // (3) 포트 Listening 시작
            sock.Listen(10);

            // (4) 연결을 받아들여 새 소켓 생성 (하나의 연결만 받아들임)
            Socket clientSock = sock.Accept();

            byte[] buff = new byte[8192];
            while (!Console.KeyAvailable) // 키 누르면 종료
            {
                // (5) 소켓 수신
                int n = clientSock.Receive(buff);

                string data = Encoding.UTF8.GetString(buff, 0, n);
                Console.WriteLine(data);

                // (6) 소켓 송신
                clientSock.Send(buff, 0, n, SocketFlags.None);  // echo
            }

            // (7) 소켓 닫기
            clientSock.Close();
            sock.Close();
        }



        Socket sock;
        List<Socket> connectedClients = new List<Socket>();
        //int m_port = 12345;

        // Start()는 Server 메인소켓을 만들고 연결을 받아들이기 시작하는 메소드

        public void Start(int m_port)
        {
            try
            {
                // (1) 소켓 객체 생성 (TCP 소켓)
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // (2) 포트에 바인드
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, m_port);
                sock.Bind(serverEP);

                // (3) 포트 Listening 시작
                sock.Listen(10);

                // (4-1) 연결을 받아들여 새 소켓 생성
                sock.BeginAccept(AcceptCallback, null);
                Console.WriteLine($"Server Start... Listen port {serverEP.Port}...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        // Close()는 만들어진 메인소켓을 해제, Dispose하고 연결되어있던 모든 Connectedclients를 해제하는 작업
        // (7) 소켓 닫기
        public void Close()
        {
            if (sock != null)
            {
                sock.Close();
                sock.Dispose();
            }

            // 접속 List에 있는 모든 Client를 Close.
            foreach (Socket socket in connectedClients)
            {
                socket.Close();
                socket.Dispose();
            }
            connectedClients.Clear();

            //mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }


        // 클라이언트에서 연결을 시도하면, 무슨 행동을 해야할지 정하는 부분.
        /*
        public class AsyncObject
        {
            public byte[] Buffer;
            public Socket WorkingSocket;
            public readonly int BufferSize;
            public AsyncObject(int bufferSize)
            {
                BufferSize = bufferSize;
                Buffer = new byte[(long)BufferSize];
            }

            public void ClearBuffer()
            {
                Array.Clear(Buffer, 0, BufferSize);
            }
        }
        */

        /* 받아들인 소켓의 정보들은 IASyncResult ar에 담겨있다.
            mainSock.EndAccept(ar);을 호출하면 메인소켓이 더이상 Client를 받아들이지 않고, 현재 받아들인 소켓의 정보를
            client라는 소켓에 저장하게 된다.
            이제 client와 연결이 완료되었고, client가 서버에 데이터를 보낼때마다 DataReceived라는 콜백함수로 넘어가서 작업을
            진행하게 된다. 
        */
        
        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // (4-2) 연결을 받아들여 새 소켓 생성
                // Accept 정보 대입
                Socket client = sock.EndAccept(ar);
                //AsyncObject obj = new AsyncObject(1920 * 1080 * 3);
                //obj.WorkingSocket = client;
                connectedClients.Add(client);
                //client.BeginReceive(obj.Buffer, 0, 1920 * 1080 * 3, 0, DataReceived, obj);

                sock.BeginAccept(AcceptCallback, null);
                // 서버 접속시 Client정보 콘솔 출력
                Console.WriteLine($"Client : (From: {client.RemoteEndPoint}, Connection time: {DateTime.Now})");

                byte[] binary = new Byte[1024];

                //Client에게 환영 메세지 전송
                client.Send(Encoding.UTF8.GetBytes("Welcome server!\r\n>"));
                
                while (true)
                {
                    // (5) 소켓 수신
                    client.Receive(binary);
                    string data = Encoding.UTF8.GetString(binary);
                    data = data.Trim('\0');
                    Console.WriteLine("Message = " + data);

                    Global.sendData = data;
                    

                    // (6) 소켓 송신
                    byte[] sendMsg = Encoding.UTF8.GetBytes("ECHO : " + data + "\r\n>");
                    client.Send(sendMsg);
                }

            }
            catch (Exception e)
            { 
                Console.WriteLine(e.ToString());
            }
        }


        /* obj.Buffer는 1920 * 1080 * 3으로 지정해준 byte[] 배열이다.
            이곳에 클라이언트에서 보낸 데이터가 입력되는데, byte[1000]의 크기로 보냈다면, 1000개만 꺼내와서
            buffer라는 배열에 따로 저장했다. 이 buffer를 사용해서 무슨 작업을 할지는 자유롭게 결정하면 된다.
        */

        /*
        public void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            int received = obj.WorkingSocket.EndReceive(ar);

            byte[] buffer = new byte[received];

            Array.Copy(obj.Buffer, 0, buffer, 0, received);
        }
        */


        //UTF8 문자열을 euc-kr문자열로 변환한다.
        private string UTF8_TO_EUCKR(string strUTF8)
        {

            return Encoding.GetEncoding("euc-kr").GetString(
                Encoding.Convert(
                Encoding.UTF8,
                Encoding.GetEncoding("euc-kr"),
                Encoding.UTF8.GetBytes(strUTF8)));
        }

        //euc-kr 문자열을 UTF8문자열로 변환한다.
        private string EUCKR_TO_UTF8(string strEUCKR)
        {
            return Encoding.UTF8.GetString(
                   Encoding.Convert(
                   Encoding.GetEncoding("euc-kr"),
                   Encoding.UTF8,
                   Encoding.GetEncoding("euc-kr").GetBytes(strEUCKR)));
        }


        /*
        public void SendData()
        {
            Console.WriteLine(Global.sendData);
        }
        */

    }




}
