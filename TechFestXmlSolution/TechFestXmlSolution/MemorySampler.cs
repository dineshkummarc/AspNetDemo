using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TechFestXmlSolution
{

    public class MemorySampler
    {
        private static PerformanceCounter _Memory;

        static MemorySampler()
        {
            string appInstanceName = AppDomain.CurrentDomain.FriendlyName;
            if (appInstanceName.Length > 14)
                appInstanceName = appInstanceName.Substring(0, 14);
            _Memory = new PerformanceCounter(".NET CLR Memory", "# Total committed Bytes", appInstanceName);
        }

        public static long Sample()
        {
            long currMemUsage = _Memory.NextSample().RawValue;
            return currMemUsage;
        }

    }
}
