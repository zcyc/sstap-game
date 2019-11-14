// Decompiled with JetBrains decompiler
// Type: SSTap.Controller.SSTapController
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using PInvoke;
using SSTap.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using User32 = SSTap.Util.User32;

namespace SSTap.Controller
{
    public class SSTapController
    {
        public IntPtr ssTapHwnd = IntPtr.Zero;
        public int SSTapPid = 0;
        public IntPtr adBrowswer = IntPtr.Zero;
        public IntPtr trafficHwnd = IntPtr.Zero;
        public IntPtr statusHwnd = IntPtr.Zero;
        public Process SSTapProcess;

        public void UpdateUserNameHandler(string userName)
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 970);
            User32.SendMessage(dlgItem, 12u, 0, "用户名: " + userName);
            PInvoke.User32.SetWindowPos(dlgItem, (IntPtr)(-1), 0, 0, 0, 0, (PInvoke.User32.SetWindowPosFlags)2);
        }

        public void UpdateFlowInfoHandler(string flow)
        {
            string lParam = "流量: " + flow;
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 971);
            PInvoke.User32.ShowWindow(dlgItem, (PInvoke.User32.WindowShowStyle)5);
            User32.SendMessage(dlgItem, 12u, 0, lParam);
        }

        public void GetSsTap()
        {
            while (this.ssTapHwnd == IntPtr.Zero)
            {
                this.ssTapHwnd = PInvoke.User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "SSTap_WND_Class_150213", null);
            }
        }

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

        public void StartSSTap()
        {
            this.SSTapProcess = Process.Start(Config.SSTapAppName);
            this.SSTapPid = this.SSTapProcess.Id;
            this.GetSsTap();
            this.SSTapProcess.Exited += new EventHandler(this.SSTapProcess_Exited);
        }

        private void SSTapProcess_Exited(object sender, EventArgs e)
        {
            Program.formMain.Close();
        }

        public void StopSSTap()
        {
            this.SSTapProcess.Kill();
        }

        public void SetSSTapSublink()
        {
            string path = Application.StartupPath + "\\config\\ssrsubscriptions.ini";
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                using (StreamWriter streamWriter = new StreamWriter((Stream)File.Open(path, FileMode.Create)))
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
                Console.WriteLine(ex);
            }
        }

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

        public RECT GetSSapMainRect()
        {
            RECT result;
            PInvoke.User32.GetWindowRect(this.ssTapHwnd, out result);
            return result;
        }

        public void ShowSSTapForm()
        {
            bool flag = !PInvoke.User32.IsWindowVisible(this.ssTapHwnd);
            if (flag)
            {
                PInvoke.User32.ShowWindow(this.ssTapHwnd, (PInvoke.User32.WindowShowStyle)9);
            }
        }

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

        public void UnSetSSTapSublink()
        {
            string path = "config/ssrsubscriptions.ini";
            try
            {
                if (!File.Exists(path))
                    return;
                File.Delete(path);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SSTapPing()
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 1131);
            while (PInvoke.User32.IsWindowVisible(this.adBrowswer))
            {
                Thread.Sleep(500);
            }
            User32.SendMessage(this.ssTapHwnd, 273u, 3535, 0);
        }

        public void GetSSTapTrafficWindow()
        {
            IntPtr dlgItem = User32.GetDlgItem(this.ssTapHwnd, 1018);
            IntPtr nextWindow = PInvoke.User32.GetNextWindow(dlgItem, (PInvoke.User32.GetNextWindowCommands)2);
            string className = PInvoke.User32.GetClassName(nextWindow, 256);
            this.trafficHwnd = nextWindow;
        }

        public void GetSSTapStatusWindow()
        {
            this.statusHwnd = User32.GetDlgItem(this.ssTapHwnd, 1071);
        }

        public IntPtr GetWarningDialogCancelButton()
        {
            List<IntPtr> allChildHandles = this.GetAllChildHandles(IntPtr.Zero);
            List<IntPtr> numList = new List<IntPtr>();
            foreach (IntPtr hDlg in allChildHandles)
            {
                if (User32.GetDlgItem(hDlg, 1630) != IntPtr.Zero)
                    return User32.GetDlgItem(hDlg, 2);
            }
            return IntPtr.Zero;
        }

        public void CloseWaringDialog(IntPtr cancelPtr)
        {
            User32.SendMessage(cancelPtr, 513U, 0, 0);
            User32.SendMessage(cancelPtr, 514U, 0, 0);
        }

        public List<IntPtr> GetAllChildHandles(IntPtr parent)
        {
            List<IntPtr> numList = new List<IntPtr>();
            GCHandle gcHandle = GCHandle.Alloc((object)numList);
            IntPtr intPtr = GCHandle.ToIntPtr(gcHandle);
            try
            {
                SSTapController.EnumWindowProc callback = new SSTapController.EnumWindowProc(this.EnumWindow);
                SSTapController.EnumChildWindows(parent, callback, intPtr);
            }
            finally
            {
                gcHandle.Free();
            }
            return numList;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcHandle = GCHandle.FromIntPtr(lParam);
            if (gcHandle.Target == null)
                return false;
            (gcHandle.Target as List<IntPtr>).Add(hWnd);
            return true;
        }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(
          IntPtr window,
          SSTapController.EnumWindowProc callback,
          IntPtr lParam);

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
    }
}
