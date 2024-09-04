using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ImageText
    {
        public string? msg { get; set; }
        public List<List<ImageTextResults>>? results { get; set; }
        public string? status { get; set; }
    }
}
