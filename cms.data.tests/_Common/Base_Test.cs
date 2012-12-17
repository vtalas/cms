using cms.data.EF.Initializers;
using cms.data.EF.RepositoryImplementation;
using cms.shared;

namespace cms.data.tests._Common
{
	public class Base_Test : ObjectBuilder
	{
		protected void SetCulture(string culture)
		{
			SharedLayer.SetCulture(culture);
		}

	}
}