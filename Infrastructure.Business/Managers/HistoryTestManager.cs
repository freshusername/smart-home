using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Business.Managers
{
	public class HistoryTestManager : BaseManager, IHistoryTestManager 
	{
		public HistoryTestManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{

		}

		public async Task<HistoryDto> GetHistoryByIdAsync(int id)
		{
			var history = unitOfWork.HistoryRepo.GetById(id);
			var result = mapper.Map<History, HistoryDto>(history);
			
			return result;
		}

		public async Task<IEnumerable<HistoryDto>> GetAllHistoriesAsync()
		{
			var histories = unitOfWork.HistoryRepo.GetAll().ToList();
			var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(histories);

			return result;
		}

        public SensorDto GetSensorByToken(Guid token)
        {
            var sensor = mapper.Map<Sensor,SensorDto>(unitOfWork.SensorRepo.GetByToken(token));

            return sensor;
        }

        public OperationDetails AddHistory(string value , int sensorId)
        {
           
            var history = new History
            {
                Date = DateTimeOffset.Now,
                SensorId = sensorId,
                
            };

            var valuemMdel = ValueParser.Parse(value);

            if (valuemMdel is int)
                history.IntValue = valuemMdel;
             else
            if (valuemMdel is double)
                history.DoubleValue = valuemMdel;
             else
            if (valuemMdel is bool)
                history.BoolValue = valuemMdel;
             else
                history.StringValue = valuemMdel;

            if (history == null)
                return new OperationDetails(false, "Operation did not succeed!", "");
            unitOfWork.HistoryRepo.Insert(history);

            return new OperationDetails(true, "Operation succeed", "");

        }
    }
}
