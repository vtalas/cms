using System;
using System.Collections.Generic;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.DataProvider;
using cms.data.EF.RepositoryImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using System.Linq;

namespace cms.data.tests._Common
{
	public static class GridAssertsHelper
	{
		public static Grid Check(this Grid item)
		{
			Assert.IsNotNull(item);
			return item;
		}

		public static Grid HasGridElementsCountAndValid(this Grid item, int expectedTotalCount)
		{
			var orderByParent = item.GridElements.Where(x=>x.Parent!=null).OrderBy(x=>x.Parent.Id);

			var tempParant = Guid.NewGuid();
			foreach (var gridElement in orderByParent)
			{
				if (tempParant != gridElement.Parent.Id)
				{
					orderByParent.Where(x => x.Parent.Id == gridElement.Parent.Id).HasGridElementsValid();
				}
				tempParant = gridElement.Parent.Id;
			}
			
			item.GridElements.Where(x => x.Parent == null).HasGridElementsValid();

			Assert.AreEqual(expectedTotalCount, item.GridElements.Count);
			

			return item;
		}
		
		public static IEnumerable<GridElement> HasGridElementsValid(this IEnumerable<GridElement> listWithSameParent)
		{
			foreach (var gridelement in listWithSameParent)
			{
				Assert.AreEqual(1, listWithSameParent.Count(x => x.Position == gridelement.Position), "position " + gridelement.Position + " is more then once");
			}
			return listWithSameParent;
		}

		public static Grid HasGridElementIndex(this Grid item, GridElement parent, int expectedIndex)
		{
			var index = item.GridElements.Where(x => x.Parent == parent).OrderBy(a => a.Position).ToList().FindIndex(a => a.Position == expectedIndex);
			Assert.AreEqual(expectedIndex, index);
			return item;
		}
	}

	public static class ResourcesAssertsHelper
	{

		public static Dictionary<string, ResourceDtoLoc> EmptyResourcesDto()
		{
			return new Dictionary<string, ResourceDtoLoc>();
		}

		public static Resource CheckResource(this IEntityWithResource item, string key, string culture)
		{
			var getbyLink = item.Resources.GetByKey(key, culture);
			Assert.IsNotNull(getbyLink);
			return getbyLink;
		}

		public static void HasResoucesCount(this IEntityWithResource item, int expected)
		{
			Assert.AreEqual(expected, item.Resources.Count);
		}

		public static Resource ValueIs(this Resource item, string value)
		{
			Assert.AreEqual(value, item.Value);
			return item;
		}

		public static Resource IdIs(this Resource item, Guid expected)
		{
			Assert.AreEqual(expected, item.Id);
			return item;
		}

		public static Resource OwnerIs(this Resource item, Guid expectedId)
		{
			Assert.AreEqual(expectedId, item.Owner);
			return item;
		}

		public static Dictionary<string, ResourceDtoLoc> WithResource(this Dictionary<string, ResourceDtoLoc> item, string key, string value, int id)
		{
			item.Add(key, new ResourceDtoLoc() { Id = id, Value = value });
			return item;
		}

		public static void TotalResourcesCount_Check(this IRepository repository, int expected)
		{
			Assert.AreEqual(expected, repository.Resources.Count());
		}

		public static void TotalGridElementsCount_Check(this IRepository repository, int expected)
		{
			Assert.AreEqual(expected, repository.GridElements.Count());
		}

	}

}
