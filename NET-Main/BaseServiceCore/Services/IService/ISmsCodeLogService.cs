using BaseModel;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;

namespace BaseServiceCore.Services.IService
{
    /// <summary>
    /// 短信验证码记录service接口
    /// </summary>
    public interface ISmsCodeLogService : IBaseService<SmsCodeLog>
    {
        PagedInfo<SmsCodeLogDto> GetList(SmscodeLogQueryDto parm);

        SmsCodeLog GetInfo(long Id);

        SmsCodeLog AddSmscodeLog(SmsCodeLog parm);

    }
}
