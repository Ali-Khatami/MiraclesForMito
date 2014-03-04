using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MiraclesForMito.Models;

namespace MiraclesForMito.Controllers
{
	public class NotifiedListItemController : ApiController
	{
		private SiteDB db = new SiteDB();
		
		// POST api/NotifiedListItem (Create in CRUD)
		public HttpResponseMessage PostNotifiedListItem(NotifiedListItem item)
		{
			if (ModelState.IsValid)
			{
				// check to see if email is already added. We only need to add it once, but we
				// want to return a success because the request didn't fail
				if (db.NotifiedList.FirstOrDefault(currItem => currItem.Email == item.Email) != null)
				{
					return Request.CreateResponse(HttpStatusCode.OK, ModelState);
				}

				db.NotifiedList.Add(item);
				db.SaveChanges();

				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, item);
				response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = item.ID }));

				return response;
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}
