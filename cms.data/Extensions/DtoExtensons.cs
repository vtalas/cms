using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.Extensions
{
	public static class DtoExtensons
	{
		//TODO: tohle je nahovno
		static string _currentCulture { get { return SharedLayer.Culture; } }

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

		public static IList<ApplicationSettingDto> ToDtos(this IEnumerable<ApplicationSetting> source)
		{
			return source.Select(item => ToDto((ApplicationSetting)item)).ToList();
		}

		public static IList<GridElementDto> ToDtos(this ICollection<GridElement> source)
		{
			return source.Select(item => item.ToDto()).ToList();
		}

		public static IDictionary<string, ResourceDtoLoc> ToDtos(this ICollection<Resource> source)
		{
			return source.ToDtos(_currentCulture);
		}

		public static IDictionary<string, ResourceDtoLoc> ToDtos(this ICollection<Resource> source, string culture)
		{
			var dictionary = new Dictionary<string, ResourceDtoLoc>();
			if (source == null)
				return dictionary;

			var list = source.Where(x => (x.Culture == culture || x.Culture == null) && x.Key != null);
			foreach (var item in list)
			{
				if (dictionary.ContainsKey(item.Key))
				{
					dictionary[item.Key] = item.ToDto();
				}
				else
				{
					dictionary.Add(item.Key, item.ToDto());
				}
			}
			return dictionary;
		}

		public static Resource ToResource(this ResourceDtoLoc s, string key, Guid owner, string culture)
		{
			var cultureUpadated = JsonDataEfHelpers.CorrectCulture(key, culture);

			return new Resource
					{
						Id = s.Id,
						Key = key,
						Owner = owner,
						Culture = cultureUpadated,
						Value = s.Value
					};
		}

		public static IEnumerable<MenuItemDto> getchildren(GridElement item, IEnumerable<GridElement> source)
		{
			var children = new List<MenuItemDto>();
			if (source.Any(x => x.Parent != null && x.Parent.Id == item.Id))
			{
				children = source.Where(x => x.Parent != null && x.Parent.Id == item.Id).Select(x => x.ToMenuItemDto(source)).OrderBy(a => a.Position).ToList();
			}

			return children;
		}

		public static MenuItemDto ToMenuItemDto(this GridElement source, IEnumerable<GridElement> allElements)
		{
			var a = new MenuItemDto
				{
					Id = source.Id,
					Content = source.Content,
					Skin = source.Skin,
					Position = source.Position,
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
						Content =  source.Content != null ? (JObject)JsonConvert.DeserializeObject(source.Content) : null, 
						Position = source.Position,
						Skin = source.Skin,
						Type = source.Type,
						Width = source.Width,
						ParentId = source.Parent == null ? string.Empty : source.Parent.Id.ToString(),
						Resources = source.Resources.ToDtos()
					};
		}

		public static Grid ToGrid(this GridPageDto source)
		{
			var a = new Grid
			{
				Home = source.Home,
				Category = source.Category ?? CategoryEnum.Page,
				Id = source.Id.IsEmpty() ? Guid.NewGuid() : source.Id,
			};
			a.AddResource("name", source.Name)
				.AddResource(SpecialResourceEnum.Link, source.Link);
			return a;
		}

		public static Grid AddResource(this Grid source, string key, string value)
		{
			var cultureUpadated = JsonDataEfHelpers.CorrectCulture(key, _currentCulture);
			var res = new Resource
						 {
							 Key = key,
							 Culture = cultureUpadated,
							 Value = value,
							 Owner = source.Id
						 };
			source.Resources.Add(res);
			return source;
		}

		public static MenuDto ToMenuDto(this Grid source)
		{
			if (source.Category != null && source.Category == CategoryEnum.Page)
			{
				throw new InvalidEnumArgumentException("InvalidCategoryPage");
			}
			return new MenuDto
						{
							Home = source.Home,
							Id = source.Id,
							Link = source.Resources.GetValueByKey(SpecialResourceEnum.Link, null),
							Name = source.Resources.GetValueByKey("name", _currentCulture),
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
				Link = source.Resources.GetValueByKey(SpecialResourceEnum.Link, null),
				Name = source.Resources.GetValueByKey("name", _currentCulture),
				Category = source.Category,
				Authorize = source.Authorize
			};

		}

		public static GridElement ToGridElement(this GridElementDto source, ApplicationSetting currentApplication, GridElement parent)
		{
			var a = new GridElement
			{
				Content = JsonConvert.SerializeObject(source.Content),
				Id = source.Id.IsEmpty() ? Guid.NewGuid() : source.Id,
				Parent = parent,
				Position = source.Position,
				//				Resources = source.Resources.to,
				Skin = source.Skin,
				Type = source.Type,
				Width = source.Width,
				AppliceSetting = currentApplication,
			};
			return a;
		}

		public static GridElement ToGridElement(this GridElementDto source, Grid grid, GridElement parent, ApplicationSetting currentApplication)
		{
			var gridElement = source.ToGridElement(currentApplication, parent);
			gridElement.Grid = grid;
			return gridElement;
		}

		public static UserDataDto ToDto(this UserData data)
		{
			return new UserDataDto
			{
				Id = data.Id,
				UserName = data.User.UserName.Split('_')[1],
				Key = data.Key,
				Created = data.Created.ToLocalTime(),
				Value = data.Value
			};
		}
		public static UserProfileDto ToDto(this UserProfile user)
		{
			return new UserProfileDto
			{
				Id = user.Id,
				UserName = user.UserName.Split('_')[1],
				ApplicationUser = user.ApplicationUser.HasValue && user.ApplicationUser.Value > 0
			};
		}

		public static IList<UserProfileDto> ToDtos(this IEnumerable<UserProfile> users)
		{
			return users.Select(ToDto).ToList();
		}

		public static IList<UserDataDto> ToDtos(this ICollection<UserData> data) 
		{
			return data.Select(ToDto).ToList();
		} 
	}
}