using NeoDataLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAukcija.Controllers
{
    public class AuctionController : Controller
    {
        // GET: Auction
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(NeoDataLayer.DomainModel.Auction auction)
        {
            NeoDataLayer.DataProvider.CreateAuction(auction);
            return View();
        }

        public ActionResult Auctions()
        {
            List<Auction> auctions= NeoDataLayer.DataProvider.GetAllAuctions();
            return View(auctions);
        }

        public ActionResult SeeSubjects(string title)
        {
            List<Subject> subjects = NeoDataLayer.DataProvider.GetAllSubjects(title);
            return View(subjects);
        }

        [HttpPost]
        public ActionResult SeeSubjects(int offerPrice,string itemName, string title)
        {
            bool tmp = NeoDataLayer.DataProvider.OfferPrice(title, itemName, offerPrice);
            List<Subject> subjects = NeoDataLayer.DataProvider.GetAllSubjects(title);
            return View(subjects);
        }

        public ActionResult Subject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subject(string title, NeoDataLayer.DomainModel.Subject s)
        {
            User user = NeoDataLayer.Store.GetInstance().GetUser();
            bool tmp = NeoDataLayer.DataProvider.AddSubject(title, s, user);
            if(tmp)
                ViewBag.Message = "You successfully added new subject on your auction!";
            return View();
        }
    }
}