using System.Web;
using System.Web.Optimization;

namespace GIR.Sigim.Presentation.WebUI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/libs/jquery-2.0.2.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        //"~/Scripts/libs/jquery-2.0.2.min.js",
                        "~/Scripts/libs/jquery-ui-1.10.3.min.js",

                        //jquerypriceformat.com/
                        "~/Scripts/jquery.price_format.2.0.js",


                        //"~/Scripts/libs/jquery.unobtrusive-ajax.min.js",
                        //"~/Scripts/plugin/jquery-form/jquery-form.min.js",
                        //JS TOUCH : include this plugin for mobile drag / drop touch events
                        "~/Scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                        //BOOTSTRAP JS
                        "~/Scripts/bootstrap/bootstrap.min.js",
                        //CUSTOM NOTIFICATION
                        "~/Scripts/notification/SmartNotification.min.js",
                        //JARVIS WIDGETS
                        "~/Scripts/smartwidgets/jarvis.widget.min.js",
                        //SPARKLINES
                        "~/Scripts/plugin/sparkline/jquery.sparkline.min.js",
                        //JQUERY VALIDATE
                        "~/Scripts/plugin/validation/jquery.validate.min.js",
                        "~/Scripts/plugin/validation/jquery.validate.unobtrusive.min.js",
                        //JQUERY MASKED INPUT
                        "~/Scripts/plugin/masked-input/jquery.maskedinput.min.js",
                        //JQUERY SELECT2 INPUT
                        "~/Scripts/plugin/select2/select2.min.js",
                        //JQUERY UI + Bootstrap Slider
                        "~/Scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                        //browser msie issue fix
                        "~/Scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                        //FastClick: For mobile devices
                        "~/Scripts/plugin/fastclick/fastclick.min.js",
                        //FormToJson
                        "~/Scripts/plugin/form-to-json/form2js.js",
                        "~/Scripts/plugin/form-to-json/jquery.toObject.js",
                        //IMPORTANT: APP CONFIG
                        "~/Scripts/app.config.js",
                        //MAIN APP JS FILE
                        "~/Scripts/app.min.js",
                        //Voice command : plugin
		                //"~/Scripts/speech/voicecommand.min.js",
                        "~/Scripts/sigim_scripts.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive-ajax").Include(
                "~/Scripts/libs/jquery.unobtrusive-ajax.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery-form").Include(
                "~/Scripts/plugin/jquery-form/jquery-form.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/smartadmin-skins.css"));

            bundles.Add(new StyleBundle("~/bundles/css/font-awesome").Include(
                "~/Content/css/font-awesome.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/fonts/font-awesome").Include(
                "~/Content/fonts/fontawesome-webfont.eot", new CssRewriteUrlTransform())
                .Include("~/Content/fonts/fontawesome-webfont.svg", new CssRewriteUrlTransform())
                .Include("~/Content/fonts/fontawesome-webfont.ttf", new CssRewriteUrlTransform())
                .Include("~/Content/fonts/fontawesome-webfont.woff", new CssRewriteUrlTransform())
                .Include("~/Content/fonts/FontAwesome.otf", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/smartadmin-production").Include(
                "~/Content/css/smartadmin-production.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/sigim_style").Include(
                "~/Content/css/sigim_style.css"));
        }
    }
}