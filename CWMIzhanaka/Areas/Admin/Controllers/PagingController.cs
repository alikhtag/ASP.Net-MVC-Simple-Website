using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagingController : Controller
    {
        // GET: Admin/Paging
        //Home page of paging
        public ActionResult Index()
        {
            List<PageViewModel> pageList;

            using (DataBase db = new DataBase())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageViewModel(x)).ToList();
            }
            //Display list of pages
            return View(pageList);
        }
        // GET: Admin/Paging/AddNewPage
        [HttpGet]
        public ActionResult AddNewPage()
        {
            return View();
        }
        // Used to handle input about the new page
        // Post: Admin/Paging/AddNewPage
        [HttpPost]
        public ActionResult AddNewPage(PageViewModel mdl)
        {
            // Check if the model state is right
            if (!ModelState.IsValid)
            {
                return View(mdl);
            }
            // Put data into the page database
            using (DataBase db = new DataBase())
            {
                string slug;

                PageDataHandler pdh = new PageDataHandler();

                pdh.Title = mdl.Title;

                // If slug was not entered
                if (string.IsNullOrWhiteSpace(mdl.Slug))
                {
                    slug = mdl.Title.Replace(" ", "_").ToLower();
                }
                else
                {
                    slug = mdl.Slug.Replace(" ", "_").ToLower();
                }
                //Check if title or slug are unique 
                if (db.Pages.Any(x => x.Title == mdl.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "The title or slug are not unique");
                    return View(mdl);
                }
                //Add and save the database
                pdh.Slug = mdl.Slug;
                pdh.Body = mdl.Body;
                pdh.Sorting = 100;
                pdh.HasSidebar = mdl.HasSidebar;
                db.Pages.Add(pdh);
                db.SaveChanges();
            }
            //Display success message
            TempData["SM"] = "New Page has been added";
            //Return to get method
            return RedirectToAction("AddNewPage");
        }
        // Used to get the page info for editiing using the unique primary key id
        // GET: Admin/Paging/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageViewModel pVM;
            using (DataBase db = new DataBase())
            {
                PageDataHandler pdh = db.Pages.Find(id);
                //Check if page is present
                if (pdh == null)
                {
                    return Content("This page is not present in database");
                }
                pVM = new PageViewModel(pdh);
            }
            return View(pVM);
        }
        // POST: Admin/Paging/EditPage/
        [HttpPost]
        public ActionResult EditPage(PageViewModel mdl)
        {
            if (!ModelState.IsValid)
            {
                return View(mdl);
            }

            using (DataBase db = new DataBase())
            {
                int id = mdl.Id;

                string slug = "home";

                PageDataHandler pdh = db.Pages.Find(id); 

                pdh.Title = mdl.Title;

                if (mdl.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(mdl.Slug))
                    {
                        slug = mdl.Title.Replace(" ", "_").ToLower();
                    }
                    else
                    {
                        slug = mdl.Slug.Replace(" ", "_").ToLower();
                    }
                }

                if (db.Pages.Where(x => x.Id != id).Any(x=>x.Title == mdl.Title)||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "The title or slug are not unique");
                    return View(mdl);
                }
                pdh.Slug = mdl.Slug;
                pdh.Body = mdl.Body;
                pdh.HasSidebar = mdl.HasSidebar;
                db.SaveChanges();


            }
            //Succsess message to promt that page was edited
            TempData["SM"] = "Page has been edited";
            return RedirectToAction("EditPage");
        }
        // Used to show details of page and preview it
        // GET: Admin/Paging/DetailsPg/id
        public ActionResult DetailsPg(int id)
        {
            PageViewModel pVM;
            using (DataBase db = new DataBase())
            {
                PageDataHandler pdh = db.Pages.Find(id);

                if(pdh == null)
                {
                    return Content("This page is not present in database");
                }

                pVM = new PageViewModel(pdh);
            }
                return View(pVM); 
        }
        // Used to delete the page
        // GET: Admin/Paging/DelPage/id
        public ActionResult DelPage(int id)
        {
            using (DataBase db = new DataBase())
            {
                PageDataHandler pdh = db.Pages.Find(id);
                db.Pages.Remove(pdh);
                db.SaveChanges();
            }
            return RedirectToAction("Index"); 
        }
        // Reordeing the sorting of pages using jquery
        // GET: Admin/Paging/Reorder
        [HttpPost]
        public void Reorder(int[] id)
        {
            using (DataBase db = new DataBase())
            {
                int i = 1;

                PageDataHandler pdh;

                foreach (var pgId in id)
                {
                    pdh = db.Pages.Find(pgId);
                    pdh.Sorting = i;
                    db.SaveChanges();
                    i++;
                }
            }

        }
        //Used to edit information on the sidebar
        // GET: Admin/Paging/SideBarEditor
        [HttpGet]
        public ActionResult SideBarEditor()
        {
            SidebarViewModel sbvm;
            using (DataBase db = new DataBase())
            {
                SideBarDataHandler sbdh = db.Sidebar.Find(1);

                sbvm = new SidebarViewModel(sbdh); 
            }
                return View(sbvm); 
        }
        // Add edited sidebar to database
        // Post: Admin/Paging/SideBarEditor
        [HttpPost]
        public ActionResult SideBarEditor( SidebarViewModel sbvm)
        {
            using (DataBase db = new DataBase())
            {
                SideBarDataHandler sbdh = db.Sidebar.Find(1);
                sbdh.Body = sbvm.Body;

                db.SaveChanges(); 
            }

            TempData["SM"] = "Side Bar edited succesfully";

            return RedirectToAction("SideBarEditor"); 
        }

    }
}