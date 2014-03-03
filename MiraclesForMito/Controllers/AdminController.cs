using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraclesForMito.Controllers.ActionFilters;
using MiraclesForMito.Models;
using MiraclesForMito.Utilities;
using Newtonsoft.Json;

namespace MiraclesForMito.Controllers
{
    public class AdminController : Controller
    {
		SiteDB db = new SiteDB();

        /// <summary>
        /// The login form for the admin site. Will auto redirect if logged in.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string email, string password)
        {
			if (db.Admins.Count() == 0)
			{
				db.Admins.Add(new AdminUser() { Email = "ali.d.khatami@gmail.com", FirstName = "Ali", LastName = "Khatami", Password = "Test123" });
				db.SaveChanges();
			}

			bool bAttemptedLogin = !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password);

			// create the user instace
			AdminUser userFromCredentials = null;

			// try to find the user from credentials
			if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
			{
				userFromCredentials = db.Admins.FirstOrDefault(user => user.Email.ToLower() == email.ToLower() && user.Password == password);
			}
						
			// check if user is logged in as an admin already
			if (UserUtils.CurrentUser != null || userFromCredentials != null)
			{
				// create a session cookie for the user then redirect them
				UserUtils.CreateEncryptedUserCookie((userFromCredentials != null) ? userFromCredentials.ID : UserUtils.CurrentUser.ID);

				// redirect to the events page which is the first link in the navigation
				Response.Redirect("~/Admin/Events");
			}

			// if we didn't redirect it means someone unsuccessfully tried to login
			if (bAttemptedLogin)
			{
				ViewBag.FailedLogin = true;
			}

            return View();
        }

		/// <summary>
		/// The login form for the admin site. Will auto redirect if logged in.
		/// </summary>
		/// <returns></returns>
		public ActionResult ForgotPassword(ForgotPasswordModel model)
		{
			// The user shouldn't be here
			if (UserUtils.CurrentUser != null)
			{
				// take them to the events page
				Response.Redirect("~/Admin/Events");
			}

			// set the user before trying to change the password
			model.SetDBInstance(db);
			// this will only change the password if someone actually passes the necessary strings
			model.SendNewPassword();

			// pass the model
			return View(model);
		}

		/// <summary>
        /// The login form for the admin site. Will auto redirect if logged in.
        /// </summary>
        /// <returns></returns>
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			// The user shouldn't be here
			if (UserUtils.CurrentUser == null)
			{
				// log them in
				Response.Redirect("~/Admin");
			}

			// set the user before trying to change the password
			model.SetUser(UserUtils.CurrentUser, db);
			// this will only change the password if someone actually passes the necessary strings
			model.ChangePassword();

			// pass the model
			return View(model);
		}

		/// <summary>
		/// Log out the user out and redirect them to the public landing page.
		/// </summary>
		public void Logout()
		{
			// delete the user cookie. Redirect them to the site
			UserUtils.DestroyEncryptedUserCookie();

			Response.Redirect("~/Admin");
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult Events()
		{
			return View();
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult PaginateEvents(PaginationModel model)
		{
			// deserialize the data
			var additionalData = JsonConvert.DeserializeObject<Dictionary<string, int>>(model.AdditionalData);

			// cast the value to the event type we want
			EventViewType viewType = (EventViewType)additionalData["EventType"];

			return PartialView(
				"~/Views/Admin/EventsPaginationBody.cshtml",
				new EventPaginationModel(viewType, db, model.PageIndex, model.PageSize)
			);
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult EventEdit(int? id)
		{
			return View(db.Events.Find(id));
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult BlogPosts()
		{
			return View(
				new PaginationModel()
				{
					PageIndex = 0,// start from the first page
					AJAXUrl = "~/Admin/PaginateBlogPosts", // where the ajax call should go
					PageSize = 4,
					TotalCount = db.BlogPosts.Count()
				}
			);
		}

		[AdminsOnly]
		public ActionResult PaginateBlogPosts(PaginationModel model)
		{
			return PartialView("~/Views/Admin/BlogPostsPaginationBody.cshtml", model);
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult BlogPostEdit(int? id)
		{
			return View(db.BlogPosts.Find(id));
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult AdminUsers()
		{
			return View();
		}

		[AdminsOnly]
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult AdminUserEdit(int? id)
		{
			return View(db.Admins.Find(id));
		}
    }
}
