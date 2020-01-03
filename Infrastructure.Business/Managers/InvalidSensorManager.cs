
using System;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model;
using System.Linq;
using System.Collections.Generic;
using Domain.Core.Model.Enums;
using Infrastructure.Business.Filters;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class InvalidSensorManager : BaseManager, IInvalidSensorManager
    {
        public InvalidSensorManager(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<IEnumerable<HistoryDto>> getInvalidSensors(SortState sortState)
        {
            var histories = await unitOfWork.HistoryRepo.GetAll();

            var historiesfilter = histories.Where(p => p.Sensor.IsActivated==false);

            var historiesmapper = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(historiesfilter);

            var res = SortValue.SortHistories(sortState, historiesmapper);

            return res;
        }

        public async Task<Sensor> GetSensorById(int sensorId)
        {
            return await unitOfWork.SensorRepo.GetById(sensorId);
        }
    }
}
