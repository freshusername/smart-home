using Domain.Core.Model;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Managers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class SensorManagerTest : TestInitializer
    {
        private SensorManager _manager;
        private SensorDto existingSensorDto;
        private Sensor existingSensor;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            _manager = new SensorManager(mockUnitOfWork.Object, mockMapper.Object);
        }

        [Test]
        public void Create_ValidDto_ReturnTrue()
        {
            Guid guid = Guid.NewGuid();
            SensorDto newSensorDto = new SensorDto(){Name = "Correct",Token = guid, IsActive = true, IsValid=true};
            Sensor newSensor = new Sensor();

            mockMapper.Setup(m => m
                .Map<Sensor>(newSensorDto))
                    .Returns(newSensor);

            var result = _manager.Create(newSensorDto);

            Assert.IsTrue(result.Result.Succeeded);
        }
    }
}
