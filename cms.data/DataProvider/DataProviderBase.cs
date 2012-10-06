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

	public class DataProviderBase : IDisposable
	{
		protected ApplicationSetting CurrentApplication { get; set; }

		protected string CurrentCulture
		{
			get { return SharedLayer.Culture; }
		}

		public IRepository Repository { get; set; }

		public DataProviderBase(ApplicationSetting application, IRepository repository)
		{
			CurrentApplication = application;
			Repository = repository;
		}

		protected IQueryable<Grid> AvailableGrids
		{
			get
			{
				var a = Repository.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id);
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
			var gridElement = Repository.GridElements.SingleOrDefault(x => x.Id == id);

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
			Repository.SaveChanges();
			return gridElement;
		}

		public void Delete(Guid guid, Guid gridid)
		{
			var itemDb = GetGridElement(guid);
			itemDb.Resources.ToList().ForEach(resource => Repository.Remove(resource));
			Repository.Remove(itemDb);
			Repository.SaveChanges();
		}

		public GridElementDto Update(GridElementDto item)
		{
			var itemDb = GetGridElement(item.Id);
			var newParent = string.IsNullOrEmpty(item.ParentId) ? null : GetGridElement(new Guid(item.ParentId));

			itemDb.RefreshPositions(item, newParent);
			itemDb.UpdateResourceList(item.ResourcesLoc, CurrentCulture, Repository);
			itemDb.Content = item.Content;
			itemDb.Parent = newParent;
			itemDb.Skin = item.Skin;
			Repository.SaveChanges();
			return itemDb.ToDto();
		}

		public void Dispose()
		{
			if (Repository != null)
			{
				Repository.Dispose();
			}
		}
	}

	public static class PositionOnGridElementExtension
	{
		public static void AddToCorrectPosition(this ICollection<GridElement> allGridElements, GridElement newitem)
		{
			var ordered = allGridElements.OrderBy(x => x.Position);

			foreach (
				var gridElement in
					ordered.Where(gridElement => gridElement.Position >= newitem.Position && gridElement.Parent == newitem.Parent))
			{
				gridElement.Position++;
			}
			allGridElements.Add(newitem);
		}

		public static void CorrectPostion(this IEnumerable<GridElement> allGridElements, int oldPosition, int newPosition)
		{
			var ordered = allGridElements.OrderBy(x => x.Position);

			foreach (var current in ordered)
			{
				if (current.Position >= oldPosition && current.Position <= newPosition)
				{
					current.Position--;
				}
				if (current.Position >= newPosition && current.Position <= oldPosition)
				{
					current.Position++;
				}
			}
		}

		public static void CorrectPostionSoftAdd(this IEnumerable<GridElement> allGridElements, int newPosition)
		{
			var ordered = allGridElements.OrderBy(x => x.Position);

			foreach (var current in ordered.Where(current => current.Position >= newPosition))
				current.Position++;
		}

		public static void CorrectPostionSoftRemove(this IEnumerable<GridElement> allGridElements, int oldPosition)
		{
			var ordered = allGridElements.OrderBy(x => x.Position);

			foreach (var current in ordered.Where(current => current.Position >= oldPosition))
			{
				current.Position--;
			}
		}
	}
}