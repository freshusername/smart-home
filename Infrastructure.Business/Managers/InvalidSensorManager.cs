
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
            var resultList = unitOfWork.HistoryRepo.GetAll().ToList();

            var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(resultList);

            return result;
        }


    }
}
