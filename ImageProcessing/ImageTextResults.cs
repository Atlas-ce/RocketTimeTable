using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ImageTextResults
    {
        public string? confidence { get; set; }
        public string? text { get; set; }
        public List<List<int>>? text_region { get; set; }
    }
}
