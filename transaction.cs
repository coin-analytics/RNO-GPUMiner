// Decompiled with JetBrains decompiler
// Type: coinminner.transaction
// Assembly: rnocoinminer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D88D9D4B-A690-412D-B858-EE24C0C62E1A
// Assembly location: C:\Program Files (x86)\RNO\RnoMiner(Beta)\rnocoinminer.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace coinminner
{
  public class transaction : Form
  {
    private IContainer components;
    private TextBox LogBox;

    public transaction()
    {
      this.InitializeComponent();
    }

    private void transaction_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }

    public void setLog(string log)
    {
      this.LogBox.Text = log;
      this.LogBox.SelectionStart = this.LogBox.Text.Length;
      this.LogBox.ScrollToCaret();
    }

    private void LogBox_TextChanged(object sender, EventArgs e)
    {
    }

    private void transaction_Load(object sender, EventArgs e)
    {
      this.MaximizeBox = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (transaction));
      this.LogBox = new TextBox();
      this.SuspendLayout();
      this.LogBox.BackColor = Color.Black;
      this.LogBox.Font = new Font("Yu Gothic UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.LogBox.ForeColor = Color.White;
      this.LogBox.Location = new Point(0, 1);
      this.LogBox.Multiline = true;
      this.LogBox.Name = "LogBox";
      this.LogBox.ReadOnly = true;
      this.LogBox.ScrollBars = ScrollBars.Vertical;
      this.LogBox.Size = new Size(1138, 711);
      this.LogBox.TabIndex = 27;
      this.LogBox.TextChanged += new EventHandler(this.LogBox_TextChanged);
      this.AutoScaleDimensions = new SizeF(13f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1140, 712);
      this.Controls.Add((Control) this.LogBox);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (transaction);
      this.Text = "mining log";
      this.FormClosing += new FormClosingEventHandler(this.transaction_FormClosing);
      this.Load += new EventHandler(this.transaction_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
