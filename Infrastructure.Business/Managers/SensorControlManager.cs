﻿using AutoMapper;
using Domain.Core.CalculateModel;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.SensorControl;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class SensorControlManager : BaseManager, ISensorControlManager
    {
        public SensorControlManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        public List<SensorControlDto> GetSensorControls()
        {
            var sensorControl = unitOfWork.SensorControlRepo.GetAll().Result;
            var result = mapper.Map<IEnumerable<SensorControl>, IEnumerable<SensorControlDto>>(sensorControl).ToList();
            return result;
        }

        public SensorControlDto GetById(int id)
        {
            var sensorControl = unitOfWork.SensorControlRepo.GetById(id).Result;
            if (sensorControl == null) return null;

            var result = mapper.Map<SensorControl, SensorControlDto>(sensorControl);

            return result;
        }

        public OperationDetails Update(SensorControlDto controlDto)
        {
            var sensorControl = mapper.Map<SensorControlDto, SensorControl>(controlDto);

            try
            {
                unitOfWork.SensorControlRepo.Update(sensorControl);
                unitOfWork.Save();
            }
            catch
            {
                return new OperationDetails(false);
            }

            return new OperationDetails(true);
        }

        public OperationDetails Add(SensorControlDto controlDto)
        {
            var sensorControl = mapper.Map<SensorControlDto, SensorControl>(controlDto);
            sensorControl.IsActive = true;

            try
            {
                unitOfWork.SensorControlRepo.Insert(sensorControl);
                unitOfWork.Save();
            }
            catch
            {
                return new OperationDetails(false);
            }

            return new OperationDetails(true);
        }

        public OperationDetails UpdateById(int id, bool isActive)
        {
            var sensorControl = unitOfWork.SensorControlRepo.GetById(id).Result;
            if (sensorControl == null) return new OperationDetails(false, "", "");

            sensorControl.IsActive = isActive;
            unitOfWork.SensorControlRepo.Update(sensorControl);

            unitOfWork.Save();
            return new OperationDetails(true, "", "");
        }

        public OperationDetails Delete(int id)
        {
            var sensorControl = unitOfWork.SensorControlRepo.GetById(id).Result;
            if (sensorControl == null) return new OperationDetails(false, "", "");

            unitOfWork.SensorControlRepo.Delete(sensorControl);

            unitOfWork.Save();
            return new OperationDetails(true, "", "");
        }
    }
}
