namespace BaseServiceCore.Model
{
    /// <summary>
    /// 判断题
    /// </summary>
    [SugarTable("Judge", "判断题")]
    [Tenant("0")]
    public class Judge : BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid JudgeId { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }
        public Guid CourseId { get; set; }
        public string Ideas { get; set; }
    }
}
