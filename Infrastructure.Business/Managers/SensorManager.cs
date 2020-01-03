using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Business.DTOs.SensorType;

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
	            if (!sensorDto.IconId.HasValue)
	            {
                    sensorDto.IconId = (await unitOfWork.SensorTypeRepo.GetById(sensorDto.SensorTypeId)).IconId;
                }

	            Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
	            try
	            {
		            await unitOfWork.SensorRepo.Insert(sensor);

		            unitOfWork.Save();
	            }
	            catch (Exception ex)
	            {
		            return new OperationDetails(false, ex.Message, "Error");
				}
                
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

        public OperationDetails AddUnclaimedSensor(Guid token , string value)
        {
                                                        
            var sensor = new Sensor { Name = "Unidentified", Token = token, CreatedOn= DateTimeOffset.Now,IsActivated = false};

            if (sensor == null)
                return new OperationDetails(false, "Operation did not succeed!", "");
            unitOfWork.SensorRepo.Insert(sensor);

            unitOfWork.Save();
            return new OperationDetails(true , "Operation succeed" , sensor.Id.ToString());
        }
             
    }
}
