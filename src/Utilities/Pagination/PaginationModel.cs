namespace Utilities.Pagination
{
    public class PaginationModel
    {
        private int? _pageSize;
        private int _pageIndex;

        public int? PageSize
        {
            get
            {
                return _pageSize;
            } 
            set
            {
                _pageSize = value.HasValue && value.Value >= 0 ? value : default;
            }
        }

        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value > 0 ? value : 1;
            }
        }
    }
}
