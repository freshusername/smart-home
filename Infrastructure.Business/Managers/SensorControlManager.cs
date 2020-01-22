using AutoMapper;
using Domain.Core.CalculateModel;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class SensorControlManager : BaseManager , ISensorControlManager
    {
        public SensorControlManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        public List<SensorControlDto> GetSensorControls()
        {
            var data = unitOfWork.SensorControlRepo.GetAll().Result;
             var result = mapper.Map<IEnumerable<SensorControl>, IEnumerable<SensorControlDto>>(data).ToList();
            return result;
        }


        public SensorControlDto GetById(int id)
        {
            var data = unitOfWork.SensorControlRepo.GetById(id).Result;
             var result = mapper.Map<SensorControl,SensorControlDto>(data);
              var sensor = unitOfWork.SensorRepo.GetByToken(result.Control.Token);
             result.ControlSensor = sensor;
            return result;
        }

        public OperationDetails Update(SensorControlDto controlDto)
        {
            var result = mapper.Map<SensorControlDto, SensorControl>(controlDto);
             if (result == null) return new OperationDetails(false , "" , "");
              unitOfWork.SensorControlRepo.Update(result);
             unitOfWork.Save();
            return new OperationDetails(true , "" , "");
        }
    }
}
