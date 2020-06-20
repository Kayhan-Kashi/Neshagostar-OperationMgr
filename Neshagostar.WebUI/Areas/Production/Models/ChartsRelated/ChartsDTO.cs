using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Production.Models.ChartsRelated
{
    public class ChartsDTO
    {
        public Guid SaloonId { get; set; }
        public Guid DeviceId { get; set; }
        public Guid ProductionLineId { get; set; }

        public string Date { get; set; }
        public double WastageAmount { get; set; }
        public double ConsumedAmount { get; set; }
        public double MaxConsumed { get; set; }
        public double MaxWastageAmount { get; set; }
        public double MinConsumed { get; set; }
        public double MinWastage { get; set; }
        public double AverageConsumed { get; set; }
        public double AverageWastage { get; set; }
        public double TotalMaterialConsumed { get; set; }
        public double TotalWastage { get; set; }
        

    }
}