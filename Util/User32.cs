using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SSTap.Util
{
    internal class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetDlgItemText(
          IntPtr hDlg,
          int nIDDlgItem,
          [Out] StringBuilder lpString,
          int nMaxCount);
    }
}
