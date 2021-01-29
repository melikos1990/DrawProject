using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class QuestionClassificationGuideFacade : IQuestionClassificationGuideFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        public QuestionClassificationGuideFacade(IMasterAggregate MasterAggregate,
                                 IOrganizationAggregate OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }


        /// <summary>
        /// 新增常用語
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Create(QuestionClassificationGuide data)
        {
            #region 既有單位/問題分類

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_BU_FAIL);
            var isExitClassificationID = _MasterAggregate.QuestionClassificationGuide_T1_
                                                .HasAny(x => x.CLASSIFICATION_ID == data.ClassificationID);
            if (isExitClassificationID)
                throw new Exception(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_DUPLICATE_CLASSIFICATIOINID);
            #endregion

            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            var result = _MasterAggregate.QuestionClassificationGuide_T1_T2_.Add(data);
            await result.Async();
        }
        /// <summary>
        /// 單一更新常用語
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Update(QuestionClassificationGuide data)
        {
            #region 既有單位/問題分類

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_BU_FAIL);


            #endregion

            var con = new MSSQLCondition<QUESTION_CLASSIFICATION_GUIDE>(x => x.CLASSIFICATION_ID == data.ClassificationID);

            con.ActionModify(x =>
            {
                x.CONTENT = data.Content;
                x.CLASSIFICATION_ID = data.ClassificationID;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _MasterAggregate.QuestionClassificationGuide_T1_T2_.Update(con);
            await result.Async();
        }
    }
}
