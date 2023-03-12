using Wati.Template.Common.Dtos.Request;

namespace Wati.Template.Service.Services.Interfaces;

public interface IDomainOrchestrator
{
    ValueTask<IEnumerable<DomainResponseDto>> GetAllDomainsAsync();
}