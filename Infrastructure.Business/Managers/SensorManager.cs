using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
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

        public OperationDetails AddUnclaimedSensor(Guid token , string value)
        {
            var measurmentType = GetMeasurment(value);
            var sensorType = new SensorType { MeasurmentType = measurmentType };
                                  
            if (sensorType == null)
                return new OperationDetails(false, "Operation did not succeed!", "");
            unitOfWork.SensorTypeRepo.Insert(sensorType);

            var sensor = new Sensor {Token = token, CreatedOn= DateTimeOffset.Now,IsActivated = false , SensorTypeId = sensorType.Id };

            if (sensor == null)
                return new OperationDetails(false, "Operation did not succeed!", "");
            unitOfWork.SensorRepo.Insert(sensor);

            return new OperationDetails(true , "Operation succeed" , sensor.Id.ToString());
        }

        private MeasurmentType GetMeasurment(string value)
        {
            var valuemMdel = ValueParser.Parse(value);

            if (valuemMdel is int)
                return MeasurmentType.Int;
            else
            if (valuemMdel is double)
                return MeasurmentType.Double;
            else
            if (valuemMdel is bool)
                return MeasurmentType.Bool;
            else
                return MeasurmentType.String;
           
        }
       
    }
}
