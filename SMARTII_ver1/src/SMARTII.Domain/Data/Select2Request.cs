namespace SMARTII.Domain.Data
{
    public class Select2Request
    {
        public int size { get; set; }

        public int pageIndex { get; set; }

        public string keyword { get; set; }

        public int? parentID { get; set; }

        public int start { get { return this.pageIndex * this.size; } }
    }

    public class Select2Request<T> : Select2Request
    {
        public T criteria { get; set; }
    }
}