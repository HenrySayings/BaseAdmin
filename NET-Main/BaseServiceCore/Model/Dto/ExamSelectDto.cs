using BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto
{
    public class ExamSelectDto
    {
        public Guid SelectId { get; set; }
        public bool Type { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
        public List<Answer> answerSheet { get; set; }
    }
    public class Answer
    {
        public string value { get; set; }
        public string Name { get; set; }
    }
    public class ExamSelectQueryDto : PagerInfo
    {
    }
}
