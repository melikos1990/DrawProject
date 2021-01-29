using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class QuestionClassificationFacade : IQuestionClassificationFacade
    {
        private readonly IMasterAggregate _IMasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public QuestionClassificationFacade(ICommonAggregate CommonAggregate,
                                             IMasterAggregate IMasterAggregate,
                                             IOrganizationAggregate OrganizationAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _IMasterAggregate = IMasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        

        public async Task DeleteAsync(int id)
        {
            var now = DateTime.Now;
            var children = new List<QUESTION_CLASSIFICATION>();
            

            _IMasterAggregate.QuestionClassification_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                var query = db.QUESTION_CLASSIFICATION
                                .AsQueryable();

                var question = query.Where(x => x.ID == id).SingleOrDefault();

                if (question == null)
                    throw new Exception(QuestionClassification_lang.QUESTIONCLASSIFICATION_NOT_FOUND);

                children.AddRange(this.RecursivelyChildren(question));


                var childrenIds = children.Select(x => x.ID).ToList();
                childrenIds.Add(question.ID);
                
                if (hasNotFinishedCase(db, childrenIds))
                    throw new Exception(QuestionClassification_lang.QUESTIONCLASSIFICATION_HASNOTFINISHEDCASE);

                children.ForEach(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                });

                question.IS_ENABLED = false;
                question.UPDATE_DATETIME = now;
                question.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;

                db.SaveChanges();
            });
        }

        public async Task DeleteRangeAsync(int[] ids)
        {
            var now = DateTime.Now;
            var children = new List<QUESTION_CLASSIFICATION>();

            _IMasterAggregate.QuestionClassification_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                var query = db.QUESTION_CLASSIFICATION
                                .AsQueryable();

                var questions = query.Where(x => ids.Contains(x.ID)).ToList();

                questions.ForEach(item =>
                {
                    children.AddRange(this.RecursivelyChildren(item));

                    item.IS_ENABLED = false;
                    item.UPDATE_DATETIME = now;
                    item.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name; 
                });


                var childrenIds = children.Select(x => x.ID).ToList();
                childrenIds.AddRange(questions.Select(x => x.ID));

                if (hasNotFinishedCase(db, childrenIds))
                    throw new Exception(QuestionClassification_lang.QUESTIONCLASSIFICATION_HASNOTFINISHEDCASE);

                children.ForEach(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                });

                db.SaveChanges();
            });
        }

        public async Task UpdateAsync(Domain.Master.QuestionClassification domain)
        {
            var children = new List<QUESTION_CLASSIFICATION>();
            var now = DateTime.Now;
            

            this._IMasterAggregate.QuestionClassification_T1_T2_.Operator(context =>
            {
                var db = (Database.SMARTII.SMARTIIEntities)context;
                
                var isExist = db.QUESTION_CLASSIFICATION.Any(x => x.PARENT_ID == domain.ParentID && x.NAME == domain.Name && x.ID != domain.ID && x.NODE_ID == domain.NodeID);
                
                // 是否存在同樣名稱
                if(isExist) throw new Exception(QuestionClassification_lang.QUESTIONCLASSIFICATION_NAME_DUPLICATE);

                var question = db.QUESTION_CLASSIFICATION
                                .AsQueryable()
                                .Where(x =>
                                    x.NODE_ID == domain.NodeID &&
                                    x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter &&
                                    x.ID == domain.ID).SingleOrDefault();

                if (question == null) throw new Exception(Common_lang.NOT_FOUND_DATA);


                // 撈出底下的巢狀資料 (跳過自己)
                children.AddRange(this.RecursivelyChildren(question));
                children = children.Skip(1).ToList();

                // 啟用狀態是否異動
                if (question.IS_ENABLED != domain.IsEnabled)
                {

                    var childrenIds = children.Select(x => x.ID).ToList();
                    childrenIds.Add(question.ID);

                    if (hasNotFinishedCase(db, childrenIds))
                        throw new Exception(QuestionClassification_lang.QUESTIONCLASSIFICATION_HASNOTFINISHEDCASE);


                    children.ForEach(x =>
                    {
                        x.IS_ENABLED = domain.IsEnabled;
                    });
                }

                // 層級是否異動
                if (question.LEVEL != domain.Level)
                {
                    // 計算層級差異
                    var offset = domain.Level - question.LEVEL;

                    children.ForEach(x =>
                    {
                        x.LEVEL = x.LEVEL + offset;
                    });
                }


                question.NAME = domain.Name;
                question.IS_ENABLED = domain.IsEnabled;
                question.PARENT_ID = domain.ParentID;
                question.LEVEL = domain.Level;
                question.UPDATE_USERNAME = domain.UpdateUserName;
                question.UPDATE_DATETIME = domain.UpdateDateTime;
                

                db.SaveChanges();
            });
        }


        public async Task CreateAsync(QuestionClassification domain)
        {

            // 比對名稱是否重複
            if(_IMasterAggregate.QuestionClassification_T1_T2_.HasAny(x =>
                x.PARENT_ID == domain.ParentID &&
                x.NAME == domain.Name &&
                x.NODE_ID == domain.NodeID
            ))
                throw new Exception(QuestionClassification_lang.QUESTIONCLASSIFICATION_NAME_DUPLICATE);
            
            // 取得最後的擺放位置 預設為0
            var con = new MSSQLCondition<QUESTION_CLASSIFICATION>(x => x.NODE_ID == domain.NodeID && x.PARENT_ID == domain.ParentID);
            con.OrderBy(x => x.ORDER, OrderType.Desc);
            var order = _IMasterAggregate.QuestionClassification_T1_T2_.GetFirstOrDefault(con)?.Order ?? 0;

            domain.Order = order + 1;


            _IMasterAggregate.QuestionClassification_T1_T2_.Add(domain);
        }


        private List<QUESTION_CLASSIFICATION> RecursivelyChildren(QUESTION_CLASSIFICATION question)
        {
            var result = new List<QUESTION_CLASSIFICATION>();

            if (question.QUESTION_CLASSIFICATION1 != null && question.QUESTION_CLASSIFICATION1.Count > 0)
            {
                question.QUESTION_CLASSIFICATION1.ToList().ForEach(child =>
                {
                    result.AddRange(this.RecursivelyChildren(child));
                });
            }

            result.Insert(0, question);

            return result;
        }


        private bool hasNotFinishedCase(IDisposable context, IEnumerable<int> collectionIds)
        {
            var db = (SMARTIIEntities)context;
            return db.CASE.Any(x => collectionIds.Contains(x.QUESION_CLASSIFICATION_ID) && x.CASE_TYPE != (byte)CaseType.Finished);
        }

    }
}
