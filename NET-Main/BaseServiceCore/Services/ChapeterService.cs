using BaseInfrastructure.Attribute;
using BaseModel;
using BaseModel.Business;
using BaseModel.Dto;
using BaseRepository;
using BaseService.Business.IBusinessService;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;
using BaseServiceCore.Model.Dto.UniappDto;
using JinianNet.JNTemplate;
using SqlSugar.DistributedSystem.Snowflake;

namespace BaseServiceCore.Services
{
    /// <summary>
    /// 章节管理Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IChapeterService), ServiceLifetime = LifeTime.Transient)]
    public class ChapeterService : BaseService<Chapeter>, IChapeterService
    {
        /// <summary>
        /// 查询章节管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<ChapeterDto> GetList(ChapeterQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .LeftJoin<Course>((c, ch) => c.CourseId == ch.CourseId)
                .OrderBy("ChapterName asc")
                .Where(predicate.ToExpression())
                .Select((c,ch) => new ChapeterDto
                 {
                    ChapeterId=c.ChapeterId,
                     ChapterName = c.ChapterName,
                     ChapterItemName = c.ChapterItemName,
                     Course = ch.CourseName,
                     CreateTime = c.CreateTime,
                     CreateUser = c.CreateUser,
                     ChapterUrl = c.ChapterUrl,
                     Desc=c.Desc,
                 })
                .ToPage(parm);

            return response;
        }
        /// <summary>
        /// 绑定下拉框
        /// </summary>
        /// <returns></returns>
        public List<ChaperSelectDto> GetChaperSelectList()
        {
            return Queryable().Select(x => new ChaperSelectDto { ChapeterId = x.ChapeterId, ChapterName = x.ChapterName }).ToList();
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Chapeter GetInfo(Guid Id)
        {
            var response = Queryable()
                .Where(x => x.ChapeterId == Id)
                .First();

            return response;
        }
        /// <summary>
        /// 添加章节管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Chapeter AddChapeter(Chapeter model)
        {
            Insertable(model).ExecuteCommandAsync();
            return model;
        }

        /// <summary>
        /// 修改章节管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateChapeter(Chapeter model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 导入章节管理
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportChapeter(List<Chapeter> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                //.WhereColumns(it => it.UserName)//如果不是主键可以这样实现（多字段it=>new{it.x1,it.x2}）
                .ToStorage();
            var result = x.AsInsertable.ExecuteCommand();//插入可插入部分;

            string msg = $"插入{x.InsertList.Count} 更新{x.UpdateList.Count} 错误数据{x.ErrorList.Count} 不计算数据{x.IgnoreList.Count} 删除数据{x.DeleteList.Count} 总共{x.TotalList.Count}";
            Console.WriteLine(msg);

            //输出错误信息               
            foreach (var item in x.ErrorList)
            {
                Console.WriteLine("错误" + item.StorageMessage);
            }
            foreach (var item in x.IgnoreList)
            {
                Console.WriteLine("忽略" + item.StorageMessage);
            }

            return (msg, x.ErrorList, x.IgnoreList);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<Chapeter> QueryExp(ChapeterQueryDto parm)
        {
            var predicate = Expressionable.Create<Chapeter>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChapterName), it => it.ChapterName.Contains(parm.ChapterName));
            return predicate;
        }

        public async Task<ChapeterUnOutputDto> GetChapeterUnInfo(Guid CouseId)
        {
            var entity = Queryable()
              .LeftJoin<Course>((x, y) => x.CourseId == y.CourseId)
              .Where((x, y) => x.CourseId == CouseId)
              .GroupBy((x, y) => new { x.ChapterName, y.CourseName })
              .Select((x, y) => new ChapeterUnOutputDto
              {
                  Title = y.CourseName,
                  ChapeterItem = new List<ChapeterItemInputDto>()
              }).ToList();
            await Context.ThenMapperAsync(entity, async x =>
            {
                x.ChapeterItem = await Queryable().Where(x => x.CourseId == CouseId).GroupBy(x => x.ChapterName).Select(x => new ChapeterItemInputDto
                {
                    ChapeterItemsName = new List<ChapeterItemsDto>(),
                    ChapeterName = x.ChapterName
                }).OrderByDescending(x=>x.ChapeterName).ToListAsync();
            });

            await Context.ThenMapperAsync(entity.Select(y => y.ChapeterItem), async y =>
            {
                foreach (var item in y)
                {
                    item.ChapeterItemsName = await Queryable().Where(y => y.CourseId == CouseId)
                    .Where(x=>x.ChapterName==item.ChapeterName).GroupBy(y => new { y.ChapterName, y.ChapterItemName, y.ChapterUrl }).Select(y => new ChapeterItemsDto
                    {
                        ChapeterItemName = y.ChapterItemName,
                        FileUrl = y.ChapterUrl
                    }).OrderByDescending(x=>x.ChapeterItemName).ToListAsync();
                }
            });
            return entity.FirstOrDefault();

        }
    }
}