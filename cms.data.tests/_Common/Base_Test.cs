using System;
using WebMatrix.WebData;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests._Common
{
	public class Base_Test
	{
		protected const string CultureCs = "cs";
		protected const string CultureEn = "en";

		protected Grid CreateDefaultGrid()
		{
			return CreateDefaultGrid(Guid.NewGuid());
		}

		protected Grid CreateDefaultGrid(Guid id)
		{
			return new Grid()
			{
				Id = id,
				Category = CategoryEnum.Page,
			};
		}

		protected ApplicationSetting CreateDefaultApplication(string name, Guid id)
		{
			return new ApplicationSetting()
			{
				Name = name,
				Id = id,
				DefaultLanguage = CultureCs
			};
		}

		protected ApplicationSetting CreateDefaultApplication(string name)
		{
			return CreateDefaultApplication(name, Guid.NewGuid());
		}

		protected int CreateUser(string name)
		{
			WebSecurity.CreateUserAndAccount(name, "a");
			return WebSecurity.GetUserId(name);
		}

	}
}