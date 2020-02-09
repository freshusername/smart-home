using Domain.Core.Model;
using Domain.Core.Model.Enums;
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
    class ReportElementManagerTest : TestInitializer
    {
        private ReportElementManager _manager;
        private static Mock<IHistoryManager> _historyManager;
        private static List<ReportElement> reportElements;
        private static List<History> histories;
        private static ReportElement _reportElement;
        private static ReportElementDto _reportElementDto;
        private static HistoryDto _historyDto;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _historyManager = new Mock<IHistoryManager>();
            _manager = new ReportElementManager(
                _historyManager.Object,
                mockUnitOfWork.Object,
                mockMapper.Object);

            _reportElement = new ReportElement
            {
                Id = 1,
                Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                Sensor = new Sensor() { Id = 1, Name = "Sensor1" },
                SensorId = 1
            };
            _reportElementDto = new ReportElementDto
            {
                Id = 1,
                Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                SensorId = 1
            };
            _historyDto = new HistoryDto
            {
                Id = 3,
                Date = new DateTimeOffset(),
                IntValue = 3
            };

            reportElements = new List<ReportElement>() {
                new ReportElement {
                    Id = 1,
                    Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1" },
                    Dashboard = new Dashboard(){ Id = 1, Name ="Dashboard1"},
                    SensorId = 1
                },
                new ReportElement {
                    Id = 2,
                    Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                    Sensor = new Sensor() { Id = 2, Name = "Sensor2" },
                    Dashboard = new Dashboard(){ Id = 1, Name ="Dashboard1"},
                    SensorId = 2
                }
            };
            histories = new List<History>() {
                new History {
                    Id = 1,
                    Date = DateTimeOffset.Now.AddDays(-(int)_reportElement.Hours),
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    BoolValue = true
                },
                new History {
                    Id = 2,
                    Date = new DateTimeOffset(),
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    IntValue = 2
                },
                new History {
                    Id = 3,
                    Date = new DateTimeOffset(),
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    IntValue = 3
                },
                //for GetWordCloudById_IfNoHistories_Returns_False
                new History {
                    Id = 4,
                    Date = DateTimeOffset.Now.AddDays(-(int)_reportElement.Hours),
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    IntValue = 3
                }
            };


            mockUnitOfWork.Setup(uow => uow.HistoryRepo
                .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<IEnumerable<History>>(histories));


            mockUnitOfWork.Setup(h => h.HistoryRepo
                .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                    .Returns((int i, DateTimeOffset date) =>
                        Task.FromResult(histories.Where(x => x.Id == i && x.Date == date)));

            mockUnitOfWork.Setup(h => h.ReportElementRepo.GetById(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(_reportElement));
        }

        [Test]
        public void CreateReportElement_Returns_True()
        {
            //arrange           
            Guid userId = new Guid("7dd2f2d5-841e-4d53-9465-60947b29ccb8");
            ReportElement newReportElement = new ReportElement();

            mockMapper.Setup(re => re.Map<ReportElementDto, ReportElement>(_reportElementDto))
                .Returns(newReportElement);

            mockUnitOfWork.Setup(re => re.ReportElementRepo.Insert(newReportElement));

            //act
            var result = _manager.CreateReportElement(_reportElementDto, userId.ToString()).Result;

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetWordCloudById_GetsByValidId_Returns_True()
        {
            //arrange
            mockMapper.Setup(m => m
                .Map<ReportElement, ReportElementDto>(_reportElement))
                    .Returns(_reportElementDto);

            mockUnitOfWork.Setup(uow => uow.HistoryRepo
                .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<IEnumerable<History>>(histories));

            //act
            var result = _manager.GetWordCloudById(1).Result;

            //assert
            Assert.IsTrue(result.IsCorrect);
        }
        [Test]
        public void GetWordCloudById_GetsByInValidId_Returns_False()
        {
            //arrange

            //act
            var result = _manager.GetWordCloudById(0).Result;

            //assert
            Assert.IsFalse(result.IsCorrect);
        }

        [Test]
        public void GetWordCloudById_IfNoHistories_Returns_False()
        {
            //arrange
            mockMapper.Setup(m => m
                .Map<ReportElement, ReportElementDto>(_reportElement))
                    .Returns(new ReportElementDto { Id = 2, SensorId = 3 });

            mockUnitOfWork.Setup(h => h.ReportElementRepo.GetById(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(new ReportElement { Id = 2, SensorId = 3 }));


            mockUnitOfWork.Setup(uow => uow.HistoryRepo
                .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<IEnumerable<History>>(new List<History>()));

            //act
            var result = _manager.GetWordCloudById(2).Result;

            //assert
            Assert.IsFalse(result.IsCorrect);
        }

    }
}
