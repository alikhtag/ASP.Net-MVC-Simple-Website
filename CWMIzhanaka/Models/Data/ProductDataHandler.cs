using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CWMIzhanaka.Models.Data
{
    [Table ("Products")]
    public class ProductDataHandler
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public int CategoryFId { get; set; }
        public decimal Price { get; set; }
        public string Slug { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey ("CategoryFId")]
        public virtual CategoryDataHandler GetCateg { get; set; }
    }
}