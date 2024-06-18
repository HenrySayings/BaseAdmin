
using MiniExcelLibs.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseModel.Dto
{
    /// <summary>
    /// 课程查询对象
    /// </summary>
    public class CourseQueryDto : PagerInfo 
    {
    }

    /// <summary>
    /// 课程输入输出对象
    /// </summary>
    public class CourseDto
    {
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "CourseName不能为空")]
        public string CourseName { get; set; }


        public string CreateUser { get; set; }
        public string PicUrl { get; set; }

        public DateTime? CreateTime { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool IsDeleted { get; set; }



        [ExcelColumn(Name = "IsDeleted")]
        public string IsDeletedLabel { get; set; }
    }
}