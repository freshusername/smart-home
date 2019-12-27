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

        //public async Task<OperationDetails> AddHistory<T>(T value)
        //{
        //    value.GetType();
        //    var histroy = new History
        //    {
        //        Date = DateTimeOffset.Now,

        //    };
        //}
    }
}
