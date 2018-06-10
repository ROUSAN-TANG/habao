using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;//域名解析功能的命名空间
using System.Collections;//代送器的命名空间
using System.Net.Sockets;//Sockets命名空间
using System.Threading;//线程命名空间
using System.IO;
using GobangClass;

namespace ClientGobang
{
    public partial class Frm_Chessboard : Form
    {
        public Frm_Chessboard()
        {
            InitializeComponent();
        }

        public static int ConnHandle = 0;//对连接、准备、开始按钮的操作标识进行记录
        public static float Mouse_X = 0;//记录鼠标的X坐标
        public static float Mouse_Y = 0;//记录鼠标的Y坐标
        public static int[,] note = new int[15, 15];//记录棋子的摆放位置
        public static bool Conqueror = false;//记录当前是否已决出胜负
        public static int CKind = -1;//记录取胜的棋种
        public static bool dropchild = true;//当前该谁落子
        public static bool StartListen = false;//是否启动了监听
        public static int jlsf = -1;
        public static string LineName = "";//线程名
        public static bool Agin_if = false;//判断是否重新开始
        public static int if_UPdata = 0;//是否更改分数
        public static bool childSgin = true;//棋子类型
        public static bool ChildSgin = true;//对方棋子类型

        public int BBow = 0;
        int BwinCount = 0;
        int WwinCount = 0;
        bool Out = false;
        public bool CanAgin, CanAgins, CanDown, WhoFisrtDown;
        private Socket listener;
        private Thread mainThread;
        ClassUsers users;
        ClientClass frmClient = new ClientClass();

        public static string GIP = ClientClass.GameIP;//IP地址
        public static string GPort = "11003";//端口号
        public static string Gem_N = "";//名称
        public static int Gem_F = 0;//分数
        public static int Gem_C = 0;//头像
        public static int Gem_S = 0;//性别

        public void GetGameInfo(string temInfo)
        {
            if (temInfo.Length == 0)                                //如果获取的信息为空，并不是对面没有玩家
                return;
            string Tem_Str = GIP;                                   //记录对方的信息
            if (Tem_Str.IndexOf('|') == -1)                         //判断玩家信息是否正确
                return;
            try
            {
                GIP = Tem_Str.Substring(0, Tem_Str.IndexOf('|'));       //获取对方玩家的IP地址
                                                                        //获取截取字段之后的所有字符串
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_N = Tem_Str.Substring(0, Tem_Str.IndexOf('|'));             //获取对方玩家的名称
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_F = Convert.ToInt32(Tem_Str.Substring(0, Tem_Str.IndexOf('|')));    //获取对方玩家的分数
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_C = Convert.ToInt32(Tem_Str.Substring(0, Tem_Str.IndexOf('|')));    //获取对方玩家的头像编号
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_S = Convert.ToInt32(Tem_Str);                           //获取对方玩家的性别
            }
            catch
            {
                MessageBox.Show("获取对方的数据时出错");
            }
        }

