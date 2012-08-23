using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Dtos
{
	public static class DtoExtensons
	{
		//TODO: tohle je nahovno
		static string _currentCulture {get
		{
			return shared.SharedLayer.Culture;
		}} 

		public static bool IsEmpty(this Guid s )
		{
			var zeros = new Guid("00000000-0000-0000-0000-000000000000");
			return s.Equals(zeros);
		}
		
		public static ResourceDtoLoc ToDto(this Resource source)
		{
			return new ResourceDtoLoc()
			{
				Value = source.Value,
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
		
		public static IDictionary<string,ResourceDtoLoc> ToDtos(this ICollection<Resource> source)
		{
			if (source == null) 
				return new Dictionary<string, ResourceDtoLoc>();

			return source.Where(x => (x.Culture == _currentCulture || x.Culture == null) && x.Key != null)
				.ToDictionary(x=>x.Key, v=>v.ToDto() );
		}

		public static Resource ToResource(this ResourceDtoLoc s)
		{
			return new Resource
			       	{
			       		Id = s.Id,
			       		Value = s.Value
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
		 	       		Width = source.Width,
						ParentId = source.Parent == null ? string.Empty : source.Parent.Id.ToString(),
		 	       		ResourcesLoc = source.Resources.ToDtos()
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
				ResourceDto = source.Resource != null ? source.Resource.ToDto() : new ResourceDtoLoc(),
				Name = source.Name
			};

		}
	}
}