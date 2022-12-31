using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.InternalDatabase
{

    public abstract class RepositoryBase<T> : IRepositoryBase<T>
    {
        protected ISession _session { get { return nhibernateSessionProvider.CurrentSession(); } }
        protected ITransaction _transaction = null;
        private NHibernateSessionProvider nhibernateSessionProvider { get; set; }
        private IServiceProvider serviceProvider { get; set; }

        public RepositoryBase(IServiceProvider serviceProvider, NHibernateSessionProvider sessionProvider)
        {
            this.nhibernateSessionProvider = sessionProvider;
            this.serviceProvider = serviceProvider;
        }

        #region IRepository Members

        public abstract Task<IList<T>> GetAllAsync();

        public abstract Task<T> GetAsync(Guid id);

        //public abstract Task Delete(T entity);

        //public abstract Task<T> Save(T entity);

        public abstract Task<T> Update(T entity);
        //public Task Delete(T entity)
        //{
        //    return _session.DeleteAsync(entity);
        //}
        //public Task<IList<T>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetAsync(Guid id)
        //{
        //    return _session.LoadAsync<T>(id);
        //}

        public Task<T> Save(T entity)
        {
            //_session.SaveOrUpdate(entity);
            return (Task<T>)_session.SaveOrUpdateAsync(entity);
        }
        //public Task<T> Update(T entity)
        //{
        //    return Save(entity);
        //}
        #endregion
    }
}
