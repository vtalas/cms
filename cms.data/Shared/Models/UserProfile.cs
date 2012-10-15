using System.ComponentModel.DataAnnotations.Schema;

namespace cms.data.Shared.Models
{
	[Table("UserProfile")]
	public class UserProfile
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string UserName { get; set; }
	}
}