using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksRest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdventureWorksRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AdventureWorks2012Context _context;

        public ProductController(ILogger<ProductController> logger, AdventureWorks2012Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Product> Get(string name, string description, DateTime? sellStartDate, int page = 1, int pageSize = 20)
        {
            if(page < 1 || pageSize < 1)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var products = _context.Product.AsEnumerable();
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            
            if (sellStartDate != null)
            {
                products = products.Where(p => p.SellStartDate.Equals(sellStartDate));
            }

            if (!string.IsNullOrEmpty(description))
            {
                var descriptionIds = (from des in _context.ProductDescription.AsEnumerable() where des.Description.Contains(description, StringComparison.OrdinalIgnoreCase) select des.ProductDescriptionId).ToHashSet();
                var productModelIds = (from pmpd in _context.ProductModelProductDescriptionCulture.AsEnumerable() where descriptionIds.Contains(pmpd.ProductDescriptionId) select pmpd.ProductModelId).ToHashSet();
                products = products.Where(p => p.ProductModelId.HasValue && productModelIds.Contains(p.ProductModelId.Value));
            }

            Helpers.AddPaginationMeta(Response.Headers, page, pageSize, products);
            if (page > 1)
            {
                products = products.Skip((page - 1) * pageSize);
            }

            return products.Take(pageSize).ToList();
        }
    }
}
