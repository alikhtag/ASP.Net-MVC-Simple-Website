using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CWMIzhanaka
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Used to make diferent privilaged roles. 
        protected void Application_AuthenticateRequest()
        {
            // Check if user is logged in
            if (User == null) { return; }

            string username = Context.User.Identity.Name;

            string[] roles = null;

            using (DataBase db = new DataBase())
            {
                // Populate roles
                AccountsDataHandler adh = db.Accounts.FirstOrDefault(a => a.Username == username);

                roles = db.UserRoles.Where(a => a.UserId == adh.UserId).Select(a => a.Role.Role).ToArray();
            }

            IIdentity identity = new GenericIdentity(username);
            IPrincipal newUser = new GenericPrincipal(identity, roles);
            Context.User = newUser;
        }
    }
}
