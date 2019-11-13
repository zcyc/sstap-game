// Decompiled with JetBrains decompiler
// Type: SSTap.Model.Notice
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

namespace SSTap.Model
{
  internal class Notice
  {
    public string title;
    public string content;
    public long time;

    public Notice()
    {
      this.time = 0L;
      this.content = "";
      this.title = "";
    }
  }
}
