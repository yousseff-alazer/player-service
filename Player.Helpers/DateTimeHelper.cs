using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Reservation.Helpers
{
   public class DateTimeHelper
    {
        public const string ServerDateFormat = "dd/MM/yyyy";

        public const string ServerDateTimeFormat = "dd/MM/yyyy HH:mm";
        public static string ToArabicDate(DateTime dateStr)
        {
            return dateStr.ToString("dd, MMMM, yyyy", new CultureInfo("ar-AE"));
        }
    }
}
