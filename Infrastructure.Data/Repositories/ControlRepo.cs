using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ControlRepo : BaseRepository<Control>, IControlRepo
    {
        public ControlRepo(ApplicationsDbContext context) : base(context)
        {

        }      
    }
}