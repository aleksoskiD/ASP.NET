using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskMenagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // for [authorize] on all controllers
            GlobalFilters.Filters.Add(new AuthorizeAttribute());

            
            //ModelBinders.Binders.DefaultBinder = new JsonModelBinder();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
