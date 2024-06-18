using BaseModel;
using BaseModel.Dto;
using BaseServiceCore;
using BaseServiceCore.Model;

namespace BaseService.Business.IBusinessService
{
    /// <summary>
    /// 试卷表service接口
    /// </summary>
    public interface IExamService : IBaseService<Exam>
    {
        PagedInfo<ExamDto> GetList(ExamQueryDto parm);
        Exam GetInfo(Guid Id);
        Exam AddExam(Exam parm);
        int UpdateExam(Exam parm);
    }
}
