using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraclesForMito.Utilities;

namespace MiraclesForMito.Controllers.ActionFilters
{
	/// <summary>
	/// Filter that checks to see if the person viewing a specific section is allowed to see this section
	/// and if not where they are suppose to go instead.
	/// </summary>
	public class AdminsOnly : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			// noone is logged in -- force them to login
			if (UserUtils.CurrentUser == null)
			{
				HttpContext.Current.Response.Redirect("~/Admin/", true);
			}
			// someone is logged in but they need to change their password -- force them to change their password
			else if (UserUtils.CurrentUser.ForceChangePassword.GetValueOrDefault(false))
			{
				HttpContext.Current.Response.Redirect("~/Admin/ChangePassword", true);
			}
			// ELSE - someone is logged in and they are good to go
		}
	}
}