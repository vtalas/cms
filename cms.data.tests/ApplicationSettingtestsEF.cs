using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.Initializers;

namespace cms.data.tests
{
	[TestFixture]
	public class ApplicationSettingtestsEF
	{
		private IDataProvider D { get; set; }
		
		[SetUp]
		public void Setup()
		{
			D = new SessionProvider( new DropAndCreate());
		}

	}
}
