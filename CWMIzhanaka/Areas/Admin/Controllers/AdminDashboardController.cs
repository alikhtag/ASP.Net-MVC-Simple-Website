using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWMIzhanaka.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        // GET: Admin/AdminDashboard
        // Access the admin dashboard page
        public ActionResult Index()
        {
            return View();
        }
    }
}