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
	public class BlogPostController : ApiController
	{
		private SiteDB db = new SiteDB();
		
		// PUT api/BlogPost/5 (Update in CRUD)
		public HttpResponseMessage PutBlogPost(int id, BlogPost post)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			if (id != post.ID)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}

			// figure out if the post was previously published
			// we have to use a new context to mess with the states
			bool bWasThePostPreviouslyPublished = new SiteDB().BlogPosts.Find(id).Published.GetValueOrDefault(false);
			
			// set the last updated date.
			post.UpdatedDate = DateTime.Now;

			// check to make sure the SEO link is unique
			if (db.BlogPosts.FirstOrDefault(currPost => currPost.SEOLink == post.SEOLink && currPost.ID != id) != null)
			{
				ModelState.AddModelError("SEOLink", "The SEO link must be unique and cannot match an existing blog post");

				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			db.Entry(post).State = EntityState.Modified;

			try
			{
				db.SaveChanges();

				// if the post was not previously published, but now is, we need to send emails to those
				// that were signed up to receive emails.
				if (!bWasThePostPreviouslyPublished && post.Published.GetValueOrDefault(false))
				{
					foreach (var notificationItem in db.NotifiedList)
					{
						SendPostedEmail(notificationItem.Email, post);
					}
				}
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		// POST api/BlogPost (Create in CRUD)
		public HttpResponseMessage PostBlogPost(BlogPost post)
		{
			if (ModelState.IsValid)
			{
				post.InsertDate = DateTime.Now;
				post.UpdatedDate = DateTime.Now;
				db.BlogPosts.Add(post);
				db.SaveChanges();

				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, post);
				response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = post.ID }));

				// if the post is saved and the post has published set to true we need to send emails
				if (post != null && post.Published.GetValueOrDefault(false))
				{
					foreach (var notificationItem in db.NotifiedList)
					{
						SendPostedEmail(notificationItem.Email, post);
					}
				}

				return response;
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
		}

		// DELETE api/BlogPost/5 (Delete in CRUD)
		public HttpResponseMessage DeleteBlogPost(int id)
		{
			BlogPost post = db.BlogPosts.Find(id);

			if (post == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}

			db.BlogPosts.Remove(post);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			}

			return Request.CreateResponse(HttpStatusCode.OK, post);
		}

		private void SendPostedEmail(string email, BlogPost post)
		{
			EmailHelpers.SendEmail(
				new System.Net.Mail.MailMessage(EmailHelpers.SEND_EMAIL_ADDRESS, email)
				{
					Subject = "Miracles for Mito New Blog Post!",
					Body = "The Miracles for Mito Blog team has posted a new blog post, <a title=\"Click here to visit the Miracles for Mito Blog\" href=\"" +
						(string.Format("~/Blog/Post/{0}", post.SEOLink)).ToAbsoluteUrl() +
					"\">" + post.Title + "</a>.<br/><br/>" +
					@"Sincerely,<br/>
					The Miracles for Mito Dev Team<br/><br/>" +
					"<a href=\"" + (string.Format("~/Unsubscribe/{0}", SimpleCrypto.Encrypt(email))).ToAbsoluteUrl() + "\">Click here to unsubscribe from these emails</a>",
					IsBodyHtml = true
				}
			);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}
