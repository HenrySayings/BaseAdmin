using BaseServiceCore.Model.Enums;

namespace BaseServiceCore.Model
{
    /// <summary>
    /// 题目分值表
    /// </summary>
    [SugarTable("Examquestion", "题目分值管理")]
    [Tenant("0")]
    public class Examquestion
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ExamquestionId { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public decimal Score { get; set; }
    }
}
