using Application.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class CreateDonorCommandHandlerTest
    {
        private readonly Mock<IDonorUseCases> _donorUseCasesMock = new();
    }
}
