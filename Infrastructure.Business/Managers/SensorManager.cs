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
        public OperationDetails Update(SensorDto sensorDto)
        {
            Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
            try
            {
                unitOfWork.SensorRepo.Update(sensor);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "Sensor has been updated!", "Name");
        }
        public OperationDetails Delete(SensorDto sensorDto)
        {
            Sensor sensor = mapper.Map<SensorDto, Sensor>(sensorDto);
            try
            {
                unitOfWork.SensorRepo.Delete(sensor);
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

        public OperationDetails AddUnclaimedSensor(Guid token, string value)
        {

            var sensor = new Sensor { Name = "Unidentified", Token = token, CreatedOn = DateTimeOffset.Now, IsActivated = false };

            if (sensor == null)
                return new OperationDetails(false, "Operation did not succeed!", "");
            unitOfWork.SensorRepo.Insert(sensor);

            unitOfWork.Save();
            return new OperationDetails(true, "Operation succeed", sensor.Id.ToString());
        }

        public async Task<IEnumerable<SensorDto>> GetSensorsByReportElementType(ReportElementType type, int dashboardId)
        {
            var dashboard = await unitOfWork.DashboardRepo.GetById(dashboardId);
            var sensors = new List<Sensor>();
            sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.Int, dashboard.AppUserId));
            sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.Double, dashboard.AppUserId));
            if (type == ReportElementType.TimeSeries || type == ReportElementType.Clock)
                sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.Bool, dashboard.AppUserId));
            if (type == ReportElementType.Clock || type == ReportElementType.Wordcloud)
                sensors.AddRange(await unitOfWork.SensorRepo.GetSensorsByMeasurementTypeAndUserId(MeasurementType.String, dashboard.AppUserId));
            var res = mapper.Map<IEnumerable<Sensor>, IEnumerable<SensorDto>>(sensors);
            return res;
        }

    }
}
