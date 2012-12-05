using System;
using System.Collections.Generic;
using Moq;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using System.Linq;

namespace cms.data.tests._Common
{
	public class NoDbBase_Test
	{
		protected const string CultureCs = "cs";
		protected const string CultureEn = "en";

		protected IRepository _repository;
		protected static IList<Resource> _allResources;
		protected static IList<Grid> _allGrids;
	
		protected IRepository GetRepositoryMock()
		{
			var repo = new Mock<IRepository>();
			repo.Setup(x => x.Resources).Returns(_allResources.AsQueryable());
			repo.Setup(x => x.Grids).Returns(_allGrids.AsQueryable());
			
			repo.Setup(x => x.Add(It.IsAny<Resource>())).Returns((Resource input) =>
				{
					input.Id = _allResources.Count + 1;
					return input;
				});

			repo.Setup(x => x.Add(It.IsAny<Grid>())).Returns((Grid input) =>
				{
					input.Id = Guid.NewGuid();
					return input;
				});

			return repo.Object;
		}

		protected Grid CreateDefaultGrid()
		{
			return new Grid()
			{
				Id = Guid.NewGuid(),
				Category = "page",
			};
		}
		protected Grid CreateDefaultGrid(Guid id)
		{
			return new Grid()
			{
				Id = id,
				Category = "page",
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