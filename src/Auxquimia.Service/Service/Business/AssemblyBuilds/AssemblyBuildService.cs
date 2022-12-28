namespace Auxquimia.Service.Business.AssemblyBuilds
{
    using Auxquimia.Config;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Dto.Management.Metrics;
    using Auxquimia.Enums;
    using Auxquimia.Exceptions;
    using Auxquimia.Filters.Business.AssemblyBuilds;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Repository.Authentication;
    using Auxquimia.Repository.Business.Formulas;
    using Auxquimia.Repository.Management.Business.AssemblyBuilds;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Service.Authentication;
    using Auxquimia.Service.Business.Formulas;
    using Auxquimia.Service.Management.Factories;
    using Auxquimia.Service.Management.Metrics;
    using Auxquimia.Utils;
    using Auxquimia.Utils.FileStorage;
    using Auxquimia.Utils.FileStorage.Model;
    using Auxquimia.Utils.Opc;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using static Auxquimia.Utils.Constants;

    /// <summary>
    /// Defines the <see cref="AssemblyBuildService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class AssemblyBuildService : IAssemblyBuildService
    {
        /// <summary>
        /// Defines the assemblyBuildRepository.
        /// </summary>
        private readonly IAssemblyBuildRepository assemblyBuildRepository;

        /// <summary>
        /// Defines the factoryService.
        /// </summary>
        private readonly IFactoryService factoryService;

        /// <summary>
        /// Defines the reactorService.
        /// </summary>
        private readonly IReactorService reactorService;

        /// <summary>
        /// Defines the reactorRepository.
        /// </summary>
        private readonly IReactorRepository reactorRepository;

        /// <summary>
        /// Defines the userService.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Defines the userRepository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Defines the roleRepository.
        /// </summary>
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Defines the unitService.
        /// </summary>
        private readonly IUnitService unitService;

        /// <summary>
        /// Defines the netsuiteFormulaService.
        /// </summary>
        private readonly INetsuiteFormulaService netsuiteFormulaService;

        /// <summary>
        /// Defines the netsuiteFormulaStepService.
        /// </summary>
        private readonly INetsuiteFormulaStepService netsuiteFormulaStepService;

        /// <summary>
        /// Defines the formulaService.
        /// </summary>
        private readonly IFormulaService formulaService;

        /// <summary>
        /// Defines the formulaStepService.
        /// </summary>
        private readonly IFormulaStepService formulaStepService;

        /// <summary>
        /// Defines the formulaRepository.
        /// </summary>
        private readonly IFormulaRepository formulaRepository;

        /// <summary>
        /// Defines the formulaStepRepository.
        /// </summary>
        private readonly IFormulaStepRepository formulaStepRepository;

        /// <summary>
        /// Defines the netsuiteFormulaRepository.
        /// </summary>
        private readonly INetsuiteFormulaRepository netsuiteFormulaRepository;

        /// <summary>
        /// Defines the netsuiteFormulaStepRepository.
        /// </summary>
        private readonly INetsuiteFormulaStepRepository netsuiteFormulaStepRepository;

        /// <summary>
        /// Defines the contextConfigProvider.
        /// </summary>
        private readonly ContextConfigProvider contextConfigProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildService"/> class.
        /// </summary>
        /// <param name="assemblyBuildRepository">The assemblyBuildRepository<see cref="IAssemblyBuildRepository"/>.</param>
        /// <param name="contextConfigProvider">The contextConfigProvider<see cref="ContextConfigProvider"/>.</param>
        /// <param name="factoryService">The factoryService<see cref="IFactoryService"/>.</param>
        /// <param name="reactorService">The reactorService<see cref="IReactorService"/>.</param>
        /// <param name="serviceRepository">The serviceRepository<see cref="IReactorRepository"/>.</param>
        /// <param name="userService">The userService<see cref="IUserService"/>.</param>
        /// <param name="roleRepository">The roleRepository<see cref="IRoleRepository"/>.</param>
        /// <param name="userRepository">The userRepository<see cref="IUserRepository"/>.</param>
        /// <param name="netsuiteFormulaService">The netsuiteFormulaService<see cref="INetsuiteFormulaService"/>.</param>
        /// <param name="netsuiteFormulaStepService">The netsuiteFormulaStepService<see cref="INetsuiteFormulaStepService"/>.</param>
        /// <param name="formulaService">The formulaService<see cref="IFormulaService"/>.</param>
        /// <param name="formulaStepService">The formulaStepService<see cref="IFormulaStepService"/>.</param>
        /// <param name="netsuiteFormulaRepository">The netsuiteFormulaRepository<see cref="INetsuiteFormulaRepository"/>.</param>
        /// <param name="netsuiteFormulaStepRepository">The netsuiteFormulaStepRepository<see cref="INetsuiteFormulaStepRepository"/>.</param>
        /// <param name="formulaRepository">The formulaRepository<see cref="IFormulaRepository"/>.</param>
        /// <param name="formulaStepRepository">The formulaStepRepository<see cref="IFormulaStepRepository"/>.</param>
        /// <param name="unitService">The unitService<see cref="IUnitService"/>.</param>
        public AssemblyBuildService(IAssemblyBuildRepository assemblyBuildRepository,
            ContextConfigProvider contextConfigProvider, IFactoryService factoryService,
            IReactorService reactorService, IReactorRepository serviceRepository,
            IUserService userService,
            IRoleRepository roleRepository, IUserRepository userRepository,
            INetsuiteFormulaService netsuiteFormulaService,
            INetsuiteFormulaStepService netsuiteFormulaStepService,
            IFormulaService formulaService,
            IFormulaStepService formulaStepService,
            INetsuiteFormulaRepository netsuiteFormulaRepository,
            INetsuiteFormulaStepRepository netsuiteFormulaStepRepository,
            IFormulaRepository formulaRepository,
            IFormulaStepRepository formulaStepRepository,
            IUnitService unitService)
        {
            this.assemblyBuildRepository = assemblyBuildRepository;
            this.contextConfigProvider = contextConfigProvider;
            this.factoryService = factoryService;
            this.reactorService = reactorService;
            this.reactorRepository = serviceRepository;
            this.userService = userService;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.netsuiteFormulaService = netsuiteFormulaService;
            this.netsuiteFormulaStepService = netsuiteFormulaStepService;
            this.formulaRepository = formulaRepository;
            this.formulaStepRepository = formulaStepRepository;
            this.formulaService = formulaService;
            this.formulaStepService = formulaStepService;

            this.netsuiteFormulaRepository = netsuiteFormulaRepository;
            this.netsuiteFormulaStepRepository = netsuiteFormulaStepRepository;
            this.unitService = unitService;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        public async Task<IList<AssemblyBuildDto>> GetAllAsync()
        {
            var result = await assemblyBuildRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<AssemblyBuild>, IList<AssemblyBuildDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> GetAsync(Guid id)
        {
            AssemblyBuild result = await assemblyBuildRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
        }

        /// <summary>
        /// The GetCountWoByStatus.
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Dictionary{ABStatus, int}}"/>.</returns>
        public async Task<Dictionary<ABStatus, int>> GetCountWoByStatus(string userName, Guid factoryId)
        {
            User user = await this.userRepository.FindByUsernameAsync(userName);

            if (user == null)
            {
                return null;
            }


            Dictionary<ABStatus, int> result = new Dictionary<ABStatus, int>();
            //Operator restrictions
            BaseAssemblyBuildSearchFilter filter = await FilterWithOperatorRestrictions(user, new BaseAssemblyBuildSearchFilter());
            if (factoryId != default(Guid))
            {
                filter.FactoryId = factoryId;
            }

            filter.Status = ABStatus.PENDING;
            result.Add(ABStatus.PENDING, assemblyBuildRepository.GetTotalWoByStatus(filter));
            filter.Status = ABStatus.WAITING;
            result.Add(ABStatus.WAITING, assemblyBuildRepository.GetTotalWoByStatus(filter));
            filter.Status = ABStatus.PROGRESS;
            result.Add(ABStatus.PROGRESS, assemblyBuildRepository.GetTotalWoByStatus(filter));
            filter.Status = ABStatus.FINISHED;
            result.Add(ABStatus.FINISHED, assemblyBuildRepository.GetTotalWoByStatus(filter));

            return result;
        }

        /// <summary>
        /// The FilterWithOperatorRestrictions.
        /// </summary>
        /// <param name="currentOperator">The currentOperator<see cref="User"/>.</param>
        /// <param name="myFilter">The myFilter<see cref="BaseAssemblyBuildSearchFilter"/>.</param>
        /// <returns>The <see cref="Task{BaseAssemblyBuildSearchFilter}"/>.</returns>
        private async Task<BaseAssemblyBuildSearchFilter> FilterWithOperatorRestrictions(User currentOperator, BaseAssemblyBuildSearchFilter myFilter)
        {
            Role adminRole = await this.roleRepository.getByName(Roles.ADMINISTRATOR);
            Role managerRole = await this.roleRepository.getByName(Roles.MANAGER);
            Role userRole = await this.roleRepository.getByName(Roles.USER);

            List<Role> roles = currentOperator.Roles.Select(x => x.Role).ToList();
            Guid defaultFactoryFilter = myFilter.FactoryId;
            if (roles.Contains(userRole))
            {
                myFilter.OperatorAssignedId = currentOperator.Id;
                myFilter.FactoryId = default(Guid);

            }
            if (roles.Contains(managerRole))
            {
                myFilter.OperatorAssignedId = default(Guid);
                myFilter.FactoryId = currentOperator.Factory.Id;
            }
            if (roles.Contains(adminRole))
            {

                myFilter.OperatorAssignedId = default(Guid);
                myFilter.FactoryId = default(Guid);
            }

            //Restore facotry filter
            if (defaultFactoryFilter != default(Guid))
            {
                myFilter.FactoryId = defaultFactoryFilter;
            }
            return myFilter;
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuildDto}}"/>.</returns>
        public async Task<Page<AssemblyBuildDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await assemblyBuildRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<AssemblyBuild>, Page<AssemblyBuildDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuildListDto}}"/>.</returns>
        public async Task<Page<AssemblyBuildListDto>> PaginatedAsync(FindRequestDto<BaseAssemblyBuildSearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestDto<BaseAssemblyBuildSearchFilter>, FindRequestImpl<BaseAssemblyBuildSearchFilter>>();
            var result = await assemblyBuildRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<AssemblyBuild>, Page<AssemblyBuildListDto>>();
        }

        /// <summary>
        /// The SearchByRole.
        /// </summary>
        /// <param name="operatorName">The operatorName<see cref="string"/>.</param>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuildListDto}}"/>.</returns>
        public async Task<Page<AssemblyBuildListDto>> SearchByRole(string operatorName, FindRequestDto<BaseAssemblyBuildSearchFilter> filter)
        {
            User user = await this.userRepository.FindByUsernameAsync(operatorName);

            if (user == null)
            {
                return null;
            }
            BaseAssemblyBuildSearchFilter nfilter = await FilterWithOperatorRestrictions(user, filter.Filter);
            filter.Filter = nfilter;
            var findRequest = filter.PerformMapping<FindRequestDto<BaseAssemblyBuildSearchFilter>, FindRequestImpl<BaseAssemblyBuildSearchFilter>>();
            var result = await assemblyBuildRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<AssemblyBuild>, Page<AssemblyBuildListDto>>();
        }

        /// <summary>
        /// The GetByMultipleStatus.
        /// </summary>
        /// <param name="operatorName">The operatorName<see cref="string"/>.</param>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuildListDto}}"/>.</returns>
        public async Task<Page<AssemblyBuildListDto>> GetByMultipleStatus(string operatorName, FindRequestDto<BaseAssemblyBuildSearchFilter> filter)
        {
            if (filter == null) { return default(Page<AssemblyBuildListDto>); }
            User user = await this.userRepository.FindByUsernameAsync(operatorName);

            if (user == null)
            {
                return default(Page<AssemblyBuildListDto>);
            }
            //Operator restrictions
            filter.Filter = await FilterWithOperatorRestrictions(user, filter.Filter);
            var findRequest = filter.PerformMapping<FindRequestDto<BaseAssemblyBuildSearchFilter>, FindRequestImpl<BaseAssemblyBuildSearchFilter>>();
            var result = await assemblyBuildRepository.GetByMultipleStatus(findRequest).ConfigureAwait(false);

            return result.PerformMapping<Page<AssemblyBuild>, Page<AssemblyBuildListDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<AssemblyBuildDto> SaveAsync(AssemblyBuildDto entity)
        {
            AssemblyBuild assemblyBuild = entity.PerformMapping<AssemblyBuildDto, AssemblyBuild>();

            if (assemblyBuild.Status == null || assemblyBuild.Status == ABStatus.PENDING)
            {

                assemblyBuild.Status = ABStatus.PENDING;

                if (entity.Formula != null)
                {
                    AssemblyBuild resultAssembly = await assemblyBuildRepository.SaveAsync(assemblyBuild).ConfigureAwait(false); //Save Assembly
                    resultAssembly.Formula.AssemblyBuild = resultAssembly;
                    await this.formulaRepository.UpdateAsync(resultAssembly.Formula);
                }
                else if (entity.NetsuiteFormula != null)
                {
                    AssemblyBuild editedAssembly = assemblyBuild;
                    editedAssembly.NetsuiteFormula = null;
                    AssemblyBuild resultAssembly = await assemblyBuildRepository.SaveAsync(editedAssembly).ConfigureAwait(false); //Save Assembly

                    NetsuiteFormula formula = await HandleNetsuiteFormula(entity, resultAssembly);
                    if (formula != null)
                    {
                        formula.AssemblyBuild = resultAssembly;
                        await netsuiteFormulaRepository.UpdateAsync(formula).ConfigureAwait(false);
                        resultAssembly.NetsuiteFormula = formula;
                        await assemblyBuildRepository.UpdateAsync(resultAssembly).ConfigureAwait(false);
                    }

                }
                return entity;
            }

            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{AssemblyBuildDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<AssemblyBuildDto> entity)
        {
            return assemblyBuildRepository.SaveAsync(entity.PerformMapping<IList<AssemblyBuildDto>, IList<AssemblyBuild>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<AssemblyBuildDto> UpdateAsync(AssemblyBuildDto entity)
        {
            AssemblyBuild storedAssemblyBuild = await assemblyBuildRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            AssemblyBuild assemblyBuild = entity.PerformMapping(storedAssemblyBuild);
            AssemblyBuild result = storedAssemblyBuild;
            if (result.Formula != null)
            {
                result.Formula.AssemblyBuild = result;
            }
            if (assemblyBuild.Status == ABStatus.PENDING)
            {
                result = await assemblyBuildRepository.UpdateAsync(assemblyBuild).ConfigureAwait(false);
            }
            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The LoadFromFtp.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<IList<AssemblyBuildDto>> LoadFromFtp()
        {
            string ftpServer = contextConfigProvider.FtpServer;
            string ftpUsername = contextConfigProvider.FtpUsername;
            string ftpPassword = contextConfigProvider.FtpPassword;
            string filePath = contextConfigProvider.FtpFilePath_Read;
            FactoryListDto factory = await this.factoryService.FindMainFactoryList();

            IList<AssemblyBuildDto> assemblies = new List<AssemblyBuildDto>();
            try
            {
                using (Stream fileStream = new FtpHelper(ftpServer, ftpUsername, ftpPassword, filePath).getFileStreamToRead())
                {
                    using (FileHelper fileHelper = FileHelper.Reader(fileStream))
                    {
                        fileHelper.SkipLine(); //Skip headers
                        Dictionary<string, object> step;
                        string readingAssembly = Strings.EMPTY_STRING;
                        AssemblyBuildDto actualAssembly = null;
                        NetsuiteFormulaStepDto actualStep = null;
                        int previousStepNumber = 0, readingStepNumber = 0;
                        while (fileHelper.Read())
                        {
                            try
                            {
                                step = fileHelper.ReadStepFromCSV();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                throw new CustomException(Constants.Ftp.Errors.PARSING_ERROR);
                            }

                            //Check if theres steps to read.
                            if (step[Constants.Ftp.ASSEMBLY_NUMBER] != null || (string)step[Constants.Ftp.ASSEMBLY_NUMBER] != default(string))
                            {
                                readingAssembly = (string)step[Constants.Ftp.ASSEMBLY_NUMBER];
                            }
                            else
                            {
                                break;
                            }

                            //Initialize Assembly for the first time or check if its different assembly
                            if (actualAssembly == null || !actualAssembly.AssemblyBuildNumber.Equals(readingAssembly))
                            {
                                if (actualAssembly != null)
                                {

                                    //Save previous assembly

                                    //Adding to returning list
                                    assemblies = await SaveReadingAssembly(assemblies, actualAssembly);
                                }
                                actualAssembly = new AssemblyBuildDto()
                                {
                                    AssemblyBuildNumber = readingAssembly,
                                    NetsuiteFormula = new NetsuiteFormulaDto(),
                                    Factory = factory,
                                    Status = ABStatus.PENDING
                                };
                                actualAssembly.NetsuiteFormula.AssemblyBuild = actualAssembly;
                            }
                            //reading step
                            readingStepNumber = (int)step[Constants.Ftp.STEP_NUMBER];
                            if (readingStepNumber == 0)
                            {
                                actualAssembly.NetsuiteFormula.BlenderFinalTime = (Int16)step[Constants.Ftp.STEP_BLENDING_TIME];
                                actualAssembly.NetsuiteFormula.BlenderFinalPercentaje = Convert.ToInt16((string)step[Constants.Ftp.STEP_BLENDER_SPEED2]);
                                actualAssembly.NetsuiteFormula.Name = (string)step[Constants.Ftp.STEP_ITEM_DISPLAY_NAME];
                                //Reading assembly units
                                string unitName = (string)step[Constants.Ftp.STEP_ITEM_UNITS];
                                UnitDto formulaUnit = null;
                                if (StringUtils.HasText(unitName))
                                {
                                    formulaUnit = await unitService.FindByName(unitName);
                                }
                                actualAssembly.NetsuiteFormula.Units = formulaUnit;
                                actualAssembly.NetsuiteFormula.InventoryLot = (string)step[Constants.Ftp.STEP_INVENTORY_LOT];
                                actualAssembly.NetsuiteFormula.InventoryDetailId = (long)step[Constants.Ftp.STEP_INVENTORY_DETAIL];
                                actualAssembly.AssemblyCode = (string)step[Constants.Ftp.STEP_ITEM_CODE];
                                actualAssembly.NetsuiteFormula.BatchNumber = (long)step[Constants.Ftp.STEP_BATCH_NUMBER];
                            }
                            else
                            {

                                //Different step


                                //Creating assembly steps
                                actualAssembly.Date = (long)step[Constants.Ftp.ASSEMBLY_LOAD_DATE];
                                if ((string)step[Constants.Ftp.ASSEMBLY_BLENDER] != default(string) && actualAssembly.Blender == null)
                                {
                                    string blenderCode = (string)step[Constants.Ftp.ASSEMBLY_BLENDER];
                                    ReactorDto reactor = await this.reactorService.FindByCodeAsync(blenderCode);
                                    if (reactor != null)
                                    {
                                        actualAssembly.Blender = reactor;
                                    }
                                }
                                UserDto stepOperator = null;
                                if ((int)step[Constants.Ftp.STEP_OPERATOR] != default(int))
                                {
                                    int operatorCode = (int)step[Constants.Ftp.STEP_OPERATOR];
                                    stepOperator = await this.userService.FindByCode(operatorCode, factory.Id);
                                }

                                //Reading step units
                                string unitName = (string)step[Constants.Ftp.STEP_ITEM_UNITS];
                                UnitDto stepUnit = null;
                                if (StringUtils.HasText(unitName))
                                {
                                    stepUnit = await unitService.FindByName(unitName);
                                    if(stepUnit == null)
                                    {
                                        stepUnit = await unitService.FindByCode(unitName);
                                    }
                                }
                                //New step
                                actualStep = new NetsuiteFormulaStepDto()
                                {
                                    ItemCode = (string)step[Constants.Ftp.STEP_ITEM_CODE],
                                    ItemName = (string)step[Constants.Ftp.STEP_ITEM_DISPLAY_NAME],
                                    QtyRequired = (decimal)step[Constants.Ftp.STEP_ITEM_QTY_REQUIRED] < 0 ? (decimal)step[Constants.Ftp.STEP_ITEM_QTY_REQUIRED] * -1 : (decimal)step[Constants.Ftp.STEP_ITEM_QTY_REQUIRED],
                                    Units = stepUnit,
                                    AdditionSequence = readingStepNumber,
                                    InventoryDetailId = (long)step[Constants.Ftp.STEP_INVENTORY_DETAIL],
                                    //InventoryLot = (long)step[Constants.Ftp.STEP_INVENTORY_LOT],
                                    Operator = stepOperator,
                                    StirringRate1 = (string)step[Constants.Ftp.STEP_BLENDER_SPEED1],
                                    StirringTime = (Int16)step[Constants.Ftp.STEP_BLENDING_TIME],
                                    BatchNumber = (long)step[Constants.Ftp.STEP_BATCH_NUMBER],
                                    Temperature = (string)step[Constants.Ftp.STEP_TEMPERATURE],
                                    Written = false
                                };
                                actualStep.InventoryLot = StringUtils.HasText((string)step[Constants.Ftp.STEP_INVENTORY_LOT]) ? (string)step[Constants.Ftp.STEP_INVENTORY_LOT] : default(string);
                                actualStep.Formula = actualAssembly.NetsuiteFormula;
                                actualAssembly.NetsuiteFormula.Steps.Add(actualStep);
                            }
                        }

                        //Checks if file ends and last assembly was not saved
                        if (!assemblies.Contains(actualAssembly))
                        {
                            assemblies = await SaveReadingAssembly(assemblies, actualAssembly);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DEBUG EXCEPTION] Exception while reading from ftp {e.Message}");
                throw new CustomException(e.Message);
            }
            return assemblies;
        }

        /// <summary>
        /// The SaveReadingAssembly.
        /// </summary>
        /// <param name="actualList">The actualList<see cref="IList{AssemblyBuildDto}"/>.</param>
        /// <param name="assemblyToSave">The assemblyToSave<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        private async Task<IList<AssemblyBuildDto>> SaveReadingAssembly(IList<AssemblyBuildDto> actualList, AssemblyBuildDto assemblyToSave)
        {
            if (actualList == null)
            {
                actualList = new List<AssemblyBuildDto>();
            }
            await this.SaveAsync(assemblyToSave);

            //Adding to returning list
            actualList.Add(assemblyToSave);
            return actualList;
        }

        /// <summary>
        /// The SendAssemblyToWaitingQueue.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<AssemblyBuildDto> SendAssemblyToWaitingQueue(Guid assemblyId)
        {

            if (assemblyId == default(Guid))
            {
                throw new CustomException(Constants.Opc.Errors.ILLEGAL_STATE_ASSEMBLY);
            }

            AssemblyBuild assemblyEntity = await assemblyBuildRepository.GetToWaitingQueueAsync(assemblyId).ConfigureAwait(false);
            if (assemblyEntity != null && assemblyEntity.Status == ABStatus.PENDING && assemblyEntity.Operator != null && (assemblyEntity.Formula != null || assemblyEntity.NetsuiteFormula != null) && assemblyEntity.Deadline != default(long))
            {

                assemblyEntity.Status = ABStatus.WAITING;
                assemblyEntity.ToProductionDate = DateHelper.GetTodayUnixTimeMilliseconds();
                AssemblyBuild result = await assemblyBuildRepository.UpdateAsync(assemblyEntity);
                AssemblyBuildDto assemblyProduction = result.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
                return assemblyProduction;
            }
            else
            {
                throw new CustomException(Constants.Opc.Errors.ILLEGAL_STATE_ASSEMBLY);
            }
        }

        /// <summary>
        /// The SendAssemblyBuildToOpc.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> SendAssemblyBuildToOpc(Guid assemblyId)
        {
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetToProductionAsync(assemblyId);

            if (assembly == null)
            {
                throw new CustomException(Constants.Opc.Errors.ILLEGAL_STATE_ASSEMBLY);
            }
            string serverURL = assembly.Factory.OpcServer;
            return OpcManager.Get(serverURL).WriteAssembly(assembly.PerformMapping<AssemblyBuild, AssemblyBuildDto>());
        }

        /// <summary>
        /// The ReadAssemblyBuildFromOpc.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Dictionary{string, object}}"/>.</returns>
        public async Task<Dictionary<string, object>> ReadAssemblyBuildFromOpc(Guid assemblyId)
        {
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetAsync(assemblyId);
            return OpcManager.Get(assembly.Factory.OpcServer).ReadOpcStepData();
        }

        /// <summary>
        /// The UpdateOperatorAsync.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="operatorId">The operatorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<AssemblyBuildDto> UpdateOperatorAsync(Guid assemblyId, Guid operatorId)
        {
            AssemblyBuild assembly = await assemblyBuildRepository.GetAsync(assemblyId).ConfigureAwait(false);
            assembly.Operator = await userRepository.GetAsync(operatorId).ConfigureAwait(false);
            assembly = await assemblyBuildRepository.UpdateAsync(assembly).ConfigureAwait(false);
            return assembly.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
        }

        /// <summary>
        /// The HandleNetsuiteFormula.
        /// </summary>
        /// <param name="source">The source<see cref="AssemblyBuildDto"/>.</param>
        /// <param name="destination">The destination<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        [Transaction(ReadOnly = false)]
        private async Task<NetsuiteFormula> HandleNetsuiteFormula(AssemblyBuildDto source, AssemblyBuild destination)
        {
            NetsuiteFormula actualFormula = source.NetsuiteFormula.PerformMapping<NetsuiteFormulaDto, NetsuiteFormula>();
            NetsuiteFormula savedFormula = destination.NetsuiteFormula;
            NetsuiteFormula updatedFormula = null;
            if (actualFormula == null)
            {
                return null;
            }
            if (savedFormula == null)
            {
                //Nueva formula
                NetsuiteFormula resultFormula = null;
                resultFormula = await this.netsuiteFormulaRepository.SaveAsync(actualFormula);
                if (resultFormula != null)
                {
                    await HandleNetsuiteSteps(source.NetsuiteFormula, resultFormula);
                }

                updatedFormula = resultFormula;

            }
            else if (savedFormula != actualFormula)
            {
                //Actualizar formula
                NetsuiteFormula resultFormula = null;
                resultFormula = await this.netsuiteFormulaRepository.UpdateAsync(actualFormula);
                if (resultFormula != null)
                {
                    await HandleNetsuiteSteps(source.NetsuiteFormula, resultFormula);
                }
                updatedFormula = resultFormula;
            }
            return updatedFormula;
        }

        /// <summary>
        /// The HandleNetsuiteSteps.
        /// </summary>
        /// <param name="source">The source<see cref="NetsuiteFormulaDto"/>.</param>
        /// <param name="destination">The destination<see cref="NetsuiteFormula"/>.</param>
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaStep}}"/>.</returns>
        [Transaction(ReadOnly = false)]
        private async Task<IList<NetsuiteFormulaStep>> HandleNetsuiteSteps(NetsuiteFormulaDto source, NetsuiteFormula destination)
        {
            IList<NetsuiteFormulaStep> steps = source.Steps.PerformMapping<IList<NetsuiteFormulaStepDto>, IList<NetsuiteFormulaStep>>(); //Se pierden los lotes
            steps = steps ?? new List<NetsuiteFormulaStep>();
            IList<NetsuiteFormulaStep> actualSteps = destination.Steps ?? new List<NetsuiteFormulaStep>();
            // Delete no longer Steps
            IList<NetsuiteFormulaStep> removedSteps = actualSteps;
            if (removedSteps.Any())
            {
                removedSteps = removedSteps.Except(steps).ToList();
                NetsuiteFormulaDto destinationDto = destination.PerformMapping<NetsuiteFormula, NetsuiteFormulaDto>();
                foreach (NetsuiteFormulaStep step in removedSteps)
                {
                    NetsuiteFormulaStep toDelete = await netsuiteFormulaStepRepository.GetAsync(step.Id).ConfigureAwait(false);
                    if (toDelete != null)
                    {
                        await netsuiteFormulaStepRepository.DeleteAsync(toDelete).ConfigureAwait(false);
                        actualSteps.Remove(toDelete);
                    }
                }
            }

            // Edited steps are managed at the moment so it is not necessary take care about them here

            IList<NetsuiteFormulaStep> newSteps = steps.Where(x => x.Id == default(Guid)).ToList();
            IList<NetsuiteFormulaStepDto> newStepsDto = source.Steps.Where(x => x.Id == null).ToList();
            // Add new Steps
            if (newStepsDto.Any())
            {
                foreach (NetsuiteFormulaStepDto step in newStepsDto)
                {
                    NetsuiteFormulaStep stepEntity = step.PerformMapping<NetsuiteFormulaStepDto, NetsuiteFormulaStep>(); //se pierden los lotes
                    stepEntity.Formula = destination;
                    // Save on BBDD
                    NetsuiteFormulaStep stepSaved = await netsuiteFormulaStepRepository.SaveAsync(stepEntity).ConfigureAwait(false);
                    actualSteps.Add(stepEntity);
                }
            }
            //destination.Steps = actualSteps;
            return actualSteps;
        }

        /// <summary>
        /// The UpdateAssemblyFromSatellite.
        /// </summary>
        /// <param name="assemblyToUpdate">The assemblyToUpdate<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<bool> UpdateAssemblyFromSatellite(AssemblyBuildDto assemblyToUpdate)
        {
            bool result = false;

            //if (assemblyToUpdate == null)
            //    return result;

            //AssemblyBuildDto actualAssembly = null;
            //if (assemblyToUpdate.Id != null && assemblyToUpdate.Id != default(string))
            //{
            //    actualAssembly = await GetAsync(assemblyToUpdate.Id.PerformMapping<string, Guid>());
            //}else if(assemblyToUpdate.AssemblyBuildNumber != null && assemblyToUpdate.AssemblyBuildNumber != default(string))
            //{
            //    //Buscar por codigo.
            //}

            //if (actualAssembly == null)
            //    return result;


            return result;
        }

        /// <summary>
        /// The GetToWaitingQueueAsync.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> GetToWaitingQueueAsync(Guid assemblyBuildId)
        {
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetToWaitingQueueAsync(assemblyBuildId);
            return assembly.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
        }

        /// <summary>
        /// The GetToProductionAsync.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> GetToProductionAsync(Guid assemblyBuildId)
        {
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetToProductionAsync(assemblyBuildId);
            AssemblyBuildDto assemblyCloned = assembly.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
            AssemblyBuildDto assemblySelected = SelectUniqueLotForSteps(assemblyCloned);
            return assemblySelected;
        }

        /// <summary>
        /// The GetSync.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="AssemblyBuildDto"/>.</returns>
        public AssemblyBuildDto GetSync(Guid assemblyId)
        {
            AssemblyBuild result = assemblyBuildRepository.GetSync(assemblyId);
            return result.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
        }

        /// <summary>
        /// The GenerateRecords.
        /// </summary>
        /// <param name="assembly">The assembly<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="IList{AssemblyRecord}"/>.</returns>
        private IList<AssemblyRecord> GenerateRecords(AssemblyBuild assembly)
        {
            IList<AssemblyRecord> records = new List<AssemblyRecord>();
            if (assembly.NetsuiteFormula != null)
            {
                records = GenerateFormulaRecords(assembly);
            }
            return records;
        }

        /// <summary>
        /// The GenerateFormulaRecords.
        /// </summary>
        /// <param name="assembly">The assembly<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="IList{AssemblyRecord}"/>.</returns>
        private IList<AssemblyRecord> GenerateFormulaRecords(AssemblyBuild assembly)
        {
            IList<AssemblyRecord> records = new List<AssemblyRecord>();
            //string noneWord = "None";
            //string unknownWord = "Unknown";
            string DateFormat = "dd/MM/yyyy";
            if (assembly.NetsuiteFormula != null)
            {
                AssemblyRecord record;
                decimal weightSum = 0;
                decimal realWeightSum = 0;
                long startDateAux = 0;
                long endDateAux = 0;
                long firstDate = 0;
                long lastDate = 0;
                IList<NetsuiteFormulaStep> orderedSteps = assembly.NetsuiteFormula.Steps.OrderBy(x => x.AdditionSequence).ToList();
                foreach (NetsuiteFormulaStep step in orderedSteps)
                {
                    record = new AssemblyRecord();

                    record.AssemblyNumber = assembly.AssemblyBuildNumber;
                    record.Date = DateHelper.DateTimeToString(DateHelper.UnixTimeMillisecondsToDateTime(assembly.Date), DateFormat);
                    record.Item = step.ItemCode + "";
                    record.DisplayName = step.ItemName;
                    record.Quantity = (int)step.QtyRequired * -1 + "";
                    record.Units = step.Units.Name;
                    record.ComponentSequence = step.AdditionSequence + "";
                    record.ComponentInventoryDetaylId = step.InventoryDetailId != default(long) ? step.InventoryDetailId + "" : null;
                    record.ComponentInventoryLot = step.InventoryLot;
                    record.BlenderNumber = assembly.Blender.Code + "";
                    record.OperatorNo = assembly.Operator.Code + "";
                    record.VelocidadAgitacion1 = step.StirringRate1;
                    record.AgitationTime = step.StirringTime + "";
                    record.Batchnumber = step.BatchNumber + "";
                    record.Temperature = step.Temperature != null ? step.Temperature : null;
                    record.RealWeight = (int)step.RealQtyProduced * -1 + "";
                    startDateAux = step.StartDate;
                    endDateAux = step.EndDate;
                    record.StartDate = startDateAux == default(long) ? null : DateHelper.DateTimeToString(DateHelper.UnixTimeMillisecondsToDateTime(startDateAux), DateFormat);
                    record.EndDate = endDateAux == default(long) ? null : DateHelper.DateTimeToString(DateHelper.UnixTimeMillisecondsToDateTime(endDateAux), DateFormat);
                    weightSum += (int)step.QtyRequired;
                    realWeightSum += (int)step.RealQtyProduced;
                    if (firstDate == default(long))
                    {
                        firstDate = startDateAux;
                    }
                    lastDate = endDateAux;
                    records.Add(record);
                }
                record = new AssemblyRecord();
                record.AssemblyNumber = assembly.AssemblyBuildNumber;
                record.Date = DateHelper.DateTimeToString(DateHelper.UnixTimeMillisecondsToDateTime(assembly.Date), DateFormat);
                record.Item = assembly.AssemblyCode != null ? assembly.AssemblyCode + "" : null;
                record.DisplayName = assembly.NetsuiteFormula.Name;
                record.Quantity = weightSum + "";
                record.Units = assembly.NetsuiteFormula.Units.Name;
                record.ComponentSequence = "0";
                record.ComponentInventoryDetaylId = assembly.NetsuiteFormula.InventoryDetailId != default(long) ? assembly.NetsuiteFormula.InventoryDetailId + "" : null;
                record.ComponentInventoryLot = assembly.NetsuiteFormula.InventoryLot;
                record.BlenderNumber = assembly.Blender.Code + "";
                record.OperatorNo = assembly.Operator.Code + "";
                record.VelocidadAgitacion1 = "0";
                record.VelocidadAgitacion2 = assembly.NetsuiteFormula.BlenderFinalPercentaje + "";
                record.AgitationTime = assembly.NetsuiteFormula.BlenderFinalTime + "";
                record.Batchnumber = assembly.NetsuiteFormula.BatchNumber != default(long) ? assembly.NetsuiteFormula.BatchNumber + "" : null;
                record.Temperature = null;
                record.RealWeight = "0";
                startDateAux = firstDate;
                endDateAux = lastDate;
                record.StartDate = startDateAux == default(long) ? null : DateHelper.DateTimeToString(DateHelper.UnixTimeMillisecondsToDateTime(startDateAux), DateFormat);
                record.EndDate = endDateAux == default(long) ? null : DateHelper.DateTimeToString(DateHelper.UnixTimeMillisecondsToDateTime(endDateAux), DateFormat);
                records.Add(record);

                records = records.OrderBy(r => r.ComponentSequence).ToList();
            }
            return records;
        }

        /// <summary>
        /// The SendToNetsuite.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<IList<AssemblyBuildListDto>> SendToNetsuite()
        {
            IList<AssemblyBuild> assembliesToWrite = await this.assemblyBuildRepository.GetAssembliesToNetsuite().ConfigureAwait(false);
            IList<AssemblyRecord> records = new List<AssemblyRecord>();
            foreach (AssemblyBuild assembly in assembliesToWrite)
            {
                IList<AssemblyRecord> recordsFromAssembly = GenerateRecords(assembly);
                foreach (AssemblyRecord record in recordsFromAssembly)
                {
                    records.Add(record);
                }
                assembly.NetsuiteWritted = true;
                await this.assemblyBuildRepository.UpdateAsync(assembly).ConfigureAwait(true);
            }
            if (records.Count > 0)
            {
                string ftpServer = contextConfigProvider.FtpServer;
                string ftpUsername = contextConfigProvider.FtpUsername;
                string ftpPassword = contextConfigProvider.FtpPassword;
                string filePath = contextConfigProvider.FtpFilePath_Write;
                using (Stream fileStream = new FtpHelper(ftpServer, ftpUsername, ftpPassword, filePath).getFileStreamToWrite())
                {
                    using (FileHelper fileHelper = FileHelper.Writer(fileStream))
                    {
                        fileHelper.Write(records);
                    }
                }
            }
            return assembliesToWrite.PerformMapping<IList<AssemblyBuild>, IList<AssemblyBuildListDto>>();
        }

        /// <summary>
        /// The FindAssembliesWaitingToProduction.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        public async Task<IList<AssemblyBuildDto>> FindAssembliesWaitingToProduction(Guid reactorId)
        {
            IList<AssemblyBuild> assemblies = await this.assemblyBuildRepository.FindAssembliesWaitingToProduction(reactorId);
            IList<AssemblyBuildDto> assmebliesWaiting = assemblies.PerformMapping<IList<AssemblyBuild>, IList<AssemblyBuildDto>>();
            return assmebliesWaiting;
        }

        /// <summary>
        /// The MarkStepsAsWritten.
        /// </summary>
        /// <param name="assemblyWritten">The assemblyWritten<see cref="AssemblyBuildDto"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<AssemblyBuildDto> MarkStepsAsWritten(AssemblyBuildDto assemblyWritten, ISession session = null)
        {
            if (assemblyWritten == null)
            {
                return null;
            }
            if (assemblyWritten.Formula != null)
            {
                IList<FormulaStepDto> steps = assemblyWritten.Formula.Steps;
                foreach (FormulaStepDto step in steps)
                {
                    if (!step.Written)
                    {
                        Guid stepId = step.Id.PerformMapping<string, Guid>();
                        await this.formulaStepService.MarkStepAsWritted(stepId, session);
                    }

                }
            }
            else if (assemblyWritten.NetsuiteFormula != null)
            {
                IList<NetsuiteFormulaStepDto> steps = assemblyWritten.NetsuiteFormula.Steps;
                foreach (NetsuiteFormulaStepDto step in steps)
                {
                    Guid stepId = step.Id.PerformMapping<string, Guid>();
                    await this.netsuiteFormulaStepService.MarkStepAsWritted(stepId, session);
                }
            }
            else
            {
                return null;
            }
            return assemblyWritten;
        }

        /// <summary>
        /// The GetWithNewLotForStep.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="stepRequired">The stepRequired<see cref="int"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> GetWithNewLotForStep(Guid assemblyId, int stepRequired, ISession session = null)
        {
            AssemblyBuild assembly = null;
            if (session != null)
            {
                assembly = await this.assemblyBuildRepository.GetAsyncWithSession(session, assemblyId);
            }
            else
            {
                assembly = await this.assemblyBuildRepository.GetAsync(assemblyId);
            }
            AssemblyBuildDto assemblyDto = assembly.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
            AssemblyBuildDto assemblyFilled = SelectUniqueLotForSteps(assemblyDto, stepRequired);
            return assemblyFilled;
        }

        /// <summary>
        /// The SelectUniqueLotForSteps.
        /// </summary>
        /// <param name="assemblyBuild">The assemblyBuild<see cref="AssemblyBuild"/>.</param>
        /// <param name="stepLot">The stepLot<see cref="int"/>.</param>
        /// <returns>The <see cref="AssemblyBuild"/>.</returns>
        private AssemblyBuildDto SelectUniqueLotForSteps(AssemblyBuildDto assemblyBuild, int stepLot = 0)
        {
            if (assemblyBuild == null)
            {
                return null;
            }
            if (assemblyBuild.Formula != null)
            {
                IList<FormulaStepDto> steps = new List<FormulaStepDto>();
                int maxSteps = 0;
                if (assemblyBuild.Formula.Steps.Count > 0)
                {
                    maxSteps = assemblyBuild.Formula.Steps.GroupBy(x => x.Step).Count();
                }
                for (int i = 1; i <= maxSteps; i++)
                {
                    IList<FormulaStepDto> stepstWritten = assemblyBuild.Formula.Steps.Where(s => s.Written && s.Step == i).ToList();
                    FormulaStepDto myStep = null;
                    if (stepstWritten.Count > 0)
                    {
                        if (stepLot >= 0 && stepLot <= maxSteps && i == stepLot) //Solicitud de lote
                        {
                            IList<FormulaStepDto> stepstNotWritten = assemblyBuild.Formula.Steps.Where(s => !s.Written && s.Step == i).ToList();
                            if (stepstNotWritten.Count > 0)
                            {
                                myStep = stepstNotWritten.First();
                            }
                        }

                        if (myStep == null)
                        {
                            myStep = stepstWritten.Last();
                        }
                        steps.Add(myStep);
                    }
                    else
                    {
                        IList<FormulaStepDto> stepstNotWritten = assemblyBuild.Formula.Steps.Where(s => !s.Written && s.Step == i).ToList();
                        if (stepstNotWritten.Count > 0)
                        {
                            myStep = stepstNotWritten.First();
                            steps.Add(myStep);
                        }
                        else
                        {
                            IList<FormulaStepDto> existingSteps = assemblyBuild.Formula.Steps.Where(s => s.Step == i).ToList();
                            if (existingSteps.Count > 0)
                            {
                                steps.Add(existingSteps.First());
                            }
                        }
                    }
                }
                assemblyBuild.Formula.Steps = steps;
            }
            else if (assemblyBuild.NetsuiteFormula != null)
            {
                List<NetsuiteFormulaStepDto> steps = new List<NetsuiteFormulaStepDto>();
                int maxSteps = 0;
                if (assemblyBuild.NetsuiteFormula.Steps.Count > 0)
                {
                    maxSteps = assemblyBuild.NetsuiteFormula.Steps.GroupBy(x => x.AdditionSequence).Count();
                }
                for (int i = 1; i <= maxSteps; i++)
                {
                    //VIEJO
                    IList<NetsuiteFormulaStepDto> stepstWritten = assemblyBuild.NetsuiteFormula.Steps.Where(s => s.Written && s.AdditionSequence == i).ToList();
                    NetsuiteFormulaStepDto myStep = null;
                    if (stepstWritten.Count > 0)
                    {
                        if (stepLot >= 0 && stepLot <= maxSteps && i == stepLot) //Solicitud de lote
                        {
                            IList<NetsuiteFormulaStepDto> stepstNotWritten = assemblyBuild.NetsuiteFormula.Steps.Where(s => !s.Written && s.AdditionSequence == i).ToList();
                            myStep = stepstNotWritten.First();
                        }

                        if (myStep == null)
                        {
                            myStep = stepstWritten.Last();
                        }
                        steps.Add(myStep);
                    }
                    else
                    {
                        IList<NetsuiteFormulaStepDto> stepstNotWritten = assemblyBuild.NetsuiteFormula.Steps.Where(s => !s.Written && s.AdditionSequence == i).ToList();
                        if (stepstNotWritten.Count > 0)
                        {
                            myStep = stepstNotWritten.First();
                            steps.Add(myStep);
                        }
                        else
                        {
                            IList<NetsuiteFormulaStepDto> existingSteps = assemblyBuild.NetsuiteFormula.Steps.Where(s => s.AdditionSequence == i).ToList();
                            if (existingSteps.Count > 0)
                            {
                                steps.Add(existingSteps.First());
                            }
                        }
                    }
                }
                assemblyBuild.NetsuiteFormula.Steps = steps;
            }
            return assemblyBuild;
        }

        /// <summary>
        /// The SendBackToProgress.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<AssemblyBuildDto> SendBackToProgress(string username, Guid assemblyId)
        {
            User user = await this.userRepository.FindByUsernameAsync(username).ConfigureAwait(false);
            if (user == null || user.Roles.Where(x => x.Role.Name == "ADMINISTRATOR" || x.Role.Name == "MANAGER").ToList().Count == 0)
            {
                return null;
            }
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetAsync(assemblyId).ConfigureAwait(false);
            if (assembly != null && assembly.Status == ABStatus.WAITING)
            {
                assembly.Status = ABStatus.PENDING;
                assembly.ToProductionDate = default(long);
                AssemblyBuild assemblyUpdated = await this.assemblyBuildRepository.UpdateAsync(assembly);
                return assemblyUpdated.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
            }
            return null;
        }

        /// <summary>
        /// The NewLotAvailableForStep.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="stepRequired">The stepRequired<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> NewLotAvailableForStep(ISession session, Guid assemblyId, int stepRequired)
        {
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetAsyncWithSession(session, assemblyId);
            if (assembly == null)
            {
                return false;
            }
            if (assembly.Formula != null)
            {
                IList<FormulaStep> steps = assembly.Formula.Steps;
                if (steps.Count > 0)
                {
                    IList<FormulaStep> lotsAvailable = steps.Where(s => s.Step == stepRequired && !s.Written).ToList();
                    if (lotsAvailable.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            else if (assembly.NetsuiteFormula != null)
            {

                IList<NetsuiteFormulaStep> steps = assembly.NetsuiteFormula.Steps;
                if (steps.Count > 0)
                {
                    IList<NetsuiteFormulaStep> lotsAvailable = steps.Where(s => s.AdditionSequence == stepRequired && !s.Written).ToList();
                    if (lotsAvailable.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// The CreateEmptyLot.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="stepRequired">The stepRequired<see cref="int"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> CreateEmptyLot(Guid assemblyId, int stepRequired, ISession session = null)
        {
            AssemblyBuild assembly = null;
            bool result = false;
            if (session != null)
            {
                assembly = await this.assemblyBuildRepository.GetAsyncWithSession(session, assemblyId);
            }
            else
            {
                assembly = await this.assemblyBuildRepository.GetAsync(assemblyId);
            }
            if (assembly == null)
            {
                return result;
            }

            if (assembly.Formula != null)
            {
                IList<FormulaStep> steps = assembly.Formula.Steps;
                if (steps.Count > 0)
                {
                    IList<FormulaStep> lotsAvailable = steps.Where(s => s.Step == stepRequired).ToList();
                    if (lotsAvailable.Count > 0)
                    {
                        FormulaStep stepToClone = lotsAvailable.First();
                        FormulaStep newLot = new FormulaStep()
                        {
                            Step = stepToClone.Step,
                            Formula = stepToClone.Formula,
                            SetPoint = 0,
                            StartDate = 0,
                            EndDate = 0,
                            Written = false,
                            InventoryLot = string.Empty,
                            ItemCode = stepToClone.ItemCode,
                            ItemName = stepToClone.ItemName,
                            Operator = stepToClone.Operator,
                            BlenderPercentaje = stepToClone.BlenderPercentaje,
                        };
                        if (session == null)
                        {
                            await this.formulaStepRepository.SaveAsync(newLot).ConfigureAwait(false);
                        }
                        else
                        {
                            await this.formulaStepRepository.SaveStepWithSession(session, newLot).ConfigureAwait(false);
                        }
                        result = true;
                    }
                }
            }
            else if (assembly.NetsuiteFormula != null)
            {
                IList<NetsuiteFormulaStep> steps = assembly.NetsuiteFormula.Steps;
                if (steps.Count > 0)
                {
                    IList<NetsuiteFormulaStep> lotsAvailable = steps.Where(s => s.AdditionSequence == stepRequired).ToList();
                    if (lotsAvailable.Count > 0)
                    {
                        NetsuiteFormulaStep stepToClone = lotsAvailable.First();
                        NetsuiteFormulaStep newLot = new NetsuiteFormulaStep()
                        {
                            AdditionSequence = stepToClone.AdditionSequence,
                            BatchNumber = stepToClone.BatchNumber,
                            Formula = stepToClone.Formula,
                            QtyRequired = 0,
                            StartDate = 0,
                            EndDate = 0,
                            Written = false,
                            InventoryLot = string.Empty,
                            InventoryDetailId = stepToClone.InventoryDetailId,
                            Units = stepToClone.Units,
                            ItemCode = stepToClone.ItemCode,
                            ItemName = stepToClone.ItemName,
                            RawMaterialUnits = stepToClone.RawMaterialUnits,
                            Operator = stepToClone.Operator,
                            StirringRate1 = stepToClone.StirringRate1,
                            StirringTime = stepToClone.StirringTime,
                            Temperature = stepToClone.Temperature
                        };
                        if (session == null)
                        {
                            await this.netsuiteFormulaStepRepository.SaveAsync(newLot).ConfigureAwait(false);
                        }
                        else
                        {
                            await this.netsuiteFormulaStepRepository.SaveStepWithSession(session, newLot).ConfigureAwait(false);
                        }
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// The ReloadWithNewLotAsync.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> ReloadWithNewLotAsync(Guid assemblyBuildId, int stepSequence = 0)
        {
            AssemblyBuild assembly = await this.assemblyBuildRepository.GetAsync(assemblyBuildId);
            AssemblyBuildDto assemblyCloned = assembly.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
            AssemblyBuildDto assemblySelected = SelectUniqueLotForSteps(assemblyCloned, stepSequence);
            return assemblySelected;
        }
    }
}
