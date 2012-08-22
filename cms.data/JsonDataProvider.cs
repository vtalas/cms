using System;
using System.Collections.Generic;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data
{

	public abstract class JsonDataProvider: IResources,IDisposable,IMenu
	{
		public string ApplicationName { get; protected set; }
		public Guid ApplicationId { get; protected set; }
		public abstract ApplicationSetting CurrentApplication { get; }
		protected string CurrentCulture {get { return shared.SharedLayer.Culture; }}

		protected JsonDataProvider(Guid applicationId )
		{
			this.ApplicationId  = applicationId;
		}
		
		protected JsonDataProvider(string application )
		{
			ApplicationName = application;
		}

		public abstract void DeleteApplication(Guid guid);
		public abstract GridElement AddGridElementToGrid(GridElement gridElement, Guid gridId);
		public abstract Grid GetGrid(Guid guid);
		public abstract GridPageDto GetGridPage(Guid guid);
		public abstract void DeleteGrid(Guid guid);
		public abstract GridPageDto Add(GridPageDto newitem);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);
		public abstract void DeleteGridElement(Guid guid, Guid gridid);
		public abstract GridElement GetGridElement(Guid guid);
		public abstract GridElementDto Update(GridElementDto item);
		public abstract GridPageDto GetGridPage(string link);
		public abstract GridPageDto Update(GridPageDto item);
		public abstract ApplicationSetting GetApplication(Guid id);
		public abstract ApplicationSetting GetApplication(string name);
		public abstract IEnumerable<GridPageDto> GridPages();
		
		//public abstract ResourceDto Add(ResourceDto resource);
		//public abstract ResourceDto GetResourceDto(Guid elementId, string key, string culture);
		

		
		public abstract void Dispose();
		public abstract IEnumerable<ApplicationSettingDto> Applications();
	}

	public interface IMenu
	{
		//MenuDto GetGrid(Guid guid);
	}


	public interface IResources
	{
		//ResourceDto Add(ResourceDto resource);
		//ResourceDto GetResourceDto(Guid elementId, string key, string culture);
	}
}

