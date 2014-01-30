using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MiraclesForMito.Models;
using System.Linq;

namespace MiraclesForMito.Controllers
{
	public class EventController : ApiController
	{
		private SiteDB db = new SiteDB();
		
		// PUT api/Event/5 (Update in CRUD)
		[HttpPut]
		public HttpResponseMessage Put(int id, Event mitoEvent)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			if (id != mitoEvent.ID)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}

			db.Entry(mitoEvent).State = EntityState.Modified;

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

		// POST api/Event (Create in CRUD)
		[HttpPost]
		public HttpResponseMessage PostEvent(Event mitoEvent)
		{
			if (ModelState.IsValid)
			{
				db.Events.Add(mitoEvent);
				db.SaveChanges();

				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, mitoEvent);
				response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = mitoEvent.ID }));
				return response;
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
		}

		// DELETE api/Event/5 (Delete in CRUD)
		[HttpDelete]
		public HttpResponseMessage DeleteEvent(int id)
		{
			Event mitoEvent = db.Events.Find(id);

			if (mitoEvent == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}

			// We need to find all blog posts with this event and clear their event link
			var posts = db.BlogPosts.Where(post => post.LinkedEvent != null && post.LinkedEvent.ID == id);

			if (posts != null && posts.Count() > 0)
			{
				foreach (var post in posts)
				{
					// set the value of each post's linked event to null 
					post.LinkedEvent = null;
				}
			}

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			}

			// once we have correctly removed the linked instances we can safely remove the event
			db.Events.Remove(mitoEvent);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			}

			return Request.CreateResponse(HttpStatusCode.OK, mitoEvent);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}
