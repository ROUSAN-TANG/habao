using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GobangClass
{
    /// <summary>
    /// 记录单人的信息
    /// </summary>
    [Serializable]
    public class ClassUserInfo
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// 用户正在登录的主机IP
        /// </summary>
        private string userIP;

        public string UserIP
        {
            get { return userIP; }
            set { userIP = value; }
        }
        /// <summary>
        /// 用户正在登录的主机端口号
        /// </summary>
        private string userPort;

        public string UserPort
        {
            get { return userPort; }
            set { userPort = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// 当前用户的得分数
        /// </summary>
        private String fraction;

        public String Fraction
        {
            get { return fraction; }
            set { fraction = value; }
        }
        /// <summary>
        /// 当前用户状态
        /// </summary>
        private String state;

        public String State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// 当前用户的区号
        /// </summary>
        private String borough;

        public String Borough
        {
            get { return borough; }
            set { borough = value; }
        }
        /// <summary>
        /// 当前用户的房间号
        /// </summary>
        private String roomMark;

        public String RoomMark
        {
            get { return roomMark; }
            set { roomMark = value; }
        }
        /// <summary>
        /// 当前用户的桌号
        /// </summary>
        private String deskMark;

        public String DeskMark
        {
            get { return deskMark; }
            set { deskMark = value; }
        }
        /// <summary>
        /// 当前用户的坐位号
        /// </summary>
        private String seatMart;

        public String SeatMart
        {
            get { return seatMart; }
            set { seatMart = value; }
        }
        /// <summary>
        /// 当前桌显示用户名称的控件名
        /// </summary>
        private String userCaption;

        public String UserCaption
        {
            get { return userCaption; }
            set { userCaption = value; }
        }
        /// <summary>
        /// 当前用户的头像
        /// </summary>
        private String caput;

        public String Caput
        {
            get { return caput; }
            set { caput = value; }
        }

        /// <summary>
        /// 当前用户的性别
        /// </summary>
        private String sex;

        public String Sex
        {
            get { return sex; }
            set { sex = value; }
        }
    }
}
