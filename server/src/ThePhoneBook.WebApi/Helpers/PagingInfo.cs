using System;

namespace ThePhoneBook.WebApi.Helpers
{
    public class PagingInfo
    {
        public PagingInfo(int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public int PageSize { get; }
    }
}