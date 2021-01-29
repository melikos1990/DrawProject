namespace SMARTII.Domain.Master
{
    public interface IItemRelationship
    {
        int ItemID { get; set; }

        string ItemName { get; set; }
    }

    public interface INullableItemRelationship
    {
        int? ItemID { get; set; }

        string ItemName { get; set; }
    }
}