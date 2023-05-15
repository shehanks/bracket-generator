using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Utilities.Helpers
{
    public static class NumberHelper
    {
        public static bool IsPowerOfTwo(this int number)
        {
            double log = Math.Log(number, 2);
            double pow = Math.Pow(2, Math.Round(log));
            return pow == number;
        }
        
        public static bool IsPowerOfTwo(this double number)
        {
            double log = Math.Log(number, 2);
            double pow = Math.Pow(2, Math.Round(log));
            return pow == number;
        }
    }
}
