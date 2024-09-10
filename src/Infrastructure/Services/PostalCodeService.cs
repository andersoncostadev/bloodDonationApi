using Domain.Entities.v1;
using Domain.Services;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class PostalCodeService : IPostalCodeService
    {
        private readonly HttpClient _httpClient;

        public PostalCodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AddressEntity> GetAddressByPostalCodeAsync(string postalCode)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{postalCode}/json/");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var address = JsonConvert.DeserializeObject<ViaCepResponse>(content);

                if (address == null || string.IsNullOrEmpty(address.Cep))
                    return null;

                return new AddressEntity
                {
                    Street = address.Logradouro,
                    Neighborhood = address.Bairro,
                    City = address.Localidade,
                    State = address.Uf,
                    ZipCode = address.Cep
                };
            } catch(JsonReaderException)
            {
                return null;
            }
        }
    }
}
