using System;
using System.Collections.Generic;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data
{
	public abstract class JsonDataProvider :  IDisposable
	{
		public string ApplicationName { get; protected set; }
		public Guid ApplicationId { get; protected set; }
		public abstract ApplicationSetting CurrentApplication { get; }
		protected string CurrentCulture {get { return shared.SharedLayer.Culture; }}

		protected JsonDataProvider(Guid applicationId )
		{
			ApplicationId  = applicationId;
		}
		
		protected JsonDataProvider(string application )
		{
			ApplicationName = application;
		}

		public abstract MenuAbstract Menu { get; }
		public abstract PageAbstract Page { get; }
		public abstract IEnumerable<GridListDto> Grids();

		///!! tohle este ocekovat
		public abstract void DeleteApplication(Guid guid);
		public abstract GridElement AddGridElementToGrid(GridElement gridElement, Guid gridId);
		public abstract Grid GetGrid(Guid guid);
		public abstract void DeleteGrid(Guid guid);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);
		public abstract void DeleteGridElement(Guid guid, Guid gridid);
		public abstract GridElement GetGridElement(Guid guid);
		public abstract GridElementDto Update(GridElementDto item);
		public abstract GridPageDto Update(GridPageDto item);
		public abstract ApplicationSetting GetApplication(Guid id);
		public abstract ApplicationSetting GetApplication(string name);
		public abstract IEnumerable<ApplicationSettingDto> Applications();
		
		
		public abstract void Dispose();
	}

	public abstract class PageAbstract : DataProviderBase
	{
		protected PageAbstract(Guid applicationId) : base(applicationId){}
		public abstract GridPageDto Get(string link);
		public abstract GridPageDto Get(Guid id);
		public abstract IEnumerable<GridPageDto> List();
		public abstract GridPageDto Add(GridPageDto newitem);
	}

	public abstract class MenuAbstract : DataProviderBase
	{
		protected MenuAbstract(Guid applicationId) : base(applicationId){}
		public abstract MenuDto Get(Guid guid);
		public abstract MenuDto Add(MenuDto newitem);
		public abstract Guid AddMenuItem(MenuItemDto newitem, Guid gridId);
		public abstract IEnumerable<MenuDto> List();
	}

}

