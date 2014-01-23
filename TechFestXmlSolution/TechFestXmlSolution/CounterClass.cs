using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace TechFestXmlSolution
{
    
    /// <summary>
    /// class CounterClass is used to get Cpu usage and timing calculations 
    /// </summary>
    class CounterClass
    {
        public CounterClass()
        { 
        
        }
        
        PerformanceCounter counter = null;
        DateTime startTime;
        TimeSpan timeTaken;
        public  float _cpuUsage=0;
        public  List<float> _numberOfHit=new List<float>(1000) ;
       
        public float CpuUsage
        {
            get
            {
                if (counter == null)
                {
                    counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                }
                return (counter.NextValue());
            }
        }

      
       
        #region to calculate time span
        public DateTime StartTime
        {

            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
            
                
        }

        public TimeSpan TimeTaken
        {
            get
            {
                timeTaken = DateTime.Now.Subtract(startTime);
                return timeTaken;
            }
        }
        #endregion
    }
}
