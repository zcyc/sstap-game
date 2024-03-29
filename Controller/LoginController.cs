﻿using SSTap.Model;
using SSTap.View;
using System.Windows.Forms;

namespace SSTap.Controller
{
    internal class LoginController
    {
        public UserInfo userInfo;

        public void ShowLogin()
        {
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
                return;
            loginForm.Close();
        }

        public DialogResult ShowAlipay(string order_type)
        {
            return DialogResult.Cancel;
        }

        public DialogResult ShowCharge()
        {
            if (this.userInfo.GetAlipayPrice() != null)
                return new ChargeForm().ShowDialog();
            int num = (int)MessageBox.Show("获取价格信息失败，请联系网站管理员");
            return DialogResult.Cancel;
        }

        public int CheckPayCallback()
        {
            return 0;
        }

        public void ShowChangePassword()
        {
            int num = (int)new ChangePassowrdForm().ShowDialog();
        }
    }
}
