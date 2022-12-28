namespace Auxquimia.Repository.Management.Business.AssemblyBuilds
{
    using Auxquimia.Enums;
    using Auxquimia.Exceptions;
    using Auxquimia.Filters.Business.AssemblyBuilds;
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AssemblyBuildRepository" />.
    /// </summary>
    internal class AssemblyBuildRepository : NHibernateRepository, IAssemblyBuildRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public AssemblyBuildRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        public Task<IList<AssemblyBuild>> GetAllAsync()
        {
            return GetAllAsync<AssemblyBuild>();
        }

        /// <summary>
        /// The GetAssembliesToNetsuite.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        public async Task<IList<AssemblyBuild>> GetAssembliesToNetsuite()
        {
            return CurrentSession.QueryOver<AssemblyBuild>().Where(x => x.Status == ABStatus.FINISHED && !x.NetsuiteWritted && x.NetsuiteFormula != null && x.Formula == null).List();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public Task<AssemblyBuild> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<AssemblyBuild>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByMultipleStatus.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuild}}"/>.</returns>
        public Task<Page<AssemblyBuild>> GetByMultipleStatus(FindRequestImpl<BaseAssemblyBuildSearchFilter> filter)
        {
            IQueryOver<AssemblyBuild, AssemblyBuild> qo = CurrentSession.QueryOver<AssemblyBuild>();
            if (filter.Filter != null)
            {
                BaseAssemblyBuildSearchFilter uFilter = filter.Filter;
                qo.And(x => x.Status.IsIn(uFilter.MultipleStatusQuery));

                qo = ConstructQueryWithFilter(qo, uFilter);
            }


            return PaginatedAsync<AssemblyBuild>(qo, filter.PageRequest);
        }

        /// <summary>
        /// The GetSync.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="AssemblyBuild"/>.</returns>
        public AssemblyBuild GetSync(Guid assemblyId)
        {
            return CurrentSession.QueryOver<AssemblyBuild>().Where(x => x.Id == assemblyId).SingleOrDefault();
        }

        public Task<AssemblyBuild> GetToWaitingQueueAsync(Guid id)
        {
            return CurrentSession.QueryOver<AssemblyBuild>()
               .Where(x =>
               x.Id == id
               && x.Status == ABStatus.PENDING
               && (x.Formula != null || x.NetsuiteFormula != null)
               && x.Blender != null
               && x.Deadline != default(long)
               )
               .SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetToProductionAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public Task<AssemblyBuild> GetToProductionAsync(Guid id)
        {
            return CurrentSession.QueryOver<AssemblyBuild>()
                .Where(x =>
                x.Id == id
                && x.Status == ABStatus.WAITING
                && (x.Formula != null || x.NetsuiteFormula != null)
                && x.Blender != null
                && x.Deadline != default(long)
                )
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetTotalWoByStatus.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseAssemblyBuildSearchFilter"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetTotalWoByStatus(BaseAssemblyBuildSearchFilter filter)
        {
            IQueryOver<AssemblyBuild, AssemblyBuild> qo = CurrentSession.QueryOver<AssemblyBuild>();
            if (filter.Status == null)
            {
                throw new CustomException("ERROR STATUS NULL.");
            }
            qo.And(x => x.Status == filter.Status);
            if (filter.FactoryId != default(Guid))
            {
                qo.And(x => x.Factory.Id == filter.FactoryId);
            }
            qo = ConstructQueryWithFilter(qo, filter);

            return qo.RowCount();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuild}}"/>.</returns>
        public Task<Page<AssemblyBuild>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<AssemblyBuild>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuild}}"/>.</returns>
        public Task<Page<AssemblyBuild>> PaginatedAsync(FindRequestImpl<BaseAssemblyBuildSearchFilter> filter)
        {
            IQueryOver<AssemblyBuild, AssemblyBuild> qo = CurrentSession.QueryOver<AssemblyBuild>();

            if (filter.Filter != null)
            {
                BaseAssemblyBuildSearchFilter uFilter = filter.Filter;
                qo = ConstructQueryWithFilter(qo, uFilter);

            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The ConstructQueryWithFilter.
        /// </summary>
        /// <param name="qo">The qo<see cref="IQueryOver{AssemblyBuild, AssemblyBuild}"/>.</param>
        /// <param name="uFilter">The uFilter<see cref="BaseAssemblyBuildSearchFilter"/>.</param>
        /// <returns>The <see cref="IQueryOver{AssemblyBuild, AssemblyBuild}"/>.</returns>
        private IQueryOver<AssemblyBuild, AssemblyBuild> ConstructQueryWithFilter(IQueryOver<AssemblyBuild, AssemblyBuild> qo, BaseAssemblyBuildSearchFilter uFilter)
        {
            if (StringUtils.HasText(uFilter.AssemblyBuildNumber))
            {
                qo.And(Restrictions.On<AssemblyBuild>(x => x.AssemblyBuildNumber).IsInsensitiveLike(uFilter.AssemblyBuildNumber, MatchMode.Anywhere));
            }
            if (uFilter.Status != null)
            {
                qo.And(x => x.Status == uFilter.Status);
            }
            if (uFilter.FactoryId != default(Guid))
            {
                qo.And(x => x.Factory.Id == uFilter.FactoryId);
            }
            if (uFilter.OperatorAssignedId != default(Guid))
            {
                qo.And(x => x.Operator.Id == uFilter.OperatorAssignedId);
            }
            if(uFilter.BlenderId != default(Guid))
            {
                qo.And(x => x.Blender.Id == uFilter.BlenderId);
            }
            return qo;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public async Task<AssemblyBuild> SaveAsync(AssemblyBuild entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{AssemblyBuild}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<AssemblyBuild> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public async Task<AssemblyBuild> UpdateAsync(AssemblyBuild entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The GetAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public Task<AssemblyBuild> GetAsyncWithSession(ISession session, Guid id)
        {
            return session.QueryOver<AssemblyBuild>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The UpdateAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public async Task<AssemblyBuild> UpdateAsyncWithSession(ISession session, AssemblyBuild entity)
        {
            return await session.MergeAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The SaveAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        public async Task<AssemblyBuild> SaveAsyncWithSession(ISession session, AssemblyBuild entity)
        {
            await session.SaveAsync(entity).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// The FindAssembliesWaitingToProduction.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        public Task<IList<AssemblyBuild>> FindAssembliesWaitingToProduction(Guid reactorId)
        {
            return CurrentSession.QueryOver<AssemblyBuild>().Where(x =>
            x.Status == ABStatus.WAITING
            && x.Blender.Id == reactorId
            ).OrderBy(x => x.Deadline).Asc.ListAsync();
        }

        /// <summary>
        /// The CheckAssembliesOnProduction.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        public Task<IList<AssemblyBuild>> CheckAssembliesOnProduction(Guid reactorId)
        {
            return CurrentSession.QueryOver<AssemblyBuild>().Where(x =>
             x.Status == ABStatus.PROGRESS
             && x.Blender.Id == reactorId
             ).ListAsync();
        }
    }
}
