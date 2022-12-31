using Auxquimia.Utils.MVC.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.InternalDatabase
{
    public interface IRepositoryBase<T> { 
        Task<IList<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
    }
}
