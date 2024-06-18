using BaseInfrastructure.Attribute;
using BaseModel;
using BaseModel.Dto;
using BaseRepository;
using BaseService.Business.IBusinessService;
using BaseServiceCore;
using BaseServiceCore.Model;
using System.Linq;

namespace BaseService.Business
{
    /// <summary>
    /// 成绩表Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IUserScoreService), ServiceLifetime = LifeTime.Transient)]
    public class UserScoreService : BaseService<UserScore>, IUserScoreService
    {
        /// <summary>
        /// 查询成绩表列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<UserScoreDto> GetList(UserScoreQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select(x=>new UserScoreDto
                {
                    Name=x.UserName,
                    Score=x.Score,
                    DateTime=x.CreateTime,
                    Exams="练习"
                })
                .ToPage(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserScore GetInfo(Guid Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加成绩表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserScore AddUserScore(UserScore model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改成绩表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateUserScore(UserScore model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<UserScore> QueryExp(UserScoreQueryDto parm)
        {
            var predicate = Expressionable.Create<UserScore>();

            return predicate;
        }
    }
}