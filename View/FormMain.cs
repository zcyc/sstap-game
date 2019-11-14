using PInvoke;
using SSTap.Controller;
using SSTap.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SSTap.View
{
    public class FormMain : Form
    {
        public int labelFlag = 0;
        private bool flag1 = false;
        private int trafficWidth = 0;
        private int trafficHeight = 0;
        //private bool adStatus = false;
        private IContainer components = (IContainer)null;
        public SSTapController sSTapController;
        public Thread ThreadUpdateSSTapUi;
        public Thread ThreadWatchSSTapStatus;
        public Thread ThreadUpdateUserInfo;
        public Thread ThreadUpdateUserInfos;
        public Thread ThreadCloseWarningDialog;
        public FormMain.UpdateSSTapUiDelegate updateSSTapUiDelegate;
        public FormMain.UpdateUserInfoDelegate updateUserInfoDelegate;
        public FormMain.ExitLabelDelegate exitLabelDelegate;
        private Point mouseOff;
        private bool leftFlag;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label ExpireLabel;
        private Label FlowInfoLabel;
        private Label UserNameLabel;
        private Label label4;
        private Label ClassLabel;
        private Panel AdPanel;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pictureBox1;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel9;
        private Label TitleLabel;
        private Panel LogoPanel;
        private TableLayoutPanel tableLayoutPanel10;
        private Label MinLabel;
        private Label ExitLabel;
        private NotifyIcon MainNotifyIcon;
        private ContextMenuStrip MainContextMenuStrip;
        private ToolStripMenuItem 显示主界面SToolStripMenuItem;
        private ToolStripMenuItem 退出XToolStripMenuItem;
        private ToolTip toolTip1;

        public FormMain()
        {
            this.InitializeComponent();
            this.sSTapController = new SSTapController();
            this.sSTapController.SetSSTapSublink();
            this.updateSSTapUiDelegate = new FormMain.UpdateSSTapUiDelegate(this.UpdateSSTapUiHandler);
            this.updateUserInfoDelegate = new FormMain.UpdateUserInfoDelegate(this.UpdateUserInfoHandler);
            this.exitLabelDelegate = new FormMain.ExitLabelDelegate(this.ShowExitLabelHandler);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.sSTapController.StartSSTap();
            this.trafficHeight = this.panel3.Height;
            this.trafficWidth = this.panel3.Width;
            User32.SetParent(this.sSTapController.ssTapHwnd, this.panel1.Handle);
            this.sSTapController.GetSSTapStatusWindow();
            this.panel1.Width = 422;
            this.panel1.Height = 156;
            this.ThreadUpdateSSTapUi = new Thread(new ThreadStart(this.UpdateSSTapUi));
            this.ThreadUpdateSSTapUi.Start();
            this.ThreadWatchSSTapStatus = new Thread(new ThreadStart(this.ThreadWatchSStapStatus));
            this.ThreadWatchSSTapStatus.Start();
            this.ThreadUpdateUserInfo = new Thread(new ThreadStart(this.ThreadUpdateUserInfoHandler));
            this.ThreadUpdateUserInfo.Start();
            this.ThreadUpdateUserInfos = new Thread(new ThreadStart(this.UpdateUserInfo));
            this.ThreadUpdateUserInfos.Start();
        }

        public void UpdateSSTapUi()
        {
            this.sSTapController.HideAdvertisements();
            PInvoke.RECT ssapMainRect = this.sSTapController.GetSSapMainRect();
            PInvoke.RECT clientRect = this.sSTapController.GetClientRect();
            PInvoke.RECT ssapAdRect = this.sSTapController.GetSSapAdRect();
            User32.MoveWindow(this.sSTapController.ssTapHwnd, 0, 0, 0, 0, true);
            this.sSTapController.GetSSTapTrafficWindow();
            this.sSTapController.SSTapPing();
            User32.MoveWindow(this.sSTapController.ssTapHwnd, (int)(ssapMainRect.left - clientRect.left), (int)(ssapMainRect.top - clientRect.top), (int)(ssapMainRect.right - ssapMainRect.left), (int)(ssapMainRect.bottom - ssapMainRect.top), true);
            this.updateSSTapUiDelegate((int)(clientRect.right - clientRect.left), ssapAdRect.bottom - clientRect.top + 3);
            this.flag1 = true;
        }

        public void ShowExitLabelHandler()
        {
            try
            {
                base.Invoke(new Action(delegate ()
                {
                    this.label5.Parent = this.panel1;
                    this.label5.Left = 100;
                    this.label5.Top = 15;
                    this.label5.Visible = true;
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void UpdateSSTapUiHandler(int width, int height)
        {
            try
            {
                base.Invoke(new Action(delegate ()
                {
                    bool flag = width != 0 && height != 0;
                    if (flag)
                    {
                        this.panel1.Size = new Size(width, height);
                        this.trafficWidth = width;
                        this.trafficHeight = height;
                        User32.SetParent(this.sSTapController.statusHwnd, this.panel4.Handle);
                    }
                    this.trafficWidth = this.panel3.Width;
                    this.trafficHeight = this.panel3.Height;
                    User32.SetParent(this.sSTapController.trafficHwnd, this.panel3.Handle);
                    User32.MoveWindow(this.sSTapController.trafficHwnd, 0, 0, this.panel3.Width, this.panel3.Height, true);
                    this.pictureBox1.Visible = false;
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("退出程序将导致正在加速的游戏掉线，是否退出?", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                this.sSTapController.StopSSTap();
                this.sSTapController.UnSetSSTapSublink();
                this.ThreadUpdateSSTapUi.Abort();
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

        public void ThreadWatchSStapStatus()
        {
            while (true)
            {
                Process[] processes = Process.GetProcesses();
                bool flag = true;
                foreach (Process process in processes)
                {
                    if (process.Id == this.sSTapController.SSTapPid)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    this.sSTapController.UnSetSSTapSublink();
                    Environment.Exit(0);
                }
                Thread.Sleep(300);
            }
        }

        public void ThreadUpdateUserInfoHandler()
        {
            while (true)
            {
                if (this.flag1)
                {
                    User32.MoveWindow(this.sSTapController.trafficHwnd, 0, 0, this.trafficWidth, this.trafficHeight, true);
                    User32.MoveWindow(this.sSTapController.statusHwnd, 0, 0, this.panel4.Width, this.panel4.Height, true);
                }
                this.sSTapController.ShowSSTapForm();
                if (this.labelFlag == 50)
                    this.exitLabelDelegate();
                ++this.labelFlag;
                Thread.Sleep(100);
            }
        }

        public void UpdateUserInfoHandler()
        {
            try
            {
                base.Invoke(new Action(delegate ()
                {
                    string name = Program.loginController.userInfo.Name;
                    this.UserNameLabel.Text = name;
                    this.toolTip1.SetToolTip(this.UserNameLabel, name);
                    this.ClassLabel.Text = Program.loginController.userInfo.Class.ToString();
                    this.FlowInfoLabel.Text = Program.loginController.userInfo.UpdateNodeFlow();
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime dateTime2 = dateTime.AddMilliseconds((double)Program.loginController.userInfo.Expires).ToLocalTime();
                    this.ExpireLabel.Text = dateTime2.ToString("yyyy年MM月dd日\n HH:mm:ss");
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void UpdateUserInfo()
        {
            while (true)
            {
                Program.loginController.userInfo.GetAccount();
                this.updateUserInfoDelegate();
                Thread.Sleep(10000);
            }
        }

        private void AdPanel_Click(object sender, EventArgs e)
        {
            Process.Start(Config.BaseUrl);
        }

        public void CloseWarningForm()
        {
            IntPtr zero = IntPtr.Zero;
            bool flag = true;
            while (flag)
            {
                IntPtr dialogCancelButton = this.sSTapController.GetWarningDialogCancelButton();
                if (!(dialogCancelButton == IntPtr.Zero))
                {
                    this.sSTapController.CloseWaringDialog(dialogCancelButton);
                    flag = false;
                    Thread.Sleep(50);
                }
            }
        }

        private void AdPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            this.mouseOff = new Point(-e.X, -e.Y);
            this.leftFlag = true;
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.leftFlag)
                return;
            this.leftFlag = false;
        }

        private void ExitLabel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MinLabel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.MainNotifyIcon.Visible = true;
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

        private void MinLabel_MouseLeave(object sender, EventArgs e)
        {
            this.MinLabel.BackColor = Color.White;
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

        private void 显示主界面SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.MainNotifyIcon.Visible = false;
        }

        private void MainNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.MainNotifyIcon.Visible = false;
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("退出程序将导致正在加速的游戏掉线，是否退出?", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            this.sSTapController.StopSSTap();
            this.sSTapController.UnSetSSTapSublink();
            this.ThreadUpdateSSTapUi.Abort();
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
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMain));
            this.panel1 = new Panel();
            this.pictureBox1 = new PictureBox();
            this.label5 = new Label();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.panel2 = new Panel();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.ExpireLabel = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label1 = new Label();
            this.label2 = new Label();
            this.ClassLabel = new Label();
            this.UserNameLabel = new Label();
            this.FlowInfoLabel = new Label();
            this.AdPanel = new Panel();
            this.panel3 = new Panel();
            this.panel4 = new Panel();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.tableLayoutPanel9 = new TableLayoutPanel();
            this.TitleLabel = new Label();
            this.LogoPanel = new Panel();
            this.tableLayoutPanel10 = new TableLayoutPanel();
            this.MinLabel = new Label();
            this.ExitLabel = new Label();
            this.MainNotifyIcon = new NotifyIcon(this.components);
            this.MainContextMenuStrip = new ContextMenuStrip(this.components);
            this.显示主界面SToolStripMenuItem = new ToolStripMenuItem();
            this.退出XToolStripMenuItem = new ToolStripMenuItem();
            this.toolTip1 = new ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.MainContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            this.panel1.BackColor = SystemColors.ControlLightLight;
            this.panel1.Controls.Add((Control)this.pictureBox1);
            this.panel1.Location = new Point(3, 285);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(374, 51);
            this.panel1.TabIndex = 1;
            this.pictureBox1.Image = (Image)Resources._000;
            this.pictureBox1.Location = new Point(72, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(224, 146);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.label5.AutoSize = true;
            this.label5.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.label5.Location = new Point(186, 85);
            this.label5.Name = "label5";
            this.label5.Size = new Size(194, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "程序正在退出，请稍等......";
            this.label5.Visible = false;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add((Control)this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add((Control)this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add((Control)this.AdPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add((Control)this.panel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add((Control)this.panel4, 0, 4);
            this.tableLayoutPanel1.Location = new Point(3, 39);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel1.Size = new Size(380, 431);
            this.tableLayoutPanel1.TabIndex = 2;
            this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panel2.BackColor = Color.Transparent;
            this.panel2.BackgroundImage = (Image)Resources.panel1;
            this.panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.panel2.Controls.Add((Control)this.tableLayoutPanel2);
            this.panel2.Controls.Add((Control)this.label5);
            this.panel2.Location = new Point(3, 156);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(374, 123);
            this.panel2.TabIndex = 0;
            this.tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel2.Controls.Add((Control)this.ExpireLabel, 3, 1);
            this.tableLayoutPanel2.Controls.Add((Control)this.label3, 2, 1);
            this.tableLayoutPanel2.Controls.Add((Control)this.label4, 0, 1);
            this.tableLayoutPanel2.Controls.Add((Control)this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add((Control)this.label2, 2, 0);
            this.tableLayoutPanel2.Controls.Add((Control)this.ClassLabel, 1, 1);
            this.tableLayoutPanel2.Controls.Add((Control)this.UserNameLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add((Control)this.FlowInfoLabel, 3, 0);
            this.tableLayoutPanel2.Location = new Point(9, 9);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel2.Size = new Size(353, 102);
            this.tableLayoutPanel2.TabIndex = 0;
            this.ExpireLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.ExpireLabel.AutoSize = true;
            this.ExpireLabel.Font = new Font("Microsoft YaHei UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ExpireLabel.ForeColor = Color.Snow;
            this.ExpireLabel.Location = new Point(245, 55);
            this.ExpireLabel.Name = "ExpireLabel";
            this.ExpireLabel.Size = new Size(105, 42);
            this.ExpireLabel.TabIndex = 7;
            this.ExpireLabel.Text = "2017-12-12\r\n17:00:00";
            this.label3.Anchor = AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft YaHei UI", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.label3.ForeColor = Color.WhiteSmoke;
            this.label3.Location = new Point(164, 62);
            this.label3.Name = "label3";
            this.label3.Size = new Size(75, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "有效期";
            this.label4.Anchor = AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft YaHei UI", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.label4.ForeColor = Color.WhiteSmoke;
            this.label4.Location = new Point(3, 62);
            this.label4.Name = "label4";
            this.label4.Size = new Size(54, 28);
            this.label4.TabIndex = 3;
            this.label4.Text = "等级";
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft YaHei UI", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.label1.ForeColor = Color.WhiteSmoke;
            this.label1.Location = new Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(75, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            this.label2.Anchor = AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft YaHei UI", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.label2.ForeColor = Color.WhiteSmoke;
            this.label2.Location = new Point(185, 11);
            this.label2.Name = "label2";
            this.label2.Size = new Size(54, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "流量";
            this.ClassLabel.Anchor = AnchorStyles.Left;
            this.ClassLabel.AutoSize = true;
            this.ClassLabel.Font = new Font("Microsoft YaHei UI", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ClassLabel.ForeColor = Color.Snow;
            this.ClassLabel.Location = new Point(84, 64);
            this.ClassLabel.Name = "ClassLabel";
            this.ClassLabel.Size = new Size(23, 25);
            this.ClassLabel.TabIndex = 5;
            this.ClassLabel.Text = "5";
            this.UserNameLabel.Anchor = AnchorStyles.Left;
            this.UserNameLabel.Font = new Font("Microsoft YaHei UI", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.UserNameLabel.ForeColor = Color.Snow;
            this.UserNameLabel.Location = new Point(84, 13);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new Size(74, 25);
            this.UserNameLabel.TabIndex = 4;
            this.UserNameLabel.Text = "Name";
            this.FlowInfoLabel.Anchor = AnchorStyles.Left;
            this.FlowInfoLabel.AutoSize = true;
            this.FlowInfoLabel.Font = new Font("Microsoft YaHei UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.FlowInfoLabel.ForeColor = Color.Snow;
            this.FlowInfoLabel.Location = new Point(245, 15);
            this.FlowInfoLabel.Name = "FlowInfoLabel";
            this.FlowInfoLabel.Size = new Size(77, 21);
            this.FlowInfoLabel.TabIndex = 6;
            this.FlowInfoLabel.Text = "FlowInfo";
            this.AdPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.AdPanel.BackgroundImage = (Image)Resources.main_header;
            this.AdPanel.BackgroundImageLayout = ImageLayout.Zoom;
            this.AdPanel.Cursor = Cursors.Hand;
            this.AdPanel.Location = new Point(3, 3);
            this.AdPanel.Name = "AdPanel";
            this.AdPanel.Size = new Size(374, 147);
            this.AdPanel.TabIndex = 2;
            this.AdPanel.Click += new EventHandler(this.AdPanel_Click);
            this.AdPanel.Paint += new PaintEventHandler(this.AdPanel_Paint);
            this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panel3.Location = new Point(3, 342);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(374, 66);
            this.panel3.TabIndex = 3;
            this.panel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panel4.Location = new Point(0, 411);
            this.panel4.Margin = new Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(380, 20);
            this.panel4.TabIndex = 4;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel3.Controls.Add((Control)this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel3.Controls.Add((Control)this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Location = new Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel3.Size = new Size(386, 473);
            this.tableLayoutPanel3.TabIndex = 5;
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
            this.tableLayoutPanel9.Size = new Size(383, 33);
            this.tableLayoutPanel9.TabIndex = 5;
            this.tableLayoutPanel9.MouseDown += new MouseEventHandler(this.FormMain_MouseDown);
            this.tableLayoutPanel9.MouseMove += new MouseEventHandler(this.tableLayoutPanel9_MouseMove);
            this.tableLayoutPanel9.MouseUp += new MouseEventHandler(this.FormMain_MouseUp);
            this.TitleLabel.Anchor = AnchorStyles.Left;
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)134);
            this.TitleLabel.Location = new Point(33, 5);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new Size(69, 22);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "MoeSS";
            this.TitleLabel.MouseDown += new MouseEventHandler(this.FormMain_MouseDown);
            this.TitleLabel.MouseMove += new MouseEventHandler(this.TitleLabel_MouseMove);
            this.TitleLabel.MouseUp += new MouseEventHandler(this.FormMain_MouseUp);
            this.LogoPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.LogoPanel.BackgroundImage = (Image)Resources.login_logo;
            this.LogoPanel.BackgroundImageLayout = ImageLayout.Zoom;
            this.LogoPanel.Location = new Point(3, 4);
            this.LogoPanel.Name = "LogoPanel";
            this.LogoPanel.Size = new Size(24, 24);
            this.LogoPanel.TabIndex = 1;
            this.LogoPanel.MouseDown += new MouseEventHandler(this.FormMain_MouseDown);
            this.LogoPanel.MouseMove += new MouseEventHandler(this.LogoPanel_MouseMove);
            this.LogoPanel.MouseUp += new MouseEventHandler(this.FormMain_MouseUp);
            this.tableLayoutPanel10.Anchor = AnchorStyles.Right;
            this.tableLayoutPanel10.AutoSize = true;
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel10.Controls.Add((Control)this.MinLabel, 0, 0);
            this.tableLayoutPanel10.Controls.Add((Control)this.ExitLabel, 1, 0);
            this.tableLayoutPanel10.Location = new Point(304, 3);
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
            this.MainNotifyIcon.ContextMenuStrip = this.MainContextMenuStrip;
            this.MainNotifyIcon.Icon = (Icon)componentResourceManager.GetObject("MainNotifyIcon.Icon");
            this.MainNotifyIcon.Text = "MoeSS";
            this.MainNotifyIcon.DoubleClick += new EventHandler(this.MainNotifyIcon_DoubleClick);
            this.MainContextMenuStrip.Items.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) this.显示主界面SToolStripMenuItem,
        (ToolStripItem) this.退出XToolStripMenuItem
            });
            this.MainContextMenuStrip.Name = "MainContextMenuStrip";
            this.MainContextMenuStrip.Size = new Size(149, 48);
            this.显示主界面SToolStripMenuItem.Name = "显示主界面SToolStripMenuItem";
            this.显示主界面SToolStripMenuItem.Size = new Size(148, 22);
            this.显示主界面SToolStripMenuItem.Text = "显示主界面(&S)";
            this.显示主界面SToolStripMenuItem.Click += new EventHandler(this.显示主界面SToolStripMenuItem_Click);
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new Size(148, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            this.退出XToolStripMenuItem.Click += new EventHandler(this.退出XToolStripMenuItem_Click);
            this.AutoScaleDimensions = new SizeF(6f, 12f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.White;
            this.BackgroundImage = (Image)Resources.bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(404, 524);
            this.Controls.Add((Control)this.tableLayoutPanel3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Name = nameof(FormMain);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "游戏加速";
            this.FormClosing += new FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new EventHandler(this.FormMain_Load);
            this.MouseDown += new MouseEventHandler(this.FormMain_MouseDown);
            this.MouseUp += new MouseEventHandler(this.FormMain_MouseUp);
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.MainContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public delegate void UpdateSSTapUiDelegate(int width, int height);

        public delegate void UpdateUserInfoDelegate();

        public delegate void ExitLabelDelegate();
    }
}
