using System;
using NUnit.Framework;
using cms.data.DataProvider;
using cms.data.EF.DataProviderImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.tests._Common;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class GridElementAbstract_Test : InjectableBase_Test
	{
		private GridElementAbstract _implemtation;
		private readonly Guid _gridIdFromApp2 = new Guid("1111111e-1115-480b-9ab7-a3ab3c0f6643");
		private int _gridsCountBefore;
		private int _resourcesCountBefore;

		public GridElementAbstract_Test()
			: this(new Base_MockDb().GetRepositoryMock)
		{
		}

		public GridElementAbstract_Test(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();

			var app1 = AApplication("prd App")
				.WithGrid(
					AGrid()
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					AGrid(_gridId)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 221)
						.WithResource("name", "NAME BBB EN", CultureEn, 222)
						.WithGridElement(AGridElement("text", 0 ))
						.WithGridElement(AGridElement("text", 1 ))
						.WithGridElement(
								AGridElement("text", 2 )
									.WithResource("aaa","aaavalue")
								)
				).AddTo(Repository);
			_implemtation = new GridElementAbstractImpl(app1, Repository);
		}


		[Test]
		public void aaa()
		{
			var el = _implemtation.AddToGrid(AGridElement("text", 1), _gridId);
			Assert.AreEqual(1, el.Position);
		}

		[Test]
		public void xxx()
		{
			var el = _implemtation.AddToGrid(AGridElement("text").WithResource("xxx","xxxvalue"), Guid.NewGuid());
			
			Console.WriteLine(el.Id);
			Console.WriteLine(el.Resources.GetByKey("xxx",CultureCs).Value);
			Console.WriteLine(el.Resources.GetByKey("xxx",CultureEn).Value);
			Console.WriteLine(el.Position);
		}

	}
}