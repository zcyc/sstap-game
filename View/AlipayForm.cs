// Decompiled with JetBrains decompiler
// Type: SSTap.View.AlipayForm
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZXing.QrCode.Internal;

namespace SSTap.View
{
  public class AlipayForm : Form
  {
    private bool _flag = false;
    private IContainer components = (IContainer) null;
    private string AlipayUrl;
    private string OrderId;
    private AlipayForm.WaitForResultDelegate resultDelegate;
    private Thread t;
    private TableLayoutPanel tableLayoutPanel1;
    private PictureBox pictureBox1;
    private Label StatusLabel;

    public AlipayForm(string alipay_url, string order_id)
    {
      this.InitializeComponent();
      this.AlipayUrl = alipay_url;
      this.OrderId = order_id;
      this.resultDelegate = new AlipayForm.WaitForResultDelegate(this.WaitForResultHandler);
    }

    private void AlipayForm_Load(object sender, EventArgs e)
    {
      this.GenQR(this.AlipayUrl);
      this.t = new Thread(new ThreadStart(this.ThreadWaitForResult));
      this.t.Start();
    }

        public void WaitForResultHandler(string status)
        {
            base.Invoke(new Action(delegate ()
            {
                this.StatusLabel.Text = status;
            }));
        }

        public void ThreadWaitForResult()
    {
      this.resultDelegate("扫码后，请不要关闭本窗口，请耐心等待充值结果返回");
      for (int index = 0; index < 100; ++index)
      {
        if (Program.loginController.userInfo.CheckStatus(this.OrderId))
        {
          this.resultDelegate("充值成功，请关闭本窗口");
          this._flag = true;
          this.DialogResult = DialogResult.OK;
          this.t.Abort();
          break;
        }
        Thread.Sleep(1000);
      }
    }

        private void GenQR(string alipay_url)
        {
            QRCode qrcode = Encoder.encode(alipay_url, ErrorCorrectionLevel.M);
            ByteMatrix matrix = qrcode.Matrix;
            int num = Math.Max(this.pictureBox1.Height / matrix.Height, 1);
            int num2 = matrix.Width * num;
            int num3 = matrix.Height * num;
            int val = this.pictureBox1.Width - num2;
            int val2 = this.pictureBox1.Height - num3;
            int num4 = Math.Max(val, val2);
            this.pictureBox1.SizeMode = ((num4 >= 7 * num) ? PictureBoxSizeMode.Zoom : PictureBoxSizeMode.CenterImage);
            Bitmap image = new Bitmap(matrix.Width * num, matrix.Height * num);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.White);
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    for (int i = 0; i < matrix.Width; i++)
                    {
                        for (int j = 0; j < matrix.Height; j++)
                        {
                            bool flag = matrix[i, j] != 0;
                            if (flag)
                            {
                                graphics.FillRectangle(brush, num * i, num * j, num, num);
                            }
                        }
                    }
                }
            }
            this.pictureBox1.Image = image;
        }

        private void AlipayForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._flag)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Abort;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.pictureBox1 = new PictureBox();
      this.StatusLabel = new Label();
      this.tableLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Controls.Add((Control) this.pictureBox1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.StatusLabel, 0, 1);
      this.tableLayoutPanel1.Location = new Point(12, 12);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90.7173f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.282701f));
      this.tableLayoutPanel1.Size = new Size(260, 287);
      this.tableLayoutPanel1.TabIndex = 0;
      this.pictureBox1.Location = new Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(254, 254);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.StatusLabel.AutoSize = true;
      this.StatusLabel.Location = new Point(3, 260);
      this.StatusLabel.Name = "StatusLabel";
      this.StatusLabel.Size = new Size(0, 12);
      this.StatusLabel.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 311);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.ImeMode = ImeMode.Disable;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AlipayForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "请使用支付宝扫码支付";
      this.TopMost = true;
      this.FormClosing += new FormClosingEventHandler(this.AlipayForm_FormClosing);
      this.Load += new EventHandler(this.AlipayForm_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }

    public delegate void WaitForResultDelegate(string status);
  }
}
