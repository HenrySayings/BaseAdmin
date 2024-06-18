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
    /// 成绩表
    /// </summary>
    [Verify]
    [Route("business/UserScore")]
    public class UserScoreController : BaseController
    {
        /// <summary>
        /// 成绩表接口
        /// </summary>
        private readonly IUserScoreService _UserScoreService;

        public UserScoreController(IUserScoreService UserScoreService)
        {
            _UserScoreService = UserScoreService;
        }

        /// <summary>
        /// 查询成绩表列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "userscore:list")]
        public IActionResult QueryUserScore([FromQuery] UserScoreQueryDto parm)
        {
            var response = _UserScoreService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询成绩表详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "userscore:query")]
        public IActionResult GetUserScore(Guid Id)
        {
            var response = _UserScoreService.GetInfo(Id);
            
            var info = response.Adapt<UserScoreDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加成绩表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "userscore:add")]
        [Log(Title = "成绩表", BusinessType = BusinessType.INSERT)]
        public IActionResult AddUserScore([FromBody] UserScoreDto parm)
        {
            var modal = parm.Adapt<UserScore>().ToCreate(HttpContext);
            modal.Id=Guid.NewGuid();
            modal.CreateTime=DateTime.Now;
            modal.UserName = HttpContext.User.Identity.Name;
            var response = _UserScoreService.AddUserScore(modal);
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新成绩表
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "userscore:edit")]
        [Log(Title = "成绩表", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateUserScore([FromBody] UserScoreDto parm)
        {
            var modal = parm.Adapt<UserScore>().ToUpdate(HttpContext);
            var response = _UserScoreService.UpdateUserScore(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除成绩表   
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "userscore:delete")]
        [Log(Title = "成绩表", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteUserScore([FromRoute]Guid ids)
        {
            return ToResponse(_UserScoreService.Delete(ids));
        }

    }
}