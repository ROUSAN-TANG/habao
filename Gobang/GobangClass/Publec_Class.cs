using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;//��Ӵ������������ռ�
using System.Net;//���������Ϣ�������ռ�
using System.Runtime.InteropServices;//���"kernel32"API����

namespace GobangClass
{
    /// <summary>
    /// ��ȡ��������windows·��
    /// </summary>
    public class Publec_Class
    {
        #region  ȫ�ֱ���
        public static string ServerIP = "";//������IP
        public static string ServerPort = "";//���صĶ˿ں�
        public static string ClientIP = "";//��ǰ�û���IP
        public static string ClientPort = "";
        public static string ClientName = "";//��ǰ�û�������
        public static string UserName;
        public static string UserID;//��ǰ�û���IP
        public static int CaputID = 0;//��ǰ�û���ͷ��

        public static int UserSex = 0;//��ǰ�û����Ա�
        public static int Fraction = 0;//��ǰ�û��ķ���
        public static int TAreaM = 0;//����
        public static int TRoomM = 0;//�����
        public static string DeskM = "";//����
        public static string SeatM = "";//��λ��
        public static string UserCaption;//��λ���е��û�����
        public static string UserSeat = "";//��ǰ�û�����λ��
        public static string SubtenseIP = "";//��¼�Ծ��жԷ���IP��ַ
        #endregion

        #region  ��ȡ������
        /// <summary>
        /// ��ȡ������
        /// </summary>
        public string MyHostIP()
        {
            // ��ʾ������
            string hostname = Dns.GetHostName();
            // ��ʾÿ��IP��ַ
            IPHostEntry hostent = Dns.GetHostByName(hostname); // ������Ϣ
            return hostent.AddressList[0].ToString();
        }
        #endregion
    }
}
