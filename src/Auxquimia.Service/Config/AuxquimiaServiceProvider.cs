namespace Auxquimia.Config
{
    using Auxquimia.Repository.Authentication;
    using Auxquimia.Repository.Business.Formulas;
    using Auxquimia.Repository.Management.Business.AssemblyBuilds;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Service.Business.AssemblyBuilds;
    using Izertis.NHibernate.Repositories;
    using NHibernate;
    using System;

    /// <summary>
    /// Defines the <see cref="IAuxquimiaServiceProvider" />.
    /// </summary>
    public interface IAuxquimiaServiceProvider
    {
    }

    /// <summary>
    /// Defines the <see cref="AuxquimiaServiceProvider" />.
    /// </summary>
    public class AuxquimiaServiceProvider : IAuxquimiaServiceProvider
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        private static AuxquimiaServiceProvider instance { get; set; }

        /// <summary>
        /// Defines the ServiceProvider.
        /// </summary>
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// Defines the sessionFactoryProvider.
        /// </summary>
        private readonly IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuxquimiaServiceProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public AuxquimiaServiceProvider(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.sessionFactoryProvider = sessionFactoryProvider;
            instance = this;
        }

        /// <summary>
        /// The GetAssemblyBuildService.
        /// </summary>
        /// <returns>The <see cref="IAssemblyBuildService"/>.</returns>
        public static IAssemblyBuildService GetAssemblyBuildService()
        {
            if (instance != null)
            {
                IAssemblyBuildService service = (IAssemblyBuildService)instance.ServiceProvider.GetService(typeof(IAssemblyBuildService));
                return service;
            }
            return null;
        }

        /// <summary>
        /// The GetAssemblyBuildRepository.
        /// </summary>
        /// <returns>The <see cref="IAssemblyBuildRepository"/>.</returns>
        public static IAssemblyBuildRepository GetAssemblyBuildRepository()
        {
            if (instance != null)
            {
                IAssemblyBuildRepository repo = (IAssemblyBuildRepository)instance.ServiceProvider.GetService(typeof(IAssemblyBuildRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetUserRepository.
        /// </summary>
        /// <returns>The <see cref="IUserRepository"/>.</returns>
        public static IUserRepository GetUserRepository()
        {
            if (instance != null)
            {
                IUserRepository repo = (IUserRepository)instance.ServiceProvider.GetService(typeof(IUserRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetReactorRepository.
        /// </summary>
        /// <returns>The <see cref="IReactorRepository"/>.</returns>
        public static IReactorRepository GetReactorRepository()
        {
            if (instance != null)
            {
                IReactorRepository repo = (IReactorRepository)instance.ServiceProvider.GetService(typeof(IReactorRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetNetsuiteStepRepository.
        /// </summary>
        /// <returns>The <see cref="INetsuiteFormulaRepository"/>.</returns>
        public static INetsuiteFormulaStepRepository GetNetsuiteStepRepository()
        {
            if (instance != null)
            {
                INetsuiteFormulaStepRepository repo = (INetsuiteFormulaStepRepository)instance.ServiceProvider.GetService(typeof(INetsuiteFormulaStepRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetFormulaStepRepository.
        /// </summary>
        /// <returns>The <see cref="IFormulaStepRepository"/>.</returns>
        public static IFormulaStepRepository GetFormulaStepRepository()
        {
            if (instance != null)
            {
                IFormulaStepRepository repo = (IFormulaStepRepository)instance.ServiceProvider.GetService(typeof(IFormulaStepRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetNetsuiteRepository.
        /// </summary>
        /// <returns>The <see cref="INetsuiteFormulaRepository"/>.</returns>
        public static INetsuiteFormulaRepository GetNetsuiteRepository()
        {
            if (instance != null)
            {
                INetsuiteFormulaRepository repo = (INetsuiteFormulaRepository)instance.ServiceProvider.GetService(typeof(INetsuiteFormulaRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetFormulaRepository.
        /// </summary>
        /// <returns>The <see cref="IFormulaRepository"/>.</returns>
        public static IFormulaRepository GetFormulaRepository()
        {
            if (instance != null)
            {
                IFormulaRepository repo = (IFormulaRepository)instance.ServiceProvider.GetService(typeof(IFormulaRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// The GetNetsuiteFormulaStepRepository.
        /// </summary>
        /// <returns>The <see cref="INetsuiteFormulaStepRepository"/>.</returns>
        public static INetsuiteFormulaStepRepository GetNetsuiteFormulaStepRepository()
        {
            if (instance != null)
            {
                INetsuiteFormulaStepRepository repo = (INetsuiteFormulaStepRepository)instance.ServiceProvider.GetService(typeof(INetsuiteFormulaStepRepository));
                return repo;
            }
            return null;
        }

        /// <summary>
        /// Gets the SessionFactoryProvider.
        /// </summary>
        protected IFluentNhibernateLocalSessionFactoryProvider SessionFactoryProvider
        {
            get { return this.sessionFactoryProvider; }
        }

        /// <summary>
        /// Gets the SessionFactory.
        /// </summary>
        protected ISessionFactory SessionFactory
        {
            get { return SessionFactoryProvider.GetSessionFactory(); }
        }

        /// <summary>
        /// The OpenSession.
        /// </summary>
        /// <returns>The <see cref="ISession"/>.</returns>
        public static ISession OpenSession()
        {
            return instance.SessionFactoryProvider.GetSessionFactory().OpenSession();
        }

        /// <summary>
        /// The CurrentSession.
        /// </summary>
        /// <returns>The <see cref="ISession"/>.</returns>
        public static ISession CurrentSession()
        {
            return instance.SessionFactoryProvider.GetCurrentSession();
        }
    }
}
