using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ClientGobang.ChessClass
{
    class Client
    {
        private Socket _client; //跟服务器连接的socket
        private bool _connected; //socket连接状态,表示断开或已经连接两种情况

        //连接服务器
        public void ConnectServer(string ip, int port)
        {
            _client = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(
                IPAddress.Parse(ip),
                port);

            _client.Connect(ep);
            _connected = true;
        }

        //返回连接状态
        public bool Connected
        {
            get { return _connected; }
        }
        //关闭连接
        public void CloseConnection()
        {
            if (_connected)
            {
                _client.Close();
                _connected = false;
            }
        }
        //发送消息
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message + "\r\n\r\n");
            _client.Send(buffer);
        }
    }
}
