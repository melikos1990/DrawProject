namespace SMARTII.Domain.Case
{
    public interface ICaseSourceRelationship
    {
        string SourceID { get; set; }

        CaseSource CaseSource { get; set; }
    }
}
