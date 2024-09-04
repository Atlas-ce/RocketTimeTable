using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDataModule
{
    public class CourseAttributes
    {
        public int courseID { get; set; }
        //课程基本属性
        public string? courseName { get; set; }
        public string? courseClassRoom { get; set; } 
        public string? courseInstructor { get; set; }
        //public string courseStartTimeAndEndTime { get; set; } = string.Empty;
        //判断属性(每天的第？节开始和结束)
        public int courseStartRanking { get; set; }
        public int courseEndRanking { get; set; }
        public string? CourseStartTime { get; set; }
        public string? CourseEndTime { get; set; }
        public int courseWeek { get; set; }
        //课程上课的周
        public string? courseDuration { get; set; }
    }
}
