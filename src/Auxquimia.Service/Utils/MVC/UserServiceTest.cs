using Auxquimia.Model.Authentication;
using Auxquimia.Utils.MVC.InternalDatabase;
using Auxquimia.Utils.MVC.Tools.Servs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.Database.MVC
{
    internal class UserServiceTest : IService<User, Guid>
    {
        public Task<IList<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByID(Guid id)
        {
            Task<User> person = null;
            using (RepositoryBase<User> repository = new RepositoryBase<User>())
            {
                try
                {
                    repository.BeginTransaction();

                    person = repository.GetAsync(id);
                }
                catch
                {
                    repository.RollbackTransaction();
                }
            }
            return person;
        }

        public Task<User> SaveAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(IList<User> entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
