using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class HistoryManagerTest : TestInitializer
    {
        private IHistoryManager _manager;
        private IHubContext<GraphHub> _hubContext;
        private HistoryDto _mockHistoryDto;


        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _manager = new HistoryManager(mockUnitOfWork.Object, mockMapper.Object, _hubContext);
            _mockHistoryDto = new HistoryDto
            {
                Date = new DateTimeOffset(DateTime.Now),
                StringValue = null,
                IntValue = null,
                BoolValue = true,
                SensorId = 3
            };
        }

        [Test]
        public void Get_Returns_True()
        {
            //arrange
            int sensorId = 1;
            int days = 3;
            //act
            var result = _manager.GetGraphBySensorId(sensorId, days);
            //assert
            Assert.IsTrue(result.Result.IsCorrect);
        }

        [Test]
        public void Create_Return_True()
        {
            //arrange
            string value = "true";
            int sensorId = 3;
            //act
            var result = _manager.AddHistory(value, sensorId);
            //assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
