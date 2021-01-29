using SMARTII.Domain.Case;

namespace SMARTII.COMMON_BU.Service
{
    public class CaseSpecificFactory : ICaseSpecificFactory
    {
        public Case Update(Case @case)
        {

            // 根據不同BU 特殊欄位之更新


            return @case;
        }
    }
}
