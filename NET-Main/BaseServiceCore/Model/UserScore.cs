using BaseModel.Business;

namespace BaseServiceCore.Model
{
    /// <summary>
    /// 成绩表
    /// </summary>
    [SugarTable("UserScore", "成绩表")]
    [Tenant("0")]
    public class UserScore : BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        public string UserName{ get; set; }
        public decimal Score { get; set; }
    }
}
