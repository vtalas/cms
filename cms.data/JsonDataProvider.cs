using System;
using System.Collections.Generic;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data
{

	public abstract class JsonDataProvider
	{
		public string ApplicationName { get; protected set; }
		public Guid ApplicationId { get; protected set; }
		public abstract ApplicationSetting CurrentApplication { get; }

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
		public abstract IEnumerable<Grid> Grids();
		public abstract Grid GetGrid(Guid guid);
		public abstract GridPageDto GetGridPage(Guid guid);
		public abstract void DeleteGrid(Guid guid);
		public abstract GridPageDto Add(GridPageDto newitem);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);
		public abstract void DeleteGridElement(Guid guid, Guid gridid);
		public abstract GridElement GetGridElement(Guid guid);
		public abstract GridElement Update(GridElement item);
		public abstract GridPageDto GetGridPage(string link);
		public abstract GridPageDto Update(GridPageDto item);
		public abstract ApplicationSetting GetApplication(Guid id);
		public abstract ApplicationSetting GetApplication(string name);
		public abstract IEnumerable<GridPageDto> GridPages();
	}
}

