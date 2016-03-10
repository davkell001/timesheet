using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace MBTimeSheetWebApp
{
    public class Global : HttpApplication
    {
        public static string Username { get; set; }
        public static string CompanyName { get; set; }
        public static int CompanyId { get; set; }
        public static string ReportPath { get; set; }
        public static string ConnectionString { get; set; }
        public static List<string> ConnectionVariables { get; set; }

        public static void SplitConnectionString()
        {
            List<string> list = new List<string>();
            string connect = ConnectionString.Trim();

            string[] splitStr = connect.Split(',');

            foreach (string variable in splitStr)
            {
                list.Add(variable.Trim());
            }
            ConnectionVariables = list;
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}