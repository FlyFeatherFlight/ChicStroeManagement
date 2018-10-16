using System.Web;
using System.Web.Optimization;

namespace ChicStoreManagement
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.0.0.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/moment-with-locales.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jscookie").Include(
"~/Scripts/jquery.min.js",
"~/Scripts/jquery.cookie.js"));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      //"~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/bootstrap-datetimepicker.zh-CN.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/PagedList.css", 
                      //"~/Content/jquery.datetimepicker.min.css",
                      //"~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/bootstrap-datetimepicker.css"
                      ));
           
            //jquery-ui
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-1.12.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
               "~/Scripts/jquery-ui-1.12.1.min.js"));

            try
            {
            //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //    "~/Scripts/jquery.datetimepicker.full.min.js"));
              
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //    "~/Scripts/jquery.datetimepicker.min.js"));
               // bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
               //"~/Scripts/calendar.js"));
            }
            catch (System.Exception e)
            {

                throw e;
            }
          

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
               "~/Content/themes/base/jquery-ui.css"));
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
               "~/Content/themes/base/jquery-ui.min.css"));
            //bundles.Add(new ScriptBundle("~/Content/themes/i18n").Include(
            //    "~~/Scripts/jquery.ui.datepicker-zh-CN.js"));
        }
    }
}
