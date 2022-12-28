namespace Auxquimia.Filters.Business.Formulas
{
    using System;

    /// <summary>
    /// Defines the <see cref="InventoryLotSearchFilter" />.
    /// </summary>
    public class InventoryLotSearchFilter
    {
        /// <summary>
        /// Gets or sets the Lot.
        /// </summary>
        public long Lot { get; set; }

        /// <summary>
        /// Gets or sets the StepId.
        /// </summary>
        public Guid StepId { get; set; }
    }
}
