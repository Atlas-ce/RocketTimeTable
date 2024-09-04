using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSemesterSettingDataModel
{
    public class UserSemesterSettingAttributes
    {
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public int SemesterLastsWeeks { get; set; }
        //每天的课
        public int PerDayClass { get; set; }
        //下午晚上开始的时间第？节课分割线
        public int AM { get; set; }
        public int PM { get; set; }
        //每节课的持续时间
        public int CourseTime { get; set; }
        public string[] EachCourseStartTime { get; set; }
        public string[] EachCourseEndTime { get; set; }
    }
}
