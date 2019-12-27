using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
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
            //BaseManager must be async?
            //Sensor sensorCheck = await unitOfWork.SensorRepo.GetById(sensorDto.Id);
            Sensor sensorCheck = unitOfWork.SensorRepo.GetById(sensorDto.Id);

            if (sensorCheck == null)
            {
                Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);

                //await unitOfWork.SensorRepo.Insert(sensor_to_add);
                unitOfWork.SensorRepo.Insert(sensor);
                unitOfWork.Save();

                //return new OperationDetails(true, "Hotel added", "Name");
            }

            //return new OperationDetails(false, "Hotel with the same name and location already exists", "Name");
        }
        public IEnumerable<SensorDto> GetAllSensors()
        {
            var sensors = unitOfWork.SensorRepo.GetAll().ToList();
            var result = mapper.Map<IEnumerable<Sensor>, IEnumerable<SensorDto>>(sensors);

            return result;
        }
    }
}
