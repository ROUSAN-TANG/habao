using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;
using System.Windows.Forms;

namespace ClientGobang.ChessClass
{


    //建立一个添加棋子的事件,触发Form1.cs的AddChess方法
    public class AddChessEventArgs : EventArgs
    {
        public string Number;
        public string Im;
        public string Bow;
    }
    //建立一个添加消息的事件,触发Form1.cs的AddMessage方法
    public class AddMessageEventArgs : EventArgs
    {
        public string Talk;
    }
    //建立一个触发Form1.cs的AginMessage方法,实现双方同时刷新桌面
    public class AginMessageEventArgs : EventArgs
    {
        public string Agin;
    }
    //建立一个触发Form1.cs的FiDnMessage方法,实现谁先下棋
    public class FiDnMessageEventArgs : EventArgs
    {
        public string FiDn;
    }
    public class ClientObject
    {
        private Thread _workThread;//声明一个线程名称
        private IPEndPoint _remoteIPEndPoint;
        private Socket _clientSocket;
        public event EventHandler<AddChessEventArgs> OnAddChess;
        public event EventHandler<AddMessageEventArgs> OnAddMessage;
        public event EventHandler<AginMessageEventArgs> OnAginMessage;
        public event EventHandler<FiDnMessageEventArgs> OnFiDnMessage;
        public IPEndPoint RemoteIPEndPoint
        {
            get { return _remoteIPEndPoint; }
            set { _remoteIPEndPoint = value; }
        }
        public Socket ClientSocket//接受Form1传送过来的属性
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        //接收消息方法
        public void StartReceive()
        {
            _workThread = new Thread(
                new ThreadStart(this.receiveMessage));
            _workThread.Start();
        }
        public void receiveMessage()
        {
            //接收消息并处理协议
            while (true)
            {
                byte[] buffer = new byte[1024];
                int recByte = 0;
                string text = string.Empty;

                while (true)
                {
                    try
                    {
                        recByte = _clientSocket.Receive(buffer);
                    }
                    catch
                    {
                        return;
                    }
                    if (recByte == 0) return;
                    text += Encoding.Default.GetString(buffer, 0, recByte);
                    if (text.IndexOf("\r\n\r\n") > 0) break;
                }
                //分割头部信息和判断信息
                string header = text.Substring(0, 4);
                string content = text.Substring(5, text.Length - 9);

                switch (header)
                {
                    case "Down":
                        handleDownMessage(content);
                        break;
                    case "Send":
                        handleSendMessage(content);
                        break;
                    case "Agin":
                        handleAginMessage(content);
                        break;
                    case "FiDn":
                        handleFiDnMessage(content);
                        break;
                    default:
                        break;
                }
            }
        }


        //处理下棋消息
        public void handleDownMessage(string content)
        {
            int pos = content.IndexOf("|");
            int kos = content.IndexOf("@");
            string num = content.Substring(0, pos);
            string bow = content.Substring(kos + 1);
            string im = content.Substring(pos + 1, 1);


            AddChessEventArgs arg = new AddChessEventArgs();
            arg.Number = num;
            arg.Im = im;
            arg.Bow = bow;
            OnAddChess(this, arg);

        }
        //处理接受到对方说的话的消息
        public void handleSendMessage(string content)
        {
            string talk = content;
            AddMessageEventArgs org = new AddMessageEventArgs();
            org.Talk = talk;
            OnAddMessage(this, org);

        }
        //处理对方发过来的该谁先下棋的消息
        public void handleFiDnMessage(string content)
        {
            string talk = content;
            FiDnMessageEventArgs drg = new FiDnMessageEventArgs();
            drg.FiDn = talk;
            OnFiDnMessage(this, drg);

        }
        //处理刷新棋盘和重新开始的消息
        public void handleAginMessage(string content)
        {
            string talk = content;
            AginMessageEventArgs crg = new AginMessageEventArgs();
            crg.Agin = talk;
            OnAginMessage(this, crg);

        }
        //结束客户端
        public void StopClient()
        {
            _clientSocket.Close();
        }

        public void sendMessage(string message)//发送消息
        {
            byte[] buffer = Encoding.Default.GetBytes(message + "\r\n\r\n");
            _clientSocket.Send(buffer);
        }
    }
}
