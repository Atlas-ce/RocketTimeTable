using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicMethods
{
    public class ModuleDataStorage
    {
       
        public static void UseFileStream(string fileName, string data)
        {
            using (FileStream fileStream = new FileStream(FileSystem.Current.AppDataDirectory + "\\" + fileName + ".txt", FileMode.Create))
            {
                UseStreamWriter(fileStream, data);
            }
        }
        public static void UseStreamWriter(FileStream fileStream, string data)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(data);
            }
        }
    }
}
