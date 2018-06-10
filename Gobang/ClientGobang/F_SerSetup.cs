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

        //获取本机IP地址
        private void F_SerSetup_Load(object sender, EventArgs e)
        {
            IPHostEntry ip = Dns.GetHostByName(Dns.GetHostName());//获取指定主机的IP地址
            text_IP.Text = ip.AddressList[0].ToString(); //将得到的IP地址显示在文本框中
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (text_PassWord.Text.Trim() == text_PassWord2.Text.Trim())        //当密码输入相同
            {
                udpSocket1.LocalPort = Convert.ToInt32(text_Port.Text.Trim());  //获取端口号
                udpSocket1.Active = true;
                ClassMsg msg = new ClassMsg();                          //引用类库中的ClassMsg类
                msg.UserName = text_Name.Text;                          //获取注册的用户名
                msg.PassWord = text_PassWord.Text;                      //获取用户密码
                msg.sendKind = SendKind.SendCommand;                //设置为发送命令
                msg.msgCommand = MsgCommand.Registering;            //将消息命令设为用户注册
                serID = text_IP.Text.Trim();                                //获取服务器的IP地址
                msg.RIP = serID.Trim();                             //获取本机的IP地址
                msg.RPort = text_Port.Text.Trim();                          //获取端口号
                msg.CPhoto = comboBox_CPhoto.SelectedIndex;         //获取头像的编号
                if (comboBox_Sex.SelectedIndex <= 0)                        //获取性别
                    msg.Sex = 0;
                else
                    msg.Sex = 1;
                //用udpSocket1控件的Send方法将消息发给服务器
                udpSocket1.Send(IPAddress.Parse(serID), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
            }
            else
            {
                text_PassWord.Text = "";
                text_PassWord2.Text = "";
                MessageBox.Show("密码与确认密码不匹配，请重新输入。");
            }
        }

        private delegate void DataArrivaldelegate(byte[] Data, System.Net.IPAddress Ip, int Port);

        private void sockUDP1_DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival); //托管
            this.BeginInvoke(outdelegate, new object[] { Data, Ip, Port }); //异步执行托管
        }

        private void DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port) //当有数据到达后的处理进程
        {
            try
            {
                ClassMsg msg = new ClassSerializers().DeSerializeBinary((new System.IO.MemoryStream(Data))) as ClassMsg;

                switch (msg.msgCommand)
                {
                    case MsgCommand.Registered://注册成功
                        {
                            DialogResult = DialogResult.OK;//设置窗体的对话框结果
                            //向INI文件中写入服务器IP地址
                            WritePrivateProfileString("MyGobang", "IP", serID, Application.StartupPath + "\\Gobang.ini");
                            //向INI文件中写入服务器端口号
                            WritePrivateProfileString("MyGobang", "Port", text_Port.Text.Trim(), Application.StartupPath + "\\Gobang.ini");
                            //向INI文件中写入用户名称
                            WritePrivateProfileString("MyGobang", "Name", text_Name.Text.Trim(), Application.StartupPath + "\\Gobang.ini");
                            //向INI文件中写入用户的头像编号
                            WritePrivateProfileString("MyGobang", "Caput", comboBox_CPhoto.SelectedIndex.ToString(), Application.StartupPath + "\\Gobang.ini");
                            udpSocket1.Active = false;
                            this.Close();
                            break;
                        }
                    case MsgCommand.RegisterAt://注册失败
                        {
                            MessageBox.Show("用户名已被注册，请重新输入！");
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
                sf.Alignment = StringAlignment.Near;                            //设置文本的对齐方式
                                                                                //如果当前项没有焦点或没有键盘加速键
                if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                {
                    imageList1.Draw(e.Graphics, r.Left, r.Top, e.Index);            //绘制图像
                    e.DrawFocusRectangle();                             //显示取得焦点时的虚线框
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);       //设置各项的背景颜色
                    imageList1.Draw(e.Graphics, r.Left, r.Top, e.Index);            //绘制图片
                    e.DrawFocusRectangle();                             //显示取得焦点时的虚线框
                }
            }
        }

        private void F_SerSetup_Shown(object sender, EventArgs e)
        {
            comboBox_CPhoto.Items.Clear();
            comboBox_CPhoto.DrawMode = DrawMode.OwnerDrawFixed;         //所有元素是手动绘制的
            comboBox_CPhoto.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 0; i < imageList1.Images.Count; i++)                   //根据绘制图片的个数，添加空的列表项
            {
                comboBox_CPhoto.Items.Add("");
            }
        }
    }
}