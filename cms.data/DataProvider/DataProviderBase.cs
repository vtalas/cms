using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.DataProvider
{

	public class DataProviderBase : IGridElementDataProvider
	{
		protected ApplicationSetting CurrentApplication { get; set; }

		protected string CurrentCulture { get { return SharedLayer.Culture; } }

		protected IRepository db { get; set; }

		public DataProviderBase(ApplicationSetting application, IRepository repository )
		{
			CurrentApplication = application;
			db = repository;
		}

		protected IQueryable<Grid> AvailableGrids
		{
			get
			{
				var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id );
				return a;
			}
		}

		protected Grid GetGrid(string linkValue)
		{
			Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue(SpecialResourceEnum.Link, linkValue, null);
			var a = AvailableGrids.Include(x => x.Resources).ToList().FirstOrDefault(aa);
			return a;
		}

		protected Grid GetGrid(Guid id)
		{
			var grid = AvailableGrids.SingleOrDefault(x => x.Id == id);
			return grid;
		}

		protected bool LinkExist(string linkValue)
		{
			Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue(SpecialResourceEnum.Link, linkValue, null);
			return AvailableGrids.Include(x => x.Resources).ToList().Any(aa);
		}

		protected void ValidateLink(string linkValue)
		{
			if (LinkExist(linkValue))
			{
				throw new Exception(string.Format("item with link '{0}' allready Exists", linkValue));
			}
		}

		public GridElement AddToGrid(GridElement gridElement, Guid gridId)
		{
			var grid = GetGrid(gridId);
			if (grid == null)
			{
				throw new ObjectNotFoundException("grid not found");
			}

			grid.GridElements.AddToCorrectPosition(gridElement);
			db.SaveChanges();
			return gridElement;
		}

		public void Delete(Guid guid, Guid gridid)
		{
			throw new NotImplementedException();
		}

		public GridElementDto Update(GridElementDto item)
		{
			throw new NotImplementedException();
		}
	}

	public static class chghjshd
	{
		public static void AddToCorrectPosition(this ICollection<GridElement> list, GridElement newitem)
		{
			var ordered = list.OrderBy(x => x.Position);

			foreach (var gridElement in ordered.Where(gridElement => gridElement.Position >= newitem.Position))
			{
				gridElement.Position++;
			}
			list.Add(newitem);
		}
	}
}