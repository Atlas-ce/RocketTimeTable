
using Newtonsoft.Json;
using PublicMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDataModule.LocalData
{
    public class LocalDataStorage
    {
        public void StorageCourseData(string courseData)
        {
            ModuleDataStorage.UseFileStream("UserCourseData", courseData);
        }
    }
}
