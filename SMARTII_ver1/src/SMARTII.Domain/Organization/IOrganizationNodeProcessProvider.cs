using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface IOrganizationNodeProcessProvider
    {
        /// <summary>
        /// 取得組織樹
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IOrganizationNode>> GetAll();

        /// <summary>
        /// 新增節點
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Task Create(IOrganizationNode node);

        /// <summary>
        /// 修改節點基本資訊
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Task<IOrganizationNode> Update(IOrganizationNode node);

        /// <summary>
        /// 拖拉組織樹
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Task UpdateTree(IOrganizationNode node);

        /// <summary>
        /// 節點上停用
        /// </summary>
        /// <param name="NodeID"></param>
        /// <returns></returns>
        Task Disable(int nodeID);

        /// <summary>
        /// 登入時，取得組織人員設定
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        Task<IOrganizationNode> Get(int nodeID);

        /// <summary>
        /// 取得節點資訊
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        Task<IOrganizationNode> GetComplete(int nodeID);

        /// <summary>
        /// 新增節點職稱(多筆)
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="jobIDs"></param>
        /// <returns></returns>
        Task AddJobs(int nodeID, int[] jobIDs);

        /// <summary>
        /// 刪除節點職稱(單筆)
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <returns></returns>
        Task DeleteJob(int nodeJobID);

        /// <summary>
        /// 新增節點職稱人員(多筆)
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        Task AddUsers(int nodeJobID, string[] userIDs);

        /// <summary>
        /// 刪除節點職稱人員(單筆)
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task DeleteUser(int nodeJobID, string userID);

        /// <summary>
        /// 確認停用之廠商狀態
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        Task<bool> CheckDisableVendor(int nodeID);
    }
}
