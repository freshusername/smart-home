using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.History;
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
			var history = await unitOfWork.HistoryRepo.GetById(id);
			var result = mapper.Map<History, HistoryDto>(history);
			
			return result;
		}

		public async Task<IEnumerable<HistoryDto>> GetAllHistoriesAsync()
		{
			var histories = await unitOfWork.HistoryRepo.GetAll();
			var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(histories);

			return result;
		}
	}
}
