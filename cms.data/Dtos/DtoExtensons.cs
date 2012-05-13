using cms.data.Models;

namespace cms.data.Dtos
{
	public static class DtoExtensons
	{
		 public static GridPageDto ToDto(this GridPage source)
		 {
		 	return new GridPageDto
		 	       	{
		 	       		Home = source.Home,
		 	       		Id = source.Id,
		 	       		Name = source.Name,
		 	       		Link = source.Link
						
		 	       	};
		 }

		 public static GridElementDto ToDto(this GridElement source)
		 {
		 	return new GridElementDto
		 	       	{
		 	       		Id = source.Id,
		 	       		Content = source.Content,
		 	       		Line = source.Line,
		 	       		Position = source.Position,
		 	       		Skin = source.Skin,
		 	       		Type = source.Type,
		 	       		Width = source.Width
		 	       	};
		 }
	}
}