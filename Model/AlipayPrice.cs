﻿namespace SSTap.Model
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
