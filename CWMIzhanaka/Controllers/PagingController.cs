using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Controllers
{
    public class PagingController : Controller
    {
        // Display stored page from database
        // GET: Index/(page)
        public ActionResult Index(string pg ="")
        {
            //If nothing was set pg to "home"
            if(pg == "")
            {
                pg = "home"; 
            }

            PageViewModel pvm;
            PageDataHandler pdh;

            using( DataBase db = new DataBase())
            {
                //If page was not found go home
                if(!db.Pages.Any(x => x.Slug == pg)){
                    return RedirectToAction("Index", new { pg = "" });
                }
            }

            using (DataBase db = new DataBase())
            {
                //Take desired page data using slug attribute
                pdh = db.Pages.Where(x => x.Slug == pg).FirstOrDefault(); 
               
            }

            ViewBag.pgTitle = pdh.Title;
            //Check if page has sidebar
            if (pdh.HasSidebar)
            {
                ViewBag.SB = "True";
            } else
            {
                ViewBag.SB = "false"; 
            }

            pvm = new PageViewModel(pdh);
            //Display the page
            return View(pvm);
        }
        //Used to add added pages to Navbar
        public ActionResult PgMenuView()
        {
            List<PageViewModel> listPVM;

            using (DataBase db = new DataBase())
            {
                listPVM = db.Pages
                    .ToArray()
                    .OrderBy(a => a.Sorting)
                    .Where(a => a.Slug != "home")
                    .Select(a => new PageViewModel(a))
                    .ToList(); 
            }

            return PartialView(listPVM); 
        }
        public ActionResult SideBarView()
        {
            SidebarViewModel svm;
            using (DataBase db = new DataBase())
            {
                SideBarDataHandler sbdh = db.Sidebar.Find(1);

                svm = new SidebarViewModel(sbdh);
            }
            return PartialView(svm);

        }

    }
}