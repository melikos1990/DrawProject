using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentCommunicate : CaseAssignmentBase
    {
        public CaseAssignmentCommunicate()
        {
        }


        /// <summary>
        /// 流水號
        /// </summary>
        [Description("流水號")]
        public int ID { get; set; }

        /// <summary>
        /// 通知對象
        /// </summary>
        [Description("通知對象")]
        public List<CaseAssignmentCommunicateUser> Users { get; set; }

        #region override
        public override CaseAssignmentProcessType CaseAssignmentProcessType { get; set; } =
                        CaseAssignmentProcessType.Communication;

        #endregion

    }
}
