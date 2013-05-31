using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms.data.Shared.Models
{
	[Table("UserProfile")]
	public class UserProfile
	{
		private DateTime Start
		{
			get
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			}
		}

		public UserProfile()
		{
			Applications = new Collection<ApplicationSetting>();
			ApplicationUser = 0;
			AccessToken = Guid.NewGuid().ToString("N");
			Expire = (long)DateTime.UtcNow.AddMinutes(20).Subtract(Start).TotalSeconds;
			RefreshToken = Guid.NewGuid().ToString("N");
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string UserName { get; set; }
		public int? ApplicationUser { get; set; }

		public string AccessToken { get; set; }
		public long? Expire { get; set; }
		public string RefreshToken { get; set; }

		public ICollection<ApplicationSetting> Applications { get; set; }

		public bool IsApplicationUser { get { return ApplicationUser.HasValue && ApplicationUser.Value > 0; } }

		public bool IsAccessTokenExpired
		{
			get
			{
				var valid = Expire.HasValue && DateTime.UtcNow.Subtract(Start).TotalSeconds < Expire.Value;
				return !valid;
			}
		}

//		public DateTime ExpireDate
//		{
//			get
//			{
//				 return Expire.HasValue && DateTime.UtcNow. Subtract(Expire.Value);
//			}
//		}
	}
}