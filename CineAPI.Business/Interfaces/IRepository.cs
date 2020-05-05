using CineAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineAPI.Business.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetAllPaginate(int page, int limitPage);

        Task<T> GetById(int id);

        Task<int> CountActived();

        Task<int> CountDesactived();

        Task<IEnumerable<ComboBoxViewModel>> GetComboBox();

        Task<T> Create(T entity);

        Task<bool> Update(T entity);

        Task<bool> DeleteById(int id);

        Task<bool> IsExist(int id);
    }
}
