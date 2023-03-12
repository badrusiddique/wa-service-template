using EnsureThat;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wati.Template.Common.Configurations;
using Wati.Template.Common.Dtos.Response;
using Wati.Template.Common.Dtos.ThirdParty;
using Wati.Template.Repository.Repositories;

namespace Wati.Template.Repository.Contexts.Interfaces
{
    public class  ThirdPartyDataContext : IThirdPartyDataContext
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ThirdPartyDataContext> _logger;

        public ThirdPartyDataContext(
            HttpClient httpClient,
            ILogger<ThirdPartyDataContext> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        

        public async ValueTask<DomainDto> GetEmployeeByUserNameAsync(string userName)
        {
            EnsureArg.IsNotNullOrEmpty(userName, nameof(userName), o => o.WithMessage($"invalid employee user-name: {userName}"));

            try
            {
                var option = new RequestConfiguration { Uri = $"base-url/{userName}" };
                return (await _httpClient.GetAsync<ApiResponseDto<DomainDto>>(option))?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while GetEmployeeByUserNameAsync : {ex.Message}", JsonConvert.SerializeObject(ex));
                return default;
            }
        }
    }
}
