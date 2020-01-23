using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Services
{
    public class ActionService : IActionService
    {

        private readonly IUnitOfWork _db;

        public ActionService(IUnitOfWork _unitOfWork)
        {
            _db = _unitOfWork;
        }

        public async Task<OperationDetails> CheckStatus(Guid token)
        {
            var data = await _db.SensorControlRepo.GetByToken(token);          
             if (data == null || data.Count() == 0) return new OperationDetails(false, "", "");
           
                foreach (var item in data)
                { 
                    if(item.IsActive)
                    {
                      var result = Verification(item.SensorId, item.minValue,item.maxValue);
                       if (result) return new OperationDetails(true, "", "");
                    }
                }
                     
            return new OperationDetails(false , "" , "");
        }

        private bool Verification(int sensorId, int? minValue, int? maxValue)
        {
            var sensor = _db.SensorRepo.GetById(sensorId).Result;
             var seconds = DateTimeOffset.Now.AddSeconds(-4);
            var period = 5;

            switch (sensor.SensorType.MeasurementType)
            {
                case MeasurementType.Bool:
                    var lastHistory = _db.HistoryRepo.GetLastHistoryBySensorIdAndDate(sensorId, seconds);
                     if (lastHistory == null) return false;

                    if (lastHistory.BoolValue.Value) return true;
                break;

                case MeasurementType.Int:
                    if (minValue != null && maxValue != null)
                    {
                        var min = _db.HistoryRepo.GetIntMinValueForPeriod(sensorId, period);
                         if (min == null) return false;

                        var max = _db.HistoryRepo.GetIntMaxValueForPeriod(sensorId, period);
                         if (min == null) return false;

                        if (min.Value <= minValue || max.Value >= maxValue) return true;
                    }
                    else if (minValue != null)
                    {
                        var min = _db.HistoryRepo.GetIntMinValueForPeriod(sensorId, period);
                         if (min == null) return false;

                        if (min.Value <= minValue) return true;
                    }
                    else if (maxValue != null)
                    {
                        var max = _db.HistoryRepo.GetIntMaxValueForPeriod(sensorId, period);
                         if (max == null) return false;

                        if (max.Value <= minValue) return true;
                    }
                break;
            }

            return false;
        }
    }
}
