using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.ASO.Domain
{
    public class ASOAssignmentCallHistory
    {
        /// <summary>
        /// 來源
        /// </summary>
        public CaseSourceType CaseSourceType { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 序號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 立案日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 問題分類
        /// </summary>
        public QuestionClassification ClassList { get; set; }
        /// <summary>
        /// 反應區課
        /// </summary>
        public string ComplainParent { get; set; }
        /// <summary>
        /// 反應門市
        /// </summary>
        public string ComplainedNodeName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 聯繫電話
        /// </summary>
        public string ConcatMobile { get; set; }
        /// <summary>
        /// 是否開立客訴
        /// </summary>
        public string IsInvoice { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 回覆
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 處理人員
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 轉派單位人員
        /// </summary>
        public string AssignmentUserName { get; set; }
        /// <summary>
        /// 結案日期
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        /// 結案時間
        /// </summary>
        public string FinishTime { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public string CaseType { get; set; }
        /// <summary>
        /// 信件回覆日期
        /// </summary>
        public string ReplyDate { get; set; }
        /// <summary>
        /// 信件回覆時間
        /// </summary>
        public string ReplyTime { get; set; }
        /// <summary>
        /// 立案時間(A)
        /// </summary>
        public string CreatDateTime { get; set; }
        /// <summary>
        /// 轉派時間(B)
        /// </summary>
        public string AssignmentCreatDateTime { get; set; }
        /// <summary>
        /// 轉派內容
        /// </summary>
        public string AssignmentContent { get; set; }
        /// <summary>
        /// 轉派單位回覆時間(C)
        /// </summary>
        public string AssignmentFinishDateTime { get; set; }
        /// <summary>
        /// 轉派回覆內容
        /// </summary>
        public string AssignmentResume { get; set; }
        /// <summary>
        /// 結案時間(D)
        /// </summary>
        public string CaseFinishDateTime { get; set; }
        /// <summary>
        /// 非轉派案件處理時間(D-A)
        /// </summary>
        public string WithoutAssignmentProcessDateTime { get; set; }
        /// <summary>
        /// 轉派案件處理時間(C-B)
        /// </summary>
       public string AssignmentProcessDateTime { get; set; }
       /// <summary>
       /// 滿意度
       /// </summary>
        public string Satisfaction { get; set; }
    }
}
