using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Utils.FileStorage.Model
{
    public class AssemblyRecord
    {
        public AssemblyRecord()
        {
            this.AssemblyNumber = default(string);
            this.Date = default(string);
            this.Item = default(string);
            this.DisplayName = default(string);
            this.Quantity = default(string);
            this.Units = default(string);
            this.ComponentSequence = default(string);
            this.ComponentInventoryDetaylId = default(string);
            this.ComponentInventoryLot = default(string);
            this.BlenderNumber = default(string);
            this.OperatorNo = default(string);
            this.VelocidadAgitacion1 = default(string);
            this.VelocidadAgitacion2 = default(string);
            this.AgitationTime = default(string);
            this.Batchnumber = default(string);
            this.Temperature = default(string);
            this.RealWeight = default(string);
            this.StartDate = default(string);
            this.EndDate = default(string);

        }
        public string AssemblyNumber { get; set; }
        public string Date {get; set;}
        public string Item { get; set; }
        public string DisplayName { get; set; }
        public string Quantity { get; set; }
        public string Units { get; set; }
        public string ComponentSequence { get; set; }
        public string ComponentInventoryDetaylId { get; set; }
        public string ComponentInventoryLot { get; set;}
        public string BlenderNumber { get; set; }
        public string OperatorNo { get; set; }
        public string VelocidadAgitacion1 { get; set; }
        public string VelocidadAgitacion2 { get; set; }
        public string AgitationTime { get; set; }
        public string Batchnumber { get; set; }
        public string Temperature { get; set; }
        //Datos nuevos
        public string RealWeight { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
