using BaseModel;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;

namespace BaseServiceCore.Services.IService
{
    /// <summary>
    /// 邮件发送记录service接口
    /// </summary>
    public interface IEmailLogService : IBaseService<EmailLog>
    {
        PagedInfo<EmailLogDto> GetList(EmailLogQueryDto parm);

        EmailLog GetInfo(long Id);

        EmailLog AddEmailLog(EmailLog parm);

        int UpdateEmailLog(EmailLog parm);
    }
}
