
using System;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model;
using System.Linq;
using System.Collections.Generic;
using Domain.Core.Model.Enums;
using Infrastructure.Business.Filters;

namespace Infrastructure.Business.Managers
{
    public class InvalidSensorManager : BaseManager, IInvalidSensorManager
    {
        public InvalidSensorManager(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork, mapper)
        { }

        public IEnumerable<HistoryDto> getInvalidSensors(SortState sortState)
        {
            var histories = unitOfWork.HistoryRepo.GetAll();

            var historiesfilter = histories.Where(p => p.Sensor.IsActivated==true);

            var historiesmapper = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(historiesfilter);

            var res = SortValue.SortHistories(sortState, historiesmapper);

            return res;
        }


	}
}
