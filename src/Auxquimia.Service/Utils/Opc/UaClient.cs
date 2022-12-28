namespace Auxquimia.Utils
{
    using Auxquimia.Exceptions;
    using global::Opc.Ua;
    using global::Opc.Ua.Client;
    using global::Opc.Ua.Configuration;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="UAClient" />.
    /// </summary>
    public class UaClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        private ApplicationConfiguration configuration { get; set; }

        /// <summary>
        /// Gets or sets the applicationInstance.
        /// </summary>
        private ApplicationInstance applicationInstance { get; set; }

        /// <summary>
        /// Gets or sets the ServerUrl.
        /// </summary>
        internal string ServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the selectedEndpoint.
        /// </summary>
        private EndpointDescription selectedEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        private Session session { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UaClient"/> class.
        /// </summary>
        /// <param name="serverUrl">The serverUrl<see cref="string"/>.</param>
        /// <param name="config">The config<see cref="ApplicationConfiguration"/>.</param>
        /// <param name="appInstance">The appInstance<see cref="ApplicationInstance"/>.</param>
        public UaClient(string serverUrl, ApplicationConfiguration config, ApplicationInstance appInstance)
        {
            this.ServerUrl = serverUrl;
            this.configuration = config;
            this.applicationInstance = appInstance;
            this.selectedEndpoint = createEndpoint();
            Connect();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UaClient"/> class.
        /// </summary>
        /// <param name="serverUrl">The serverUrl<see cref="string"/>.</param>
        public UaClient(string serverUrl)
        {
            this.ServerUrl = serverUrl;
            this.configuration = CreateDefaultConfiguration();
            this.applicationInstance = CreateDefaultApplicationInstance("AuxquimiaOPCClientInstance", this.configuration);
            this.selectedEndpoint = createEndpoint();
            Connect();
        }

        /// <summary>
        /// The CreateDefaultConfiguration.
        /// </summary>
        /// <returns>The <see cref="ApplicationConfiguration"/>.</returns>
        private ApplicationConfiguration CreateDefaultConfiguration()
        {
            ApplicationConfiguration config = new ApplicationConfiguration()
            {
                ApplicationName = "AuxquimiaOPCClient",
                ApplicationUri = Utils.Format(@"urn:{0}:AuxquimiaOPCClient", System.Net.Dns.GetHostName()),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = "AuxquimiaOPCClient" },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates" },
                    AutoAcceptUntrustedCertificates = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                TraceConfiguration = new TraceConfiguration()
            };
            config.Validate(ApplicationType.Client).GetAwaiter().GetResult();
            if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
            }
            return config;
        }

        /// <summary>
        /// The CreateDefaultApplicationInstance.
        /// </summary>
        /// <param name="applicationName">The applicationName<see cref="string"/>.</param>
        /// <param name="config">The config<see cref="ApplicationConfiguration"/>.</param>
        /// <returns>The <see cref="ApplicationInstance"/>.</returns>
        private ApplicationInstance CreateDefaultApplicationInstance(string applicationName, ApplicationConfiguration config)
        {
            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = applicationName,
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = config
            };
            application.CheckApplicationInstanceCertificate(false, 2048).GetAwaiter().GetResult();

            return application;
        }

        /// <summary>
        /// The createEndpoint.
        /// </summary>
        /// <returns>The <see cref="EndpointDescription"/>.</returns>
        private EndpointDescription createEndpoint()
        {
            EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(this.ServerUrl, useSecurity: true);
            return selectedEndpoint;
        }

        /// <summary>
        /// The Connect.
        /// </summary>
        private void Connect()
        {
            Console.WriteLine("Connecting...");
            Console.WriteLine($"Creating a session with your server: {selectedEndpoint.EndpointUrl} ");
            try
            {
                this.session = Session.Create(
                this.configuration,
                new ConfiguredEndpoint(null, this.selectedEndpoint, EndpointConfiguration.Create(this.configuration)),
                false,
                "",
                60000,
                null,
                null
                ).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw new CustomException(Constants.Opc.Errors.ERROR_CONNECTING);
            }
        }

        /// <summary>
        /// The Disconnect.
        /// </summary>
        private void Disconnect()
        {
            try
            {
                if (session != null)
                {
                    session.Close();
                    session.Dispose();
                    session = null;
                }
            }
            catch (Exception ex)
            {
                // Log Error
                throw new CustomException(Constants.Opc.Errors.ERROR_DISCONNECTING);
            }
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// The ReadNodes.
        /// </summary>
        /// <param name="nodeNames">The nodeNames<see cref="IList{string}"/>.</param>
        /// <returns>The <see cref="Dictionary{string, object}"/>.</returns>
        public Dictionary<string, object> ReadNodes(IList<string> nodeNames)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (session == null || session.Connected == false)
            {

                throw new CustomException(Constants.Opc.Errors.NOT_CONNECTED);
            }


            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            foreach (string nodename in nodeNames)
            {
                nodesToRead.Add(new ReadValueId() { NodeId = new NodeId(nodename), AttributeId = Attributes.Value });
            }

            session.Read(
                   null,
                   0,
                   TimestampsToReturn.Both,
                   nodesToRead,
                   out DataValueCollection resultsValues,
                   out DiagnosticInfoCollection diagnosticInfos);

            // Display the results.
            DataValue dataValue;
            DataValue[] dataValueArray = resultsValues.ToArray();
            int it = 0;
            foreach (string nodeName in nodeNames)
            {
                dataValue = dataValueArray[it];
                result.Add(nodeName, dataValue.Value);
                it++;
            }

            return result;
        }

        /// <summary>
        /// The ReadNode.
        /// </summary>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private object ReadNode(string nodeName)
        {
            if (session == null || session.Connected == false)
            {

                throw new CustomException(Constants.Opc.Errors.IDEX_OUT_OF_BOUNDS);
            }
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            nodesToRead.Add(new ReadValueId() { NodeId = new NodeId(nodeName), AttributeId = Attributes.Value });
            session.Read(
                  null,
                  0,
                  TimestampsToReturn.Both,
                  nodesToRead,
                  out DataValueCollection resultsValues,
                  out DiagnosticInfoCollection diagnosticInfos);
            if (resultsValues.Count > 0)
            {
                DataValue readValue = resultsValues.ToArray()[0];
                return readValue.Value;
            }
            return null;
        }

        /// <summary>
        /// The ReadNodeArray.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        private T ReadNodeArray<T>(string nodeName, int index)
        {

            object readValue = ReadNode(nodeName);
            T value = default(T);
            if (readValue != null)
            {
                if (readValue is T[])
                {
                    T[] valueArray = (T[])readValue;
                    if (index >= valueArray.Length)
                    {


                        throw new CustomException(Constants.Opc.Errors.IDEX_OUT_OF_BOUNDS);
                    }
                    value = valueArray[index];
                }
                else
                {
                    value = (T)readValue;
                }
            }
            return value;
        }

        /// <summary>
        /// The ReadValueFromArray.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public T ReadValueFromArray<T>(string nodeName, int index)
        {
            T value = ReadNodeArray<T>(nodeName, index);
            if (value != null)
            {
                T tValue = (T)value;
                return tValue;
            }
            return default(T);
        }

        /// <summary>
        /// The Read.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public T Read<T>(string nodeName)
        {
            object value = ReadNode(nodeName);
            if (value != null)
            {
                T tValue = (T)value;
                return tValue;
            }
            return default(T);
        }

        /// <summary>
        /// The WriteNode.
        /// </summary>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private object WriteNode(string nodeName, object value)
        {
            object valueRead = null;
            if (session == null || session.Connected == false)
            {

                throw new CustomException(Constants.Opc.Errors.IDEX_OUT_OF_BOUNDS);
            }
            WriteValueCollection nodesToWrite = new WriteValueCollection();
            nodesToWrite.Add(new WriteValue() { NodeId = new NodeId(nodeName), AttributeId = Attributes.Value, Value = new DataValue() { Value = value } });
            session.Write(
                null,
                nodesToWrite,
                out StatusCodeCollection statusCodes,
                out DiagnosticInfoCollection diagnosticInfos
                );
            if (statusCodes.Count > 0)
            {
                valueRead = Read<object>(nodeName);
            }
            return valueRead;
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public T Write<T>(string nodeName, T value)
        {
            T valueWrited;
            valueWrited = (T)WriteNode(nodeName, value);
            return valueWrited;
        }

        /// <summary>
        /// The WriteOnArray.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="nodeName">The nodeName<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public T WriteOnArray<T>(string nodeName, T value, int index)
        {
            T[] array = Read<T[]>(nodeName);
            if (array.Length > 0 && array.Length > index && index > 0)
            {
                array[index] = value;
            }
            T[] arrayWrited = Write<T[]>(nodeName, array);
            return arrayWrited[index];
        }
    }
}
