using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RocketTimeTable.ViewModel
{
    public partial class MainModel:ObservableObject
    {
        [ObservableProperty]
        public double _MyX;
    }
}
