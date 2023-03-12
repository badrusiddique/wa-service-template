using AutoMapper;
using Wati.Template.Common.Dtos.Request;
using Wati.Template.Data.Entities;

namespace Wati.Template.Repository.Mapper;

public class DomainMapper : Profile
{
    public DomainMapper()
    {
        // Database entity - Domain entity mapping
        CreateMap<Domain, DomainModel>()
            .ForMember(x => x.LastUpdatedAt, y => y.MapFrom(z => z.CreatedAt));

        // Domain entity mapping - View DTO mapping
        CreateMap<DomainQueryModel, DomainQueryModel>();
        CreateMap<DomainModel, DomainResponseDto>();
    }
}