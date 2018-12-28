using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class DateUtils
    {
        public static bool Intersects(DateTime x_Start, DateTime x_End, DateTime y_Start, DateTime y_End)
        { 
            // end of a is earlier than end of b
            // start of b is before end of a
            if (x_End <= y_End && y_Start < x_End)
                return true;

            // start of a is before end of b
            if (y_End <= x_End && x_Start < y_End)
                return true;

            if (x_Start == y_Start && x_End == y_End)
                return true;

            return false;
        }
    }
}
