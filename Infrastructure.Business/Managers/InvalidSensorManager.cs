
using System;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.Business.DTOs;
using Domain.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Business.Managers
{
    class InvalidSensorManager : BaseManager, IInvalidSensorManager
    {
        public InvalidSensorManager(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork, mapper)
        { }

        public List<HistoryDTO> getInvalidSensors()
        {
            List<HistoryDTO> resultList = new List<HistoryDTO>();
            HistoryDTO h;

            foreach (var history in unitOfWork.HistoryRepo.GetAll())
            {
                var sensor = unitOfWork.SensorRepo.GetById(history.SensorId);

                if (sensor.ActivatedOn == null)
                {
                    h = mapper.Map<History, HistoryDTO>(history);
                    resultList.Add(h);
                }
            }

            return resultList;
        }


    }
}
