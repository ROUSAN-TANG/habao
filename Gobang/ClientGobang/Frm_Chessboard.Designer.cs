namespace ClientGobang
{
    partial class Frm_Chessboard
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Chessboard));
            this.panel_Title = new System.Windows.Forms.Panel();
            this.pictureBox_Min = new System.Windows.Forms.PictureBox();
            this.pictureBox_Close = new System.Windows.Forms.PictureBox();
            this.panel_All = new System.Windows.Forms.Panel();
            this.panel_Check = new System.Windows.Forms.Panel();
            this.pBox_Sign = new System.Windows.Forms.Panel();
            this.rtxtChat = new System.Windows.Forms.RichTextBox();
            this.label_F = new System.Windows.Forms.Label();
            this.label_N = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox_C = new System.Windows.Forms.PictureBox();
            this.label_Rname = new System.Windows.Forms.Label();
            this.label_Lname = new System.Windows.Forms.Label();
            this.pictureBox_Chart = new System.Windows.Forms.PictureBox();
            this.comboBox_Hair = new System.Windows.Forms.ComboBox();
            this.pictureBox_Q_Left = new System.Windows.Forms.PictureBox();
            this.pictureBox_Q_Right = new System.Windows.Forms.PictureBox();
            this.label_Left = new System.Windows.Forms.Label();
            this.label_Right = new System.Windows.Forms.Label();
            this.pictureBox_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_Left = new System.Windows.Forms.PictureBox();
            this.label_Fraction = new System.Windows.Forms.Label();
            this.label_Genre = new System.Windows.Forms.Label();
            this.listView_Battle = new System.Windows.Forms.ListView();
            this.pboxStart = new System.Windows.Forms.PictureBox();
            this.udpSocket1 = new GobangClass.UDPSocket(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Close)).BeginInit();
            this.panel_All.SuspendLayout();
            this.panel_Check.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_C)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Q_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Q_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxStart)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.BackColor = System.Drawing.Color.Transparent;
            this.panel_Title.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_Title.BackgroundImage")));
            this.panel_Title.Controls.Add(this.pictureBox_Min);
            this.panel_Title.Controls.Add(this.pictureBox_Close);
            this.panel_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Title.Location = new System.Drawing.Point(0, 0);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(945, 29);
            this.panel_Title.TabIndex = 0;
            this.panel_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_Title_MouseDown);
            this.panel_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_Title_MouseMove);
            // 
            // pictureBox_Min
            // 
            this.pictureBox_Min.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Min.Image")));
            this.pictureBox_Min.Location = new System.Drawing.Point(881, 6);
            this.pictureBox_Min.Name = "pictureBox_Min";
            this.pictureBox_Min.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_Min.TabIndex = 1;
            this.pictureBox_Min.TabStop = false;
            this.pictureBox_Min.Tag = "1";
            this.pictureBox_Min.Click += new System.EventHandler(this.pictureBox_Min_Click_1);
            this.pictureBox_Min.MouseEnter += new System.EventHandler(this.pictureBox_Min_MouseEnter);
            this.pictureBox_Min.MouseLeave += new System.EventHandler(this.pictureBox_Min_MouseLeave);
            // 
            // pictureBox_Close
            // 
            this.pictureBox_Close.BackColor = System.Drawing.SystemColors.Info;
            this.pictureBox_Close.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Close.Image")));
            this.pictureBox_Close.Location = new System.Drawing.Point(910, 6);
            this.pictureBox_Close.Name = "pictureBox_Close";
            this.pictureBox_Close.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_Close.TabIndex = 0;
            this.pictureBox_Close.TabStop = false;
            this.pictureBox_Close.Tag = "3";
            this.pictureBox_Close.Click += new System.EventHandler(this.pictureBox_Close_Click);
            this.pictureBox_Close.MouseEnter += new System.EventHandler(this.pictureBox_Min_MouseEnter);
            this.pictureBox_Close.MouseLeave += new System.EventHandler(this.pictureBox_Min_MouseLeave);
            // 
            // panel_All
            // 
            this.panel_All.BackColor = System.Drawing.Color.Transparent;
            this.panel_All.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_All.BackgroundImage")));
            this.panel_All.Controls.Add(this.panel_Check);
            this.panel_All.Controls.Add(this.rtxtChat);
            this.panel_All.Controls.Add(this.label_F);
            this.panel_All.Controls.Add(this.label_N);
            this.panel_All.Controls.Add(this.label2);
            this.panel_All.Controls.Add(this.label1);
            this.panel_All.Controls.Add(this.pictureBox_C);
            this.panel_All.Controls.Add(this.label_Rname);
            this.panel_All.Controls.Add(this.label_Lname);
            this.panel_All.Controls.Add(this.pictureBox_Chart);
            this.panel_All.Controls.Add(this.comboBox_Hair);
            this.panel_All.Controls.Add(this.pictureBox_Q_Left);
            this.panel_All.Controls.Add(this.pictureBox_Q_Right);
            this.panel_All.Controls.Add(this.label_Left);
            this.panel_All.Controls.Add(this.label_Right);
            this.panel_All.Controls.Add(this.pictureBox_Right);
            this.panel_All.Controls.Add(this.pictureBox_Left);
            this.panel_All.Controls.Add(this.label_Fraction);
            this.panel_All.Controls.Add(this.label_Genre);
            this.panel_All.Controls.Add(this.listView_Battle);
            this.panel_All.Controls.Add(this.pboxStart);
            this.panel_All.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_All.Location = new System.Drawing.Point(0, 29);
            this.panel_All.Name = "panel_All";
            this.panel_All.Size = new System.Drawing.Size(945, 650);
            this.panel_All.TabIndex = 1;
            // 
            // panel_Check
            // 
            this.panel_Check.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_Check.BackgroundImage")));
            this.panel_Check.Controls.Add(this.pBox_Sign);
            this.panel_Check.Location = new System.Drawing.Point(93, 41);
            this.panel_Check.Name = "panel_Check";
            this.panel_Check.Size = new System.Drawing.Size(538, 538);
            this.panel_Check.TabIndex = 0;
            this.panel_Check.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_Check_MouseDown);
            // 
            // pBox_Sign
            // 
            this.pBox_Sign.BackColor = System.Drawing.Color.Red;
            this.pBox_Sign.Location = new System.Drawing.Point(305, 290);
            this.pBox_Sign.Name = "pBox_Sign";
            this.pBox_Sign.Size = new System.Drawing.Size(8, 8);
            this.pBox_Sign.TabIndex = 0;
            this.pBox_Sign.Visible = false;
            // 
            // rtxtChat
            // 
            this.rtxtChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtChat.Location = new System.Drawing.Point(716, 324);
            this.rtxtChat.Name = "rtxtChat";
            this.rtxtChat.Size = new System.Drawing.Size(211, 273);
            this.rtxtChat.TabIndex = 21;
            this.rtxtChat.Text = "";
            // 
            // label_F
            // 
            this.label_F.AutoSize = true;
            this.label_F.Location = new System.Drawing.Point(851, 74);
            this.label_F.Name = "label_F";
            this.label_F.Size = new System.Drawing.Size(0, 12);
            this.label_F.TabIndex = 20;
            // 
            // label_N
            // 
            this.label_N.AutoSize = true;
            this.label_N.Location = new System.Drawing.Point(850, 48);
            this.label_N.Name = "label_N";
            this.label_N.Size = new System.Drawing.Size(0, 12);
            this.label_N.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(802, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "分  数：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(802, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "用户名：";
            // 
            // pictureBox_C
            // 
            this.pictureBox_C.Location = new System.Drawing.Point(742, 46);
            this.pictureBox_C.Name = "pictureBox_C";
            this.pictureBox_C.Size = new System.Drawing.Size(47, 77);
            this.pictureBox_C.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_C.TabIndex = 16;
            this.pictureBox_C.TabStop = false;
            // 
            // label_Rname
            // 
            this.label_Rname.AutoSize = true;
            this.label_Rname.Location = new System.Drawing.Point(29, 375);
            this.label_Rname.Name = "label_Rname";
            this.label_Rname.Size = new System.Drawing.Size(0, 12);
            this.label_Rname.TabIndex = 15;
            // 
            // label_Lname
            // 
            this.label_Lname.AutoSize = true;
            this.label_Lname.Location = new System.Drawing.Point(643, 375);
            this.label_Lname.Name = "label_Lname";
            this.label_Lname.Size = new System.Drawing.Size(0, 12);
            this.label_Lname.TabIndex = 14;
            // 
            // pictureBox_Chart
            // 
            this.pictureBox_Chart.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Chart.Image")));
            this.pictureBox_Chart.Location = new System.Drawing.Point(890, 602);
            this.pictureBox_Chart.Name = "pictureBox_Chart";
            this.pictureBox_Chart.Size = new System.Drawing.Size(35, 19);
            this.pictureBox_Chart.TabIndex = 13;
            this.pictureBox_Chart.TabStop = false;
            this.pictureBox_Chart.Tag = "4";
            this.pictureBox_Chart.Click += new System.EventHandler(this.pictureBox_Chart_Click);
            this.pictureBox_Chart.MouseEnter += new System.EventHandler(this.pictureBox_Min_MouseEnter);
            this.pictureBox_Chart.MouseLeave += new System.EventHandler(this.pictureBox_Min_MouseLeave);
            // 
            // comboBox_Hair
            // 
            this.comboBox_Hair.FormattingEnabled = true;
            this.comboBox_Hair.Items.AddRange(new object[] {
            "我等的花儿也谢了。",
            "你下的太好了，让让我吧"});
            this.comboBox_Hair.Location = new System.Drawing.Point(714, 602);
            this.comboBox_Hair.Name = "comboBox_Hair";
            this.comboBox_Hair.Size = new System.Drawing.Size(173, 20);
            this.comboBox_Hair.TabIndex = 12;
            this.comboBox_Hair.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_Hair_KeyPress);
            // 
            // pictureBox_Q_Left
            // 
            this.pictureBox_Q_Left.Location = new System.Drawing.Point(32, 221);
            this.pictureBox_Q_Left.Name = "pictureBox_Q_Left";
            this.pictureBox_Q_Left.Size = new System.Drawing.Size(40, 37);
            this.pictureBox_Q_Left.TabIndex = 11;
            this.pictureBox_Q_Left.TabStop = false;
            // 
            // pictureBox_Q_Right
            // 
            this.pictureBox_Q_Right.Location = new System.Drawing.Point(644, 221);
            this.pictureBox_Q_Right.Name = "pictureBox_Q_Right";
            this.pictureBox_Q_Right.Size = new System.Drawing.Size(40, 37);
            this.pictureBox_Q_Right.TabIndex = 10;
            this.pictureBox_Q_Right.TabStop = false;
            // 
            // label_Left
            // 
            this.label_Left.AutoSize = true;
            this.label_Left.Location = new System.Drawing.Point(21, 378);
            this.label_Left.Name = "label_Left";
            this.label_Left.Size = new System.Drawing.Size(0, 12);
            this.label_Left.TabIndex = 9;
            // 
            // label_Right
            // 
            this.label_Right.AutoSize = true;
            this.label_Right.Location = new System.Drawing.Point(637, 378);
            this.label_Right.Name = "label_Right";
            this.label_Right.Size = new System.Drawing.Size(0, 12);
            this.label_Right.TabIndex = 8;
            // 
            // pictureBox_Right
            // 
            this.pictureBox_Right.Location = new System.Drawing.Point(642, 277);
            this.pictureBox_Right.Name = "pictureBox_Right";
            this.pictureBox_Right.Size = new System.Drawing.Size(47, 77);
            this.pictureBox_Right.TabIndex = 7;
            this.pictureBox_Right.TabStop = false;
            // 
            // pictureBox_Left
            // 
            this.pictureBox_Left.Location = new System.Drawing.Point(29, 277);
            this.pictureBox_Left.Name = "pictureBox_Left";
            this.pictureBox_Left.Size = new System.Drawing.Size(47, 77);
            this.pictureBox_Left.TabIndex = 6;
            this.pictureBox_Left.TabStop = false;
            // 
            // label_Fraction
            // 
            this.label_Fraction.AutoSize = true;
            this.label_Fraction.Location = new System.Drawing.Point(803, 106);
            this.label_Fraction.Name = "label_Fraction";
            this.label_Fraction.Size = new System.Drawing.Size(65, 12);
            this.label_Fraction.TabIndex = 4;
            this.label_Fraction.Text = "棋子类型：";
            // 
            // label_Genre
            // 
            this.label_Genre.AutoSize = true;
            this.label_Genre.Location = new System.Drawing.Point(869, 106);
            this.label_Genre.Name = "label_Genre";
            this.label_Genre.Size = new System.Drawing.Size(0, 12);
            this.label_Genre.TabIndex = 3;
            // 
            // listView_Battle
            // 
            this.listView_Battle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView_Battle.Location = new System.Drawing.Point(718, 195);
            this.listView_Battle.Name = "listView_Battle";
            this.listView_Battle.Size = new System.Drawing.Size(207, 115);
            this.listView_Battle.TabIndex = 2;
            this.listView_Battle.UseCompatibleStateImageBehavior = false;
            // 
            // pboxStart
            // 
            this.pboxStart.BackColor = System.Drawing.Color.Transparent;
            this.pboxStart.Image = ((System.Drawing.Image)(resources.GetObject("pboxStart.Image")));
            this.pboxStart.Location = new System.Drawing.Point(314, 588);
            this.pboxStart.Name = "pboxStart";
            this.pboxStart.Size = new System.Drawing.Size(101, 34);
            this.pboxStart.TabIndex = 1;
            this.pboxStart.TabStop = false;
            this.pboxStart.Click += new System.EventHandler(this.pboxStart_Click);
            // 
            // udpSocket1
            // 
            this.udpSocket1.Active = false;
            this.udpSocket1.LocalHost = "127.0.0.1";
            this.udpSocket1.LocalPort = 11000;
            this.udpSocket1.DataArrival += new GobangClass.UDPSocket.DataArrivalEventHandler(this.udpSocket1_DataArrival);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "黑棋子.png");
            this.imageList1.Images.SetKeyName(1, "白棋子.png");
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "头像1.png");
            this.imageList2.Images.SetKeyName(1, "头像2.png");
            this.imageList2.Images.SetKeyName(2, "头像3.png");
            this.imageList2.Images.SetKeyName(3, "头像4.png");
            this.imageList2.Images.SetKeyName(4, "头像5.png");
            this.imageList2.Images.SetKeyName(5, "头像6.png");
            this.imageList2.Images.SetKeyName(6, "无人时的问号.png");
            // 
            // Frm_Chessboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 679);
            this.Controls.Add(this.panel_All);
            this.Controls.Add(this.panel_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Chessboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_Chessboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Chessboard_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Chessboard_Load);
            this.Shown += new System.EventHandler(this.Frm_Chessboard_Shown);
            this.panel_Title.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Close)).EndInit();
            this.panel_All.ResumeLayout(false);
            this.panel_All.PerformLayout();
            this.panel_Check.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_C)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Q_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Q_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxStart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.Panel panel_All;
        private System.Windows.Forms.PictureBox pictureBox_Close;
        private System.Windows.Forms.PictureBox pboxStart;
        private GobangClass.UDPSocket udpSocket1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listView_Battle;
        private System.Windows.Forms.Label label_Fraction;
        private System.Windows.Forms.Label label_Genre;
        private System.Windows.Forms.PictureBox pictureBox_Q_Left;
        private System.Windows.Forms.PictureBox pictureBox_Q_Right;
        private System.Windows.Forms.Label label_Left;
        private System.Windows.Forms.Label label_Right;
        private System.Windows.Forms.PictureBox pictureBox_Right;
        private System.Windows.Forms.PictureBox pictureBox_Left;
        private System.Windows.Forms.PictureBox pictureBox_Min;
        private System.Windows.Forms.PictureBox pictureBox_Chart;
        private System.Windows.Forms.ComboBox comboBox_Hair;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label label_Rname;
        private System.Windows.Forms.Label label_Lname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox_C;
        private System.Windows.Forms.Label label_F;
        private System.Windows.Forms.Label label_N;
        private System.Windows.Forms.RichTextBox rtxtChat;
        public System.Windows.Forms.Panel panel_Check;
        public System.Windows.Forms.Panel pBox_Sign;
    }
}