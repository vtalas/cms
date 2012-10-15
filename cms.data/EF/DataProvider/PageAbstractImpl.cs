using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProvider
{
	public class PageAbstractImpl : PageAbstract
	{
		private EfContext db { get; set; }
		
		public PageAbstractImpl(Guid applicationId) : base(applicationId){}
		public PageAbstractImpl(Guid applicationId, EfContext context) : base(applicationId)
		{
			db = context;
		}

		IQueryable<Grid> AvailableGridsPage()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == ApplicationId && x.Category == CategoryEnum.Page);
			return a;
		}
		ApplicationSetting CurrentApplication { get { return db.ApplicationSettings.Single(x => x.Id == ApplicationId); } }

		public override IEnumerable<GridPageDto> List()
		{
			var a = AvailableGridsPage().ToList();
			return a.Select(grid => grid.ToGridPageDto()).ToList();
		}

		public override GridPageDto Get(Guid id)
		{
			var grid = AvailableGridsPage().Single(x => x.Id == id);
			return grid.ToGridPageDto();
		}

		//TODO: pokud nenanjde melo by o vracet homepage
		public override GridPageDto Get(string link)
		{
			var a = AvailableGridsPage().FirstOrDefault(x => x.Resource.Value == link);
			//var a = Grids().FirstOrDefault(x => x.Resource.Value == link);
			if (a == null)
			{
				throw new ObjectNotFoundException(string.Format("'{0}' not found", link));
			}
			//if (a.Count() > 1)
			//{

			//}
			return a.ToGridPageDto();
		}

		void CheckIfLinkExist(GridPageDto newitem)
		{
			var linkExist = AvailableGridsPage().Any(x => x.Resource.Value == newitem.ResourceDto.Value);
			if (linkExist)
			{
				throw new ArgumentException("link exists");
			}
		}

		public override GridPageDto Add(GridPageDto newitem)
		{
			var item = newitem.ToGrid();

			if (newitem.ResourceDto == null)
			{
				newitem.ResourceDto = new ResourceDtoLoc { Value = item.Name.Replace(" ", string.Empty) };
			}

			CheckIfLinkExist(newitem);

			CurrentApplication.Grids.Add(item);
			db.Grids.Add(item);

			var res = db.Resources.Add(newitem.ResourceDto.ToResource());
			res.Owner = item.Id;
			item.Resource = res;

			db.SaveChanges();
			return item.ToGridPageDto();
		}

	}
}