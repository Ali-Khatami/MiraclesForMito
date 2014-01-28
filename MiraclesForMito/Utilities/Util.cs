using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Utilities
{
	public class Util
	{
		public static string ResolveUrlInlineScript
		{
			get
			{
				return "String.prototype.ResolveUrl = function(){ var sbaseUrl = '" + (VirtualPathUtility.ToAbsolute("~/")) + "'; return this.replace('~/', sbaseUrl); }";
			}
		}
	}
}