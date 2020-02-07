using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class ReportElementManagerTest : TestInitializer
    {
        private ReportElementManager manager;
        private static Mock<IHistoryManager> mockHistoryManager;
        private static List<ReportElement> reportElements;
        private static List<History> histories;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            mockHistoryManager = new Mock<IHistoryManager>();

            manager = new ReportElementManager(
               mockHistoryManager.Object, 
               mockUnitOfWork.Object, 
               mockMapper.Object);


            reportElements = new List<ReportElement>() {
                new ReportElement {
                    Id = 1,
                    Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1" },
                    SensorId = 1
                },
                new ReportElement {
                    Id = 2,
                    Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                    Sensor = new Sensor() { Id = 2, Name = "Sensor2" },
                    SensorId = 2
                }
            };

            histories = new List<History>() {
                new History {
                    Id = 1,
                    Date = new DateTimeOffset(),
                    Sensor = new Sensor() { Id = 1, Name = "Sensor1", SensorType = new SensorType() { MeasurementType = Domain.Core.Model.Enums.MeasurementType.Int} },
                    IntValue = 1
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
        }

        [Test]
        public void GetDataForTimeSeries_InvalidReportElementId_ReturnNull()
        {

            var result = manager.GetDataForTimeSeries(0).Result;

            Assert.IsNull(result);
        }

        [Test]
        public void GetDataForTimeSeries_NoHistories_ReturnNotCorrect()
        {

            var result = manager.GetDataForTimeSeries(2).Result;

            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Sensor2", result.SensorName);
            Assert.AreEqual(false, result.IsCorrect);
        }

    }
}
