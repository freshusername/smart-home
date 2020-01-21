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
                    if(item.IsActive && item.Sensor.SensorType.MeasurementType == MeasurementType.Bool)
                    {
                        var result = CheckForBool(item.SensorId);
                         return result;
                    }
                }
                     
            return new OperationDetails(false , "" , "");
        }

        private OperationDetails CheckForBool(int sendorId)
        {
            DateTimeOffset date = DateTimeOffset.Now.AddSeconds(-4);        
              var history =  _db.HistoryRepo.GetLastHistoryBySensorIdAndDate(sendorId , date);

            if (history.BoolValue == true)
              return new OperationDetails(true, "" , "");

            return new OperationDetails(false, "", "");
        }
    }
}
