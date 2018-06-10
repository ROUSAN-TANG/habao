using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GobangClass
{
    /// <summary>
    /// ��¼���˵���Ϣ
    /// </summary>
    [Serializable]
    public class ClassUserInfo
    {
        /// <summary>
        /// �û����
        /// </summary>
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// �û����ڵ�¼������IP
        /// </summary>
        private string userIP;

        public string UserIP
        {
            get { return userIP; }
            set { userIP = value; }
        }
        /// <summary>
        /// �û����ڵ�¼�������˿ں�
        /// </summary>
        private string userPort;

        public string UserPort
        {
            get { return userPort; }
            set { userPort = value; }
        }
        /// <summary>
        /// �û���
        /// </summary>
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// ��ǰ�û��ĵ÷���
        /// </summary>
        private String fraction;

        public String Fraction
        {
            get { return fraction; }
            set { fraction = value; }
        }
        /// <summary>
        /// ��ǰ�û�״̬
        /// </summary>
        private String state;

        public String State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// ��ǰ�û�������
        /// </summary>
        private String borough;

        public String Borough
        {
            get { return borough; }
            set { borough = value; }
        }
        /// <summary>
        /// ��ǰ�û��ķ����
        /// </summary>
        private String roomMark;

        public String RoomMark
        {
            get { return roomMark; }
            set { roomMark = value; }
        }
        /// <summary>
        /// ��ǰ�û�������
        /// </summary>
        private String deskMark;

        public String DeskMark
        {
            get { return deskMark; }
            set { deskMark = value; }
        }
        /// <summary>
        /// ��ǰ�û�����λ��
        /// </summary>
        private String seatMart;

        public String SeatMart
        {
            get { return seatMart; }
            set { seatMart = value; }
        }
        /// <summary>
        /// ��ǰ����ʾ�û����ƵĿؼ���
        /// </summary>
        private String userCaption;

        public String UserCaption
        {
            get { return userCaption; }
            set { userCaption = value; }
        }
        /// <summary>
        /// ��ǰ�û���ͷ��
        /// </summary>
        private String caput;

        public String Caput
        {
            get { return caput; }
            set { caput = value; }
        }

        /// <summary>
        /// ��ǰ�û����Ա�
        /// </summary>
        private String sex;

        public String Sex
        {
            get { return sex; }
            set { sex = value; }
        }
    }
}
