using System;

namespace cms.data
{
	public class DataProviderBase
	{
		protected Guid ApplicationId { get; set; }

		protected string CurrentCulture { get { return shared.SharedLayer.Culture; } }

		protected DataProviderBase(Guid applicationId )
		{
			ApplicationId  = applicationId;
		}

	}
}