using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IImportStoreService
    {
        /// <summary>
        /// 匯入門市
        /// </summary>
        void ImportStore();
        /// <summary>
        /// 匯入門市人員
        /// </summary>
        void ImportStorePersonnel();
        /// <summary>
        /// 匯入門市資訊
        /// </summary>
        void ImportStoreInformation();
    }
}
