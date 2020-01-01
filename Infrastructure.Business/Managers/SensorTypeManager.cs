using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class SensorTypeManager : BaseManager, ISensorTypeManager
    {
        public SensorTypeManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<OperationDetails> Create(SensorTypeDto sensorTypeDto)
        {
            SensorType sensortype = mapper.Map<SensorTypeDto, SensorType>(sensorTypeDto);
            try
            {
                unitOfWork.SensorTypeRepo.Insert(sensortype);
                unitOfWork.Save();
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "New sensor type has been added", "Name");
        }

        public async Task<OperationDetails> Delete(int id)
        {
            try
            {
                SensorType sensorType = unitOfWork.SensorTypeRepo.GetById(id);
                unitOfWork.SensorTypeRepo.Delete(sensorType);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "Sensor type has been deleted", "Name");
        }

        public async Task<SensorTypeDto> GetSensorTypeByIdAsync(int id)
        {
            var sensorType = unitOfWork.SensorTypeRepo.GetById(id);
            var result = mapper.Map<SensorType, SensorTypeDto>(sensorType);

            return result;
        }

        public async Task<IEnumerable<SensorTypeDto>> GetAllSensorTypesAsync()
        {
            var sensorTypes = unitOfWork.SensorTypeRepo.GetAll();
            var result = mapper.Map<IEnumerable<SensorType>, IEnumerable<SensorTypeDto>>(sensorTypes);

            return result;
        }

    }
}
