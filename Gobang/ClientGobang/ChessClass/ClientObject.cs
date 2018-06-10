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


    //����һ��������ӵ��¼�,����Form1.cs��AddChess����
    public class AddChessEventArgs : EventArgs
    {
        public string Number;
        public string Im;
        public string Bow;
    }
    //����һ�������Ϣ���¼�,����Form1.cs��AddMessage����
    public class AddMessageEventArgs : EventArgs
    {
        public string Talk;
    }
    //����һ������Form1.cs��AginMessage����,ʵ��˫��ͬʱˢ������
    public class AginMessageEventArgs : EventArgs
    {
        public string Agin;
    }
    //����һ������Form1.cs��FiDnMessage����,ʵ��˭������
    public class FiDnMessageEventArgs : EventArgs
    {
        public string FiDn;
    }
    public class ClientObject
    {
        private Thread _workThread;//����һ���߳�����
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
        public Socket ClientSocket//����Form1���͹���������
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        //������Ϣ����
        public void StartReceive()
        {
            _workThread = new Thread(
                new ThreadStart(this.receiveMessage));
            _workThread.Start();
        }
        public void receiveMessage()
        {
            //������Ϣ������Э��
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
                //�ָ�ͷ����Ϣ���ж���Ϣ
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


        //����������Ϣ
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
        //������ܵ��Է�˵�Ļ�����Ϣ
        public void handleSendMessage(string content)
        {
            string talk = content;
            AddMessageEventArgs org = new AddMessageEventArgs();
            org.Talk = talk;
            OnAddMessage(this, org);

        }
        //����Է��������ĸ�˭���������Ϣ
        public void handleFiDnMessage(string content)
        {
            string talk = content;
            FiDnMessageEventArgs drg = new FiDnMessageEventArgs();
            drg.FiDn = talk;
            OnFiDnMessage(this, drg);

        }
        //����ˢ�����̺����¿�ʼ����Ϣ
        public void handleAginMessage(string content)
        {
            string talk = content;
            AginMessageEventArgs crg = new AginMessageEventArgs();
            crg.Agin = talk;
            OnAginMessage(this, crg);

        }
        //�����ͻ���
        public void StopClient()
        {
            _clientSocket.Close();
        }

        public void sendMessage(string message)//������Ϣ
        {
            byte[] buffer = Encoding.Default.GetBytes(message + "\r\n\r\n");
            _clientSocket.Send(buffer);
        }
    }
}
