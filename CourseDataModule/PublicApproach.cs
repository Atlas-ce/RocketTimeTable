using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDataModule
{
    public class PublicApproach
    {
        public static List<CourseAttributes> AllCourseDataToDaily(List<CourseAttributes> userData)
        {
            if ((int)DateTime.Now.DayOfWeek != 0)
            {
                var ResponeData = (from data in userData where data.courseWeek == (int)DateTime.Now.DayOfWeek select data).ToList();
                return (List<CourseAttributes>)ResponeData;
            }
            else
            {
                var ResponeData = (from data in userData where data.courseWeek == 7 select data).ToList();
                return (List<CourseAttributes>)ResponeData;
            }
        }
    }
}
