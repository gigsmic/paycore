using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AdventureWorksRest.Models
{
    public class PurchasesInTimeFrame
    {
        public decimal TotalTrafficSum { get { return Purchases.Sum(p => p.TrafficSum); }  }
        public decimal TotalUnitsSold { get { return Purchases.Sum(p => p.UnitsSold); } }
        public IEnumerable<PurchasesGrouped> Purchases { get; set; }
    }
}
