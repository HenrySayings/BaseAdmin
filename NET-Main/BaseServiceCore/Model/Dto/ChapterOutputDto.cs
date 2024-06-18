using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto
{
    public record ChapterOutputDto
    {
        public string ChapterName { get; init; }

        public string ChapterItemName { get; init; }

        public string Course { get; init; }
        public string ChapterUrl { get; init; }

        public string Desc { get; init; }
        public string CreateUser { get; init; }

        public DateTime? CreateTime { get; init; }
    }
}
