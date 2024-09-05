using Application.Interfaces;
using MediatR;

namespace Application.Queries.StockBlood.GetStockBloodReport
{
    public class GetStockBloodReportQueryHandler : IRequestHandler<GetStockBloodReportQuery, GetStockBloodReportQueryResponse>
    {
        private readonly IStockBloodUseCases _stockBloodUseCases;

        public GetStockBloodReportQueryHandler(IStockBloodUseCases stockBloodUseCases)
        {
            _stockBloodUseCases = stockBloodUseCases;
        }

        public async Task<GetStockBloodReportQueryResponse> Handle(GetStockBloodReportQuery request, CancellationToken cancellationToken)
        {
            var stockBlood = await _stockBloodUseCases.GetBloodStockReportAsync(request.PageNumber, request.PageSize) 
                ?? throw new ApplicationException("Failed to get stock blood report");

            var totalCount = stockBlood.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new GetStockBloodReportQueryResponse(stockBlood, request.PageNumber, request.PageSize, totalPages);
        }
    }
}
