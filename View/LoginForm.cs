// Decompiled with JetBrains decompiler
// Type: SSTap.View.LoginForm
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using Newtonsoft.Json;
using SSTap.Model;
using SSTap.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SSTap.View
{
    public class LoginForm : Form
    {
        private bool _flag = false;
        private bool _is_runing = false;
        private string _config_path = "web.json";
        private IContainer components = (IContainer)null;
        private LoginForm.LoginDelegate loginDelegate;
        private LoginConfig _config;
        private Point mouseOff;
        private bool leftFlag;
        private CheckBox RememberPasswordCheckbox;
        private KaguyaButton LoginButton;
        private TextBox PasswordText;
        private TextBox UsernameText;
        private System.Windows.Forms.Timer timer1;
        private Label InfoLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private LinkLabel linkLabel1;
        private TableLayoutPanel tableLayoutPanel8;
        private TextBox WebTextBox;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel9;
        private Label TitleLabel;
        private Panel LogoPanel;
        private TableLayoutPanel tableLayoutPanel10;
        private Label MinLabel;
        private Label ExitLabel;
        private TableLayoutPanel tableLayoutPanel11;

        public LoginForm()
        {
            this.InitializeComponent();
            this.loginDelegate = new LoginForm.LoginDelegate(this.LoginUiHandler);
        }

        private void LoginUiHandler(string status, int flag)
        {
            try
            {
                base.Invoke(new Action(delegate ()
                {
                    this.InfoLabel.Text = status;
                    bool flag2 = flag == 1;
                    if (flag2)
                    {
                        this.LoginButton.Enabled = true;
                        this.LoginButton.Text = "登录";
                    }
                    else
                    {
                        bool flag3 = flag == 2;
                        if (flag3)
                        {
                            this.LoginButton.Enabled = false;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void CloseHandler(object Sender, EventArgs e)
        {
            if (this._is_runing)
                return;
            Thread.Sleep(100);
            this.Close();
        }

        private void ThreadLogin()
        {
            this._is_runing = true;
            this.loginDelegate("正在登陆...", 0);
            if (Program.loginController.userInfo.Login(this.UsernameText.Text, this.PasswordText.Text))
            {
                this.loginDelegate("登录成功，正在获取节点信息...", 0);
                if (!Program.loginController.userInfo.GetSubLink())
                {
                    this.loginDelegate("获取节点信息失败，请联系管理员", 1);
                    int num = (int)MessageBox.Show("获取节点信息失败，请联系管理员");
                }
                else
                {
                    this._config.isChecked = this.RememberPasswordCheckbox.Checked;
                    this._config.username = this.UsernameText.Text;
                    this._config.password = !this._config.isChecked ? "" : this.PasswordText.Text;
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)File.Open(this._config_path, FileMode.Create)))
                        {
                            string str = JsonConvert.SerializeObject((object)this._config, (Formatting)1);
                            streamWriter.Write(str);
                            streamWriter.Flush();
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (Program.loginController.userInfo.Nodes.Count == -1 && Program.loginController.ShowCharge() == DialogResult.Cancel)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this._is_runing = false;
                    }
                    this.DialogResult = DialogResult.OK;
                    this._flag = true;
                    this._is_runing = false;
                }
            }
            else
            {
                this.loginDelegate("登陆失败" + Program.loginController.userInfo.msg, 1);
                int num = (int)MessageBox.Show(Program.loginController.userInfo.msg, "登陆失败");
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (this.UsernameText.Text == "")
            {
                int num = (int)MessageBox.Show("用户名不能为空");
                this.UsernameText.Focus();
            }
            else if (this.PasswordText.Text == "")
            {
                int num = (int)MessageBox.Show("密码不能为空");
                this.PasswordText.Focus();
            }
            else if (this.WebTextBox.Text == "")
            {
                int num = (int)MessageBox.Show("网站链接不能为空");
                this.WebTextBox.Focus();
            }
            else
            {
                Config.BaseUrl = this.WebTextBox.Text;
                Program.loginController.userInfo = new UserInfo();
                this.LoginButton.Enabled = false;
                this.LoginButton.Text = "登录中...";
                this.TopMost = false;
                new Thread(new ThreadStart(this.ThreadLogin)).Start();
                this.timer1.Enabled = true;
                this.timer1.Interval = 100;
                this.timer1.Start();
                this.timer1.Tick += new EventHandler(this.CloseHandler);
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._flag)
                return;
            Environment.Exit(0);
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.LoginButton.BackColor = Color.FromArgb(90, 162, 205);
            this._config = File.Exists(this._config_path) ? (LoginConfig)JsonConvert.DeserializeObject<LoginConfig>(File.ReadAllText(this._config_path)) : new LoginConfig();
            this.UsernameText.Text = this._config.username;
            if (this._config.isChecked)
            {
                this.PasswordText.Text = this._config.password;
                this.RememberPasswordCheckbox.Checked = true;
            }
            else
                this.RememberPasswordCheckbox.Checked = false;
            this.AcceptButton = (IButtonControl)this.LoginButton;
        }

        private void RememberPasswordCheckbox_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://91vps.win/2017/12/30/a_sstap/");
        }

        private void ExitLabel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ExitLabel_MouseHover(object sender, EventArgs e)
        {
            this.ExitLabel.BackColor = Color.FromArgb(250, 5, 5);
            this.ExitLabel.ForeColor = Color.White;
        }

        private void ExitLabel_MouseLeave(object sender, EventArgs e)
        {
            this.ExitLabel.BackColor = Color.White;
            this.ExitLabel.ForeColor = Color.Black;
        }

        private void MinLabel_MouseHover(object sender, EventArgs e)
        {
            this.MinLabel.BackColor = Color.Silver;
        }

        private void MinLabel_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MinLabel_MouseLeave(object sender, EventArgs e)
        {
            this.MinLabel.BackColor = Color.White;
        }

        private void LoginForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.leftFlag)
                return;
            this.leftFlag = false;
        }

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            this.mouseOff = new Point(-e.X, -e.Y);
            this.leftFlag = true;
        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.leftFlag)
                return;
            Point mousePosition = Control.MousePosition;
            mousePosition.Offset(this.mouseOff.X, this.mouseOff.Y);
            this.Location = mousePosition;
        }

        private void tableLayoutPanel9_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.leftFlag)
                return;
            Point mousePosition = Control.MousePosition;
            mousePosition.Offset(this.mouseOff.X, this.mouseOff.Y);
            this.Location = mousePosition;
        }

        private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.leftFlag)
                return;
            Point mousePosition = Control.MousePosition;
            mousePosition.Offset(this.mouseOff.X - this.LogoPanel.Width - 12, this.mouseOff.Y - this.TitleLabel.Top);
            this.Location = mousePosition;
        }

        private void LogoPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.leftFlag)
                return;
            Point mousePosition = Control.MousePosition;
            mousePosition.Offset(this.mouseOff.X - 6, this.mouseOff.Y - this.LogoPanel.Top);
            this.Location = mousePosition;
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LoginForm));
            this.RememberPasswordCheckbox = new CheckBox();
            this.PasswordText = new TextBox();
            this.UsernameText = new TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.InfoLabel = new Label();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.tableLayoutPanel8 = new TableLayoutPanel();
            this.WebTextBox = new TextBox();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.tableLayoutPanel4 = new TableLayoutPanel();
            this.tableLayoutPanel5 = new TableLayoutPanel();
            this.tableLayoutPanel6 = new TableLayoutPanel();
            this.tableLayoutPanel7 = new TableLayoutPanel();
            this.linkLabel1 = new LinkLabel();
            this.tableLayoutPanel9 = new TableLayoutPanel();
            this.TitleLabel = new Label();
            this.tableLayoutPanel10 = new TableLayoutPanel();
            this.MinLabel = new Label();
            this.ExitLabel = new Label();
            this.tableLayoutPanel11 = new TableLayoutPanel();
            this.LogoPanel = new Panel();
            this.panel1 = new Panel();
            this.panel4 = new Panel();
            this.panel2 = new Panel();
            this.panel3 = new Panel();
            this.LoginButton = new KaguyaButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.SuspendLayout();
            this.RememberPasswordCheckbox.Anchor = AnchorStyles.None;
            this.RememberPasswordCheckbox.AutoSize = true;
            this.RememberPasswordCheckbox.Font = new Font("微软雅黑", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.RememberPasswordCheckbox.Location = new Point(3, 3);
            this.RememberPasswordCheckbox.Name = "RememberPasswordCheckbox";
            this.RememberPasswordCheckbox.Size = new Size(80, 23);
            this.RememberPasswordCheckbox.TabIndex = 16;
            this.RememberPasswordCheckbox.Text = "记住密码";
            this.RememberPasswordCheckbox.UseVisualStyleBackColor = true;
            this.RememberPasswordCheckbox.CheckedChanged += new EventHandler(this.RememberPasswordCheckbox_CheckedChanged);
            this.PasswordText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.PasswordText.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.PasswordText.ImeMode = ImeMode.Disable;
            this.PasswordText.Location = new Point(51, 3);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new Size(293, 29);
            this.PasswordText.TabIndex = 13;
            this.UsernameText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.UsernameText.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.UsernameText.ImeMode = ImeMode.Disable;
            this.UsernameText.Location = new Point(52, 3);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new Size(292, 29);
            this.UsernameText.TabIndex = 12;
            this.InfoLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.ForeColor = Color.RoyalBlue;
            this.InfoLabel.Location = new Point(3, 469);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new Size(0, 12);
            this.InfoLabel.TabIndex = 18;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add((Control)this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add((Control)this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add((Control)this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel1.Controls.Add((Control)this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel1.Location = new Point(0, 36);
            this.tableLayoutPanel1.Margin = new Padding(0, 0, 10, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel1.Size = new Size(347, 433);
            this.tableLayoutPanel1.TabIndex = 19;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Controls.Add((Control)this.tableLayoutPanel8, 0, 2);
            this.tableLayoutPanel2.Controls.Add((Control)this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add((Control)this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Location = new Point(0, 250);
            this.tableLayoutPanel2.Margin = new Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.Size = new Size(347, 105);
            this.tableLayoutPanel2.TabIndex = 1;
            this.tableLayoutPanel8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel8.AutoSize = true;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.85542f));
            this.tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 86.14458f));
            this.tableLayoutPanel8.Controls.Add((Control)this.WebTextBox, 1, 0);
            this.tableLayoutPanel8.Controls.Add((Control)this.panel4, 0, 0);
            this.tableLayoutPanel8.Location = new Point(0, 70);
            this.tableLayoutPanel8.Margin = new Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel8.Size = new Size(347, 35);
            this.tableLayoutPanel8.TabIndex = 2;
            this.WebTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.WebTextBox.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.WebTextBox.ImeMode = ImeMode.Disable;
            this.WebTextBox.Location = new Point(51, 3);
            this.WebTextBox.Name = "WebTextBox";
            this.WebTextBox.Size = new Size(293, 29);
            this.WebTextBox.TabIndex = 13;
            this.tableLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.15663f));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85.84338f));
            this.tableLayoutPanel3.Controls.Add((Control)this.UsernameText, 1, 0);
            this.tableLayoutPanel3.Controls.Add((Control)this.panel2, 0, 0);
            this.tableLayoutPanel3.Location = new Point(0, 0);
            this.tableLayoutPanel3.Margin = new Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel3.Size = new Size(347, 35);
            this.tableLayoutPanel3.TabIndex = 0;
            this.tableLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.85542f));
            this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 86.14458f));
            this.tableLayoutPanel4.Controls.Add((Control)this.PasswordText, 1, 0);
            this.tableLayoutPanel4.Controls.Add((Control)this.panel3, 0, 0);
            this.tableLayoutPanel4.Location = new Point(0, 35);
            this.tableLayoutPanel4.Margin = new Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel4.Size = new Size(347, 35);
            this.tableLayoutPanel4.TabIndex = 1;
            this.tableLayoutPanel5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.37209f));
            this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 86.62791f));
            this.tableLayoutPanel5.Controls.Add((Control)this.LoginButton, 1, 0);
            this.tableLayoutPanel5.Location = new Point(0, 355);
            this.tableLayoutPanel5.Margin = new Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel5.Size = new Size(347, 46);
            this.tableLayoutPanel5.TabIndex = 2;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.95349f));
            this.tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 86.04651f));
            this.tableLayoutPanel6.Controls.Add((Control)this.tableLayoutPanel7, 1, 0);
            this.tableLayoutPanel6.Location = new Point(0, 401);
            this.tableLayoutPanel6.Margin = new Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel6.Size = new Size(344, 32);
            this.tableLayoutPanel6.TabIndex = 3;
            this.tableLayoutPanel7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.tableLayoutPanel7.AutoSize = true;
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel7.Controls.Add((Control)this.RememberPasswordCheckbox, 0, 0);
            this.tableLayoutPanel7.Controls.Add((Control)this.linkLabel1, 1, 0);
            this.tableLayoutPanel7.Location = new Point(172, 0);
            this.tableLayoutPanel7.Margin = new Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel7.Size = new Size(172, 29);
            this.tableLayoutPanel7.TabIndex = 0;
            this.linkLabel1.ActiveLinkColor = Color.Gray;
            this.linkLabel1.Anchor = AnchorStyles.Right;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new Font("微软雅黑", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.linkLabel1.LinkColor = Color.Gray;
            this.linkLabel1.Location = new Point(122, 6);
            this.linkLabel1.Margin = new Padding(3, 3, 15, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(35, 19);
            this.linkLabel1.TabIndex = 17;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "帮助";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.tableLayoutPanel9.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel9.Controls.Add((Control)this.TitleLabel, 1, 0);
            this.tableLayoutPanel9.Controls.Add((Control)this.LogoPanel, 0, 0);
            this.tableLayoutPanel9.Controls.Add((Control)this.tableLayoutPanel10, 2, 0);
            this.tableLayoutPanel9.Location = new Point(3, 0);
            this.tableLayoutPanel9.Margin = new Padding(3, 0, 0, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel9.Size = new Size(354, 33);
            this.tableLayoutPanel9.TabIndex = 4;
            this.tableLayoutPanel9.MouseDown += new MouseEventHandler(this.LoginForm_MouseDown);
            this.tableLayoutPanel9.MouseMove += new MouseEventHandler(this.tableLayoutPanel9_MouseMove);
            this.tableLayoutPanel9.MouseUp += new MouseEventHandler(this.LoginForm_MouseUp);
            this.TitleLabel.Anchor = AnchorStyles.Left;
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)134);
            this.TitleLabel.Location = new Point(33, 5);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new Size(42, 22);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "登录";
            this.TitleLabel.MouseDown += new MouseEventHandler(this.LoginForm_MouseDown);
            this.TitleLabel.MouseMove += new MouseEventHandler(this.TitleLabel_MouseMove);
            this.TitleLabel.MouseUp += new MouseEventHandler(this.LoginForm_MouseUp);
            this.tableLayoutPanel10.Anchor = AnchorStyles.Right;
            this.tableLayoutPanel10.AutoSize = true;
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel10.Controls.Add((Control)this.MinLabel, 0, 0);
            this.tableLayoutPanel10.Controls.Add((Control)this.ExitLabel, 1, 0);
            this.tableLayoutPanel10.Location = new Point(275, 3);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel10.Size = new Size(76, 27);
            this.tableLayoutPanel10.TabIndex = 2;
            this.MinLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.MinLabel.AutoSize = true;
            this.MinLabel.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)134);
            this.MinLabel.ForeColor = Color.Black;
            this.MinLabel.Location = new Point(3, 0);
            this.MinLabel.Name = "MinLabel";
            this.MinLabel.Padding = new Padding(3, 2, 3, 0);
            this.MinLabel.Size = new Size(32, 27);
            this.MinLabel.TabIndex = 0;
            this.MinLabel.Text = "ー";
            this.MinLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.MinLabel.Click += new EventHandler(this.MinLabel_Click);
            this.MinLabel.MouseLeave += new EventHandler(this.MinLabel_MouseLeave);
            this.MinLabel.MouseHover += new EventHandler(this.MinLabel_MouseHover);
            this.ExitLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.ExitLabel.AutoSize = true;
            this.ExitLabel.Font = new Font("微软雅黑", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)134);
            this.ExitLabel.ForeColor = Color.Black;
            this.ExitLabel.Location = new Point(41, 0);
            this.ExitLabel.Name = "ExitLabel";
            this.ExitLabel.Padding = new Padding(2, 0, 0, 3);
            this.ExitLabel.Size = new Size(32, 27);
            this.ExitLabel.TabIndex = 1;
            this.ExitLabel.Text = "×";
            this.ExitLabel.Click += new EventHandler(this.ExitLabel_Click);
            this.ExitLabel.MouseLeave += new EventHandler(this.ExitLabel_MouseLeave);
            this.ExitLabel.MouseHover += new EventHandler(this.ExitLabel_MouseHover);
            this.tableLayoutPanel11.AutoSize = true;
            this.tableLayoutPanel11.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel11.ColumnCount = 1;
            this.tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel11.Controls.Add((Control)this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel11.Controls.Add((Control)this.InfoLabel, 0, 2);
            this.tableLayoutPanel11.Controls.Add((Control)this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel11.Location = new Point(1, 0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 3;
            this.tableLayoutPanel11.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel11.Size = new Size(357, 481);
            this.tableLayoutPanel11.TabIndex = 20;
            this.LogoPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.LogoPanel.BackgroundImage = (Image)Resources.login_logo;
            this.LogoPanel.BackgroundImageLayout = ImageLayout.Zoom;
            this.LogoPanel.Location = new Point(3, 4);
            this.LogoPanel.Name = "LogoPanel";
            this.LogoPanel.Size = new Size(24, 24);
            this.LogoPanel.TabIndex = 1;
            this.LogoPanel.MouseDown += new MouseEventHandler(this.LoginForm_MouseDown);
            this.LogoPanel.MouseMove += new MouseEventHandler(this.LogoPanel_MouseMove);
            this.LogoPanel.MouseUp += new MouseEventHandler(this.LoginForm_MouseUp);
            this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.panel1.BackgroundImage = (Image)Resources.login_logo;
            this.panel1.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel1.Location = new Point(73, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(200, 244);
            this.panel1.TabIndex = 0;
            this.panel4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.panel4.BackgroundImage = (Image)Resources.globe_512;
            this.panel4.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel4.Location = new Point(16, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(29, 29);
            this.panel4.TabIndex = 14;
            this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            this.panel2.BackgroundImage = (Image)Resources.abstract_user_flat_1;
            this.panel2.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel2.Location = new Point(16, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(30, 29);
            this.panel2.TabIndex = 13;
            this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.panel3.BackgroundImage = (Image)Resources.witchlines_Simple_key;
            this.panel3.BackgroundImageLayout = ImageLayout.Zoom;
            this.panel3.Location = new Point(16, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(29, 21);
            this.panel3.TabIndex = 14;
            this.LoginButton.BackColor = Color.White;
            this.LoginButton.Cursor = Cursors.Hand;
            this.LoginButton.FlatAppearance.BorderColor = Color.FromArgb(90, 162, 205);
            this.LoginButton.FlatAppearance.BorderSize = 0;
            this.LoginButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 121, 182);
            this.LoginButton.FlatStyle = FlatStyle.Flat;
            this.LoginButton.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.LoginButton.ForeColor = Color.Snow;
            this.LoginButton.Location = new Point(49, 3);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new Size(292, 40);
            this.LoginButton.TabIndex = 14;
            this.LoginButton.Text = "登 录";
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new EventHandler(this.LoginButton_Click);
            this.AutoScaleDimensions = new SizeF(96f, 96f);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.White;
            this.ClientSize = new Size(359, 484);
            this.Controls.Add((Control)this.tableLayoutPanel11);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Name = nameof(LoginForm);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.TopMost = true;
            this.FormClosing += new FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new EventHandler(this.LoginForm_Load);
            this.MouseDown += new MouseEventHandler(this.LoginForm_MouseDown);
            this.MouseMove += new MouseEventHandler(this.LoginForm_MouseMove);
            this.MouseUp += new MouseEventHandler(this.LoginForm_MouseUp);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public delegate void LoginDelegate(string status, int flag);

        public delegate void CloseDelegate();
    }
}
