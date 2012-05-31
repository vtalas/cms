using System.Web;
using dotless.Core.Input;

namespace cms.Controllers
{
	public class ServerPathResolver : IPathResolver
	{
		private HttpServerUtilityBase Server { get; set; }
		public ServerPathResolver(HttpServerUtilityBase server)
		{
			Server = server;
		}

		public string GetFullPath(string path)
		{
			return Server.MapPath(path);
		}
	}
}