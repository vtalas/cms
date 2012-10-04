using System;
using cms.data.Shared.Models;

namespace cms.data
{
	public class DataProviderBase
	{
		protected ApplicationSetting CurrentApplication { get; set; }

		protected string CurrentCulture { get { return shared.SharedLayer.Culture; } }

		protected DataProviderBase(ApplicationSetting application )
		{
			CurrentApplication = application;
		}

	}
}