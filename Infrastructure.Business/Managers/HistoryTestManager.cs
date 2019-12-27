﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Infrastructure.Business.DTOs;
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

        public GraphDTO GetGraphBySensorId(int SensorId)
        {
            IEnumerable<History> histories = unitOfWork.HistoryRepo.GetHistoriesBySensorId(SensorId);
            GraphDTO graph = new GraphDTO
            {
                SensorId = SensorId,
                SensorName = histories.First().Sensor.Name,
                SensorType = histories.First()
                                            .Sensor
                                            .SensorType
                                            .Name,
                MeasurmentType = histories.First()
                                            .Sensor
                                            .SensorType
                                            .MeasurmentType,
                Dates = new List<DateTimeOffset>()
            };
            foreach(History history in histories)
            {
                graph.Dates.Add(history.Date);
            }
            switch (graph.MeasurmentType)
            {
                case MeasurmentType.Int:
                    graph.IntValues = new List<int>();
                    foreach(var history in histories)
                    {
                        graph.IntValues.Add(history.IntValue.Value);
                    }
                    break;
                case MeasurmentType.Double:
                    graph.DoubleValues = new List<double>();
                    foreach(var history in histories)
                    {
                        graph.DoubleValues.Add(history.DoubleValue.Value);
                    }
                    break;
                case MeasurmentType.Bool:
                    graph.BoolValues = new List<bool>();
                    foreach(var history in histories)
                    {
                        graph.BoolValues.Add(history.BoolValue.Value);
                    }
                    break;
                case MeasurmentType.String:
                    graph.StringValues = new List<string>();
                    foreach (var history in histories)
                    {
                        graph.StringValues.Add(history.StringValue);
                    }
                    break;
                default:
                    break;
            }
            return graph;
        }
    }
}
