using Domain.Core.Model;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class IconRepo : BaseRepository<Icon>, IIconRepo
    {
        public IconRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override Icon GetById(int id)
        {
            return context.Icons.Find(id);
        }
    }
}