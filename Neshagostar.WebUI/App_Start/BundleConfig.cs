using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Neshagostar.WebUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/MarketShop/js/bootstrap/css/bootstrap.min.css",
                "~/Content/MarketShop/js/bootstrap/css/bootstrap-rtl.min.css",
                "~/Content/MarketShop/css/font-awesome/css/font-awesome.min.css",
                "~/Content/MarketShop/css/stylesheet.css",
                "~/Content/jquery-ui.css"
                //"~/Content/MarketShop/css/owl.carousel.css",
                //"~/Content/MarketShop/css/owl.transitions.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css2").Include(
                "~/Content/MarketShop/css/responsive.css",
                "~/Content/MarketShop/css/stylesheet-rtl.css",
                "~/Content/MarketShop/css/responsive-rtl.css",
                "~/Content/MarketShop/css/stylesheet-skin2.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Content/MarketShop/js/bootstrap/js/bootstrap.min.js",
                //"~/Content/MarketShop/js/jquery.easing-1.3.min.js",
                //"~/Content/MarketShop/js/jquery.dcjqaccordion.min.js",
                "~/Content/MarketShop/js/owl.carousel.min.js",
                "~/Scripts/jquery-ui.js"
                ));

            bundles.Add(new StyleBundle("~/Content/Admincss").Include(
                "~/Content/site.css",
                "~/Content/bootstrap.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/adminscripts").Include(
                "~/Scripts/jquery-3.0.0.min.js",
                "~/scripts/jquery.validate.js",
                "~/Scripts/modernizr-2.6.3.js"
                //"~/Content/MarketShop/js/owl.carousel.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/accountscripts").Include(
                "~/Scripts/jquery-3.0.0.min.js",
                "~/scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/modernizr-2.6.3.js",
                "~/Scripts/bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                 "~/scripts/jquery.validate.js",
                "~/scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"
               ));
        }
    }
}