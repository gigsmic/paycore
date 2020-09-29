using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksRest.Models
{
    public partial class PurchasesGrouped
    {
        public DateTime? DueDate { get; set; }
        public decimal TrafficSum { get; set; }
        public decimal UnitsSold { get; set; }
    }

}
