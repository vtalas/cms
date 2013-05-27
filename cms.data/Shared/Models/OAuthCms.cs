using System;

namespace cms.data.Shared.Models
{
	public class OAuthCms : IEntity
	{
		public OAuthCms()
		{

			AccessToken = Guid.NewGuid().ToString("N");
			Start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			Expire = (long)DateTime.UtcNow.AddMinutes(2).Subtract(Start).TotalSeconds;
			RefreshToken = Guid.NewGuid().ToString("N");

		}

		private DateTime Start { get; set; }

		public int Id { get; set; }
		public string AccessToken { get; set; }
		public long Expire { get; set; }
		public string RefreshToken { get; set; }
		public virtual UserProfile User { get; set; }

		public bool IsAccessExpired
		{
			get { return DateTime.UtcNow.Subtract(Start).TotalSeconds > Expire; }
		}

	}
}