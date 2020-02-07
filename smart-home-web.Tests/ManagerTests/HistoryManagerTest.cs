using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class HistoryManagerTest : TestInitializer
    {
        private IHistoryManager _manager;
        private IHubContext<GraphHub> _hubContext;
        private History _existingBoolHistory;
        private History _existingDoubleHistory;
        private History _existingStringHistory;
        private History _existingIntHistory;
        private HistoryDto _mockHistoryDto;



        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _manager = new HistoryManager(mockUnitOfWork.Object, mockMapper.Object, _hubContext);
            CultureInfo ci = CultureInfo.InvariantCulture;

            _existingBoolHistory = new History
            {
                Id = 1,
                Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                StringValue = null,
                IntValue = null,
                DoubleValue = null,
                BoolValue = true,
                SensorId = 3
            };
            _existingStringHistory = new History
            {
                Id = 2,
                Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                StringValue = "456",
                IntValue = null,
                DoubleValue = null,
                BoolValue = null,
                SensorId = 3
            };
            _existingIntHistory = new History
            {
                Id = 3,
                Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                StringValue = null,
                IntValue = 456,
                DoubleValue = null,
                BoolValue = null,
                SensorId = 3
            };
            _existingDoubleHistory = new History
            {
                Id = 3,
                Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                StringValue = null,
                IntValue = null,
                DoubleValue = 456.0,
                BoolValue = null,
                SensorId = 3
            };

        }

        [Test]
        public void CheckLastHistoryBySensorId_IfExists_ReturnsTrue()
        {
            //arrange
            int sensorId = 5;
            HistoryDto _historyDto = new HistoryDto();

            //mockMapper.Setup(h => h.Map<History, HistoryDto>(_existingHistory))
            //    .Returns(_historyDto);

            mockUnitOfWork.Setup(h => h.HistoryRepo.GetLastHistoryBySensorId(sensorId))
                .Returns(_existingBoolHistory);

            //act
            var result = _manager.CheckLastHistoryBySensorIdExists(sensorId);

            //assert
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Create_Return_True()
        {
            //arrange
            string value = "true";
            int sensorId = 3;
            History newHistory = new History();

            mockMapper.Setup(h => h.Map<HistoryDto, History>(_mockHistoryDto))
                    .Returns(newHistory);

            mockUnitOfWork.Setup(h => h.HistoryRepo.Insert(newHistory));

            //act
            var result = _manager.AddHistory(value, sensorId);

            //assert
            Assert.IsTrue(result.Succeeded);
        }

        
        
    }
}
