// Decompiled with JetBrains decompiler
// Type: SSTap.View.FormUpdate
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using SSTap.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SSTap.View
{
    public class FormUpdate : Form
    {
        public bool completed = true;
        private IContainer components = (IContainer)null;
        public FormUpdate.UpdateGUI updateGUI;
        public FormUpdate.InitGUI initGUI;
        private TableLayoutPanel tableLayoutPanel1;
        private ProgressBar progressBar1;
        private Label label1;

        public FormUpdate()
        {
            this.InitializeComponent();
            this.updateGUI = new FormUpdate.UpdateGUI(this.UpdateGUIHandler);
            this.initGUI = new FormUpdate.InitGUI(this.InitGUIHandler);
        }

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\update\\" + Program.newPackageName + ".exe";
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            new Thread(new ThreadStart(this.DownloadSS)).Start();
        }

        public void InitGUIHandler(int max)
        {
            base.Invoke(new Action(delegate ()
            {
                this.progressBar1.Maximum = max;
            }));
        }

        public void UpdateGUIHandler(int prog, float percent)
        {
            base.Invoke(new Action(delegate ()
            {
                this.label1.Text = string.Format("{0:F}", percent) + "%";
                this.progressBar1.Value = prog;
            }));
        }

        public void DownloadSS()
        {
            int index = new Random().Next(0, Program.downloadLink.Count);
            string requestUriString = Program.downloadLink[index];
            string str = Application.StartupPath + "\\update\\" + Program.newPackageName + ".exe";
            if (!this.completed)
                return;
            try
            {
                HttpWebResponse response = (HttpWebResponse)WebRequest.Create(requestUriString).GetResponse();
                long contentLength = response.ContentLength;
                this.initGUI((int)contentLength);
                Stream responseStream = response.GetResponseStream();
                Stream stream = (Stream)new FileStream(str, FileMode.Create);
                long num = 0;
                byte[] buffer = new byte[1024];
                int count = responseStream.Read(buffer, 0, buffer.Length);
                while (count > 0)
                {
                    num = (long)count + num;
                    Application.DoEvents();
                    stream.Write(buffer, 0, count);
                    count = responseStream.Read(buffer, 0, buffer.Length);
                    float percent = (float)((double)num / (double)contentLength * 100.0);
                    this.updateGUI((int)num, percent);
                }
                stream.Close();
                responseStream.Close();
            }
            catch (WebException ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
            }
            Report.ReportClientUpdate();
            if (!System.IO.File.Exists(str))
            {
                int num = (int)MessageBox.Show("更新失败！");
                Environment.Exit(0);
            }
            int num1 = (int)MessageBox.Show("新版程序下载成功，请按照提示进行安装");
            Process.Start(str);
            Environment.Exit(0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SSTap.View.FormUpdate));
            this.tableLayoutPanel1 = new global::System.Windows.Forms.TableLayoutPanel();
            this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
            this.label1 = new global::System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Location = new global::System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
            this.tableLayoutPanel1.Size = new global::System.Drawing.Size(495, 25);
            this.tableLayoutPanel1.TabIndex = 0;
            this.progressBar1.Anchor = global::System.Windows.Forms.AnchorStyles.Left;
            this.progressBar1.Location = new global::System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new global::System.Drawing.Size(445, 19);
            this.progressBar1.Style = global::System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            this.label1.Anchor = global::System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new global::System.Drawing.Point(454, 6);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(0, 12);
            this.label1.TabIndex = 1;
            base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
            base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = global::System.Drawing.Color.White;
            base.ClientSize = new global::System.Drawing.Size(519, 51);
            base.Controls.Add(this.tableLayoutPanel1);
            base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
            //base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormUpdate";
            base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动更新";
            base.Load += new global::System.EventHandler(this.FormUpdate_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            base.ResumeLayout(false);
        }

        public delegate void UpdateGUI(int prog, float percent);

        public delegate void InitGUI(int max);
    }
}
