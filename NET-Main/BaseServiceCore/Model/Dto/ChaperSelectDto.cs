using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto
{
    public record ChaperSelectDto
    {
        public Guid ChapeterId { get; init; }
        public string ChapterName { get; init; }
    }
}
