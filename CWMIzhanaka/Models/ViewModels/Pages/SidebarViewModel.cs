using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Models.ViewModels.Pages
{
    public class SidebarViewModel
    {
        public SidebarViewModel() {}

        public SidebarViewModel(SideBarDataHandler sbdh)
        {
            Id = sbdh.Id;
            Body = sbdh.Body; 
        }

        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}