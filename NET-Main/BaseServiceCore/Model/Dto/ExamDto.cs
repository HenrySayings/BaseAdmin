
using MiniExcelLibs.Attributes;

namespace BaseModel.Dto
{
    /// <summary>
    /// 试卷表查询对象
    /// </summary>
    public class ExamQueryDto : PagerInfo 
    {
    }

    /// <summary>
    /// 试卷表输入输出对象
    /// </summary>
    public class ExamDto
    {
        public Guid ExamId { get; set; }

        public string ExamName { get; set; }

        public string Course { get; set; }

        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Duration { get; set; }

        public bool IsEnable { get; set; }

        public bool IsFinish { get; set; }

        public decimal ExamScore { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateTime { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool IsDeleted { get; set; }



        [ExcelColumn(Name = "IsEnable")]
        public string IsEnableLabel { get; set; }
    }
}