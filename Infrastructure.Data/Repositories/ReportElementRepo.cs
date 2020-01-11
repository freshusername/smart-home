using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ReportElementRepo : BaseRepository<ReportElement>, IReportElementRepo
    {
        public ReportElementRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override async Task<ReportElement> GetById(int id)
        {
            var reportElement = await context.ReportElements
                .Include(re => re.Sensor)
                .Include(re => re.Dashboard)
                .FirstOrDefaultAsync(re => re.Id == id);
            return reportElement;
        }
    }
}
