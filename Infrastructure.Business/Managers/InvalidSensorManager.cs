
using System;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model;
using System.Linq;
using System.Collections.Generic;

namespace Infrastructure.Business.Managers
{
    public class InvalidSensorManager : BaseManager, IInvalidSensorManager
    {
        public InvalidSensorManager(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork, mapper)
        { }

        public IEnumerable<HistoryDto> getInvalidSensors()
        {
            var histories = unitOfWork.HistoryRepo.GetAll().ToList();

            var resultList = histories.Where(p => p.Sensor.IsActivated == false);

            var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(resultList);

            return result;
        }


    }
}
