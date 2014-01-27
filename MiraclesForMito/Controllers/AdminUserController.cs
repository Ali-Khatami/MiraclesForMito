using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MiraclesForMito.Models;

namespace MiraclesForMito.Controllers
{
	public class AdminUserController : ApiController
	{
		private SiteDB db = new SiteDB();
		
		// PUT api/AdminUser/5 (Update in CRUD)
		public HttpResponseMessage PutAdminUser(int id, AdminUser user)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			if (id != user.ID)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}

			db.Entry(user).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		// POST api/AdminUser (Create in CRUD)
		public HttpResponseMessage PostAdminUser(AdminUser user)
		{
			if (ModelState.IsValid)
			{
				db.Admins.Add(user);
				db.SaveChanges();

				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
				response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.ID }));
				return response;
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
		}

		// DELETE api/AdminUser/5 (Delete in CRUD)
		public HttpResponseMessage DeleteAdminUser(int id)
		{
			AdminUser user = db.Admins.Find(id);

			if (user == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}

			db.Admins.Remove(user);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			}

			return Request.CreateResponse(HttpStatusCode.OK, user);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}
