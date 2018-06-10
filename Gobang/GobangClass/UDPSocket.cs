using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace GobangClass
{
    public partial class UDPSocket : Component
    {
        #region  ȫ�ֱ���
        private IPEndPoint ServerEndPoint = null;   //��������˵�
        private UdpClient UDP_Server = new UdpClient(); //�����������,Ҳ����UDP��Socket
        private System.Threading.Thread thdUdp; //����һ���߳�
        private string localHost = "127.0.0.1";
        #endregion

        #region  ����������¼�
        public delegate void DataArrivalEventHandler(byte[] Data, IPAddress Ip, int Port);  //������һ���йܷ���
        public event DataArrivalEventHandler DataArrival;   //ͨ���й����ڿؼ��ж���һ���¼�
        #endregion

        #region  �������������
        [Browsable(true), Category("Local"), Description("����IP��ַ")]   //�ڡ����ԡ���������ʾlocalHost����
        public string LocalHost
        {
            get { return localHost; }
            set { localHost = value; }
        }

        private int localPort = 11000;
        [Browsable(true), Category("Local"), Description("���ض˿ں�")] //�ڡ����ԡ���������ʾlocalPort����
        public int LocalPort
        {
            get { return localPort; }
            set { localPort = value; }
        }

        private bool active = false;
        [Browsable(true), Category("Local"), Description("�������")]   //�ڡ����ԡ���������ʾactive����
        public bool Active
        {
            get { return active; }
            set //�����Զ�ȡֵ
            {
                active = value;
                if (active) //��ֵΪTrueʱ
                {
                    OpenSocket();   //�򿪼���
                }
                else
                {
                    CloseSocket();  //�رռ���
                }
            }
        }
        #endregion

        public UDPSocket()
        {
            InitializeComponent();
        }

        public UDPSocket(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region  �Է��͵����Ž��м���
        /// <summary>
        /// �Է��͵����Ž��м���
        /// </summary>
        protected void Listener()   //����
        {
            ServerEndPoint = new IPEndPoint(IPAddress.Any, localPort);   //��IP��ַ�Ͷ˿ں�������˵�洢
            if (UDP_Server != null)
                UDP_Server.Close();
            try
            {
                UDP_Server = new UdpClient(localPort);  //����һ���µĶ˿ں�
            }
            catch
            {
                int port = new Random().Next(localPort + 1, 65535);//������ɶ˿ں�
                UDP_Server = new UdpClient(port);  //����һ���µĶ˿ں�
                MessageBox.Show(localPort + " �˿��Ѿ���ռ�ã�ϵͳ���Զ�Ϊ������˿ںţ��Զ�����Ķ˿ں�Ϊ��" + port);
            }
            UDP_Server.Client.ReceiveBufferSize = 1000000000;   //���ջ�������С
            UDP_Server.Client.SendBufferSize = 1000000000;  //���ͻ�������С
            try
            {
                thdUdp = new Thread(new ThreadStart(GetUDPData));   //����һ���߳�
                thdUdp.Start(); //ִ�е�ǰ�߳�
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + "\n" + "����ʧ��");  //��ʾ�̵߳Ĵ�����Ϣ
            }
        }
        #endregion

        #region  ��ȡ��ǰ���յ���Ϣ
        /// <summary>
        /// ��ȡ¼ǰ���յ���Ϣ
        /// </summary>
        private void GetUDPData()   //��ȡ��ǰ���յ���Ϣ
        {
            while (active)
            {
                try
                {
                    byte[] Data = UDP_Server.Receive(ref ServerEndPoint);   //����ȡ��Զ����Ϣת���ɶ�������
                    DataArrival?.Invoke(Data, ServerEndPoint.Address, ServerEndPoint.Port);//���õ�ǰ�ؼ���DataArrival�¼�����Ϣ����Զ�̼����
                    Thread.Sleep(0);
                }
                catch { }
            }
        }
        #endregion

        #region  ��ȡί�ж���
        /// <summary>
        /// ��ȡί�ж���
        /// </summary>
        private void CallBackMethod(IAsyncResult ar)
        {
            //���첽״̬ar.AsyncState�У���ȡί�ж���
            DataArrivalEventHandler dn = (DataArrivalEventHandler)ar.AsyncState;
            //һ��ҪEndInvoke����������³��ܲ�
            dn.EndInvoke(ar);
        }
        #endregion

        #region  ����Ϣ���͸�Զ�̼����
        /// <summary>
        /// ����Ϣ���͸�Զ�̼����
        /// </summary>
        public void Send(System.Net.IPAddress Host, int Port, byte[] Data)
        {
            try
            {
                int pp = this.localPort;//��ָ����IP��ַ�Ͷ˿ںų�ʼ��server
                UDP_Server.Send(Data, Data.Length, new IPEndPoint(Host, Port));//����Ϣ����Զ�̼����
            }
            catch { }
        }
        #endregion

        #region  ��socket
        /// <summary>
        /// ��socket
        /// </summary>
        private void OpenSocket()    //��socket
        {
            if (UDP_Server != null)
                UDP_Server.Close();
            Listener();         //ͨ���÷�����UDPЭ����м���
        }
        #endregion

        #region  �ر�socket
        /// <summary>
        /// �ر�socdet
        /// </summary>
        private void CloseSocket()    //�ر�socket
        {
            if (UDP_Server != null)     //���socket��Ϊ��
                UDP_Server.Close();     //�ر�socket
            if (thdUdp != null)         //����Զ����̱߳���
            {
                Thread.Sleep(30);       //˯�����߳�
                thdUdp.Abort();         //�ر����߳�
            }
        }
        #endregion
    }
}
