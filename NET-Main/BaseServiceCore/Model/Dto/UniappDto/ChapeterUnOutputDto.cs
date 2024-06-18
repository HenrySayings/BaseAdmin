using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseServiceCore.Model.Dto.UniappDto
{
    public record ChapeterUnOutputDto
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public List<ChapeterItemInputDto> ChapeterItem { get; set; }
    }
    public record ChapeterItemInputDto
    {
        /// <summary>
        /// 章节名称
        /// </summary>
        public string ChapeterName { get; init; }
        public List<ChapeterItemsDto> ChapeterItemsName { get; set; }
    }
    public record ChapeterItemsDto
    {
        /// <summary>
        /// 小节名称
        /// </summary>
        public string ChapeterItemName { get; init; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string FileUrl { get; init; }
    }
}
