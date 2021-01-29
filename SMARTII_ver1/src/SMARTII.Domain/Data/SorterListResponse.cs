namespace SMARTII.Domain.Data
{
    public class SorterListResponse<T>
    {
        public SorterListResponse()
        {
        }

        public string id { get; set; }

        public string text { get; set; }

        public int order { get; set; }

        public T extend { get; set; }
    }
}
