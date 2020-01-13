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
            List<Auction> auctions = NeoDataLayer.DataProvider.GetAllOrganizatorAuctions();
            return View(auctions);
        }

        public ActionResult MakePurchase(string title)
        {
            NeoDataLayer.DataProvider.MakePurchase(title);
            List<Auction> auctions = NeoDataLayer.DataProvider.GetAllOrganizatorAuctions();
            return View(auctions);
        }

        public ActionResult Purchases()
        {
            List<Subject> subjects = NeoDataLayer.DataProvider.GetAllPurchases(NeoDataLayer.Store.GetInstance().loggedUser);
            return View(subjects);
        }

        public ActionResult LicitatedItems()
        {
            List<Subject> subjects = NeoDataLayer.DataProvider.GetAllLicitatedSubjects(NeoDataLayer.Store.GetInstance().loggedUser);
            return View(subjects);
        }
    }
}
