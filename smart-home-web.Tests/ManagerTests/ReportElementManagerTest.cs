﻿using Domain.Core.Model;
using Infrastructure.Business.DTOs.History;
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
        private static ReportElement _reportElement;
        private static GaugeDto _gaugeDto;
        private static HistoryDto _historyDto;

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

           _reportElement = new ReportElement {
                Id = 1,
                Hours = Domain.Core.Model.Enums.ReportElementHours.Hour168,
                Sensor = new Sensor() { Id = 1, Name = "Sensor1"},
                SensorId = 1
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

        [Test]
        public void GetGaugeById_InvalidReportElementId_ReturnNotCorrect()
        {
            ReportElement _report = null;

            mockUnitOfWork.Setup(u => u
              .ReportElementRepo.GetById(2)).Returns(Task.FromResult(_report));
          
            var result = manager.GetGaugeById(2).Result;

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void GetGaugeById_InvalidReportElementId_ReturnCorrect()
        {
           
            mockUnitOfWork.Setup(u => u
              .ReportElementRepo.GetById(1)).Returns(Task.FromResult(_reportElement));

            mockMapper.Setup(m => m
              .Map<ReportElement, GaugeDto>(_reportElement))
                  .Returns(_gaugeDto);

            mockHistoryManager.Setup(u => u
             .GetMinValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(10);

            mockHistoryManager.Setup(u => u
             .GetMaxValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(30);

            mockHistoryManager.Setup(u => u
            .GetLastHistoryBySensorId(_reportElement.SensorId.Value)).Returns(_historyDto);

            var result = manager.GetGaugeById(1).Result;

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
           
            mockHistoryManager.Setup(u => u
            .GetLastHistoryBySensorId(_reportElement.SensorId.Value)).Returns(_historyDto);

            var result = manager.GetGaugeById(1).Result;

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

            mockHistoryManager.Setup(u => u
             .GetMinValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(20);

            mockHistoryManager.Setup(u => u
             .GetMaxValueForPeriod(_reportElement.SensorId.Value, (int)_reportElement.Hours)).Returns(20);

            mockHistoryManager.Setup(u => u
            .GetLastHistoryBySensorId(_reportElement.SensorId.Value)).Returns(_historyDto);

            var result = manager.GetGaugeById(1).Result;

            Assert.IsTrue(result.IsValid);
        }

    }
}
