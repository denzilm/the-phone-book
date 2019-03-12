namespace ThePhoneBook.WebApi.Api.Helpers
{
    /// <summary>
    /// Describes paging information
    /// </summary>
    public class PagingRequest
    {
        /// <summary>
        /// The current page
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of items per page
        /// </summary>
        public int PageSize { get; set; } = 4;
    }
}