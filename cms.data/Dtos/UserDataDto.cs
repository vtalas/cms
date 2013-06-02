using System;

namespace cms.data.Dtos
{
	public class UserDataDto
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public string UserName { get; set; }
		public DateTime Created { get; set; }
	}
}