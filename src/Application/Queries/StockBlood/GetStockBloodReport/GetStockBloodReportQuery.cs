using MediatR;

namespace Application.Queries.StockBlood.GetStockBloodReport
{
    public class GetStockBloodReportQuery : IRequest<GetStockBloodReportQueryResponse>
    {
        public GetStockBloodReportQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
