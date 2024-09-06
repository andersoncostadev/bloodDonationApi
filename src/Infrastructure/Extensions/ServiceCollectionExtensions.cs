using Application.Commands.v1.Donation.Create;
using Application.Commands.v1.Donor.Create;
using Application.Commands.v1.Donor.Delete;
using Application.Commands.v1.Donor.Update;
using Application.Interfaces;
using Application.Profiles;
using Application.Queries.Address.GetPostalCode;
using Application.Queries.Donor.GetById;
using Application.Queries.Donor.GetFullName;
using Application.UseCases;
using Domain.Events;
using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DonorProfile),
                                 typeof(AddressProfile),
                                 typeof(DonationProfile),
                                 typeof(StockBloodProfile));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDonorRepository, DonorRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IDonationRepository, DonationRepository>();
            services.AddScoped<IStockBloodRepository, StockBloodRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDonorUseCases, DonorUseCases>();
            services.AddScoped<IAddressUseCases, AddressUseCases>();
            services.AddScoped<IDonationUseCases, DonationUseCases>();
            services.AddScoped<IStockBloodUseCases, StockBloodUseCases>();

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IDomainEventHandler<BloodStockLowEvent>, BloodStockLowEventHandler>();
            services.AddScoped<IEmailService, SmtpEmailService>();


            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateDonorCommand>, CreateDonorValidator>();
            services.AddScoped<IValidator<DeleteDonorCommand>, DeleteDonorValidation>();
            services.AddScoped<IValidator<UpdateDonorCommand>, UpdateDonorValidator>();
            services.AddScoped<IValidator<GetByIdDonorQuery>, GetByIdDonorValidator>();
            services.AddScoped<IValidator<GetByFullNameQuery>, GetByFullNameValidator>();

            services.AddScoped<IValidator<GetPostalCodeQuery>, GetPostalCodeValidator>();

            services.AddScoped<IValidator<CreateDonationCommand>, CreateDonationValidator>();

            return services;
        }

        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDonorCommand).Assembly));

            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddHttpClient<IPostalCodeService, PostalCodeService>();

            return services;
        }
    }
}
