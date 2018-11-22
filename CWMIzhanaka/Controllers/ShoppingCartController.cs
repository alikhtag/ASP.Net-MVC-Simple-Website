using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Controllers
{
    public class ShoppingCartController : Controller
    {
        //Used to display the ShoppingCart
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cartBasket = Session["ShoppingCart"] as List<ShoppingCartViewModel> ?? new List<ShoppingCartViewModel>();

            if (Session["ShoppingCart"] == null || cartBasket.Count == 0)
            {
                ViewBag.CartMsg = "Shopping cart is empty";
                return View();
            }
            decimal cartTotalPrice = 0m;

            foreach (var product in cartBasket)
            {
                cartTotalPrice += product.TotalPrice;
            }
            ViewBag.TotalPrice = cartTotalPrice;

            return View(cartBasket);
        }
        //Display shopping cart info in nav bar
        //GET: ShoppingCart/AddToShoppingTop
        public ActionResult ShoppingCartTop()
        {
            ShoppingCartViewModel scvm = new ShoppingCartViewModel();
            decimal price = 0m;
            int quantity = 0;
            // Display quantity of items and their total price
            if (Session["ShoppingCart"] != null)
            {
                var cartList = (List<ShoppingCartViewModel>)Session["ShoppingCart"];
                foreach (var product in cartList)
                {
                    quantity += product.Quantity;
                    price += product.Price * quantity;

                }
                scvm.Quantity = quantity;
                scvm.Price = price;
            }
            else
            {
                scvm.Quantity = 0;
                scvm.Price = 0m;
            }


            return PartialView(scvm);
        }
        //Add products to shopping cart
        //GET: ShoppingCart/AddToShoppingCart
        public ActionResult AddToShoppingCart(int id)
        {
            List<ShoppingCartViewModel> shoppingCartList = Session["ShoppingCart"] as List<ShoppingCartViewModel> ?? new List<ShoppingCartViewModel>();

            ShoppingCartViewModel scvm = new ShoppingCartViewModel();

            using (DataBase db = new DataBase())
            {
                //Get the product using id
                ProductDataHandler pdh = db.Products.Find(id);

                //Make a list of items in cart using shopping cart view model
                var addToCart = shoppingCartList.FirstOrDefault(a => a.ProductId == id);

                if (addToCart == null)
                {
                    shoppingCartList.Add(new ShoppingCartViewModel()
                    {
                        ProdName = pdh.Name,
                        ProductId = pdh.ProductId,
                        Quantity = 1,
                        Price = pdh.Price,
                        Image = pdh.ImagePath
                    });
                }
                else
                {
                    addToCart.Quantity++;
                }
                int quantity = 0;
                decimal price = 0m;
                //Add how many products in list and total price
                foreach (var product in shoppingCartList)
                {
                    quantity += product.Quantity;
                    price += product.Quantity * product.Price;
                }
                //Display the price and quantity
                scvm.Price = price;
                scvm.Quantity = quantity;

                Session["ShoppingCart"] = shoppingCartList;

                return PartialView(scvm);


            }
        }
        //Delete product from shopping cart
        //GET: ShoppingCart/DelProd
        public void DelProd(int productId)
        {
            List<ShoppingCartViewModel> listSCVM = Session["ShoppingCart"] as List<ShoppingCartViewModel>;
            using (DataBase db = new DataBase())
            {
                ShoppingCartViewModel scvm = listSCVM.FirstOrDefault(a => a.ProductId == productId);
                listSCVM.Remove(scvm); 
            }
        }
    }
}