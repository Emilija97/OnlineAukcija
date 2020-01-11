using NeoDataLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAukcija.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AuctionPortal()
        {
            ViewBag.Title = "Home Page";
            User u = NeoDataLayer.Store.GetInstance().GetUser();
            return View(u);
        }
    }
}
