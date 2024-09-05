using MediatR;

namespace Application.Queries.Address.GetPostalCode
{
    public class GetPostalCodeQuery : IRequest<GetPostalCodeQueryResponse>
    {
        public GetPostalCodeQuery(string postalCode)
        {
            PostalCode = postalCode;
        }

        public string PostalCode { get; set; }
    }
}
