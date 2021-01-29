using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Areas.Master.Models.UserParameter
{
    public class UserParameterDetailViewModel
    {
        public UserParameterDetailViewModel()
        {
        }
        public UserParameterDetailViewModel(Domain.Master.UserParameter data)
        {
            this.UserID = data.UserID;
            this.NavigateOfNewbie = data.NavigateOfNewbie;
            this.FavoriteFeature = data.FavoriteFeature;
            this.ImagePath = data.ImagePath;
        }

        public Domain.Master.UserParameter ToDomain()
        {
            var result = new Domain.Master.UserParameter();
            result.UserID = this.UserID;
            result.NavigateOfNewbie = this.NavigateOfNewbie;
            result.FavoriteFeature = this.FavoriteFeature;
            result.ImagePath = this.ImagePath;
            result.Picture = this.Picture?.FirstOrDefault();
            return result;
        }
        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 是否彈出功能導覽
        /// </summary>
        public bool NavigateOfNewbie { get; set; }
        /// <summary>
        /// 快速功能列
        /// </summary>
        public List<PageAuthFavorite> FavoriteFeature { get; set; }
        /// <summary>
        /// 個人大頭照附件路徑
        /// </summary>
        public string ImagePath { get; set; }

        public HttpFile[] Picture { get; set; }

    }
}
