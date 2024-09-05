using Application.Dtos;

namespace Application.Queries.StockBlood.GetStockBloodReport
{
    public class GetStockBloodReportQueryResponse
    {
        public GetStockBloodReportQueryResponse(IList<StockBloodDto>? stockBlood, int pageNumber, int pageSize, int totalPages)
        {
            StockBlood = stockBlood;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IList<StockBloodDto>? StockBlood { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
