using BaseCommon;
using BaseInfrastructure.Attribute;
using BaseInfrastructure.Controllers;
using BaseInfrastructure.Enums;
using BaseInfrastructure.WebExtensions;
using BaseModel.Dto;
using BaseService.Business.IBusinessService;
using BaseServiceCore.Filters;
using BaseServiceCore.Model;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

//创建时间：2024-05-01
namespace BaseAdmin.WebApi.Controllers
{
    /// <summary>
    /// 章节管理
    /// </summary>
    [Verify]
    [Route("business/Chapeter")]
    public class ChapeterController : BaseController
    {
        /// <summary>
        /// 章节管理接口
        /// </summary>
        private readonly IChapeterService _ChapeterService;

        public ChapeterController(IChapeterService ChapeterService)
        {
            _ChapeterService = ChapeterService;
        }

        /// <summary>
        /// 查询章节管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "chapeter:list")]
        public IActionResult QueryChapeter([FromQuery] ChapeterQueryDto parm)
        {
            var response = _ChapeterService.GetList(parm);
            return SUCCESS(response);
        }
        [HttpGet("select")]
        [ActionPermissionFilter(Permission = "chapeter:list")]
        public IActionResult QueryChapeterBySelect()
        {
            var response = _ChapeterService.GetChaperSelectList();
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询章节管理详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "chapeter:query")]
        public IActionResult GetChapeter(Guid Id)
        {
            var response = _ChapeterService.GetInfo(Id);

            var info = response.Adapt<ChapeterDto>();
            return SUCCESS(info);
        }


        [HttpPost("GetChapeterUnInfo")]
        [ActionPermissionFilter(Permission = "chapeter:list")]
        public async Task<IActionResult> GetChapeterUnInfo(Guid Id)
        {
            return SUCCESS(await _ChapeterService.GetChapeterUnInfo(Id));
        }
        /// <summary>
        /// 添加章节管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "chapeter:add")]
        [Log(Title = "章节管理", BusinessType = BusinessType.INSERT)]
        public IActionResult AddChapeter([FromBody] ChapeterDto parm)
        {
            var modal = parm.Adapt<Chapeter>().ToCreate(HttpContext);
            modal.ChapeterId = Guid.NewGuid();
            modal.CreateUser = HttpContext.User.Identity.Name;
            var response = _ChapeterService.AddChapeter(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新章节管理
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "chapeter:edit")]
        [Log(Title = "章节管理", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateChapeter([FromBody] ChapeterDto parm)
        {
            var modal = parm.Adapt<Chapeter>().ToUpdate(HttpContext);
            modal.UpdateUser = HttpContext.User.Identity.Name;
            var response = _ChapeterService.UpdateChapeter(modal);
            return ToResponse(response);
        }

        /// <summary>
        /// 删除章节管理
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "chapeter:delete")]
        [Log(Title = "章节管理", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteChapeter([FromRoute] Guid ids)
        {
            return ToResponse(_ChapeterService.Delete(ids));
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "章节管理导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "chapeter:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<ChapeterDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<ChapeterDto>(startCell: "A1").ToList();
            }
            return SUCCESS(_ChapeterService.ImportChapeter(list.Adapt<List<Chapeter>>()));
        }

        /// <summary>
        /// 章节管理导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "章节管理模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<ChapeterDto>() { }, "Chapeter");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}