using System.Collections.Generic;

namespace CineAPI.ViewModels
{
    public class PaginationViewModel<T> where T : class
    {
        public int Page { get; set; }

        public int LimitPage { get; set; }

        public int TotalPages { get; set; }

        public ICollection<T> Data { get; set; }
    }
}