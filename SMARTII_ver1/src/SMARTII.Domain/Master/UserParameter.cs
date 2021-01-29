using System.Collections.Generic;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Domain.Master
{
    public class UserParameter
    {
        public UserParameter()
        {
        }
        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 是否顯示功能導覽
        /// </summary>
        public bool NavigateOfNewbie { get; set; }

        /// <summary>
        /// 是否顯示官網來信
        /// </summary>
        public bool NoticeOfWebsite { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public HttpFile Picture { get; set; }

        /// <summary>
        /// 快速功能列(我的最愛)
        /// </summary>
        public List<PageAuthFavorite> FavoriteFeature { get; set; } = new List<PageAuthFavorite>();
    }
}
