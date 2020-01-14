using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class IconRepo : BaseRepository<Icon>, IIconRepo
    {
        public IconRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public async override Task<Icon> GetById(int id)
        {
            return await context.Icons.FindAsync(id);
        }
    }
}