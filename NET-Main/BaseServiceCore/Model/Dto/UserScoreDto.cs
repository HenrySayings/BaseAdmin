
using MiniExcelLibs.Attributes;

namespace BaseModel.Dto
{
    /// <summary>
    /// 成绩表查询对象
    /// </summary>
    public class UserScoreQueryDto : PagerInfo 
    {
    }

    /// <summary>
    /// 成绩表输入输出对象
    /// </summary>
    public class UserScoreDto
    {
        public decimal Score { get; set; }
        public string Name { get; set; }
        public string Exams { get; set; }
        public DateTime? DateTime { get; set; }
    }
}