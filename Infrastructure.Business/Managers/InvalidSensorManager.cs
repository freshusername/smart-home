
using System;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model;
using System.Collections.Generic;

namespace Infrastructure.Business.Managers
{
    public class InvalidSensorManager : BaseManager, IInvalidSensorManager
    {
        public InvalidSensorManager(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork, mapper)
        { }

        public List<HistoryDto> getInvalidSensors()
        {
            List<HistoryDto> resultList = new List<HistoryDto>();
            HistoryDto h;

            foreach (var history in unitOfWork.HistoryRepo.GetAll())
            {
                var sensor = unitOfWork.SensorRepo.GetById(history.Sensor.Id);

                if (sensor.IsActivated == null)
                {
                    h = mapper.Map<History, HistoryDto>(history);
                    resultList.Add(h);
                }
            }

            return resultList;
        }


    }
}
