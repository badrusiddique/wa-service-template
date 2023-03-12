using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wati.Template.Common.Dtos.Request;

namespace Wati.Template.Repository.Builders.Interfaces;

public interface IDomainDtoBuilder
{
    IDomainDtoBuilder WithName(string name);
    IDomainDtoBuilder WithDescription(string description);
    DomainResponseDto Create();
}