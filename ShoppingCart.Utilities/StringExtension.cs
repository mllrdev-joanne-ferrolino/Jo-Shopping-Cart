using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Utilities
{
    public static class StringExtension
    {
        public static int ToInt(this string str, int defaultValue = 0) 
        {
            int parsedInt;
            return int.TryParse(str, out parsedInt) ? parsedInt : defaultValue;
            
        }

        public static float ToFloat(this string str, float defaultValue = 0f) 
        {
            double parsedDouble;
            return double.TryParse(str, out parsedDouble) ? (float)parsedDouble : defaultValue;
        }

        public static DateTime ToDateTime(this string str, DateTime defaultValue = new DateTime()) 
        {
            DateTime parsedDateTime;
            return DateTime.TryParse(str, out parsedDateTime) ? parsedDateTime : defaultValue;
        }
    }
}
