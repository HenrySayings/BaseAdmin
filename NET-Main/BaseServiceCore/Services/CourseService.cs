using Infrastructure.Extensions;
using BaseModel;
using BaseModel.Dto;
using BaseModel.Business;
using BaseRepository;
using BaseService.Business.IBusinessService;
using System.Linq;
using BaseInfrastructure.Attribute;
using BaseServiceCore;
using BaseServiceCore.Model.Dto;

namespace BaseService.Business
{
    /// <summary>
    /// 课程Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ICourseService), ServiceLifetime = LifeTime.Transient)]
    public class CourseService : BaseService<Course>, ICourseService
    {
        /// <summary>
        /// 查询课程列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<CourseDto> GetList(CourseQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                //.OrderBy("CourseId asc")
                .Where(predicate.ToExpression())
                .ToPage<Course, CourseDto>(parm);

            return response;
        }
        public List<CourseSelectDto> GetCourseSelectList()
        {
            return Queryable().Select(x => new CourseSelectDto { CourseId = x.CourseId, CourseName = x.CourseName }).ToList();
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Course GetInfo(Guid Id)
        {
            var response = Queryable()
                .Where(x => x.CourseId == Id)
                .First();
            return response;
        }

        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Course AddCourse(Course model)
        {
            Insertable(model).ExecuteCommandAsync();
            return model;
        }

        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCourse(Course model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<Course> QueryExp(CourseQueryDto parm)
        {
            var predicate = Expressionable.Create<Course>();
            return predicate;
        }

        public List<UniCourseOutputDto> GetUniCourses()
        {
            return Queryable().Select(x => new UniCourseOutputDto { PicUrl = x.PicUrl, CourseName = x.CourseName , CourseId =x.CourseId}).ToList();
        }
    }
}