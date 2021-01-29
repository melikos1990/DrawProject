namespace SMARTII.Domain.Organization
{
    public interface IUserRelationship
    {
        string UserID { get; set; }

        string UserName { get; set; }

        User User { get; set; }
    }
}