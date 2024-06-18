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
using BaseServiceCore.Model.Dto;
using System.Linq;
using BaseCommon.MiniExcelHelper;
using Microsoft.AspNetCore.Mvc.Filters;
using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using MiniExcelLibs;

//创建时间：2024-05-11
namespace BaseAdmin.WebApi.Controllers
{
    /// <summary>
    /// 选择题/多选题
    /// </summary>
    [Verify]
    [Route("business/Select")]
    public class SelectController : BaseController
    {
        /// <summary>
        /// 选择题/多选题接口
        /// </summary>
        private readonly ISelectService _SelectService;
        private readonly IUserScoreService _UserScoreService;

        public SelectController(ISelectService SelectService, IUserScoreService UserScoreService)
        {
            _SelectService = SelectService;
            _UserScoreService = UserScoreService;
        }

        /// <summary>
        /// 查询选择题/多选题列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "select:list")]
        public IActionResult QuerySelect([FromQuery] SelectQueryDto parm)
        {
            var response = _SelectService.GetList(parm);
            return SUCCESS(response);
        }
        /// <summary>
        /// 考试题目接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("examlist")]
        [ActionPermissionFilter(Permission = "select:list")]
        public IActionResult ExamQuerySelect()
        {
            var response = _SelectService.ExamSelect();
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询选择题/多选题详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "select:query")]
        public IActionResult GetSelect(Guid Id)
        {
            var response = _SelectService.GetInfo(Id);
            var info = response.Adapt<SelectDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加选择题/多选题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "select:add")]
        [Log(Title = "选择题/多选题", BusinessType = BusinessType.INSERT)]
        public IActionResult AddSelect([FromBody] SelectDto parm)
        {
            var modal = parm.Adapt<Select>().ToCreate(HttpContext);
            modal.CreateUser = HttpContext.User.Identity.Name;
            modal.CreateTime = DateTime.Now;
            var response = _SelectService.AddSelect(modal);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新选择题/多选题
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "select:edit")]
        [Log(Title = "选择题/多选题", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateSelect([FromBody] SelectDto parm)
        {
            var modal = parm.Adapt<Select>().ToUpdate(HttpContext);
            parm.Chapter = "";
            modal.UpdateUser = HttpContext.User.Identity.Name;
            modal.UpdateTime = DateTime.Now;
            var response = _SelectService.UpdateSelect(modal);
            return ToResponse(response);
        }
        /// <summary>
        /// 导入题目
        /// </summary>
        /// <returns></returns>
        [HttpPost("importData")]
        [ActionPermissionFilter(Permission = "select:add")]
        [Log(Title = "选择题/多选题", BusinessType = BusinessType.INSERT)]
        public IActionResult ImportSelect([FromForm(Name = "file")] IFormFile formFile)
        {
            List<Select> selects = new();
            using (var stream = formFile.OpenReadStream())
            {
                selects = stream.Query<Select>(startCell: "A1").ToList();
            }
            foreach (var select in selects)
            {
                select.CreateUser= HttpContext.User.Identity.Name;
            }
            return SUCCESS(_SelectService.Insert(selects));
        }
        [HttpGet("importTemplate")]
        [Log(Title = "题目模板", BusinessType = BusinessType.EXPORT, IsSaveRequestData = true, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            (string, string) result = DownloadImportTemplate(new List<Select>(),"select");
            return ExportExcel(result.Item2, result.Item1);
        }
        /// <summary>
        /// 删除选择题/多选题
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "select:delete")]
        [Log(Title = "选择题/多选题", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteSelect([FromRoute]Guid ids)
        {
           if (_SelectService.DeleteById(ids))
            return SUCCESS("删除成功");
           else
            return Content("删除失败");
        }
    }
}