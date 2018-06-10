using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using GobangClass;

namespace ClientGobang
{
    public partial class F_SerSetup : Form
    {
        public F_SerSetup()
        {
            InitializeComponent();
        }

        ClientClass frmClass = new ClientClass();
        Publec_Class PubClass =new Publec_Class();
        string serID = "";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //��ȡ����IP��ַ
        private void F_SerSetup_Load(object sender, EventArgs e)
        {
            IPHostEntry ip = Dns.GetHostByName(Dns.GetHostName());//��ȡָ��������IP��ַ
            text_IP.Text = ip.AddressList[0].ToString(); //���õ���IP��ַ��ʾ���ı�����
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (text_PassWord.Text.Trim() == text_PassWord2.Text.Trim())        //������������ͬ
            {
                udpSocket1.LocalPort = Convert.ToInt32(text_Port.Text.Trim());  //��ȡ�˿ں�
                udpSocket1.Active = true;
                ClassMsg msg = new ClassMsg();                          //��������е�ClassMsg��
                msg.UserName = text_Name.Text;                          //��ȡע����û���
                msg.PassWord = text_PassWord.Text;                      //��ȡ�û�����
                msg.sendKind = SendKind.SendCommand;                //����Ϊ��������
                msg.msgCommand = MsgCommand.Registering;            //����Ϣ������Ϊ�û�ע��
                serID = text_IP.Text.Trim();                                //��ȡ��������IP��ַ
                msg.RIP = serID.Trim();                             //��ȡ������IP��ַ
                msg.RPort = text_Port.Text.Trim();                          //��ȡ�˿ں�
                msg.CPhoto = comboBox_CPhoto.SelectedIndex;         //��ȡͷ��ı��
                if (comboBox_Sex.SelectedIndex <= 0)                        //��ȡ�Ա�
                    msg.Sex = 0;
                else
                    msg.Sex = 1;
                //��udpSocket1�ؼ���Send��������Ϣ����������
                udpSocket1.Send(IPAddress.Parse(serID), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
            }
            else
            {
                text_PassWord.Text = "";
                text_PassWord2.Text = "";
                MessageBox.Show("������ȷ�����벻ƥ�䣬���������롣");
            }
        }

        private delegate void DataArrivaldelegate(byte[] Data, System.Net.IPAddress Ip, int Port);

        private void sockUDP1_DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival); //�й�
            this.BeginInvoke(outdelegate, new object[] { Data, Ip, Port }); //�첽ִ���й�
        }

        private void DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port) //�������ݵ����Ĵ������
        {
            try
            {
                ClassMsg msg = new ClassSerializers().DeSerializeBinary((new System.IO.MemoryStream(Data))) as ClassMsg;

                switch (msg.msgCommand)
                {
                    case MsgCommand.Registered://ע��ɹ�
                        {
                            DialogResult = DialogResult.OK;//���ô���ĶԻ�����
                            //��INI�ļ���д�������IP��ַ
                            WritePrivateProfileString("MyGobang", "IP", serID, Application.StartupPath + "\\Gobang.ini");
                            //��INI�ļ���д��������˿ں�
                            WritePrivateProfileString("MyGobang", "Port", text_Port.Text.Trim(), Application.StartupPath + "\\Gobang.ini");
                            //��INI�ļ���д���û�����
                            WritePrivateProfileString("MyGobang", "Name", text_Name.Text.Trim(), Application.StartupPath + "\\Gobang.ini");
                            //��INI�ļ���д���û���ͷ����
                            WritePrivateProfileString("MyGobang", "Caput", comboBox_CPhoto.SelectedIndex.ToString(), Application.StartupPath + "\\Gobang.ini");
                            udpSocket1.Active = false;
                            this.Close();
                            break;
                        }
                    case MsgCommand.RegisterAt://ע��ʧ��
                        {
                            MessageBox.Show("�û����ѱ�ע�ᣬ���������룡");
                            text_Name.Text = "";
                            text_PassWord.Text = "";
                            text_PassWord2.Text = "";
                            break;
                        }
                }
            }
            catch { }
        }

        private void F_SerSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpSocket1.Active = false;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void comboBox_CPhoto_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            Size imageSize = imageList1.ImageSize;
            if (e.Index >= 0)
            {
                string s = (string)comboBox_CPhoto.Items[e.Index];
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;                            //�����ı��Ķ��뷽ʽ
                                                                                //�����ǰ��û�н����û�м��̼��ټ�
                if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                {
                    imageList1.Draw(e.Graphics, r.Left, r.Top, e.Index);            //����ͼ��
                    e.DrawFocusRectangle();                             //��ʾȡ�ý���ʱ�����߿�
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);       //���ø���ı�����ɫ
                    imageList1.Draw(e.Graphics, r.Left, r.Top, e.Index);            //����ͼƬ
                    e.DrawFocusRectangle();                             //��ʾȡ�ý���ʱ�����߿�
                }
            }
        }

        private void F_SerSetup_Shown(object sender, EventArgs e)
        {
            comboBox_CPhoto.Items.Clear();
            comboBox_CPhoto.DrawMode = DrawMode.OwnerDrawFixed;         //����Ԫ�����ֶ����Ƶ�
            comboBox_CPhoto.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 0; i < imageList1.Images.Count; i++)                   //���ݻ���ͼƬ�ĸ�������ӿյ��б���
            {
                comboBox_CPhoto.Items.Add("");
            }
        }
    }
}