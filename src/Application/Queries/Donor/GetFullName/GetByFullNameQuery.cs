using MediatR;

namespace Application.Queries.Donor.GetFullName
{
    public class GetByFullNameQuery : IRequest<GetByFullNameQueryResponse>
    {
        public GetByFullNameQuery(string? fullName)
        {
            FullName = fullName;
        }

        public string? FullName { get; set; }
    }
}
