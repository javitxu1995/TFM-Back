namespace Auxquimia.Utils
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Date helper methods.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Epoch Time....
        /// </summary>
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// DateTime to milliseconds.
        /// </summary>
        /// <param name="time">.</param>
        /// <returns>.</returns>
        public static long ToUnixTimeMilliseconds(this DateTime time)
        {
            return (long)(time - epoch).TotalMilliseconds;
        }

        /// <summary>
        /// Returns a DateTime from a unix timestam.
        /// </summary>
        /// <param name="millisecondTimeStamp">The millisecondTimeStamp<see cref="long"/>.</param>
        /// <returns>.</returns>
        public static DateTime UnixTimeMillisecondsToDateTime(this long millisecondTimeStamp)
        {
            return epoch.AddMilliseconds(millisecondTimeStamp);
        }

        /// <summary>
        /// Return the timestamp for the start of the current day.
        /// </summary>
        /// <returns>.</returns>
        public static long GetTodayUnixTimeMilliseconds()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Gets the date in timestamp as a result of adding a number of days to another date in time stamp.
        /// </summary>
        /// <param name="millisecondTimeStamp">.</param>
        /// <param name="days">.</param>
        /// <returns>.</returns>
        public static long AddDaysToUnixTimeMilliseconds(long millisecondTimeStamp, int days)
        {
            return ToUnixTimeMilliseconds(UnixTimeMillisecondsToDateTime(millisecondTimeStamp).AddDays(days));
        }

        /// <summary>
        /// Return the timestamp after sum one month to a previous timestamp.
        /// </summary>
        /// <param name="millisecondTimeStamp">.</param>
        /// <param name="months">.</param>
        /// <returns>.</returns>
        public static long AddMonthsToUnixTimeMilliseconds(long millisecondTimeStamp, int months)
        {
            return ToUnixTimeMilliseconds(UnixTimeMillisecondsToDateTime(millisecondTimeStamp).AddMonths(months));
        }

        /// <summary>
        /// Gets the Month name in Spanish.
        /// </summary>
        /// <param name="month">.</param>
        /// <returns>.</returns>
        public static string GetMonthTranslation(this int month)
        {
            return CultureInfo.GetCultureInfo(Constants.Strings.SPANISH_CULTUREINFO).DateTimeFormat.GetMonthName(month);
        }

        /// <summary>
        /// Gets the name of the month for a given Unix date.
        /// </summary>
        /// <param name="epoch">.</param>
        /// <returns>.</returns>
        public static string GetMonthName(long epoch)
        {
            var date = UnixTimeMillisecondsToDateTime(epoch);
            return date.ToString("MMMM", new CultureInfo(Constants.Strings.SPANISH_CULTUREINFO)).ToUpperInvariant();
        }

        /// <summary>
        /// Returns a string from a unix timestam with specific format.
        /// </summary>
        /// <param name="millisecondTimeStamp">.</param>
        /// <param name="format">.</param>
        /// <returns>.</returns>
        public static string UnixTimeMillisecondsToString(long? millisecondTimeStamp, string format)
        {
            string result = String.Empty;
            if (millisecondTimeStamp.HasValue)
            {
                result = UnixTimeMillisecondsToDateTime(millisecondTimeStamp.Value).ToString(format, CultureInfo.InvariantCulture);
            }
            return result;
        }

        /// <summary>
        /// Returns a string from a unix timestam with specific format.
        /// </summary>
        /// <param name="dateTime">The dateTime<see cref="DateTime"/>.</param>
        /// <param name="format">.</param>
        /// <returns>.</returns>
        public static string DateTimeToString(DateTime dateTime, string format)
        {
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The DecimalToBcd.
        /// </summary>
        /// <param name="dec">The dec<see cref="int"/>.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        public static byte DecimalToBcd(int dec)
        {
            if (dec > 99)
            {
                throw new ArgumentOutOfRangeException("dec", "Number is above 99");
            }
            return (byte)(((dec / 10) << 4) + (dec % 10));
        }

        /// <summary>
        /// The BcdToDecimal.
        /// </summary>
        /// <param name="bcd">The bcd<see cref="byte"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int BcdToDecimal(byte bcd)
        {
            return ((bcd >> 4) * 10) + bcd % 16;
        }

        /// <summary>
        /// The BCDToDateTime.
        /// </summary>
        /// <param name="byteDate">The byteDate<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime BCDToDateTime(byte[] byteDate)
        {
            int year = DateHelper.BcdToDecimal(byteDate[0]) + 2000;
            int month = DateHelper.BcdToDecimal(byteDate[1]);
            int day = DateHelper.BcdToDecimal(byteDate[2]);
            int hour = DateHelper.BcdToDecimal(byteDate[3]);
            int minute = DateHelper.BcdToDecimal(byteDate[4]);
            int second = DateHelper.BcdToDecimal(byteDate[5]);
            DateTime datetime = new DateTime(year, month, day, hour, minute, second);
            return datetime;
        }

        /// <summary>
        /// The BCDToLong.
        /// </summary>
        /// <param name="byteDate">The byteDate<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public static long BCDToLong(byte[] byteDate)
        {
            if (IsDefaultBDCDate(byteDate))
            {
                return default(long);
            }
            DateTime dateTime = BCDToDateTime(byteDate);
            return ToUnixTimeMilliseconds(dateTime);
        }

        /// <summary>
        /// The IsDefaultBDCDate.
        /// </summary>
        /// <param name="bcdDate">The bcdDate<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsDefaultBDCDate(byte[] bcdDate)
        {
            if (bcdDate == null || bcdDate.Length != 8)
            {
                return true;
            }

            for (int i = 0; i < 8; i++)
            {
                if (bcdDate[i] != default(byte)) { return false; }
            }
            return true;
        }

        /// <summary>
        /// The DateTimeToBCD.
        /// </summary>
        /// <param name="date">The date<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] DateTimeToBCD(DateTime date)
        {
            byte[] bDate;
            byte year = DateHelper.DecimalToBcd(date.Year - 2000);
            byte month = DateHelper.DecimalToBcd(date.Month);
            byte day = DateHelper.DecimalToBcd(date.Day);
            byte hour = DateHelper.DecimalToBcd(date.Hour);
            byte minute = DateHelper.DecimalToBcd(date.Minute);
            byte second = DateHelper.DecimalToBcd(date.Second);
            byte LSD = 0;
            byte DOW = 1;
            bDate = new byte[8]
            {
                year, month, day, hour, minute, second, LSD, DOW
            };
            return bDate;
        }
    }
}
