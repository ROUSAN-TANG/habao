using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ClientGobang
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ClientGobang.ChessClass.Client cc = new ClientGobang.ChessClass.Client(); 
            Program.PublicClientObject = cc;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_Hall());
        }

        private static ClientGobang.ChessClass.Client _clientobject;

        public static ClientGobang.ChessClass.Client PublicClientObject
        {
            set { _clientobject = value; }
            get { return _clientobject; }
        }
    }
}