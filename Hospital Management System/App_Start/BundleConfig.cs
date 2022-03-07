using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Hospital_Management_System
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundle) 
        {
            StyleBundle myCssBundle = new StyleBundle("~/Content/MyCSS");
            myCssBundle.Include("~/Content/bootstrap.min.css", "~/Content/Site.css", "~/Content/bootstrap-theme.min.css", "~/Content/DataTables/css/jquery.dataTables.min.css", "~/Content/toastr.min.css");

            ScriptBundle myScriptBundle = new ScriptBundle("~/Scripts/MyScript");
            myScriptBundle.Include("~/Scripts/jquery-3.5.1.min.js", "~/Scripts/bootstrap.min.js", "~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/toastr.min.js", "~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/jszip.min.js", "~/Scripts/DataTables/buttons.html5.min.js", "~/Scripts/DataTables/buttons.print.min.js");

            bundle.Add(myCssBundle);
            bundle.Add(myScriptBundle);

            //BundleTable.EnableOptimizations = true;

        }
    }
}