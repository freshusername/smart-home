using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Managers;
using Infrastructure.Data.Repositories;
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
            _manager = new SensorManager(mockUnitOfWork.Object, mockMapper.Object);

            Guid guid = new Guid();

            _existingSensor = new Sensor { Id = 1, Name = "Existing", Token = guid, IsActive = true, IsValid = true };
            _existingSensorDto = new SensorDto { Id = 1, Name = "Existing", Token = guid, IsActive = true, IsValid = true };
        }
        #region Create
        [Test]
        public void Create_ValidDto_ReturnTrue()
        {
            Guid guid = Guid.NewGuid();
            SensorDto newSensorDto = new SensorDto() { Name = "Correct", Token = guid, IsActive = true, IsValid = true};
            Sensor newSensor = new Sensor() { Name = "Correct", Token = guid, IsActive = true, IsValid = true};

            mockMapper.Setup(m => m
                .Map<SensorDto,Sensor>(newSensorDto))
                    .Returns(newSensor);

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetByToken(guid));

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.Insert(newSensor));
            
            var result = _manager.Create(newSensorDto);

            Assert.IsTrue(result.Result.Succeeded);
        }

        [Test]
        public void Create_DtoIsNull_ReturnFalse()
        {
            SensorDto newSensorDto = null;

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetByToken(_existingSensor.Token)).Returns(_existingSensor);

            var result = _manager.Create(newSensorDto);

            Assert.IsFalse(result.Result.Succeeded);
        }

        [Test]
        public void Create_SensorExist_ReturnFalse()
        {
            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetByToken(_existingSensor.Token)).Returns(_existingSensor);

            var result = _manager.Create(_existingSensorDto);

            Assert.IsFalse(result.Result.Succeeded);
        }
        #endregion Create
        #region Update
        [Test]
        public void Update_ValidDto_ReturnTrue()
        {
            mockMapper.Setup(m => m
                .Map<SensorDto, Sensor>(_existingSensorDto))
                    .Returns(_existingSensor);

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetById(_existingSensor.Id)).Returns(Task.FromResult(_existingSensor));

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.Update(_existingSensor));

            var result = _manager.Update(_existingSensorDto);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public void Update_DtoIsNull_ReturnFalse()
        {
            SensorDto nullSensorDto = null;

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetById(_existingSensor.Id)).Returns(Task.FromResult(_existingSensor));

            var result = _manager.Update(nullSensorDto);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void Update_SensorNotExist_ReturnFalse()
        {
            SensorDto notExistingSensorDto = null;
            Sensor notExistingSensor = null;

            mockUnitOfWork.Setup(uow => uow
                .SensorRepo.GetById(0)).Returns(Task.FromResult(notExistingSensor));
            
            var result = _manager.Update(notExistingSensorDto);

            Assert.IsFalse(result.Succeeded);
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
