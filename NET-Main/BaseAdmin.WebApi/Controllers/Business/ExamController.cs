using Microsoft.AspNetCore.Mvc;
using BaseModel.Dto;
using BaseService.Business.IBusinessService;
using BaseServiceCore.Filters;
using BaseInfrastructure.Controllers;
using BaseInfrastructure.Enums;
using BaseInfrastructure.Attribute;
using BaseServiceCore.Model;
using BaseInfrastructure.WebExtensions;
using BaseCommon;

//创建时间：2024-05-11
namespace BaseAdmin.WebApi.Controllers
{
    /// <summary>
    /// 试卷表
    /// </summary>
    [Verify]
    [Route("business/Exam")]
    public class ExamController : BaseController
    {
        /// <summary>
        /// 试卷表接口
        /// </summary>
        private readonly IExamService _ExamService;

        public ExamController(IExamService ExamService)
        {
            _ExamService = ExamService;
        }

        /// <summary>
        /// 查询试卷表列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "exam:list")]
        public IActionResult QueryExam([FromQuery] ExamQueryDto parm)
        {
            var response = _ExamService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询试卷表详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "exam:query")]
        public IActionResult GetExam(Guid Id)
        {
            var response = _ExamService.GetInfo(Id);
            
            var info = response.Adapt<ExamDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加试卷表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "exam:add")]
        [Log(Title = "试卷表", BusinessType = BusinessType.INSERT)]
        public IActionResult AddExam([FromBody] ExamDto parm)
        {
            var modal = parm.Adapt<Exam>().ToCreate(HttpContext);
            var response = _ExamService.AddExam(modal);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新试卷表
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "exam:edit")]
        [Log(Title = "试卷表", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateExam([FromBody] ExamDto parm)
        {
            var modal = parm.Adapt<Exam>().ToUpdate(HttpContext);
            var response = _ExamService.UpdateExam(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除试卷表
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "exam:delete")]
        [Log(Title = "试卷表", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteExam([FromRoute]Guid ids)
        {
            return ToResponse(_ExamService.Delete(ids));
        }

    }
}