using BaseModel;
using BaseModel.Dto;
using BaseServiceCore;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;
using BaseServiceCore.Model.Dto.UniappDto;

namespace BaseService.Business.IBusinessService
{
    /// <summary>
    /// 章节管理service接口
    /// </summary>
    public interface IChapeterService : IBaseService<Chapeter>
    {
        PagedInfo<ChapeterDto> GetList(ChapeterQueryDto parm);

        Chapeter GetInfo(Guid Id);
        List<ChaperSelectDto> GetChaperSelectList();
        Chapeter AddChapeter(Chapeter parm);
        int UpdateChapeter(Chapeter parm);
        (string, object, object) ImportChapeter(List<Chapeter> list);
        Task<ChapeterUnOutputDto> GetChapeterUnInfo(Guid CouseId);

    }
}
