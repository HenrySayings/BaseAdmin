using BaseModel;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;

namespace BaseServiceCore.Services.IService
{
    /// <summary>
    /// 用户在线时长service接口
    /// </summary>
    public interface IUserOnlineLogService : IBaseService<UserOnlineLog>
    {
        PagedInfo<UserOnlineLogDto> GetList(UserOnlineLogQueryDto parm);

        UserOnlineLog AddUserOnlineLog(UserOnlineLog parm);

        PagedInfo<UserOnlineLogDto> ExportList(UserOnlineLogQueryDto parm);
    }
}
