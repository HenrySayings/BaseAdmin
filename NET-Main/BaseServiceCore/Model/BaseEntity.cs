using MiniExcelLibs.Attributes;

namespace BaseServiceCore.Model
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class BaseEntity
    {
        [ExcelIgnore]
        public string CreateUser { get; set; }
        [ExcelIgnore]
        public DateTime CreateTime { get; set; }
        [ExcelIgnore]
        public string UpdateUser { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
        [ExcelIgnore]
        public bool IsDeleted { get{return false;} } 
    }
}
