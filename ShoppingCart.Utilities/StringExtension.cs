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

            if (int.TryParse(str, out parsedInt))
            {
                return parsedInt;
            }

            return defaultValue;
            
        }

        public static float ToFloat(this string str) 
        {
            double parsedDouble;

            if (double.TryParse(str, out parsedDouble))
            {
                return (float)parsedDouble;
            }

            return 0f;
        }
    }
}
