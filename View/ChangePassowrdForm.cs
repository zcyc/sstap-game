using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SSTap.View
{
    public class ChangePassowrdForm : Form
    {
        private IContainer components = (IContainer)null;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox CurrentPasswordText;
        private TextBox NewPasswordText;
        private TextBox ConfirmPasswordText;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Button button1;
        private Button button2;

        public ChangePassowrdForm()
        {
            this.InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.CurrentPasswordText.Text == "")
            {
                int num = (int)MessageBox.Show("当前密码不能为空");
                this.CurrentPasswordText.Focus();
            }
            else if (this.NewPasswordText.Text == "")
            {
                int num = (int)MessageBox.Show("新密码不能为空");
                this.NewPasswordText.Focus();
            }
            else if (this.ConfirmPasswordText.Text != this.NewPasswordText.Text)
            {
                int num = (int)MessageBox.Show("两次输入的新密码必须一致");
                this.ConfirmPasswordText.Focus();
            }
            else if (Program.loginController.userInfo.ChangePassword(this.CurrentPasswordText.Text, this.NewPasswordText.Text))
            {
                int num = (int)MessageBox.Show("修改成功，请重新登录");
                Application.Restart();
            }
            else
            {
                int num1 = (int)MessageBox.Show("修改失败");
            }
        }

        private void ChangePassowrdForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = (IButtonControl)this.button1;
            this.CancelButton = (IButtonControl)this.button2;
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.CurrentPasswordText = new TextBox();
            this.NewPasswordText = new TextBox();
            this.ConfirmPasswordText = new TextBox();
            this.label3 = new Label();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.button1 = new Button();
            this.button2 = new Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel1.Controls.Add((Control)this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add((Control)this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add((Control)this.CurrentPasswordText, 1, 0);
            this.tableLayoutPanel1.Controls.Add((Control)this.NewPasswordText, 1, 1);
            this.tableLayoutPanel1.Controls.Add((Control)this.ConfirmPasswordText, 1, 2);
            this.tableLayoutPanel1.Controls.Add((Control)this.label3, 0, 2);
            this.tableLayoutPanel1.Location = new Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.Size = new Size(260, 81);
            this.tableLayoutPanel1.TabIndex = 0;
            this.label1.Anchor = AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前密码";
            this.label2.Anchor = AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 34);
            this.label2.Name = "label2";
            this.label2.Size = new Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "新密码";
            this.CurrentPasswordText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.CurrentPasswordText.Location = new Point(74, 3);
            this.CurrentPasswordText.Name = "CurrentPasswordText";
            this.CurrentPasswordText.PasswordChar = '*';
            this.CurrentPasswordText.Size = new Size(183, 21);
            this.CurrentPasswordText.TabIndex = 3;
            this.NewPasswordText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.NewPasswordText.Location = new Point(74, 30);
            this.NewPasswordText.Name = "NewPasswordText";
            this.NewPasswordText.PasswordChar = '*';
            this.NewPasswordText.Size = new Size(183, 21);
            this.NewPasswordText.TabIndex = 4;
            this.ConfirmPasswordText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ConfirmPasswordText.Location = new Point(74, 57);
            this.ConfirmPasswordText.Name = "ConfirmPasswordText";
            this.ConfirmPasswordText.PasswordChar = '*';
            this.ConfirmPasswordText.Size = new Size(183, 21);
            this.ConfirmPasswordText.TabIndex = 5;
            this.label3.Anchor = AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 61);
            this.label3.Name = "label3";
            this.label3.Size = new Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "确认新密码";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Controls.Add((Control)this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add((Control)this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Location = new Point(6, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 51f));
            this.tableLayoutPanel2.Size = new Size(266, 126);
            this.tableLayoutPanel2.TabIndex = 1;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel3.Controls.Add((Control)this.button1, 0, 0);
            this.tableLayoutPanel3.Controls.Add((Control)this.button2, 1, 0);
            this.tableLayoutPanel3.Location = new Point(3, 90);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel3.Size = new Size(257, 31);
            this.tableLayoutPanel3.TabIndex = 1;
            this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.button1.Location = new Point(50, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Location = new Point(131, 3);
            this.button2.Name = "button2";
            this.button2.Size = new Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.AutoScaleDimensions = new SizeF(6f, 12f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ClientSize = new Size(279, 136);
            this.ControlBox = false;
            this.Controls.Add((Control)this.tableLayoutPanel2);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ImeMode = ImeMode.Disable;
            this.MinimizeBox = false;
            this.Name = nameof(ChangePassowrdForm);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "修改密码";
            this.Load += new EventHandler(this.ChangePassowrdForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
