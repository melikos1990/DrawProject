namespace SMARTII.Domain.Case
{
    /// <summary>
    /// 採各BU 進行更新
    /// 目標 : Particular
    /// </summary>
    public interface ICaseSpecificFactory
    {
        /// <summary>
        /// 更新特殊欄位
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        Case Update(Case @case);
    }
}
