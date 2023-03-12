namespace Wati.Template.Common.Dtos.Request;

public class DomainResponseDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}