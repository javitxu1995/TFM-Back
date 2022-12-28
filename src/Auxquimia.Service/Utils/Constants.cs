namespace Auxquimia.Utils
{
    public static class Constants
    {
        public static object Config { get; internal set; }

        public static class FormatStrings
        {
            public const string SHORT_DATE = "dd/MM/yyyy";
        }

        public static class Roles
        {
            public const string ADMINISTRATOR = "ADMINISTRATOR";
            public const string MANAGER = "MANAGER";
            public const string USER = "USER";
        }

        public static class Strings
        {
            public const string EMPTY_STRING = "";

            public const string WHITESPACE_STRING = " ";

            public const string SPANISH_CULTUREINFO = "ES-es";
        }

        public static class Exceptions
        {
            public const string ERROR_USER_NO_FACTORY = "toast.error-user-no-facotry";
            public const string ERROR_CODE_NOT_AVAILABLE = "toast.code-not-available";
        }

        public static class Opc
        {
            internal static class Errors
            {
                public const string ILLEGAL_STATE_ASSEMBLY = "opc.illegal-state-assembly";
                public const string IDEX_OUT_OF_BOUNDS = "opc.array_index_out_of_bounds";
                public const string MAX_STEP_LIMIT = "opc.max_step_limit";
                public const string STRING_EXCEEDS_64 = "opc.string-exceeds-64-characters";
                public const string STEP_NUMBER_ERROR = "opc.step-number-error";
                public const string ERROR_CONNECTING = "opc.error-connect";
                public const string ERROR_DISCONNECTING = "opc.error-disconnect";
                public const string NOT_CONNECTED = "opc.not-connected";
            }

            private const string SEPARATOR = "/";
            private const string OPC_SERVER_NAME = "SIMATIC.S7_1500";
            private const string NAMESPACE = "2";
            public const int MAX_STEP_SIZE = 51;

            //Input for Opc
            private const string INPUT_DATABASE = "98_DatosEntradaClienteOPC";
            private const string INPUT_FOLDER = INPUT_DATABASE + SEPARATOR + "DatosEntradaClienteOPC";
            public const string IN_AGITADOR_1_PASO = "ns=" + NAMESPACE + ";s=" + INPUT_FOLDER + SEPARATOR + "Agitador1Paso";
            public const string IN_AGITADOR_2_ULTIMO_PASO = "ns=" + NAMESPACE + ";s=" + INPUT_FOLDER + SEPARATOR + "Agitador2UltimoPaso";
            public const string IN_CONSIGNA_PESO_PASO = "ns=" + NAMESPACE + ";s=" + INPUT_FOLDER + SEPARATOR + "ConsignaPesoPaso";
            public const string IN_NOMBRE_RECETA = "ns=" + NAMESPACE + ";s=" + INPUT_FOLDER + SEPARATOR + "NombreReceta";
            public const string IN_PRODUCTO_PASO = "ns=" + NAMESPACE + ";s=" + INPUT_FOLDER + SEPARATOR + "ProductoPaso";
            public const string IN_TIEMPO_MTO_ULTIMO_PASO = "ns=" + NAMESPACE + ";s=" + INPUT_FOLDER + SEPARATOR + "TiempoMtoUltimoPaso";

            //Output for Opc
            private const string OUTPUT_DATABASE = "99_DatosSalidaServidorOPC";
            public const string OUT_AGITADOR_1_PASO = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "Agitador1Paso";
            public const string OUT_CONSIGNA_PESO = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "ConsignaPeso";
            public const string OUT_HORA_FIN_DOSIFICACION = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "HoraFinDosificacion";
            public const string OUT_HORA_INICIO_DOSIFICACION = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "HoraInicioDosificacion";
            public const string OUT_OPERADOR_PASO = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "OperadorPaso";
            public const string OUT_PESO_PASO = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "PesoPaso";
            public const string OUT_PRODUCTO_PASO = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "ProductoPaso";
            public const string OUT_TIEMPOS_EVENTOS = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "TiemposEventos";
            public const string OUT_N_PASO_RECETA_CLIENTE = "ns=" + NAMESPACE + ";s=" + OUTPUT_DATABASE + SEPARATOR + "NºPasoRecetaCliente";

            // JSON KEYs
            public const string KEY_PRODUCTO = "PRODUCT";
            public const string KEY_CONSIGNA = "CONSIGNA";
            public const string KEY_BLENDER = "BLENDER";
            public const string KEY_START_DATE = "START_DATE";
            public const string KEY_END_DATE = "END_DATE";
            public const string KEY_PESO_PASO = "PESO_PASO";
            public const string KEY_OPERADOR = "OPERADOR";
            public const string KEY_STEP_NUMBER = "STEP";
        }
        public static class Ftp
        {
            //Assembly keys
            public const string ASSEMBLY_NUMBER = "assembly_number";
            public const string ASSEMBLY_LOAD_DATE = "assembly_load_date";
            public const string ASSEMBLY_BLENDER = "assembly_blender";

            //NetsuiteStep keys
            public const string STEP_ITEM_CODE = "step_item_code";
            public const string STEP_ITEM_DISPLAY_NAME = "step_item_display_name";
            public const string STEP_ITEM_QTY_REQUIRED = "step_item_qty_required";
            public const string STEP_ITEM_UNITS = "step_item_units";
            public const string STEP_NUMBER = "step_number";
            public const string STEP_INVENTORY_DETAIL = "step_inventory_detail";
            public const string STEP_INVENTORY_LOT = "step_inventory_lot";
            public const string STEP_OPERATOR = "step_operator";
            public const string STEP_BLENDER_SPEED1 = "step_blending_speed_1";
            public const string STEP_BLENDER_SPEED2 = "step_blending_speed_2";
            public const string STEP_BLENDING_TIME = "step_bledning_time";
            public const string STEP_BATCH_NUMBER = "step_batch_number";
            public const string STEP_TEMPERATURE = "step_temperature";
   

            internal static class Errors
            {
                public const string READER_INITIALIZE_ERROR = "ftp.reader-initialize-error";
                public const string PARSING_ERROR = "ftp.parsing-error";
                public const string REACTOR_CODE_ERROR = "ftp.error-reading-reactor-code";
            }
        }

        public static class Kafka
        {
            internal class Configuration
            {
                public const int LOOP_MILLISECONDS = 15000;
                public const int CONSUME_MAX_TIME_MILLISECONDS = 5000;
                public const string SATELLITE_EXTENSION_TOPIC = "_SAT";
                public const string AUXQUIMIA_EXTENSION_TOPIC = "_AXQ";
            }
            internal class Errors
            {
                public const string PRODUCER_NOT_INITIALIZED = "kafka.producer-not-initialized";
                public const string CONSUMER_NOT_INITIALIZED = "kafka.consumer-not-initialized";
                public const string MESSAGE_SEND_NULL = "kafka.message-send-null";
                public const string SERIALIZE_ERROR = "kafka.error-serialize";
                public const string DESERIALIZE_ERROR = "kafka.error-deserialize";
            }
        }
    }
}
