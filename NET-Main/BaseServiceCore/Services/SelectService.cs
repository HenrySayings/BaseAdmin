using Infrastructure.Extensions;
using BaseModel;
using BaseModel.Dto;
using BaseRepository;
using BaseService.Business.IBusinessService;
using System.Linq;
using BaseInfrastructure.Attribute;
using BaseServiceCore;
using BaseServiceCore.Model;
using BaseCommon;
using System.Data.SqlTypes;
using BaseServiceCore.Model.Dto;
using Mapster;
using SqlSugar;
using Microsoft.Data.SqlClient;

namespace BaseService.Business
{
    /// <summary>
    /// 选择题/多选题Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ISelectService), ServiceLifetime = LifeTime.Transient)]
    public class SelectService : BaseService<Select>, ISelectService
    {
        /// <summary>
        /// 查询选择题/多选题列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<SelectDto> GetList(SelectQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .LeftJoin<Chapeter>((s, c) => Guid.Parse(s.ChapeterId) == c.ChapeterId)
                .Select((s, c) => new SelectDto
                {
                    SelectId = s.SelectId,
                    Question=s.Question,
                    Answer=s.Answer,
                    OptionA=s.OptionA,
                    OptionB=s.OptionB,
                    OptionC=s.OptionC,
                    OptionD=s.OptionD,
                    Chapter = c.ChapterName
                })
                .ToPage(parm);

            return response;
        }
        public  List<ExamSelectDto> ExamSelect()
        {
            var randSingle = RandomUtil.Sampling(Queryable().ToList(), 50).Select(x => new ExamSelectDto
            {
                SelectId= x.SelectId,
                Answer = x.Answer,
                Title = x.Question,
                Type = x.IsSingle,
                answerSheet = new List<Answer>()
                {
                    new Answer{value="A",Name=x.OptionA},
                    new Answer{value="B",Name=x.OptionB},
                    new Answer{value="C",Name=x.OptionC},
                    new Answer{value="D",Name=x.OptionD}
                }
            }).ToList();
            return  randSingle;
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Select GetInfo(Guid Id)
        {
            var response = Queryable()
                .Where(x => x.SelectId == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加选择题/多选题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Select AddSelect(Select model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改选择题/多选题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateSelect(Select model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<Select> QueryExp(SelectQueryDto parm)
        {
            var predicate = Expressionable.Create<Select>();
            return predicate;
        }
        private static Expressionable<Select> QueryExps(ExamSelectQueryDto parm)
        {
            var predicate = Expressionable.Create<Select>();
            return predicate;
        }

        public int ImportSelect(List<SelectDto> importList)
        {
            return 0;
        }

        public int InsetList(List<Select> Entities)
        {
            return Insert(Entities);
        }
    }
}