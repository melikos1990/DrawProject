using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IBillboardFacade
    {
        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<Billboard> Get(int ID);

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="data"></param>
        Task Create(Billboard data);

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="data"></param>
        Task Update(Billboard data);

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="ID"></param>
        Task Delete(int ID);

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="IDs"></param>
        Task DeleteRange(int[] IDs);

        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void DeleteFileWithUpdate(int id, string key);
    }
}