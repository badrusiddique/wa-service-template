using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wati.Template.Common.Dtos.Request;
using Wati.Template.Common.Dtos.Response;
using Wati.Template.Service.Services.Interfaces;

namespace Wati.Template.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/domains")]
public class DomainsController
{
    private readonly IDomainOrchestrator _orchestrator;

    public DomainsController(IDomainOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    /// <summary>
    /// Get all domain records
    /// </summary>
    /// <remarks>
    ///
    ///     GET /api/domains
    ///
    /// </remarks>
    /// <returns>DomainResponseDto[]</returns>
    [HttpGet]
    public async ValueTask<ApiResponse<IEnumerable<DomainResponseDto>>> GetAllAsync()
    {
        var domains = await _orchestrator.GetAllDomainsAsync();
        return ApiResponse<IEnumerable<DomainResponseDto>>.ParseResponse(domains);
    }
}