using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProviderImplementation
{
	public class DataEfPublic : DataProviderAbstract
	{
		public IRepository Repository { get; set; }

		public DataEfPublic(Guid applicationId, IRepository repository, int userId)
			: base(applicationId, userId)
		{
			Repository = repository;
		}

		public override string ApplicationName { get { return CurrentApplication.Name; } }

		public override sealed ApplicationSetting CurrentApplication
		{
			get
			{
				if (!ApplicationId.IsEmpty())
				{
					var aaa = Repository.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId);
					if (aaa == null)
						throw new Exception("Cannot get aplication");
					return aaa;
				}
				throw new Exception("Cannot get aplication");
			}
		}

		public override MenuAbstract Menu
		{
			get { throw new NotImplementedException(); }
		}

		public override PageAbstract Page
		{
			get { throw new NotImplementedException(); }
		}

		public override IKeyValueStorage Settings
		{
			get { return new Settings(CurrentApplication, Repository); }
		}

		public override IEnumerable<GridListDto> Grids()
		{
			throw new NotImplementedException();
		}

		public override ApplicationSetting Add(ApplicationSetting newitem, int userId)
		{
			throw new NotImplementedException();
		}

		public override void DeleteApplication(Guid guid)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<ApplicationSetting> Applications()
		{
			throw new NotImplementedException();
		}
	}

}