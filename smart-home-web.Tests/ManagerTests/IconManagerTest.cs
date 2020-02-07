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
        private Mock<IconManager> managermock;
        private static Mock<IHostingEnvironment> _env;
        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();

            _env = new Mock<IHostingEnvironment>();
            managermock = new Mock<IconManager>(mockUnitOfWork.Object, mockMapper.Object, _env.Object) { CallBase = true };
            _manager = new IconManager(mockUnitOfWork.Object, mockMapper.Object, _env.Object);
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
        public void CreateAndGetIconId_NullDto_ReturnFalse()
        {
            var path = Path.Combine("images", "Icons");
            var r = _env.Object;
            var f = r.WebRootPath;
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var file = fileMock.Object;
            IconDto newIconDto = new IconDto() { Id = 1, Path = "/images/Icons/light_sensor.png" };
            Icon newIcon = new Icon() { Id = 1, Path = "/images/Icons/light_sensor.png" };
            mockMapper.Setup(m => m
                .Map<IconDto, Icon>(It.IsAny<IconDto>()))
                    .Returns(It.IsAny<Icon>());
            managermock.SetupAllProperties();
            managermock.SetupGet(m => m.UploadPath).Returns(()=>Path.Combine(path));

            mockUnitOfWork.Setup(uof => uof
                .IconRepo.Insert(newIcon));

            var result = _manager.CreateAndGetIconId(file);

            Assert.AreEqual(1, result.Result);
        }

    }
}
