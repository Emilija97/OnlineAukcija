using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAukcija.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(NeoDataLayer.DomainModel.User u)
        {
            if(u.role)
            {
                NeoDataLayer.DataProvider.RegisterOrganizer(u);
            }
            else
            {
                NeoDataLayer.DataProvider.RegisterUser(u);
            }
            return View();
        }
    }
}