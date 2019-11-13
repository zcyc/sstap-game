using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PInvoke;
using SSTap.Util;
using User32 = SSTap.Util.User32;

namespace SSTap.Controller
{
    // Token: 0x02000025 RID: 37
    public class SSTapController
    {
        // Token: 0x060000C7 RID: 199 RVA: 0x00009AE0 File Offset: 0x00007CE0
        public void UpdateUserNameHandler(string userName)
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 970);
            User32.SendMessage(dlgItem, 12u, 0, "用户名: " + userName);
            PInvoke.User32.SetWindowPos(dlgItem, (IntPtr)(-1), 0, 0, 0, 0, (PInvoke.User32.SetWindowPosFlags)2);
        }

        // Token: 0x060000C9 RID: 201 RVA: 0x00009B64 File Offset: 0x00007D64
        public void UpdateFlowInfoHandler(string flow)
        {
            string lParam = "流量: " + flow;
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 971);
            PInvoke.User32.ShowWindow(dlgItem, (PInvoke.User32.WindowShowStyle)5);
            User32.SendMessage(dlgItem, 12u, 0, lParam);
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00009BA4 File Offset: 0x00007DA4
        public void GetSsTap()
        {
            while (this.ssTapHwnd == IntPtr.Zero)
            {
                this.ssTapHwnd = PInvoke.User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "SSTap_WND_Class_150213", null);
            }
        }

        // Token: 0x060000CB RID: 203 RVA: 0x00009BE8 File Offset: 0x00007DE8
        public void HideAdvertisements()
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 1122);
            while (dlgItem == IntPtr.Zero || !PInvoke.User32.IsWindowVisible(dlgItem))
            {
                Thread.Sleep(100);
            }
            FindWindow findWindow = new FindWindow(this.ssTapHwnd, "Shell Embedding", null, 1);
            this.adBrowswer = findWindow.p_hWnd;
        }

        // Token: 0x060000CC RID: 204 RVA: 0x00009C50 File Offset: 0x00007E50
        public void StartSSTap()
        {
            this.SSTapProcess = Process.Start(Config.SSTapAppName);
            this.SSTapPid = this.SSTapProcess.Id;
            this.GetSsTap();
            this.SSTapProcess.Exited += this.SSTapProcess_Exited;
        }

        // Token: 0x060000CD RID: 205 RVA: 0x00009C9E File Offset: 0x00007E9E
        private void SSTapProcess_Exited(object sender, EventArgs e)
        {
            Program.formMain.Close();
        }

        // Token: 0x060000CE RID: 206 RVA: 0x00009CAC File Offset: 0x00007EAC
        public void StopSSTap()
        {
            this.SSTapProcess.Kill();
        }

        // Token: 0x060000CF RID: 207 RVA: 0x00009CBC File Offset: 0x00007EBC
        public void SetSSTapSublink()
        {
            string path = Application.StartupPath + "\\config\\ssrsubscriptions.ini";
            try
            {
                bool flag = File.Exists(path);
                if (flag)
                {
                    File.Delete(path);
                }
                using (StreamWriter streamWriter = new StreamWriter(File.Open(path, FileMode.Create)))
                {
                    streamWriter.WriteLine("[basic]");
                    streamWriter.WriteLine("groupname1=" + Config.BaseUrl);
                    streamWriter.WriteLine("url1=" + Program.loginController.userInfo.SubLink);
                    streamWriter.WriteLine("recently_updated1=");
                    streamWriter.WriteLine("frequency1=0");
                    streamWriter.WriteLine("default_params_of_obfs1=");
                    streamWriter.WriteLine("p_type1=5");
                    streamWriter.WriteLine("p_server1=");
                    streamWriter.WriteLine("p_server_port1=0");
                    streamWriter.WriteLine("p_username1=");
                    streamWriter.WriteLine("p_password1=");
                    streamWriter.WriteLine("p_protocol1=");
                    streamWriter.WriteLine("p_protocolparam1=");
                    streamWriter.WriteLine("p_obfs1=");
                    streamWriter.WriteLine("p_obfsparam1=");
                    streamWriter.WriteLine("p_method1=");
                    streamWriter.Flush();
                }
            }
            catch (IOException ex)
            {
            }
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00009E2C File Offset: 0x0000802C
        public RECT GetClientRect()
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 1168);
            while (!PInvoke.User32.IsWindowVisible(dlgItem))
            {
                Thread.Sleep(100);
            }
            RECT result;
            PInvoke.User32.GetWindowRect(dlgItem, out result);
            return result;
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00009E74 File Offset: 0x00008074
        public RECT GetSSapMainRect()
        {
            RECT result;
            PInvoke.User32.GetWindowRect(this.ssTapHwnd, out result);
            return result;
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x00009E98 File Offset: 0x00008098
        public void ShowSSTapForm()
        {
            bool flag = !PInvoke.User32.IsWindowVisible(this.ssTapHwnd);
            if (flag)
            {
                PInvoke.User32.ShowWindow(this.ssTapHwnd, (PInvoke.User32.WindowShowStyle)9);
            }
        }

        // Token: 0x060000D3 RID: 211 RVA: 0x00009EC8 File Offset: 0x000080C8
        public RECT GetSSapAdRect()
        {
            while (!PInvoke.User32.IsWindowVisible(User32.GetDlgItem(this.ssTapHwnd, 1122)))
            {
                Thread.Sleep(100);
            }
            RECT result;
            PInvoke.User32.GetWindowRect(User32.GetDlgItem(this.ssTapHwnd, 1122), out result);
            return result;
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x00009F1C File Offset: 0x0000811C
        public void UnSetSSTapSublink()
        {
            string path = "config/ssrsubscriptions.ini";
            try
            {
                bool flag = File.Exists(path);
                if (flag)
                {
                    File.Delete(path);
                }
            }
            catch (IOException ex)
            {
            }
        }

        // Token: 0x060000D5 RID: 213 RVA: 0x00009F5C File Offset: 0x0000815C
        public void SSTapPing()
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 1131);
            while (PInvoke.User32.IsWindowVisible(this.adBrowswer))
            {
                Thread.Sleep(500);
            }
            User32.SendMessage(this.ssTapHwnd, 273u, 3535, 0);
        }

        // Token: 0x060000D6 RID: 214 RVA: 0x00009FB0 File Offset: 0x000081B0
        public void GetSSTapTrafficWindow()
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 1018);
            IntPtr nextWindow = PInvoke.User32.GetNextWindow(dlgItem, (PInvoke.User32.GetNextWindowCommands)2);
            string className = PInvoke.User32.GetClassName(nextWindow, 256);
            this.trafficHwnd = nextWindow;
        }

        // Token: 0x060000D7 RID: 215 RVA: 0x00009FEA File Offset: 0x000081EA
        public void GetSSTapStatusWindow()
        {
            this.statusHwnd = User32.GetDlgItem(this.ssTapHwnd, 1071);
        }

        // Token: 0x060000D8 RID: 216 RVA: 0x0000A004 File Offset: 0x00008204
        public IntPtr GetWarningDialogCancelButton()
        {
            List<IntPtr> allChildHandles = this.GetAllChildHandles(IntPtr.Zero);
            List<IntPtr> list = new List<IntPtr>();
            foreach (IntPtr hDlg in allChildHandles)
            {
                IntPtr dlgItem = User32.GetDlgItem(hDlg, 1630);
                bool flag = dlgItem != IntPtr.Zero;
                if (flag)
                {
                    return User32.GetDlgItem(hDlg, 2);
                }
            }
            return IntPtr.Zero;
        }

        // Token: 0x060000D9 RID: 217 RVA: 0x0000A098 File Offset: 0x00008298
        public void CloseWaringDialog(IntPtr cancelPtr)
        {
            User32.SendMessage(cancelPtr, 513u, 0, 0);
            User32.SendMessage(cancelPtr, 514u, 0, 0);
        }

        // Token: 0x060000DA RID: 218 RVA: 0x0000A0B8 File Offset: 0x000082B8
        public List<IntPtr> GetAllChildHandles(IntPtr parent)
        {
            List<IntPtr> list = new List<IntPtr>();
            GCHandle value = GCHandle.Alloc(list);
            IntPtr lParam = GCHandle.ToIntPtr(value);
            try
            {
                SSTapController.EnumWindowProc callback = new SSTapController.EnumWindowProc(this.EnumWindow);
                SSTapController.EnumChildWindows(parent, callback, lParam);
            }
            finally
            {
                value.Free();
            }
            return list;
        }

        // Token: 0x060000DB RID: 219 RVA: 0x0000A118 File Offset: 0x00008318
        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gchandle = GCHandle.FromIntPtr(lParam);
            bool flag = gchandle.Target == null;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                List<IntPtr> list = gchandle.Target as List<IntPtr>;
                list.Add(hWnd);
                result = true;
            }
            return result;
        }

        // Token: 0x060000DC RID: 220
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, SSTapController.EnumWindowProc callback, IntPtr lParam);

        // Token: 0x040000C3 RID: 195
        public IntPtr ssTapHwnd = IntPtr.Zero;

        // Token: 0x040000C4 RID: 196
        public Process SSTapProcess;

        // Token: 0x040000C5 RID: 197
        public int SSTapPid = 0;

        // Token: 0x040000C6 RID: 198
        public IntPtr adBrowswer = IntPtr.Zero;

        // Token: 0x040000C7 RID: 199
        public IntPtr trafficHwnd = IntPtr.Zero;

        // Token: 0x040000C8 RID: 200
        public IntPtr statusHwnd = IntPtr.Zero;

        // Token: 0x02000026 RID: 38
        // (Invoke) Token: 0x060000DE RID: 222
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
    }
}
