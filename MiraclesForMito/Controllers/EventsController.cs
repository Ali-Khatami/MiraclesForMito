using MiraclesForMito.Models;
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
            return View(this._db.Events);
        }

        public ActionResult Detail(int id)
        {
            return View(this._db.Events.FirstOrDefault(e => e.ID == id));
        }
    }
}
