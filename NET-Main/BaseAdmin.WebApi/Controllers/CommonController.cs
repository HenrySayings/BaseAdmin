﻿using BaseCommon;
using BaseInfrastructure.Attribute;
using BaseInfrastructure.Controllers;
using BaseInfrastructure.CustomException;
using BaseInfrastructure.Enums;
using BaseInfrastructure.Extensions;
using BaseInfrastructure.Model;
using BaseInfrastructure.WebExtensions;
using BaseService.IService;
using BaseServiceCore.Filters;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;
using BaseServiceCore.Services;
using BaseServiceCore.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniExcelLibs;

namespace BaseAdmin.WebApi.Controllers
{
    /// <summary>
    /// 公共模块
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "sys")]
    //[Produces("application/json")]
    public class CommonController : BaseController
    {
        private OptionsSetting OptionsSetting;
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IWebHostEnvironment WebHostEnvironment;
        private ISysFileService SysFileService;
        private IHelloService HelloService;

        public CommonController(
            IOptions<OptionsSetting> options,
            IWebHostEnvironment webHostEnvironment,
            ISysFileService fileService,
            IHelloService helloService)
        {
            WebHostEnvironment = webHostEnvironment;
            SysFileService = fileService;
            OptionsSetting = options.Value;
            HelloService = helloService;
        }

        /// <summary>
        /// home
        /// </summary>
        /// <returns></returns>
        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("启动成功");
        }

        /// <summary>
        /// hello
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("/hello")]
        [HttpGet]
        public IActionResult Hello(string name)
        {
            return Ok(HelloService.SayHello(name));
        }

        /// <summary>
        /// 企业消息测试
        /// </summary>
        /// <param name="msg">要发送的消息</param>
        /// <param name="toUser">要发送的人@all所有，xxx单独发送对个人</param>
        /// <returns></returns>
        [Route("/sendMsg")]
        [HttpGet]
        [Log(Title = "企业消息测试")]
        public IActionResult SendMsg(string msg, string toUser = "")
        {
            WxNoticeHelper.SendMsg("消息测试", msg, toUser, WxNoticeHelper.MsgType.markdown);
            return SUCCESS(msg);
        }

        #region 上传

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="uploadDto">自定义文件名</param>
        /// <param name="storeType">上传类型1、保存到本地 2、保存到阿里云</param>
        /// <returns></returns>
        [HttpPost()]
        [Verify]
        [ActionPermissionFilter(Permission = "common")]
        public async Task<IActionResult> UploadFile([FromForm] UploadDto uploadDto, StoreType storeType = StoreType.LOCAL)
        {
            if (uploadDto?.File == null) throw new CustomException(ResultCode.PARAM_ERROR, "上传文件不能为空");
            SysFile file = new();
            IFormFile formFile = uploadDto.File;
            string fileExt = Path.GetExtension(formFile.FileName);//文件后缀
            double fileSize = Math.Round(formFile.Length / 1024.0, 2);//文件大小KB

            if (OptionsSetting.Upload.NotAllowedExt.Contains(fileExt))
            {
                return ToResponse(ResultCode.CUSTOM_ERROR, "上传失败，未经允许上传类型");
            }
            if (uploadDto.FileNameType == 1)
            {
                uploadDto.FileName = Path.GetFileNameWithoutExtension(formFile.FileName);
            }
            else if (uploadDto.FileNameType == 3)
            {
                uploadDto.FileName = SysFileService.HashFileName();
            }
            switch (storeType)
            {
                case StoreType.LOCAL:
                    string savePath = Path.Combine(WebHostEnvironment.WebRootPath);
                    if (uploadDto.FileDir.IsEmpty())
                    {
                        uploadDto.FileDir = OptionsSetting.Upload.LocalSavePath;
                    }
                    file = await SysFileService.SaveFileToLocal(savePath, uploadDto.FileName, uploadDto.FileDir, HttpContext.GetName(), formFile);
                    break;
                case StoreType.REMOTE:
                    break;
                case StoreType.ALIYUN:
                    int AlimaxContentLength = OptionsSetting.ALIYUN_OSS.MaxSize;
                    if (OptionsSetting.ALIYUN_OSS.REGIONID.IsEmpty())
                    {
                        return ToResponse(ResultCode.CUSTOM_ERROR, "配置文件缺失");
                    }
                    if (fileSize / 1024 > AlimaxContentLength)
                    {
                        return ToResponse(ResultCode.CUSTOM_ERROR, "上传文件过大，不能超过 " + AlimaxContentLength + " MB");
                    }
                    file = new(formFile.FileName, uploadDto.FileName, fileExt, fileSize + "kb", uploadDto.FileDir, HttpContext.GetName())
                    {
                        StoreType = (int)StoreType.ALIYUN,
                        FileType = formFile.ContentType
                    };
                    file = await SysFileService.SaveFileToAliyun(file, formFile);

                    if (file.Id <= 0) { return ToResponse(ApiResult.Error("阿里云连接失败")); }
                    break;
                case StoreType.TENCENT:
                    break;
                case StoreType.QINIU:
                    break;
                default:
                    break;
            }
            return SUCCESS(new
            {
                url = file.AccessUrl,
                fileName = file.FileName,
                fileId = file.Id.ToString()
            });
        }

        #endregion

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Verify]
        [ActionPermissionFilter(Permission = "common")]
        [Log(Title = "下载文件", IsSaveResponseData = false)]
        public IActionResult DownloadFile(string? path, long fileId = 0)
        {
            var tempPath = path;
            if (fileId > 0)
            {
                var fileInfo = SysFileService.GetById(fileId);
                if (fileInfo != null)
                {
                    tempPath = fileInfo.FileUrl;
                }
            }
            string fullPath = tempPath;
            if (tempPath.StartsWith("/"))
            {
                fullPath = Path.Combine(WebHostEnvironment.WebRootPath, tempPath.ReplaceFirst("/", ""));
            }
            string fileName = Path.GetFileName(fullPath);
            return DownFile(fullPath, fileName);
        }

        /// <summary>
        /// 初始化种子数据
        /// </summary>
        /// <param name="clean">是否清空数据</param>
        /// <returns></returns>
        [HttpGet]
        [ActionPermissionFilter(Permission = "common")]
        [Log(BusinessType = BusinessType.INSERT, Title = "初始化数据")]
        public IActionResult InitSeedData(bool clean = false)
        {
            if (!WebHostEnvironment.IsDevelopment())
            {
                return ToResponse(ResultCode.CUSTOM_ERROR, "导入数据失败");
            }
            var path = Path.Combine(WebHostEnvironment.WebRootPath, "data.xlsx");
            SeedDataService seedDataService = new();
            var result = seedDataService.InitSeedData(path, clean);
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.ForegroundColor = ConsoleColor.White;
            return SUCCESS(new
            {
                result
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionPermissionFilter(Permission = "common")]
        [Log(BusinessType = BusinessType.INSERT, Title = "初始化数据")]
        public IActionResult UpdateSeedData()
        {
            if (!WebHostEnvironment.IsDevelopment())
            {
                return ToResponse(ResultCode.CUSTOM_ERROR, "导入数据失败");
            }
            var path = Path.Combine(WebHostEnvironment.WebRootPath, "data.xlsx");
            SeedDataService seedDataService = new();

            var sysNotice = MiniExcel.Query<SysNotice>(path, sheetName: "notice").ToList();
            var result = seedDataService.InitNoticeData(sysNotice);

            var sysMenu = MiniExcel.Query<SysMenu>(path, sheetName: "menu").Where(f => f.MenuId >= 1104).ToList();
            var result5 = seedDataService.InitMenuData(sysMenu);

            return SUCCESS(new
            {
                result,
                result5
            });
        }
    }
}
