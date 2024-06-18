using Microsoft.AspNetCore.Mvc;
using BaseModel.Dto;
using BaseModel.Business;
using BaseService.Business.IBusinessService;
using BaseServiceCore.Filters;
using BaseInfrastructure.Controllers;
using BaseInfrastructure.Attribute;
using BaseInfrastructure.Enums;
using BaseInfrastructure.WebExtensions;
using BaseCommon;

//创建时间：2024-05-12
namespace BaseAdmin.WebApi.Controllers
{
    /// <summary>
    /// 课程
    /// </summary>
    [Verify]
    [Route("business/CourseService")]
    public class CourseController : BaseController
    {
        /// <summary>
        /// 课程接口
        /// </summary>
        private readonly ICourseService _CourseService;

        public CourseController(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        /// <summary>
        /// 查询课程列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "course:list")]
        public IActionResult QueryCourse([FromQuery] CourseQueryDto parm)
        {
            var response = _CourseService.GetList(parm);
            return SUCCESS(response);
        }
        [HttpGet("select")]
        [ActionPermissionFilter(Permission = "course:list")]
        public IActionResult QueryCourseBySelect()
        {
            var response = _CourseService.GetCourseSelectList();
            return SUCCESS(response);
        }
        [HttpGet("GetUniCourses")]
        [ActionPermissionFilter(Permission = "course:list")]
        public IActionResult GetUniCourses()
        {
            var response = _CourseService.GetUniCourses();
            return SUCCESS(response);
        }
        /// <summary>
        /// 查询课程详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "course:query")]
        public IActionResult GetCourse(Guid Id)
        {
            var response = _CourseService.GetInfo(Id);
            
            var info = response.Adapt<CourseDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "course:add")]
        [Log(Title = "课程", BusinessType = BusinessType.INSERT)]
        public IActionResult AddCourse([FromBody] CourseDto parm)
        {
            var modal = parm.Adapt<Course>().ToCreate(HttpContext);
            modal.CourseId= Guid.NewGuid();
            modal.CreateUser = HttpContext.User.Identity.Name;
            modal.CreateTime = DateTime.Now;
            modal.IsDeleted = false;
            var response = _CourseService.AddCourse(modal);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新课程
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "course:edit")]
        [Log(Title = "课程", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateCourse([FromBody] CourseDto parm)
        {
            var modal = parm.Adapt<Course>().ToUpdate(HttpContext);
            modal.UpdateUser = HttpContext.User.Identity.Name;
            modal.UpdateTime = DateTime.Now;
            modal.IsDeleted = false;
            var response = _CourseService.UpdateCourse(modal);
            return ToResponse(response);
        }

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "course:delete")]
        [Log(Title = "课程", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteCourse([FromRoute]Guid ids)
        {
            return ToResponse(_CourseService.Delete(ids));
        }

    }
}