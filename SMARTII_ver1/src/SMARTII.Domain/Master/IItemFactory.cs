using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.Master
{
    public interface IItemFactory
    {
        Task Update<T>(Item item);

        Task Create<T>(Item item);

        void DeleteImage(int id, string key);

        bool Import(DataTable data, ref ErrorContext<Item> errorCollection);
    }
}
