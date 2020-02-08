using Domain.Core.Model;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class IconManagerTest : TestInitializer
    {
        private IconManager _manager;
        private Mock<IconManager> _mockManager;
        private static Mock<IHostingEnvironment> _mockEnv;
        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();

            _mockEnv = new Mock<IHostingEnvironment>();
            _mockManager = new Mock<IconManager>(mockUnitOfWork.Object, mockMapper.Object, _mockEnv.Object);
            _manager = new IconManager(mockUnitOfWork.Object, mockMapper.Object, _mockEnv.Object);
        }
        #region Create
        [Test]
        public void Create_ValidDto_ReturnTrue()
        {
            IconDto newIconDto = new IconDto() { Path = "/images/Icons/light_sensor.png" };
            Icon newIcon = new Icon() { Path = "/images/Icons/light_sensor.png" };

            mockMapper.Setup(m => m
                .Map<IconDto, Icon>(newIconDto))
                    .Returns(newIcon);

            mockUnitOfWork.Setup(uow => uow
                .IconRepo.Insert(newIcon));

            var result = _manager.Create(newIconDto);

            Assert.IsTrue(result.Result.Succeeded);
        }

        [Test]
        public void Create_NullDto_ReturnFalse()
        {
            IconDto newIconDto = null;
            Icon newIcon = null;

            mockUnitOfWork.Setup(uow => uow
                .IconRepo.Insert(newIcon));

            var result = _manager.Create(newIconDto);

            Assert.IsFalse(result.Result.Succeeded);
        }
        #endregion 
        [Test]
        public void CreateAndGetIconId_ValidFormFile_ReturnTrue()
        {
            var path = Path.Combine("images", "Icons");
            var fileMock = new Mock<IFormFile>();
            var file = fileMock.Object;

            Icon newIcon = new Icon() { Id = 1, Path = "/images/Icons/test.png" };

            _mockEnv.Setup(e => e.WebRootPath).Returns("Icons");

            mockMapper.Setup(m => m
                .Map<IconDto, Icon>(It.IsAny<IconDto>()))
                    .Returns(newIcon);

            _mockManager.SetupGet(m => m.UploadPath)
                .Returns(()=>Path.Combine(path));
            
            mockUnitOfWork.Setup(uof => uof
                .IconRepo.Insert(newIcon));

            var result = _manager.CreateAndGetIconId(file);

            Assert.AreEqual(1, result.Result);
        }
    }
}
