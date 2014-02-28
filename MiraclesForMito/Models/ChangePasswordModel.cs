using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Models
{
	public class ChangePasswordModel
	{
		/// <summary>
		/// The old password, generally set from model binding.
		/// </summary>
		public string OldPassword { get; set; }
		
		/// <summary>
		/// The new password, generally set from model binding.
		/// </summary>
		public string NewPassword { get; set; }

		/// <summary>
		/// The confirmation of the new password to ensure they match, generally set from model binding.
		/// </summary>
		public string NewPasswordConfirm { get; set; }

		/// <summary>
		/// Message if there is any error.
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Determines whether or not someone actually attempted to change the password or if it's the inital
		/// page load.
		/// </summary>
		public bool AttemptedChange { get; set; }

		/// <summary>
		/// The user to change the password for
		/// </summary>
		public AdminUser CurrentUser;

		protected SiteDB DBInstance;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userToModify"></param>
		public void SetUser(AdminUser user, SiteDB dbInstance)
		{
			this.DBInstance = dbInstance;

			if (this.DBInstance == null)
			{
				throw new Exception("dbInstance must not be null");
			}

			this.CurrentUser = this.DBInstance.Admins.Find(user.ID);
		}

		public bool ChangePassword()
		{
			bool bPasswordChangedSuccessfully = false;

			// we can't proceed without a user
			if (this.CurrentUser == null)
			{
				throw new Exception("User must be set prior to changing password");
			}

			// make sure we have all the values before we update the password
			if (!string.IsNullOrEmpty(this.OldPassword) && !string.IsNullOrEmpty(this.NewPassword) && !string.IsNullOrEmpty(this.NewPasswordConfirm))
			{
				// someone tried to change their password
				this.AttemptedChange = true;

				// Everything looks good!
				if (this.CurrentUser != null && this.CurrentUser.Password == this.OldPassword && this.NewPassword == this.NewPasswordConfirm)
				{
					// update the this.CurrentUsers password
					this.CurrentUser.Password = this.NewPassword;
					// make sure that they are not forced to change their password
					this.CurrentUser.ForceChangePassword = false;
					// save the changes in the db
					this.DBInstance.SaveChanges();
					// set the boolean for return
					bPasswordChangedSuccessfully = true;

				}
				// Error state - incorrect password
				else if (this.CurrentUser != null && this.CurrentUser.Password != this.OldPassword)
				{
					this.ErrorMessage = "Incorrect old password.";
				}
				// Error state - new passwords don't match for some reason
				else if (this.CurrentUser != null && this.NewPassword != this.NewPasswordConfirm)
				{
					this.ErrorMessage = "New passwords did not match.";
				}
				else // user is null -- this should almost never happen
				{
					this.ErrorMessage = "Unable to change password.";
				}
			}

			return bPasswordChangedSuccessfully;
		}
	}
}