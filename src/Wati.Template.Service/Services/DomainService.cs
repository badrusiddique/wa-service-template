using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wati.Template.Common.Dtos.Request;
using Wati.Template.Data.Entities;
using Wati.Template.Repository.Repositories.Interfaces;

namespace Wati.Template.Service.Services.Interfaces
{
    public class DomainService :IDomainService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain> _repository;

        public DomainService(IMapper mapper, IRepository<Domain> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        

        public async ValueTask<IEnumerable<DomainModel>> GetAllDomainsAsync()
        {
            var domains = await _repository.All().ToListAsync();

            return _mapper.Map<IEnumerable<DomainModel>>(domains);
        }
    }
}
