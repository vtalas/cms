using System.IO;
using Newtonsoft.Json.Linq;

namespace cms.Code.Bootstraper.DataRepository
{
	public class BootstrapDataRepositoryImpl : BootstrapDataRepository
	{
		private string BasePath { get; set; }
		public BootstrapDataRepositoryImpl(string basepath)
		{
			BasePath = basepath;
		}
		
		public override JObject Default()
		{
			var file = Path.Combine(BasePath, "default_bootstrap.json");
			return JObject.Parse(FileExts.GetContent(file));
		}

		public override JObject Get(string id)
		{
			var variablesUserj = UserDataPath(id);

			if (!File.Exists(variablesUserj))
			{
				var data = Default();
				Save(id, data);
				return data;
			}
			var filecontent = FileExts.GetContent(variablesUserj);
			if (string.IsNullOrEmpty(filecontent))
			{
				return Default();
			}

			return JObject.Parse(filecontent);
		}

		public override void Save(string id, string data)
		{
			FileExts.SetContent(UserDataPath(id),data);
		}

		public override void Save(string userId, JObject data)
		{
			FileExts.SetContent( UserDataPath(userId), data);
		}

		private string UserDataPath(string id )
		{
			return Path.Combine(BasePath, string.Format("userdata/variables_{0}.json", id));
		}



	}
}