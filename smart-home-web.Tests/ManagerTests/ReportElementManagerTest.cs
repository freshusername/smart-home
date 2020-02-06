using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class ReportElementManagerTest : TestInitializer
    {
        private IReportElementManager _manager;
        private IHistoryManager _historyManager;
        private ReportElementDto _mockReportelementDto;
        private IHubContext<GraphHub> _hubContext;

        //private GraphDto existingGraphDto;
        //private Sensor existingSensor;
        //= new Mock<IHubContext<GraphHub>>()

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _historyManager = new HistoryManager(mockUnitOfWork.Object, mockMapper.Object, _hubContext);
            _manager = new ReportElementManager(_historyManager, mockUnitOfWork.Object, mockMapper.Object);
            _mockReportelementDto = new ReportElementDto
            {
                Type = ReportElementType.Heatmap,
                Hours = ReportElementHours.Hour24,
                DashboardId = 3,
                SensorId = 0,
                X = 0,
                Y = 0,
                Width = 0,
                Height = 0,
                IsLocked = false
            };
        }

        [Test]
        public void CreateReportElement_Returns_True()
        {
            //arrange           
            Guid userId = new Guid("7dd2f2d5-841e-4d53-9465-60947b29ccb8");
            ReportElement newReportElement = new ReportElement();

            mockMapper.Setup(re => re.Map<ReportElementDto, ReportElement>(_mockReportelementDto))
                .Returns(newReportElement);

            mockUnitOfWork.Setup(re => re.ReportElementRepo.Insert(newReportElement));

            //act
            var result = _manager.CreateReportElement(_mockReportelementDto, userId.ToString()).Result;

            //assert
            Assert.IsTrue(result);
        }
    }
}
