using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IKMClassificationFacade
    {
        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Create(KMData group);

        /// <summary>
        /// 單一更新明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(KMData group);

        /// <summary>
        /// 單一刪除明細
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task Delete(int ID);

        /// <summary>
        /// 單一新增分類
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task CreateClassification(KMClassification group);

        /// <summary>
        /// 更新分類名稱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task RenameClassification(int classificationID, string name);
        /// <summary>
        /// 新增根節點
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task CreateRootClassification(int nodeID, string name);
        /// <summary>
        /// 拖曳分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task DragClassification(int classificationID, int? parentID);
        /// <summary>
        /// 刪除分類
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task DeleteClassification(int ID);
        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void DeleteFileWithUpdate(int id, string key);

    }
}
