using System.IO;

namespace cms.Code.Bootstraper
{
	public abstract class BootstrapDataRepository
	{
		public abstract string Default();
		public abstract string Get(string id);
		public abstract void Save(string id,string data);
	}
	public class Less
	{
		public void xxx ()
		{

		}
	}

	public class BootstrapDataRepositoryImpl : BootstrapDataRepository
	{
		private string BasePath { get; set; }
		public BootstrapDataRepositoryImpl(string basepath)
		{
			BasePath = basepath;
		}
		
		public override string Default()
		{
			var file = Path.Combine(BasePath, "default_bootstrap.json");
			return getContent(file);
		}

		public override string Get(string id)
		{
			var variablesUserj = UserDataPath(id);

			if (!File.Exists(variablesUserj))
			{
				var data = Default();
				Save(id, data);
				return data;
			}
			
			return getContent(variablesUserj);
		}

		public override void Save(string id, string data)
		{
			SetContent(UserDataPath(id),data);
		}
		
		private string UserDataPath(string id )
		{
			return Path.Combine(BasePath, string.Format("userdata/variables_{0}.json", id));
		}

		private string getContent(string file)
		{
			if (File.Exists(file))
			{
				var content = File.ReadAllText(file);
				return content;
			}
			throw new FileNotFoundException(file);
		}

		private void SetContent(string file, string content)
		{
			var target = new StreamWriter(file);
			target.Write(content);
			target.Close();
		}
	}
}