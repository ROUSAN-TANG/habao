using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;//添加代送器的命名空间
using System.Net;//添加主机信息的命名空间
using System.Runtime.InteropServices;//添加"kernel32"API函数

namespace GobangClass
{
    /// <summary>
    /// 获取主机名及windows路径
    /// </summary>
    public class Publec_Class
    {
        #region  全局变量
        public static string ServerIP = "";//服务器IP
        public static string ServerPort = "";//本地的端口号
        public static string ClientIP = "";//当前用户的IP
        public static string ClientPort = "";
        public static string ClientName = "";//当前用户的名称
        public static string UserName;
        public static string UserID;//当前用户的IP
        public static int CaputID = 0;//当前用户的头像

        public static int UserSex = 0;//当前用户的性别
        public static int Fraction = 0;//当前用户的分数
        public static int TAreaM = 0;//区号
        public static int TRoomM = 0;//房间号
        public static string DeskM = "";//桌号
        public static string SeatM = "";//坐位号
        public static string UserCaption;//坐位号中的用户名称
        public static string UserSeat = "";//当前用户的坐位名
        public static string SubtenseIP = "";//记录对决中对方的IP地址
        #endregion

        #region  获取主机名
        /// <summary>
        /// 获取主机名
        /// </summary>
        public string MyHostIP()
        {
            // 显示主机名
            string hostname = Dns.GetHostName();
            // 显示每个IP地址
            IPHostEntry hostent = Dns.GetHostByName(hostname); // 主机信息
            return hostent.AddressList[0].ToString();
        }
        #endregion
    }
}
