using MediatR;

namespace Application.Queries.Donor.GetById
{
    public class GetByIdDonorQuery : IRequest<GetByIdDonorQueryResponse>
    {
        public GetByIdDonorQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
