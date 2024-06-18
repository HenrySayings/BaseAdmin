using BaseModel;
using BaseModel.Dto;
using BaseServiceCore;
using BaseServiceCore.Model;

namespace BaseService.Business.IBusinessService
{
    /// <summary>
    /// 成绩表service接口
    /// </summary>
    public interface IUserScoreService : IBaseService<UserScore>
    {
        PagedInfo<UserScoreDto> GetList(UserScoreQueryDto parm);

        UserScore GetInfo(Guid Id);


        UserScore AddUserScore(UserScore parm);
        int UpdateUserScore(UserScore parm);


    }
}
