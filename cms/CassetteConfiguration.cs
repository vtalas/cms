using Cassette;
using Cassette.Configuration;
using Cassette.HtmlTemplates;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace cms
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {

        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            
			// TODO: Configure your bundles here...
            // Please read http://getcassette.net/documentation/configuration

            // This default configuration treats each file as a separate 'bundle'.
            // In production the content will be minified, but the files are not combined.
            // So you probably want to tweak these defaults!
            bundles.AddPerIndividualFile<StylesheetBundle>("Content");
            bundles.AddPerIndividualFile<ScriptBundle>("Scripts",null,
				b => b.Processor = new ScriptPipeline
				{
					CoffeeScriptCompiler = new IECoffeeScriptCompiler()
				});

            // To combine files, try something like this instead:
            //   bundles.Add<StylesheetBundle>("Content");
            // In production mode, all of ~/Content will be combined into a single bundle.

			bundles.Add<ScriptBundle>("Scripts/Admin",
				b => b.Processor = new ScriptPipeline
				{
					CoffeeScriptCompiler = new IECoffeeScriptCompiler()
				});


			//Jquery HTml 
			bundles.Add<HtmlTemplateBundle>("HtmlTemplates",
				bundle => bundle.Processor = new JQueryTmplPipeline()
			);



            // If you want a bundle per folder, try this:
            //bundles.AddPerSubDirectory<ScriptBundle>("Scripts/Admin");
            // Each immediate sub-directory of ~/Scripts will be combined into its own bundle.
            // This is useful when there are lots of scripts for different areas of the website.

        }
    }
}