using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.Database.MVC
{
    public class RepositoryBase<T> : IRepository<T>, IDisposable
    {
        protected ISession _session = null;
        protected ITransaction _transaction = null;

        public RepositoryBase()
        {
            _session = Database.OpenSession();
        }
        public RepositoryBase(ISession session)
        {
            _session = session;
        }

        #region Transaction and Session Management Methods

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            // _transaction will be replaced with a new transaction            // by NHibernate, but we will close to keep a consistent state.
            _transaction.Commit();

            CloseTransaction();
        }

        public void RollbackTransaction()
        {
            // _session must be closed and disposed after a transaction            // rollback to keep a consistent state.
            _transaction.Rollback();

            CloseTransaction();
            CloseSession();
        }

        private void CloseTransaction()
        {
            _transaction.Dispose();
            _transaction = null;
        }

        private void CloseSession()
        {
            _session.Close();
            _session.Dispose();
            _session = null;
        }

        #endregion

        #region IRepository Members
        public Task Delete(T entity)
        {
            return _session.DeleteAsync(entity);
        }
        public Task<IList<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Guid id)
        {
            return _session.LoadAsync<T>(id);
        }

        public Task Save(T entity)
        {
            //_session.SaveOrUpdate(entity);
            return _session.SaveAsync(entity);
        }
        #endregion
        #region IDisposable Members
        public void Dispose()
        {
            if (_transaction != null)
            {
                // Commit transaction by default, unless user explicitly rolls it back.
                // To rollback transaction by default, unless user explicitly commits,                // comment out the line below.
                CommitTransaction();
            }

            if (_session != null)
            {
                _session.Flush(); // commit session transactions
                CloseSession();
            }
        }
        #endregion
    }
}
