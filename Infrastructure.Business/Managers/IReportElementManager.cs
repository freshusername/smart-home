using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IReportElementManager
    {
        Task<ReportElement> GetById(int id);
        Task<WordCloudDTO> GetWordCloudBySensorId(int ReportElementId);
        void EditWordCloud(ReportElement reportElement);
    }
}
