namespace Auxquimia.Dto.Business.Formulas
{
    using Auxquimia.Dto.Authentication;
    using System;

    /// <summary>
    /// Defines the <see cref="FormulaStepDto" />.
    /// </summary>
    [Serializable]
    public class FormulaStepDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public FormulaDto Formula { get; set; }

        /// <summary>
        /// Gets or sets the Step.
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Gets or sets the SetPoint.
        /// </summary>
        public float SetPoint { get; set; }

        /// <summary>
        /// Gets or sets the ItemCode.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the ItemName.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the InventoryLot.
        /// </summary>
        public string InventoryLot { get; set; }

        /// <summary>
        /// Gets or sets the BlenderPercentaje.
        /// </summary>
        public Int16 BlenderPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the RealWeight.
        /// </summary>
        public float RealWeight { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public long StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        public long EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        public string Username { get; set; }


        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public UserDto Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Written.
        /// </summary>
        public bool Written { get; set; }

        /// <summary>
        /// Gets or sets if the product is water or not
        /// </summary>
        public bool IsWater { get; set; }

        /// <summary>
        /// The CheckStep.
        /// </summary>
        public void CheckStep()
        {
            BlenderPercentaje = (Int16)(BlenderPercentaje > 100 ? 100 : BlenderPercentaje);
            BlenderPercentaje = (Int16)(BlenderPercentaje < 0 ? 0 : BlenderPercentaje);
        }
    }
}
