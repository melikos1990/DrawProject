using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Areas.Master.Models.CaseFinishReason;
using SMARTII.Areas.Master.Models.CaseTag;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseDetailViewModel
    {
        public CaseDetailViewModel()
        {
        }

        public CaseDetailViewModel(Domain.Case.Case @case)
        {
            if (@case == null) return;

            this.GroupID = @case.GroupID;
            this.CaseID = @case.CaseID;
            this.SourceID = @case.SourceID;
            this.NodeID = @case.NodeID;
            this.OrganizationType = @case.OrganizationType;
            this.ApplyDateTime = @case.ApplyDateTime;
            this.ApplyUserID = @case.ApplyUserID;
            this.ApplyUserName = @case.ApplyUserName;
            this.CaseType = @case.CaseType;
            this.CaseTypeName = @case.CaseType.GetDescription();
            this.CaseWarningID = @case.CaseWarningID;
            this.CaseWarningName = @case.CaseWarning?.Name;
            this.Content = @case.Content;
            this.GroupID = @case.GroupID;
            this.CreateDateTime = @case.CreateDateTime;
            this.CreateUserName = @case.CreateUserName;
            this.ExpectDateTime = @case.ExpectDateTime;
            this.FinishFilePath = @case.FinishFilePath;
            this.FilePath = @case.FilePath;
            this.FinishContent = @case.FinishContent;
            this.FinishDateTime = @case.FinishDateTime;
            this.FinishEMLFilePath = @case.FinishEMLFilePath;
            this.FinishReplyDateTime = @case.FinishReplyDateTime;
            this.FinishUserName = @case.FinishUserName;
            this.IsReport = @case.IsReport;
            this.IsAttension = @case.IsAttension;
            this.PromiseDateTime = @case.PromiseDateTime;
            this.QuestionClassificationID = @case.QuestionClassificationID;
            this.RelationCaseIDs = @case.RelationCaseIDs;
            this.Particular = @case.GetParticular<ExpandoObject>();
            this.JContent = @case.JContent;
            this.CaseRemindIDs = @case.CaseReminds?.Where(x => x.AssignmentID == null)?.Select(x => x.ID).ToList() ?? new List<int>(); // 只取設定該案件的追蹤(不含轉派)
            this.QuestionClassificationGroups = Enumerable.Zip(
             @case?.QuestionClassificationParentPathByArray ?? new string[] { },
             @case?.QuestionClassificationParentNamesByArray ?? new string[] { },
             (id, text) =>
             {
                 return new SelectItem()
                 {
                     id = id,
                     text = text
                 };
             }).ToList();

            this.CaseTags = @case.CaseTags?
                                 .Select(x => new CaseTagListViewModel(x))
                                 .ToList();

            this.CaseComplainedUsers = @case.CaseComplainedUsers?
                                            .Select(x => new CaseComplainedUserViewModel(x))
                                            .ToList();

            this.CaseConcatUsers = @case.CaseConcatUsers?
                                        .Select(x => new CaseConcatUserViewModel(x))
                                        .ToList();

            this.CaseFinishReasons = @case.CaseFinishReasonDatas?
                                          .Select(x => new CaseFinishDataListViewModel(x))
                                          .ToList();

            this.LookupUsers =
            KeyValueInstance<string, User>.Room
                                          .GetLookupUsers(this.CaseID)?
                                          .Select(x =>

                                              new SelectItem()
                                              {
                                                  id = x.UserID,
                                                  text = x.Name
                                              }
                                          )
                                          .ToList();


        }

        public Domain.Case.Case ToDomain()
        {
            var @case = new Domain.Case.Case();

            @case.GroupID = this.GroupID;
            @case.CaseID = this.CaseID;
            @case.SourceID = this.SourceID;
            @case.NodeID = this.NodeID;
            @case.OrganizationType = this.OrganizationType;
            @case.ApplyDateTime = this.ApplyDateTime;
            @case.ApplyUserID = this.ApplyUserID;
            @case.ApplyUserName = this.ApplyUserName;

            @case.CaseType = this.CaseType;
            @case.CaseWarningID = this.CaseWarningID;
            @case.Content = this.Content;
            @case.CreateDateTime = this.CreateDateTime;
            @case.CreateUserName = this.CreateUserName;
            @case.ExpectDateTime = this.ExpectDateTime;
            @case.Files = this.Files;
            @case.FinishContent = this.FinishContent;
            @case.FinishDateTime = this.FinishDateTime;
            @case.FinishEMLFilePath = this.FinishEMLFilePath;
            @case.FinishFiles = this.FinishFiles;
            @case.FinishUserName = this.FinishUserName;
            @case.IsReport = this.IsReport;
            @case.IsAttension = this.IsAttension;
            @case.PromiseDateTime = this.PromiseDateTime;
            @case.QuestionClassificationID = this.QuestionClassificationID;
            @case.RelationCaseIDs = this.RelationCaseIDs;
            @case.JContent = this.JContent;

            @case.CaseComplainedUsers = this.CaseComplainedUsers?
                                            .Select(x => x.ToDomain())
                                            .ToList();

            @case.CaseConcatUsers = this.CaseConcatUsers?
                                        .Select(x => x.ToDomain())
                                        .ToList();

            @case.CaseTags = this.CaseTagsMark?
                                 .Select(x =>
                                    {
                                        var isIdentity = int.TryParse(x.id, out int currentID);

                                        return new CaseTag()
                                        {
                                            ID = isIdentity ? currentID : 0,
                                            Name = x.text,
                                            Target = isIdentity == false
                                        };
                                    })
                                 .ToList() ?? new List<CaseTag>();

            @case.CaseFinishReasonDatas = this.CaseFinishReasons?
                                              .Select(x => x.ToDomain())
                                              .ToList();




            return @case;
        }


        /// <summary>
        /// 案件來源代號
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 企業代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        ///案件負責人代號
        /// </summary>
        public string ApplyUserID { get; set; }

        /// <summary>
        ///認養時間
        /// </summary>
        public DateTime ApplyDateTime { get; set; }

        /// <summary>
        /// 認養人姓名
        /// </summary>
        public string ApplyUserName { get; set; }

        /// <summary>
        /// 反應內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 案件狀態(0 : 立案 ; 1 : 處理中 ; 2 : 結案)
        /// </summary>
        public CaseType CaseType { get; set; }

        /// <summary>
        /// 案件狀態(0 : 立案 ; 1 : 處理中 ; 2 : 結案)
        /// </summary>
        public string CaseTypeName { get; set; }

        /// <summary>
        /// 執行單位代號
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        ///應完成時間
        /// </summary>
        public DateTime PromiseDateTime { get; set; }

        /// <summary>
        ///客戶期望完成時間
        /// </summary>
        public DateTime? ExpectDateTime { get; set; }
        /// <summary>
        /// 案件附件路徑
        /// </summary>
        public string[] FilePath { get; set; }

        /// <summary>
        /// 案件附件
        /// </summary>
        public List<HttpFile> Files { get; set; }


        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }

        /// <summary>
        /// 結案附件路徑
        /// </summary>
        public string[] FinishFilePath { get; set; }

        /// <summary>
        /// 結案附件
        /// </summary>
        public List<HttpFile> FinishFiles { get; set; }

        /// <summary>
        ///結案時間
        /// </summary>
        public DateTime? FinishDateTime { get; set; }

        /// <summary>
        /// 結案人
        /// </summary>
        public string FinishUserName { get; set; }

        /// <summary>
        /// 結案附件留存路徑
        /// </summary>
        public string FinishEMLFilePath { get; set; }

        /// <summary>
        /// 結案回覆時間
        /// </summary>
        public DateTime? FinishReplyDateTime { get; set; }
        

        /// <summary>
        ///是否列入日報
        /// </summary>
        public bool IsReport { get; set; }

        /// <summary>
        ///是否待關注
        /// </summary>
        public bool IsAttension { get; set; }

        /// <summary>
        ///問題分類代號
        /// </summary>
        public int QuestionClassificationID { get; set; }

        /// <summary>
        ///緊急程度代號
        /// </summary>
        public int CaseWarningID { get; set; }

        /// <summary>
        /// 緊急程度名稱
        /// </summary>
        public string CaseWarningName { get; set; }


        /// <summary>
        /// 相關案件代號清單
        /// </summary>
        public string[] RelationCaseIDs { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 動態欄位
        /// </summary>
        public ExpandoObject Particular { get; set; } = new ExpandoObject();

        /// <summary>
        /// 動態文字
        /// </summary>
        public string JContent { get; set; }

        /// <summary>
        /// 案件追蹤識別值
        /// </summary>
        public List<int> CaseRemindIDs { get; set; }

        #region Other

        public List<SelectItem> QuestionClassificationGroups { get; set; } = new List<SelectItem>();

        public List<SelectItem> CaseTagsMark { get; set; }

        public List<CaseTagListViewModel> CaseTags { get; set; }

        public List<CaseComplainedUserViewModel> CaseComplainedUsers { get; set; }

        public List<CaseConcatUserViewModel> CaseConcatUsers { get; set; }

        public List<CaseFinishDataListViewModel> CaseFinishReasons { get; set; }


        public List<SelectItem> LookupUsers { get; set; }

        #endregion


        #region 操作組織相關

        /// <summary>
        /// 操作人員組織
        /// </summary>
        public int EditorNodeJobID { get; set; }

        #endregion




    }
}
