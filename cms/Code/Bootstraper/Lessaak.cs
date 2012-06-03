using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Loggers;
using dotless.Core.Parser;

namespace cms.Code.Bootstraper
{
	public class Lessaak
	{
		private ILogger Logger { get; set; }
		private IPathResolver PathResolver { get; set; }
		public Lessaak(ILogger logger, IPathResolver pathResolver)
		{
			Logger = logger;
			PathResolver = pathResolver;
		}

		public string LessToCss(string lessfile)
		{
			var parser = new Parser();
			var imp = new Importer(new FileReader(PathResolver));

			parser.Importer = imp;

			var a = new LessEngine
			        	{
			        		Logger = Logger,
			        		Parser = parser
			        	};

			//a.Compress = true;
			return a.TransformToCss(FileExts.GetContent(lessfile), lessfile);

		}

	}
}