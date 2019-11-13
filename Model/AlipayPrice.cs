// Decompiled with JetBrains decompiler
// Type: SSTap.Model.AlipayPrice
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

namespace SSTap.Model
{
  public class AlipayPrice
  {
    public int Hour;
    public string HourName;
    public int Day;
    public string DayName;
    public int Week;
    public string WeekName;
    public int Month;
    public string MonthName;
    public int Season;
    public string SeasonName;
    public int Year;
    public string YearName;

    public AlipayPrice()
    {
      this.DayName = "";
      this.WeekName = "5G/月";
      this.HourName = "";
      this.MonthName = "20G/月";
      this.SeasonName = "20G/月 包季";
      this.YearName = "无限包年";
      this.Hour = 0;
      this.Day = 0;
      this.Week = 0;
      this.Month = 0;
      this.Season = 0;
      this.Year = 0;
    }
  }
}
