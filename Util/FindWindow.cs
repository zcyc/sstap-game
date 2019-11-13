// Decompiled with JetBrains decompiler
// Type: SSTap.Util.FindWindow
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using System;
using System.Runtime.InteropServices;

namespace SSTap.Util
{
  internal class FindWindow
  {
    private string m_classname;
    private string m_caption;
    private DateTime start;
    private int m_timeout;
    private IntPtr m_hWnd;
    public IntPtr p_hWnd;
    private bool m_IsTimeOut;

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(
      IntPtr window,
      FindWindow.EnumWindowProc callback,
      IntPtr i);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindowEx(
      IntPtr hwndParent,
      IntPtr hwndChildAfter,
      string lpszClass,
      string lpszWindow);

    public IntPtr FoundHandle
    {
      get
      {
        return this.m_hWnd;
      }
    }

    public bool IsTimeOut
    {
      get
      {
        return this.m_IsTimeOut;
      }
      set
      {
        this.m_IsTimeOut = value;
      }
    }

    public FindWindow(IntPtr hwndParent, string classname, string caption, int timeout)
    {
      this.m_hWnd = IntPtr.Zero;
      this.p_hWnd = IntPtr.Zero;
      this.m_classname = classname;
      this.m_caption = caption;
      this.m_timeout = timeout;
      this.start = DateTime.Now;
      this.FindChildClassHwnd(hwndParent, IntPtr.Zero);
    }

    private bool FindChildClassHwnd(IntPtr hwndParent, IntPtr lParam)
    {
      FindWindow.EnumWindowProc callback = new FindWindow.EnumWindowProc(this.FindChildClassHwnd);
      this.p_hWnd = hwndParent;
      IntPtr windowEx = FindWindow.FindWindowEx(hwndParent, IntPtr.Zero, this.m_classname, this.m_caption);
      if (windowEx != IntPtr.Zero)
      {
        this.m_hWnd = windowEx;
        this.m_IsTimeOut = false;
        return false;
      }
      FindWindow.EnumChildWindows(hwndParent, callback, IntPtr.Zero);
      return true;
    }

    private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);
  }
}
