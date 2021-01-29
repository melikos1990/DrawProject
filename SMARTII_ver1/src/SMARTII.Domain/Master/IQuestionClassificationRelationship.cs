namespace SMARTII.Domain.Master
{
    public interface IQuestionClassificationRelationship
    {
        int QuestionClassificationID { get; set; }

        string QuestionClassificationName { get; set; }

        string QuestionClassificationParentNames { get; set; }

        string QuestionClassificationParentPath { get; set; }

        string[] QuestionClassificationParentNamesByArray { get; set; }

        string[] QuestionClassificationParentPathByArray { get; set; }
    }

    public interface INullableQuestionClassificationRelationship
    {
        int? QuestionClassificationID { get; set; }

        string QuestionClassificationName { get; set; }

        string QuestionClassificationParentNames { get; set; }

        string QuestionClassificationParentPath { get; set; }

        string[] QuestionClassificationParentNamesByArray { get; set; }

        string[] QuestionClassificationParentPathByArray { get; set; }
    }
}