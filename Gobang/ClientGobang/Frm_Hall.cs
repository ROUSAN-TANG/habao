using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GobangClass;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;


namespace ClientGobang
{
    public partial class Frm_Hall : Form
    {
        public Frm_Hall()
        {
            InitializeComponent();
            PlaySong("audio\\" + new Random().Next(1, 8) + ".wav");
        }

        #region  全局变量
        ClientClass frmClient = new ClientClass();
        Publec_Class PubClass = new Publec_Class();
        ClassUsers users;
        public static bool PressGame = false;
        #endregion

        #region 循环播放背景音乐
        private const int MM_MCINOTIFY = 0x3B9;//声明播放完时向系统发送的指令
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnString, int returnSize, IntPtr hwndCallback);
        //播放音乐
        private void PlaySong(string file)
        {
            mciSendString("close media", null, 0, IntPtr.Zero);//关闭
            mciSendString("open " + file + " alias media", null, 0, IntPtr.Zero);//播放指定音乐，alias表示将文件别名为media
            mciSendString("play media notify", null, 0, this.Handle);//播放
        }
        protected override void DefWndProc(ref Message m)//重写方法，接收自定义的指令消息
        {
            base.DefWndProc(ref m);
            if (m.Msg == MM_MCINOTIFY)//MM_MCINOTIFY
            {
                PlaySong("audio\\" + new Random().Next(1, 8) + ".wav");//播放指定音乐
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //frmClient.ImageDir();
            pictureBox_Back.Dock = DockStyle.Fill;
            F_Logon FrmLogon = new F_Logon();   //创建并引用登录窗体
            if (FrmLogon.ShowDialog(this) == DialogResult.OK)   //当登窗体的对话框的返回值为OK时
            {
                FrmLogon.Dispose();
                this.WindowState = FormWindowState.Maximized;
                udpSocket1.LocalHost = Publec_Class.ServerIP;
                udpSocket1.LocalPort = Convert.ToInt32(Publec_Class.ServerPort);
                udpSocket1.Active = true;
            }
            else
            {
                FrmLogon.Dispose();
                Close();
            }
        }

        private void Image_Close_Click(object sender, EventArgs e)
        {
            ClassMsg msg = new ClassMsg();//实例化ClassMsg类

            msg.UserName = Publec_Class.ClientName;
            msg.RoomMark = Publec_Class.TRoomM.ToString();
            msg.AreaMark = Publec_Class.TAreaM.ToString();

            msg.sendKind = SendKind.SendCommand;//当前为消息发送命令
            msg.msgCommand = MsgCommand.ExitToHall;//退出大厅
            //发送消息
            udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP.Trim()), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
        }

