namespace Auxquimia.Dto.Business.Opc
{
    using Auxquimia.Utils;
    using System;

    /// <summary>
    /// Defines the <see cref="OpcOrder" />.
    /// </summary>
    [Serializable]
    internal class OpcOrder : ICloneable
    {
        /// <summary>
        /// Gets or sets the AssemblyNumber.
        /// </summary>
        public string AssemblyNumber { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyId.
        /// </summary>
        public string AssemblyId { get; set; }

        /// <summary>
        /// Gets or sets the OpcServer.
        /// </summary>
        public string OpcServer { get; set; }

        /// <summary>
        /// Gets or sets the FactoryName.
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// Gets or sets the DbEntrada.
        /// </summary>
        public int DbEntrada { get; set; }

        /// <summary>
        /// Gets or sets the DbSalida.
        /// </summary>
        public int DbSalida { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Finalized.
        /// </summary>
        public bool Finalized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Abort.
        /// </summary>
        public bool Abort { get; set; }

        /// <summary>
        /// Gets or sets the Entrada.
        /// </summary>
        public IN Entrada { get; set; } = new IN();

        /// <summary>
        /// Gets or sets the Salida.
        /// </summary>
        public OUT Salida { get; set; } = new OUT();

        /// <summary>
        /// The Clone.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// The InitAttributes.
        /// </summary>
        /// <param name="maxSize">The maxSize<see cref="int"/>.</param>
        public void InitAttributes(int maxSize)
        {
            this.Finalized = false;
            //Entrada
            this.Entrada.Agitador1Paso = DefaultArray<Int16>(maxSize, default(Int16));
            this.Entrada.ConsignaPesoPaso = DefaultArray<float>(maxSize, default(float));
            this.Entrada.LotePaso = DefaultArray<string>(maxSize, "0");
            this.Entrada.ProductoPaso = DefaultArray<string>(maxSize, "0");
            this.Entrada.ProductoRFIDPaso = DefaultArray<string>(maxSize, "0");
            this.Entrada.TiempoMTOPaso = DefaultArray<Int16>(maxSize, default(Int16));
            this.Entrada.TiempoMTOAgitador2UltimoPaso = default(Int16);
            this.Entrada.VelocidadAgitador2UltimoPaso = default(Int16);
            this.Entrada.NombrePaso = DefaultArray<string>(maxSize, "0");

            //Salida
            this.Salida.Agitador1Paso = DefaultArray<Int16>(maxSize, default(Int16));
            this.Salida.ConsignaPeso = DefaultArray<float>(maxSize, default(float));
            this.Salida.OperadorPaso = DefaultArray<string>(maxSize, "0");
            this.Salida.ProductoPaso = DefaultArray<string>(maxSize, "0");
            this.Salida.PesoPaso = DefaultArray<float>(maxSize, default(float));
            this.Salida.TiempoMTOPaso = DefaultArray<Int16>(maxSize, default(Int16));
            this.Salida.NºPasoRecetaCliente = default(Int16);
            this.Salida.HoraInicioDosificacion = default(long);
            this.Salida.HoraFinDosificacion = default(long);
            this.Salida.PesoTotalReactor = default(float);
            this.Salida.BotonConfirmacionDiferenteLote = false;
            this.Salida.BotonConfirmacionMismoLote = false;
            this.Salida.EstadoDosificacion = false;
            this.Salida.TiempoEventos = DefaultArray<long>(Constants.Opc.MAX_STEP_SIZE, default(long));
        }

        /// <summary>
        /// The DefaultArray.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <returns>The <see cref="T[]"/>.</returns>
        private T[] DefaultArray<T>(int size, T value)
        {
            T[] result = new T[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = value;
            }
            return result;
        }

        /// <summary>
        /// The DefaultMatrix.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="sizex">The sizex<see cref="int"/>.</param>
        /// <param name="sizey">The sizey<see cref="int"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <returns>The <see cref="T[,]"/>.</returns>
        private T[,] DefaultMatrix<T>(int sizex, int sizey, T value)
        {
            T[,] result = new T[sizex, sizey];
            for (int i = 0; i < sizex; i++)
            {
                for (int j = 0; j < sizey; j++)
                {
                    result[i, j] = value;
                }

            }
            return result;
        }

        /// <summary>
        /// Defines the <see cref="IN" />.
        /// </summary>
        public class IN
        {
            /// <summary>
            /// Gets or sets the NombreReceta.
            /// </summary>
            public string NombreReceta { get; set; }

            /// <summary>
            /// Gets or sets the ConsignaPesoPaso.
            /// </summary>
            public float[] ConsignaPesoPaso { get; set; }

            /// <summary>
            /// Gets or sets the ProductoRFIDPaso.
            /// </summary>
            public string[] ProductoRFIDPaso { get; set; }

            /// <summary>
            /// Gets or sets the Agitador1Paso.
            /// </summary>
            public Int16[] Agitador1Paso { get; set; }

            /// <summary>
            /// Gets or sets the TiempoMTOPaso.
            /// </summary>
            public Int16[] TiempoMTOPaso { get; set; }

            /// <summary>
            /// Gets or sets the TiempoMTOAgitador2UltimoPaso.
            /// </summary>
            public Int16 TiempoMTOAgitador2UltimoPaso { get; set; }

            /// <summary>
            /// Gets or sets the VelocidadAgitador2UltimoPaso.
            /// </summary>
            public Int16 VelocidadAgitador2UltimoPaso { get; set; }

            /// <summary>
            /// Gets or sets the NombrePaso.
            /// </summary>
            public string[] NombrePaso { get; set; }

            /// <summary>
            /// Gets or sets the ProductoPaso.
            /// </summary>
            public string[] ProductoPaso { get; set; }

            /// <summary>
            /// Gets or sets the LotePaso.
            /// </summary>
            public string[] LotePaso { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether FeedbackConfirmacionLote.
            /// </summary>
            public bool FeedbackConfirmacionLote { get; set; }
        }

        /// <summary>
        /// Defines the <see cref="OUT" />.
        /// </summary>
        public class OUT
        {
            /// <summary>
            /// Gets or sets the PesoPaso.
            /// </summary>
            public float[] PesoPaso { get; set; }

            /// <summary>
            /// Gets or sets the OperadorPaso.
            /// </summary>
            public string[] OperadorPaso { get; set; }

            /// <summary>
            /// Gets or sets the HoraInicioDosificacion.
            /// </summary>
            public long HoraInicioDosificacion { get; set; }

            /// <summary>
            /// Gets or sets the HoraFinDosificacion.
            /// </summary>
            public long HoraFinDosificacion { get; set; }

            /// <summary>
            /// Gets or sets the TiempoEventos.
            /// </summary>
            public long[] TiempoEventos { get; set; }

            /// <summary>
            /// Gets or sets the ProductoPaso.
            /// </summary>
            public string[] ProductoPaso { get; set; }

            /// <summary>
            /// Gets or sets the Agitador1Paso.
            /// </summary>
            public Int16[] Agitador1Paso { get; set; }

            /// <summary>
            /// Gets or sets the TiempoMTOPaso.
            /// </summary>
            public Int16[] TiempoMTOPaso { get; set; }

            /// <summary>
            /// Gets or sets the ConsignaPeso.
            /// </summary>
            public float[] ConsignaPeso { get; set; }

            /// <summary>
            /// Gets or sets the PesoTotalReactor.
            /// </summary>
            public float PesoTotalReactor { get; set; }

            /// <summary>
            /// Gets or sets the NºPasoRecetaCliente.
            /// </summary>
            public Int16 NºPasoRecetaCliente { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether BotonConfirmacionMismoLote.
            /// </summary>
            public bool BotonConfirmacionMismoLote { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether BotonConfirmacionDiferenteLote.
            /// </summary>
            public bool BotonConfirmacionDiferenteLote { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether EstadoDosificacion.
            /// </summary>
            public bool EstadoDosificacion { get; set; }
        }
    }
}
