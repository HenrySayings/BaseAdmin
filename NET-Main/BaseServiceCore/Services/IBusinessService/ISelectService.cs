using BaseModel;
using BaseModel.Dto;
using BaseServiceCore;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;

namespace BaseService.Business.IBusinessService
{
    /// <summary>
    /// 选择题/多选题service接口
    /// </summary>
    public interface ISelectService : IBaseService<Select>
    {
        PagedInfo<SelectDto> GetList(SelectQueryDto parm);

        Select GetInfo(Guid Id);
        /// <summary>
        /// 获取考试选择题
        /// </summary>
        /// <returns></returns>
        List<ExamSelectDto> ExamSelect();
        Select AddSelect(Select parm);
        int InsetList(List<Select> Entities);
        int UpdateSelect(Select parm);
        int ImportSelect(List<SelectDto> importList);
    }
}
