namespace ADIA.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartMonth(DateTime? dt = null)
        {
            DateTime result = DateTime.Now;
            if (dt != null)
            {
                result = DateTime.Parse(dt.ToString());
            }

            return DateTime.Parse($"01/{result.Month}/{result.Year} 00:00:00");
        }

        public static DateTime GetEndMonth(DateTime? dt = null)
        {
            DateTime result = DateTime.Now;
            if (dt != null)
            {
                result = DateTime.Parse(dt.ToString());
            }

            return DateTime.Parse(
                $"{DateTime.DaysInMonth(result.Year, result.Month)}/{result.Month}/{result.Year} 23:59:59");
        }

        public static DateTime GetStartMonth(this DateTime dt)
        {
            return DateTime.Parse($"01/{dt.Month}/{dt.Year} 00:00:00");
        }

        public static DateTime GetEndMonth(this DateTime dt)
        {
            return DateTime.Parse(
                $"{DateTime.DaysInMonth(dt.Year, dt.Month)}/{dt.Month}/{dt.Year} 23:59:59");
        }

        public static DateTime GetStartDay(this DateTime dt)
        {
            var str_dt = $"{dt.ToShortDateString()} 00:00:00";
            return DateTime.Parse(str_dt);
        }

        public static DateTime GetEndDay(this DateTime dt)
        {
            var str_dt = $"{dt.ToShortDateString()} 23:59:59";
            return DateTime.Parse(str_dt);
        }

        public static DateTime GetStartYear(this DateTime dt)
        {
            return DateTime.Parse($"01/01/{dt.Year} 00:00:00");
        }

        public static DateTime GetEndYear(this DateTime dt)
        {
            return DateTime.Parse(
                $"31/12/{dt.Year} 23:59:59");
        }
    }
}
