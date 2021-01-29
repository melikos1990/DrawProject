using System.Threading.Tasks;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.IO
{
    public interface IVoiceFacade
    {
        Task<VoiceSearchResponse> GetList(VoiceRequest data);

        Task<VoiceMatchResponse> Match(string eid);
    }
}