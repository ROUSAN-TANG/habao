using System;
using System.Collections.Generic;
using System.Text;

namespace ClientGobang.ChessClass
{

    public class ClientObjectEventArgs : EventArgs
    {
        private string _message;

        public string Message
        {
            get { return _message; }
        }

        public ClientObjectEventArgs(string message)
        {
            this._message = message;
        }

    }

}
