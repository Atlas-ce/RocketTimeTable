using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicMethods;

namespace CourseDataModule.LocalData
{
    public class LocalDataRead
    {
        public List<CourseAttributes> GetLocalCourseData()
        {
            List<CourseAttributes> userCourse = new List<CourseAttributes>();
            string JsonContent = ModuleDataRead.GetModuleDataStorage("UserCourseData");
            userCourse = JsonConvert.DeserializeObject<List<CourseAttributes>>(JsonContent);
            return userCourse;
        }
    }
}
