using System.Dynamic;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Notification;

namespace SMARTII.Domain.Master
{
    public interface IMasterAggregate
    {
        IMSSQLRepository<CASE_TEMPLATE> CaseTemplate_T1_ { get; }
        IMSSQLRepository<CASE_TEMPLATE, CaseTemplate> CaseTemplate_T1_T2_ { get; }
        IMSSQLRepository<ITEM, Item<ExpandoObject>> Item_T1_T2_Expendo_ { get; }
        IMSSQLRepository<ITEM, Item> Item_T1_T2_ { get; }
        IMSSQLRepository<QUESTION_CLASSIFICATION> QuestionClassification_T1_ { get; }
        IMSSQLRepository<QUESTION_CLASSIFICATION, QuestionClassification> QuestionClassification_T1_T2_ { get; }
        IMSSQLRepository<QUESTION_CLASSIFICATION_ANSWER> QuestionClassificationAnswer_T1_ { get; }
        IMSSQLRepository<QUESTION_CLASSIFICATION_ANSWER, QuestionClassificationAnswer> QuestionClassificationAnswer_T1_T2_ { get; }
        IMSSQLRepository<QUESTION_CLASSIFICATION_GUIDE> QuestionClassificationGuide_T1_ { get; }
        IMSSQLRepository<QUESTION_CLASSIFICATION_GUIDE, QuestionClassificationGuide> QuestionClassificationGuide_T1_T2_ { get; }
        IMSSQLRepository<VW_QUESTION_CLASSIFICATION_NESTED> VWQuestionClassification { get; }
        IMSSQLRepository<VW_QUESTION_CLASSIFICATION_NESTED, QuestionClassification> VWQuestionClassification_QuestionClassification_ { get; }
        IMSSQLRepository<VW_QUESTION_CLASSIFICATION_ANSWER_NESTED, QuestionClassificationAnswer> VWQuestionClassificationAnswer_QuestionClassificationAnswer_ { get; }
        IMSSQLRepository<VW_QUESTION_CLASSIFICATION_GUIDE_NESTED, QuestionClassificationGuide> VWQuestionClassificationGuide_QuestionClassificationGuide_ { get; }
        IMSSQLRepository<KM_CLASSIFICATION> KMClassification_T1_ { get; }
        IMSSQLRepository<KM_CLASSIFICATION, KMClassification> KMClassification_T1_T2_ { get; }
        IMSSQLRepository<VW_KM_CLASSIFICATION_NESTED> VWKMClassification { get; }
        IMSSQLRepository<VW_KM_CLASSIFICATION_NESTED, KMClassification> VWKMClassification_KMClassification_ { get; }
        IMSSQLRepository<KM_DATA> KMData_T1_ { get; }
        IMSSQLRepository<KM_DATA, KMData> KMData_T1_T2_ { get; }
        IMSSQLRepository<CASE_FINISH_REASON_CLASSIFICATION> CaseFinishReasonClassification_T1_ { get; }
        IMSSQLRepository<CASE_FINISH_REASON_CLASSIFICATION, CaseFinishReasonClassification> CaseFinishReasonClassification_T1_T2_ { get; }

        IMSSQLRepository<CASE_FINISH_REASON_DATA> CaseFinishReasonData_T1_ { get; }
        IMSSQLRepository<CASE_FINISH_REASON_DATA, CaseFinishReasonData> CaseFinishReasonData_T1_T2_ { get; }

        IMSSQLRepository<CASE_TAG> CaseTag_T1_ { get; }
        IMSSQLRepository<CASE_TAG, CaseTag> CaseTag_T1_T2_ { get; }
        IMSSQLRepository<CASE_WARNING> CaseWarning_T1_ { get; }
        IMSSQLRepository<CASE_WARNING, CaseWarning> CaseWarning_T1_T2_ { get; }

        IMSSQLRepository<BILL_BOARD> Billboard_T1_ { get; }
        IMSSQLRepository<BILL_BOARD, Billboard> Billboard_T1_T2_ { get; }

        IMSSQLRepository<WORK_SCHEDULE> WorkSchedule_T1_ { get; }
        IMSSQLRepository<WORK_SCHEDULE, WorkSchedule> WorkSchedule_T1_T2_ { get; }



        IMSSQLRepository<USER_PARAMETER> UserParameter_T1_ { get; }
        IMSSQLRepository<USER_PARAMETER, UserParameter> UserParameter_T1_T2_ { get; }


    }
}
