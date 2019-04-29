// Decompiled with JetBrains decompiler
// Type: coinminner.CoinMinner
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
using System.Text;
using System.Windows.Forms;

namespace coinminner
{
  public class CoinMinner : Form
  {
    private string domain = "http://rnoapi.com";
    private Point mousePoint;
    private HttpWebRequest wReq;
    private Stream PostDataStream;
    private Stream respPostStream;
    private StreamReader readerPost;
    private HttpWebResponse wResp;
    private IContainer components;
    private PictureBox pictureBox1;
    private TextBox WalletTextBox;
    private Label label2;
    private Label label3;
    private Button ConnectButton;
    private Label label1;
    private Label label4;
    private Label label5;
    private Label errorLabel;
    private PictureBox pictureBox2;
    private Label label6;
    private PictureBox pictureBox3;
    private Label label7;

    public CoinMinner()
    {
      this.InitializeComponent();
      ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.label7.Parent = (Control) this.pictureBox1;
      StringBuilder stringBuilder = new StringBuilder();
      string text = this.WalletTextBox.Text;
      stringBuilder.Append("miningWallet=" + text);
      byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
      this.wReq = (HttpWebRequest) WebRequest.Create(new Uri(this.domain + "/api/versionCheck"));
      this.wReq.Method = "POST";
      this.wReq.ContentType = "application/x-www-form-urlencoded";
      this.wReq.ContentLength = (long) bytes.Length;
      this.PostDataStream = this.wReq.GetRequestStream();
      this.PostDataStream.Write(bytes, 0, bytes.Length);
      this.PostDataStream.Close();
      this.wResp = (HttpWebResponse) this.wReq.GetResponse();
      this.respPostStream = this.wResp.GetResponseStream();
      this.readerPost = new StreamReader(this.respPostStream, Encoding.Default);
      JObject jobject = JObject.Parse(this.readerPost.ReadToEnd());
      if (!(jobject["result"].ToString() == "True") || !(jobject["msg"][(object) "programVersion"].ToString() != "1.2"))
        return;
      int num = (int) MessageBox.Show("프로그램이 업데이트 되었습니다. 새로운 버젼을 다운로드 해주세요.");
      Application.Exit();
    }

    private void Form1_MouseDown(object sender, MouseEventArgs e)
    {
      this.mousePoint = new Point(e.X, e.Y);
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
        return;
      this.Location = new Point(this.Left - (this.mousePoint.X - e.X), this.Top - (this.mousePoint.Y - e.Y));
    }

    private void label3_Click(object sender, EventArgs e)
    {
    }

