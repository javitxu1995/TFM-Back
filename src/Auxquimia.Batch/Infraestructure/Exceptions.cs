using System;
namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InternalErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        public InternalErrorException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción.</param>
        /// <param name="innerException">La excepción que es la causa de la excepción actual o una referencia nula (Nothing en Visual Basic) si no se especifica ninguna excepción interna.</param>
        public InternalErrorException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class IOException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IOException"/> class.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        public IOException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOException"/> class.
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción.</param>
        /// <param name="innerException">La excepción que es la causa de la excepción actual o una referencia nula (Nothing en Visual Basic) si no se especifica ninguna excepción interna.</param>
        public IOException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class SqlException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlException"/> class.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        public SqlException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlException"/> class.
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción.</param>
        /// <param name="innerException">La excepción que es la causa de la excepción actual o una referencia nula (Nothing en Visual Basic) si no se especifica ninguna excepción interna.</param>
        public SqlException(string message, Exception innerException) : base(message, innerException) { }
    }
}
