using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSettingModule
{
    public class UserSetiingAttributes
    {
        public string Defaultsemesterschedule { get; set; } = string.Empty;
        public bool NetworkPermissions { get; set; }
        public string StudentNumber { get; set; } = string.Empty;
        public bool IsExamWeek { get; set; }
        public int ExamWeekDuration { get; set; }
    }
}
