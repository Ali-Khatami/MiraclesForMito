using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class AdminUser
	{
		/// <summary>
		/// Unique ID for each student
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// User's first name
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// User's last name
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// The email address for users, they will log into the site with this.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Requires user to change their password
		/// </summary>
		public bool? ForceChangePassword { get; set; }
	}
}