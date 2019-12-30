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
            //try
            //{
                SensorType sensortype = mapper.Map<SensorTypeDto, SensorType>(sensorTypeDto);
                unitOfWork.SensorTypeRepo.Insert(sensortype);
                unitOfWork.Save();
            //}
            //catch(Exception ex)
            //{
            //    return new OperationDetails(false, ex.Message, "Error");
            //}
            return new OperationDetails(true, "New sensor type has been added", "Name");
        }

        public async Task<IEnumerable<SensorTypeDto>> GetAllSensorTypesAsync()
        {
            var sensorTypes = unitOfWork.SensorTypeRepo.GetAll();
            var result = mapper.Map<IEnumerable<SensorType>, IEnumerable<SensorTypeDto>>(sensorTypes);

            return result;
        }
    }
}
