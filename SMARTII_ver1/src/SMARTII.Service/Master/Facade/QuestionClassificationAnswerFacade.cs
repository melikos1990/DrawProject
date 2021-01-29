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
    public class QuestionClassificationAnswerFacade : IQuestionClassificationAnswerFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        public QuestionClassificationAnswerFacade(IMasterAggregate MasterAggregate,
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
        public async Task Create(List<QuestionClassificationAnswer> data)
        {
            List<string> repeatList = new List<string>();
            foreach (var item in data)
            {
                #region 驗證名稱/既有單位

                var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                    .HasAny(x => x.BU_ID == item.NodeID);
                if (!isExitHeadquarters)
                    throw new Exception(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_BU_FAIL);

                var isExistTitleInList = data.Where(x => x.ClassificationID == item.ClassificationID && x.Title == item.Title);
                if (isExistTitleInList.Count() != 1)
                    throw new Exception(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DUPLICATE_TITLE);

                string title = item.Title.Trim();
                //驗證主旨是否重複
                var isExistTitle = _MasterAggregate.QuestionClassificationAnswer_T1_
                                                     .HasAny(x => x.CLASSIFICATION_ID == item.ClassificationID &&
                                                                  x.NODE_ID == item.NodeID &&
                                                                  x.TITLE == title);
                if (isExistTitle)
                    repeatList.Add(title);
                #endregion

                item.CreateDateTime = DateTime.Now;
                item.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
            }

            if (repeatList.Any())
            {
                throw new Exception(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DUPLICATE_TITLE + "。" +
                        QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_REPEATTITLE  + string.Join("、", repeatList));
            }

            var result = _MasterAggregate.QuestionClassificationAnswer_T1_T2_.AddRange(data);

            await result.Async();
        }
        /// <summary>
        /// 單一更新常用語
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Update(QuestionClassificationAnswer data)
        {
            #region 驗證名稱/既有單位

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_BU_FAIL);
            string title = data.Title.Trim();

            // 修改後的 , 其他的資料(除了本身)是否有相同處置名稱
            var item = _MasterAggregate.QuestionClassificationAnswer_T1_
                                       .HasAny(x => x.ID != data.ID &&
                                                 x.CLASSIFICATION_ID == data.ClassificationID &&
                                                 x.NODE_ID == data.NodeID &&
                                                 x.TITLE == title);

            if (item)
                throw new Exception(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DUPLICATE_TITLE);
            #endregion

            var con = new MSSQLCondition<QUESTION_CLASSIFICATION_ANSWER>(x => x.ID == data.ID);
            con.ActionModify(x =>
            {
                x.TITLE = data.Title;
                x.CONTENT = data.Content;
                x.CLASSIFICATION_ID = data.ClassificationID;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _MasterAggregate.QuestionClassificationAnswer_T1_T2_.Update(con);

            await result.Async();
        }
    }
}
