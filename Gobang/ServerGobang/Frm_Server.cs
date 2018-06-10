using System;
using System.Data.SqlClient;//SqlDataReader对象的命名空间
using System.Drawing;
using System.Windows.Forms;
using GobangClass;//添加公共类
using System.Net;//托管的命名空间

namespace ServerGobang
{
    public partial class Frm_Server : Form
    {
        public Frm_Server()
        {
            InitializeComponent();
        }

        #region  声名变量
        string fellin = "";//设置排名的SQL语句
        #endregion

        private void Tool_Open_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Text == "开始服务")
            {
                ((ToolStripMenuItem)sender).Text = "结束服务";//设置菜单项的文本信息
                udpSocket1.Active = true;//打开监听
            }
            else
            {
                ((ToolStripMenuItem)sender).Text = "开始服务";
                ClassOptionData OptionData = new ClassOptionData();//创建ClassOptionData对象
                //将数据表中的用户状态都设为退出状态
                OptionData.ExSQL("Update tb_Gobang Set State =" + Convert.ToInt32(MsgCommand.Close) + ",Borough = 0 ,RoomMark=0 ,DeskMark='0',SeatMark='0' Where ID >0");
                udpSocket1.Active = false;//关闭监听
                UpdateUser();//刷新列表
            }
        }

        private void udpSocket1_DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival);
            this.BeginInvoke(outdelegate, new object[] { Data, Ip, Port }); //设置托管
        }

        private delegate void DataArrivaldelegate(byte[] Data, System.Net.IPAddress Ip, int Port);

        private void DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port) //当有数据到达后的处理进程
        {
            try
            {
                ClassMsg msg = new ClassSerializers().DeSerializeBinary((new System.IO.MemoryStream(Data))) as ClassMsg;
                switch (msg.sendKind)
                {
                    case SendKind.SendCommand://命令
                        {
                            switch (msg.msgCommand)
                            {
                                case MsgCommand.Registering://注册用户
                                    RegisterUser(msg, Ip, Port);
                                    break;
                                case MsgCommand.Logining://登录用户
                                    UserLogin(msg, Ip, Port, 1);
                                    break;
                                case MsgCommand.SendToOne://发送消息给单用户
                                    SendUserMsg(msg, Ip, Port);
                                    break;
                                case MsgCommand.ComeToHall:
                                    UpdateUser(msg, Ip, Port, msg.msgCommand);//更新用户在线状态
                                    FurbishMsg(msg);
                                    UpdateUserList(msg, Ip, Port);//返回所有进入大厅的用户
                                    break;
                                case MsgCommand.ExitToHall://退出游戏大厅
                                    UpdateUser(msg, Ip, Port, msg.msgCommand);//更新用户在线状态
                                    break;
                                case MsgCommand.BeginToGame:
                                    UpdateUser(msg, Ip, Port, msg.msgCommand);//更新用户在线状态
                                    break;
                                case MsgCommand.ExitToArea:
                                    MessColley(msg, Ip, Port, msg.msgCommand);
                                    break;
                                case MsgCommand.EndToGame:
                                    UpdateUser(msg, Ip, Port, msg.msgCommand);//更新用户在线状态
                                    break;
                                case MsgCommand.BegingRival:
                                    UserGame(msg, Ip, Port, msg.msgCommand);//获取用户玩家的信息
                                    break;
                                case MsgCommand.ComeToSay:
                                    UserText(msg, Ip, Port, msg.msgCommand);
                                    break;
                                case MsgCommand.UPDataFract:
                                    UpdatePlayChess(msg, Ip, Port, msg.msgCommand);//更改下棋后的分数
                                    break;
                                case MsgCommand.GetGameF:
                                    GameF(msg, Ip, Port, msg.msgCommand);//更改下棋后的分数
                                    break;
                            }
                            break;
                        }
                    case SendKind.SendMsg://消息
                        {
                            switch (msg.msgCommand)
                            {
                                case MsgCommand.SendToOne://发送消息给单用户
                                    SendUserMsg(msg, Ip, Port);
                                    break;
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 恢复消息的MsgCommand命令
        /// </summary>
        /// <param name="msg"></param>
        private void FurbishMsg(ClassMsg msg)
        {
            if (msg.msgCommand == MsgCommand.BeginToGameL || msg.msgCommand == MsgCommand.BeginToGameH)
                msg.msgCommand = MsgCommand.BeginToGame;
            if (msg.msgCommand == MsgCommand.EndToGameL || msg.msgCommand == MsgCommand.EndToGameH)
                msg.msgCommand = MsgCommand.EndToGame;
            if (msg.msgCommand == MsgCommand.ComeToHallL || msg.msgCommand == MsgCommand.ComeToHallH)
                msg.msgCommand = MsgCommand.ComeToHall;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        private void RegisterUser(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {
            if (IfRegisterAt(msg, Ip, Port) == false)
            {
                msg = InsertUser(msg, Ip, Port);
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <param name="State"></param>
        private void UserLogin(ClassMsg msg, System.Net.IPAddress Ip, int Port, int State)
        {
            msg = UpdateLoginUser(msg, Ip, Port);
        }

        /// <summary>
        /// 发送消息给单用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        private void SendUserMsg(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {
            ClassOptionData OptionData = new ClassOptionData();
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang Where ID = " + msg.RID);
            DataReader.Read();
            string ip = DataReader.GetString(1);
            int port = DataReader.GetInt32(2);
            udpSocket1.Send(IPAddress.Parse(ip), port, new ClassSerializers().SerializeBinary(msg).ToArray());
            DataReader.Dispose();
        }

        /// <summary>
        /// 判断用户是否注册
        /// </summary>
        /// <param msg="ClassMsg"></param>
        /// <param Ip="System.Net.IPAddress"></param>
        /// <param Port="int"></param>
        /// <returns></returns>
        private bool IfRegisterAt(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {
            bool RegAt = true;
            //RegisterMsg registermsg = (RegisterMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));
            ClassOptionData OptionData = new ClassOptionData();
            MsgCommand Sate = msg.msgCommand;
            String UserName = msg.UserName; //注册用户的名称
            String PassWord = msg.PassWord;//注册用户的密码
            String vIP = Ip.ToString();//注册用户的IP地址
            SqlDataReader DataReader;

            //查找注册用户
            DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang where UserName=" + "'" + UserName + "'");
            if (DataReader.Read())
            {
                RegAt = true;
                msg.msgCommand = MsgCommand.RegisterAt;//存在注册用户
                SendMsgToOne(Ip, Port, msg);//将注册命令返回给注册用户
            }
            else
            {
                DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang where IP=" + "'" + Ip.ToString() + "'");
                if (DataReader.Read())
                {
                    OptionData.ExSQL("Delete tb_Gobang where IP=" + "'" + Ip.ToString() + "'");
                }

                RegAt = false;
                msg.msgCommand = MsgCommand.Registered;//用户注册结束命令

            }
            return RegAt;
        }

        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        private ClassMsg InsertUser(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {
            //RegisterMsg registermsg = (RegisterMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));

            ClassOptionData OptionData = new ClassOptionData();
            MsgCommand Sate = msg.msgCommand;
            String UserName = msg.UserName; //注册用户的名称
            String PassWord = msg.PassWord;//注册用户的密码
            String vIP = Ip.ToString();//注册用户的IP地址
            int CPhot = msg.CPhoto;//注册用户的头象
            int Sex = msg.Sex;//用户的性别
            //向数据表中添加注册信息
            OptionData.ExSQL("insert into tb_Gobang (IP,Port,UserName,PassWord,State,Caput,Sex) values ('" + vIP + "'," +
                Port.ToString() + ",'" + UserName + "','" + PassWord + "'," + Convert.ToString((int)(MsgCommand.Registered)) + "," + CPhot + "," + Sex + ")");
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang");
            UpdateUser();//更新用户列表
            //OptionData.Dispose();
            msg.msgCommand = MsgCommand.Registered;//用户注册结束命令
            SendMsgToOne(Ip, Port, msg);//将注册命令返回给注册用户
            return msg;
        }

        /// <summary>
        /// 更改登录用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        private ClassMsg UpdateLoginUser(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {
            //RegisterMsg registermsg = (RegisterMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));

            ClassOptionData OptionData = new ClassOptionData();//创建并引用ClassOptionData
            MsgCommand msgState = msg.msgCommand;   //获取接收消息的命令
            String UserName = msg.UserName;//登录用户名称
            String PassWord = msg.PassWord;//用户密码
            String vIP = Ip.ToString();//用户IP地址

            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang Where UserName = " + "'" + UserName + "'" + " and PassWord = "
                            + "'" + PassWord + "'");//在数据库中通过用户名和密码进行查找
            if (DataReader.HasRows)
            {
                DataReader.Read();//读取查找到的记录
                string ID = Convert.ToString(DataReader.GetInt32(0));//获取第一条记录中的ID字段值
                msg.Fraction = DataReader.GetInt32(5);//获取当前用户的分数
                msg.Sex = DataReader.GetInt32(13);//获取当前用户性别
                //修改当前记录的标识为上线状态
                OptionData.ExSQL("Update tb_Gobang Set State = " + Convert.ToString((int)(MsgCommand.Logined)) + ",IP = " + "'" + vIP + "',Port = " + "'" + Port.ToString() + "'" + " Where ID = " + ID);
                msg.msgCommand = MsgCommand.Logined;//设置为上线命令
                SendMsgToOne(Ip, Port, msg);//将消息返回给发送用户/////--------------------------------////
                UpdateUser();//更新用户列表
            }
            return msg;
        }

        /// <summary>
        /// 更改游戏中的用户状态
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <param Nsign="MsgCommand"></param>
        /// <returns></returns>
        private ClassMsg UpdateUser(ClassMsg msg, System.Net.IPAddress Ip, int Port, MsgCommand Nsign)
        {
            ClassOptionData OptionData = new ClassOptionData();//创建并引用ClassOptionData
            MsgCommand msgState = msg.msgCommand;   //获取接收消息的命令
            String vIP = Ip.ToString();//用户IP地址
            String Uname = "";
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang Where IP = " + "'" + vIP + "'");//在数据库中通过IP进行查找
            if (DataReader.Read())//读取查找到的记录
            {
                Uname = DataReader.GetString(3);
                MsgCommand msgCsign = Nsign;
                string ID = Convert.ToString(DataReader.GetInt32(0));//获取第一条记录中的ID字段值
                //修改当前记录的标识为上线状态
                if (msg.msgCommand == MsgCommand.ComeToHall || msg.msgCommand == MsgCommand.BeginToGame)
                {
                    if (msg.msgCommand == MsgCommand.ComeToHall)
                    {
                        msgCsign = MsgCommand.ComeToHallL;
                        OptionData.ExSQL("Update tb_Gobang Set State = " + Convert.ToInt32(Nsign) + ",IP = " + "'" + vIP + "',Port = " + "'" + Port.ToString() + "',Borough = " + Convert.ToInt32(msg.AreaMark) + " ,RoomMark=" + Convert.ToInt32(msg.RoomMark) + " Where ID = " + ID);
                    }
                    if (msg.msgCommand == MsgCommand.BeginToGame)
                    {
                        msgCsign = MsgCommand.BeginToGameL;
                        OptionData.ExSQL("Update tb_Gobang Set State = " + Convert.ToInt32(Nsign) + ",IP = " + "'" + vIP + "',Port = " + "'" + Port.ToString() + "',Borough = " + Convert.ToInt32(msg.AreaMark) + " ,RoomMark=" + Convert.ToInt32(msg.RoomMark) + " ,DeskMark='" + msg.DeskMark + "',SeatMark='" + msg.SeatMark + "',UserCaption='" + msg.UserCaption + "' Where ID = " + ID);
                    }
                }
                else
                {
                    if (msg.msgCommand == MsgCommand.ExitToHall || msg.msgCommand== MsgCommand.EndToGame)
                    {
                        if (msg.msgCommand == MsgCommand.ExitToHall)
                            OptionData.ExSQL("Update tb_Gobang Set State = " + Convert.ToInt32(Nsign) + ",IP = " + "'" + vIP + "',Port = " + "'" + Port.ToString() + "',Borough = 0 ,RoomMark=0 ,DeskMark='0',SeatMark='0'" + " Where ID = " + ID);
                        if (msg.msgCommand == MsgCommand.EndToGame)
                        {
                            msgCsign = MsgCommand.EndToGameL;
                            OptionData.ExSQL("Update tb_Gobang Set State = " + Convert.ToInt32(MsgCommand.BeginToGame) + ",IP = " + "'" + vIP + "',Port = " + "'" + Port.ToString() + "',DeskMark='0',SeatMark='0'" + " Where ID = " + ID);
                        }
                    }
                    else
                    {
                        OptionData.ExSQL("Update tb_Gobang Set State = " + Convert.ToInt32(Nsign) + ",IP = " + "'" + vIP + "',Port = " + "'" + Port.ToString() + "'" + " Where ID = " + ID);
                    }
                }
                if (msg.msgCommand == MsgCommand.ExitToHall)//如果当前用户退出大厅
                {
                    msg.msgCommand = MsgCommand.Close;
                    SendMsgToOne(Ip, Port, msg);//将消息返回给发送用户
                }
                else
                {
                    msg.msgCommand = msgCsign;
                    msg.sendKind = SendKind.SendCommand;
                    SendMsgToOne(Ip, Port, msg);//将消息返回给发送用户
                }
                DataReader.Dispose();
                FurbishMsg(msg);
                //应有一个群发的方法
                MessColley(msg, Ip, Port, Nsign);

            }
            return msg;
        }

        /// <summary>
        /// 聊天记录群发
        /// </summary>
        private void UserText(ClassMsg msg, System.Net.IPAddress Ip, int Port, MsgCommand Nsign)
        {
            MessColley(msg, Ip, Port, Nsign);
        }

        /// <summary>
        /// 更改下棋后的分数
        /// </summary>
        private void UpdatePlayChess(ClassMsg msg, System.Net.IPAddress Ip, int Port, MsgCommand Nsign)
        {
            ClassOptionData OptionData = new ClassOptionData();
            OptionData.ExSQL("Update tb_Gobang Set Fraction =" + msg.Fraction + " Where IP ='" + msg.RIP+"'");
            UpdateUser();//更新用户列表

        }


        /// <summary>
        /// 将信息发送给所有用户
        /// </summary>
        private void MessColley(ClassMsg msg, System.Net.IPAddress Ip, int Port, MsgCommand Nsign)
        {
            MsgCommand MsgSign = new MsgCommand();
            MsgSign = Nsign;
            if (Nsign == MsgCommand.ComeToHall)
                MsgSign = MsgCommand.ComeToHallH;

            if (Nsign == MsgCommand.BeginToGame)
                MsgSign = MsgCommand.BeginToGameH;

            if (Nsign == MsgCommand.EndToGame)
                MsgSign = MsgCommand.EndToGameH;
            ClassOptionData OptionData = new ClassOptionData();//创建并引用ClassOptionData
            bool DRbool = false;
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang where (State>=" + Convert.ToInt32(MsgCommand.Logining) + " and State<=" + Convert.ToInt32(MsgCommand.ExitToArea) + ")");// and (IP<>'" + Ip.ToString() + "')");
            if (DataReader.HasRows)
            {
                DRbool = DataReader.Read();
                while (DRbool)
                {
                    msg.msgCommand = MsgSign;
                    SendMsgToOne(IPAddress.Parse(DataReader.GetString(1)), Convert.ToInt32(DataReader.GetString(2)), msg);//群发给所有在线用户
                    DRbool = DataReader.Read();
                }
                DataReader.Dispose();
            }
            UpdateUser();//更新用户列表
        }

        /// <summary>
        /// 获取玩家的信息
        /// </summary>
        private void UserGame(ClassMsg msg, System.Net.IPAddress Ip, int Port, MsgCommand Nsign)
        {
            ClassUsers Users = new ClassUsers();
            ClassOptionData OptionData = new ClassOptionData();
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang where (IP=" + msg.SIP.Trim() + ")");//查询进入大厅，或开始游戏的对象
            DataReader.Read();//读取玩家信息

            ClassUserInfo UserItem = new ClassUserInfo();//创建并引用ClassUserInfo类
            UserItem.UserIP = DataReader.GetString(1);//记录用户的IP地址
            UserItem.UserPort = DataReader.GetString(2);//记录端口号
            UserItem.UserName = DataReader.GetString(3);//记录用户名称
            UserItem.Fraction = Convert.ToString(DataReader.GetInt32(5));//当前分数
            UserItem.Caput = Convert.ToString(DataReader.GetInt32(12));//头像
            UserItem.Sex = Convert.ToString(DataReader.GetInt32(13));//头像
            Users.add(UserItem);//将单用户信息添加到用户列表中

            msg.Data = new ClassSerializers().SerializeBinary(Users).ToArray();//将用户列表写入到二进制流中
            msg.msgCommand = MsgCommand.EndRival;
            DataReader.Dispose();
            udpSocket1.Send(Ip, Port, new ClassSerializers().SerializeBinary(msg).ToArray());
        }

        /// <summary>
        /// 获取玩家的分数
        /// </summary>
        private void GameF(ClassMsg msg, System.Net.IPAddress Ip, int Port, MsgCommand Nsign)
        {
            ClassOptionData OptionData = new ClassOptionData();
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select Fraction From tb_Gobang where (IP=" + msg.RIP.Trim() + ")");//查询进入大厅，或开始游戏的对象
            DataReader.Read();//读取玩家信息

            msg.Fraction = DataReader.GetInt32(0);//记录用户的IP地址
            udpSocket1.Send(Ip, Port, new ClassSerializers().SerializeBinary(msg).ToArray());
        }

        /// <summary>
        /// 更新用户列表
        /// </summary>
        private void UpdateUser()
        {
            ClassOptionData OptionData = new ClassOptionData();
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang" + fellin);
            LV_SysUser.Items.Clear();
            bool DRbool = false;
            if (DataReader.HasRows)
            {
                DRbool = DataReader.Read();
                while (DRbool)
                {
                    ListViewItem listItem = new ListViewItem();
                    listItem.Text = Convert.ToString(DataReader.GetInt32(0));
                    listItem.SubItems.Add(DataReader.GetString(1));
                    listItem.SubItems.Add(DataReader.GetString(2));
                    listItem.SubItems.Add(DataReader.GetString(3));
                    listItem.SubItems.Add(Convert.ToString(DataReader.GetInt32(5)));
                    listItem.SubItems.Add(Convert.ToString(DataReader.GetInt32(6)));
                    LV_SysUser.Items.Add(listItem);
                    DRbool = DataReader.Read();
                }
                DataReader.Dispose();
            }
        }

        /// <summary>
        /// 向所有在线用户发送进入大厅及游戏的用户信息
        /// </summary>
        private void UpdateUserList(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {

            ClassUsers Users = new ClassUsers();
            ClassOptionData OptionData = new ClassOptionData();
            SqlDataReader DataReader = OptionData.ExSQLReDr("Select * From tb_Gobang where (State>=" + Convert.ToInt32(MsgCommand.BeginToGame) + " and State<=" + Convert.ToInt32(MsgCommand.ExitToArea) + ")");//查询进入大厅，或开始游戏的对象

            while (DataReader.Read())//遍历所有用户
            {
                ClassUserInfo UserItem = new ClassUserInfo();//创建并引用ClassUserInfo类
                UserItem.UserID = Convert.ToString(DataReader.GetInt32(0));//记录用户用编号
                UserItem.UserIP = DataReader.GetString(1);//记录用户的IP地址
                UserItem.UserPort = DataReader.GetString(2);//记录端口号
                UserItem.UserName = DataReader.GetString(3);//记录用户名称
                UserItem.Fraction = Convert.ToString(DataReader.GetInt32(5));//当前分数
                UserItem.State = Convert.ToString(DataReader.GetInt32(6));//记录当前状态
                UserItem.Borough = Convert.ToString(DataReader.GetInt32(7));//区号
                UserItem.RoomMark = Convert.ToString(DataReader.GetInt32(8));//房间号
                if (msg.msgCommand == MsgCommand.BeginToGame || DataReader.GetInt32(6) == Convert.ToInt32(MsgCommand.BeginToGame))
                {

                    UserItem.DeskMark = DataReader.GetString(9);//桌号
                    UserItem.SeatMart = DataReader.GetString(10);//坐位号
                    UserItem.UserCaption = DataReader.GetString(11);//用户名
                }
                UserItem.Caput = Convert.ToString(DataReader.GetInt32(12));//头像
                UserItem.Sex = Convert.ToString(DataReader.GetInt32(13));//头像
                Users.add(UserItem);//将单用户信息添加到用户列表中
            }

            msg.Data = new ClassSerializers().SerializeBinary(Users).ToArray();//将用户列表写入到二进制流中
            msg.msgCommand = MsgCommand.UserList;
            DataReader.Dispose();
            msg.msgCommand = MsgCommand.UserList;
            //udpSocket1.Send(Ip, Port, new ClassSerializers().SerializeBinary(msg).ToArray());
            MessColley(msg, Ip, Port, msg.msgCommand);//群发

        }

        /// <summary>
        /// 将消息返回给发送用户
        /// </summary>
        private void SendMsgToOne(System.Net.IPAddress ip, int port, ClassMsg msg)//发送消息给一个用户
        {
            try
            {
                udpSocket1.Send(ip, port, new ClassSerializers().SerializeBinary(msg).ToArray());
            }
            catch { }
        }
        
        private void Frm_Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpSocket1.Active = false;//将UDP协议设置为不可用
            udpSocket1.Dispose();//释放资源
            ClassOptionData OptionData = new ClassOptionData();//创建ClassOptionData对象
            //设置用户在线状态
            OptionData.ExSQL("Update tb_Gobang Set State =" + Convert.ToInt32(MsgCommand.Close) + ",Borough = 0 ,RoomMark=0 ,DeskMark='0',SeatMark='0' Where ID >0");
        }

        private void Frm_Server_Shown(object sender, EventArgs e)
        {
            Tool_BS.Text = "  从大到小";
            Tool_SB.Text = "  从小到大";
            UpdateUser();//加载用户列表
        }

        private void Tool_BS_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)Tool_BS).Text == "√从大到小")
            {
                ((ToolStripMenuItem)Tool_BS).Text = "  从大到小";
                fellin = "";
            }
            else
            {
                ((ToolStripMenuItem)Tool_BS).Text = "√从大到小";
                ((ToolStripMenuItem)Tool_SB).Text = "  从小到大";
                fellin = " ORDER BY Fraction DESC,ID ";//设置排序的条件
            }
            UpdateUser();//加载用户列表
        }

        private void Tool_SB_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)Tool_SB).Text == "√从小到大")
            {
                ((ToolStripMenuItem)Tool_SB).Text = "  从小到大";
                fellin = "";
            }
            else
            {
                ((ToolStripMenuItem)Tool_SB).Text = "√从小到大";
                ((ToolStripMenuItem)Tool_BS).Text = "  从大到小";
                fellin = " ORDER BY Fraction ASC,ID ";//设置排序的条件
            }
            UpdateUser();//加载用户列表
        }

        private void Tool_Refurbish_Click(object sender, EventArgs e)
        {
            UpdateUser();//刷新用户列表
        }

        private void Tool_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}