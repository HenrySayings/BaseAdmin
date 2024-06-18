using Infrastructure.Extensions;
using BaseModel;
using BaseModel.Dto;
using BaseRepository;
using BaseService.Business.IBusinessService;
using System.Linq;
using BaseInfrastructure.Attribute;
using BaseServiceCore;
using BaseServiceCore.Model;
using BaseModel.Business;

namespace BaseService.Business
{
    /// <summary>
    /// 试卷表Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IExamService), ServiceLifetime = LifeTime.Transient)]
    public class ExamService : BaseService<Exam>, IExamService
    {
        /// <summary>
        /// 查询试卷表列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<ExamDto> GetList(ExamQueryDto parm)
        {
            var predicate = QueryExp(parm);
            var response = Queryable()
                .LeftJoin<Course>((x, y) => x.CourseId == y.CourseId)
                .Where(predicate.ToExpression())
                .Select((x,y) => new ExamDto
                {
                    CourseName = y.CourseName,
                    ExamId = x.ExamId,
                    ExamName = x.ExamName,
                    Duration = x.Duration ,
                    CreateTime = x.CreateTime,
                    UpdateTime = x.UpdateTime,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    IsDeleted = x.IsDeleted,
                    CourseId = x.CourseId,
                    ExamScore = x.ExamScore,
                    CreateUser = x.CreateUser,
                    IsFinish = x.IsFinish
                })
                .ToPage(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Exam GetInfo(Guid Id)
        {
            var response = Queryable()
                .Where(x => x.ExamId == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加试卷表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Exam AddExam(Exam model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改试卷表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateExam(Exam model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<Exam> QueryExp(ExamQueryDto parm)
        {
            var predicate = Expressionable.Create<Exam>();

            return predicate;
        }
    }
}