using Newtonsoft.Json;
using PublicMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSemesterSettingDataModel
{
    public class SemesterSettingLocalDataStorage
    {
        public List<UserSemesterSettingAttributes> UserSettings()
        {
            List<UserSemesterSettingAttributes> userSettings = new List<UserSemesterSettingAttributes>();
            string jsonContent = ModuleDataRead.GetModuleDataStorage("UserSemesterSettingData");
            userSettings = JsonConvert.DeserializeObject<List<UserSemesterSettingAttributes>>(jsonContent);
            return userSettings;
        }
    }
}
