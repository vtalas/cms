using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Dtos
{
	public static class DtoExtensons
	{
		public static bool IsEmpty(this Guid s )
		{
			var zeros = new Guid("00000000-0000-0000-0000-000000000000");
			return s.Equals(zeros);
		}
		
		public static ResourceDto ToDto(this Resource source)
		{
			return new ResourceDto
			{
				Key = source.Key,
				Value = source.Value,
				Culture = source.Culture,
				Id = source.Id
			};
			
		}
		
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
		
		public static IEnumerable<ResourceDto> ToDtos(this ICollection<Resource> source)
		{
			if (source == null) 
				return new LinkedList<ResourceDto>();
			return source.Select(item => item.ToDto()).ToList();
		}

		public static Resource ToResource(this ResourceDto s)
		{
			return new Resource{Id = s.Id,}.UpdateValues(s);
		}

		public static Resource UpdateValues(this Resource destination, ResourceDto source )
		{
			destination.Value = source.Value;
			destination.Culture = source.Culture;
			destination.Key = source.Key;
			return destination;
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
		 	       		Resources = source.Resources.ToDtos()
					};
		 }

		public static Grid ToGrid(this GridPageDto source)
		{
			var a = new Grid
			{
				Name = source.Name,
				Home = source.Home,
				Id = source.Id.IsEmpty() ? Guid.NewGuid() : source.Id
			};
			return a;
		}

		public static GridPageDto ToGridPageDto(this Grid source)
		{
			return new GridPageDto
			{
				Lines = source.GridElements.ToLines(),
				Home = source.Home,
				Id = source.Id,
				Resource = source.Resource.ToDto(),
				Name = source.Name
			};

		}
	}
}