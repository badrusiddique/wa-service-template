using Wati.Template.Common.Dtos.ThirdParty;

namespace Wati.Template.Repository.Contexts.Interfaces;

public interface IThirdPartyDataContext
{
    ValueTask<DomainDto> GetEmployeeByUserNameAsync(string userName);
}