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
using System.Globalization;
using System.Linq;
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
        private ReportElement _existingElement;
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
                Id = 1,
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
            _existingElement = new ReportElement()
            {
                Id = 1,
                DashboardId = 3,
                Height = 0,
                Width = 0,
                X = 0,
                Y = 0,
                Hours = ReportElementHours.AllTime,
                IsLocked = true,
                SensorId = 3,
                Type = ReportElementType.Wordcloud
            };

            mockMapper.Setup(m => m.Map<ReportElement, ReportElementDto>(_existingElement))
                .Returns(_mockReportelementDto);

            mockUnitOfWork.Setup(h => h.HistoryRepo
                .GetHistoriesBySensorIdAndDate(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                    .Returns((int i, DateTimeOffset date) =>
                        Task.FromResult(GetMockHistories().Where(x => x.Id == i && x.Date == date)));

            mockUnitOfWork.Setup(h => h.ReportElementRepo.GetById(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(_existingElement));


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

        [Test]
        public void GetWordCloudById_GetsExistingElement_Returns_True()
        {
            //arrange

            //act
            var result = _manager.GetWordCloudById(_existingElement.Id).Result;

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetWordCloudById_IfNoHistories_Returns_False()
        {
            //arrange

            //act
            var result = _manager.GetWordCloudById(6).Result;

            //assert
            Assert.IsFalse(result.IsCorrect);
        }

        [Test]
        public void GetWordCloudById_IfNoValues_Returns_False()
        {
            //arrange

            //act
            var result = _manager.GetWordCloudById(6).Result;
            //assert
            Assert.IsFalse(result.IsCorrect);
        }

        private IEnumerable<History> GetMockHistories()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            var histories = new List<History>
            {
                new History { Id = 1, Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci), StringValue = null, IntValue = null, DoubleValue = 456.0, BoolValue = null, SensorId = 3 },      
                //new History { Id = 2, Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci), StringValue = null, IntValue = 456, DoubleValue = null, BoolValue = null, SensorId = 6 },
                //new History { Id = 3, Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci), StringValue = null, IntValue = null, DoubleValue = 456.0, BoolValue = null, SensorId = 1 },
                //new History { Id = 4, Date = DateTime.ParseExact("02/05/2020", "MM/dd/yyyy", ci), StringValue = null, IntValue = null, DoubleValue = null, BoolValue = true, SensorId = 5 },
            };
            return histories;
        }
    }
}
