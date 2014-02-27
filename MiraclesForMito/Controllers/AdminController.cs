using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraclesForMito.Models;

namespace MiraclesForMito.Controllers
{
    public class AdminController : Controller
    {
		SiteDB db = new SiteDB();

        /// <summary>
        /// The login form for the admin site. Will auto redirect if logged in.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
			// check if user is logged in as an admin already
			if (false)
			{
				Response.Redirect("~/Admin/Events");
			}
            return View();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult Events()
		{
			return View();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult EventEdit(int? id)
		{
			return View(db.Events.Find(id));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult BlogPosts()
		{
			return View();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult BlogPostEdit(int? id)
		{
			return View(db.BlogPosts.Find(id));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult AdminUsers()
		{
			if (db.Admins.Count() == 0)
			{
				db.Admins.Add(new AdminUser() { Email = "ali.d.khatami@gmail.com", FirstName = "Ali", LastName = "Khatami", Password = "Miracles1" });
				db.SaveChanges();
			}

			return View();
		}

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
