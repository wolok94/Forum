namespace Forum.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchPhrase { get; set; }
        public PaginationFilter()
        {

        }

        public PaginationFilter(int pageNumber, int pageSize, string? searchPhrase)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 1 ? 1 : pageSize;
            this.SearchPhrase = searchPhrase;
        }
    }
}
