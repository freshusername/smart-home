using Domain.Core.Model;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class ReportElementRepo : BaseRepository<ReportElement>, IReportElementRepo
    {
        public ReportElementRepo(ApplicationsDbContext context) : base(context)
        {

        }
    }
}
