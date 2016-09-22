using System.Web;
using System.Web.Optimization;

namespace dwm_condominios
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content-z/css").Include(
                      "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                      //"~/Content/vendors/font-awesome/css/font-awesome.min.css",
                      "~/Content/vendors/iCheck/skins/flat/green.css",
                      "~/Content/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                      "~/Content/production/css/maps/jquery-jvectormap-2.0.3.css",
                      "~/Content/vendors/select2/dist/css/select2.min.css",
                      "~/Content/production/css/custom.css",
                      "~/Scripts/css/datepicker.css",
                      "~/Content/Site.css",
                      "~/Content/vendors/summernote/css/summernote.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/vendors/jquery/dist/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
                        "~/Content/vendors/fastclick/lib/fastclick.js"));

            bundles.Add(new ScriptBundle("~/bundles/nprogress").Include(
                        "~/Content/vendors/nprogress/nprogress.js"));

            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
                        "~/Content/vendors/Chart.js/dist/Chart.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/gauge").Include(
                        "~/Content/vendors/bernii/gauge.js/dist/gauge.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-progressbar").Include(
                        "~/Content/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/icheck").Include(
                        "~/Content/vendors/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/skycons").Include(
                        "~/Content/vendors/skycons/skycons.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                        "~/Content/vendors/Flot/jquery.flot.js",
                        "~/Content/vendors/Flot/jquery.flot.pie.js",
                        "~/Content/vendors/Flot/jquery.flot.time.js",
                        "~/Content/vendors/Flot/jquery.flot.stack.js",
                        "~/Content/vendors/Flot/jquery.flot.resize.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include("~/Content/vendors/select2/dist/js/select2.full.min.js",
                    "~/Content/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/flot-plugins").Include(
                    "~/Content/production/js/flot/jquery.flot.orderBars.js",
                    "~/Content/production/js/flot/date.js",
                    "~/Content/production/js/flot/jquery.flot.spline.js",
                    "~/Content/production/js/flot/curvedLines.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/jVectorMap").Include(
                        "~/Content/production/js/maps/jquery-jvectormap-2.0.3.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-daterangepicker").Include(
                        "~/Content/production/js/moment/moment.min.js",
                        "~/Content/production/js/datepicker/daterangepicker.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Content/production/js/custom.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jVectorMap2").Include(
                        "~/Content/production/js/maps/jquery-jvectormap-world-mill-en.js",
                        "~/Content/production/js/maps/jquery-jvectormap-us-aea-en.js",
                        "~/Content/production/js/maps/gdp-data.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-wysiwyg").Include(
                        "~/Content/vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js",
                        "~/Content/vendors/jquery.hotkeys/jquery.hotkeys.js",
                        "~/Content/vendors/google-code-prettify/src/prettify.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/summernote").Include(
                        "~/Content/vendors/summernote/js/summernote.min.js",
                        "~/Content/vendors/summernote/js/summernote-pt-BR.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputs-jquery-ui").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js", // serve para renderizar a página dentro do DIV
                        "~/Scripts/jquery.maskedinput.js",
                        "~/Scripts/inputs-jquery-ui.js",
                        "~/scripts/js/bootstrap-datepicker.js",
                        "~/Scripts/modernizr-2.6.2.js"
                        ));

            //bundles.Add(new StyleBundle("~/bundles/autocomplete.css").Include(
            //            "~/Content/autocomplete/autocomplete.css"
            //        ));

            //bundles.Add(new ScriptBundle("~/bundles/autocomplete.js").Include(
            //            "~/Content/autocomplete/jquery.ui.autocomplete.js"
            //        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
