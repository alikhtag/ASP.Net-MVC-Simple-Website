using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Models.ViewModels.Store
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
        }
        public ProductViewModel(ProductDataHandler pdh)
        {
            ProductId = pdh.ProductId;
            Name = pdh.Name;
            Description = pdh.Description;
            CategoryName = pdh.CategoryName;
            CategoryFId = pdh.CategoryFId;
            Price = pdh.Price;
            Slug = pdh.Slug;
            ImagePath = pdh.ImagePath;
        }
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int CategoryFId { get; set; }
        public decimal Price { get; set; }
        public string Slug { get; set; }
        public string ImagePath { get; set; }

        public IEnumerable<SelectListItem> GetCateg { get; set; }
        public IEnumerable<String> Images { get; set; }
    }
}