        private void Frm_Chessboard_Shown(object sender, EventArgs e)
        {
            frmClient.Format_ListV(listView_Battle, imageList2);        //对用户列表进行初始化
                                                                        //将当前用户的信息添加到用户列表中
            frmClient.Data_List(listView_Battle, Publec_Class.UserName, Publec_Class.Fraction.ToString(), Publec_Class.CaputID.ToString());
            label_N.Text = Publec_Class.UserName;                   //显示当前用户的名称
            label_F.Text = Publec_Class.Fraction.ToString();            //显示当前用户的分数
            pictureBox_C.Image = null;                              //清空当前用户的头像
            if (Publec_Class.UserSex == 0)                          //如果当前用户是男性
            {
                pictureBox_C.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\男人.png");
            }
            else
            {
                pictureBox_C.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\女人.png");
            }
            GIP = ClientClass.GameIP.Trim();                        //获取对方玩家的相关信息
            if (GIP == "")                                      //如果为空，表示只有自已进入该桌
            {
                pboxStart.Enabled = false;                          //使“开始”按钮不可用
                pictureBox_Left.Image = null;                       //清空对决双方的头像图片
                pictureBox_Right.Image = null;
                if (Publec_Class.UserSex == 0)                      //如果当前用户是男性
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\男人.png");
                }
                else
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\女人.png");
                }
                label_Right.Text = Publec_Class.UserName;           //显示当前用户的名称
                pictureBox_Q_Right.Image = null;                    //清空显示棋子类型的图片
                                                                    //显示当前用户为黑棋
                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");
                pboxStart.Image = null;                      //清空“开始”按钮
                                                                    //使“开始”按钮为灰度显示
                pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮灰.png");
                pboxStart.Enabled = false;                      //使“开始”按钮不可用
            }
            else
            {
                GetGameInfo(ClientClass.GameIP);                    //获取对方玩家的相关信息
                ClassMsg temMsg = new ClassMsg();
                ClassUsers Users = new ClassUsers();
                ClassUserInfo UserItem = new ClassUserInfo();           //创建并引用ClassUserInfo类
                pictureBox_Left.Image = null;                       //清空当前用户的头像
                if (Publec_Class.UserSex == 0)                      //如果当前用户是男性
                {
                    pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\男人.png");
                }
                else
                {
                    pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\女人.png");
                }
                pictureBox_Q_Left.Image = null;                     //清空当前用户的棋子类型
                                                                    //显示当前用户的棋子类型为白棋
                pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\白棋.png");
                label_Left.Text = Publec_Class.UserName;            //显示当前用户的名称
                pictureBox_Right.Image = null;                      //清空对方玩家的头像
                if (Gem_S == 0)                                 //如果对方玩家是男性
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\男人.png");
                }
                else
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\女人.png");
                }
                label_Right.Text = Gem_N;                           //显示对方玩家的名称
                pictureBox_Q_Right.Image = null;                    //清空对方玩家的棋子类型
                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");
                //将对方玩家的信息显示在用户列表中
                frmClient.Data_List(listView_Battle, Gem_N, Gem_F.ToString(), Gem_C.ToString());
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.GetGameF;
                temMsg.RIP = GIP;

                //原来是11000
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11001, new ClassSerializers().SerializeBinary(temMsg).ToArray());
                pboxStart.Image = null;                     //清空“开始”按钮
                pboxStart.Enabled = true;                       //使“开始”按钮可用
                                                                //加载“开始”按钮的可用图片
                pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮.png");
                UserItem.UserIP = Publec_Class.ClientIP;            //记录用户的IP地址
                UserItem.UserPort = Publec_Class.ServerPort;            //记录端口号
                UserItem.UserName = Publec_Class.UserName;          //记录用户名称
                UserItem.Fraction = Publec_Class.Fraction.ToString();       //当前分数
                UserItem.Caput = Publec_Class.CaputID.ToString();       //头像
                UserItem.Sex = Publec_Class.UserSex.ToString();     //性别
                Users.add(UserItem);                                //将单用户信息添加到用户列表中
                pboxStart.Enabled = true;
                ThreadStart ts = new ThreadStart(this.StartServer);     //开启服务
                mainThread = new Thread(ts);
                mainThread.Name = "GOB_Chess";
                LineName = mainThread.Name;
                mainThread.Start();                             //连接服务
                                                                //将用户列表写入二进制流中
                temMsg.Data = new ClassSerializers().SerializeBinary(Users).ToArray();
                temMsg.msgCommand = MsgCommand.BeginJoin;       //设置发送的消息命令
                                                                //将当前用户的信息发送给对方玩家
                udpSocket1.Send(IPAddress.Parse(GIP), 11001, new ClassSerializers().SerializeBinary(temMsg).ToArray());
                StartListen = true;
                ConnHandle = 1;
            }
            panel_Check.Click += new EventHandler(asd);         //加载棋盘的单击事件
            CanDown = false;
            for (int i = 0; i < 15; i++)                                //对棋子的记录位置进行初始化
                for (int j = 0; j < 15; j++)
                    note[i, j] = -1;
        }

        private void pboxStart_Click(object sender, EventArgs e)
        {
            pboxStart.Image = null;//清空“开始”按钮
            //设置“开始”按钮为灰度
            pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮灰.png");
            pboxStart.Enabled = false;//使“开始”按钮不可用

            switch (ConnHandle)
            {
                case 1://第1次单击“开始”按钮，用于连接对方并开始下棋
                    {
                        ChessClass.Client cc = Program.PublicClientObject;
                        if (cc.Connected)//如果已开启了连接
                        {
                            cc.CloseConnection();//断开连接
                        }

                        try
                        {
                            if (GIP == "")//如果没有对方的IP地址
                                break;
                            cc.ConnectServer(GIP, int.Parse("11003"));//建立连接
                        }
                        catch
                        {
                            MessageBox.Show("连接服务器失败");//如果连接失败，使“开始”按钮可用
                            this.pboxStart.Image = null;
                            this.pboxStart.Enabled = true;
                            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮.png");
                            return;
                        }
                        WhoFisrtDown = false;//设置谁先下棋
                        cc.SendMessage("FiDn" + "Me");//发送信息
                        CanDown = true;
                        ConnHandle = 2;
                        break;
                    }
                case 2://第2次及其以后单击，重新开始游戏
                    {

                        CanAgin = true;
                        WhoFisrtDown = false;
                        dropchild = true;
                        CanDown = true;
                        Conqueror = false;
                        ChessClass.Client cc = Program.PublicClientObject;
                        cc.SendMessage("FiDn" + "Me");//发送信息，是否重新下棋
                        if (Agin_if == false)
                        {
                            cc.SendMessage("Agin#" + "OK?");
                            Agin();//设置重新下棋的初始值
                        }
                        else
                        {
                            Agin_if = false;
                            if (panel_Check.Controls.Count > 10)
                            {
                                Agin();
                            }
                        }
                        BBow = BBow + 1;
                        pBox_Sign.Visible = false;//使棋子的标识图片不可见
                        Conqueror = false;
                        break;
                    }
            }
            childSgin = true;
        }

        private void panel_Check_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void panel_Title_MouseDown(object sender, MouseEventArgs e)
        {
            frmClient.CPoint = new Point(-e.X, -e.Y);//记录当前坐标
        }

        private void panel_Title_MouseMove(object sender, MouseEventArgs e)
        {
            frmClient.FrmMove(this, e);//移动窗体
        }

        public ClassMsg temMsg2 = new ClassMsg();
        private void pictureBox_Close_Click(object sender, EventArgs e)
        {
            ChessClass.Client cc = Program.PublicClientObject;
            cc.CloseConnection();
            if (Out == true)
            {
                listener.Close();
            }
            ConnHandle = 0;
            if (GIP.Trim().Length > 4)
            {
                temMsg2.msgCommand = MsgCommand.ExitJoin;
                udpSocket1.Send(IPAddress.Parse(GIP), 11001, new ClassSerializers().SerializeBinary(temMsg2).ToArray());
                MessageBox.Show("是否关闭窗体");
                
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        //点击棋盘
        public void asd(object sender, EventArgs e)
        {
            if (dropchild == false)//如果当前用户已下完棋
                return;
            if ((Mouse_X <= 30.0 / 2.0) || (Mouse_Y <= 30.0 / 2.0))//如果落子点超出落子范围
                return;
            if (CanDown == true)//如果已开始下棋
            {
                if (Conqueror == true)//如果本棋局已有赢家
                {
                    if (CKind == 0)//黑棋赢
                        Bwin();
                    if (CKind == 1)//白棋赢
                        Wwin();
                    return;
                }
                if (WhoFisrtDown == true)//对方先落子
                {
                    int Column = Convert.ToInt32(Math.Round((Mouse_X - 30) / 35));//获取棋子在棋盘中的列数
                    int Row = Convert.ToInt32(Math.Round((Mouse_Y - 30) / 35));//获取棋子在棋盘中的行数
                    int bw = 0;

                    if (note[Convert.ToInt32(Row), Convert.ToInt32(Column)] >= 0)//如果当前已有棋子
                        return;
                    PictureBox pictureBoxTem = new PictureBox();//动态创建一个图片控件
                    pictureBoxTem.Parent = panel_Check;//设置其父容器
                    pictureBoxTem.Location = new Point(Column * 35 + 9, Row * 35 + 9);//设置图片控件的位置
                    //设置图片控件的名称
                    pictureBoxTem.Name = "pictureBox" + Row.ToString() + "*" + Column.ToString();
                    //设置标识图片的位置（用于标识最后下的棋子）
                    pBox_Sign.Location = new Point(Column * 35 + 20, Row * 35 + 20);

                    pictureBoxTem.Size = new Size(30, 30);//设置棋子的大小
                    pictureBoxTem.SizeMode = PictureBoxSizeMode.StretchImage;//设置图像的样式
                    ChessClass.Client cc = Program.PublicClientObject;
                    BBow++;
                    pBox_Sign.Visible = true;//使标识图片可见
                    pBox_Sign.BringToFront();//如果当前的棋子是黑棋
                    if (BBow % 2 == 1)
                    {
                        label_Genre.Text = "黑棋";
                        label_Genre.Tag = 0;
                        pictureBoxTem.Image = imageList1.Images[0];//显示当前棋子为黑棋
                        bw = 0;//标识当前用户为黑棋
                        note[Row, Column] = 0;//记录当前棋子的落子位置
                        //将当前下的棋子信息发送给对方玩家
                        cc.SendMessage("Down#" + Row.ToString() + "*" + Column.ToString() + "|" + "0" + "@" + BBow.ToString());
                    }
                    else
                    {
                        label_Genre.Text = "白棋";
                        label_Genre.Tag = 1;
                        pictureBoxTem.Image = imageList1.Images[1];//显示当前棋子为白棋
                        bw = 1;//标识当前用户为白棋
                        note[Row, Column] = 1;//记录当前棋子的落子位置
                        //将当前下的棋子信息发送给对方玩家
                        cc.SendMessage("Down#" + Row.ToString() + "*" + Column.ToString() + "|" + "1" + "@" + BBow.ToString());
                    }
                    //根据棋子类型显示相应的图片
                    if (childSgin == true)
                    {
                        if (BBow % 2 == 1)
                        {
                            if (label_Left.Text.Trim() == Publec_Class.UserName)
                            {
                                pictureBox_Q_Left.Image = null;
                                pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");
                            }
                            else
                            {
                                pictureBox_Q_Right.Image = null;
                                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\白棋.png");
                            }
                        }
                        else
                        {
                            if (label_Left.Text.Trim() == Publec_Class.UserName)
                            {
                                pictureBox_Q_Left.Image = null;
                                pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\白棋.png");
                            }
                            else
                            {
                                pictureBox_Q_Right.Image = null;
                                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");
                            }
                        }
                        childSgin = false;
                    }
                    note[Row, Column] = bw;
                    if_UPdata = 0;
                    Arithmetic(bw, Row, Column);//计算当前下的棋子是否赢
                }
                else
                {
                    MessageBox.Show("对方还没有落子！");
                    dropchild = true;
                    return;
                }
            }
            else
            {
                MessageBox.Show("本盘棋局没有开始无法下棋！");
                dropchild = true;
                return;
            }
            dropchild = false;
        }

        //添加对方下的棋
        public void AddChess(object sender, ChessClass.AddChessEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(
                    new EventHandler<ChessClass.AddChessEventArgs>(this.AddChess),
                    new object[] { sender, e });
            }
            else
            {

                string Cplace = e.Number;//获取棋子的名称
                string Ccolumn = Cplace.Substring(0, Cplace.IndexOf("*"));//获取棋子所在列数
                //获取棋子所在行数
                string Crow = Cplace.Substring(Cplace.IndexOf("*") + 1, Cplace.Length - (Cplace.IndexOf("*") + 1));
                PictureBox pictureBoxTem = new PictureBox();//创建一个PictureBox控件
                pictureBoxTem.Parent = panel_Check;//设置其父容器
                //计算棋子的位置
                pictureBoxTem.Location = new Point(Convert.ToInt32(Crow) * 35 + 9, Convert.ToInt32(Ccolumn) * 35 + 9);
                pictureBoxTem.Name = "pictureBox" + e.Number;//设置棋子的名称
                pictureBoxTem.Size = new Size(30, 30);//设置棋子的大小
                pictureBoxTem.SizeMode = PictureBoxSizeMode.StretchImage;
                //记录棋子的类型及位置
                note[Convert.ToInt32(Ccolumn), Convert.ToInt32(Crow)] = Convert.ToInt32(e.Im);
                int num = Int32.Parse(e.Im);//记录棋子的类型
                BBow = Int32.Parse(e.Bow);
                pictureBoxTem.Image = imageList1.Images[num];//添加棋子图片
                pBox_Sign.Visible = true;
                //设置标识图片的位置
                pBox_Sign.Location = new Point(Convert.ToInt32(Crow) * 35 + 20, Convert.ToInt32(Ccolumn) * 35 + 20);
                pBox_Sign.BringToFront();
                if_UPdata = 1;
                Arithmetic(num, Convert.ToInt32(Ccolumn), Convert.ToInt32(Crow));//对棋子进行计算

                if_UPdata = 0;
                //根据棋子类型显示相应的图片
                if (ChildSgin == true)
                {
                    if (BBow % 2 == 1)
                    {
                        if (label_Left.Text.Trim() == Publec_Class.UserName)
                        {
                            pictureBox_Q_Left.Image = null;
                            pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");
                        }
                        else
                        {
                            pictureBox_Q_Right.Image = null;
                            pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\白棋.png");
                        }
                    }
                    else
                    {
                        if (label_Left.Text.Trim() == Publec_Class.UserName)
                        {
                            pictureBox_Q_Left.Image = null;
                            pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\白棋.png");
                        }
                        else
                        {
                            pictureBox_Q_Right.Image = null;
                            pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");
                        }
                    }
                    ChildSgin = false;
                }
            }
            WhoFisrtDown = true;
            dropchild = true;
        }
        //判断黑棋是否赢
        public void Bwin()
        {
            MessageBox.Show("黑子赢了!!请重新开始游戏！");

            ClassMsg temMsg = new ClassMsg();
            if (label_Genre.Text == "黑棋")
            {
                for (int i = 0; i < listView_Battle.Items.Count; i++)
                {
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Publec_Class.UserName.Trim())
                    {
                        Publec_Class.Fraction = Publec_Class.Fraction + 10;
                        listView_Battle.Items[i].SubItems[2].Text = Publec_Class.Fraction.ToString();
                        label_F.Text = Publec_Class.Fraction.ToString();
                    }
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Gem_N)
                    {
                        Gem_F = Gem_F - 10;
                        listView_Battle.Items[i].SubItems[2].Text = Gem_F.ToString();
                    }
                }

            }
            else
            {
                for (int i = 0; i < listView_Battle.Items.Count; i++)
                {
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Publec_Class.UserName.Trim())
                    {
                        Publec_Class.Fraction = Publec_Class.Fraction - 10;
                        listView_Battle.Items[i].SubItems[2].Text = Publec_Class.Fraction.ToString();
                        label_F.Text = Publec_Class.Fraction.ToString();
                    }
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Gem_N)
                    {
                        Gem_F = Gem_F + 10;
                        listView_Battle.Items[i].SubItems[2].Text = Gem_F.ToString();
                    }
                }
            }
            if (if_UPdata == 0)
            {
                temMsg.RIP = Publec_Class.ClientIP;//记录用户的IP地址
                temMsg.RPort = Publec_Class.ServerPort;//记录端口号
                temMsg.Fraction = Publec_Class.Fraction;//当前分数
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.UPDataFract;
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());

                temMsg.RIP = GIP;//记录用户的IP地址
                temMsg.RPort = GPort;//记录端口号
                temMsg.Fraction = Gem_F;//当前分数
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.UPDataFract;
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());
            }
            BwinCount++;
            CanAgins = false;
            Conqueror = true;
            CKind = 0;
            CanAgin = false;
            WhoFisrtDown = false;
            dropchild = true;
            this.pboxStart.Image = null;
            this.pboxStart.Enabled = true;
            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮.png");
        }
        //判断白棋是否赢
        public void Wwin()
        {
            MessageBox.Show("白子赢了!!请重新开始游戏！");

            ClassMsg temMsg = new ClassMsg();
            if (label_Genre.Text == "白棋")
            {
                for (int i = 0; i < listView_Battle.Items.Count; i++)
                {
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Publec_Class.UserName.Trim())
                    { 
                        Publec_Class.Fraction=Publec_Class.Fraction+10;
                        listView_Battle.Items[i].SubItems[2].Text = Publec_Class.Fraction.ToString();
                        label_F.Text = Publec_Class.Fraction.ToString();
                    }
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Gem_N)
                    {
                        Gem_F = Gem_F - 10;
                        listView_Battle.Items[i].SubItems[2].Text = Gem_F.ToString();
                    }
                }

            }
            else
            {
                for (int i = 0; i < listView_Battle.Items.Count; i++)
                {
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Publec_Class.UserName.Trim())
                    {
                        Publec_Class.Fraction = Publec_Class.Fraction - 10;
                        listView_Battle.Items[i].SubItems[2].Text = Publec_Class.Fraction.ToString();
                        label_F.Text = Publec_Class.Fraction.ToString();
                    }
                    if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Gem_N)
                    {
                        Gem_F = Gem_F + 10;
                        listView_Battle.Items[i].SubItems[2].Text = Gem_F.ToString();
                    }
                }
            }

            if (if_UPdata == 0)
            {
                temMsg.RIP = Publec_Class.ClientIP;//记录用户的IP地址
                temMsg.RPort = Publec_Class.ServerPort;//记录端口号
                temMsg.Fraction = Publec_Class.Fraction;//当前分数
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.UPDataFract;
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());

                temMsg.RIP = GIP;//记录用户的IP地址
                temMsg.RPort = GPort;//记录端口号
                temMsg.Fraction = Gem_F;//当前分数
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.UPDataFract;
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());
            }
            WwinCount++;
            CanAgins = false;
            Conqueror = true;
            CKind = 1;
            CanAgin = false;
            WhoFisrtDown = false;
            dropchild = true;
            this.pboxStart.Image = null;
            this.pboxStart.Enabled = true;
            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮.png");
        }

        //启动服务
        public void StartServer()
        {

            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 11003);
            try
            {
                listener.Bind(ep);
            }
            catch
            {
                if (listener != null)
                    listener.Close();
                if (mainThread != null)
                {
                    Thread.Sleep(30);//睡眠主线程
                    mainThread.Abort();//关闭子线程
                }
                ThreadStart ts = new ThreadStart(this.StartServer);
                mainThread = new Thread(ts);
                mainThread.Name = "GOB_Chess";
                LineName = mainThread.Name;
                mainThread.Start();
            }
            listener.Listen(100);
            Out = true;
            while (true)
            {
                //接收连接请求
                Socket server;
                try
                {
                    server = listener.Accept();
                }
                catch
                {
                    break;
                }

                //创建客户端对象
                ChessClass.ClientObject client = new ChessClass.ClientObject();
                client.ClientSocket = server;
                //接收对方下的棋
                client.OnAddChess += new EventHandler<ChessClass.AddChessEventArgs>(AddChess);
                client.OnAddMessage += new EventHandler<ChessClass.AddMessageEventArgs>(AddMessage);
                client.OnAginMessage += new EventHandler<ChessClass.AginMessageEventArgs>(AginMessage);
                client.OnFiDnMessage += new EventHandler<ChessClass.FiDnMessageEventArgs>(FiDnMessage);
                client.receiveMessage();
            }
        }

        //添加对方发来的信息
        public void AddMessage(object sender, ChessClass.AddMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(
                    new EventHandler<ClientGobang.ChessClass.AddMessageEventArgs>(AddMessage),
                    new object[] { sender, e });
            }
            else
            {

            }

        }
        public void First()
        {
            if (WhoFisrtDown == true)
            {
                MessageBox.Show("游戏开始，请先下。");
            }
            else
            {
                MessageBox.Show("游戏开始，请后下。");
            }
        }
        public void FiDnMessage(object sender, ChessClass.FiDnMessageEventArgs e)//谁先下棋的判断
        {
            if (this.InvokeRequired)
            {
                this.Invoke(
                    new EventHandler<ChessClass.FiDnMessageEventArgs>(FiDnMessage),
                    new object[] { sender, e });
            }
            else
            {
                WhoFisrtDown = true;
            }
            //First();

        }

        //判断重新开始的条件
        public void Agin()
        {
            if (CanAgins == false && CanAgin == true)//如果棋局已有赢家
            {
                for (int i = 0; i < panel_Check.Controls.Count; i++)//遍历棋盘上的所有棋子
                {
                    if (panel_Check.Controls[i] is PictureBox)//如果找到棋子
                    {
                        panel_Check.Controls.RemoveAt(i);//删除当前棋子
                        i = -1;
                    }
                }
                //初始化记录棋子位置的多维数组
                for (int i = 0; i < 15; i++)
                    for (int j = 0; j < 15; j++)
                        note[i, j] = -1;
            }
        }
        //重新开始
        public void AginMessage(object sender, ChessClass.AginMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(
                    new EventHandler<ChessClass.AginMessageEventArgs>(AginMessage),
                    new object[] { sender, e });
            }
            else
            {
                if (e.Agin == "OK?")
                {
                    CanAgins = false;
                    Conqueror = false;
                    CanAgin = true;
                    CanDown = true;
                    Agin_if = true;

                }
                Agin();
            }

        }

        public void Arithmetic(int n, int Arow, int Acolumn)
        {
            int BCount = 1;
            CKind = -1;
            //横向查找
            bool Lbol = true;
            bool Rbol = true;
            int jlsf = 0;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Acolumn + i) > 14)                     //如果棋子超出最大列数
                    Rbol = false;
                if ((Acolumn - i) < 0)                      //如果棋子超出最小列数
                    Lbol = false;
                if (Rbol == true)
                {
                    if (note[Arow, Acolumn + i] == n)           //如果横向向右有相同的棋子
                        ++BCount;
                    else
                        Rbol = false;
                }
                if (Lbol == true)
                {
                    if (note[Arow, Acolumn - i] == n)           //如果横向向左有相同的棋子
                        ++BCount;
                    else
                        Lbol = false;
                }
                if (BCount >= 5)                            //如果同类型的棋子数大于等于5
                {
                    if (n == 0)                         //黑棋方赢
                        Bwin();
                    if (n == 1)                         //白棋方赢
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
            //纵向查找
            bool Ubol = true;
            bool Dbol = true;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Arow + i) > 14)                        //如果超出棋盘的最大行数
                    Dbol = false;
                if ((Arow - i) < 0)                         //如果超出棋盘的最小行数
                    Ubol = false;
                if (Dbol == true)
                {
                    if (note[Arow + i, Acolumn] == n)           //如果向上有同类型的棋子
                        ++BCount;
                    else
                        Dbol = false;
                }
                if (Ubol == true)
                {
                    if (note[Arow - i, Acolumn] == n)           //如果向下有同类型的棋子
                        ++BCount;
                    else
                        Ubol = false;
                }
                if (BCount >= 5)                            //如果同类型的棋子大于等于5
                {
                    if (n == 0)                         //黑棋赢
                        Bwin();
                    if (n == 1)                         //白棋赢
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
            //对角线查找
            bool LUbol = true;
            bool RDbol = true;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Arow - i) < 0 || (Acolumn - i < 0))            //如果超出左面的斜线
                    LUbol = false;
                if ((Arow + i) > 14 || (Acolumn + i > 14))      //如果超出右面的斜线
                    RDbol = false;
                if (LUbol == true)
                {
                    if (note[Arow - i, Acolumn - i] == n)       //如果左上斜线上有相同类型的棋子
                        ++BCount;
                    else
                        LUbol = false;
                }
                if (RDbol == true)
                {
                    if (note[Arow + i, Acolumn + i] == n)   //如果右下斜线上有相同类型的棋子
                        ++BCount;
                    else
                        RDbol = false;
                }
                if (BCount >= 5)                        //如果同类型的棋子大于等于5
                {
                    if (n == 0)
                        Bwin();
                    if (n == 1)
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
            //反斜查找
            bool RUbol = true;
            bool LDbol = true;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Arow - i) < 0 || (Acolumn + i > 14))
                    RUbol = false;
                if ((Arow + i) > 14 || (Acolumn - i < 0))
                    LDbol = false;
                if (RUbol == true)
                {
                    if (note[Arow - i, Acolumn + i] == n)       //如果左下斜线上有相同类型的棋子
                        ++BCount;
                    else
                        RUbol = false;
                }
                if (LDbol == true)
                {
                    if (note[Arow + i, Acolumn - i] == n)   //如果右上斜线上有相同类型的棋子
                        ++BCount;
                    else
                        LDbol = false;
                }
                if (BCount >= 5)                        //如果同类型的棋子大于等于5
                {
                    if (n == 0)
                        Bwin();
                    if (n == 1)
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
        }

        //发送消息
        public void sendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message + "\r\n\r\n");
            listener.Send(buffer);
        }

        private void Frm_Chessboard_Load(object sender, EventArgs e)
        {
            udpSocket1.LocalHost = Publec_Class.ClientIP;
            udpSocket1.LocalPort = 11001;
            udpSocket1.Active = true;
        }

        private void udpSocket1_DataArrival(byte[] Data, IPAddress Ip, int Port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival);
            this.BeginInvoke(outdelegate, new object[] { Data, Ip, Port }); //设置托管
        }

        private delegate void DataArrivaldelegate(byte[] Data, IPAddress Ip, int Port);


        private void DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port) //当有数据到达后的处理进程
        {
            try
            {
                //将接收的消息转换成自定义集合ClassMsg
                ClassMsg msg = (ClassMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(Data));
                switch (msg.msgCommand)
                {
                    case MsgCommand.BeginJoin://进入大厅获取在线用户列表
                        {

                            users = (ClassUsers)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));//获取所有用户信息
                            ClassUserInfo UserItem = new ClassUserInfo();
                            UserItem = users[0];
                            GIP = UserItem.UserIP;//记录用户的IP地址
                            GPort = UserItem.UserPort;//记录端口号
                            Gem_N = UserItem.UserName;//记录用户名称
                            Gem_F = Convert.ToInt32(UserItem.Fraction);//当前分数
                            Gem_C = Convert.ToInt32(UserItem.Caput);//头像
                            Gem_S = Convert.ToInt32(UserItem.Sex);//性别

                            ThreadStart ts = new ThreadStart(this.StartServer);
                            mainThread = new Thread(ts);
                            mainThread.Name = "GOB_Chess";
                            LineName = mainThread.Name;
                            mainThread.Start();
                            StartListen = true;
                            ConnHandle = 1;

                            label_Left.Text = Gem_N;//显示对方玩家的名称
                            frmClient.Data_List(listView_Battle, Gem_N, Gem_F.ToString(), Gem_C.ToString());
                           
                            pictureBox_Left.Image = null;
                            if (Gem_S == 0) //如果对方玩家是男性
                            {
                                pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\男人.png");
                            }
                            else
                            {
                                pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\女人.png");
                            }
                            //显示对方玩家的棋子类型
                            pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\黑棋.png");

                            this.pboxStart.Image = null;
                            this.pboxStart.Enabled = true;
                            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮.png");
                            break;
                        }
                    case MsgCommand.OddLittle:
                        {
                            rtxtChat.ReadOnly = true;//设为只读
                            rtxtChat.ForeColor = Color.SlateGray;//设置字体颜色
                            rtxtChat.AppendText(msg.UserName);//添加发送信息的用户名
                            rtxtChat.AppendText("\r\n");//换行
                            rtxtChat.AppendText(msg.MsgText);//添加发送的信息
                            rtxtChat.AppendText("\r\n");//换行
                            rtxtChat.ScrollToCaret();//将信息添加到控件
                            break;
                        }
                    case MsgCommand.GetGameF:
                        {
                            Gem_N = msg.Fraction.ToString();
                            for (int i = 0; i < listView_Battle.Items.Count; i++)
                            {
                                if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Gem_N.Trim())
                                {
                                    listView_Battle.Items[i].SubItems[2].Text = Gem_N;
                                }
                            }
                            break;
                        }
                    case MsgCommand.ExitJoin:
                        {
                            if (label_Left.Text.Trim() == Publec_Class.UserName)
                            {
                                pictureBox_Q_Right.Image = null;
                                pictureBox_Right.Image = null;
                                label_Right.Text = "";
                            }
                            else
                            {
                                pictureBox_Q_Left.Image = null;
                                pictureBox_Left.Image = null;
                                label_Left.Text = "";
                            }
                            GIP = "";
                            int ListCount = listView_Battle.Items.Count;
                            for (int i = 0; i < ListCount; i++)
                            {
                                if (listView_Battle.Items[i].SubItems[1].Text.Trim() == Gem_N.Trim())
                                {
                                    listView_Battle.Items[i].Remove();
                                    i = i - 1;
                                    ListCount = ListCount - 1;
                                }
                            }
                            CanAgins = false;
                            CanAgin = true;
                            Agin();

                            this.pboxStart.Image = null;
                            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\开始按钮灰.png");
                            dropchild = true;

                            if (listener != null)
                                listener.Close();
                            if (mainThread != null)
                            {
                                Thread.Sleep(30);       //睡眠主线程
                                mainThread.Abort();         //关闭子线程
                            }

                            break;
                        }
                }
            }
            catch { }
        }

        private void Frm_Chessboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listener != null)
                listener.Close();
            if (mainThread != null)
            {
                Thread.Sleep(30);       //睡眠主线程
                mainThread.Abort();         //关闭子线程
            }

            if (udpSocket1.Active == true)
            {
                udpSocket1.Active = false;
                udpSocket1.LocalHost = "127.0.0.1";
                udpSocket1.LocalPort = 11000;
            }
        }

        private void pictureBox_Min_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox_Min_MouseEnter(object sender, EventArgs e)
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

        private void pictureBox_Min_MouseLeave(object sender, EventArgs e)
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
        public ClassMsg OddtemMsg = new ClassMsg();
        public string OddText = "";
        //显示聊天信息
        private void pictureBox_Chart_Click(object sender, EventArgs e)
        {
            OddtemMsg.msgCommand = MsgCommand.OddLittle;
            OddtemMsg.UserName = Publec_Class.UserName;
            OddtemMsg.MsgText = comboBox_Hair.Text;
            if (GIP == "")
                return;
            udpSocket1.Send(IPAddress.Parse(GIP), 11001, new ClassSerializers().SerializeBinary(OddtemMsg).ToArray());

            rtxtChat.ReadOnly = true;//设为只读
            rtxtChat.ForeColor = Color.SlateGray;//设置字体颜色
            rtxtChat.AppendText(Publec_Class.UserName);//添加发送信息的用户名
            rtxtChat.AppendText("\r\n");//换行
            rtxtChat.AppendText(comboBox_Hair.Text);//添加发送的信息
            rtxtChat.AppendText("\r\n");//换行
            rtxtChat.ScrollToCaret();//将信息添加到控件

            comboBox_Hair.Text = "";
        }

        //按下回车键，发送聊天消息
        private void comboBox_Hair_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                pictureBox_Chart_Click(sender, e);
            }
        }

    }
}