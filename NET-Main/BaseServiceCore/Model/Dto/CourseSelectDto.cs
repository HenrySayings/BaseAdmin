using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto
{
    public record CourseSelectDto
    {
        public Guid CourseId { get; init; }
        public string CourseName { get; init; }
    }
}
