using System;

namespace cms.data.Shared.Models
{
	public class UserData : IEntity
	{
		public UserData()
		{
			Created = DateTime.UtcNow;
		}

		public int Id { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public DateTime Created { get; set; }
		public virtual UserProfile User { get; set; }
	}
}