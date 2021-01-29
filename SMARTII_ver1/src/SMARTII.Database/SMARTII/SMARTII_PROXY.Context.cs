namespace SMARTII.Database.SMARTII
{
    public partial class SMARTIIEntities
    {
        public SMARTIIEntities(string conn) : base(conn)
        {
            Database.CommandTimeout = 120;
        }
    }
}
