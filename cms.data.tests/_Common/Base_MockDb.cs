using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using cms.data.EF.RepositoryImplementation;
using cms.data.Repository;
using cms.data.Shared.Models;

namespace cms.data.tests._Common
{
	public class Base_MockDb 
	{
		protected IRepository _repository;
		protected static IList<Resource> _allResources;
		protected static IList<Grid> _allGrids;
		protected static IList<ApplicationSetting> _applicationSettings;

		public IRepository GetRepositoryMock()
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
					//AttachTo(input, input.ApplicationSettings);
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

			repo.Setup(x => x.Remove(It.IsAny<Grid>())).Callback((Grid input)=> _allGrids.Remove(input));

			return repo.Object;
		}
	}

}