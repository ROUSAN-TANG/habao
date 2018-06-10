using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;//添加命名空间
using GobangClass;
using System.IO;//添加的命名空间:Directory

namespace ClientGobang
{
    class ClientClass
    {
        #region  定义全局变量
        Publec_Class PublecClass = new Publec_Class();
        public static string ImaDir = Application.StartupPath;//存储图片的路径
        public Point CPoint;  //添加命名空间using System.Drawing;
        public static string GameIP = "";//对方玩家的IP
        public static string GamePort = "";//对方玩家的端口号
        public static string GameName = "";//对方玩家的名称
        public static string GameFraction = "";//对方玩家的分数
        public static string GameCaput = "";//对方玩家的头像
        public static string GameSex = "";//对方玩家的性别
        public static bool BeginGame = false;
        #endregion

        #region  隐藏Panel控件
        /// <summary>
        /// 隐藏Panel控件
        /// </summary>
        public void HidePanel(Panel PanelPlock)
        {
            if (PanelPlock.Visible)
                PanelPlock.Visible = false;
            else
                PanelPlock.Visible = true;
        }
        #endregion

        #region  判断端口号是否正确
        /// <summary>
        /// 判断端口号是否正确
        /// </summary>
        public void TextValue(TextBox Textb)
        {
            if (Textb.Text == "11000")
                Textb.Text = "1100";
        }
        #endregion

        #region  返回上一级目录
        /// <summary>
        /// 返回上一级目录
        /// </summary>
        /// <param dir="string">目录</param>
        /// <returns>返回String对象</returns>
        public string UpAndDown_Dir(string dir)
        {
            string Change_dir = "";
            Change_dir = Directory.GetParent(dir).FullName;
            return Change_dir;
        }
        #endregion

        #region  对组件进行批量操作
        /// <summary>
        /// 对组件进行批量操作
        /// </summary>
        public void SetLabelModule(FlowLayoutPanel FLPanel, ImageList ImaList)
        {
            string d_1 = "";
            int Ptag = 0;
            foreach (Control FP in FLPanel.Controls)                            //遍历FlowLayoutPanel控件中的所有控件
            {
                foreach (Control P in ((Panel)FP).Controls)                 //遍历Panel控件中的所有控件
                {
                    if (P is Label)                                     //如果是Label按件
                    {
                        P.Font = new Font("宋体", 9);                         //设置字体
                        P.ForeColor = Color.Black;                          //设置字体颜色
                        P.BackColor = Color.Transparent;                    //将背景色设为透明
                        d_1 = P.Name;                                   //获取当前控件的名称
                        if (d_1.LastIndexOf("_1") >= 0 || d_1.LastIndexOf("_2") >= 0)
                        {
                            P.Text = "";                                    //设置该控件文本为空
                        }
                    }
                    if (P is PictureBox)                                    //如果是PictureBox控件
                    {
                        PictureBox pb = (PictureBox)P;
                        Ptag = Convert.ToInt32(pb.Tag.ToString());              //获取该控件的Tag值
                        if (Ptag == 1 || Ptag == 2)                         //如果是指定的控件
                        {
                            pb.Enabled = true;                              //设置当前控件为可用状态
                            pb.Image = ImaList.Images[ImaList.Images.Count - 1];    //添加指定的图片
                        }
                    }
                }
            }
        }
        #endregion

