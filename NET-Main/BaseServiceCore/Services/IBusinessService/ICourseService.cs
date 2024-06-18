using BaseModel;
using BaseModel.Dto;
using BaseModel.Business;
using BaseServiceCore;
using BaseServiceCore.Model.Dto;

namespace BaseService.Business.IBusinessService
{
    /// <summary>
    /// 课程service接口
    /// </summary>
    public interface ICourseService : IBaseService<Course>
    {
        PagedInfo<CourseDto> GetList(CourseQueryDto parm);

        Course GetInfo(Guid Id);
        List<UniCourseOutputDto> GetUniCourses();
        List<CourseSelectDto> GetCourseSelectList();
        Course AddCourse(Course parm);
        int UpdateCourse(Course parm);


    }
}
