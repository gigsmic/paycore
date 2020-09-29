using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksRest
{
    public static class Helpers
    {
        public static void AddPaginationMeta(IHeaderDictionary headers, int page, int pageSize, IEnumerable<object> collection)
        {
            var count = collection.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var metadata = new
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                HasNext = page < totalPages,
                HasPrevious = page > 1
            };

            headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        }
    }
}
