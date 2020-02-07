using Domain.Core.Model;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Managers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace smart_home_web.Tests.ManagerTests
{

	[TestFixture]
	class DashboardManagerTest : TestInitializer
	{
		private DashboardManager _manager;
		//private DashboardDto _existingDashboardDto;
		//private Dashboard _existingDashboard;
		private UserManager<AppUser> _existingUserManager;

		[SetUp]
		protected override void Initialize()
		{
			base.Initialize();
			_manager = new DashboardManager(mockUnitOfWork.Object, mockMapper.Object, _existingUserManager);

			//_existingDashboardDto = new DashboardDto
			//{
			//	AppUser = _existingUserManager.User
			//}
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("SomeString")]
		[TestCase("   ")]
		[TestCase("null")]
		[TestCase(@"LongStringLongStringLongStringLongStringLongStringLongStringLongString
					LongStringLongStringLongStringLongStringLongStringLongStringLongString
					LongStringLongStringLongStringLongStringLongStringLongStringLongString
					LongStringLongStringLongStringLongStringLongStringLongStringLongString
					LongStringLongStringLongStringLongStringLongStringLongStringLongString")]
		public void GetByUserId_InvalidId_ReturnsEmpty(string userId)
		{
			var exdash = new List<Dashboard>
			{
				new Dashboard
				{
					Name = "Dima",
					AppUserId = "1"
				}
			};

			var resultDashboardsDto = new List<DashboardDto>
			{
				new DashboardDto
				{
					Name = "Dima",
					AppUserId = "1"
				}
			};

			mockUnitOfWork.Setup(uof => uof.DashboardRepo.GetAll())
				.Returns(Task.FromResult<IEnumerable<Dashboard>>(exdash));

			mockMapper.Setup(m => m
				.Map<IEnumerable<Dashboard>, IEnumerable<DashboardDto>>(exdash))
				.Returns(resultDashboardsDto);

			var result = _manager.GetByUserId(userId).Result;

			Assert.IsEmpty(result);
		}

		[Test]
		[TestCase(-1)]
		[TestCase(0)]
		[TestCase(int.MaxValue)]
		[TestCase(int.MinValue)]
		public void GetById_InvalidId_ReturnsEmpty(int id)
		{
			var exdash = new Dashboard
			{
				Name = "Dima",
				AppUserId = "1",
				Id = 1
			};

			var resultDashboardsDto = new DashboardDto
			{
				Name = "Dima",
				AppUserId = "1",
				Id = 1
			};

			mockUnitOfWork.Setup(uof => uof.DashboardRepo.GetById(It.Is<int>(x => x == 1)))
				.Returns(Task.FromResult<Dashboard>(exdash));

			mockMapper.Setup(m => m
				.Map<Dashboard, DashboardDto>(exdash))
				.Returns(resultDashboardsDto);

			var result = _manager.GetById(id).Result;

			Assert.IsNull(result);
		}

		[Test]
		[TestCase(1)]
		public void GetById_ValidId_ReturnsDashboardDto(int id)
		{
			var exdash = new Dashboard
			{
				Name = "Dima",
				AppUserId = "1",
				Id = 1
			};

			var resultDashboardsDto = new DashboardDto
			{
				Name = "Dima",
				AppUserId = "1",
				Id = 1
			};

			mockUnitOfWork.Setup(uof => uof.DashboardRepo.GetById(It.Is<int>(x => x == 1)))
				.Returns(Task.FromResult<Dashboard>(exdash));

			mockMapper.Setup(m => m
				.Map<Dashboard, DashboardDto>(exdash))
				.Returns(resultDashboardsDto);

			var result = _manager.GetById(id).Result;

			Assert.AreEqual(result, resultDashboardsDto);
		}
	}
}
