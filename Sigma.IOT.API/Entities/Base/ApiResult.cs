namespace Sigma.IOT.API.Entities.Base
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<T>? Object { get; set; }
        public long TotalLines { get; set; }
        public decimal PageSize { get; set; }
        public decimal PageNumber { get; set; }
        public decimal TotalPages
        {
            get
            {
                decimal tPages = 0;

                if (PageSize > 0)
                    tPages = Math.Ceiling(TotalLines / PageSize);

                return tPages;
            }
        }

        public ApiResult<T> GetResult(bool invalid = false)
        {
            if (Object == null || Object.Count == 0)
            {
                Message = "No items found";
                Object = null;
                Success = false;
            }
            else
            {
                Success = !invalid;
            }

            return this;
        }
    }
}
