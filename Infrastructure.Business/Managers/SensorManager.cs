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

        public async Task<OperationDetails> Create(SensorDto sensorDto)
        {
            try
            {
                if (sensorDto.IconId == 0)
                {
                    sensorDto.IconId = (await unitOfWork.SensorTypeRepo.GetById(sensorDto.SensorTypeId)).IconId.Value;
                }
                Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
                await unitOfWork.SensorRepo.Insert(sensor);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "New sensor has been added", "Name");
        }

        public async Task<IEnumerable<SensorDto>> GetAllSensorsAsync()
        {
            var sensors = await unitOfWork.SensorRepo.GetAll();
            var model = mapper.Map<IEnumerable<Sensor>, IEnumerable<SensorDto>>(sensors);

            return model;
        }
    }
}
