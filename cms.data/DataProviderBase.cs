using System;

namespace cms.data
{
	public class DataProviderBase
	{
		protected Guid ApplicationId { get; set; }

		protected DataProviderBase(Guid applicationId )
		{
			ApplicationId  = applicationId;
		}

	}
}