using System;
using System.Collections.Generic;
using cms.data.Dtos;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.DataProvider
{
	public abstract class DataProviderAbstract
	{
		protected int UserId { get; set; }
		protected string CurrentCulture { get { return SharedLayer.Culture; } }

		public abstract string ApplicationName { get; }
		public Guid ApplicationId { get; protected set; }
		public abstract ApplicationSetting CurrentApplication { get;}
		public abstract bool IsUserAuthorized(int userId);

		protected DataProviderAbstract(Guid applicationId, int userId)
		{
			ApplicationId  = applicationId;
			UserId = userId;
		}

		public abstract MenuAbstract Menu { get; }
		public abstract PageAbstract Page { get; }
		public abstract IKeyValueStorage Settings { get; }
	
		public abstract IEnumerable<GridListDto> Grids();

		public abstract ApplicationSetting Add(ApplicationSetting newitem, int userId);
		public abstract void DeleteApplication(Guid guid);
		public abstract IEnumerable<ApplicationSetting> Applications();
	}

	public class CmsUsers
	{
		public CmsUsers(DataProviderAbstract datasource)
		{
		
		}
	}

	public abstract class PageAbstract : DataProviderBase
	{
		protected PageAbstract(ApplicationSetting application, IRepository repo) : base(application, repo) { }
		public abstract GridPageDto Get(string link);
		public abstract GridPageDto Get(Guid id);
		public abstract IEnumerable<GridPageDto> List();
		public abstract GridPageDto Add(GridPageDto newitem);
		public abstract GridPageDto Update(GridPageDto item);
		public abstract void Delete(Guid guid);
	}

	public abstract class MenuAbstract : DataProviderBase
	{
		protected MenuAbstract(ApplicationSetting application, IRepository repo) : base(application, repo) { }
		public abstract MenuDto Get(Guid guid);
		public abstract MenuDto Add(MenuDto newitem);
		public abstract MenuItemDto UpdateMenuItem(MenuItemDto menuItem);
		public abstract MenuItemDto AddMenuItem(MenuItemDto menuItem, Guid menuId);
		public abstract IEnumerable<MenuDto> List();
		public abstract void Delete(Guid guid);
	}
}

