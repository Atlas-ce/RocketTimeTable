using Newtonsoft.Json;
using PublicMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSettingModule
{
    public class SettingLocalDataStorage
    {
        public List<UserSetiingAttributes> UserSettings()
        {
            List<UserSetiingAttributes> userSettings = new List<UserSetiingAttributes>();
            string jsonContent = ModuleDataRead.GetModuleDataStorage("UserSettingData");
            userSettings = JsonConvert.DeserializeObject<List<UserSetiingAttributes>>(jsonContent);
            return userSettings;
        }
    }
}
