using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using cms.data.EF;
using cms.data.Shared;
using WebMatrix.WebData;
using cms.Code;

namespace cms.Controllers
{
	[Authorize(Roles = "superuser")]
	public class SuperUserController : CmsControllerBase
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Applications()
		{
			using (var repository = new RepositoryFactory())
			{
				return View(repository.Create.ApplicationSettings.ToList());
			}
		}

		[HttpPost]
		public ActionResult ApplicationActions(string username, Guid id, string command)
		{
			switch (command)
			{
				case "add":
					ActionAddUserToApp(username, id);
					break;
				case "remove":
					ActionRemoveUserFromApp(username, id);
					break;
			}
			return RedirectToAction("ApplicationDetail", new {id = id});
		}

		private void ActionAddUserToApp(string username, Guid id)
		{
			using (var repository = new RepositoryFactory())
			{
				var db = repository.Create;

				var app = db.ApplicationSettings.SingleOrDefault(x => x.Id == id);
				var user = db.UserProfile.SingleOrDefault(x => x.UserName == username);
				app.Users.Add(user);
				db.SaveChanges();
			}
		}
		private void ActionRemoveUserFromApp(string username, Guid id)
		{
			using (var repository = new RepositoryFactory())
			{
				var db = repository.Create;

				var app = db.ApplicationSettings.SingleOrDefault(x => x.Id == id);
				var user = db.UserProfile.SingleOrDefault(x => x.UserName == username);
				app.Users.Remove(user);
				db.SaveChanges();
			}
		}

		[HttpPost]
		public ActionResult RemoveUserFromApplication(string username, Guid id)
		{
			using (var repository = new RepositoryFactory())
			{
				var db = repository.Create;

				var app = db.ApplicationSettings.SingleOrDefault(x => x.Id == id);
				var user = db.UserProfile.SingleOrDefault(x => x.UserName == username);
				app.Users.Remove(user);
				db.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public ActionResult ApplicationDetail(Guid id)
		{
			using (var repository = new RepositoryFactory())
			{
				ViewBag.Users = repository.Create.UserProfile.ToList();
				var app = repository.Create.ApplicationSettings.Include(x => x.Users).ToList().SingleOrDefault(x => x.Id == id);
				return View(app);
			}
		}

		public ActionResult ListUsers()
		{
			using (var repository = new RepositoryFactory())
			{
				return View(repository.Create.UserProfile.ToList());
			}
		}

		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
					Roles.AddUserToRole(model.UserName, "admin");
					WebSecurity.Login(model.UserName, model.Password);
					return RedirectToAction("Index", "Home");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}
			return View(model);
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}

	}
}
