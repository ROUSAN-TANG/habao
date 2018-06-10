using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;//�����������ܵ������ռ�
using System.Collections;//�������������ռ�
using System.Net.Sockets;//Sockets�����ռ�
using System.Threading;//�߳������ռ�
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

        public static int ConnHandle = 0;//�����ӡ�׼������ʼ��ť�Ĳ�����ʶ���м�¼
        public static float Mouse_X = 0;//��¼����X����
        public static float Mouse_Y = 0;//��¼����Y����
        public static int[,] note = new int[15, 15];//��¼���ӵİڷ�λ��
        public static bool Conqueror = false;//��¼��ǰ�Ƿ��Ѿ���ʤ��
        public static int CKind = -1;//��¼ȡʤ������
        public static bool dropchild = true;//��ǰ��˭����
        public static bool StartListen = false;//�Ƿ������˼���
        public static int jlsf = -1;
        public static string LineName = "";//�߳���
        public static bool Agin_if = false;//�ж��Ƿ����¿�ʼ
        public static int if_UPdata = 0;//�Ƿ���ķ���
        public static bool childSgin = true;//��������
        public static bool ChildSgin = true;//�Է���������

        public int BBow = 0;
        int BwinCount = 0;
        int WwinCount = 0;
        bool Out = false;
        public bool CanAgin, CanAgins, CanDown, WhoFisrtDown;
        private Socket listener;
        private Thread mainThread;
        ClassUsers users;
        ClientClass frmClient = new ClientClass();

        public static string GIP = ClientClass.GameIP;//IP��ַ
        public static string GPort = "11003";//�˿ں�
        public static string Gem_N = "";//����
        public static int Gem_F = 0;//����
        public static int Gem_C = 0;//ͷ��
        public static int Gem_S = 0;//�Ա�

        public void GetGameInfo(string temInfo)
        {
            if (temInfo.Length == 0)                                //�����ȡ����ϢΪ�գ������Ƕ���û�����
                return;
            string Tem_Str = GIP;                                   //��¼�Է�����Ϣ
            if (Tem_Str.IndexOf('|') == -1)                         //�ж������Ϣ�Ƿ���ȷ
                return;
            try
            {
                GIP = Tem_Str.Substring(0, Tem_Str.IndexOf('|'));       //��ȡ�Է���ҵ�IP��ַ
                                                                        //��ȡ��ȡ�ֶ�֮��������ַ���
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_N = Tem_Str.Substring(0, Tem_Str.IndexOf('|'));             //��ȡ�Է���ҵ�����
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_F = Convert.ToInt32(Tem_Str.Substring(0, Tem_Str.IndexOf('|')));    //��ȡ�Է���ҵķ���
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_C = Convert.ToInt32(Tem_Str.Substring(0, Tem_Str.IndexOf('|')));    //��ȡ�Է���ҵ�ͷ����
                Tem_Str = Tem_Str.Substring(Tem_Str.IndexOf('|') + 1, Tem_Str.Length-Tem_Str.IndexOf('|') - 1);
                Gem_S = Convert.ToInt32(Tem_Str);                           //��ȡ�Է���ҵ��Ա�
            }
            catch
            {
                MessageBox.Show("��ȡ�Է�������ʱ����");
            }
        }

        private void Frm_Chessboard_Shown(object sender, EventArgs e)
        {
            frmClient.Format_ListV(listView_Battle, imageList2);        //���û��б���г�ʼ��
                                                                        //����ǰ�û�����Ϣ��ӵ��û��б���
            frmClient.Data_List(listView_Battle, Publec_Class.UserName, Publec_Class.Fraction.ToString(), Publec_Class.CaputID.ToString());
            label_N.Text = Publec_Class.UserName;                   //��ʾ��ǰ�û�������
            label_F.Text = Publec_Class.Fraction.ToString();            //��ʾ��ǰ�û��ķ���
            pictureBox_C.Image = null;                              //��յ�ǰ�û���ͷ��
            if (Publec_Class.UserSex == 0)                          //�����ǰ�û�������
            {
                pictureBox_C.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
            }
            else
            {
                pictureBox_C.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\Ů��.png");
            }
            GIP = ClientClass.GameIP.Trim();                        //��ȡ�Է���ҵ������Ϣ
            if (GIP == "")                                      //���Ϊ�գ���ʾֻ�����ѽ������
            {
                pboxStart.Enabled = false;                          //ʹ����ʼ����ť������
                pictureBox_Left.Image = null;                       //��նԾ�˫����ͷ��ͼƬ
                pictureBox_Right.Image = null;
                if (Publec_Class.UserSex == 0)                      //�����ǰ�û�������
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                }
                else
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\Ů��.png");
                }
                label_Right.Text = Publec_Class.UserName;           //��ʾ��ǰ�û�������
                pictureBox_Q_Right.Image = null;                    //�����ʾ�������͵�ͼƬ
                                                                    //��ʾ��ǰ�û�Ϊ����
                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                pboxStart.Image = null;                      //��ա���ʼ����ť
                                                                    //ʹ����ʼ����ťΪ�Ҷ���ʾ
                pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť��.png");
                pboxStart.Enabled = false;                      //ʹ����ʼ����ť������
            }
            else
            {
                GetGameInfo(ClientClass.GameIP);                    //��ȡ�Է���ҵ������Ϣ
                ClassMsg temMsg = new ClassMsg();
                ClassUsers Users = new ClassUsers();
                ClassUserInfo UserItem = new ClassUserInfo();           //����������ClassUserInfo��
                pictureBox_Left.Image = null;                       //��յ�ǰ�û���ͷ��
                if (Publec_Class.UserSex == 0)                      //�����ǰ�û�������
                {
                    pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                }
                else
                {
                    pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\Ů��.png");
                }
                pictureBox_Q_Left.Image = null;                     //��յ�ǰ�û�����������
                                                                    //��ʾ��ǰ�û�����������Ϊ����
                pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                label_Left.Text = Publec_Class.UserName;            //��ʾ��ǰ�û�������
                pictureBox_Right.Image = null;                      //��նԷ���ҵ�ͷ��
                if (Gem_S == 0)                                 //����Է����������
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                }
                else
                {
                    pictureBox_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\Ů��.png");
                }
                label_Right.Text = Gem_N;                           //��ʾ�Է���ҵ�����
                pictureBox_Q_Right.Image = null;                    //��նԷ���ҵ���������
                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                //���Է���ҵ���Ϣ��ʾ���û��б���
                frmClient.Data_List(listView_Battle, Gem_N, Gem_F.ToString(), Gem_C.ToString());
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.GetGameF;
                temMsg.RIP = GIP;

                //ԭ����11000
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11001, new ClassSerializers().SerializeBinary(temMsg).ToArray());
                pboxStart.Image = null;                     //��ա���ʼ����ť
                pboxStart.Enabled = true;                       //ʹ����ʼ����ť����
                                                                //���ء���ʼ����ť�Ŀ���ͼƬ
                pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť.png");
                UserItem.UserIP = Publec_Class.ClientIP;            //��¼�û���IP��ַ
                UserItem.UserPort = Publec_Class.ServerPort;            //��¼�˿ں�
                UserItem.UserName = Publec_Class.UserName;          //��¼�û�����
                UserItem.Fraction = Publec_Class.Fraction.ToString();       //��ǰ����
                UserItem.Caput = Publec_Class.CaputID.ToString();       //ͷ��
                UserItem.Sex = Publec_Class.UserSex.ToString();     //�Ա�
                Users.add(UserItem);                                //�����û���Ϣ��ӵ��û��б���
                pboxStart.Enabled = true;
                ThreadStart ts = new ThreadStart(this.StartServer);     //��������
                mainThread = new Thread(ts);
                mainThread.Name = "GOB_Chess";
                LineName = mainThread.Name;
                mainThread.Start();                             //���ӷ���
                                                                //���û��б�д�����������
                temMsg.Data = new ClassSerializers().SerializeBinary(Users).ToArray();
                temMsg.msgCommand = MsgCommand.BeginJoin;       //���÷��͵���Ϣ����
                                                                //����ǰ�û�����Ϣ���͸��Է����
                udpSocket1.Send(IPAddress.Parse(GIP), 11001, new ClassSerializers().SerializeBinary(temMsg).ToArray());
                StartListen = true;
                ConnHandle = 1;
            }
            panel_Check.Click += new EventHandler(asd);         //�������̵ĵ����¼�
            CanDown = false;
            for (int i = 0; i < 15; i++)                                //�����ӵļ�¼λ�ý��г�ʼ��
                for (int j = 0; j < 15; j++)
                    note[i, j] = -1;
        }

        private void pboxStart_Click(object sender, EventArgs e)
        {
            pboxStart.Image = null;//��ա���ʼ����ť
            //���á���ʼ����ťΪ�Ҷ�
            pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť��.png");
            pboxStart.Enabled = false;//ʹ����ʼ����ť������

            switch (ConnHandle)
            {
                case 1://��1�ε�������ʼ����ť���������ӶԷ�����ʼ����
                    {
                        ChessClass.Client cc = Program.PublicClientObject;
                        if (cc.Connected)//����ѿ���������
                        {
                            cc.CloseConnection();//�Ͽ�����
                        }

                        try
                        {
                            if (GIP == "")//���û�жԷ���IP��ַ
                                break;
                            cc.ConnectServer(GIP, int.Parse("11003"));//��������
                        }
                        catch
                        {
                            MessageBox.Show("���ӷ�����ʧ��");//�������ʧ�ܣ�ʹ����ʼ����ť����
                            this.pboxStart.Image = null;
                            this.pboxStart.Enabled = true;
                            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť.png");
                            return;
                        }
                        WhoFisrtDown = false;//����˭������
                        cc.SendMessage("FiDn" + "Me");//������Ϣ
                        CanDown = true;
                        ConnHandle = 2;
                        break;
                    }
                case 2://��2�μ����Ժ󵥻������¿�ʼ��Ϸ
                    {

                        CanAgin = true;
                        WhoFisrtDown = false;
                        dropchild = true;
                        CanDown = true;
                        Conqueror = false;
                        ChessClass.Client cc = Program.PublicClientObject;
                        cc.SendMessage("FiDn" + "Me");//������Ϣ���Ƿ���������
                        if (Agin_if == false)
                        {
                            cc.SendMessage("Agin#" + "OK?");
                            Agin();//������������ĳ�ʼֵ
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
                        pBox_Sign.Visible = false;//ʹ���ӵı�ʶͼƬ���ɼ�
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
            frmClient.CPoint = new Point(-e.X, -e.Y);//��¼��ǰ����
        }

        private void panel_Title_MouseMove(object sender, MouseEventArgs e)
        {
            frmClient.FrmMove(this, e);//�ƶ�����
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
                MessageBox.Show("�Ƿ�رմ���");
                
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        //�������
        public void asd(object sender, EventArgs e)
        {
            if (dropchild == false)//�����ǰ�û���������
                return;
            if ((Mouse_X <= 30.0 / 2.0) || (Mouse_Y <= 30.0 / 2.0))//������ӵ㳬�����ӷ�Χ
                return;
            if (CanDown == true)//����ѿ�ʼ����
            {
                if (Conqueror == true)//������������Ӯ��
                {
                    if (CKind == 0)//����Ӯ
                        Bwin();
                    if (CKind == 1)//����Ӯ
                        Wwin();
                    return;
                }
                if (WhoFisrtDown == true)//�Է�������
                {
                    int Column = Convert.ToInt32(Math.Round((Mouse_X - 30) / 35));//��ȡ�����������е�����
                    int Row = Convert.ToInt32(Math.Round((Mouse_Y - 30) / 35));//��ȡ�����������е�����
                    int bw = 0;

                    if (note[Convert.ToInt32(Row), Convert.ToInt32(Column)] >= 0)//�����ǰ��������
                        return;
                    PictureBox pictureBoxTem = new PictureBox();//��̬����һ��ͼƬ�ؼ�
                    pictureBoxTem.Parent = panel_Check;//�����丸����
                    pictureBoxTem.Location = new Point(Column * 35 + 9, Row * 35 + 9);//����ͼƬ�ؼ���λ��
                    //����ͼƬ�ؼ�������
                    pictureBoxTem.Name = "pictureBox" + Row.ToString() + "*" + Column.ToString();
                    //���ñ�ʶͼƬ��λ�ã����ڱ�ʶ����µ����ӣ�
                    pBox_Sign.Location = new Point(Column * 35 + 20, Row * 35 + 20);

                    pictureBoxTem.Size = new Size(30, 30);//�������ӵĴ�С
                    pictureBoxTem.SizeMode = PictureBoxSizeMode.StretchImage;//����ͼ�����ʽ
                    ChessClass.Client cc = Program.PublicClientObject;
                    BBow++;
                    pBox_Sign.Visible = true;//ʹ��ʶͼƬ�ɼ�
                    pBox_Sign.BringToFront();//�����ǰ�������Ǻ���
                    if (BBow % 2 == 1)
                    {
                        label_Genre.Text = "����";
                        label_Genre.Tag = 0;
                        pictureBoxTem.Image = imageList1.Images[0];//��ʾ��ǰ����Ϊ����
                        bw = 0;//��ʶ��ǰ�û�Ϊ����
                        note[Row, Column] = 0;//��¼��ǰ���ӵ�����λ��
                        //����ǰ�µ�������Ϣ���͸��Է����
                        cc.SendMessage("Down#" + Row.ToString() + "*" + Column.ToString() + "|" + "0" + "@" + BBow.ToString());
                    }
                    else
                    {
                        label_Genre.Text = "����";
                        label_Genre.Tag = 1;
                        pictureBoxTem.Image = imageList1.Images[1];//��ʾ��ǰ����Ϊ����
                        bw = 1;//��ʶ��ǰ�û�Ϊ����
                        note[Row, Column] = 1;//��¼��ǰ���ӵ�����λ��
                        //����ǰ�µ�������Ϣ���͸��Է����
                        cc.SendMessage("Down#" + Row.ToString() + "*" + Column.ToString() + "|" + "1" + "@" + BBow.ToString());
                    }
                    //��������������ʾ��Ӧ��ͼƬ
                    if (childSgin == true)
                    {
                        if (BBow % 2 == 1)
                        {
                            if (label_Left.Text.Trim() == Publec_Class.UserName)
                            {
                                pictureBox_Q_Left.Image = null;
                                pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                            }
                            else
                            {
                                pictureBox_Q_Right.Image = null;
                                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                            }
                        }
                        else
                        {
                            if (label_Left.Text.Trim() == Publec_Class.UserName)
                            {
                                pictureBox_Q_Left.Image = null;
                                pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                            }
                            else
                            {
                                pictureBox_Q_Right.Image = null;
                                pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                            }
                        }
                        childSgin = false;
                    }
                    note[Row, Column] = bw;
                    if_UPdata = 0;
                    Arithmetic(bw, Row, Column);//���㵱ǰ�µ������Ƿ�Ӯ
                }
                else
                {
                    MessageBox.Show("�Է���û�����ӣ�");
                    dropchild = true;
                    return;
                }
            }
            else
            {
                MessageBox.Show("�������û�п�ʼ�޷����壡");
                dropchild = true;
                return;
            }
            dropchild = false;
        }

        //��ӶԷ��µ���
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

                string Cplace = e.Number;//��ȡ���ӵ�����
                string Ccolumn = Cplace.Substring(0, Cplace.IndexOf("*"));//��ȡ������������
                //��ȡ������������
                string Crow = Cplace.Substring(Cplace.IndexOf("*") + 1, Cplace.Length - (Cplace.IndexOf("*") + 1));
                PictureBox pictureBoxTem = new PictureBox();//����һ��PictureBox�ؼ�
                pictureBoxTem.Parent = panel_Check;//�����丸����
                //�������ӵ�λ��
                pictureBoxTem.Location = new Point(Convert.ToInt32(Crow) * 35 + 9, Convert.ToInt32(Ccolumn) * 35 + 9);
                pictureBoxTem.Name = "pictureBox" + e.Number;//�������ӵ�����
                pictureBoxTem.Size = new Size(30, 30);//�������ӵĴ�С
                pictureBoxTem.SizeMode = PictureBoxSizeMode.StretchImage;
                //��¼���ӵ����ͼ�λ��
                note[Convert.ToInt32(Ccolumn), Convert.ToInt32(Crow)] = Convert.ToInt32(e.Im);
                int num = Int32.Parse(e.Im);//��¼���ӵ�����
                BBow = Int32.Parse(e.Bow);
                pictureBoxTem.Image = imageList1.Images[num];//�������ͼƬ
                pBox_Sign.Visible = true;
                //���ñ�ʶͼƬ��λ��
                pBox_Sign.Location = new Point(Convert.ToInt32(Crow) * 35 + 20, Convert.ToInt32(Ccolumn) * 35 + 20);
                pBox_Sign.BringToFront();
                if_UPdata = 1;
                Arithmetic(num, Convert.ToInt32(Ccolumn), Convert.ToInt32(Crow));//�����ӽ��м���

                if_UPdata = 0;
                //��������������ʾ��Ӧ��ͼƬ
                if (ChildSgin == true)
                {
                    if (BBow % 2 == 1)
                    {
                        if (label_Left.Text.Trim() == Publec_Class.UserName)
                        {
                            pictureBox_Q_Left.Image = null;
                            pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                        }
                        else
                        {
                            pictureBox_Q_Right.Image = null;
                            pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                        }
                    }
                    else
                    {
                        if (label_Left.Text.Trim() == Publec_Class.UserName)
                        {
                            pictureBox_Q_Left.Image = null;
                            pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                        }
                        else
                        {
                            pictureBox_Q_Right.Image = null;
                            pictureBox_Q_Right.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                        }
                    }
                    ChildSgin = false;
                }
            }
            WhoFisrtDown = true;
            dropchild = true;
        }
        //�жϺ����Ƿ�Ӯ
        public void Bwin()
        {
            MessageBox.Show("����Ӯ��!!�����¿�ʼ��Ϸ��");

            ClassMsg temMsg = new ClassMsg();
            if (label_Genre.Text == "����")
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
                temMsg.RIP = Publec_Class.ClientIP;//��¼�û���IP��ַ
                temMsg.RPort = Publec_Class.ServerPort;//��¼�˿ں�
                temMsg.Fraction = Publec_Class.Fraction;//��ǰ����
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.UPDataFract;
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());

                temMsg.RIP = GIP;//��¼�û���IP��ַ
                temMsg.RPort = GPort;//��¼�˿ں�
                temMsg.Fraction = Gem_F;//��ǰ����
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
            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť.png");
        }
        //�жϰ����Ƿ�Ӯ
        public void Wwin()
        {
            MessageBox.Show("����Ӯ��!!�����¿�ʼ��Ϸ��");

            ClassMsg temMsg = new ClassMsg();
            if (label_Genre.Text == "����")
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
                temMsg.RIP = Publec_Class.ClientIP;//��¼�û���IP��ַ
                temMsg.RPort = Publec_Class.ServerPort;//��¼�˿ں�
                temMsg.Fraction = Publec_Class.Fraction;//��ǰ����
                temMsg.sendKind = SendKind.SendCommand;
                temMsg.msgCommand = MsgCommand.UPDataFract;
                udpSocket1.Send(IPAddress.Parse(Publec_Class.ServerIP), 11000, new ClassSerializers().SerializeBinary(temMsg).ToArray());

                temMsg.RIP = GIP;//��¼�û���IP��ַ
                temMsg.RPort = GPort;//��¼�˿ں�
                temMsg.Fraction = Gem_F;//��ǰ����
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
            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť.png");
        }

        //��������
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
                    Thread.Sleep(30);//˯�����߳�
                    mainThread.Abort();//�ر����߳�
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
                //������������
                Socket server;
                try
                {
                    server = listener.Accept();
                }
                catch
                {
                    break;
                }

                //�����ͻ��˶���
                ChessClass.ClientObject client = new ChessClass.ClientObject();
                client.ClientSocket = server;
                //���նԷ��µ���
                client.OnAddChess += new EventHandler<ChessClass.AddChessEventArgs>(AddChess);
                client.OnAddMessage += new EventHandler<ChessClass.AddMessageEventArgs>(AddMessage);
                client.OnAginMessage += new EventHandler<ChessClass.AginMessageEventArgs>(AginMessage);
                client.OnFiDnMessage += new EventHandler<ChessClass.FiDnMessageEventArgs>(FiDnMessage);
                client.receiveMessage();
            }
        }

        //��ӶԷ���������Ϣ
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
                MessageBox.Show("��Ϸ��ʼ�������¡�");
            }
            else
            {
                MessageBox.Show("��Ϸ��ʼ������¡�");
            }
        }
        public void FiDnMessage(object sender, ChessClass.FiDnMessageEventArgs e)//˭��������ж�
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

        //�ж����¿�ʼ������
        public void Agin()
        {
            if (CanAgins == false && CanAgin == true)//����������Ӯ��
            {
                for (int i = 0; i < panel_Check.Controls.Count; i++)//���������ϵ���������
                {
                    if (panel_Check.Controls[i] is PictureBox)//����ҵ�����
                    {
                        panel_Check.Controls.RemoveAt(i);//ɾ����ǰ����
                        i = -1;
                    }
                }
                //��ʼ����¼����λ�õĶ�ά����
                for (int i = 0; i < 15; i++)
                    for (int j = 0; j < 15; j++)
                        note[i, j] = -1;
            }
        }
        //���¿�ʼ
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
            //�������
            bool Lbol = true;
            bool Rbol = true;
            int jlsf = 0;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Acolumn + i) > 14)                     //������ӳ����������
                    Rbol = false;
                if ((Acolumn - i) < 0)                      //������ӳ�����С����
                    Lbol = false;
                if (Rbol == true)
                {
                    if (note[Arow, Acolumn + i] == n)           //���������������ͬ������
                        ++BCount;
                    else
                        Rbol = false;
                }
                if (Lbol == true)
                {
                    if (note[Arow, Acolumn - i] == n)           //���������������ͬ������
                        ++BCount;
                    else
                        Lbol = false;
                }
                if (BCount >= 5)                            //���ͬ���͵����������ڵ���5
                {
                    if (n == 0)                         //���巽Ӯ
                        Bwin();
                    if (n == 1)                         //���巽Ӯ
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
            //�������
            bool Ubol = true;
            bool Dbol = true;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Arow + i) > 14)                        //����������̵��������
                    Dbol = false;
                if ((Arow - i) < 0)                         //����������̵���С����
                    Ubol = false;
                if (Dbol == true)
                {
                    if (note[Arow + i, Acolumn] == n)           //���������ͬ���͵�����
                        ++BCount;
                    else
                        Dbol = false;
                }
                if (Ubol == true)
                {
                    if (note[Arow - i, Acolumn] == n)           //���������ͬ���͵�����
                        ++BCount;
                    else
                        Ubol = false;
                }
                if (BCount >= 5)                            //���ͬ���͵����Ӵ��ڵ���5
                {
                    if (n == 0)                         //����Ӯ
                        Bwin();
                    if (n == 1)                         //����Ӯ
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
            //�Խ��߲���
            bool LUbol = true;
            bool RDbol = true;
            BCount = 1;
            for (int i = 1; i <= 5; i++)
            {
                if ((Arow - i) < 0 || (Acolumn - i < 0))            //������������б��
                    LUbol = false;
                if ((Arow + i) > 14 || (Acolumn + i > 14))      //������������б��
                    RDbol = false;
                if (LUbol == true)
                {
                    if (note[Arow - i, Acolumn - i] == n)       //�������б��������ͬ���͵�����
                        ++BCount;
                    else
                        LUbol = false;
                }
                if (RDbol == true)
                {
                    if (note[Arow + i, Acolumn + i] == n)   //�������б��������ͬ���͵�����
                        ++BCount;
                    else
                        RDbol = false;
                }
                if (BCount >= 5)                        //���ͬ���͵����Ӵ��ڵ���5
                {
                    if (n == 0)
                        Bwin();
                    if (n == 1)
                        Wwin();
                    jlsf = n;
                    break;
                }
            }
            //��б����
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
                    if (note[Arow - i, Acolumn + i] == n)       //�������б��������ͬ���͵�����
                        ++BCount;
                    else
                        RUbol = false;
                }
                if (LDbol == true)
                {
                    if (note[Arow + i, Acolumn - i] == n)   //�������б��������ͬ���͵�����
                        ++BCount;
                    else
                        LDbol = false;
                }
                if (BCount >= 5)                        //���ͬ���͵����Ӵ��ڵ���5
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

        //������Ϣ
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
            this.BeginInvoke(outdelegate, new object[] { Data, Ip, Port }); //�����й�
        }

        private delegate void DataArrivaldelegate(byte[] Data, IPAddress Ip, int Port);


        private void DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port) //�������ݵ����Ĵ������
        {
            try
            {
                //�����յ���Ϣת�����Զ��弯��ClassMsg
                ClassMsg msg = (ClassMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(Data));
                switch (msg.msgCommand)
                {
                    case MsgCommand.BeginJoin://���������ȡ�����û��б�
                        {

                            users = (ClassUsers)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));//��ȡ�����û���Ϣ
                            ClassUserInfo UserItem = new ClassUserInfo();
                            UserItem = users[0];
                            GIP = UserItem.UserIP;//��¼�û���IP��ַ
                            GPort = UserItem.UserPort;//��¼�˿ں�
                            Gem_N = UserItem.UserName;//��¼�û�����
                            Gem_F = Convert.ToInt32(UserItem.Fraction);//��ǰ����
                            Gem_C = Convert.ToInt32(UserItem.Caput);//ͷ��
                            Gem_S = Convert.ToInt32(UserItem.Sex);//�Ա�

                            ThreadStart ts = new ThreadStart(this.StartServer);
                            mainThread = new Thread(ts);
                            mainThread.Name = "GOB_Chess";
                            LineName = mainThread.Name;
                            mainThread.Start();
                            StartListen = true;
                            ConnHandle = 1;

                            label_Left.Text = Gem_N;//��ʾ�Է���ҵ�����
                            frmClient.Data_List(listView_Battle, Gem_N, Gem_F.ToString(), Gem_C.ToString());
                           
                            pictureBox_Left.Image = null;
                            if (Gem_S == 0) //����Է����������
                            {
                                pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");
                            }
                            else
                            {
                                pictureBox_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\Ů��.png");
                            }
                            //��ʾ�Է���ҵ���������
                            pictureBox_Q_Left.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\����.png");

                            this.pboxStart.Image = null;
                            this.pboxStart.Enabled = true;
                            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť.png");
                            break;
                        }
                    case MsgCommand.OddLittle:
                        {
                            rtxtChat.ReadOnly = true;//��Ϊֻ��
                            rtxtChat.ForeColor = Color.SlateGray;//����������ɫ
                            rtxtChat.AppendText(msg.UserName);//��ӷ�����Ϣ���û���
                            rtxtChat.AppendText("\r\n");//����
                            rtxtChat.AppendText(msg.MsgText);//��ӷ��͵���Ϣ
                            rtxtChat.AppendText("\r\n");//����
                            rtxtChat.ScrollToCaret();//����Ϣ��ӵ��ؼ�
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
                            this.pboxStart.Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��ʼ��ť��.png");
                            dropchild = true;

                            if (listener != null)
                                listener.Close();
                            if (mainThread != null)
                            {
                                Thread.Sleep(30);       //˯�����߳�
                                mainThread.Abort();         //�ر����߳�
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
                Thread.Sleep(30);       //˯�����߳�
                mainThread.Abort();         //�ر����߳�
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
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��С����ɫ.jpg");
                        break;
                    }
                case 2:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��󻯱�ɫ.jpg");
                        break;
                    }
                case 3:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\�رձ�ɫ.jpg");
                        break;
                    }
                case 4:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\���Ͱ�ť��ɫ.png");
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
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��С����ť.jpg");
                        break;
                    }
                case 2:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\��󻯰�ť.jpg");
                        break;
                    }
                case 3:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\�رհ�ť.jpg");
                        break;
                    }
                case 4:
                    {
                        ((PictureBox)sender).Image = null;
                        ((PictureBox)sender).Image = Image.FromFile(ClientClass.ImaDir + "\\Image\\���Ͱ�ť.png");
                        break;
                    }
            }
        }
        public ClassMsg OddtemMsg = new ClassMsg();
        public string OddText = "";
        //��ʾ������Ϣ
        private void pictureBox_Chart_Click(object sender, EventArgs e)
        {
            OddtemMsg.msgCommand = MsgCommand.OddLittle;
            OddtemMsg.UserName = Publec_Class.UserName;
            OddtemMsg.MsgText = comboBox_Hair.Text;
            if (GIP == "")
                return;
            udpSocket1.Send(IPAddress.Parse(GIP), 11001, new ClassSerializers().SerializeBinary(OddtemMsg).ToArray());

            rtxtChat.ReadOnly = true;//��Ϊֻ��
            rtxtChat.ForeColor = Color.SlateGray;//����������ɫ
            rtxtChat.AppendText(Publec_Class.UserName);//��ӷ�����Ϣ���û���
            rtxtChat.AppendText("\r\n");//����
            rtxtChat.AppendText(comboBox_Hair.Text);//��ӷ��͵���Ϣ
            rtxtChat.AppendText("\r\n");//����
            rtxtChat.ScrollToCaret();//����Ϣ��ӵ��ؼ�

            comboBox_Hair.Text = "";
        }

        //���»س���������������Ϣ
        private void comboBox_Hair_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                pictureBox_Chart_Click(sender, e);
            }
        }

    }
}