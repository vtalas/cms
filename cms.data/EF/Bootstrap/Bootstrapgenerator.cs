using cms.data.Shared.Models;

namespace cms.data.EF.Bootstrap
{
	public enum Boostrapperstatus :byte
	{
		Private =1,
		Public = 2,
		Default= 3
	}
	
	public class Bootstrapgenerator : IEntity
	{
	
		public virtual int Id { get; set; }
		public virtual string UserId { get; set; }
		public virtual string Name { get; set; }
		public virtual string Data { get; set; }
		public byte? StatusData { get; set; }
		public Boostrapperstatus Status { 
			get{return (Boostrapperstatus)StatusData.Value;} 
			set{StatusData = (byte)value;} }
	}
}