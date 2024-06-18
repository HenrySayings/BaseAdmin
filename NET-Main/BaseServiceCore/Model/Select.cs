using MiniExcelLibs.Attributes;

namespace BaseServiceCore.Model
{
    /// <summary>
    /// 选择题/多选题
    /// </summary>
    [SugarTable("Select", "选择题/多选题")]
    [Tenant("0")]
    public class Select : BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        [ExcelIgnore]
        public Guid SelectId { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "题目", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string Question { get; set; }
        [SugarColumn(Length = 999, ColumnDescription = "答案", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string Answer { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "是否单选", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public bool IsSingle { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "章节编号", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string ChapeterId { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "A", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string OptionA { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "B", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string OptionB { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "C", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string OptionC { get; set; }
        [SugarColumn(Length = 255, ColumnDescription = "D", ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string OptionD { get; set; }
        [ExcelIgnore]
        public string Ideas { get; set; }
    }
}
