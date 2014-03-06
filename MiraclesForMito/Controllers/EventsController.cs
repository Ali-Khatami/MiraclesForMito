using MiraclesForMito.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiraclesForMito.Controllers
{
    public class EventsController : Controller
    {
        private SiteDB _db;
        
        public EventsController()
        {
            _db = new SiteDB();
        }
        
        public ActionResult Index()
        {
            return View(_db);
        }

        public ActionResult Detail(int id)
        {
            return View(this._db.Events.FirstOrDefault(e => e.ID == id));
        }

        public ActionResult PaginateEvents(PaginationModel model)
        {
            // deserialize the data
            var additionalData = JsonConvert.DeserializeObject<Dictionary<string, int>>(model.AdditionalData);

            // cast the value to the event type we want
            EventViewType viewType = (EventViewType)additionalData["EventType"];

            return PartialView(
                "~/Views/Events/EventsPaginationBody.cshtml",
                new EventPaginationModel(viewType, _db, model.PageIndex, model.PageSize)
            );
        }
    }
}
