using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineAPI.Business.Interfaces
{
    public interface IViewModel<T> where T : class
    {
        Task<IEnumerable<T>> GetAllDetails();

        Task<T> GetDetails(int id);
    }
}
