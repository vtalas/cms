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

	public class DataProviderBase 
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

		protected GridElement GetGridElement(Guid id)
		{
			var gridElement = db.GridElements.SingleOrDefault(x => x.Id == id);

			if (gridElement == null || !AvailableGrids.Any(x => x.Id == gridElement.Grid.Id))
			{
				throw new ObjectNotFoundException("grid element not found");
			}
			return gridElement;
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

		public GridElement Update(GridElement item)
		{
			var gridlementOld = GetGridElement(item.Id);
			if (item.Grid == null)
			{
				throw new ObjectNotFoundException("grid element owner not found");
			}
			var parents = item.Grid.GridElements.Where(x=>x.Parent == item.Parent);

			var oldPosition = gridlementOld.Position;
			var newPosition = item.Position;

			foreach (var current in parents.OrderBy(x => x.Position))
			{
				var movedDown = oldPosition < newPosition;
				if (movedDown)
				{
					if (current.Position >= oldPosition && current.Position <= newPosition)
					{
						current.Position--;
					}
				}
				else
				{
					if (current.Position >= newPosition && current.Position <= oldPosition)
					{
						current.Position++;
					}
				}
			}

			var parentsCount = parents.Count();
			gridlementOld.Content = item.Content;
			gridlementOld.Grid = item.Grid;
			gridlementOld.Parent = item.Parent;
			gridlementOld.Position = item.Position > parentsCount && parentsCount > 0 ? parentsCount - 1  : item.Position;
			gridlementOld.Resources = item.Resources;
			gridlementOld.Skin = item.Skin;

			db.SaveChanges();

			return gridlementOld;
		}
	}

	public static class chghjshd
	{
		public static void AddToCorrectPosition(this ICollection<GridElement> allGridElements, GridElement newitem)
		{
			var ordered = allGridElements.OrderBy(x => x.Position);

			foreach (var gridElement in ordered.Where(gridElement => gridElement.Position >= newitem.Position && gridElement.Parent == newitem.Parent))
			{
				gridElement.Position++;
			}
			allGridElements.Add(newitem);
		}
	}
}