namespace Auxquimia.Utils.Opc
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Auxquimia.Utils.Constants;

    /// <summary>
    /// Defines the <see cref="OpcManager" />.
    /// </summary>
    public class OpcManager
    {
        /// <summary>
        /// Gets or sets the OPC_SERVER.
        /// </summary>
        private string OPC_SERVER { get; set; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        private static OpcManager instance { get; set; }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="ServerUrl">The ServerUrl<see cref="string"/>.</param>
        /// <returns>The <see cref="OpcManager"/>.</returns>
        public static OpcManager Get(string ServerUrl)
        {
            if (instance == null)
            {
                instance = new OpcManager(ServerUrl);
            }
            else
            {
                instance.OPC_SERVER = ServerUrl;
            }
            return instance;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="OpcManager"/> class from being created.
        /// </summary>
        /// <param name="serverUrl">The serverUrl<see cref="string"/>.</param>
        private OpcManager(string serverUrl)
        {
            OPC_SERVER = serverUrl;
        }

        /// <summary>
        /// The WriteSteps.
        /// </summary>
        /// <param name="steps">The steps<see cref="IList{FormulaStepDto}"/>.</param>
        /// <returns>The <see cref="IList{FormulaStepDto}"/>.</returns>
        public IList<FormulaStepDto> WriteSteps(IList<FormulaStepDto> steps)
        {
            if (steps.Count > Constants.Opc.MAX_STEP_SIZE)
                throw new CustomException(Constants.Opc.Errors.MAX_STEP_LIMIT);
            string[] productArray = Enumerable.Repeat<string>("0", Constants.Opc.MAX_STEP_SIZE).ToArray();
            float[] consignaArray = new float[Constants.Opc.MAX_STEP_SIZE];
            Int16[] blenderArray = new Int16[Constants.Opc.MAX_STEP_SIZE];
            productArray[0] = "0";
            consignaArray[0] = 0;
            blenderArray[0] = 0;
            int iterator = 1;
            foreach (FormulaStepDto step in steps)
            {
                step.CheckStep();
                productArray[iterator] = step.ItemName;
                consignaArray[iterator] = step.SetPoint;
                blenderArray[iterator] = step.BlenderPercentaje;
                iterator++;
            }
            string[] productArrayRead;
            float[] consignaArrayRead;
            Int16[] blenderArrayRead;
            using (UaClient opcManager = new UaClient(OPC_SERVER))
            {
                productArrayRead = opcManager.Write<string[]>(Opc.IN_PRODUCTO_PASO, productArray);
                consignaArrayRead = opcManager.Write<float[]>(Opc.IN_CONSIGNA_PESO_PASO, consignaArray);
                blenderArrayRead = opcManager.Write<Int16[]>(Opc.IN_AGITADOR_1_PASO, blenderArray);
            }

            IList<FormulaStepDto> readSteps = new List<FormulaStepDto>();
            FormulaStepDto readStep;
            for (iterator = 1; iterator < steps.Count; iterator++)
            {
                readStep = new FormulaStepDto()
                {
                    ItemName = productArrayRead[iterator],
                    SetPoint = consignaArrayRead[iterator],
                    BlenderPercentaje = blenderArrayRead[iterator]
                };
                readSteps.Add(readStep);
            }
            return readSteps;
        }

        /// <summary>
        /// The WriteFormula.
        /// </summary>
        /// <param name="formula">The formula<see cref="FormulaDto"/>.</param>
        /// <returns>The <see cref="FormulaDto"/>.</returns>
        public FormulaDto WriteFormula(FormulaDto formula)
        {
            string formulaName = formula.Name;
            Int16 blenderLastStep = formula.BlenderFinalPercentaje;
            Int16 mtoLastStep = formula.BlenderFinalTime;
            IList<FormulaStepDto> steps = formula.Steps;
            using (UaClient opcManager = new UaClient(OPC_SERVER))
            {
                formulaName = opcManager.Write<string>(Opc.IN_NOMBRE_RECETA, formula.Name);
                blenderLastStep = opcManager.Write<Int16>(Opc.IN_AGITADOR_2_ULTIMO_PASO, formula.BlenderFinalPercentaje);
                mtoLastStep = opcManager.Write<Int16>(Opc.IN_TIEMPO_MTO_ULTIMO_PASO, formula.BlenderFinalTime);
            }
            steps = WriteSteps(steps);
            FormulaDto readFormula = new FormulaDto()
            {
                Steps = steps,
                Name = formulaName,
                BlenderFinalPercentaje = blenderLastStep,
                BlenderFinalTime = mtoLastStep
            };
            return readFormula;
        }

        /// <summary>
        /// The WriteAssembly.
        /// </summary>
        /// <param name="assemblyBuildDto">The assemblyBuildDto<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="AssemblyBuildDto"/>.</returns>
        public AssemblyBuildDto WriteAssembly(AssemblyBuildDto assemblyBuildDto)
        {
            AssemblyBuildDto assemblyRead = assemblyBuildDto;
            FormulaDto formulaRead = WriteFormula(assemblyBuildDto.Formula);
            assemblyRead.Formula = formulaRead;
            return assemblyRead;
        }

        /// <summary>
        /// The GetActualStep.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetActualStep()
        {
            Int16 actualStep;
            using (UaClient opcHelper = new UaClient(OPC_SERVER))
            {
                actualStep = opcHelper.Read<Int16>(Constants.Opc.OUT_N_PASO_RECETA_CLIENTE);
            }
            return actualStep;
        }

        /// <summary>
        /// The ReadOpcStepData.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, object}"/>.</returns>
        public Dictionary<string, object> ReadOpcStepData()
        {
            string productName;
            float consigna;
            Int16 blender;
            byte[] startDateb, endDateb;
            long startDate, endDate;
            string operador, productoPaso;
            float pesoPaso;
            int stepIndex = 0;
            using (UaClient opcManager = new UaClient(OPC_SERVER))
            {
                stepIndex = opcManager.Read<Int16>(Constants.Opc.OUT_N_PASO_RECETA_CLIENTE);
                productName = opcManager.ReadValueFromArray<string>(Constants.Opc.OUT_PRODUCTO_PASO, stepIndex);
                consigna = opcManager.ReadValueFromArray<float>(Constants.Opc.OUT_CONSIGNA_PESO, stepIndex);
                blender = opcManager.ReadValueFromArray<Int16>(Constants.Opc.OUT_AGITADOR_1_PASO, stepIndex);
                startDateb = opcManager.ReadValueFromArray<byte[]>(Constants.Opc.OUT_HORA_INICIO_DOSIFICACION, stepIndex);
                endDateb = opcManager.ReadValueFromArray<byte[]>(Constants.Opc.OUT_HORA_FIN_DOSIFICACION, stepIndex);
                operador = opcManager.ReadValueFromArray<string>(Constants.Opc.OUT_OPERADOR_PASO, stepIndex);
                pesoPaso = opcManager.ReadValueFromArray<float>(Constants.Opc.OUT_PESO_PASO, stepIndex);

            }
            startDate = GetLongDate(startDateb);
            endDate = GetLongDate(endDateb);
            return new Dictionary<string, object>()
            {
                { Constants.Opc.KEY_PRODUCTO , productName },
                { Constants.Opc.KEY_CONSIGNA , consigna },
                { Constants.Opc.KEY_BLENDER , blender },
                { Constants.Opc.KEY_START_DATE , startDate },
                { Constants.Opc.KEY_END_DATE , endDate },
                { Constants.Opc.KEY_PESO_PASO , pesoPaso },
                { Constants.Opc.KEY_OPERADOR , operador },
                { Constants.Opc.KEY_STEP_NUMBER , stepIndex }
            };
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="date">The date<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private byte[] GetBytes(DateTime date)
        {
            return BitConverter.GetBytes(date.ToBinary());
        }

        /// <summary>
        /// The GetLongDate.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        private long GetLongDate(byte[] bytes)
        {
            long ldate = BitConverter.ToInt64(bytes);
            DateTime aux = DateTime.FromBinary(ldate);
            return DateHelper.ToUnixTimeMilliseconds(aux);
        }

        /// <summary>
        /// The WriteFormula.
        /// </summary>
        /// <param name="performMapping">The performMapping<see cref="Func{FormulaDto}"/>.</param>
        /// <returns>The <see cref="AssemblyBuildDto"/>.</returns>
        internal AssemblyBuildDto WriteFormula(Func<FormulaDto> performMapping)
        {
            throw new NotImplementedException();
        }
    }
}
