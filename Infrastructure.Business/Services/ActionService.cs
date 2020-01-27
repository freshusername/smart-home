using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.Infrastructure;
using System;
using System.Linq;
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
                if (item.IsActive && item.Sensor.SensorType.MeasurementType == MeasurementType.Bool)
                {
                    var result = BoolVerification(item.SensorId.Value);
                    if (result) return new OperationDetails(true, "", "");
                }
                else if (item.IsActive && item.Sensor.SensorType.MeasurementType == MeasurementType.Int)
                {
                    var result = IntVerification(item.SensorId.Value, item.minValue, item.maxValue);
                    if (result) return new OperationDetails(true, "", "");
                }
                else if(item.Sensor.SensorType.IsControl)
                {
                    var result = VoiceControl(item.SensorId.Value, token);
                    if (result) return new OperationDetails(true, "", "");
                }
            }

            return new OperationDetails(false, "", "");
        }

        private bool BoolVerification(int sensorId)
        {

            var seconds = DateTimeOffset.Now.AddSeconds(-4);

            var lastHistory = _db.HistoryRepo.GetLastHistoryBySensorIdAndDate(sensorId, seconds);
            if (lastHistory == null) return false;
            if (lastHistory.BoolValue.Value) return true;

            return false;
        }

        private bool IntVerification(int sensorId , int? minValue , int? maxValue)
        {          
            var period = 5;

            if (minValue != null && maxValue != null)
            {
                var min = _db.HistoryRepo.GetIntMinValueForPeriod(sensorId, period);
                if (min == null) return false;

                var max = _db.HistoryRepo.GetIntMaxValueForPeriod(sensorId, period);
                if (max == null) return false;

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

            return false;
        }

        private bool VoiceControl(int sensorId , Guid token)
        {
            var control = _db.ControlRepo.GetByToken(token);
             var sensorControl = _db.SensorControlRepo.GetByControlIdAndSensorId(control.Id, sensorId);

            if (sensorControl == null) return false;
             if (sensorControl.IsActive) return true;

            return false;
        }

        public OperationDetails Activate(Guid controlToken , Guid sensorToken , bool isActive)
        {
            var control = _db.ControlRepo.GetByToken(controlToken);
             var sensor = _db.SensorRepo.GetByToken(sensorToken);

            if(control == null || sensor == null) return new OperationDetails(false, "", "");

            var sensorControl = _db.SensorControlRepo.GetByControlIdAndSensorId(control.Id, sensor.Id);
             if (sensorControl == null) return new OperationDetails(false, "", "");

            sensorControl.IsActive = isActive;

            return new OperationDetails(true , "", "");
        }

    }
    
}
