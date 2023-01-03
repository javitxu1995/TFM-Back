namespace Auxquimia.Service.Business.Kafka
{
    using Autofac;
    using Auxquimia.Config;
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Dto.Business.Opc;
    using Auxquimia.Enums;
    using Auxquimia.Exceptions;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Service.Business.AssemblyBuilds;
    using Auxquimia.Utils;
    using Auxquimia.Utils.Kafka;
    using Auxquimia.Utils.Kafka.Enum;
    using Auxquimia.Utils.Kafka.Model;
    using Newtonsoft.Json;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AuxquimiaKafkaService" />.
    /// </summary>
    internal class AuxquimiaKafkaService : IAuxquimiaKafkaService
    {
        /// <summary>
        /// Gets or sets a value indicating whether KafkaLoop.
        /// </summary>
        private bool KafkaLoop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether MessageRecived.
        /// </summary>
        public static bool MessageRecived { get; set; }

        /// <summary>
        /// Gets or sets the KafkaEndpoint.
        /// </summary>
        private string KafkaEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the Assembly.
        /// </summary>
        public static AssemblyBuildDto Assembly { get; set; }

        /// <summary>
        /// Gets or sets the MessageKafka.
        /// </summary>
        private MessageKafka MessageKafka { get; set; }

        /// <summary>
        /// Defines the serviceProvider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Defines the contextConfig.
        /// </summary>
        private readonly IContextConfigProvider contextConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuxquimiaKafkaService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="contextConfigProvider">The contextConfigProvider<see cref="IContextConfigProvider"/>.</param>
        public AuxquimiaKafkaService(IServiceProvider serviceProvider, IContextConfigProvider contextConfigProvider)
        {
            KafkaLoop = true;

            this.serviceProvider = serviceProvider;
            this.contextConfig = contextConfigProvider;
            this.KafkaEndpoint = this.contextConfig.KafkaServer;
            MessageRecived = false;
        }

        /// <summary>
        /// The WriteAssembly.
        /// </summary>
        /// <param name="assemblyBuild">The assemblyBuild<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task WriteAssembly(AssemblyBuildDto assemblyBuild)
        {
            string topic = assemblyBuild.Blender.Id;
            OpcOrder order = AssemblyToOpcOrder(assemblyBuild);
            string content = JsonConvert.SerializeObject(order);
            try
            {
                MessageKafka message = new MessageKafka(MessageType.PRODUCTION, content, KafkaManager.CONSUMER_NAME);
                //Escribir en Auxquimia
                await KafkaManager.Get(KafkaEndpoint).Produce(topic + Constants.Kafka.Configuration.AUXQUIMIA_EXTENSION_TOPIC, message);
            }
            catch (Exception e)
            {
                throw new CustomException(Constants.Kafka.Errors.SERIALIZE_ERROR);
            }
        }

        /// <summary>
        /// The InitNewConsumer.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public async void InitNewConsumer(string topic)
        {
            //Leer de satelite
            KafkaManager.Get(KafkaEndpoint).CreateConsumer(new string[] { (topic) },
                 async (result) =>
                 {
                     if (result != null)
                     {
                         Console.WriteLine($"[KAFKA CONSUMER] Consumer from {result.Topic} received messeage.");
                         Console.WriteLine("---------");
                         Console.WriteLine($"[KAFKA CONSUMER] Message offset {result.Offset}.");
                         Console.WriteLine("---------");
                         if (result.Message != null && result.Message.Value != null)
                         {
                             string message = result.Message.Value;
                             try
                             {
                                 MessageKafka msn = JsonConvert.DeserializeObject<MessageKafka>(message);
                                 if (msn.Type == MessageType.INITIALIZATION)
                                 {
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Initialization message");
                                 }
                                 else if (msn.Type == MessageType.PRODUCTION)
                                 {
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Production message");
                                     OpcOrder order = JsonConvert.DeserializeObject<OpcOrder>(msn.Content);
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Assembly {order.AssemblyNumber} recived! - Factory {order.FactoryName} - Step {order.Salida.NºPasoRecetaCliente}");
                                     await FillAssemblyWithOrder(order).ConfigureAwait(false);

                                 }
                                 else if (msn.Type == MessageType.CONFIGURATION)
                                 {
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Configuration message");
                                 }

                             }
                             catch (ObjectDisposedException ode)
                             {
                                 Console.WriteLine($"Kafka Result error => ERROR while searching on db - {ode.Message}");
                             }
                             catch (Exception e)
                             {
                                 Console.WriteLine($"Exception catched \n- {e.Message} \n- {e.StackTrace}");
                             }
                         }

                     }
                     else
                     {
                         Console.WriteLine("[KAFKA CONSUMER] No result after consume");
                     }
                 });
        }

        /// <summary>
        /// The AddSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void AddSubscription(string topic)
        {
            string topicModified = "";
            if (topic.Contains(Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC))
            {
                topicModified = topic;
            }
            else
            {
                topicModified = topic + Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC;
            }
            if (!KafkaManager.Get(KafkaEndpoint).IsConsumerCreated())
            {
                InitNewConsumer(topicModified);
            }
            else
            {
                KafkaManager.Get(KafkaEndpoint).AddSubscription(topicModified);
            }
        }

        /// <summary>
        /// The GetSubscriptions.
        /// </summary>
        /// <returns>The <see cref="IList{string}"/>.</returns>
        public IList<string> GetSubscriptions()
        {
            IList<string> subs = new List<string>();
            subs = KafkaManager.Get(KafkaEndpoint).GetSubscriptions();
            return subs;
        }

        /// <summary>
        /// The ConsumeAll.
        /// </summary>
        public void ConsumeAll()
        {
            KafkaManager.Get(KafkaEndpoint).ConsumeAll();
        }

        /// <summary>
        /// The ClearAllTopics.
        /// </summary>
        public void ClearAllTopics()
        {
            KafkaManager.Get(KafkaEndpoint).ClearConsumerTopics();
        }

        /// <summary>
        /// The UpdateTopics.
        /// </summary>
        /// <param name="newTopics">The newTopics<see cref="IList{string}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool UpdateTopics(IList<string> newTopics)
        {
            bool result = false;

            newTopics = newTopics.Select(x => x = x + Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC).ToList();
            IList<string> actualTopics = GetSubscriptions();
            IList<string> topicsToAdd = newTopics.Except(actualTopics).ToList();
            IList<string> topicsToRemove = actualTopics.Except(newTopics).ToList();
            foreach (string sub in topicsToAdd)
            {
                AddSubscription(sub);
            }
            foreach (string sub in topicsToRemove)
            {
                RemoveSubscription(sub);
            }
            result = true;


            return result;
        }

        /// <summary>
        /// The RemoveSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool RemoveSubscription(string topic)
        {
            bool result = false;

            IList<string> actualTopics = GetSubscriptions();
            topic = topic + Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC;
            if (actualTopics.Contains(topic))
            {
                KafkaManager.Get(KafkaEndpoint).RemoveSubscription(topic);
            }
            result = true;

            return result;
        }

        /// <summary>
        /// The Consume.
        /// </summary>
        public async void Consume()
        {
            if (!KafkaManager.Get(KafkaEndpoint).IsConsumerCreated())
            {
                Console.WriteLine($"[KAFKA DEBUG CONSUME] Consumer not created or no topics, lets get all!");
                await UpdateKafkaConsumerTopics().ConfigureAwait(false);
            }
            KafkaManager.Get(KafkaEndpoint).Consume();
        }

        /// <summary>
        /// The StopLoop.
        /// </summary>
        public void StopLoop()
        {
            KafkaLoop = false;
        }

        /// <summary>
        /// The RestartLoop.
        /// </summary>
        public void RestartLoop()
        {
            if (!KafkaLoop)
            {
                KafkaLoop = true;
                InitConsumerThread();
            }
        }

        /// <summary>
        /// The InitConsumerThread.
        /// </summary>
        public void InitConsumerThread()
        {
            Task.Run(
               () =>
               {
                   Console.WriteLine("[KAFKA] Waiting 30secs to init.");
                   Thread.Sleep(30000);
                   while (KafkaLoop)
                   {
                       Console.WriteLine("[KAFKA] Consuming...");
                       Console.WriteLine($"[KAFKA] Topic subscribed {GetSubscriptions().Count}");
                       Consume();
                       Thread.Sleep(10000);
                   }

               });
        }

        /// <summary>
        /// The UpdateKafkaConsumerTopics.
        /// </summary>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> UpdateKafkaConsumerTopics()
        {
            IList<Reactor> reactors;
            ISession session = null;
            try
            {
                session = AuxquimiaServiceProvider.OpenSession();
                reactors = await AuxquimiaServiceProvider.GetReactorRepository().GetAllAsyncWithSession(session);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[KAFKA INIT EXCEPTION] Query exception: - {e.Message}");
                reactors = new List<Reactor>();
            }
            finally
            {
                if (session != null)
                {
                    session.Close();
                }
            }
            bool result = false;
            IList<string> actualTopics = new List<string>();
            foreach (Reactor reactor in reactors)
            {
                string topic = reactor.Id.ToString();
                actualTopics.Add(topic);
            }

            result = UpdateTopics(actualTopics);
            return result;
        }

        /// <summary>
        /// The SendAssemblyBuildToKafka.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> SendAssemblyBuildToKafka(Guid assemblyId)
        {
            AssemblyBuildDto assembly = await AuxquimiaServiceProvider.GetAssemblyBuildService().GetToProductionAsync(assemblyId);

            if (assembly == null)
            {
                throw new CustomException(Constants.Opc.Errors.ILLEGAL_STATE_ASSEMBLY);
            }
            await WriteAssembly(assembly);
            return assembly;
        }

        /// <summary>
        /// The AssemblyToOpcOrder.
        /// </summary>
        /// <param name="assembly">The assembly<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="OpcOrder"/>.</returns>
        private OpcOrder AssemblyToOpcOrder(AssemblyBuildDto assembly)
        {
            OpcOrder order = new OpcOrder();
            order.AssemblyId = assembly.Id;
            order.AssemblyNumber = assembly.AssemblyBuildNumber;
            order.OpcServer = assembly.Factory.OpcServer;
            order.FactoryName = assembly.Factory.Name;
            order.DbEntrada = assembly.Blender.DbWrite;
            order.DbSalida = assembly.Blender.DbRead;
            //Init
            int maxSize = Constants.Opc.MAX_STEP_SIZE;
            order.InitAttributes(maxSize);

            if (assembly.Formula != null)
            {
                FillOrderFormula(order, assembly.Formula);
            }
            else if (assembly.NetsuiteFormula != null)
            {
                FillOrderNetsuiteFormula(order, assembly.NetsuiteFormula);
            }
            return order;
        }

        /// <summary>
        /// The FillOrderFormula.
        /// </summary>
        /// <param name="order">The order<see cref="OpcOrder"/>.</param>
        /// <param name="formula">The formula<see cref="FormulaDto"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool FillOrderFormula(OpcOrder order, FormulaDto formula)
        {
            int maxSize = Constants.Opc.MAX_STEP_SIZE;
            order.Entrada.NombreReceta = formula.Name;
            order.Entrada.VelocidadAgitador2UltimoPaso = formula.BlenderFinalPercentaje;
            order.Entrada.TiempoMTOAgitador2UltimoPaso = formula.BlenderFinalTime;
            int index = 1;
            foreach (FormulaStepDto step in formula.Steps)
            {
                if (index >= maxSize) { break; }
                order.Entrada.Agitador1Paso[index] = step.BlenderPercentaje;
                order.Entrada.ConsignaPesoPaso[index] = step.SetPoint;
                order.Entrada.LotePaso[index] = step.IsWater ? "Water" : step.InventoryLot;
                order.Entrada.ProductoPaso[index] = step.ItemCode; 
                order.Entrada.ProductoRFIDPaso[index] = step.IsWater ? "water" : HelperMethods.BuildRFIDCode(step.ItemCode + "", step.InventoryLot);
                order.Entrada.TiempoMTOPaso[index] = default(Int16);
                order.Entrada.NombrePaso[index] = step.ItemName;
                index++;
            }
            return true;
        }

        /// <summary>
        /// The FillOrderNetsuiteFormula.
        /// </summary>
        /// <param name="order">The order<see cref="OpcOrder"/>.</param>
        /// <param name="formula">The formula<see cref="NetsuiteFormulaDto"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool FillOrderNetsuiteFormula(OpcOrder order, NetsuiteFormulaDto formula)
        {
            int maxSize = Constants.Opc.MAX_STEP_SIZE;
            order.Entrada.NombreReceta = formula.Name;
            order.Entrada.VelocidadAgitador2UltimoPaso = formula.BlenderFinalPercentaje;
            order.Entrada.TiempoMTOAgitador2UltimoPaso = formula.BlenderFinalTime;
            int index = 1;
            foreach (NetsuiteFormulaStepDto step in formula.Steps)
            {
                if (index >= maxSize) { break; }
                order.Entrada.Agitador1Paso[index] = Convert.ToInt16(step.StirringRate1);
                order.Entrada.ConsignaPesoPaso[index] = (float)step.QtyRequired;
                order.Entrada.LotePaso[index] = step.InventoryLot;
                order.Entrada.ProductoPaso[index] = step.ItemCode;
                order.Entrada.ProductoRFIDPaso[index] = HelperMethods.BuildRFIDCode(step.ItemCode, step.InventoryLot);
                order.Entrada.TiempoMTOPaso[index] = default(Int16);
                order.Entrada.NombrePaso[index] = step.ItemName;
                index++;
            }
            return true;
        }

        /// <summary>
        /// The FillAssemblyWithOrder.
        /// </summary>
        /// <param name="order">The order<see cref="OpcOrder"/>.</param>
        /// <returns>The <see cref="AssemblyBuildDto"/>.</returns>
        private async Task FillAssemblyWithOrder(OpcOrder order)
        {


            using (ISession session = AuxquimiaServiceProvider.OpenSession())
            {

                try
                {
                    ITransaction tx = session.BeginTransaction();
                    AssemblyBuild assembly = null;
                    assembly = await AuxquimiaServiceProvider.GetAssemblyBuildRepository().GetAsyncWithSession(session, order.AssemblyId.PerformMapping<string, Guid>()).ConfigureAwait(false);
                    if (assembly != null)
                    {



                        if (assembly.Status == ABStatus.WAITING)
                        {
                            assembly.Status = ABStatus.PROGRESS;
                            await AuxquimiaServiceProvider.GetAssemblyBuildRepository().UpdateAsyncWithSession(session, assembly).ConfigureAwait(false);
                        }

                        if (assembly.Status == ABStatus.PROGRESS) //Only update when assembly in progress
                        {
                            await UpdateAssembyWithOpcData(session, assembly, order).ConfigureAwait(false);

                            //New Lot
                            if (order.Salida.BotonConfirmacionDiferenteLote)
                            {
                                Console.WriteLine($"[PRODUCTION - LOT] New lot request for assembly [{assembly.AssemblyBuildNumber}] - Step [{order.Salida.NºPasoRecetaCliente}].");
                                IAssemblyBuildService service = AuxquimiaServiceProvider.GetAssemblyBuildService();
                                bool lotsAvailable = await service.NewLotAvailableForStep(session, assembly.Id, order.Salida.NºPasoRecetaCliente).ConfigureAwait(false);
                                if (lotsAvailable)
                                {
                                    AssemblyBuildDto newLotAssembly = await service.GetWithNewLotForStep(assembly.Id, order.Salida.NºPasoRecetaCliente, session).ConfigureAwait(false);
                                    await WriteAssembly(newLotAssembly);
                                    await MarkStepsAsWritted(newLotAssembly, session).ConfigureAwait(false);
                                    Console.WriteLine($"[PRODUCTION - LOT] New lot for assembly [{assembly.AssemblyBuildNumber}] - Step [{order.Salida.NºPasoRecetaCliente}] has been send.");
                                }
                                else
                                {
                                    //Crear lote
                                    bool result = await service.CreateEmptyLot(assembly.Id, order.Salida.NºPasoRecetaCliente, session).ConfigureAwait(false);
                                    if (result)
                                    {
                                        Console.WriteLine($"[PRODUCTION - LOT] New lot for assembly [{assembly.AssemblyBuildNumber}] - Step [{order.Salida.NºPasoRecetaCliente}] has been created and now waiting for operator.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"[PRODUCTION - LOT] New lot for assembly [{assembly.AssemblyBuildNumber}] - Step [{order.Salida.NºPasoRecetaCliente}] cannot be created!.");
                                    }
                                }

                            }

                            //Order aborted
                            if (order.Abort)
                            {
                                assembly.Status = ABStatus.FINISHED;
                                assembly.Aborted = true;
                                await AuxquimiaServiceProvider.GetAssemblyBuildRepository().UpdateAsyncWithSession(session, assembly).ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[ASSEMBLY STATUS ERROR] Assembly [{assembly.AssemblyBuildNumber}] is not in PROGRESS, is in {Enum.GetName(typeof(ABStatus), assembly.Status)} - [NOT UPDATED]");
                        }

                    }

                    tx.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[DEBUG EXCEPTION] REVISAR FILL ASSEMBLY {e.Message}");
                }
            }
        }

        /// <summary>
        /// The MarkStepsAsWritted.
        /// </summary>
        /// <param name="assemblyWritten">The assemblyWritten<see cref="AssemblyBuildDto"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>

        private async Task<AssemblyBuildDto> MarkStepsAsWritted(AssemblyBuildDto assemblyWritten, ISession session = null)
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
                        await MarkFormulaStepAsWritted(stepId, session);
                    }

                }
            }
            else if (assemblyWritten.NetsuiteFormula != null)
            {
                IList<NetsuiteFormulaStepDto> steps = assemblyWritten.NetsuiteFormula.Steps;
                foreach (NetsuiteFormulaStepDto step in steps)
                {
                    Guid stepId = step.Id.PerformMapping<string, Guid>();
                    await MarkNetsuiteFormulaStepAsWritted(stepId, session);
                }
            }
            else
            {
                return null;
            }
            return assemblyWritten;
        }

        /// <summary>
        /// The MarkFormulaStepAsWritted.
        /// </summary>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{FormulaStepDto}"/>.</returns>
        public async Task<FormulaStepDto> MarkFormulaStepAsWritted(Guid stepId, ISession session = null)
        {
            FormulaStep step = null;
            if (session != null)
            {
                step = await AuxquimiaServiceProvider.GetFormulaStepRepository().GetAsyncWithSession(session, stepId);
            }
            else
            {
                step = await AuxquimiaServiceProvider.GetFormulaStepRepository().GetAsync(stepId);
            }
            if (step != null)
            {
                step.Written = true;
                if (session != null)
                {
                    step = await AuxquimiaServiceProvider.GetFormulaStepRepository().UpdateStepWithSession(session, step);
                }
                else
                {
                    step = await AuxquimiaServiceProvider.GetFormulaStepRepository().UpdateAsync(step);
                }
                return step.PerformMapping<FormulaStep, FormulaStepDto>();
            }
            return null;
        }

        /// <summary>
        /// The MarkNetsuiteFormulaStepAsWritted.
        /// </summary>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        public async Task<NetsuiteFormulaStepDto> MarkNetsuiteFormulaStepAsWritted(Guid stepId, ISession session = null)
        {
            NetsuiteFormulaStep step = null;
            if (session != null)
            {
                step = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().GetAsyncWithSession(session, stepId);
            }
            else
            {
                step = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().GetAsync(stepId);
            }
            if (step != null)
            {
                step.Written = true;
                if (session != null)
                {
                    step = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().UpdateStepWithSession(session, step);
                }
                else
                {
                    step = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().UpdateAsync(step);
                }
                return step.PerformMapping<NetsuiteFormulaStep, NetsuiteFormulaStepDto>();
            }
            return null;
        }

        /// <summary>
        /// The UpdateAssembyWithOpcData.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="assembly">The assembly<see cref="AssemblyBuild"/>.</param>
        /// <param name="opcData">The opcData<see cref="OpcOrder"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        private async Task<bool> UpdateAssembyWithOpcData(ISession session, AssemblyBuild assembly, OpcOrder opcData)
        {
            bool result = false;
            long horaInicio = opcData.Salida.HoraInicioDosificacion;
            //long hInicio = DateHelper.BCDToLong(horaInicio);
            long horaFin = opcData.Salida.HoraFinDosificacion;
            //long hFin = DateHelper.BCDToLong(horaFin);

            if (assembly != null && opcData != null)
            {
                int actualStep = opcData.Salida.NºPasoRecetaCliente;
                bool estadoProceso = opcData.Salida.EstadoDosificacion;
                if (actualStep > 0 || estadoProceso) //REVISAR
                {
                    bool mismoLote = opcData.Salida.BotonConfirmacionMismoLote;
                    bool diferenteLote = opcData.Salida.BotonConfirmacionDiferenteLote;
                    float pesoTotalReactor = opcData.Salida.PesoTotalReactor;


                    if (assembly.NetsuiteFormula != null)
                    {
                        for (int index = 1; index <= actualStep; index++)
                        {
                            await UpdateNetsuiteStep(session, assembly, index, opcData);
                        }
                        //await UpdateNetsuiteStep(session, assembly, actualStep, opcData);
                    }
                    else
                    {
                        for (int index = 1; index <= actualStep; index++)
                        {
                            await UpdateFormulaStep(session, assembly, index, opcData);
                        }
                        //await UpdateFormulaStep(session, assembly, actualStep, opcData);
                    }
                }

                if (assembly.Status == ABStatus.PROGRESS && opcData.Finalized)
                {
                    Console.WriteLine($"[PRODUCTION] Assembly {assembly.AssemblyBuildNumber} production has finished!");
                    assembly.Status = ABStatus.FINISHED;
                    await AuxquimiaServiceProvider.GetAssemblyBuildRepository().UpdateAsyncWithSession(session, assembly).ConfigureAwait(false);
                }


            }
            return result;
        }

        /// <summary>
        /// The UpdateNetsuiteStep.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="assembly">The assembly<see cref="AssemblyBuild"/>.</param>
        /// <param name="stepNumber">The stepNumber<see cref="int"/>.</param>
        /// <param name="opcData">The opcData<see cref="OpcOrder"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task UpdateNetsuiteStep(ISession session, AssemblyBuild assembly, int stepNumber, OpcOrder opcData)
        {
            long hTiempoPasoInicio = default(long);
            long hTiempoPasoFin = default(long);
            long[] tiemposPaso = opcData.Salida.TiempoEventos;
            if (tiemposPaso != null && stepNumber > 0)
            {
                if (stepNumber == 1)
                {
                    hTiempoPasoInicio = opcData.Salida.HoraInicioDosificacion;
                }
                else
                {
                    hTiempoPasoInicio = tiemposPaso[stepNumber - 1];
                }
                hTiempoPasoFin = tiemposPaso[stepNumber];
            }

            if (stepNumber <= assembly.NetsuiteFormula.Steps.Count && stepNumber > 0)
            {
                string currentLot = opcData.Entrada.LotePaso[stepNumber];
                NetsuiteFormulaStep step = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().GetByStepAndLotWithSession(session, stepNumber, currentLot);


                if (step != null)
                {
                    step.StartDate = hTiempoPasoInicio;
                    step.EndDate = hTiempoPasoFin;
                    //Actualizar peso del paso
                    IList<NetsuiteFormulaStep> stepsWithDifferentLot = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().FindOtherLotsWithSession(session, step.Formula.Id, stepNumber, step.InventoryLot).ConfigureAwait(false);
                    if (stepsWithDifferentLot != null && stepsWithDifferentLot.Count > 0)
                    {
                        float resto = stepsWithDifferentLot.Sum(x => x.RealQtyAdded);
                        float pesoReal = opcData.Salida.PesoPaso[stepNumber] - resto;
                        step.RealQtyAdded = pesoReal >= 0 ? pesoReal : opcData.Salida.PesoPaso[stepNumber];
                    }
                    else
                    {
                        float pesoReal = opcData.Salida.PesoPaso[stepNumber];
                        step.RealQtyAdded = pesoReal >= 0 ? pesoReal : step.RealQtyAdded;
                    }
                    //Comprobación del operador del paso
                    try
                    {
                        string usernamePaso = opcData.Salida.OperadorPaso[stepNumber];
                        if (step.Operator == null || step.Operator.Username != usernamePaso)
                        {
                            User realOperator = await AuxquimiaServiceProvider.GetUserRepository().FindByUsernameAndFactoryAsyncWithSession(session, usernamePaso, assembly.Factory.Id);
                            if (realOperator != null)
                            {
                                step.Operator = realOperator;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot parse step [{stepNumber}] - operator code - {opcData.Salida.OperadorPaso[stepNumber]}");
                    }
                    //Actualizar en BBDD
                    await AuxquimiaServiceProvider.GetNetsuiteStepRepository().UpdateStepWithSession(session, step);
                }
                else
                {
                    Console.WriteLine($"Cannot find [step {stepNumber}] - [lot {currentLot}]");
                }
            }
            else if (stepNumber == assembly.Formula.Steps.Count + 1)
            {
                //Actualizar datos de paso extra???
                Console.WriteLine($"Assembly on final step [step {stepNumber}]");
            }

            //Actualizar formula
            assembly.NetsuiteFormula.TotalWeight = opcData.Salida.PesoTotalReactor > 0 ? opcData.Salida.PesoTotalReactor : 0;
            assembly.NetsuiteFormula.StartDate = opcData.Salida.HoraInicioDosificacion > 0 ? opcData.Salida.HoraInicioDosificacion : 0;
            if (opcData.Finalized)
            {
                assembly.NetsuiteFormula.EndDate = opcData.Salida.HoraFinDosificacion > 0 ? opcData.Salida.HoraFinDosificacion : 0;
            }
            await AuxquimiaServiceProvider.GetNetsuiteRepository().UpdateFormulaWithSession(session, assembly.NetsuiteFormula);
        }

        /// <summary>
        /// The UpdateFormulaStep.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="assembly">The assembly<see cref="AssemblyBuild"/>.</param>
        /// <param name="stepNumber">The stepNumber<see cref="int"/>.</param>
        /// <param name="opcData">The opcData<see cref="OpcOrder"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task UpdateFormulaStep(ISession session, AssemblyBuild assembly, int stepNumber, OpcOrder opcData)
        {
            long hTiempoPasoInicio = default(long);
            long hTiempoPasoFin = default(long);
            long[] tiemposPaso = opcData.Salida.TiempoEventos;
            if (tiemposPaso != null && stepNumber > 0)
            {
                if (stepNumber == 1)
                {
                    hTiempoPasoInicio = opcData.Salida.HoraInicioDosificacion;
                }
                else
                {
                    hTiempoPasoInicio = tiemposPaso[stepNumber - 1];
                }
                hTiempoPasoFin = tiemposPaso[stepNumber];
            }
            if (stepNumber <= assembly.Formula.Steps.Count && stepNumber > 0)
            {

                string currentLot = opcData.Entrada.LotePaso[stepNumber];
                FormulaStep step = await AuxquimiaServiceProvider.GetFormulaStepRepository().GetByStepAndLotWithSession(session, stepNumber, currentLot);
                if (step != null)
                {
                    step.StartDate = hTiempoPasoInicio;
                    step.EndDate = hTiempoPasoFin;
                    //Actualizar peso
                    IList<FormulaStep> stepsWithDifferentLot = await AuxquimiaServiceProvider.GetFormulaStepRepository().FindOtherLotsWithSession(session, step.Formula.Id, stepNumber, step.InventoryLot).ConfigureAwait(false);
                    if (stepsWithDifferentLot != null && stepsWithDifferentLot.Count > 0)
                    {
                        float resto = stepsWithDifferentLot.Sum(x => x.RealWeight);
                        float pesoReal = opcData.Salida.PesoPaso[stepNumber] - resto;
                        step.RealWeight = pesoReal >= 0 ? pesoReal : opcData.Salida.PesoPaso[stepNumber];
                    }
                    else
                    {
                        float pesoReal = opcData.Salida.PesoPaso[stepNumber];
                        step.RealWeight = pesoReal >= 0 ? pesoReal : step.RealWeight;
                    }
                    //Comprobación del operador del paso
                    try
                    {
                        //int operadorPaso = Int32.Parse(opcData.Salida.OperadorPaso[stepNumber]);
                        string usernamePaso = opcData.Salida.OperadorPaso[stepNumber];
                        if (step.Operator == null || step.Operator.Username != usernamePaso)
                        {
                            //User realOperator = await AuxquimiaServiceProvider.GetUserRepository().FindByCodeAsyncWithSession(session, operadorPaso, assembly.Factory.Id);
                            User realOperator = await AuxquimiaServiceProvider.GetUserRepository().FindByUsernameAndFactoryAsyncWithSession(session, usernamePaso, assembly.Factory.Id);
                            if (realOperator != null)
                            {
                                step.Operator = realOperator;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot parse step [{stepNumber}] - operator code {opcData.Salida.OperadorPaso[stepNumber]}");
                    }
                    //Actualizar en BBDD
                    await AuxquimiaServiceProvider.GetFormulaStepRepository().UpdateStepWithSession(session, step);
                }
                else
                {
                    Console.WriteLine($"Cannot find [step {stepNumber}] - [lot {currentLot}]");
                }

            }
            else if (stepNumber == assembly.Formula.Steps.Count + 1)
            {
                //Actualizar datos de paso extra???
                Console.WriteLine($"Assembly on final step [step {stepNumber}]");
            }

            //Actualizar formula
            assembly.Formula.TotalWeight = opcData.Salida.PesoTotalReactor > 0 ? opcData.Salida.PesoTotalReactor : 0;
            assembly.Formula.StartDate = opcData.Salida.HoraInicioDosificacion > 0 ? opcData.Salida.HoraInicioDosificacion : 0;
            if (opcData.Finalized)
            {
                assembly.Formula.EndDate = opcData.Salida.HoraFinDosificacion > 0 ? opcData.Salida.HoraFinDosificacion : 0;
            }
            await AuxquimiaServiceProvider.GetFormulaRepository().UpdateFormulaWithSession(session, assembly.Formula);
        }

        /// <summary>
        /// The CheckAssembliesToSend.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> CheckAssembliesToSend(Guid reactorId)
        {
            Reactor reactor = await AuxquimiaServiceProvider.GetReactorRepository().GetAsync(reactorId);
            if (reactor == null)
            {
                return null;
            }
            IList<AssemblyBuild> assembliesRunning = await AuxquimiaServiceProvider.GetAssemblyBuildRepository().CheckAssembliesOnProduction(reactor.Id);
            if (assembliesRunning.Count > 0)
            {
                return null;
            }
            IList<AssemblyBuild> assembliesToProduct = await AuxquimiaServiceProvider.GetAssemblyBuildRepository().FindAssembliesWaitingToProduction(reactor.Id);
            if (assembliesToProduct.Count > 0)
            {
                AssemblyBuild assToProduct = assembliesToProduct.First();
                return assToProduct.PerformMapping<AssemblyBuild, AssemblyBuildDto>();
            }
            return null;
        }

        /// <summary>
        /// The SendAssemblyToWaitingQueue.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        public async Task<AssemblyBuildDto> SendAssemblyToProduction(Guid reactorId)
        {
            AssemblyBuildDto assemblyWaiting = await CheckAssembliesToSend(reactorId);

            if (assemblyWaiting != null)
            {
                Guid assemblyId = assemblyWaiting.Id.PerformMapping<string, Guid>();
                AssemblyBuildDto assemblyToProduction = await AuxquimiaServiceProvider.GetAssemblyBuildService().GetToProductionAsync(assemblyId);
                if (assemblyToProduction == null)
                {
                    return null;
                }
                await WriteAssembly(assemblyToProduction);
                //Assembly Writed on OPC, lets mark steps as writted.
                await AuxquimiaServiceProvider.GetAssemblyBuildService().MarkStepsAsWritten(assemblyToProduction);

                //Steps marked, now assembly must be on progress
                Guid assId = assemblyWaiting.Id.PerformMapping<string, Guid>();
                AssemblyBuild assSended = await AuxquimiaServiceProvider.GetAssemblyBuildRepository().GetAsync(assId);
                if (assSended != null)
                {
                    assSended.Status = ABStatus.PROGRESS;
                    await AuxquimiaServiceProvider.GetAssemblyBuildRepository().UpdateAsync(assSended);
                }

                return assemblyToProduction;
            }
            return null;
        }

        /// <summary>
        /// The SendAssembliesToProduction.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        public async Task<IList<AssemblyBuildDto>> SendAssembliesToProduction()
        {
            IReactorRepository repo = (IReactorRepository)this.serviceProvider.GetService(typeof(IReactorRepository));
            IList<Reactor> reactors = await repo.GetAllAsync().ConfigureAwait(false);
            IList<AssemblyBuildDto> assembliesOnProduction = new List<AssemblyBuildDto>();
            foreach (Reactor reactor in reactors)
            {
                //Writed on OPC
                AssemblyBuildDto ass = await SendAssemblyToProduction(reactor.Id).ConfigureAwait(false);
                if (ass != null)
                {
                    Guid assId = ass.Id.PerformMapping<string, Guid>();
                    AssemblyBuild assSended = await AuxquimiaServiceProvider.GetAssemblyBuildRepository().GetAsync(assId);
                    if (assSended != null)
                    {
                        assSended.Status = ABStatus.PROGRESS;
                        await AuxquimiaServiceProvider.GetAssemblyBuildRepository().UpdateAsync(assSended);
                    }
                    assembliesOnProduction.Add(ass);
                }

            }
            return assembliesOnProduction;
        }

        /// <summary>
        /// The SendAbortOrder.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> SendAbortOrder(Guid assemblyId)
        {
            AssemblyBuild assemblyBuild = await AuxquimiaServiceProvider.GetAssemblyBuildRepository().GetAsync(assemblyId).ConfigureAwait(false);
            if (assemblyBuild == null || assemblyBuild.Status != ABStatus.PROGRESS)
            {
                return false;
            }
            string topic = assemblyBuild.Blender.Id.ToString();
            OpcOrder order = new OpcOrder()
            {
                AssemblyId = assemblyBuild.Id.ToString(),
                AssemblyNumber = assemblyBuild.AssemblyBuildNumber,
                DbEntrada = assemblyBuild.Blender.DbWrite,
                DbSalida = assemblyBuild.Blender.DbRead,
                FactoryName = assemblyBuild.Factory.Name,
                Abort = true
            };
            string content = JsonConvert.SerializeObject(order);
            try
            {
                MessageKafka message = new MessageKafka(MessageType.PROCESS_CONTROL, content, KafkaManager.CONSUMER_NAME);
                //Escribir en Auxquimia
                await KafkaManager.Get(KafkaEndpoint).Produce(topic + Constants.Kafka.Configuration.AUXQUIMIA_EXTENSION_TOPIC, message);

                //Markar como orden de abortar enviada
                assemblyBuild.AbortSend = true;
                await AuxquimiaServiceProvider.GetAssemblyBuildRepository().UpdateAsync(assemblyBuild).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                throw new CustomException(Constants.Kafka.Errors.SERIALIZE_ERROR);
            }
        }

        /// <summary>
        /// The SaveAndSendNetsuiteLotNewLot.
        /// </summary>
        /// <param name="lot">The lot<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> SaveAndSendNetsuiteLotNewLot(NetsuiteFormulaStepDto lot)
        {
            bool result = false;
            if (lot != null && lot.Id != null && lot.Id != default(string) && !string.IsNullOrWhiteSpace(lot.InventoryLot))
            {
                NetsuiteFormulaStep stepLotExists = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().GetByStepAndLot(lot.AdditionSequence, lot.InventoryLot);
                if (stepLotExists == null)
                {
                    NetsuiteFormulaStep step = lot.PerformMapping<NetsuiteFormulaStepDto, NetsuiteFormulaStep>();
                    NetsuiteFormulaStep stepUpdate = await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().UpdateAsync(step).ConfigureAwait(false);

                    Guid assemblyId = stepUpdate.Formula.AssemblyBuild.Id;
                    AssemblyBuildDto assemblyToProduction = await AuxquimiaServiceProvider.GetAssemblyBuildService().ReloadWithNewLotAsync(assemblyId, lot.AdditionSequence);
                    await WriteAssembly(assemblyToProduction).ConfigureAwait(false);

                    stepUpdate.Written = true;
                    await AuxquimiaServiceProvider.GetNetsuiteFormulaStepRepository().UpdateAsync(stepUpdate).ConfigureAwait(false);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// The SaveAndSendFormulaLotNewLot.
        /// </summary>
        /// <param name="lot">The lot<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> SaveAndSendFormulaLotNewLot(FormulaStepDto lot)
        {
            bool result = false;
            if (lot != null && lot.Id != null && lot.Id != default(string) && !string.IsNullOrWhiteSpace(lot.InventoryLot))
            {
                FormulaStep stepLotExists = await AuxquimiaServiceProvider.GetFormulaStepRepository().GetByStepAndLot(lot.Step, lot.InventoryLot).ConfigureAwait(false);
                if (stepLotExists == null)
                {
                    FormulaStep step = lot.PerformMapping<FormulaStepDto, FormulaStep>();
                    FormulaStep stepUpdate = await AuxquimiaServiceProvider.GetFormulaStepRepository().UpdateAsync(step).ConfigureAwait(false);

                    Guid assemblyId = stepUpdate.Formula.AssemblyBuild.Id;
                    AssemblyBuildDto assemblyToProduction = await AuxquimiaServiceProvider.GetAssemblyBuildService().ReloadWithNewLotAsync(assemblyId, lot.Step);
                    await WriteAssembly(assemblyToProduction).ConfigureAwait(false);
                    stepUpdate.Written = true;
                    await AuxquimiaServiceProvider.GetFormulaStepRepository().UpdateAsync(stepUpdate).ConfigureAwait(false);
                    result = true;
                }
            }
            return result;
        }
    }
}
