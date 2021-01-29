using System.Collections.Generic;

namespace SMARTII.Domain.Data
{
    public interface IRecursivelyModel
    {
        /// <summary>
        /// 代號
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// 父層級節點
        /// </summary>
        int? ParentLocator { get; set; }

        /// <summary>
        /// 底下的節點
        /// </summary>
        List<IRecursivelyModel> Children { get; set; }
    }

    public interface INSMNestedModel : IRecursivelyModel
    {
        /// <summary>
        /// 左節點
        /// </summary>
        int LeftBoundary { get; set; }

        /// <summary>
        /// 右邊節點
        /// </summary>
        int RightBoundary { get; set; }

        /// <summary>
        /// 層級
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// 父層級路徑
        /// </summary>
        string ParentPath { get; set; }
    }
}
