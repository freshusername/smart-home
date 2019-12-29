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

        public GraphDTO GetGraphBySensorId(int SensorId, int days)
        {
            IEnumerable<History> histories = unitOfWork.HistoryRepo.GetHistoriesBySensorId(SensorId);
            GraphDTO graph = new GraphDTO
            {
                SensorId = SensorId,
                SensorName = histories.FirstOrDefault().Sensor.Name,
                SensorType = histories.FirstOrDefault()
                                            .Sensor
                                            .SensorType
                                            .Name,

                MeasurmentName = histories.FirstOrDefault()
                                            .Sensor
                                            .SensorType
                                            .MeasurmentName,

                MeasurmentType = histories.FirstOrDefault()
                                            .Sensor
                                            .SensorType
                                            .MeasurmentType,

                Dates = new List<DateTimeOffset>()
            };
            var date = DateTimeOffset.Now.AddDays(-days);

            graph.IntValues = new List<int>();
            graph.DoubleValues = new List<double>();
            graph.BoolValues = new List<bool>();
            graph.StringValues = new List<string>();

            foreach (History history in histories)
            {
                if (history.Date > date)
                {
                    graph.Dates.Add(history.Date);
                    switch (graph.MeasurmentType)
                    {
                        case MeasurmentType.Int:
                            graph.IntValues.Add(history.IntValue.Value);
                            break;

                        case MeasurmentType.Double:
                            graph.DoubleValues.Add(history.DoubleValue.Value);
                            break;

                        case MeasurmentType.Bool:
                            graph.BoolValues.Add(history.BoolValue.Value);
                            graph.IntValues.Add(history.BoolValue.Value ? 1 : 0);
                            break;

                        case MeasurmentType.String:
                            graph.StringValues.Add(history.StringValue);
                            graph.IntValues.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            
            return graph;
        }
    }
}
