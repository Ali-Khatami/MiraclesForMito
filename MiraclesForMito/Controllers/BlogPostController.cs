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
			
			// set the last updated date.
			post.UpdatedDate = DateTime.Now;

			db.Entry(post).State = EntityState.Modified;

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

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}
