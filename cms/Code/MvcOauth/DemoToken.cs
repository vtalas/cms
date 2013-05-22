using System;

namespace cms.Code.MvcOauth
{
	public class DemoToken
	{
		public DemoToken()
		{
			AccessToken = Guid.NewGuid().ToString("N");
			Start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			Expire = (long) DateTime.UtcNow.AddMinutes(2).Subtract(Start).TotalMilliseconds;
			RefreshToken = Guid.NewGuid().ToString("N");
		}

		private DateTime Start { get; set; }

		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public long Expire { get; set; }
		
		public bool IsAccessExpired
		{
			get { return DateTime.UtcNow.Subtract(Start).TotalMilliseconds > Expire; }
		}
	}
}
