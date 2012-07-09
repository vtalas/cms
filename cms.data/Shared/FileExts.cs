using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cms.Code.Bootstraper
{
	public class FileExts
	{
		public static string GetContent(string file)
		{
			if (File.Exists(file))
			{
				var content = File.ReadAllText(file);
				return content;
			}
			throw new FileNotFoundException(file);
		}

		public static void SetContent(string file, string content)
		{
			using (var target = new StreamWriter(file))
			{
				target.Write(content);
			}
		}

		public static void SetContent(string file, JObject data)
		{
			using (TextWriter writer = File.CreateText(file))
			{
				data.WriteTo(new JsonTextWriter(writer));
			}
		}

	}
}