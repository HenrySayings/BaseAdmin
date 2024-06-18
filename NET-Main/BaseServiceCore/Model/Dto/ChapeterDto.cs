
using MiniExcelLibs.Attributes;

namespace BaseModel.Dto
{
    /// <summary>
    /// 章节管理查询对象
    /// </summary>
    public class ChapeterQueryDto : PagerInfo
    {
        public string ChapterName { get; set; }
    }

    /// <summary>
    /// 章节管理输入输出对象
    /// </summary>
    public class ChapeterDto
    {
        public Guid ChapeterId { get; set; }

        public string ChapterName { get; set; }

        public string ChapterItemName { get; set; }

        public Guid CourseId { get; set; }

        public string Course { get; set; }

        public string ChapterUrl { get; set; }

        public string Desc { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateTime { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool IsDeleted { get; set; }



        [ExcelColumn(Name = "IsDeleted")]
        public string IsDeletedLabel { get; set; }
    }
}