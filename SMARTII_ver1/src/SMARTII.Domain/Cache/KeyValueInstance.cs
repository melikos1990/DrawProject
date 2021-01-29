namespace SMARTII.Domain.Cache
{
    public static class KeyValueInstance<T1, T2>
    {
        public static SignalRCache<T1, T2> SignalRConnections = new SignalRCache<T1, T2>();

        public static CaseCache<T1, T2> Room = new CaseCache<T1, T2>();
    }
}
