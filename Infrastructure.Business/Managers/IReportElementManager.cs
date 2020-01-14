﻿using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.SensorType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IReportElementManager
    {
        Task<ReportElement> GetById(int id);
        Task<WordCloudDTO> GetWordCloudById(int ReportElementId);
        Task<GaugeDto> GetGaugeById(int gaugeId);
        void EditWordCloud(WordCloudDTO wordCloud);
        Task<ReportElementDto> GetDataForSchedule(int id , int days);
    }
}
