
namespace BaseModel.Business
{
    /// <summary>
    /// 课程
    /// </summary>
    [SugarTable("Course")]
    public class Course
    {
        /// <summary>
        /// CourseId 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid CourseId { get; set; }

        /// <summary>
        /// CourseName 
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// CourseName 
        /// </summary>

        public string PicUrl { get; set; }

        /// <summary>
        /// CreateUser 
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public string CreateUser { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(InsertServerTime = true, IsOnlyIgnoreUpdate = true)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// UpdateUser 
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(UpdateServerTime = true, IsOnlyIgnoreInsert = true)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// IsDeleted 
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public bool IsDeleted { get; set; }

    }
}