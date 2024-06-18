
using BaseService.Business.IBusinessService;
using BaseServiceCore.Model;
using MiniExcelLibs.Attributes;

namespace BaseModel.Dto
{
    /// <summary>
    /// 选择题/多选题查询对象
    /// </summary>
    public class SelectQueryDto : PagerInfo 
    {
    }
    /// <summary>
    /// 选择题/多选题输入输出对象
    /// </summary>
    public class SelectDto
    {
        public Guid SelectId { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }
        public Guid ChapeterId { get; set; }

        public string OptionA { get; set; }

        public string OptionB { get; set; }

        public string OptionC { get; set; }

        public string OptionD { get; set; }
        public string Chapter { get; set; }
    }
}