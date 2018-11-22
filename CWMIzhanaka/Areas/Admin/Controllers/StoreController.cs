using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.ViewModels.Store;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CWMIzhanaka.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoreController : Controller
    {
        // Used to display the categories
        // GET: Admin/Store
        public ActionResult Categories()
        {
            List<CategoriesViewModel> catVMList;
            using (DataBase db = new DataBase())
            {
                catVMList = db.Category.ToArray().OrderBy(a => a.Sorting).Select(a => new CategoriesViewModel(a)).ToList();
            }
            return View(catVMList);
        }
        // Used to add the category to the database
        // POST: Admin/Store/AddCat
        [HttpPost]
        public string AddCat(string nameCat)
        {
            string id;

            using (DataBase db = new DataBase())
            {
                if (db.Category.Any(a => a.Name == nameCat))
                {
                    return "taken";
                }

                CategoryDataHandler cdh = new CategoryDataHandler();

                cdh.Name = nameCat;
                cdh.Slug = nameCat.Replace(" ", "_").ToLower();
                cdh.Sorting = 50;
                db.Category.Add(cdh);
                db.SaveChanges();

                id = cdh.Id.ToString();
            }
            return id;
        }
        // Used to reorder sorting using JS
        // POST:: Admin/Store/CatReorder
        [HttpPost]
        public void CatReorder(int[] id)
        {
            using (DataBase db = new DataBase())
            {
                int i = 1;

                CategoryDataHandler cdh;

                foreach (var catID in id)
                {
                    cdh = db.Category.Find(catID);
                    cdh.Sorting = i;
                    db.SaveChanges();
                    i++;
                }
            }

        }
        // Used to delete selected category by pasing key id of database
        // GET: Admin/Store/DelCat/id
        public ActionResult DelCat(int id)
        {
            using (DataBase db = new DataBase())
            {
                CategoryDataHandler cdh = db.Category.Find(id);
                db.Category.Remove(cdh);
                db.SaveChanges();
            }
            return RedirectToAction("Categories");
        }
        // Used to add updated category to the database using id and string input
        // POST: Admin/Store/CatUpdate
        [HttpPost]
        public string CatUpdate(string nameCatNew, int id)
        {

            using (DataBase db = new DataBase())
            {
                if (db.Category.Any(a => a.Name == nameCatNew))
                {
                    return "taken";
                }
                CategoryDataHandler cdh = db.Category.Find(id);
                cdh.Name = nameCatNew;
                cdh.Slug = nameCatNew.Replace(" ", "_").ToLower();
                db.SaveChanges();


            }
            return "done";
        }
        //Displays new Product view
        // GET: Admin/Store/AddNewProduct
        [HttpGet]
        public ActionResult AddNewProduct()
        {
            ProductViewModel pvm = new ProductViewModel();
            using (DataBase db = new DataBase())
            {
                pvm.GetCateg = new SelectList(db.Category.ToList(), "Id", "Name");
            }
            return View(pvm);
        }
        // Used to get posted information about the product
        // POST: Admin/Store/AddNewProduct
        [HttpPost]
        public ActionResult AddNewProduct(ProductViewModel pvm, HttpPostedFileBase fileIn)
        {
            if (!ModelState.IsValid)
            {
                using (DataBase db = new DataBase())
                {
                    pvm.GetCateg = new SelectList(db.Category.ToList(), "Id", "Name");
                    return View(pvm);
                }
            }

            using (DataBase db = new DataBase())
            {
                if (db.Products.Any(a => a.Name == pvm.Name))
                {

                    pvm.GetCateg = new SelectList(db.Category.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "Product with that already name exists");
                    return View(pvm);
                }
            }
            int id;

            using (DataBase db = new DataBase())
            {
                ProductDataHandler pdh = new ProductDataHandler();
                pdh.Name = pvm.Name;
                pdh.Description = pvm.Description;
                pdh.Price = pvm.Price;
                pdh.CategoryFId = pvm.CategoryFId;
                pdh.Slug = pvm.Name.Replace(" ", "_").ToLower();

                CategoryDataHandler cdh = db.Category.FirstOrDefault(a => a.Id == pvm.CategoryFId);
                pdh.CategoryName = cdh.Name;


                db.Products.Add(pdh);
                db.SaveChanges();
                id = pdh.ProductId;
                Debug.WriteLine(id);

            }
            TempData["MSG"] = "Product Has Been Added";
            // Make uri paths to store images by id
            var origPath = new DirectoryInfo(string.Format("{0}Images\\Uploaded", Server.MapPath(@"\")));
            var path1 = Path.Combine(origPath.ToString(), "ProductImages\\" + id.ToString());
            var path2 = Path.Combine(origPath.ToString(), "ProductImages\\" + id.ToString() + "\\Thumbnails");

            // Check if the folders exist if not make them
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }

            if (fileIn != null && fileIn.ContentLength > 0)
            {
                string imgExtension = fileIn.ContentType.ToLower();
                // check for correct extensions when image is inputted
                if (imgExtension != "image/jpg" &&
                    imgExtension != "image/jpeg" &&
                    imgExtension != "image/pjpeg" &&
                    imgExtension != "image/png" &&
                    imgExtension != "image/x-png" &&
                    imgExtension != "image/gif")
                {
                    using (DataBase db = new DataBase())
                    {

                        pvm.GetCateg = new SelectList(db.Category.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Error Image was not Proccessed, Try Again");
                        return View(pvm);

                    }
                }
                string nameOfImg = fileIn.FileName;
                Debug.WriteLine(nameOfImg.ToString());

                using (DataBase db = new DataBase())
                {
                    // save image path to database inorder to get it later
                    ProductDataHandler pdh = db.Products.Find(id);
                    pdh.ImagePath = nameOfImg;
                    Debug.WriteLine(pdh.ImagePath.ToString());
                    db.SaveChanges();
                }
                var imgPath = string.Format("{0}\\{1}", path1, nameOfImg);
                var imgPath1 = string.Format("{0}\\{1}", path2, nameOfImg);
                // Save image in the desired path
                fileIn.SaveAs(imgPath);

                WebImage image = new WebImage(fileIn.InputStream);
                image.Resize(300, 300);
                image.Save(imgPath1);
            }

            return RedirectToAction("AddNewProduct");

        }
        //Display all products in list using PagedList reference to make things in list to show in their pages
        // GET: Admin/Store/ProductList
        public ActionResult ProductList(int? pg, int? categoryId)
        {
            List<ProductViewModel> pvmList;

            var pgNum = pg ?? 1;

            using (DataBase db = new DataBase())
            {
                pvmList = db.Products.ToArray()
                    .Where(a => categoryId == null || categoryId == 0 || a.CategoryFId == categoryId)
                    .Select(a => new ProductViewModel(a)).ToList();

                ViewBag.CategoryList = new SelectList(db.Category.ToList(), "Id", "name");

                ViewBag.PickedCategory = categoryId.ToString();



            }
            var onePgofProd = pvmList.ToPagedList(pgNum, 3);
            ViewBag.OnePgOfProd = onePgofProd;

            return View(pvmList);
        }
        // Edit the product view
        // GET: Admin/Store/ProductEdit
        [HttpGet]
        public ActionResult ProductEdit(int id)
        {
            ProductViewModel pvm;
            using (DataBase db = new DataBase())
            {
                ProductDataHandler pdh = db.Products.Find(id);
                if (pdh == null)
                {
                    return Content("No such product found");

                }
                pvm = new ProductViewModel(pdh)
                {
                    GetCateg = new SelectList(db.Category.ToList(), "Id", "Name"),
                    Images = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploaded/ProductImages/" + id + "/Gallery/Thumbnails"))
                    .Select(a => Path.GetFileName(a))
                };


            }
            return View(pvm);
        }
        //Recieve the information about product and save it in database
        // POST: Admin/Store/ProductEdit
        [HttpPost]
        public ActionResult ProductEdit(ProductViewModel pvm, HttpPostedFileBase fileIn)
        {
            int id = pvm.ProductId;
            Debug.Write("id:" + id); 
            using (DataBase db = new DataBase())
            {
                pvm.GetCateg = new SelectList(db.Category.ToList(), "Id", "Name");
            }
            pvm.Images = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploaded/ProductImages/" + id + "/Thumbnails"))
                    .Select(a => Path.GetFileName(a));
            if (!ModelState.IsValid)
            {
                return View(pvm);
            }

            using (DataBase db = new DataBase())
            {
                if (db.Products.Where(a => a.ProductId != id).Any(a => a.Name == pvm.Name))
                {
                    ModelState.AddModelError("", "Product Name Exists in Database");
                    return View(pvm);
                }
            }
            using (DataBase db = new DataBase())
            {
                ProductDataHandler pdh = db.Products.Find(id);
                pdh.Name = pvm.Name;
                pdh.Description = pvm.Description;
                pdh.Price = pvm.Price;
                pdh.CategoryFId = pvm.CategoryFId;
                pdh.ImagePath = pvm.ImagePath;
                CategoryDataHandler cdh = db.Category.FirstOrDefault(a => a.Id == pvm.CategoryFId);
                pdh.CategoryName = cdh.Name;

                db.SaveChanges(); 
            }
            TempData["MSG"] = "Product Edited";

            if (fileIn != null && fileIn.ContentLength > 0)
            {
                string imgExtension = fileIn.ContentType.ToLower();

                if (imgExtension != "image/jpg" &&
                    imgExtension != "image/jpeg" &&
                    imgExtension != "image/pjpeg" &&
                    imgExtension != "image/png" &&
                    imgExtension != "image/x-png" &&
                    imgExtension != "image/gif")
                {
                    using (DataBase db = new DataBase())
                    {

                        pvm.GetCateg = new SelectList(db.Category.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Error Image was not Proccessed, Try Again");
                        return View(pvm);

                    }
                }
                var origPath = new DirectoryInfo(string.Format("{0}Images\\Uploaded", Server.MapPath(@"\")));
                var path1 = Path.Combine(origPath.ToString(), "ProductImages\\" + id.ToString());
                var path2 = Path.Combine(origPath.ToString(), "ProductImages\\" + id.ToString() + "\\Thumbnails");

                DirectoryInfo dirInfo1 = new DirectoryInfo(path1);
                DirectoryInfo dirInfo2 = new DirectoryInfo(path2);
                //delete files
                foreach(FileInfo fileInfo in dirInfo1.GetFiles())
                {
                    fileInfo.Delete(); 
                }

                foreach (FileInfo fileInfo in dirInfo2.GetFiles())
                {
                    fileInfo.Delete();
                }


                string nameOfImg = fileIn.FileName;
                Debug.WriteLine(nameOfImg.ToString());

                using (DataBase db = new DataBase())
                {
                    ProductDataHandler pdh = db.Products.Find(id);
                    pdh.ImagePath = nameOfImg;
                    db.SaveChanges();
                }
                var imgPath = string.Format("{0}\\{1}", path1, nameOfImg);
                var imgPath1 = string.Format("{0}\\{1}", path2, nameOfImg);

                fileIn.SaveAs(imgPath);

                WebImage image = new WebImage(fileIn.InputStream);
                image.Resize(300, 300);
                image.Save(imgPath1);
            }

            return RedirectToAction("ProductEdit"); 


        }
        //Used to delete the product
        public ActionResult ProductDel(int id)
        {

            using (DataBase db = new DataBase())
            {
                ProductDataHandler pdh = db.Products.Find(id);
                db.Products.Remove(pdh);

                db.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }
    }

}