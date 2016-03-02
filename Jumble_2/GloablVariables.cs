using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Jumble_2
{
    public class GlobalVars
    {
        public static TimeSpan time  = new TimeSpan(0,2,0);
        public static Dictionary<string, int> successfulWords = new Dictionary<string, int>();
        public static int score;
    }
}
