using Domain.Core.CalculateModel;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
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
        private static Mock<IHistoryManager> _mockHistoryManager;
        private static List<ReportElement> reportElements;
        private static List<History> histories;
        private static ReportElement _reportElement;
        private static ReportElementDto _reportElementDto;
        private static GaugeDto _gaugeDto;
        private static HistoryDto _historyDto;
        private static AvgSensorValuePerDay _avgSensorValuePerDay;
        private static BoolValuePercentagePerHour _boolValuePercentagePerHour;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _mockHistoryManager = new Mock<IHistoryManager>();

            _manager = new ReportElementManager(
               _mockHistoryManager.Object,
               mockUnitOfWork.Object,
               mockMapper.Object);

            _reportElement = new ReportElement
            {
                Id = 1,
                Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                Sensor = new Sensor() { Id = 1, Name = "Sensor1" },
                SensorId = 1
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
                    Type = ReportElementType.Wordcloud,
                    SensorId = 2
                }
            };

            histories = new List<History>() {
                new History {
                    Id = 1,
                    Date = DateTimeOffset.Now.AddDays(-(int)_reportElement.Hours),
                    Sensor = new Sensor() { Id = 5, Name = "Sensor5", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Bool} },
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
                    Sensor = new Sensor() { Id = 2, Name = "Sensor2", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    IntValue = 3
                },
                new History {
                    Id = 4,
                    Date = DateTimeOffset.Now.AddDays(-(int)_reportElement.Hours),
                    Sensor = new Sensor() { Id = 3, Name = "Sensor3", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    IntValue = 3
                }
            };

            mockUnitOfWork.Setup(u => u.ReportElementRepo
            .GetById(It.IsAny<int>()))
                .Returns((int i) =>
                    Task.FromResult(reportElements.Where(x => x.Id == i).FirstOrDefault()));

            mockUnitOfWork.Setup(u => u.HistoryRepo
            .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                .Returns((int i, DateTimeOffset date) =>
                    Task.FromResult(histories.Where(x => x.Id == i && x.Date == date)));

            mockUnitOfWork.Setup(u => u.HistoryRepo
               .GetAvgSensorsValuesPerDays(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                   .Returns(Task.FromResult<IEnumerable<AvgSensorValuePerDay>>(new List<AvgSensorValuePerDay> { _avgSensorValuePerDay }));

            _reportElementDto = new ReportElementDto
            {
                Id = 1,
                Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                DashboardId = 1,
                SensorId = 1,
                Type = ReportElementType.Wordcloud,
                IsActive = true,
                IsLocked = true,
                DashboardName = "DashboardTest"
            };

            _gaugeDto = new GaugeDto
            {
                Id = 1,
                Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                SensorId = 1,
                SensorName = "Sensor1",
                MeasurementName = "*C"
            };

            _historyDto = new HistoryDto
            {
                Id = 3,
                Date = new DateTimeOffset(),
                IntValue = 3
            };

            _avgSensorValuePerDay = new AvgSensorValuePerDay()
            {
                WeekDay = DateTime.Now,
                AvgValue = 3
            };

            _boolValuePercentagePerHour = new BoolValuePercentagePerHour()
            {
                DayDate = DateTime.Now,
                HourTime = DateTime.Now.Hour,
                TrueCount = 1,
                TrueFalseCount = 2,
                TruePercentage = 50
            };
        }

        #region DataSeries

        [Test]
        public void GetDataForTimeSeries_InvalidReportElementId_ReturnNull()
        {

            var result = _manager.GetDataForTimeSeries(0).Result;

            Assert.IsNull(result);
        }

        [Test]
        public void GetDataForTimeSeries_NoHistories_ReturnNotCorrect()
        {

            var result = _manager.GetDataForTimeSeries(2).Result;

            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Sensor2", result.SensorName);
            Assert.AreEqual(false, result.IsCorrect);
        }

        #endregion

        #region Heatmap
        [Test]
        public void GetHeatmapById_CorrectId_ReturnCorrect()
        {
            mockMapper.Setup(m => m
              .Map<Sensor, HeatmapDto>(It.IsAny<Sensor>()))
                  .Returns(new HeatmapDto());

            mockUnitOfWork.Setup(u => u.HistoryRepo
                .GetAvgSensorsValuesPerDays(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(Task.FromResult<IEnumerable<AvgSensorValuePerDay>>(new List<AvgSensorValuePerDay> { _avgSensorValuePerDay }));

            var result = _manager.GetHeatmapById(1).Result;

            Assert.IsTrue(result.IsCorrect);
        }

        [Test]
        public void GetHeatmapById_IncorrectId_ReturnNotCorrect()
        {
            var result = _manager.GetHeatmapById(0).Result;

            Assert.IsFalse(result.IsCorrect);
        }

        [Test]
        public void GetGaugeById_InvalidReportElementId_ReturnNotCorrect()
        {
            ReportElement _report = null;

            mockUnitOfWork.Setup(u => u
              .ReportElementRepo.GetById(2)).Returns(Task.FromResult(_report));
          
            var result = _manager.GetGaugeById(2).Result;

            Assert.IsFalse(result.IsValid);
        }
        #endregion

        #region BoolHeatmap
        [Test]
        public void GetBoolHeatmapById_CorrectId_ReturnCorrect()
        {
            mockMapper.Setup(m => m
              .Map<Sensor, BoolHeatmapDto>(It.IsAny<Sensor>()))
                  .Returns(new BoolHeatmapDto());

            mockUnitOfWork.Setup(u => u.HistoryRepo
                .GetBoolValuePercentagesPerHours(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(Task.FromResult<IEnumerable<BoolValuePercentagePerHour>>(new List<BoolValuePercentagePerHour> { _boolValuePercentagePerHour }));

            var result = _manager.GetBoolHeatmapById(1).Result;

            Assert.IsTrue(result.IsCorrect);
        }

        [Test]
        public void GetBoolHeatmapById_IncorrectId_ReturnNotCorrect()
        {
            var result = _manager.GetBoolHeatmapById(0).Result;

            Assert.IsFalse(result.IsCorrect);
        }

        [Test]
        public void GetBoolHeatmapById_NoBoolValueForSensor_ReturnNotCorrect()
        {
            mockUnitOfWork.Setup(u => u.HistoryRepo
                .GetBoolValuePercentagesPerHours(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            var result = _manager.GetBoolHeatmapById(1).Result;

            Assert.IsFalse(result.IsCorrect);
        }
        #endregion

        #region Gauge
        [Test]
        public void GetGaugeById_InvalidReportElementId_ReturnCorrect()
        {
           
            mockUnitOfWork.Setup(u => u
              .ReportElementRepo.GetById(1)).Returns(Task.FromResult(_reportElement));

            mockMapper.Setup(m => m
              .Map<ReportElement, GaugeDto>(_reportElement))
                  .Returns(_gaugeDto);

            _mockHistoryManager.Setup(u => u
             .GetMinValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(10);

            _mockHistoryManager.Setup(u => u
             .GetMaxValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(30);

            _mockHistoryManager.Setup(u => u
            .GetLastHistoryBySensorId(_reportElement.SensorId.Value)).Returns(_historyDto);

            var result = _manager.GetGaugeById(1).Result;

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GetGaugeById_MinOrMaxNull_ReturnNotCorrect()
        {

            mockUnitOfWork.Setup(u => u
              .ReportElementRepo.GetById(1)).Returns(Task.FromResult(_reportElement));

            mockMapper.Setup(m => m
              .Map<ReportElement, GaugeDto>(_reportElement))
                  .Returns(_gaugeDto);

            _mockHistoryManager.Setup(u => u
            .GetLastHistoryBySensorId(_reportElement.SensorId.Value)).Returns(_historyDto);

            var result = _manager.GetGaugeById(1).Result;

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void GetGaugeById_MinAndMaxEqual_ReturnCorrect()
        {

            mockUnitOfWork.Setup(u => u
              .ReportElementRepo.GetById(1)).Returns(Task.FromResult(_reportElement));

            mockMapper.Setup(m => m
              .Map<ReportElement, GaugeDto>(_reportElement))
                  .Returns(_gaugeDto);

            _mockHistoryManager.Setup(u => u
             .GetMinValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(20);

            _mockHistoryManager.Setup(u => u
             .GetMaxValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(20);

            _mockHistoryManager.Setup(u => u
            .GetLastHistoryBySensorId(_reportElement.SensorId.Value)).Returns(_historyDto);

            var result = _manager.GetGaugeById(1).Result;

            Assert.IsFalse(result.IsValid);
        }
        #endregion

        #region ReportElement
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


        #endregion

        #region WordCloud
        [Test]
        public void GetWordCloudById_GetsByValidId_Returns_True()
        {
            //arrange
            mockUnitOfWork.Setup(uow => uow.HistoryRepo
                .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                    .Returns(Task.FromResult<IEnumerable<History>>(histories));

            mockMapper.Setup(m => m
               .Map<ReportElement, ReportElementDto>(reportElements.ElementAt(1)))
                   .Returns(_reportElementDto);

            //act
            var result = _manager.GetWordCloudById(2).Result;

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

        #endregion
    }
}
