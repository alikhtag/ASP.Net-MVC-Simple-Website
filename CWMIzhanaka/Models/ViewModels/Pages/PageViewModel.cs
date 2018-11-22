using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Models.ViewModels.Pages
{
    public class PageViewModel
    {
        public PageViewModel()
        {

        }

        public PageViewModel(PageDataHandler row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar; 
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}