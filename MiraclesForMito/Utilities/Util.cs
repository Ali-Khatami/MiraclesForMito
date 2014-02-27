using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiraclesForMito.Models;

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

	public class UserUtils
	{
		public static string USER_ID_COOKIE_NAME = "MiraclesForMitoAuth";

		/// <summary>
		/// Grabs the current user in context using the encrypted cookie.
		/// </summary>
		public static AdminUser CurrentUser
		{
			get
			{
				SiteDB db = new SiteDB();

				HttpCookie userCookie = HttpContext.Current.Request.Cookies.Get(USER_ID_COOKIE_NAME);
				AdminUser user = null;

				if (userCookie != null && !string.IsNullOrEmpty(userCookie.Value))
				{
					string sUserId = SimpleCrypto.Decrypt(userCookie.Value);

					int iUserID = -1;

					if (int.TryParse(sUserId, out iUserID))
					{
						user = db.Admins.Where(u => u.ID == iUserID).FirstOrDefault();
					}
				}

				return user;
			}
		}

		/// <summary>
		/// Creates an encrypted user cookie that we can read on subsequent requests.
		/// </summary>
		/// <param name="sUserID">The userID in plaintext.</param>
		public static void CreateEncryptedUserCookie(int userID)
		{
			CreateEncryptedUserCookie(userID, false);
		}

		/// <summary>
		/// Creates an encrypted user cookie that we can read on subsequent requests.
		/// </summary>
		/// <param name="sUserID">The userID in plaintext.</param>
		/// <param name="remember">
		/// Whether or not to remember the user,ie keep the cookie around for at least a little while.
		/// If null cookie will default to session.
		/// </param>
		public static void CreateEncryptedUserCookie(int userID, bool? remember)
		{
			var cookie = new HttpCookie(
				USER_ID_COOKIE_NAME,
				SimpleCrypto.Encrypt(userID.ToString())
			);

			// only visible serverside
			cookie.HttpOnly = true;

			// if expirationdays has a value we will add that amount of days to today
			if (remember.GetValueOrDefault(false))
			{
				cookie.Expires = DateTime.Today.AddDays(30);
			}
			// else it will be a session cookie

			// set the cookie
			HttpContext.Current.Response.Cookies.Set(cookie);
		}

		/// <summary>
		/// Removes the current users encrypted user cookie.
		/// </summary>
		public static void DestroyEncryptedUserCookie()
		{
			HttpCookie userCookie = HttpContext.Current.Request.Cookies.Get(USER_ID_COOKIE_NAME);
			if (userCookie != null)
			{
				userCookie.Expires = DateTime.Today.AddDays(-1);
				HttpContext.Current.Response.Cookies.Set(userCookie);
			}
		}
	}
}