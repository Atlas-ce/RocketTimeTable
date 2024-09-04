using Newtonsoft.Json;
using PublicMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyScheduleModule
{
    public class ScheduleLocalDataStorage
    {
        public List<ScheduleAttributes> UserSchedule()
        {
            List<ScheduleAttributes> result = new List<ScheduleAttributes>();
            ModuleDataStorage moduleDataStorage = new ModuleDataStorage();
            string jsonContent = moduleDataStorage.GetModuleDataStorage("UserScheduleData");
            result = JsonConvert.DeserializeObject<List<ScheduleAttributes>>(jsonContent);
            return result;
        }
    }
}
