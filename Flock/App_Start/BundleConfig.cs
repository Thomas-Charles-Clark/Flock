using System.Web;
using System.Web.Optimization;

namespace Flock
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Clear();
            bundles.ResetAll();

            bundles.Add(new StyleBundle("~/bundles/css/bundles").Include(
                        "~/Content/bundles.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/codemirror-htmleditor").Include(
                        "~/Scripts/codemirror.js",
                        "~/Scripts/codemirror-methods.js",
                        "~/Scripts/codemirror-options.js",
                        "~/Scripts/codemirror-dialog.js",
                        "~/Scripts/codemirror-search.js",
                        "~/Scripts/codemirror-searchcursor.js",
                        "~/Scripts/codemirror-jump-to-line.js",
                        "~/Scripts/codemirror-xml.js",
                        "~/Scripts/codemirror-javascript.js",
                        "~/Scripts/codemirror-css.js",
                        "~/Scripts/codemirror-htmlmixed.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/prettify").Include(
                        "~/Scripts/Prettify/prettify.js",
                        "~/Scripts/vkbeautify.0.99.00.beta.js"));

            bundles.Add(new StyleBundle("~/bundles/css/prettify").Include(
                        "~/Content/Prettify/prettify.css",
                        "~/Content/Prettify/Themes/sunburst.css"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/common-functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/jquery.multisortable.js",
                        "~/Scripts/jquery.multidraggable.js",
                        "~/Scripts/jquery-field-chooser.js",
                        "~/Scripts/jquery-ui-extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/knockout").Include(
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/knockout-pre-rendered.js",
                        "~/Scripts/linq.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/nested-sortable").Include(
                        "~/Scripts/jquery.mjs.nestedSortable.js",
                        "~/Scripts/jquery-draggable-nestedSortable.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/composer").Include(
                        "~/Content/composition-base.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-overrides.css",
                      "~/Content/site.css"
                      //"~/Content/bootstrap-responsive.css",
                      //"~/Content/bootstrap-overrides.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/css/samples").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap.css",
                      "~/Content/samples-ui.css",
                      "~/Content/samples.css"
                    ));

            bundles.Add(new StyleBundle("~/bundles/css/searchimpact").Include(
                    "~/Content/search-impact.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/searchimpact").Include(
                    "~/Scripts/search-impact.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/samples").Include(
                        "~/Scripts/samples.js"
                ));

            bundles.Add(new StyleBundle("~/Content/preview/css").Include(
                      "~/Content/preview/bootstrap.css",
                      "~/Content/preview/bootstrap-overrides.css",
                      "~/Content/preview/site.css",
                      "~/Content/preview/devexpress-overrides.css",
                      "~/Content/preview/grids-min.css",
                      "~/Content/preview/grids-overrides.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/css/ribbon").Include(
                "~/Content/themes/admin/ribbon/jquery-ribbon-0.1.4.css",
                "~/Content/themes/admin/ribbon/ribbon-overrides.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/ribbon").Include(
                "~/Scripts/jquery-ribbon-0.1.4.js"));

            bundles.Add(new StyleBundle("~/bundles/css/grids").Include(
                "~/Content/themes/grids/grids-min.css",
                "~/Content/themes/grids/grids-overrides.css"));

            bundles.Add(new StyleBundle("~/bundles/css/admin/logon").Include(
                "~/Content/themes/admin/logon.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/calendar").Include(
                "~/Scripts/Calendar/dhtmlxscheduler.js",
                "~/Scripts/Calendar/dhtmlxscheduler-year-view.js",
                "~/Scripts/Calendar/dhtmlxscheduler-serialize.js",
                "~/Scripts/Calendar/dhtmlxscheduler-mvc.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css/calendar").Include(
                "~/Content/calendar/dhtmlxscheduler.css",
                "~/Content/calendar/calendar.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/css/cms").Include(
                "~/Content/themes/admin/cms/cms.css",
                "~/Content/themes/admin/cms/page-composer.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/page-composition").Include(
                "~/Scripts/cms/page-compositon.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/public_layout").Include(
                "~/Content/public_css/public_layout.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js/sparkline").Include(
                "~/Scripts/jquery.sparkline.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-2.0.3.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/cms-compose-page").Include(
                "~/Scripts/cms/cms-compose-page.js"
                ));

            #if !DEBUG
            BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}