using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Dtos
{
	public static class DtoExtensons
	{
		public static ApplicationSettingDto ToDto(this ApplicationSetting source)
		 {
			 return new ApplicationSettingDto
		 	       	{
						Name =source.Name,
						Id = source.Id
		 	       	};
		 }

		public static IList<ApplicationSettingDto> ToDtos(this IList<ApplicationSetting> source)
		{
			return source.Select(item => item.ToDto()).ToList();
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
		 	       		Width = source.Width,
		 	       		Resources = source.Resources
					};
		 }
	}

	public class ApplicationSettingDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}