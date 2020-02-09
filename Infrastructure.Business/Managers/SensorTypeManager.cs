using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
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

        public async Task<SensorTypeDto> Create(SensorTypeDto sensorTypeDto)
        {
            SensorType sensortype = mapper.Map<SensorTypeDto, SensorType>(sensorTypeDto);
            try
            {
                await unitOfWork.SensorTypeRepo.Insert(sensortype);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return mapper.Map<SensorType,SensorTypeDto>(sensortype);
        }

        public async Task<SensorTypeDto> Update(SensorTypeDto sensorTypeDto)
        {
            SensorType sensortype = mapper.Map<SensorTypeDto, SensorType>(sensorTypeDto);
            try
            {
                await unitOfWork.SensorTypeRepo.Update(sensortype);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return mapper.Map<SensorType, SensorTypeDto>(sensortype);
        }

        public async Task<OperationDetails> Delete(int id)
        {
            try
            {
                await unitOfWork.SensorTypeRepo.DeleteById(id);
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
            var sensorType = await unitOfWork.SensorTypeRepo.GetById(id);
            var result = mapper.Map<SensorType, SensorTypeDto>(sensorType);

            return result;
        }

        public async Task<IEnumerable<SensorTypeDto>> GetAllSensorTypesAsync()
        {
            var sensorTypes = await unitOfWork.SensorTypeRepo.GetAll();
            var result = mapper.Map<IEnumerable<SensorType>, IEnumerable<SensorTypeDto>>(sensorTypes);

            return result;
        }

        public async Task<SensorTypeDto> GetLastSensorType()
        {
            var sensorType = await unitOfWork.SensorTypeRepo.GetLastSensorType();
            return mapper.Map<SensorType, SensorTypeDto>(sensorType);
        }
    }
}
