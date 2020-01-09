using System;
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

		public async Task<IEnumerable<HistoryDto>> GetHistoriesAsync(int count, int page, SortState sortState, int sensorId = 0)
		{
			var histories = await unitOfWork.HistoryRepo.GetByPage(count, page, sortState, sensorId);
			
			var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(histories);

			return result;
		}


		public async Task<IEnumerable<HistoryDto>> GetHistoriesBySensorIdAsync(int sensorId)
		{
			var histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorId(sensorId);
			var result = mapper.Map<IEnumerable<History>, IEnumerable<HistoryDto>>(histories);

			return result;
		}

		public async Task<GraphDTO> GetGraphBySensorId(int SensorId, int days)
        {
            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorId(SensorId);
            if (!histories.Any())
                return new GraphDTO { IsCorrect = false };

            Sensor sensor = histories.FirstOrDefault().Sensor;
            GraphDTO graph = mapper.Map<Sensor, GraphDTO>(sensor);
            
            graph.Dates = new List<DateTimeOffset>();
            
            DateTimeOffset date = DateTimeOffset.Now.AddDays(-days);

            graph.IntValues = new List<int>();
            graph.DoubleValues = new List<double>();
            graph.BoolValues = new List<bool>();
            graph.StringValues = new List<string>();

            foreach (History history in histories)
            {
                if (history.Date > date)
                {
                    graph.Dates.Add(history.Date);
                    switch (graph.MeasurementType)
                    {
                        case MeasurementType.Int:
                            graph.IntValues.Add(history.IntValue.Value);
                            break;

                        case MeasurementType.Double:
                            graph.DoubleValues.Add(history.DoubleValue.Value);
                            break;

                        case MeasurementType.Bool:
                            graph.BoolValues.Add(history.BoolValue.Value);
                            graph.IntValues.Add(history.BoolValue.Value ? 1 : 0);
                            break;

                        case MeasurementType.String:
                            graph.StringValues.Add(history.StringValue);
                            break;
                        default:
                            break;
                    }
                }
            }
            if (!graph.Dates.Any())
                graph.IsCorrect = false;
            return graph;
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
            unitOfWork.Save();

            return new OperationDetails(true, "Operation succeed", "");
        
        }

		public async Task<int> GetAmountAsync()
		{
			return await unitOfWork.HistoryRepo.GetAmountAsync();
		}
    }
}


