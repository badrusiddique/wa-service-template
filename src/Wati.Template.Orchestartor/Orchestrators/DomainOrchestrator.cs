using AutoMapper;
using Wati.Template.Common.Dtos.Request;

namespace Wati.Template.Service.Services.Interfaces
{
    public class DomainOrchestrator : IDomainOrchestrator
    {
        private readonly IMapper _mapper;
        private readonly IDomainService _brokerService;

        public DomainOrchestrator(IMapper mapper, IDomainService brokerService)
        {
            _mapper = mapper;
            _brokerService = brokerService;
        }

        #region Public Methods

        public async ValueTask<IEnumerable<DomainResponseDto>> GetAllDomainsAsync()
        {
            var models = await _brokerService.GetAllDomainsAsync();
            return _mapper.Map<IEnumerable<DomainResponseDto>>(models);
        }

        #endregion
    }
}
