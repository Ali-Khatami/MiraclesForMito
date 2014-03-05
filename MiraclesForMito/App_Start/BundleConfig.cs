﻿using System.Web;
using System.Web.Optimization;

namespace MiraclesForMito
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(
				new ScriptBundle("~/bundles/js/vendor").Include(
					"~/Scripts/jquery-{version}.js",
					"~/Scripts/jquery-ui-{version}.js",
					"~/Scripts/jquery.color-2.1.2.min.js",
					"~/Scripts/bootstrap.js",
					"~/Scripts/pagination.js",
					"~/Scripts/navigation.js",
					"~/Scripts/OADate.js"
				)
			);

			bundles.Add(
				new ScriptBundle("~/bundles/js/admin").Include(
					"~/Scripts/tinymce/tinymce.min.js",
					"~/Scripts/admin.js"
				)
			);

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(
				new ScriptBundle("~/bundles/modernizr").Include(
					"~/Scripts/modernizr-*"
				)
			);

			bundles.Add(
				new StyleBundle("~/Content/css").Include(
					"~/Content/common.css",
					"~/Content/layout.css",
					"~/Content/main-page.css"
				)
			);

			bundles.Add(
				new StyleBundle("~/Content/css/Admin").Include(
					"~/Content/admin.css"
				)
			);

			bundles.Add(
				new StyleBundle("~/Content/vendor").Include(
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
					"~/Content/themes/base/jquery.ui.theme.css",
					"~/Content/bootstrap.css",
					"~/Content/bootstrap-theme.css"
				)
			);
		}
	}
}
