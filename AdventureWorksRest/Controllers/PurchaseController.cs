using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksRest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdventureWorksRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly AdventureWorks2012Context _context;

        public PurchaseController(ILogger<PurchaseController> logger, AdventureWorks2012Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public PurchasesInTimeFrame Get(DateTime? startTime, DateTime? endTime, int page = 1, int pageSize = 20)
        {
            if (page < 1 || pageSize < 1)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var purchases = _context.PurchaseOrderDetail.AsEnumerable();

            if (startTime != null)
            {
                purchases = purchases.Where(p => p.DueDate >= startTime);
            }

            if (endTime != null)
            {
                purchases = purchases.Where(p => p.DueDate <= endTime);
            }

            var purchasesGrouped = purchases.GroupBy(p => p.DueDate, (k, g) => { var trafficSum = g.Sum(pod => pod.LineTotal); var unitsSold = g.Sum(pod => pod.OrderQty); return new PurchasesGrouped() { DueDate = k, TrafficSum = trafficSum, UnitsSold = unitsSold };});

            Helpers.AddPaginationMeta(Response.Headers, page, pageSize, purchasesGrouped);
            if (page > 1)
            {
                purchasesGrouped = purchasesGrouped.Skip((page - 1) * pageSize);
            }

            return new PurchasesInTimeFrame() { Purchases = purchasesGrouped.Take(pageSize).ToList() };
        }
    }
}
