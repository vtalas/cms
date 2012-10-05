using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.Dtos
{
	public static class DtoExtensons
	{
		//TODO: tohle je nahovno
		static string _currentCulture
		{
			get
			{
				return SharedLayer.Culture;
			}
		}

		public static bool IsEmpty(this Guid s)
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
					   Name = source.Name,
					   Id = source.Id
				   };
		}

		public static IList<ApplicationSettingDto> ToDtos(this IList<ApplicationSetting> source)
		{
			return source.Select(item => item.ToDto()).ToList();
		}

		public static IList<GridElementDto> ToDtos(this ICollection<GridElement> source)
		{
			return source.Select(item => item.ToDto()).ToList();
		}

		public static IDictionary<string, ResourceDtoLoc> ToDtos(this ICollection<Resource> source)
		{
			if (source == null)
				return new Dictionary<string, ResourceDtoLoc>();

			return source.Where(x => (x.Culture == _currentCulture || x.Culture == null) && x.Key != null)
				.ToDictionary(x => x.Key, v => v.ToDto());
		}

		public static Resource ToResource(this ResourceDtoLoc s)
		{
			return new Resource
					{
						Id = s.Id,
						Value = s.Value
					};
		}

		public static IEnumerable<MenuItemDto> getchildren(GridElement item, IEnumerable<GridElement> source)
		{
			var children = new List<MenuItemDto>();
			if (source.Any(x => x.Parent != null && x.Parent.Id == item.Id))
			{
				children = source.Where(x => x.Parent != null && x.Parent.Id == item.Id).Select(x => x.ToMenuItemDto(source)).ToList();
			}
			return children;
		}

		public static MenuItemDto ToMenuItemDto(this GridElement source, IEnumerable<GridElement> allElements )
		{
			var a = new MenuItemDto
				{
					Id = source.Id,
					Content = source.Content,
					
					Skin = source.Skin,
					Type = source.Type,
					ParentId = source.Parent == null ? string.Empty : source.Parent.Id.ToString(),
					ResourcesLoc = source.Resources.ToDtos()
				};
			a.Children = getchildren(source, allElements);
			return a;
		}
		public static GridElementDto ToDto(this GridElement source)
		{
			return new GridElementDto
					{
						Id = source.Id,
						Content = source.Content,
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
				Category = source.Category ?? CategoryEnum.Page,
				Id = source.Id.IsEmpty() ? Guid.NewGuid() : source.Id
			};
			return a;
		}

		public static MenuDto ToMenuDto(this Grid source)
		{
			if (source.Category != null && source.Category == CategoryEnum.Page )
			{
				throw new InvalidEnumArgumentException("InvalidCategoryPage");
			}
			return new MenuDto
					   {
						   Home = source.Home,
						   Id = source.Id,
						   ResourceDto = source.Resource == null ? source.Resource.ToDto() : new ResourceDtoLoc(),
						   Name = source.Name,
						   Category = source.Category,
						   Children = source.GridElements.ToChildren()

					   };
		}
		
		

		public static GridPageDto ToGridPageDto(this Grid source)
		{
			return new GridPageDto
			{
				GridElements = source.GridElements.ToDtos(),
				Home = source.Home,
				Id = source.Id,
				ResourceDto = source.Resource != null ? source.Resource.ToDto() : new ResourceDtoLoc(),
				Name = source.Name,
				Category = source.Category
			};

		}
	}
}