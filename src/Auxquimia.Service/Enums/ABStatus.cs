namespace Auxquimia.Enums
{
    using System;

    /// <summary>
    /// Defines the WoStatus.
    /// </summary>
    public enum ABStatus : Byte
    {
        [StringValue("NONE")]
        NONE = 0,
        [StringValue("PENDING")]
        PENDING = 1,
        [StringValue("WAITING")]
        WAITING = 2,
        [StringValue("PROGRESS")]
        PROGRESS = 3,
        [StringValue("FINISHED")]
        FINISHED = 4
    }
}
