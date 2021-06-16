using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.Util
{
    public enum Quarter
    {
        /// <summary>
        /// 第一季度
        /// </summary>
        [Description("1th")]
        FirstQuarter = 1,
        /// <summary>
        /// 第二季度
        /// </summary>
        [Description("2nd")]
        SecondQuarter = 2,
        /// <summary>
        /// 第三季度
        /// </summary>
        [Description("3rd")]
        ThirdQuarter = 3,
        /// <summary>
        /// 第四季度
        /// </summary>
        [Description("4th")]
        FourQuarter = 4,
    }


    public class TimeRange
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public enum Season
    {
        /// <summary>
        /// 第一季度
        /// </summary>
        [Description("第一季度")]
        Spring = 1,
        /// <summary>
        /// 第二季度
        /// </summary>
        [Description("第二季度")]
        Summer = 4,
        /// <summary>
        /// 第三季度
        /// </summary>
        [Description("第三季度")]
        Autumn = 7,
        /// <summary>
        /// 第四季度
        /// </summary>
        [Description("第四季度")]
        Winter = 10
    }

    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取当前月第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthFirstDay(DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1);
        }

        /// <summary>
        /// 获取当前月的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthLastDay(DateTime now)
        {
            now = now.AddMonths(1);
            return new DateTime(now.Year, now.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// 是否为该月1号
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static bool IsFirstDay(DateTime now)
        {
            if (now.Day == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 获取当前年的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYearFirstDay(DateTime now)
        {
            return new DateTime(now.Year, 1, 1);
        }

        /// <summary>
        /// 获取当前年的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYearLastDay(DateTime now)
        {
            return new DateTime(now.Year, 12, 31);
        }

        /// <summary>
        /// 获取某日所在周的周一所在日期
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDay(DateTime day)
        {
            int index = (int)day.DayOfWeek;
            if (0 == index)
            {
                return day.AddDays(-6);
            }
            else
            {
                return day.AddDays(1 - index);
            }
        }

        /// <summary>
        /// 获取某日所在周的周日所在日期
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime GetWeekLastDay(DateTime day)
        {
            return GetWeekFirstDay(day).AddDays(6);
        }

        /// <summary>
        /// 获取当前日期对应季度的第一天
        /// </summary>
        /// <param name="now">日期</param>
        /// <returns></returns>
        public static DateTime GetQuarterFirstDay(DateTime now)
        {
            return GetQuarterFirstDay(now.Year, GetSeaonFromTime(now));
        }

        /// <summary>
        /// 获取当前日期对应的季度
        /// </summary>
        /// <param name="now">日期</param>
        /// <returns></returns>
        public static Season GetSeaonFromTime(DateTime now)
        {
            int month = now.Month;
            var result = Season.Spring;
            if (month >= 1 && month <= 3)
            {
                result = Season.Spring;
            }

            if (month >= 4 && month <= 6)
            {
                result = Season.Summer;
            }

            if (month >= 7 && month <= 9)
            {
                result = Season.Autumn;
            }

            if (month >= 10 && month <= 12)
            {
                result = Season.Winter;
            }
            return result;
        }

        /// <summary>
        /// 获取当前日期对应的季度
        /// </summary>
        /// <param name="now">日期</param>
        /// <returns></returns>
        //public static Quarter GetQuarterFromMonth(int month)
        //{
        //    var result = Quarter.FirstQuarter;

        //    var value = Math.Ceiling(month / 3.0);

        //    if (value <= 4)
        //    {
        //        result = value.ToString().ToEnum<Quarter>();
        //    }
        //    //var result = Math.Ceiling(month / 12 * 4.0).ToStr().ToEnum<Quarter>();
        //    return result;
        //}

        /// <summary>
        /// 根据传进来的时间得到上一季度的时间
        /// </summary>
        /// <param name="now"></param>
        /// <param name="sdt"></param>
        /// <param name="edt"></param>
        public static void GetSeaonFromTimeTop(DateTime now, out DateTime sdt, out DateTime edt)
        {
            //1.判断当前是不是第一季度
            string year = now.Year.ToString();
            string sdtmonth = string.Empty;

            switch (GetSeaonFromTime(now))
            {
                case Season.Spring:
                    year = (now.Year - 1).ToString();
                    sdtmonth = "10";

                    break;
                case Season.Summer:
                    sdtmonth = "1";
                    break;
                case Season.Autumn:
                    sdtmonth = "4";
                    break;
                case Season.Winter:
                    sdtmonth = "7";
                    break;
                default:
                    break;
            }
            sdt = DateTime.Parse(string.Format("{0}-{1}-01", year, sdtmonth));
            if (sdtmonth.Equals("10"))
            {
                edt = DateTime.Parse(string.Format("{0}-{1}-01", int.Parse(year) + 1, 1));
            }
            else
            {
                edt = DateTime.Parse(string.Format("{0}-{1}-01", year, int.Parse(sdtmonth) + 3));
            }
        }

        /// <summary>
        /// 获取当前日所对应季度的最后一天
        /// </summary>
        /// <param name="now">日期</param>
        /// <returns></returns>
        public static DateTime GetQuarterLastDay(DateTime now)
        {
            return GetQuarterLastDay(now.Year, GetSeaonFromTime(now));
        }

        /// <summary>
        /// 获取当前年某季度的第一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="season">季度</param>
        /// <returns></returns>
        public static DateTime GetQuarterFirstDay(int year, Season season)
        {
            DateTime result = DateTime.Now;
            switch (season)
            {
                case Season.Spring:
                    result = new DateTime(year, 1, 1);
                    break;
                case Season.Summer:
                    result = new DateTime(year, 4, 1);
                    break;
                case Season.Autumn:
                    result = new DateTime(year, 7, 1);
                    break;
                case Season.Winter:
                    result = new DateTime(year, 10, 1);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取当前年份某季度的最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="season">季度</param>
        /// <returns></returns>
        public static DateTime GetQuarterLastDay(int year, Season season)
        {
            return GetQuarterFirstDay(year, season).AddMonths(3).AddDays(-1);
        }

        /// <summary>
        /// 获取当前日期的第一个小时对应的日期
        /// </summary>
        /// <param name="day">当前日期</param>
        /// <returns></returns>
        public static DateTime GetDayFirstHour(DateTime day)
        {
            return new DateTime(day.Year, day.Month, day.Day, 1, 0, 0);
        }

        /// <summary>
        /// 获取当前日期的最后一个小时对应的日期
        /// </summary>
        /// <param name="day">当前日期</param>
        /// <returns></returns>
        public static DateTime GetDayLastHour(DateTime day)
        {
            day = day.AddDays(1);
            return new DateTime(day.Year, day.Month, day.Day, 1, 0, 0).AddHours(-1);
        }

        public static string GetCNWeekDay(DateTime day)
        {
            string week = day.DayOfWeek.ToString();
            switch (week)
            {
                case "Monday":
                    return "星期一";
                case "Tuesday":
                    return "星期二";
                case "Wednesday":
                    return "星期三";
                case "Thursday":
                    return "星期四";
                case "Friday":
                    return "星期五";
                case "Saturday":
                    return "星期六";
                default:
                    return "星期日";
            }
        }

        /// <summary>
        /// 获取下周周一时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextMonday()
        {
            var today = DateTime.Today;
            var monday = GetWeekFirstDay(today);
            return monday.AddDays(7);
        }

        /// <summary>
        /// 得到上一周的时间
        /// </summary>
        /// <param name="outDt">上一周的开始时间</param>
        /// <param name="newDt">上一周的结束时间</param>
        public static void GetfistTime(out DateTime sdtDt, out DateTime edtDt)
        {
            sdtDt = DateTimeHelper.GetNextMonday().AddDays(-14);
            edtDt = DateTimeHelper.GetNextMonday().AddDays(-8);
        }

        /// <summary>
        /// 获取月的第一个星期一时间
        /// </summary>
        /// <param name="dayTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthFirtMonday(DateTime dayTime)
        {
            var monthFirst = GetMonthFirstDay(dayTime);
            if (monthFirst.DayOfWeek == DayOfWeek.Monday)
            {
                return monthFirst;
            }
            else
            {
                return GetWeekFirstDay(dayTime.AddDays(7));
            }
        }

        /// <summary>
        /// 时间戳转换为时间格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// 获取季度天数
        /// </summary>
        /// <returns></returns>
        public static int GetQuarterDays(DateTime time)
        {
            var season = GetSeaonFromTime(time);

            var firstDay = GetQuarterFirstDay(time.Year, season);
            var endDay = GetQuarterLastDay(time.Year, season);

            return (endDay - firstDay).Days + 1;
        }

        /// <summary>
        /// 根据枚举Quarter获取季度天数
        /// </summary>
        /// <returns></returns>
        public static int GetQuarterDaysByQuarterEnum(string quarterStr, int year)
        {
            if (string.IsNullOrWhiteSpace(quarterStr))
            {
                return 0;
            }

            quarterStr = quarterStr.Substring(0, 1);

            var season = Season.Spring;
            switch (quarterStr)
            {
                case "1":
                    break;
                case "2":
                    season = Season.Summer;
                    break;
                case "3":
                    season = Season.Autumn;
                    break;
                case "4":
                    season = Season.Winter;
                    break;
                default:
                    break;
            }

            return GetQuarterDays(new DateTime(year, (int)season, 1));
        }

        /// <summary>
        /// 根据枚举Quarter获取季度开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime? GetQuarterSdtTimeByQuarter(string quarterStr, int year)
        {
            if (string.IsNullOrEmpty(quarterStr))
            {
                return null;
            }

            quarterStr = quarterStr.Substring(0, 1);
            var month = (Convert.ToInt32(quarterStr) - 1) * 3 + 1;

            var result = new DateTime(year, month, 1);
            return result;
        }


        public static string GetPrevQuarterStr(string quarter)
        {
            string result = string.Empty;
            var list = new List<string> { "1th", "2nd", "3rd", "4th" };
            if (!string.IsNullOrEmpty(quarter))
            {
                quarter = quarter.Substring(0, 1);

                var value = Convert.ToInt32(quarter);

                if (value > 0)
                {
                    var index = value - 2;

                    if (index < 0)
                    {
                        index = list.Count - 1;
                    }

                    result = list[index];
                }
            }

            return result;
        }

        public static Dictionary<string, TimeRange> GetDateTimeRanges(DateTime startTime, DateTime endTime, int type)
        {
            var timeDic = new Dictionary<string, TimeRange>();

            if(endTime < startTime)
            {
                return timeDic;
            }

            switch (type)
            {
                // 月分割
                case 0:
                    var formatStr = "yyyy-MM";
                    TimeRange timeRange = new TimeRange();
                    timeRange.StartTime = startTime;

                    // 当月
                    if (startTime.ToString(formatStr) == endTime.ToString(formatStr))
                    {
                        timeRange.EndTime = endTime;
                        timeDic.Add(startTime.ToString(formatStr), timeRange);
                    }
                    else
                    {
                        // 跨月
                        var nextTime = GetMonthLastDay(startTime);
                        timeRange.EndTime = nextTime;
                        timeDic.Add(startTime.ToString(formatStr), timeRange);

                        nextTime = nextTime.AddDays(1);
                        while (nextTime <= endTime)
                        {
                            // 当月
                            if (nextTime.ToString(formatStr) == endTime.ToString(formatStr))
                            {
                                var timeRange0 = new TimeRange() { StartTime = nextTime, EndTime = endTime };
                                timeDic.Add(nextTime.ToString(formatStr), timeRange0);
                            }
                            else
                            {
                                var temp = nextTime;
                                TimeRange timeRange0 = new TimeRange();
                                timeRange0.StartTime = temp;
                                timeRange0.EndTime = GetMonthLastDay(temp);
                                timeDic.Add(nextTime.ToString(formatStr), timeRange0);
                            }

                            nextTime = nextTime.AddMonths(1);
                        }                        
                    }
                    break;

                // 季度分割
                case 1:
                    //TimeRange timeRange1 = new TimeRange();
                    //timeRange1.StartTime = startTime;
                    //var nextTime1 = startTime.AddDays(1 - startTime.Day).AddMonths(3).AddHours(-startTime.Hour).AddMinutes(-startTime.Minute).AddSeconds(-startTime.Second);
                    //timeRange1.EndTime = nextTime1;
                    //timeDic.Add(startTime, timeRange1);
                    //while (DateTime.Compare(startTime, endTime) < 0)
                    //{
                    //    TimeRange timeRange0 = new TimeRange();
                    //    var tmpTime = nextTime1;
                    //    timeRange0.StartTime = nextTime1;
                    //    nextTime1 = nextTime1.AddDays(1 - startTime.Day).AddMonths(3).AddHours(-startTime.Hour).AddMinutes(-startTime.Minute).AddSeconds(-startTime.Second);
                    //    timeRange0.EndTime = nextTime1;
                    //    timeDic.Add(tmpTime, timeRange0);
                    //    startTime = nextTime1;
                    //}
                    break;
            }

            return timeDic;
        }        
    }
}
