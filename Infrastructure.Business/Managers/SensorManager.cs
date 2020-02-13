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
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.SignalR;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Managers;

namespace Infrastructure.Business.Interfaces
{
    public class SensorManager : BaseManager, ISensorManager
    {
        protected readonly IHubContext<GraphHub> graphHub;
        public SensorManager(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<GraphHub> hubContext) : base(unitOfWork, mapper)
        {
            graphHub = hubContext;
        }


        public async Task<SensorDto> Create(SensorDto sensorDto)
        {
            if (sensorDto == null || unitOfWork.SensorRepo.GetByToken(sensorDto.Token) != null)
            {
                return null;
            }
            Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
            try
            {
                await unitOfWork.SensorRepo.Insert(sensor);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return mapper.Map<Sensor, SensorDto>(sensor);
        }

        public async Task<SensorDto> Update(SensorDto sensorDto)
        {
            if (sensorDto == null || unitOfWork.SensorRepo.GetById(sensorDto.Id).Result == null)
            {
                return null;
            }
            Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
            try
            {
                await unitOfWork.SensorRepo.Update(sensor);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return mapper.Map<Sensor, SensorDto>(sensor);
        }

        public async Task<OperationDetails> Delete(int sensorId)
        {
            Sensor sensor = await unitOfWork.SensorRepo.GetById(sensorId);
            if (sensor == null)
                return new OperationDetails(false, "err", "err");
            try
            {
                await unitOfWork.SensorRepo.Delete(sensor);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "Sensor has been deleted!", "Name");
        }

        public async Task<SensorDto> GetSensorByIdAsync(int sensorId)
        {
            var sensor = await unitOfWork.SensorRepo.GetById(sensorId);
            var result = mapper.Map<Sensor, SensorDto>(sensor);

            return result;
        }

        public async Task<IEnumerable<SensorDto>> GetAllSensorsAsync()
        {
            var sensors = await unitOfWork.SensorRepo.GetAll();
            var model = mapper.Map<IEnumerable<Sensor>, IEnumerable<SensorDto>>(sensors);

            return model;
        }
        public async Task<IEnumerable<SensorDto>> GetAllSensorsByUserIdAsync(string userId)
        {
            var sensors = await unitOfWork.SensorRepo.GetAllSensorsByUserId(userId);
            var model = mapper.Map<IEnumerable<Sensor>, IEnumerable<SensorDto>>(sensors);

            return model;
        }

        public OperationDetails AddUnclaimedSensor(Guid token, string value)
        {
            var sensor = new Sensor { Name = "Unidentified", Token = token, CreatedOn = DateTimeOffset.Now, IsValid = false };

            if (sensor == null)
                return new OperationDetails(false, "Operation did not succeed!", "");
            unitOfWork.SensorRepo.Insert(sensor);

            unitOfWork.Save();
            return new OperationDetails(true, "Operation succeed", "", new Dictionary<string, object>() { { "id", sensor.Id } });
        }

        public async Task<List<SensorDto>> GetSensorsByReportElementType(ReportElementType type, int dashboardId)
        {
            var dashboard = await unitOfWork.DashboardRepo.GetById(dashboardId);
            var sensors = new List<Sensor>();
            if (type == ReportElementType.OnOff)
                foreach (var sensorControl in Enum.GetValues(typeof(MeasurementType)))
                    sensors.AddRange(await unitOfWork.SensorRepo.GetSensorControlsByMeasurementTypeAndUserId((MeasurementType)sensorControl, dashboard.AppUserId));
            else if (type != ReportElementType.Clock && type != ReportElementType.StatusReport)
            {
                if (type == ReportElementType.Columnrange || type == ReportElementType.Gauge || type == ReportElementType.Heatmap ||
                    type == ReportElementType.TimeSeries || type == ReportElementType.Wordcloud)
                {
                    sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.Int, dashboard.AppUserId));
                    sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.Double, dashboard.AppUserId));
                }
                if (type == ReportElementType.TimeSeries || type == ReportElementType.BoolHeatmap)
                    sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.Bool, dashboard.AppUserId));
                if (type == ReportElementType.Wordcloud)
                    sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.String, dashboard.AppUserId));
            }
            var res = mapper.Map<List<Sensor>, List<SensorDto>>(sensors);
            return res;
        }

        public SensorDto GetSensorByToken(Guid token)
        {
            var sensor = mapper.Map<Sensor, SensorDto>(unitOfWork.SensorRepo.GetByToken(token));

            return sensor;
        }

        public List<SensorDto> GetSensorsToControl()
        {
            var allsensors = unitOfWork.SensorRepo.GetAll().Result.ToList();
            List<Sensor> sensors = new List<Sensor>();

            foreach (var items in allsensors)
            {
                if (!items.SensorType.IsControl && (items.SensorType.MeasurementType == MeasurementType.Bool || items.SensorType.MeasurementType == MeasurementType.Int))
                    sensors.Add(unitOfWork.SensorRepo.GetByToken(items.Token));
            }
            var result = mapper.Map<List<Sensor>, List<SensorDto>>(sensors);

            return result;
        }

        public List<SensorDto> GetControlSensors()
        {
            var tokens = unitOfWork.ControlRepo.GetAll().Result;
            List<Sensor> sensors = new List<Sensor>();

            foreach (var items in tokens)
            {
                sensors.Add(unitOfWork.SensorRepo.GetByToken(items.Token));
            }

            var result = mapper.Map<List<Sensor>, List<SensorDto>>(sensors);

            return result;
        }

        public async Task SetActive(int id)
        {
            Sensor sensor = await unitOfWork.SensorRepo.GetById(id);
            sensor.IsActive = !sensor.IsActive;
            await graphHub.Clients.All.SendAsync("UpdateOnOff", id, sensor.IsActive);
            await unitOfWork.SensorRepo.Update(sensor);
            unitOfWork.Save();
        }

        public async Task<SensorDto> GetLastSensor()
        {
            var sensor = await unitOfWork.SensorRepo.GetLastSensor();
            return mapper.Map<Sensor, SensorDto>(sensor);
        }


    }
}
