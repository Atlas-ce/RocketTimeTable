using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketTimeTable
{
    public class EventClass
    {
        public delegate void UserStudentIdChange();
        public static event UserStudentIdChange? userSettingChange;
        public virtual void OnSettingChanged()
        {
            userSettingChange?.Invoke();
        }
        public void SetValue()
        {
            OnSettingChanged();
        }
    }
}
