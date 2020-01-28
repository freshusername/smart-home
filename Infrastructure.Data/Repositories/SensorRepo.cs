using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SensorRepo : BaseRepository<Sensor>, ISensorRepo
    {
        public SensorRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Sensor>> GetAll()
        {
            var sensors = await context.Sensors
                .Include(s => s.SensorType)
                    .ThenInclude(st => st.Icon)
                .Include(s => s.Icon)
                .ToListAsync();

            return sensors;
        }

        public override async Task<Sensor> GetById(int id)
        {
            var sensor = await context.Sensors
                .Include(s => s.SensorType)
                    .ThenInclude(st => st.Icon)
                .Include(s => s.Icon)
                .FirstOrDefaultAsync(st => st.Id == id);

            return sensor;
        }

        public async Task<Sensor> GetSensorById(int id)
        {
            var sensors = await context.Sensors.Include(s => s.SensorType).FirstOrDefaultAsync(e => e.Id == id);

            return sensors;
        }

        public Sensor GetByToken(Guid token)
        {
            var sensor = context.Sensors
                                    .Include(s => s.SensorType)
                                .FirstOrDefault(e => e.Token == token);

            return sensor;
        }

        public async Task<IEnumerable<Sensor>> GetAllSensorsByUserId(string userId)
        {
            var sensors = await context.Sensors
                    .Include(s => s.SensorType)
                        .ThenInclude(st => st.Icon)
                    .Include(s => s.Icon)
                    .Where(s => s.AppUserId == userId)
                    .ToListAsync();

            return sensors;
        }

        public async Task<IEnumerable<Sensor>> GetSensorsByMeasurementTypeAndUserId(MeasurementType type, string UserId)
        {
            var sensors = await context.Sensors
                .Where(s => s.SensorType.MeasurementType == type && s.AppUserId == UserId && s.SensorType.IsControl != true)
                .ToListAsync();

            return sensors;
        }
        
        public async Task<IEnumerable<Sensor>> GetSensorControlsByMeasurementTypeAndUserId(MeasurementType type, string UserId)
        {
            var sensors = await context.Sensors
                .Where(s => s.SensorType.MeasurementType == type && s.AppUserId == UserId && s.SensorType.IsControl != false)
                .ToListAsync();

            return sensors;
        }
        public async Task<IEnumerable<Sensor>> GetSensorControls()
        {
            var sensorControls = await context.Sensors
                .Where(s => s.SensorType.IsControl)
                .ToListAsync();

            return sensorControls;
        }       
        

        public async Task<Sensor> GetLastSensorByUserId(string userId)
        {
            var sensor = await context.Sensors
                                   .Include(s => s.SensorType)
                                       .ThenInclude(st => st.Icon)
                                   .Include(s => s.Icon)
                               .LastOrDefaultAsync(s => s.AppUserId == userId);

            return sensor;
        }
    }
}
