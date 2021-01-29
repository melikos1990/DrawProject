using System.Threading.Tasks;

namespace SMARTII.Domain.Thread
{
    public static class TaskUtility
    {
        public static async Task<T> Async<T>(this T data)
        {
            return await Task.FromResult(data);
        }
    }
}