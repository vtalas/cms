using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProviderImplementation
{

	public class DataEfAuthorized : DataProviderAbstract
	{
		public IRepository Repository { get; set; }

		private IQueryable<ApplicationSetting> AvailableApplications { get { return Repository.ApplicationSettings.Where(x => x.Users.Any(a => a.Id == UserId)); } }

		public DataEfAuthorized(Guid applicationId, IRepository repository, int userId)
			: base(applicationId, userId)
		{
			if (userId < 1)
			{
				throw new AuthenticationException("log_in");
			}
			Repository = repository;
		}

		public override string ApplicationName { get { return !ApplicationId.IsEmpty() ? CurrentApplication.Name : ""; } }

		public override sealed ApplicationSetting CurrentApplication
		{
			get
			{
				if (!ApplicationId.IsEmpty())
				{
					var aaa = Repository.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId && x.Users.Any(u => u.Id == UserId));
					if (aaa == null)
						throw new ObjectNotFoundException("Cannot get aplication");
					return aaa;
				}
				throw new ObjectNotFoundException("Cannot get aplication");
			}
		}

		public override bool IsUserAuthorized(int userId)
		{
			var application = Repository.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId && x.Users.Any(u => u.Id == userId));
			return application != null;
		}

		public override IEnumerable<ApplicationSetting> Applications()
		{
			return AvailableApplications.ToList();
		}

		public override MenuAbstract Menu
		{
			get { return new MenuAbstractImpl(CurrentApplication, Repository); }
		}

		public override PageAbstract Page
		{
			get { return new PageAbstractImpl(CurrentApplication, Repository); }
		}

		public override IKeyValueStorage Settings
		{
			get { return new Settings(CurrentApplication, Repository); }
		}

		public override IEnumerable<GridListDto> Grids()
		{
			var a = Repository.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id).ToList().Select(dto => new GridListDto
				{
					Id = dto.Id,
					Category = dto.Category,
					Link = dto.Resources.GetValueByKey(SpecialResourceEnum.Link, null),
					Name = dto.Resources.GetValueByKey("name", CurrentCulture),
					Home = dto.Home,
					Authorize = dto.Authorize
				});
			return a.ToList();

		}

		public override void DeleteApplication(Guid id)
		{
			var delete = AvailableApplications.Single(x => x.Id == id);
			Repository.Remove(delete);
		}

		public override ApplicationSetting Add(ApplicationSetting newitem, int userId)
		{
			var user = Repository.UserProfile.Single(x => x.Id == userId);
			newitem.Users.Add(user);
			Repository.Add(newitem);
			if (newitem.Grids == null)
			{
				var a = new Grid
					{
						ApplicationSettings = newitem,
						Home = true
					};
				a.WithResource("name", "homepage", CurrentCulture);
				newitem.Grids = new List<Grid>{a};
			}
			Repository.SaveChanges();
			return newitem;
		}
	}
}