using Domain.Core.Model;
using System;

namespace Domain.Interfaces.Repositories
{
	public interface IControlRepo : IGenericRepository<Control>
    {
        Control GetByToken(Guid token);
    }
}
