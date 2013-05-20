using System;
using WebMatrix.WebData;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.Initializers
{
	public class ObjectBuilder
	{
		protected const string CultureCs = "cs";
		protected const string CultureEn = "en";

		protected Grid AGrid()
		{
			return AGrid(Guid.NewGuid());
		}

		protected Grid AGrid(Guid id)
		{
			return new Grid()
				       {
					       Id = id,
					       Category = CategoryEnum.Page,
				       };
		}

		protected ApplicationSetting AApplication(string name, Guid id)
		{
			return new ApplicationSetting()
				       {
					       Name = name,
					       Id = id,
					       DefaultLanguage = CultureCs
				       };
		}

		protected ApplicationSetting AApplication(string name)
		{
			return AApplication(name, Guid.NewGuid());
		}

		protected int CreateUser(string name)
		{
			WebSecurity.CreateUserAndAccount(name, "a");
			return WebSecurity.GetUserId(name);
		}

		protected GridPageDto AGridpageDto(string name, string link)
		{
			return new GridPageDto()
				       {
					       Name = name,
					       Category = CategoryEnum.Page,
					       Home = false,
					       Link = link
				       };
		}

		protected Grid AGrid(Guid id, ApplicationSetting application, bool authorize =false)
		{
			var grid = new Grid
			{
				Id = id,
				ApplicationSettings = application,
				Authorize = authorize
			};
			return grid;
		}

		protected GridElement AGridElement(string type, ApplicationSetting application, int position = 0)
		{
			return AGridElement(type, application, Guid.NewGuid(), position);
		}

		protected GridElement AGridElement(string type, ApplicationSetting application, Guid id, int position = 0)
		{
			var grid = new GridElement
			{
				AppliceSetting = application,
				Type = type,
				Width = 12,
				Position = position,
				Id = id
			};
			return grid;
		}
	}
}