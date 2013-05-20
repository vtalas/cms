using System;
using Google.GData.Client;
using NUnit.Framework;
using cms.Code.LinkAccounts;

namespace cms.web.tests
{
	[TestFixture]
	public class GdataJsonFileStorage_tests
	{
		private const string ClientId = "568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com";
		private const string ClientSecret = "YxM0MTKCTSBV5vGlU05Yj63h";
		private const string AccessToken = "ya29.AHES6ZRQrQVpXpt-dxJ9nneI07yjAF0ou_qqtJtkgblsKFR0";

		[Test]
		public void shouldThrowException_NoAccessToken()
		{
	
			var appId = Guid.NewGuid();
			//var x = new GdataJsonFileStorage(appId);

		}
	}
}