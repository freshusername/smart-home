using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Infrastructure.Data.Repositories;
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
    class SensorManagerTest : TestInitializer
    {
        private SensorManager _manager;
        private SensorDto _existingSensorDto;
        private Sensor _existingSensor;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            Mock<IHubContext<GraphHub>> hub = new Mock<IHubContext<GraphHub>>();
            _manager = new SensorManager(mockUnitOfWork.Object, mockMapper.Object, hub.Object);

            Guid guid = new Guid();

            _existingSensor = new Sensor { Id = 1, Name = "Existing", Token = guid, IsActive = true, IsValid = true };
            _existingSensorDto = new SensorDto { Id = 1, Name = "Existing", Token = guid, IsActive = true, IsValid = true };
        }
        #region Create
        [Test]
        public void Create_ValidDto_ReturnNotNull()
        {
            Guid guid = Guid.NewGuid();
            SensorDto newSensorDto = new SensorDto() { Name = "Correct", Token = guid, IsActive = true, IsValid = true };
            Sensor newSensor = new Sensor() { Name = "Correct", Token = guid, IsActive = true, IsValid = true };

            mockMapper.Setup(m => m
                .Map<SensorDto, Sensor>(newSensorDto))
                    .Returns(newSensor);

            mockMapper.Setup(m => m
                .Map<Sensor, SensorDto>(newSensor))
                    .Returns(newSensorDto);

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetByToken(guid));

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.Insert(newSensor));

            var result = _manager.Create(newSensorDto);

            Assert.IsNotNull(result.Result);
        }

        [Test]
        public void Create_DtoIsNull_ReturnNull()
        {
            SensorDto newSensorDto = null;

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetByToken(_existingSensor.Token))
                .Returns(_existingSensor);

            var result = _manager.Create(newSensorDto);

            Assert.IsNull(result.Result);
        }

        [Test]
        public void Create_SensorExist_ReturnNull()
        {
            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetByToken(_existingSensor.Token))
                .Returns(_existingSensor);

            var result = _manager.Create(_existingSensorDto);

            Assert.IsNull(result.Result);
        }
        #endregion Create
        #region Update
        [Test]
        public void Update_ValidDto_ReturnTrue()
        {
            mockMapper.Setup(m => m
                .Map<SensorDto, Sensor>(_existingSensorDto))
                    .Returns(_existingSensor);

            mockMapper.Setup(m => m
                .Map<Sensor, SensorDto>(_existingSensor))
                    .Returns(_existingSensorDto);

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetById(_existingSensor.Id))
                .Returns(Task.FromResult(_existingSensor));

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.Update(_existingSensor));

            var result = _manager.Update(_existingSensorDto);

            Assert.IsNotNull(result.Result);
        }

        [Test]
        public void Update_DtoIsNull_ReturnNull()
        {
            SensorDto nullSensorDto = null;

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetById(_existingSensor.Id))
                .Returns(Task.FromResult(_existingSensor));

            var result = _manager.Update(nullSensorDto);

            Assert.IsNull(result.Result);
        }

        [Test]
        public void Update_SensorNotExist_ReturnNull()
        {
            SensorDto notExistingSensorDto = null;
            Sensor notExistingSensor = null;

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetById(0))
                .Returns(Task.FromResult(notExistingSensor));

            var result = _manager.Update(notExistingSensorDto);

            Assert.IsNull(result.Result);
        }
        #endregion
        #region Delete
        [Test]
        public void Delete_ValidId_ReturnTrue()
        {
            mockMapper.Setup(m => m
                .Map<SensorDto, Sensor>(_existingSensorDto))
                    .Returns(_existingSensor);

            mockUnitOfWork.Setup(uow => uow
                 .SensorRepo.GetById(_existingSensor.Id)).Returns(Task.FromResult(_existingSensor));

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.Delete(_existingSensor));

            var result = _manager.Delete(_existingSensor.Id);

            Assert.IsTrue(result.Result.Succeeded);
        }

        [Test]
        public void Delete_NotValidId_ReturnFalse()
        {
            Sensor notExistingSensor = null;

            mockUnitOfWork.Setup(uow => uow
                 .SensorRepo.GetById(0)).Returns(Task.FromResult(notExistingSensor));

            var result = _manager.Delete(0);

            Assert.IsFalse(result.Result.Succeeded);
        }
        #endregion
    }
}
