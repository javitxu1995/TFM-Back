namespace Auxquimia.Enums
{
    using System;

    public enum Unit : byte
    {

        [StringValue("")]
        NONE,

        [StringValue("KILOGRAM")]
        KILOGRAM,

        [StringValue("LITRE")]
        LITRE,

        [StringValue("GALON")]
        GALON,

        [StringValue("PIECE")]
        PIECE
    }
}
