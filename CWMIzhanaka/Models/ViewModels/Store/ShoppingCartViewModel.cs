using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWMIzhanaka.Models.ViewModels.Store
{
    public class ShoppingCartViewModel
    {
        public int ProductId { get; set; }
        public string ProdName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get { return Price * Quantity; } }
        public string Image { get; set; }

    }
}