        #region  对当前桌进行操作
        /// <summary>
        /// 对当前桌进行操作
        /// </summary>
        public ClassMsg DeskHandle(PictureBox PictBox, ImageList ImageLs, int n)
        {
            ClassUserInfo CUInfo = new ClassUserInfo();                 //创建ClassUserInfo对象
            Publec_Class.UserSeat = PictBox.Name;                       //获取座位名称
            ClassMsg msg = new ClassMsg();                          //创建ClassMsg对象
            PictBox.Image = null;                                       //将座位上的图片设为空
            int nTag = Convert.ToInt32(PictBox.Tag.ToString());             //获取当前座位的编号
            string Cauda = "";
            string d_1 = "";
            Publec_Class.DeskM = ((Panel)PictBox.Parent).Name;          //获取当前桌的名称
            Publec_Class.SeatM = PictBox.Name;                          //获取座位名称
            if (n == 1)                                             //如果是1，表示进入“对决”窗体
            {
                PictBox.Image = ImageLs.Images[Publec_Class.CaputID];       //显示当前用户的头像
                PictBox.Enabled = false;
            }
            if (n == 0)                                             //如果是0，表示退出“对决”窗体
            {
                PictBox.Image = ImageLs.Images[ImageLs.Images.Count - 1];   //设置为空座位
                PictBox.Enabled = true;
            }
            switch (nTag)
            {
                case 1:                                         //当前座位号是1
                    {
                        Cauda = "_1";
                        break;
                    }
                case 2:                                         //当前座位号是2
                    {
                        Cauda = "_2";
                        break;
                    }
            }
            foreach (Control P in (PictBox.Parent).Controls)                //遍历父级控件下的所有控件
            {
                if (P is Label)                                     //如果是Label控件
                {
                    d_1 = P.Name;                                   //获取当前控件的名称
                    if (d_1.LastIndexOf(Cauda) >= 0)                        //如果进入“对决”窗体
                    {
                        Publec_Class.UserCaption = d_1;             //记录当前座位的名称
                        if (n == 1)                                 //如果进入“对决”窗体
                        {
                            P.Text = Publec_Class.UserName;         //显示当前用户的名称
                            if (nTag == 2)                              //如果是右面的座位
                            {
                                P.Left = 134 - P.Width;             //设置用户名所在的位置
                            }
                        }
                        if (n == 0)                                 //如果退出“对决”窗体
                        {
                            P.Text = "";                                //将用户名清空
                        }
                        break;
                    }
                }
            }
            msg.RID = Publec_Class.ClientIP;                            //当前计算机的IP
            msg.UserName = Publec_Class.UserName;                       //当前用户的名称
            msg.Fraction = Convert.ToInt32(Publec_Class.Fraction);          //当前用户的分数
            msg.AreaMark = Publec_Class.TAreaM.ToString();              //区号
            msg.RoomMark = Publec_Class.TRoomM.ToString();          //房间号
            msg.DeskMark = Publec_Class.DeskM;                      //桌号
            msg.SeatMark = Publec_Class.SeatM;                          //座位号
            msg.UserCaption = Publec_Class.UserCaption;             //用户名称
            msg.CPhoto = Convert.ToInt32(Publec_Class.CaputID);         //头像
            msg.Sex = Publec_Class.UserSex;
            msg.Data = new ClassSerializers().SerializeBinary(CUInfo).ToArray();    //将用户列表写入二进制流中
            msg.sendKind = SendKind.SendCommand;                        //设置为发送命令
            msg.msgCommand = MsgCommand.BeginToGame;                //消息为进入“对决”窗体
            return msg;
        }
        #endregion

