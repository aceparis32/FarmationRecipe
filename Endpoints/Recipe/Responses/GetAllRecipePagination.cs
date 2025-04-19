namespace FarmationRecipe.Endpoints.Recipe.Responses
{
    public class GetAllRecipePagination
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public GetAllRecipePagination(int totalCount, int pageSize, int pageNumber)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
