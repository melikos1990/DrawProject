using System.Web;
using MultipartDataMediaFormatter.Infrastructure;

namespace SMARTII.Domain.Data
{
    public  class FileUploadViewModel
    {

        public HttpFile File { get; set; }

        public HttpFile[] Files { get; set; }
    }
    
}
