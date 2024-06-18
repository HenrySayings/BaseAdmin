using BaseModel;
using BaseServiceCore.Model;
using BaseServiceCore.Model.Dto;

namespace BaseServiceCore.Services.IService
{
    /// <summary>
    /// 通知公告表service接口
    ///
    /// @author Base
    /// @date 2021-12-15
    /// </summary>
    public interface ISysNoticeService : IBaseService<SysNotice>
    {
        List<SysNotice> GetSysNotices();

        PagedInfo<SysNotice> GetPageList(SysNoticeQueryDto parm);
        PagedInfo<SysNoticeDto> ExportList(SysNoticeQueryDto parm);
    }
}
