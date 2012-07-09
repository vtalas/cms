using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Loggers;
using dotless.Core.Parser;

namespace cms.Code.Bootstraper.Less
{
	public class Lessaak : ILessCompiler

	{
		private ILogger Logger { get; set; }
		private IPathResolver PathResolver { get; set; }
		public Lessaak(ILogger logger, IPathResolver pathResolver)
		{
			Logger = logger;
			PathResolver = pathResolver;
		}

		public string GenerateCss(string lessfile, bool compress = false)
		{
			var parser = new Parser();
			var imp = new Importer(new FileReader(PathResolver));

			parser.Importer = imp;

			var a = new LessEngine
			        	{
			        		Logger = Logger,
			        		Parser = parser,
			        		Compress = compress
			        	};

			return a.TransformToCss(FileExts.GetContent(lessfile), lessfile);
		}
	}

	public interface ILessCompiler
	{
		string GenerateCss(string lessfile, bool compress = false);
	}
}