using System;
using cms.Code.LinkAccounts.Picasa;

namespace cms.Controllers.Api
{
	public class PicasaServiceProvider
	{
		public Func<PicasaWrapper> Func { get; set; }

		public PicasaServiceProvider(Func<PicasaWrapper> func)
		{
			Func = func;
		}

		public PicasaWrapper Session {get { return Func.Invoke(); }}

	}
}