        #region  利用窗体上的控件移动窗体
        /// <summary>
        /// 利用控件移动窗体
        /// </summary>
        /// <param Frm="Form">窗体</param>
        /// <param e="MouseEventArgs">控件的移动事件</param>
        public void FrmMove(Form Frm, MouseEventArgs e)  //Form或MouseEventArgs添加命名空间using System.Windows.Forms;
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = Control.MousePosition;//获取当前鼠标的屏幕坐标
                myPosittion.Offset(CPoint.X, CPoint.Y);//重载当前鼠标的位置
                Frm.DesktopLocation = myPosittion;//设置当前窗体在屏幕上的位置
            }
        }
        #endregion

        #region  向在线用户列表中添加用户信息
        /// <summary>
        /// 对ListView控件进行格式化
        /// </summary>
        /// <param LV="ListView">ListView控件</param>
        public void Format_ListV(ListView LV, ImageList ImageL)
        {
            LV.Items.Clear();//清空所有项的集合
            LV.Columns.Clear();//清空所有列的集合
            LV.SmallImageList = ImageL;
            LV.GridLines = true;//在各数据之间形成网格线
            LV.View = View.Details;//显示列名称
            LV.FullRowSelect = true;//在单击某项时，对其进行选中
            LV.HeaderStyle = ColumnHeaderStyle.None;//隐藏列标题

            LV.Columns.Add(" ", 40, HorizontalAlignment.Right);//设置头像
            LV.Columns.Add("用户名", 110, HorizontalAlignment.Center);//设置头像
            LV.Columns.Add("分数", 70, HorizontalAlignment.Left);//设置头像
        }


        /// <summary>
        /// 向在线用户列表中添加用户信息
        /// </summary>
        /// <param LV="ListView">ListView控件</param>
        /// <param UName="string">用户名</param>
        /// <param F="string">分数</param>
        /// <param nC="string">头像</param>
        public void Data_List(ListView LV, string UName, string F, string nC)  //Form或MouseEventArgs添加命名空间using System.Windows.Forms;
        {
            ListViewItem item = new ListViewItem("", Convert.ToInt32(nC));
            item.SubItems.Add(UName);
            item.SubItems.Add(F);
            LV.Items.Add(item);
        }
        #endregion

        #region  在列表中移除用户信息
        /// <summary>
        /// 在列表中移除用户信息
        /// </summary>
        /// <param LV="ListView">ListView控件</param>
        /// <param UName="string">移除的用户名称</param>
        public void ReMove_List(ListView LV, string UName)
        {

            for (int i = 0; i < LV.Items.Count; i++)
            {
                if (LV.Items[i].SubItems[1].Text == UName)
                {
                    LV.Items[i].Remove();
                    break;
                }
            }
        }
        #endregion

        #region  在列表中修改用户信息
        /// <summary>
        /// 在列表中修改用户信息
        /// </summary>
        /// <param LV="ListView">ListView控件</param>
        /// <param DS="DataSet">返回查找的数据集</param>
        public void Amend_List(ListView LV, string UName, string F)
        {

            for (int i = 0; i < LV.Items.Count; i++)
            {
                if (LV.Items[i].SubItems[1].Text == UName)
                {
                    LV.Items[i].SubItems[2].Text = F;
                    break;
                }
            }
        }
        #endregion

        #region  在桌面上添加用户图标及相关信息
        /// <summary>
        /// 在桌面上添加用户图标及相关信息
        /// </summary>
        /// <param FLPanel="FlowLayoutPanel">FLPanel控件</param>
        /// <param userItem="ClassUserInfo">ClassUserInfo类（单用户信息）</param>
        /// <param ImaList="ImageList">ImageList组件</param>
        public void UserAdd_List(FlowLayoutPanel FLPanel, ClassUserInfo userItem, ImageList ImaList)
        {
            foreach (Control FP in FLPanel.Controls)
            {
                if (FP is Panel && FP.Name == userItem.DeskMark)
                {
                    foreach (Control P in ((Panel)FP).Controls)
                    {
                        if (P is PictureBox && P.Name == userItem.SeatMart)
                        {
                            ((PictureBox)P).Image = ImaList.Images[Convert.ToInt32(userItem.Caput)];
                            
                            ((PictureBox)P).AccessibleName = userItem.UserIP+"|"+userItem.UserName+"|"+userItem.Fraction+"|"+userItem.Caput+"|"+userItem.Sex;
                            ((PictureBox)P).Enabled = false;
                        }
                        if (P is Label && P.Name == userItem.UserCaption)
                        {
                            ((Label)P).Text = userItem.UserName;
                        }
                    }
                }
            }
        }
        #endregion

        #region  向指定桌添加游戏用户
        /// <summary>
        /// 向各桌添加游戏用户
        /// </summary>
        /// <param FLPanel="FlowLayoutPanel">FlowLayoutPanel控件</param>
        /// <param CUInfo="ClassMsg">ClassUserInfo类（单用户信息）</param>
        /// <param ImaList="ImageList">ImageList组件</param>
        /// <param IP="string">IP地址</param>
        /// <param MsgSign="MsgCommand">接收和消息命令</param>
        public void AddDeskMsg(FlowLayoutPanel FLPanel, ClassMsg CUInfo, ImageList ImaList, string IP, MsgCommand MsgSign)
        {
         
            if (Convert.ToInt32(CUInfo.AreaMark) == Publec_Class.TAreaM || Convert.ToInt32(CUInfo.RoomMark) == Publec_Class.TRoomM)
            {

                foreach (Control FP in FLPanel.Controls)
                {
                    if (FP is Panel && FP.Name == CUInfo.DeskMark)
                    {
                        foreach (Control P in ((Panel)FP).Controls)
                        {
                            if (P is PictureBox && P.Name == CUInfo.SeatMark)
                            {
                                if (MsgSign == MsgCommand.BeginToGameH)
                                {
                                    ((PictureBox)P).Image = ImaList.Images[Convert.ToInt32(CUInfo.CPhoto)];
                                    ((PictureBox)P).AccessibleName = IP + "|" + CUInfo.UserName + "|" + CUInfo.Fraction.ToString() + "|" + CUInfo.CPhoto.ToString() + "|" + CUInfo.Sex.ToString();
                                    ((PictureBox)P).Enabled = false;
                                }
                                if (MsgSign == MsgCommand.EndToGameH)
                                {
                                    ((PictureBox)P).Image = ImaList.Images[ImaList.Images.Count - 1];
                                    ((PictureBox)P).AccessibleName = "";
                                    ((PictureBox)P).Enabled = true;
                                }
                            }
                            if (P is Label && P.Name == CUInfo.UserCaption)
                            {
                                if (MsgSign == MsgCommand.BeginToGameH)
                                    ((Label)P).Text = CUInfo.UserName;
                                if (MsgSign == MsgCommand.EndToGameH)
                                    ((Label)P).Text = "";
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region  添加语聊的信息
        /// <summary>
        /// 添加语聊的信息
        /// </summary>
        /// <param RTX="RichTextBox">RichTextBox控件</param>
        /// <param CUInfo="ClassMsg">发送的信息</param>
        /// <param MsgSign="MsgCommand">消息命令</param>
        public void AddMsgText(RichTextBox RTX, ClassMsg CUInfo, MsgCommand MsgSign)
        {
            RTX.ReadOnly = true;//设为只读
            RTX.ForeColor = Color.SlateGray;//设置字体颜色
            RTX.AppendText(CUInfo.UserName);//添加发送信息的用户名
            RTX.AppendText("\r\n");//换行
            RTX.AppendText(CUInfo.MsgText);//添加发送的信息
            RTX.AppendText("\r\n");//换行
            RTX.ScrollToCaret();//将信息添加到控件
        }
        #endregion

        #region  获取对方玩家的IP地址
        /// <summary>
        /// 获取对方玩家的IP地址
        /// </summary>
        /// <param GameP="Panel">Panel控件</param>
        public string ToGameIP(PictureBox GameP)
        {
            int PTag = Convert.ToInt32(GameP.Tag.ToString());                   //获取当前的座位号
            string temIP = "";
            string Gname = GameP.Name;                                  //获取座位名称
            Control panPar = GameP.Parent;                                  //获取当前控件的父级控件
            if (PTag == 1)                                              //如果当前座位号为1
                PTag = 2;                                               //对方座位号就是2
            else
            {
                if (PTag == 2)                                          //如果当前座位号为2
                    PTag = 1;                                           //对方座位号就是1
            }
            foreach (Control P in panPar.Controls)                              //遍历父级控件，查找对方的座位
            {
                if (P is PictureBox && Convert.ToInt32(P.Tag.ToString()) == PTag)       //如果找到对方的座位
                {
                    temIP = P.AccessibleName;                               //获取对方的IP地址
                    break;
                }
            }
            return temIP;
        }
        #endregion

        #region  获取对方玩家的信息
        /// <summary>
        /// 获取对方玩家的信息
        /// </summary>
        /// <param Data="byte[]">用户信息</param>
        public void Game_TerraInfo(byte[] Data)
        {
            ClassUsers users = new ClassUsers();
            ClassMsg msg = (ClassMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(Data));
            users = (ClassUsers)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));//获取所有用户信息

            ClassUserInfo userItem = new ClassUserInfo();
            for (int i = 0; i < users.Count; i++)
            {
                userItem = users[i];
                if (userItem.UserIP != "")
                {
                    GameIP = userItem.UserIP;//记录用户的IP地址
                    GamePort = userItem.UserPort;//记录用户的端口号
                    GameName = userItem.UserName;//记录用户名称
                    GameFraction = userItem.Fraction;//当前分数
                    GameCaput = userItem.Caput;//头像
                    GameSex = userItem.Sex;//性别
                    break;
                }
            }
        }
        #endregion

        #region  向对方发送本地用户的信息
        /// <summary>
        /// 向对方发送本地用户的信息
        /// </summary>
        public ClassMsg Game_FarInfo(String G_ToIP)
        {
            ClassMsg msg = new ClassMsg();
            ClassUsers Users = new ClassUsers();
            ClassUserInfo UserItem = new ClassUserInfo();//创建并引用ClassUserInfo类
            UserItem.UserIP = Publec_Class.ClientIP;//记录用户的IP地址
            UserItem.UserPort = Publec_Class.ServerPort;//记录端口号
            UserItem.UserName = Publec_Class.ClientName;//记录用户名称
            UserItem.Fraction = Publec_Class.Fraction.ToString();//当前分数
            UserItem.Caput = Publec_Class.CaputID.ToString();//头像
            UserItem.Sex = Publec_Class.UserSex.ToString();//头像
            Users.add(UserItem);//将单用户信息添加到用户列表中
            
            msg.Data = new ClassSerializers().SerializeBinary(Users).ToArray();//将用户列表写入到二进制流中
            msg.msgCommand = MsgCommand.EndRival;
            return msg;
        }
        #endregion

    }
}
