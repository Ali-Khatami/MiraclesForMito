using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraclesForMito.Models;
using Newtonsoft.Json;

namespace MiraclesForMito.Controllers
{
    public class BlogController : Controller
    {
		SiteDB db = new SiteDB();

        public ActionResult Index(BlogPaginationModel model)
        {
			model.AJAXUrl = model.AJAXUrl ?? "~/Blog/Paginate";
			return View(model);
        }

		public ActionResult Paginate(PaginationModel model)
		{
			return PartialView(
				"~/Views/Blog/BlogPostsPaginationBody.cshtml",
				new BlogPaginationModel(db, model.PageIndex, model.PageSize)
			);
		}

		public ActionResult Post()
		{
			string sPostTitle = RouteData.Values["id"].ToString();
			return View("Index", new BlogPaginationModel(db, BlogFilterType.SEOTitle, sPostTitle));
		}

		public ActionResult Author()
		{
			string sAuthorName = RouteData.Values["id"].ToString();
			var model = new BlogPaginationModel(db, BlogFilterType.Author, sAuthorName);
			model.AJAXUrl = "~/Blog/AuthorPaginate";
			return View("Index", model);
		}

		public ActionResult AuthorPaginate(PaginationModel model)
		{
			// deserialize the data
			var additionalData = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.AdditionalData);

			// cast the value to the event type we want
			BlogFilterType filterType = (BlogFilterType)int.Parse(additionalData["FilterType"]);
			string filterValue = additionalData["FilterValue"];

			return PartialView(
				"~/Views/Blog/BlogPostsPaginationBody.cshtml",
				new BlogPaginationModel(db, filterType, filterValue, model.PageIndex, model.PageSize)
			);
		}
    }
}
