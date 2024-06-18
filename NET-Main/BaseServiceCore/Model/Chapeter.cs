using BaseModel.Business;

namespace BaseServiceCore.Model
{
    [SugarTable("Chapeter", "章节管理")]
    [Tenant("0")]
    public class Chapeter : BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ChapeterId { get; set; }
        public string ChapterName { get; set; }
        public string ChapterItemName { get; set; }
        public Guid CourseId { get; set; }
        public string ChapterUrl { get; set; }
        public string Desc { get; set; }
    }
}
