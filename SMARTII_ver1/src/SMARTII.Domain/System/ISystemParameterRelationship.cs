namespace SMARTII.Domain.System
{
    public interface ISystemParameterRelationship
    {
        string ID { get; set; }

        string Key { get; set; }

        string Value { get; set; }

        string Text { get; set; }
    }
}