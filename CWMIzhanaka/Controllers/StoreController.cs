using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Controllers
{
    public class StoreController : Controller
    {
        // Going /store/index redirects to homepage
        // GET: Store
        public ActionResult Index()
        {
            return View("Index", "Paging");
        }
        //Used to display menu of categories
        // GET: Stroe/Catmenu
        public ActionResult CatMenu()
        {
            List<CategoriesViewModel> listCVM;

            using (DataBase db = new DataBase())
            {
                listCVM = db.Category.ToArray().OrderBy(a => a.Sorting).Select(a => new CategoriesViewModel(a)).ToList();
            }

            return PartialView(listCVM);
        }
        //Used to return a list of product categories
        // GET: /Store/ProdCateg/name
        public ActionResult ProdCateg(string name)
        {

            List<ProductViewModel> listPVM;

            using (DataBase db = new DataBase())
            {

                CategoryDataHandler cdh = db.Category.Where(a => a.Slug == name).FirstOrDefault();
                int id = cdh.Id;

                listPVM = db.Products.ToArray().Where(a => a.CategoryFId == id).Select(a => new ProductViewModel(a)).ToList();

                var catOfProduct = db.Products.Where(a => a.CategoryFId == id).FirstOrDefault();
                ViewBag.CatName = catOfProduct.CategoryName;
            }


            return View(listPVM);
        }
        // Used to show detail about the product
        // GET: /Store/prod-detail/name
        [ActionName("prod-detail")]
        public ActionResult ProdDetail(string name)
        {
            ProductViewModel pvm;
            ProductDataHandler pdh;

            int id;

            using (DataBase db = new DataBase())
            {

                if (!db.Products.Any(a => a.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Store");
                }
                pdh = db.Products.Where(a => a.Slug == name).FirstOrDefault();
                id = pdh.ProductId;
                pvm = new ProductViewModel(pdh);
            }



            return View("ProdDetail", pvm);
        }

    }
}