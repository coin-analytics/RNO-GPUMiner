// Decompiled with JetBrains decompiler
// Type: coinminner.Minner
// Assembly: rnocoinminer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D88D9D4B-A690-412D-B858-EE24C0C62E1A
// Assembly location: C:\Program Files (x86)\RNO\RnoMiner(Beta)\rnocoinminer.exe

using coinminner.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace coinminner
{
    public class Minner : Form
    {
        private string domain = "http://rnoapi.com";
        private int cpuCount = Environment.ProcessorCount;
        private int nonce = 1;
        private Point mousePoint;
        private int nBits;
        private string nBitsStr;
        private string hashText;
        private int threadState;
        private int runMinner;
        private int runcState;
        private double minedCoin;
        private JObject walletInfo;
        private Thread[] syncThread;
        private Thread[] runThread;
        private transaction tsForm;
        private string LogBoxStr;
        private IContainer components;
        private Label label3;
        private TextBox WalletTextBox;
        private Label label2;
        private Button button1;
        private Label label1;
        private Label label4;
        private Label label5;
        private Label MiningLevelLabel;
        private Label WeightLabel;
        private Label TotalHashRateLabel;
        private Label label6;
        private ComboBox ProcessCountBox;
        private TextBox LogBox;
        private NotifyIcon notifyIcon1;
        private Button button2;
        private PictureBox pictureBox3;
        private Label label8;
        private Button button3;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private PictureBox pictureBox1;
        private Label label7;
        //[DllImport("dllsha256.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("dllsha256.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static void sha256_crypt(string input, string output);

        [DllImport("dllsha256.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static void sha256_init(int user_kpc);
        public Minner(string miningWallet, string initData)
        {
            this.InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback)((sender, certificate, chain, sslPolicyErrors) => true);
            this.WalletTextBox.Text = miningWallet;
            for (int index = 1; index <= this.cpuCount; ++index)
                this.ProcessCountBox.Items.Add((object)index);
            this.ProcessCountBox.SelectedIndex = this.cpuCount - 1;
            this.setMinner();
        }

        public void setMinner()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string text = this.WalletTextBox.Text;
                stringBuilder.Append("miningWallet=" + text);
                byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(this.domain + "/api/syncCoin"));
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                string end = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.Default).ReadToEnd();
                if (!(JObject.Parse(end)["result"].ToString() == "True"))
                    return;
                this.walletInfo = JObject.Parse(end);
                this.TotalHashRateLabel.Text = this.walletInfo["msg"][(object)"totalHashRate"].ToString();
                this.MiningLevelLabel.Text = this.walletInfo["msg"][(object)"miningLevel"].ToString();
                this.WeightLabel.Text = this.walletInfo["msg"][(object)"weight"].ToString();
                this.label10.Text = this.walletInfo["msg"][(object)"hashRate"].ToString();
                this.nBits = int.Parse(this.walletInfo["msg"][(object)"nBits"].ToString());
                this.hashText = this.walletInfo["msg"][(object)"hashText"].ToString();
                this.nBitsStr = "";
                for (int index = 0; index < this.nBits; ++index)
                    this.nBitsStr += "0";
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("서버 연결에 실패 하였습니다. 재시도 해주세요");
            }
        }

        private void Minner_MouseDown(object sender, MouseEventArgs e)
        {
            this.mousePoint = new Point(e.X, e.Y);
        }

        private void Minner_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            this.Location = new Point(this.Left - (this.mousePoint.X - e.X), this.Top - (this.mousePoint.Y - e.Y));
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Minner_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.label9.Parent = (Control)this.pictureBox1;
            this.label12.Parent = (Control)this.pictureBox1;
            this.tsForm = new transaction();
        }

        [DllImport("Gdi32.dll")]

        private static extern IntPtr CreateRoundRectRgn(
          int nLeftRect,
          int nTopRect,
          int nRightRect,
          int nBottomRect,
          int nWidthEllipse,
          int nHeightEllipse);

        public string SHA256Hash(string data)
        {
            //byte[] hash = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(data));
            string output = null;
            sha256_crypt(data, output);
            byte[] hash = Encoding.ASCII.GetBytes(output);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.AppendFormat("{0:x2}", (object)num);
            return stringBuilder.ToString();
        }

        public void runC()
        {
            do
                ;
            while (this.runcState == 1);
        }

        public void mining()
        {
            sha256_init(2048);


            while (this.threadState == 1)
            {
                if (this.runMinner != 1)
                    break;
                try
                {
                    int nonce = this.nonce;
                    int nBits = this.nBits;
                    string nBitsStr = this.nBitsStr;
                    string hashText = this.hashText;
                    string str = this.SHA256Hash(hashText + (object)nonce);
                    if (str.StartsWith(nBitsStr))
                    {
                        this.runMinner = 0;
                        StringBuilder stringBuilder = new StringBuilder();
                        string text = this.WalletTextBox.Text;
                        stringBuilder.Append("miningWallet=" + text);
                        stringBuilder.Append("&createdHashString=" + str);
                        stringBuilder.Append("&nonce=" + (object)nonce);
                        stringBuilder.Append("&hashText=" + hashText);
                        byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(this.domain + "/api/minedCoin"));
                        httpWebRequest.Method = "POST";
                        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = httpWebRequest.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        JObject jobject1 = JObject.Parse(new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.Default).ReadToEnd());
                        JObject jobject2 = JObject.Parse(jobject1["msg"].ToString());
                        if (jobject1["result"].ToString() == "True")
                        {
                            this.updateMinedCoin(jobject2["coins"].ToString());
                            this.LogBoxStr = this.LogBoxStr + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "\r\n" + jobject2["hash"].ToString() + " " + jobject2["coins"].ToString() + "\r\n";
                            this.tsForm.setLog(this.LogBoxStr);
                        }
                        this.resetMinner();
                        break;
                    }
                    ++this.nonce;
                }
                catch (Exception ex)
                {
                    this.runMinner = 1;
                }
            }
        }

        public void updateMinedCoin(string coins)
        {
            this.minedCoin += double.Parse(coins);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.threadState = 1;
            this.runcState = 1;
            this.ProcessCountBox.Enabled = false;
            this.button1.Visible = false;
            this.button3.Visible = true;
            this.resetMinner();
            this.runMinner = 1;
            int length = 0;
            int num = int.Parse(this.ProcessCountBox.SelectedItem.ToString());
            if (num >= 2)
                length = num;
            this.runThread = new Thread[length];
            for (int index = 0; index < length; ++index)
            {
                this.runThread[index] = new Thread(new ThreadStart(this.runC));
                this.runThread[index].IsBackground = true;
                this.runThread[index].Start();
            }
        }

        public void threadUp()
        {
            this.runMinner = 1;
            int length = int.Parse(this.ProcessCountBox.SelectedItem.ToString());
            this.syncThread = new Thread[length];
            this.runThread = new Thread[length];
            for (int index = 0; index < length; ++index)
            {
                this.syncThread[index] = new Thread(new ThreadStart(this.mining));
                this.syncThread[index].IsBackground = true;
                this.syncThread[index].Start();
            }
        }

        public void resetMinner()
        {
            this.runMinner = 0;
            this.setMinner();
            this.threadUp();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowIcon = true;
            this.notifyIcon1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowIcon = false;
            this.notifyIcon1.Visible = true;
        }

        private void Minner_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                return;
            this.Visible = false;
            this.ShowIcon = false;
            this.notifyIcon1.Visible = true;
        }

        private void Minner_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.threadState = 0;
            Application.Exit();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            if (!this.tsForm.IsDisposed)
                this.tsForm.Dispose();
            Application.Exit();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.threadState = 0;
            this.runcState = 0;
            this.ProcessCountBox.Enabled = true;
            this.button1.Visible = true;
            this.button3.Visible = false;
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void label12_Click_1(object sender, EventArgs e)
        {
            this.tsForm.Show();
        }

        private void ProcessCountBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void threaddd_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Process.Start("https://cafe.naver.com/RNOCOINFAMILY");
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Minner));
            this.label3 = new Label();
            this.WalletTextBox = new TextBox();
            this.label2 = new Label();
            this.button1 = new Button();
            this.label1 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.MiningLevelLabel = new Label();
            this.WeightLabel = new Label();
            this.TotalHashRateLabel = new Label();
            this.label6 = new Label();
            this.ProcessCountBox = new ComboBox();
            this.notifyIcon1 = new NotifyIcon(this.components);
            this.button2 = new Button();
            this.label8 = new Label();
            this.button3 = new Button();
            this.label9 = new Label();
            this.label10 = new Label();
            this.label11 = new Label();
            this.label12 = new Label();
            this.pictureBox1 = new PictureBox();
            this.pictureBox3 = new PictureBox();
            this.label7 = new Label();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            ((ISupportInitialize)this.pictureBox3).BeginInit();
            this.SuspendLayout();
            this.label3.AutoSize = true;
            this.label3.Font = new Font("맑은 고딕", 7.875f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label3.ForeColor = SystemColors.ControlDarkDark;
            this.label3.Location = new Point(959, 66);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(116, 30);
            this.label3.TabIndex = 14;
            this.label3.Text = "version 1.2";
            this.WalletTextBox.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.WalletTextBox.Location = new Point(590, 179);
            this.WalletTextBox.Margin = new Padding(4);
            this.WalletTextBox.Name = "WalletTextBox";
            this.WalletTextBox.ReadOnly = true;
            this.WalletTextBox.Size = new Size(489, 39);
            this.WalletTextBox.TabIndex = 15;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label2.ForeColor = SystemColors.ControlDarkDark;
            this.label2.Location = new Point(584, (int)sbyte.MaxValue);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(251, 32);
            this.label2.TabIndex = 16;
            this.label2.Text = "Mining wallet address";
            this.button1.BackColor = Color.Black;
            this.button1.FlatStyle = FlatStyle.Popup;
            this.button1.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.button1.ForeColor = Color.White;
            this.button1.Location = new Point(590, 627);
            this.button1.Name = "button1";
            this.button1.Size = new Size(189, 48);
            this.button1.TabIndex = 17;
            this.button1.Text = "채굴시작";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label1.ForeColor = SystemColors.ControlDarkDark;
            this.label1.Location = new Point(584, 253);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(149, 32);
            this.label1.TabIndex = 18;
            this.label1.Text = "Mining level";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label4.ForeColor = SystemColors.ControlDarkDark;
            this.label4.Location = new Point(584, 380);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(143, 32);
            this.label4.TabIndex = 19;
            this.label4.Text = "채굴 가중치";
            this.label5.AutoSize = true;
            this.label5.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label5.ForeColor = SystemColors.ControlDarkDark;
            this.label5.Location = new Point(584, 295);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(170, 32);
            this.label5.TabIndex = 20;
            this.label5.Text = "Total Hashrate";
            this.MiningLevelLabel.AutoSize = true;
            this.MiningLevelLabel.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.MiningLevelLabel.ForeColor = SystemColors.ControlDarkDark;
            this.MiningLevelLabel.Location = new Point(808, 253);
            this.MiningLevelLabel.Margin = new Padding(4, 0, 4, 0);
            this.MiningLevelLabel.Name = "MiningLevelLabel";
            this.MiningLevelLabel.Size = new Size(149, 32);
            this.MiningLevelLabel.TabIndex = 21;
            this.MiningLevelLabel.Text = "Mining level";
            this.WeightLabel.AutoSize = true;
            this.WeightLabel.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.WeightLabel.ForeColor = SystemColors.ControlDarkDark;
            this.WeightLabel.Location = new Point(808, 380);
            this.WeightLabel.Margin = new Padding(4, 0, 4, 0);
            this.WeightLabel.Name = "WeightLabel";
            this.WeightLabel.Size = new Size(143, 32);
            this.WeightLabel.TabIndex = 22;
            this.WeightLabel.Text = "나의 가중치";
            this.TotalHashRateLabel.AutoSize = true;
            this.TotalHashRateLabel.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.TotalHashRateLabel.ForeColor = SystemColors.ControlDarkDark;
            this.TotalHashRateLabel.Location = new Point(808, 295);
            this.TotalHashRateLabel.Margin = new Padding(4, 0, 4, 0);
            this.TotalHashRateLabel.Name = "TotalHashRateLabel";
            this.TotalHashRateLabel.Size = new Size(170, 32);
            this.TotalHashRateLabel.TabIndex = 23;
            this.TotalHashRateLabel.Text = "Total Hashrate";
            this.label6.AutoSize = true;
            this.label6.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label6.ForeColor = SystemColors.ControlDarkDark;
            this.label6.Location = new Point(584, 430);
            this.label6.Margin = new Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(223, 32);
            this.label6.TabIndex = 24;
            this.label6.Text = "채굴 프로세스 갯수";
            this.ProcessCountBox.FormattingEnabled = true;
            this.ProcessCountBox.Location = new Point(814, 432);
            this.ProcessCountBox.Name = "ProcessCountBox";
            this.ProcessCountBox.Size = new Size(121, 32);
            this.ProcessCountBox.TabIndex = 25;
            this.ProcessCountBox.KeyDown += new KeyEventHandler(this.ProcessCountBox_KeyDown);
            this.notifyIcon1.Icon = (Icon)componentResourceManager.GetObject("notifyIcon1.Icon");
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new EventHandler(this.notifyIcon1_DoubleClick);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Font = new Font("맑은 고딕", 7.875f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.button2.Location = new Point(886, 627);
            this.button2.Name = "button2";
            this.button2.Size = new Size(189, 48);
            this.button2.TabIndex = 27;
            this.button2.Text = "창 내리기";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Yu Gothic UI", 16.125f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.label8.Location = new Point(700, 44);
            this.label8.Margin = new Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new Size(237, 59);
            this.label8.TabIndex = 33;
            this.label8.Text = "RNO Miner";
            this.button3.BackColor = Color.Black;
            this.button3.FlatStyle = FlatStyle.Popup;
            this.button3.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.button3.ForeColor = Color.White;
            this.button3.Location = new Point(590, 627);
            this.button3.Name = "button3";
            this.button3.Size = new Size(189, 48);
            this.button3.TabIndex = 34;
            this.button3.Text = "채굴중지";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new EventHandler(this.button3_Click_1);
            this.label9.AutoSize = true;
            this.label9.BackColor = Color.Transparent;
            this.label9.Cursor = Cursors.Hand;
            this.label9.Font = new Font("Segoe UI Emoji", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.label9.ForeColor = Color.Black;
            this.label9.Location = new Point(168, 513);
            this.label9.Margin = new Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new Size(241, 35);
            this.label9.TabIndex = 35;
            this.label9.Text = " www.Rnocoin.com ";
            this.label9.Click += new EventHandler(this.label9_Click);
            this.label10.AutoSize = true;
            this.label10.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label10.ForeColor = SystemColors.ControlDarkDark;
            this.label10.Location = new Point(808, 337);
            this.label10.Margin = new Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new Size(142, 32);
            this.label10.TabIndex = 37;
            this.label10.Text = "Pc Hashrate";
            this.label11.AutoSize = true;
            this.label11.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label11.ForeColor = SystemColors.ControlDarkDark;
            this.label11.Location = new Point(584, 337);
            this.label11.Margin = new Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new Size(142, 32);
            this.label11.TabIndex = 36;
            this.label11.Text = "Pc Hashrate";
            this.label11.Click += new EventHandler(this.label11_Click);
            this.label12.AutoSize = true;
            this.label12.BackColor = Color.Transparent;
            this.label12.Cursor = Cursors.Hand;
            this.label12.Font = new Font("맑은 고딕", 7.875f, FontStyle.Regular, GraphicsUnit.Point, (byte)129);
            this.label12.ForeColor = SystemColors.MenuHighlight;
            this.label12.Location = new Point(203, 557);
            this.label12.Margin = new Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new Size(167, 30);
            this.label12.TabIndex = 41;
            this.label12.Text = "마이닝 로그보기";
            this.label12.Click += new EventHandler(this.label12_Click_1);
            this.pictureBox1.BackgroundImage = (Image)Resources.KakaoTalk_20190422_213551232;
            this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = (Image)componentResourceManager.GetObject("pictureBox1.ErrorImage");
            this.pictureBox1.InitialImage = (Image)componentResourceManager.GetObject("pictureBox1.InitialImage");
            this.pictureBox1.Location = new Point(-1, 1);
            this.pictureBox1.Margin = new Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(546, 690);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            this.pictureBox3.BackgroundImage = (Image)Resources.closeBtn1;
            this.pictureBox3.BackgroundImageLayout = ImageLayout.Stretch;
            this.pictureBox3.Cursor = Cursors.Hand;
            this.pictureBox3.Location = new Point(1068, 0);
            this.pictureBox3.Margin = new Padding(6);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Size(55, 52);
            this.pictureBox3.TabIndex = 31;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new EventHandler(this.pictureBox3_Click_1);
            this.label7.AutoSize = true;
            this.label7.BackColor = Color.Transparent;
            this.label7.Cursor = Cursors.Hand;
            this.label7.Font = new Font("Segoe UI Emoji", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.label7.ForeColor = Color.Black;
            this.label7.Location = new Point(584, 535);
            this.label7.Margin = new Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(336, 35);
            this.label7.TabIndex = 42;
            this.label7.Text = "알노코인 공식 카페 바로가기";
            this.label7.Click += new EventHandler(this.label7_Click);
            this.AutoScaleDimensions = new SizeF(13f, 24f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1126, 687);
            this.Controls.Add((Control)this.label7);
            this.Controls.Add((Control)this.label12);
            this.Controls.Add((Control)this.label10);
            this.Controls.Add((Control)this.label11);
            this.Controls.Add((Control)this.label9);
            this.Controls.Add((Control)this.button3);
            this.Controls.Add((Control)this.label8);
            this.Controls.Add((Control)this.pictureBox1);
            this.Controls.Add((Control)this.pictureBox3);
            this.Controls.Add((Control)this.button2);
            this.Controls.Add((Control)this.ProcessCountBox);
            this.Controls.Add((Control)this.label6);
            this.Controls.Add((Control)this.TotalHashRateLabel);
            this.Controls.Add((Control)this.WeightLabel);
            this.Controls.Add((Control)this.MiningLevelLabel);
            this.Controls.Add((Control)this.label5);
            this.Controls.Add((Control)this.label4);
            this.Controls.Add((Control)this.label1);
            this.Controls.Add((Control)this.button1);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.WalletTextBox);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Margin = new Padding(6);
            this.Name = nameof(Minner);
            this.Text = nameof(Minner);
            this.FormClosed += new FormClosedEventHandler(this.Minner_FormClosed);
            this.Load += new EventHandler(this.Minner_Load);
            this.MouseDown += new MouseEventHandler(this.Minner_MouseDown);
            this.MouseMove += new MouseEventHandler(this.Minner_MouseMove);
            this.Resize += new EventHandler(this.Minner_Resize);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            ((ISupportInitialize)this.pictureBox3).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
