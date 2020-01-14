using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.DTOs.Options;
using Infrastructure.Business.Infrastructure;

namespace Infrastructure.Business.Managers
{
	public class OptionsManager : BaseManager, IOptionsManager
	{
		public OptionsManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{

		}

		//TODO: Check if we use this anywhere
		public async Task<OperationDetails> Create(Options options)
		{
			await unitOfWork.OptionsRepo.Insert(options);
			var res = unitOfWork.Save();

			if (res > 0)
			{
				return new OperationDetails(true, "Saved successfully", "");
			}
			return new OperationDetails(true, "Something is wrong", "");
		}

		//TODO: Check if we use this anywhere
		public async Task<OptionsDto> GetById(int id)
		{
			var options = await unitOfWork.OptionsRepo.GetById(id);

			var optionsDto = mapper.Map<Options, OptionsDto>(options);

			return optionsDto;
		}
	}
}