        private void pictureBox_LA_Click(object sender, EventArgs e)
        {
            frmClient.HidePanel(panel_Tree);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmClient.HidePanel(panel_Public);
        }
        private static ClassMsg temMsg = new ClassMsg();
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string GIP = "";
            if (PressGame == false)//判断当前用户是否已进入“对决”窗口
            {
                GIP = frmClient.ToGameIP((PictureBox)sender);//获取对方玩家的IP地址
                if (GIP == null)//如果获取的信息为null
                    GIP = "";//设置为空
                if (GIP != "")//如果对面座位有人
                {
                    temMsg.sendKind = SendKind.SendCommand;//设置发送命令
                    temMsg.msgCommand = MsgCommand.BegingRival;//设置消息为BegingRival
                    temMsg.DeskMark = ((PictureBox)sender).Name;//获取座位名
                    ClientClass.GameIP = GIP;//获取对方的IP地址
                    GIP = GIP.Substring(0, GIP.IndexOf('|'));
                    temMsg.SIP = GIP.Trim();
                    //发送信息
                    udpSocket1.Send(IPAddress.Parse(GIP), 11001, new ClassSerializers().SerializeBinary(frmClient.Game_FarInfo(GIP)).ToArray());
                }
                else
                {
                    ClientClass.GameIP = "";
                    ClientClass.GamePort = "11001";
                    ClientClass.GameName = "";
                    ClientClass.GameFraction = "";
                    ClientClass.GameCaput = "";
                    ClientClass.GameSex = "";
                }
                temMsg = null;//清空ClassMsg对象列表
                temMsg = frmClient.DeskHandle(((PictureBox)sender), imageList1, 1);//获取当前桌的信息
                PressGame = true;
                //发送消息
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());
            }
        }

        private void Frm_Hall_Shown(object sender, EventArgs e)
        {
            TreeNode BNode = treeView_Area.Nodes[0];
            BNode.Expand();
            BNode.EndEdit(false);
        }

        private void Image_Max_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;
            }
        }

        private void Image_Min_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                this.WindowState = FormWindowState.Minimized;
        }

        private void panel_Title_MouseDown(object sender, MouseEventArgs e)
        {
            frmClient.CPoint = new Point(-e.X, -e.Y);
        }

        private void panel_Title_MouseMove(object sender, MouseEventArgs e)
        {
            frmClient.FrmMove(this, e);
        }

        private void udpSocket1_DataArrival(byte[] Data, IPAddress Ip, int Port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival);
            this.BeginInvoke(outdelegate, new object[] { Data, Ip, Port });
        }

        private delegate void DataArrivaldelegate(byte[] Data, System.Net.IPAddress Ip, int Port);

        private void DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port) //当有数据到达后的处理进程
        {
            try
            {
                ClassMsg msg = new ClassSerializers().DeSerializeBinary((new System.IO.MemoryStream(Data))) as ClassMsg;
                switch (msg.msgCommand)
                {
                    case MsgCommand.UserList://进入大厅获取在线用户列表
                        {
                            frmClient.Format_ListV(listView_user, imageList2);
                            GetUserList(Data, Ip, Port);
                            break;
                        }
                    case MsgCommand.Close://退出大厅
                        {
                            udpSocket1.Active = false;
                            this.Close();
                            break;
                        }
                    case MsgCommand.ComeToHallH:
                        {
                            frmClient.Data_List(listView_user, msg.UserName, msg.Fraction.ToString(), msg.CPhoto.ToString());
                            break;
                        }
                    case MsgCommand.ExitToHall:
                        {
                            frmClient.ReMove_List(listView_user, msg.UserName);
                            break;
                        }
                    case MsgCommand.BeginToGameL:
                        {
                            Frm_Chessboard FrmChessboard = new Frm_Chessboard();
                            if (FrmChessboard.ShowDialog() == DialogResult.OK)
                            {
                                ClassMsg msg1 = new ClassMsg();
                                msg1.AreaMark = Publec_Class.TAreaM.ToString();
                                msg1.RoomMark = Publec_Class.TRoomM.ToString();
                                msg1.RIP = PubClass.MyHostIP();
                                msg1.RPort = Publec_Class.ServerPort;
                                msg1.SIP = Publec_Class.ServerIP;
                                msg1.SPort = "11000";
                                msg1.sendKind = SendKind.SendCommand;
                                msg1.msgCommand = MsgCommand.ComeToHall;
                                msg1.CPhoto = Publec_Class.CaputID;
                                //发送消息
                                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(msg1).ToArray());

                                msg.sendKind = SendKind.SendCommand;
                                msg.msgCommand = MsgCommand.EndToGame;
                                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
                            }
                            break;
                        }
                    case MsgCommand.ExitToArea:
                        {
                            if (Convert.ToInt32(msg.AreaMark) == Publec_Class.TAreaM && Convert.ToInt32(msg.RoomMark) == Publec_Class.TRoomM)
                                frmClient.ReMove_List(listView_user, msg.UserName);
                            break;
                        }
                    case MsgCommand.BeginToGameH:
                        {
                            frmClient.AddDeskMsg(flowPanel_Oppose, msg, imageList1,Ip.ToString(), msg.msgCommand);
                            break;
                        }
                    case MsgCommand.EndToGameH:
                        {
                            frmClient.AddDeskMsg(flowPanel_Oppose, msg, imageList1,Ip.ToString(), msg.msgCommand);
                            PressGame = false;
                            break;
                        }
                    case MsgCommand.EndRival:
                        {
                            frmClient.Game_TerraInfo(Data);
                            break;
                        }
                    case MsgCommand.ComeToSay:
                        {
                            frmClient.AddMsgText(RTB_Dialog, msg, msg.msgCommand);
                            break;
                        }
                }
            }
            catch { }
        }

        /// <summary>
        /// 获取当前房间的用户信息
        /// </summary>
        /// <param Data="byte[]">数据流</param>
        /// <param Ip="System.Net.IPAddress ">IP地址</param>
        /// <param Port="int">端口号</param>
        private void GetUserList(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
            ClassMsg msg = (ClassMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(Data));
            users = (ClassUsers)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));//获取所有用户信息

            ClassUserInfo userItem = new ClassUserInfo();
            frmClient.Data_List(listView_user,Publec_Class.ClientName , Publec_Class.Fraction.ToString(), Publec_Class.CaputID.ToString());
            for (int i = 0; i < users.Count; i++)
            {
                userItem = users[i];
                if (userItem.Borough == Publec_Class.TAreaM.ToString() && userItem.RoomMark == Publec_Class.TRoomM.ToString())
                {
                    if (userItem.State == (Convert.ToInt32(MsgCommand.ComeToHall)).ToString())
                    {
                        if (Publec_Class.ClientName != userItem.UserName)
                            frmClient.Data_List(listView_user, userItem.UserName, userItem.Fraction, userItem.Caput);
                    }
                    if (userItem.State == (Convert.ToInt32(MsgCommand.BeginToGame)).ToString())
                    {
                        if (Publec_Class.ClientName != userItem.UserName)
                        {
                            frmClient.Data_List(listView_user, userItem.UserName, userItem.Fraction, userItem.Caput);
                            frmClient.UserAdd_List(flowPanel_Oppose, userItem,imageList1);
                        }
                    }
                }
            }

        }


        private static int IAreaM = 0;
        private static int IRoomM = 0;
        private void treeView_Area_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ClientClass CClass = new ClientClass();//创建ClientClass对象
            string TemNodeS = e.Node.Name;//获取当前节点的名称
            TreeNode tNode = new TreeNode();//定义一个节点
            if (TemNodeS.IndexOf("Room_") >= 0)//查找节点名中是否有指定的字符串
            {
                tNode = e.Node;//获取当前节点中的所有信息
                Publec_Class.TRoomM = Convert.ToInt32(tNode.Tag.ToString());//获取当前节点的房间号
                TreeNode t2Node = new TreeNode();//定义一个节点
                t2Node = (TreeNode)tNode.Parent;//获取当前节点的父节点信息
                Publec_Class.TAreaM = Convert.ToInt32(t2Node.Tag.ToString());//获取当前房间所在区号
                if (IAreaM == Publec_Class.TAreaM && IRoomM == Publec_Class.TRoomM)//如果进入的是同一个房间 
                    return;

                if (Publec_Class.TAreaM > 0 && Publec_Class.TRoomM > 0)//如果进入了房间
                {
                    ClassMsg TeMsg = new ClassMsg();//创建ClassMsg对象
                    TeMsg.sendKind = SendKind.SendCommand;//将命令设置为发送命令
                    TeMsg.msgCommand = MsgCommand.ExitToArea;//消息设为ExitToArea
                    TeMsg.AreaMark = IAreaM.ToString();//记录当前的区号
                    TeMsg.RoomMark = IRoomM.ToString();//记录当前的房间号
                    TeMsg.UserName = Publec_Class.UserName;//记录当前用户的名称
                    //将信息发送给服务器端
                    udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(TeMsg).ToArray());
                }

                if (pictureBox_Back.Visible == true)//如果大厅中的页面图片为显示状态
                {
                    pictureBox_Back.Visible = false;//隐藏该图片
                    flowPanel_Oppose.Visible = true;//显示房间的所有座位
                }

                //设置要传递的信息
                ClassMsg msg = new ClassMsg();
                msg.AreaMark = Publec_Class.TAreaM.ToString();//记录区号
                msg.RoomMark = Publec_Class.TRoomM.ToString();//记录房间号
                msg.RIP = PubClass.MyHostIP();//记录当前用户的IP地址
                msg.RPort = Publec_Class.ServerPort;//记录当前用户的端口号
                msg.SIP = Publec_Class.ServerIP;//记录服务器端的IP地址
                msg.SPort = "11000";//记录服务器端的端口号
                msg.sendKind = SendKind.SendCommand;//设置为发送命令
                msg.msgCommand = MsgCommand.ComeToHall;//设置消息为进入大厅
                msg.CPhoto = Publec_Class.CaputID;//记录头像编号
                //发送消息
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
                CClass.SetLabelModule(flowPanel_Oppose, imageList1);//对组件进行批量操作
                //记录区号和房间号，用于判断进入的房间是否为同一区的同一个房间，如果是，则不进行操作
                IAreaM = Publec_Class.TAreaM;//记录区号
                IRoomM = Publec_Class.TRoomM;//记录房间号
            }
        }

        private void treeView_Area_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (Convert.ToInt32(e.Node.Tag.ToString()) == 0)
            {
                e.Cancel = true;
            }
        }



        private void pictureBox_Hair_Click(object sender, EventArgs e)
        {
            //设置要传递的信息
            ClassMsg msg = new ClassMsg();                          //创建ClassMsg对象
            msg.sendKind = SendKind.SendCommand;                        //设置为发送命令
            msg.msgCommand = MsgCommand.ComeToSay;              //消息为语聊
            msg.UserName = Publec_Class.UserName + "：";             //记录发送用户的名称
            msg.MsgText = comboBox_Hair.Text.Trim();                    //记录要发送的语句
                                                                        //发送消息
            udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
            comboBox_Hair.Text = "";
        }

        private void comboBox_Hair_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                pictureBox_Hair_Click(sender, e);
            }
        }

        private void Image_Min_MouseEnter(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(((PictureBox)sender).Tag.ToString()))
            {
                case 1:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\最小化变色.jpg");
                        break;
                    }
                case 2:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\最大化变色.jpg");
                        break;
                    }
                case 3:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\关闭变色.jpg");
                        break;
                    }
                case 4:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\发送按钮变色.png");
                        break;
                    }
            }
        }

        private void Image_Min_MouseLeave(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(((PictureBox)sender).Tag.ToString()))
            {
                case 1:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\最小化按钮.jpg");
                        break;
                    }
                case 2:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\最大化按钮.jpg");
                        break;
                    }
                case 3:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\关闭按钮.jpg");
                        break;
                    }
                case 4:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\发送按钮.png");
                        break;
                    }
            }
        }
    }
}