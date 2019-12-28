using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class SensorManager : BaseManager, ISensorManager
    {
        public SensorManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public void Insert(SensorDto sensorDto)
        {
            Sensor sensorCheck = unitOfWork.SensorRepo.GetById(sensorDto.Id);
            //Icon iconCheck = unitOfWork.IconRepo.GetById(sensorDto.IconId);
            if (sensorCheck == null /*&& iconCheck == null*/)
            {
                Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
                unitOfWork.SensorRepo.Insert(sensor);
                unitOfWork.Save();

            }

        }
        public IEnumerable<SensorDto> GetAllSensors()
        {
            var sensors = unitOfWork.SensorRepo.GetAll().ToList();
            var result = mapper.Map<IEnumerable<Sensor>, IEnumerable<SensorDto>>(sensors);

            return result;
        }
    }
}
