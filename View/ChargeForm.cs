using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace SSTap.View
{
    public class ChargeForm : Form
    {
        private IContainer components = (IContainer)null;
        private WebBrowser webBrowser1;

        public ChargeForm()
        {
            this.InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ChargeForm.ValidateServerCertificate);
        }

        public static bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void ChargeForm_Load(object sender, EventArgs e)
        {
            this.SuppressWininetBehavior();
            this.webBrowser1.Url = new Uri(Program.loginController.userInfo.BaseUrl + "/api/redirect?target=/client/code&access_token=" + Program.loginController.userInfo.Token);
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(
          int hInternet,
          int dwOption,
          IntPtr lpBuffer,
          int dwBufferLength);

        private unsafe void SuppressWininetBehavior()
        {
            int num = 3;
            int* value = &num;
            bool flag = ChargeForm.InternetSetOption(0, 81, new IntPtr((void*)value), 4);
            bool flag2 = !flag;
            if (flag2)
            {
                MessageBox.Show("Something went wrong !>?");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.webBrowser1 = new WebBrowser();
            this.SuspendLayout();
            this.webBrowser1.Dock = DockStyle.Fill;
            this.webBrowser1.Location = new Point(0, 0);
            this.webBrowser1.MinimumSize = new Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new Size(472, 424);
            this.webBrowser1.TabIndex = 0;
            this.AutoScaleDimensions = new SizeF(6f, 12f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new Size(472, 424);
            this.Controls.Add((Control)this.webBrowser1);
            this.ImeMode = ImeMode.Disable;
            this.MinimizeBox = false;
            this.Name = nameof(ChargeForm);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "充值";
            this.Load += new EventHandler(this.ChargeForm_Load);
            this.ResumeLayout(false);
        }
    }
}
