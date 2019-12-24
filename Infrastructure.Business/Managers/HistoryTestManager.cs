using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Sensor;

namespace Infrastructure.Business.Managers
{
	class HistoryTestManager : BaseManager, IHistoryTestManager 
	{
		public HistoryTestManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
		}

		public async Task<SensorDto> GetSensorByIdAsync(int id)
		{
			throw new Exception();
		}
	}
}
