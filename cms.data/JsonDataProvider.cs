using System;
using System.Collections.Generic;
using cms.data.Dtos;
using cms.data.Models;

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
		public abstract GridElement Add(GridElement gridElement);

		public abstract IEnumerable<Grid> Grids();
		public abstract Grid GetGrid(int id);
		public abstract GridPageDto GetGridPage(int id);
		public abstract void DeleteGrid(int id);
		public abstract Grid Add(Grid newitem);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);
		public abstract void DeleteGridElement(int id);
		public abstract GridElement GetGridElement(int id);
		public abstract GridElement Update(GridElement item);
		public abstract GridPageDto GetGridPage(string link);
		public abstract T Add<T>(T newitem);
		public abstract Grid Update(Grid item);
		public abstract ApplicationSetting GetApplication(Guid id);
		public abstract ApplicationSetting GetApplication(string name);
	}
}

