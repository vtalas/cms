using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;

namespace cms.data.EF.DataProvider
{
	public class DataEfAuthorized : data.DataProvider
	{
		public EfContext db { get; set; }
		private IQueryable<ApplicationSetting> AvailableApplications { get { return db.ApplicationSettings.Where(x => x.Users.Any(a => a.Id == UserId)); } }

		public DataEfAuthorized(Guid applicationId, EfContext context, int userId)
			: base(applicationId, userId)
		{
			db = context;
		}

		public DataEfAuthorized(string applicationName, EfContext context, int userId)
			: base(applicationName, userId)
		{
			db = context;
		}

		public DataEfAuthorized(Guid applicationId, int userId) : this(applicationId, new EfContext(), userId) { }

		public DataEfAuthorized(string applicationName, int userId) : this(applicationName, new EfContext(), userId) { }

		public override sealed ApplicationSetting CurrentApplication
		{
			get
			{
				if (!ApplicationId.IsEmpty())
				{
					var aaa = db.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId && x.Users.Any(u => u.Id == UserId));
					if (aaa == null)
						throw new Exception("Cannot get aplication");
					return aaa;

				}
				if (!String.IsNullOrEmpty(ApplicationName))
				{
					var aaa = db.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId && x.Users.Any(u => u.Id == UserId));
					if (aaa == null)
						throw new Exception("Cannot get aplication");
					return aaa;

				}
				throw new Exception("Cannot get aplication");
			}
		}

		public override IEnumerable<ApplicationSetting> Applications()
		{
			return AvailableApplications.ToList();
		}

		public override MenuAbstract Menu
		{
			get { return new MenuAbstractImpl(CurrentApplication, db); }
		}

		public override PageAbstract Page
		{
			get { return new PageAbstractImpl(CurrentApplication, db); }
		}

		public override GridElementAbstract GridElement
		{
			get { return new GridElementAbstractImpl(CurrentApplication, db); }
		}


		public override IEnumerable<GridListDto> Grids()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id).Select(dto => new GridListDto
				{
					Id = dto.Id,
					Category = dto.Category,
					Name = dto.Resources.FirstOrDefault(x=>x.Key == "name" && x.Culture == CurrentCulture).Value,
					Link = dto.Resources.FirstOrDefault(x => x.Key == "link" && x.Culture == CurrentCulture).Value,
					Home = dto.Home,
				});
			return a.ToList();

		}

		public override void DeleteApplication(Guid id)
		{
			var delete = AvailableApplications.Single(x => x.Id == id);
			db.ApplicationSettings.Remove(delete);
		}

		public override ApplicationSetting Add(ApplicationSetting newitem, int userId)
		{
			var user = db.UserProfile.Single(x => x.Id == userId);
			newitem.Users.Add(user);

			db.ApplicationSettings.Add(newitem);
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
			db.SaveChanges();
			return newitem;
		}

		public override void Dispose()
		{
			if (db != null) db.Dispose();
		}


	}

}