    private void ConnectButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.WalletTextBox.Text.Length == 0)
        {
          this.errorLabel.Text = "마이닝 지갑 주소를 입력 해주세요.";
          this.WalletTextBox.Select();
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder();
          string text = this.WalletTextBox.Text;
          stringBuilder.Append("miningWallet=" + text);
          byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
          this.wReq = (HttpWebRequest) WebRequest.Create(new Uri(this.domain + "/api/syncCoin"));
          this.wReq.Method = "POST";
          this.wReq.ContentType = "application/x-www-form-urlencoded";
          this.wReq.ContentLength = (long) bytes.Length;
          this.PostDataStream = this.wReq.GetRequestStream();
          this.PostDataStream.Write(bytes, 0, bytes.Length);
          this.PostDataStream.Close();
          this.wResp = (HttpWebResponse) this.wReq.GetResponse();
          this.respPostStream = this.wResp.GetResponseStream();
          this.readerPost = new StreamReader(this.respPostStream, Encoding.Default);
          string end = this.readerPost.ReadToEnd();
          if (JObject.Parse(end)["result"].ToString() == "True")
          {
            Minner minner = new Minner(text, end);
            this.Hide();
            minner.Show();
          }
          else
            this.errorLabel.Text = "유효하지 않은 마이닝 지갑 입니다.";
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("서버 연결에 실패 하였습니다. 재시도 해주세요");
      }
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
    }

    private void errorLabel_Click(object sender, EventArgs e)
    {
    }

    private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
    {
      this.mousePoint = new Point(e.X, e.Y);
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
        return;
      this.Location = new Point(this.Left - (this.mousePoint.X - e.X), this.Top - (this.mousePoint.Y - e.Y));
    }

    private void label6_Click(object sender, EventArgs e)
    {
      Process.Start(this.domain + "/wallet");
    }

    private void pictureBox3_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void label7_Click(object sender, EventArgs e)
    {
      Process.Start(this.domain);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CoinMinner));
      this.WalletTextBox = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.ConnectButton = new Button();
      this.label1 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.errorLabel = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.pictureBox3 = new PictureBox();
      this.pictureBox2 = new PictureBox();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox3).BeginInit();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.WalletTextBox.BorderStyle = BorderStyle.FixedSingle;
      this.WalletTextBox.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.WalletTextBox.Location = new Point(557, 328);
      this.WalletTextBox.Margin = new Padding(4);
      this.WalletTextBox.Name = "WalletTextBox";
      this.WalletTextBox.Size = new Size(401, 39);
      this.WalletTextBox.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("맑은 고딕", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 129);
      this.label2.ForeColor = SystemColors.ControlDarkDark;
      this.label2.Location = new Point(552, 292);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(251, 32);
      this.label2.TabIndex = 3;
      this.label2.Text = "Mining wallet address";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("맑은 고딕", 7.875f, FontStyle.Regular, GraphicsUnit.Point, (byte) 129);
      this.label3.ForeColor = SystemColors.ControlDarkDark;
      this.label3.Location = new Point(925, 642);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(177, 30);
      this.label3.TabIndex = 4;
      this.label3.Text = "Miner version 1.2";
      this.label3.Click += new EventHandler(this.label3_Click);
      this.ConnectButton.BackColor = Color.Black;
      this.ConnectButton.FlatAppearance.BorderSize = 0;
      this.ConnectButton.FlatStyle = FlatStyle.Flat;
      this.ConnectButton.Font = new Font("맑은 고딕", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 129);
      this.ConnectButton.ForeColor = Color.White;
      this.ConnectButton.Location = new Point(557, 442);
      this.ConnectButton.Margin = new Padding(4);
      this.ConnectButton.Name = "ConnectButton";
      this.ConnectButton.Size = new Size(171, 56);
      this.ConnectButton.TabIndex = 5;
      this.ConnectButton.Text = "Connect";
      this.ConnectButton.UseVisualStyleBackColor = false;
      this.ConnectButton.Click += new EventHandler(this.ConnectButton_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("맑은 고딕", 16.125f, FontStyle.Bold, GraphicsUnit.Point, (byte) 129);
      this.label1.Location = new Point(542, 154);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(559, 59);
      this.label1.TabIndex = 1;
      this.label1.Text = "Connect into RNO Mining";
      this.label4.AutoSize = true;
      this.label4.Cursor = Cursors.Default;
      this.label4.Font = new Font("맑은 고딕", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 129);
      this.label4.ForeColor = SystemColors.ControlDarkDark;
      this.label4.Location = new Point(555, 514);
      this.label4.Margin = new Padding(6, 0, 6, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(356, 30);
      this.label4.TabIndex = 6;
      this.label4.Text = "Forgot the mining wallet address?";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("맑은 고딕", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 129);
      this.label5.ForeColor = Color.White;
      this.label5.Location = new Point(104, 352);
      this.label5.Margin = new Padding(6, 0, 6, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(0, 45);
      this.label5.TabIndex = 8;
      this.errorLabel.AutoSize = true;
      this.errorLabel.Cursor = Cursors.Hand;
      this.errorLabel.Font = new Font("맑은 고딕", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 129);
      this.errorLabel.ForeColor = Color.Crimson;
      this.errorLabel.Location = new Point(552, 388);
      this.errorLabel.Margin = new Padding(6, 0, 6, 0);
      this.errorLabel.Name = "errorLabel";
      this.errorLabel.Size = new Size(0, 30);
      this.errorLabel.TabIndex = 9;
      this.errorLabel.Click += new EventHandler(this.errorLabel_Click);
      this.label6.AutoSize = true;
      this.label6.Cursor = Cursors.Hand;
      this.label6.Font = new Font("맑은 고딕", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 129);
      this.label6.ForeColor = Color.MediumBlue;
      this.label6.Location = new Point(895, 514);
      this.label6.Margin = new Padding(6, 0, 6, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(58, 30);
      this.label6.TabIndex = 11;
      this.label6.Text = "Click";
      this.label6.Click += new EventHandler(this.label6_Click);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Cursor = Cursors.Hand;
      this.label7.Font = new Font("Segoe UI Emoji", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.ForeColor = Color.White;
      this.label7.Location = new Point(175, 512);
      this.label7.Margin = new Padding(6, 0, 6, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(241, 35);
      this.label7.TabIndex = 13;
      this.label7.Text = " www.Rnocoin.com ";
      this.label7.Click += new EventHandler(this.label7_Click);
      this.pictureBox3.BackgroundImage = (Image) Resources.closeBtn1;
      this.pictureBox3.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox3.Cursor = Cursors.Hand;
      this.pictureBox3.Location = new Point(1058, 6);
      this.pictureBox3.Margin = new Padding(6);
      this.pictureBox3.Name = "pictureBox3";
      this.pictureBox3.Size = new Size(55, 54);
      this.pictureBox3.TabIndex = 12;
      this.pictureBox3.TabStop = false;
      this.pictureBox3.Click += new EventHandler(this.pictureBox3_Click);
      this.pictureBox2.BackgroundImage = (Image) Resources.walletIco;
      this.pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox2.Location = new Point(960, 320);
      this.pictureBox2.Margin = new Padding(6);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(54, 50);
      this.pictureBox2.TabIndex = 10;
      this.pictureBox2.TabStop = false;
      this.pictureBox1.BackgroundImage = (Image) Resources.KakaoTalk_20190422_203135019;
      this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox1.Location = new Point(-11, -2);
      this.pictureBox1.Margin = new Padding(4);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(546, 690);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
      this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
      this.pictureBox1.MouseMove += new MouseEventHandler(this.pictureBox1_MouseMove);
      this.AutoScaleDimensions = new SizeF(13f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(1122, 686);
      this.ControlBox = false;
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.pictureBox3);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.pictureBox2);
      this.Controls.Add((Control) this.errorLabel);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.ConnectButton);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.WalletTextBox);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Cursor = Cursors.Default;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (CoinMinner);
      this.Text = "Form1";
      this.Load += new EventHandler(this.Form1_Load);
      this.MouseDown += new MouseEventHandler(this.Form1_MouseDown);
      this.MouseMove += new MouseEventHandler(this.Form1_MouseMove);
      ((ISupportInitialize) this.pictureBox3).EndInit();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
