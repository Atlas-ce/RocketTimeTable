using Newtonsoft.Json;
using PublicMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyScheduleModule
{
    public class ScheduleAttributes
    {
        public string ScheduleName { get; set; } = null!;
        public DateTime ScheduleTime { get; set; }
        public int ScheduleDescription { get; set; }
    }  
}
