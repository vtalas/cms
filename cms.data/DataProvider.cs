using System;
using System.Collections.Generic;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data
{
	public abstract class DataProvider :  IDisposable
	{
		protected int UserId { get; set; }
		protected string CurrentCulture { get { return shared.SharedLayer.Culture; } }

		public string ApplicationName { get; protected set; }
		public Guid ApplicationId { get; protected set; }
		public abstract ApplicationSetting CurrentApplication { get;}
	
		protected DataProvider(Guid applicationId, int userId)
		{
			ApplicationId  = applicationId;
			UserId = userId;
		}
		protected DataProvider(string applicationName, int userId)
		{
			ApplicationName = applicationName;
			UserId = userId;
		}

		public abstract MenuAbstract Menu { get; }
		public abstract PageAbstract Page { get; }
		public abstract GridElementAbstract GridElement { get; }
	
		public abstract IEnumerable<GridListDto> Grids();

		public abstract ApplicationSetting Add(ApplicationSetting newitem, int userId);
		public abstract void DeleteApplication(Guid guid);
		public abstract IEnumerable<ApplicationSetting> Applications();
		public abstract void Dispose();

	}

	public abstract class GridElementAbstract : DataProviderBase
	{
		protected GridElementAbstract(ApplicationSetting application) : base(application){}
		public abstract GridElement AddToGrid(GridElement gridElement, Guid gridId);
		public abstract void Delete(Guid guid, Guid gridid);
		public abstract GridElementDto Update(GridElementDto item);
		public abstract GridElementDto Get(Guid guid);
		
	}

	public abstract class PageAbstract : DataProviderBase
	{
		protected PageAbstract(ApplicationSetting application) : base(application) { }
		public abstract GridPageDto Get(string link);
		public abstract GridPageDto Get(Guid id);
		public abstract IEnumerable<GridPageDto> List();
		public abstract GridPageDto Add(GridPageDto newitem);
		public abstract GridPageDto Update(GridPageDto item);
		public abstract void Delete(Guid guid);
	}

	public abstract class MenuAbstract : DataProviderBase
	{
		protected MenuAbstract(ApplicationSetting application) : base(application) { }
		public abstract MenuDto Get(Guid guid);
		public abstract MenuDto Add(MenuDto newitem);
		public abstract Guid AddMenuItem(MenuItemDto newitem, Guid gridId);
		public abstract IEnumerable<MenuDto> List();
		public abstract void Delete(Guid guid);
	}
}

