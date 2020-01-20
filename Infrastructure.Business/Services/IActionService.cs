using Domain.Core.Model.Enums;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Services
{
    public interface IActionService
    {
        OperationDetails IsActive(Guid token, ActionRole role);
    }
}
