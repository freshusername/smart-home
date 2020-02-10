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
        private HistoryManager _manager;
        private Mock<IHubContext<GraphHub>> _hubContext;
        private static List<History> histories;
        private HistoryDto _historyDto;
        private Sensor _sensor;


        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _hubContext = new Mock<IHubContext<GraphHub>>();
            _manager = new HistoryManager(mockUnitOfWork.Object, mockMapper.Object, _hubContext.Object);
            CultureInfo ci = CultureInfo.InvariantCulture;

            histories = new List<History>() {
                new History
                {
                    Id = 1,
                    Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                    BoolValue = true,
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1" },
                    SensorId = 1
                },
                new History
                {
                    Id = 2,
                    Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                    StringValue = "456",
                    Sensor = new Sensor() { Id = 2, Name = "Sensor1" },
                    SensorId = 2
                },
                new History
                {
                    Id = 3,
                    Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                    IntValue = 456,
                    Sensor = new Sensor() { Id = 3, Name = "Sensor1" },
                    SensorId = 3
                },
                new History
                {
                    Id = 4,
                    Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci),
                    DoubleValue = 456.0,
                    Sensor = new Sensor() { Id = 4, Name = "Sensor1" },
                    SensorId = 4
                },
            };
            _sensor = new Sensor()
            {
                Id = 1,
                Name = "Sensor1",
                SensorType = new SensorType() { Id = 1, Name = "SensorType1", MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int },
                IsActive = false
            };
        }

        [Test]
        public void CheckLastHistoryBySensorId_IfExists_ReturnsTrue()
        {
            //arrange

            mockUnitOfWork.Setup(h => h.HistoryRepo.GetLastHistoryBySensorId(4))
                .Returns(histories.First());

            //act
            var result = _manager.CheckLastHistoryBySensorIdExists(4);

            //assert
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void AddHistory_SensorIsNotActiveReturnsFalse()
        {
            //arrange
            int sensorId = 3;
            int sensorValue = 456;

            mockMapper.Setup(h => h.Map<HistoryDto, History>(_historyDto))
                    .Returns(histories.First());

            mockUnitOfWork.Setup(h => h.HistoryRepo.Insert(histories.First()));
            mockUnitOfWork.Setup(h => h.SensorRepo.GetById(sensorId))
                .Returns(Task.FromResult(_sensor));

            //act
            var result = _manager.AddHistory(sensorValue.ToString(), sensorId);

            //assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void AddHistory_InvalidSensorId_ReturnsFalse()
        {
            //arrange
            int sensorValue = 456;
            mockMapper.Setup(h => h.Map<HistoryDto, History>(_historyDto))
                    .Returns(histories.First());

            mockUnitOfWork.Setup(h => h.SensorRepo.GetById(123));

            //act
            var result = _manager.AddHistory(sensorValue.ToString(), 123);

            //assert
            Assert.IsFalse(result.Succeeded);

        }
    }
}
