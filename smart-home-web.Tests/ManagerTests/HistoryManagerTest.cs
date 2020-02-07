using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace smart_home_web.Tests.ManagerTests
{
    [TestFixture]
    class HistoryManagerTest : TestInitializer
    {
		private ReportElementManager _manager;
		private ReportElementDto _validReportElementDto;
		private ReportElement _validReportElement;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
