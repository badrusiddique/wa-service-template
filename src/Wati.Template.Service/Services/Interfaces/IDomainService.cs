using Wati.Template.Common.Dtos.Request;

namespace Wati.Template.Service.Services.Interfaces;

public interface IDomainService
{
    ValueTask<IEnumerable<DomainModel>> GetAllDomainsAsync();
}