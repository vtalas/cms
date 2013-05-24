using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms.data.Shared.Models
{
	[Table("UserProfile")]
	public class UserProfile
	{
		public UserProfile()
		{
			Applications = new Collection<ApplicationSetting>();
		}
		
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string UserName { get; set; }
		public ICollection<ApplicationSetting> Applications { get; set; }
	}
}