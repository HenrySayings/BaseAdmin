using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto
{
    public class ImportSelectDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public Guid ChapeterId { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
    }
}
