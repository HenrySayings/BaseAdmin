using BaseModel.Business;

namespace BaseServiceCore.Model;

[SugarTable("Exam", "试卷表")]
[Tenant("0")]
public class Exam : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public Guid ExamId { get; set; }
    public string ExamName { get; set; }
    public Guid CourseId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; }
    public bool IsEnable { get; set; }
    public bool IsFinish { get; set; }
    public decimal ExamScore { get; set; }
}