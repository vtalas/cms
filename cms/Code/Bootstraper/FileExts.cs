using System.IO;

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
			var target = new StreamWriter(file);
			target.Write(content);
			target.Close();
		}

	}
}