using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Case
{
    public enum CaseAssignmentWorkType
    {
        /// <summary>
        /// 一般銷案
        /// </summary>
        [Description("一般銷案")]
        General = 0,
        /// <summary>
        /// 偕同銷案
        /// </summary>
        [Description("偕同銷案")]
        Accompanied = 1
    }
}
