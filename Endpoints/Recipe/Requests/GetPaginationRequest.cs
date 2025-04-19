namespace FarmationRecipe.Endpoints.Recipe.Requests
{
    public class GetPaginationRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetPaginationRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public GetPaginationRequest()
        {
        }
    }
}
