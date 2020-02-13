using Domain.Core.Model;
using Infrastructure.Business.DTOs;
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
    class SensorControlManagerTest : TestInitializer
    {
        private SensorControlManager _manager;
        private List<SensorControlDto> _sensorControlDtos;
        private List<SensorControl> _sensorControls;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();

            _manager = new SensorControlManager(mockUnitOfWork.Object, mockMapper.Object);

            _manager = new SensorControlManager(mockUnitOfWork.Object, mockMapper.Object);

            _sensorControls = new List<SensorControl>()
            {
                new SensorControl 
                { 
                    Id = 1,
                    Name = "SensorControl", 
                    ControlId = null,
                    IconId = null,
                    IsActive = true,
                    SensorId = 1,
                    maxValue = 1,
                    minValue = 0
                }
            };

            _sensorControlDtos = new List<SensorControlDto>()
            { 
                new SensorControlDto 
                {
                    Id = 1, 
                    Name = "SensorControlDto", 
                    ControlId = null, 
                    IconId = null, 
                    IsActive = true, 
                    SensorId = 1, 
                    maxValue = 1, 
                    minValue = 0
                }
            };

            mockMapper.Setup(m => m
                .Map<SensorControlDto, SensorControl>(_sensorControlDtos[0]))
                    .Returns(_sensorControls[0]);

            mockMapper.Setup(m => m
                .Map<SensorControl, SensorControlDto>(_sensorControls[0]))
                    .Returns(_sensorControlDtos[0]);

            mockUnitOfWork.Setup(uow => uow
                .SensorControlRepo.Insert(It.IsAny<SensorControl>()));

            mockUnitOfWork.Setup(uow => uow
                .SensorControlRepo.Update(It.IsAny<SensorControl>()));

            mockUnitOfWork.Setup(uow => uow
                .SensorControlRepo.GetById(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(_sensorControls.FirstOrDefault(sc => sc.Id == id)));
        }

        #region Add
        [Test]
        public void Add_ValidDto_ReturnSuccededTrue()
        {
            var result = _manager.Add(_sensorControlDtos[0]);

            Assert.IsTrue(result.Succeeded);
        }
        #endregion

        #region Update
        [Test]
        public void Update_ValidDto_ReturnSuccededTrue()
        {
            mockUnitOfWork.Setup(uow => uow
                .SensorControlRepo.Update(_sensorControls[0]));

            var result = _manager.Update(_sensorControlDtos[0]);

            Assert.IsTrue(result.Succeeded);
        }

        //[Test]
        //public void Update_InvalidDto_ReturnSuccededTrue()
        //{
        //    SensorControl _invalidSensorControl = new SensorControl { Id = 0, Name = "SensorControlDto", ControlId = null, IconId = null, IsActive = true, SensorId = 1, maxValue = 1, minValue = 0 };

        //    SensorControlDto _invalidSensorControlDto = new SensorControlDto { Id = 0, Name = "SensorControlDto", ControlId = null, IconId = null, IsActive = true, SensorId = 1, maxValue = 1, minValue = 0 };

        //    mockMapper.Setup(m => m
        //        .Map<SensorControlDto, SensorControl>(_invalidSensorControlDto))
        //            .Returns(_invalidSensorControl);

        //    mockMapper.Setup(m => m
        //        .Map<SensorControl, SensorControlDto>(_invalidSensorControl))
        //            .Returns(_invalidSensorControlDto);

        //    mockUnitOfWork.Setup(uow => uow
        //        .SensorControlRepo.Update(_invalidSensorControl));

        //    var result = _manager.Update(_invalidSensorControlDto);

        //    Assert.IsFalse(result.Succeeded);
        //}

        #endregion

        #region UpdateById
        [Test]
        public void UpdateById_InvalidId_ReturnSuccededFalse()
        {
            var result = _manager.UpdateById(2, false);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void UpdateById_ValidId_ReturnSuccededTrue()
        {
            var result = _manager.UpdateById(1, false);

            Assert.IsTrue(result.Succeeded);
        }
        #endregion

        #region Delete
        [Test]
        public void Delete_InvalidId_ReturnSuccededFalse()
        {
            var result = _manager.Delete(2);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void Delete_ValidId_ReturnSuccededTrue()
        {
            var result = _manager.Delete(1);

            Assert.IsTrue(result.Succeeded);
        }
        #endregion

        #region GetById
        [Test]
        public void GetById_InvalidId_ReturnNull()
        {
            var result = _manager.GetById(2);

            Assert.IsNull(result);
        }

        [Test]
        public void GetById_ValidId_ReturnNotNull()
        {
            var result = _manager.GetById(1);

            Assert.IsNotNull(result);
        }
        #endregion
    }
}
