using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto
{
    public class CommitExamInputDto
    {
        public long UserId { get; set; }
        public List<Question> Questions { get; set; }
    }
    public class Question
    {
        public Guid SelectId { get; set; }
        public string Answer { get; set; }
    }
}
