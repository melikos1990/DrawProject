using System;
using System.Linq;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class CaseTemplateFacade : ICaseTemplateFacade
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public CaseTemplateFacade(ICommonAggregate CommonAggregate,
                                  ISystemAggregate SystemAggregate,
                                  IMasterAggregate MasterAggregate,
                                  IOrganizationAggregate OrganizationAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _SystemAggregate = SystemAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        public Task<CaseTemplate> Create(CaseTemplate data)
        {
            #region 驗證主旨、預設內容、快速結案、類別
            //主旨
            var item = _MasterAggregate.CaseTemplate_T1_T2_
                                       .Get(x => x.NODE_ID == data.NodeID &&
                                                 x.CLASSIFIC_KEY == data.ClassificKey &&
                                                 x.TITLE == data.Title);

            if (item != null)
                throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_TITLE_REPEAT);
            //類別
            var classdata = _SystemAggregate.SystemParameter_T1_T2_.Get(x => x.ID == EssentialCache.LayoutValue.CaseTemplate && x.KEY == data.ClassificKey);
            if (classdata == null)
                throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_CLASSIFICKEY_NOT_EXIST);

            //預設內容
            if (data.IsDefault)
            {
                var defaultdata = _MasterAggregate.CaseTemplate_T1_T2_
                                       .GetList(x => x.NODE_ID == data.NodeID &&
                                                     x.IS_DEFAULT == true &&
                                                    x.CLASSIFIC_KEY == data.ClassificKey);
                if (defaultdata.Any())
                    throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_ISDEFAULT_REPEAT);
            }
            //快速結案
            if (data.IsFastFinished)
            {
                //取得BU KEY
                var buCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == data.NodeID);
                var bu = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buCon, x => new HeaderQuarterNode() { NodeKey = x.NODE_KEY });

                //1代表 有開快速結案
                var fastdata = _SystemAggregate.SystemParameter_T1_T2_.GetList(x => x.ID == EssentialCache.CaseValue.CASE_ALLOW_FASTCLOSE && x.KEY == bu.NodeKey && x.VALUE == "1");
                if (fastdata == null)
                    throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_FASTCLOSED_FAIL);
            }

            #endregion


            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            var result = _MasterAggregate.CaseTemplate_T1_T2_.Add(data);

            return result.Async();
        }

        public Task<CaseTemplate> Update(CaseTemplate data)
        {
            #region 驗證主旨、預設內容、快速結案、類別
            string title = data.Title.Trim();

            // 修改後的 , 其他的資料(除了本身)是否有相同
            var item = _MasterAggregate.CaseTemplate_T1_T2_
                                       .Get(x => x.ID != data.ID &&
                                                 x.NODE_ID == data.NodeID &&
                                                 x.CLASSIFIC_KEY == data.ClassificKey &&
                                                 x.TITLE == title);

            if (item != null)
                throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_TITLE_REPEAT);
            //類別
            var classdata = _SystemAggregate.SystemParameter_T1_T2_.Get(x => x.ID == EssentialCache.LayoutValue.CaseTemplate && x.KEY == data.ClassificKey);
            if (classdata == null)
                throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_CLASSIFICKEY_NOT_EXIST);

            //預設內容
            if (data.IsDefault)
            {
                var defaultdata = _MasterAggregate.CaseTemplate_T1_T2_
                                       .GetList(x => x.NODE_ID == data.NodeID &&
                                                 x.CLASSIFIC_KEY == data.ClassificKey && 
                                                 x.ID != data.ID &&
                                                 x.IS_DEFAULT == true);
                if (defaultdata.Any())
                    throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_ISDEFAULT_REPEAT);
            }
            //快速結案
            if (data.IsFastFinished)
            {
                //取得BU KEY
                var buCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == data.NodeID);
                var bu = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buCon, x => new HeaderQuarterNode() { NodeKey = x.NODE_KEY });
                //1代表 有開快速結案
                var fastdata = _SystemAggregate.SystemParameter_T1_T2_.GetList(x => x.ID == EssentialCache.CaseValue.CASE_ALLOW_FASTCLOSE && x.KEY == bu.NodeKey && x.VALUE == "1");
                if (fastdata == null)
                    throw new Exception(CaseTemplate_lang.CASE_TEMPLATE_FASTCLOSED_FAIL);
            }
            #endregion


            var con = new MSSQLCondition<CASE_TEMPLATE>(x => x.ID == data.ID);
            con.ActionModify(x =>
            {
                x.TITLE = data.Title;
                x.CONTENT = data.Content;
                x.EMAIL_TITLE = data.EmailTitle;
                x.IS_DEFAULT = data.IsDefault;
                x.IS_FAST_FINISH = data.IsFastFinished;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _MasterAggregate.CaseTemplate_T1_T2_.Update(con);

            return result.Async();
        }
        
    }
}
