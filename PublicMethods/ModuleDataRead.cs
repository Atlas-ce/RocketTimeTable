using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicMethods
{
    public class ModuleDataRead
    {
        public static string GetModuleDataStorage(string ModuleName)
        {
            string StorageAddress = FileSystem.Current.AppDataDirectory;
            string JsonContent = File.ReadAllText(StorageAddress + "\\" + ModuleName + ".txt");
            return JsonContent;
        }
    }
}
