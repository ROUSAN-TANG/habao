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
        private Socket _client; //�����������ӵ�socket
        private bool _connected; //socket����״̬,��ʾ�Ͽ����Ѿ������������

        //���ӷ�����
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

        //��������״̬
        public bool Connected
        {
            get { return _connected; }
        }
        //�ر�����
        public void CloseConnection()
        {
            if (_connected)
            {
                _client.Close();
                _connected = false;
            }
        }
        //������Ϣ
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message + "\r\n\r\n");
            _client.Send(buffer);
        }
    }
}
