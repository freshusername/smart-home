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

        public OperationDetails IsActive(Guid token , ActionRole role)
        {
            var data =  _db.SensorControlRepo.GetByTokenAndRople(token , role).Result;
             if (data == null) return new OperationDetails(false, "", "");

            var seconds = DateTimeOffset.Now.AddSeconds(-10);
             var histories =  _db.HistoryRepo.GetHistoriesBySensorIdAndDate(data.SensorId, seconds).Result;
            if (histories == null || histories.Count() == 0) return new OperationDetails(false, "", "");

            if (role == ActionRole.AlarmFire)
            {
                var result = AlarmFire(data.SensorId);
                 return result;
            }

            return new OperationDetails(false , "" , "");
        }



        public OperationDetails AlarmFire(int sendorId)
        {           
           var history = _db.HistoryRepo.GetLastHistoryBySensorId(sendorId);

            if (history.BoolValue == true)
              return new OperationDetails(true, "" , "");

            return new OperationDetails(false, "", "");
        }
    }
}
