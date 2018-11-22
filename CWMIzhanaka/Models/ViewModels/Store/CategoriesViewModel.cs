using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWMIzhanaka.Models.ViewModels.Store
{
    public class CategoriesViewModel
    {
        public CategoriesViewModel() { }
        public CategoriesViewModel(CategoryDataHandler cdh)
        {
            Id = cdh.Id;
            Name = cdh.Name;
            Slug = cdh.Slug;
            Sorting = cdh.Sorting; 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}