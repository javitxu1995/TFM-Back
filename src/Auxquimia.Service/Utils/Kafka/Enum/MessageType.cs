namespace Auxquimia.Utils.Kafka.Enum
{
    using System;

    public enum MessageType : Byte
    {
        [StringValue("CONFIGURATION")]
        CONFIGURATION,
        [StringValue("PRODUCTION")]
        PRODUCTION,
        [StringValue("INITIALIZAION")]
        INITIALIZATION,
        [StringValue("PROCESS_CONTROL")]
        PROCESS_CONTROL
    }
}
