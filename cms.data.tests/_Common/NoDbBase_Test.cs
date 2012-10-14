using System;
using System.Collections.Generic;
using Moq;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using System.Linq;
using cms.shared;

namespace cms.data.tests._Common
{
	public class NoDbBase_Test
	{
		protected const string CultureCs = "cs";
		protected const string CultureEn = "en";

		protected IRepository _repository;
		protected static IList<Resource> _allResources;
		protected static IList<Grid> _allGrids;
		protected static IList<ApplicationSetting> _applicationSettings;
	
		protected IRepository GetRepositoryMock()
		{
			_allResources = new List<Resource>();
			_allGrids = new List<Grid>();
			_applicationSettings = new List<ApplicationSetting>();

			var repo = new Mock<IRepository>();
			repo.Setup(x => x.Resources).Returns(_allResources.AsQueryable());
			repo.Setup(x => x.Grids).Returns(_allGrids.AsQueryable());
			repo.Setup(x => x.ApplicationSettings).Returns(_applicationSettings.AsQueryable());
			
			repo.Setup(x => x.Add(It.IsAny<Resource>())).Returns((Resource input) =>
				{
					input.Id = _allResources.Count + 1;
					_allResources.Add(input);
					return input;
				});

			repo.Setup(x => x.Add(It.IsAny<Grid>())).Returns((Grid input) =>
				{
					_allGrids.Add(input);
					foreach (var resource in input.Resources)
					{
						repo.Object.Add(resource);
					}
					return input;
				});

			repo.Setup(x => x.Add(It.IsAny<ApplicationSetting>())).Returns((ApplicationSetting input) =>
				{
					_applicationSettings.Add(input);
					foreach (var grid in input.Grids)
					{
						repo.Object.Add(grid);
					}
					return input;
				});

			return repo.Object;
		}

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

		protected ApplicationSetting CreateDefaultApplication(string name)
		{
			return new ApplicationSetting()
			{
				Name = name,
				Id = Guid.NewGuid(),
				DefaultLanguage = CultureCs
			};
		}
	
	
	}
}