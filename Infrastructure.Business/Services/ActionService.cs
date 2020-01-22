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
                        var result = Verification(item.SensorId ,item.Time ,item.Rule);
                       if(result) return new OperationDetails(true ,"" , "");
                    }
                }
                     
            return new OperationDetails(false , "" , "");
        }

        private bool Verification(int sensorId , int time , CheckBy rule)
        {
            DateTimeOffset date;
           var history =  _db.HistoryRepo.GetLastHistoryBySensorIdAndDate(sensorId, date);
            var sensor = _db.SensorRepo.GetSensorById(sensorId).Result;

        }

    }
}
