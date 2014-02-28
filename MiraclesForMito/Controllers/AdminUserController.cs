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
using MiraclesForMito.Utilities;

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
				string sPasswordToSave = System.Web.Security.Membership.GeneratePassword(8, 5);
				// create a random password
				user.Password = sPasswordToSave;
				// force them to change their password next time they login
				user.ForceChangePassword = true;

				db.Admins.Add(user);
				db.SaveChanges();

				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
				response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.ID }));

				EmailHelpers.SendEmail(
					new System.Net.Mail.MailMessage(EmailHelpers.SEND_EMAIL_ADDRESS, user.Email)
					{
						Subject = "Miracles for Mito -- User Account",
						Body = @"Dear " + user.FirstName + " " + user.LastName + @",<br/><br/>
							You have been added as a Miracles for Mito Admin user. Your credentials are as follows:<br/><br/>
							<strong>Username:</strong> <em>[this email]</em><br/>
							<strong>Password:</strong> " + sPasswordToSave + @"<br/><br/>
						
						Sincerely,<br/>
						The Miracles for Mito Dev Team
						",
						IsBodyHtml = true
					}
				);

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
