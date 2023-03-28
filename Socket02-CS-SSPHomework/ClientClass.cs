using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Socket02_CS_SSPHomework
{
    class ClientClass
    {
        public class Client
        {
            Socket sock;
            int m_port = 12345;
            public void Connect()
            {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress serverAddr = IPAddress.Parse("192.168.0.38");
                IPEndPoint clientEP = new IPEndPoint(serverAddr, m_port);
                sock.BeginConnect(clientEP, new AsyncCallback(ConnectCallback), sock);
            }
            public void Close()
            {
                if (sock != null)
                {
                    sock.Close();
                    sock.Dispose();
                }
            }
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
            void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;
                    client.EndConnect(ar);
                    AsyncObject obj = new AsyncObject(4096);
                    obj.WorkingSocket = sock;
                    sock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            void DataReceived(IAsyncResult ar)
            {
                AsyncObject obj = (AsyncObject)ar.AsyncState;

                int received = obj.WorkingSocket.EndReceive(ar);

                byte[] buffer = new byte[received];

                Array.Copy(obj.Buffer, 0, buffer, 0, received);
            }
            public void Send(byte[] msg)
            {
                sock.Send(msg);
            }
        }
    }
}
