﻿using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    class ReportElementRepo : BaseRepository<ReportElement>, IReportElementRepo
    {
        public ReportElementRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override async Task<ReportElement> GetById(int id)
        {
            var reportElement =  await context.ReportElements
                .Include(re => re.Sensor)
                    .ThenInclude(s => s.SensorType)
                .Include(re => re.Dashboard)
                .FirstOrDefaultAsync(re => re.Id == id);

            return reportElement;
        }

        public async Task<bool> ReportElementExist(ReportElement reportElement)
        {
            var res = await context.ReportElements
                .FirstOrDefaultAsync(re => re.SensorId == reportElement.SensorId && re.DashboardId==reportElement.DashboardId 
                                     && re.Type == reportElement.Type && re.Hours==reportElement.Hours);
            if (res != null)
                return true;
            return false;
        }
    }
}
