using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Business.Managers
{
    public class HistoryManager : BaseManager, IHistoryManager
    {
        public HistoryManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
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

		public async Task<IEnumerable<HistoryDto>> GetHistoriesAsync(int count, int page, SortState sortState, bool isActivated = true, int sensorId = 0)
		{
			var histories = await unitOfWork.HistoryRepo.GetByPage(count, page, sortState, isActivated, sensorId);
			
			var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(histories);

			return result;
		}


		public async Task<IEnumerable<HistoryDto>> GetHistoriesBySensorIdAsync(int sensorId)
		{
			var histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorId(sensorId);
			var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(histories);

			return result;
		}

        public HistoryDto GetLastHistoryBySensorId(int sensorId)
        {
            var history = unitOfWork.HistoryRepo.GetLastHistoryBySensorId(sensorId);
            var result = mapper.Map<History, HistoryDto>(history);

            return result;
        }

        public double? GetMinValueForPeriod(int sensorId, int? hours)
        {
            return unitOfWork.HistoryRepo.GetMinValueForPeriod(sensorId, hours);
        }
        public double? GetMaxValueForPeriod(int sensorId, int? hours)
        {
            return unitOfWork.HistoryRepo.GetMaxValueForPeriod(sensorId, hours);;
        }

        public async Task<GraphDto> GetGraphBySensorId(int SensorId, int days)
        {
            DateTime date = DateTime.Now.AddDays(-days);

            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(SensorId, date);

            if (!histories.Any())
                return new GraphDto { IsCorrect = false };

            Sensor sensor = histories.FirstOrDefault().Sensor;
            GraphDto graph = mapper.Map<Sensor, GraphDto>(sensor);

            graph.Dates = new List<DateTimeOffset>();
            graph.Values = new List<dynamic>();

            foreach (History history in histories)
            {
                switch (graph.MeasurementType)
                {
                    case MeasurementType.Int when history.IntValue.HasValue:
                            graph.Dates.Add(history.Date);
                            graph.Values.Add(history.IntValue.Value);
                        break;

                    case MeasurementType.Double when history.DoubleValue.HasValue:
                            graph.Dates.Add(history.Date);
                            graph.Values.Add(history.DoubleValue.Value);
                        break;

                    case MeasurementType.Bool when history.BoolValue.HasValue:
                            graph.Dates.Add(history.Date);
                            graph.Values.Add(history.BoolValue.Value ? 1 : 0);
                        break;

                    case MeasurementType.String when !String.IsNullOrEmpty(history.StringValue):
                            graph.Dates.Add(history.Date);
                            graph.Values.Add(history.StringValue);
                        break;
                }
            }
            if (!graph.Dates.Any())
                graph.IsCorrect = false;
            return graph;
        }


        

        public OperationDetails AddHistory(string value , int sensorId)
        {
           
           var history = new History {          
             Date = DateTimeOffset.Now,
           SensorId = sensorId, };              
          
            var sensor = unitOfWork.SensorRepo.GetById(sensorId).Result;
             dynamic valueModel;

            if(sensor.IsActivated)
              valueModel = ValueParser.Parse(value , sensor.SensorType.MeasurementType);
            else
              valueModel = ValueParser.Parse(value);

            if (valueModel is int)
                history.IntValue = valueModel;
             else
            if (valueModel is double)
                history.DoubleValue = valueModel;
             else
            if (valueModel is bool)
                history.BoolValue = valueModel;
             else
                history.StringValue = valueModel;


            if (!CheckValue(history))
                return new OperationDetails(false, "Operation did not succeed!", "");

            unitOfWork.HistoryRepo.Insert(history);
            unitOfWork.Save();

            return new OperationDetails(true, "Operation succeed", "");
        
        }

        public bool CheckValue(History history)
        {
            var lastHistory = unitOfWork.HistoryRepo.GetLastBySensorId(history.SensorId).Result;
            if (lastHistory.Date.AddMinutes(5) < history.Date)
                return true;
            if (lastHistory.BoolValue == history.BoolValue && lastHistory.DoubleValue == history.DoubleValue && lastHistory.IntValue == history.IntValue && lastHistory.StringValue == history.StringValue)
                return false;
            return true;
        }

		public async Task<int> GetAmountAsync(bool isActivated)
		{
			return await unitOfWork.HistoryRepo.GetAmountAsync(isActivated);
		}
    }
}


