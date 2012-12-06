using System;
using Moq;
using cms.data;
using cms.data.DataProvider;
using cms.data.Shared.Models;

namespace cms.web.tests.Code
{
	internal class UserResourceManagerTestBase
	{
		protected static readonly Guid Id = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");

		readonly ApplicationSetting _defaultApp = new ApplicationSetting
		{
			Name = "prdel",
			DefaultLanguage = "cs",
			Id = Id
		};

		public ApplicationSetting CreateDefaultApp()
		{

			var mock = new Mock<DataProviderAbstract>(new Guid("00000000-0000-0000-0000-000000000000"), 1);
			mock.Setup(x => x.Add(It.IsAny<ApplicationSetting>(), 1))
				.Returns(_defaultApp);
			var db = mock.Object;

			return db.Add(new ApplicationSetting(),1);
		}

	}
}