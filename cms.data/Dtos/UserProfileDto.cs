namespace cms.data.Dtos
{
	public class UserProfileDto 
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool ApplicationUser { get; set; }
	}
}