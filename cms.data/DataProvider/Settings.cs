using System.Linq;
using System.Management.Instrumentation;
using cms.data.EF;
using cms.data.EF.RepositoryImplementation;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.DataProvider
{
	public class Settings : IKeyValueStorage
	{
		protected IRepository Db;
		protected ApplicationSetting CurrentApplication { get; set; }

		public Settings(ApplicationSetting application, IRepository db)
		{
			CurrentApplication = application;
			Db = db;
		}

		public string SettingsStorage(string key)
		{
			var a = Db.ApplicationSettingStorage.SingleOrDefault(x => x.Key == key && x.AppliceSetting.Id == CurrentApplication.Id);
			return a == null ? string.Empty : a.Value;
		}

		public string SettingsStorage(string key, string value)
		{
			var a = Db.ApplicationSettingStorage.SingleOrDefault(x => x.Key == key && x.AppliceSetting.Id == CurrentApplication.Id);
			if (a == null)
			{
				var newsettings = new ApplicationSettingStorage
				{
					Key = key,
					Value = value,
					AppliceSetting = CurrentApplication
				};
				Db.Add(newsettings);
			}
			else
			{
				a.Value = value;
			}
			Db.SaveChanges();
			return value;
		}
	}
}