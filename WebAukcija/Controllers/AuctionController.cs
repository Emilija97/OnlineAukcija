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
            bool tmp = NeoDataLayer.DataProvider.CreateAuction(auction);
            if (tmp)
                ViewBag.Message = "You successfully added new auction!";
            else
                ViewBag.Alert = "Lost connection with database!";
            return View();
        }

        public ActionResult Auctions()
        {
            List<Auction> auctions = NeoDataLayer.DataProvider.GetAllAuctions();
            return View(auctions);
        }

        [HttpPost]
        public ActionResult Auctions(string type)
        {
            List<Auction> auctions = NeoDataLayer.DataProvider.GetAuctionsByType(type);
            return View(auctions);
        }

        public ActionResult SeeSubjects(string title)
        {
            List<Subject> subjects = NeoDataLayer.DataProvider.GetAllSubjects(title);
            return View(subjects);
        }

        [HttpPost]
        public ActionResult SeeSubjects(int offerPrice, string itemName, string title)
        {
            User user = NeoDataLayer.Store.GetInstance().GetUser();
            bool tmp = NeoDataLayer.DataProvider.OfferPrice(title, itemName, offerPrice, user);
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
            if (tmp)
                ViewBag.Message = "You successfully added new subject on your auction!";
            else
                ViewBag.Alert = "You enter wrong name for auction!";
            return View();
        }

        public ActionResult DeleteAuction(string title)
        {
            NeoDataLayer.DataProvider.DeleteAuction(title);
            return Redirect("Index");
        }
    }
}