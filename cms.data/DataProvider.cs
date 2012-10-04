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

		public abstract ApplicationSetting Add(ApplicationSetting newitem, int userId);
		public abstract MenuAbstract Menu { get; }
		public abstract PageAbstract Page { get; }
	
		public abstract IEnumerable<GridListDto> Grids();
		public abstract void DeleteApplication(Guid guid);
		
		///!! tohle este ocekovat
		public abstract GridElement AddGridElementToGrid(GridElement gridElement, Guid gridId);
		public abstract Grid GetGrid(Guid guid);
		public abstract void DeleteGrid(Guid guid);
		public abstract void DeleteGridElement(Guid guid, Guid gridid);
		public abstract GridElement GetGridElement(Guid guid);
		public abstract GridElementDto Update(GridElementDto item);

		public abstract void Dispose();

		public abstract IEnumerable<ApplicationSetting> Applications();
	}

	public abstract class PageAbstract : DataProviderBase
	{
		protected PageAbstract(ApplicationSetting application) : base(application) { }
		public abstract GridPageDto Get(string link);
		public abstract GridPageDto Get(Guid id);
		public abstract IEnumerable<GridPageDto> List();
		public abstract GridPageDto Add(GridPageDto newitem);
		public abstract GridPageDto Update(GridPageDto item);
	}

	public abstract class MenuAbstract : DataProviderBase
	{
		protected MenuAbstract(ApplicationSetting application) : base(application) { }
		public abstract MenuDto Get(Guid guid);
		public abstract MenuDto Add(MenuDto newitem);
		public abstract Guid AddMenuItem(MenuItemDto newitem, Guid gridId);
		public abstract IEnumerable<MenuDto> List();
	}
}

