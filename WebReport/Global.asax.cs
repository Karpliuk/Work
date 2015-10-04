using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.IO;

namespace WebReport
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Route", "{lang}/{page}.aspx", "~/{page}.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            //if (Session["SilabMainResultFile"] != null)
            //{
            //    if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMainResultFile"].ToString())))
            //    {
            //        File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMainResultFile"].ToString()));
            //    }
            //}
            //if (Session["SilabMicroResultFile"] != null)
            //{
            //    if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMicroResultFile"].ToString())))
            //    {
            //        File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMicroResultFile"].ToString()));
            //    }
            //}
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}