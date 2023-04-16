using System.Collections.Generic;

namespace Utilities.Pagination
{
    public class PaginationDataModel<T> where T : class
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> List { get; set; }
    }
}
