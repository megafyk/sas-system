using System.Web.Optimization;

namespace SAS.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles").Include(
                "~/scripts/angular.min.js",
                "~/scripts/angular-route.min.js",
                "~/scripts/angular-base64.js",
                "~/scripts/angular-cookies.js",
                "~/scripts/angular-material.min.js",
                "~/scripts/angular-animate.min.js",
                "~/scripts/angular-message.js",
                "~/scripts/angular-aria.min.js",
                "~/scripts/svg-assets-cache.js",
                "~/scripts/ngDialog.min.js",
                "~/scripts/jquery-3.2.1.min.js",
                "~/scripts/moment.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/scripts/app/modules/common.core.js",
                "~/scripts/app/modules/common.ui.js",
                "~/scripts/app/app.js",
                "~/scripts/app/services/apiService.js",
                "~/scripts/app/services/membershipService.js",
                "~/scripts/app/layout/customPagger.directive.js",

                "~/scripts/app/home/indexCtrl.js",
                "~/scripts/app/home/rootCtrl.js",
                "~/scripts/app/account/loginCtrl.js",
                "~/scripts/app/announcement/annCtrl.js",
                "~/scripts/app/directives/adminPop.directive.js"
            ));
            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/css/site.css",
                "~/Content/css/docs.css",
                "~/Content/css/bootstrap.min.css",
                "~/Content/angular-material.min.css",
                "~/Content/font-awesome.min.css"
            ));
            BundleTable.EnableOptimizations = false;
        }
    }
}