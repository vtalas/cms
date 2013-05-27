using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms.data.Shared.Models
{
	[Table("UserProfile")]
	public class UserProfile
	{
		public UserProfile()
		{
			Applications = new Collection<ApplicationSetting>();
			ApplicationUser = 0;
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string UserName { get; set; }
		public int? ApplicationUser { get; set; }
		public bool IsApplicationUser { get { return ApplicationUser.HasValue && ApplicationUser.Value > 0; } }
		public ICollection<ApplicationSetting> Applications { get; set; }
	}
}