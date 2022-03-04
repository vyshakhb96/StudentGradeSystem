using System.Web;
using System.Web.Optimization;

 namespace StudentCrudOperation.Web
 {
     public class BundleConfig
     {
        public static void RegisterBundles(BundleCollection bundle)
        {
            StyleBundle myCssBundle = new StyleBundle("~/Content/MyCSS");
            myCssBundle.Include("~/Content/bootstrap.min.css", "~/Content/Site.css", "~/Style/StyleSheet.css", "~/Content/DataTables/css/jquery.dataTables.css", "~/Content/toastr.min.css");


            ScriptBundle myScriptBundle = new ScriptBundle("~/Scripts/MyScript");
            myScriptBundle.Include("~/Scripts/jquery-1.9.1.js", "~/Scripts/jquery-1.9.1.min.js", "~/Scripts/bootstrap.min.js", "~/Scripts/DataTables/jquery.dataTables.js", "~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.js", "~/Scripts/toastr.min.js", "~/Scripts/jszip.min.js", "~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.html5.min.js","~/Scripts/DataTables/buttons.print.min.js");


            bundle.Add(myCssBundle);
            bundle.Add(myScriptBundle);

            BundleTable.EnableOptimizations = true;

        }
     }
  }
