using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiraclesForMito.Utilities;

namespace MiraclesForMito.Models
{
	public class ForgotPasswordModel
	{
		/// <summary>
		/// The email address to find the user by.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Determines whether or not an attempt to reset password was actually made.
		/// </summary>
		public bool AttemptedReset;

		/// <summary>
		/// The error message for the model.
		/// </summary>
		public string ErrorMessage;

		protected SiteDB DBInstance;

		/// <summary>
		/// Set the db instance to allow you to find the user in SendNewPassword
		/// </summary>
		/// <param name="dbInstance"></param>
		public void SetDBInstance(SiteDB dbInstance)
		{
			this.DBInstance = dbInstance;
		}

		public bool SendNewPassword()
		{
			if (this.DBInstance == null)
			{
				throw new Exception("Database instance must be passed before calling SendNewPassword");
			}

			bool bSuccessfullyReset = false;

			// someone actually passed an email rather than just the page loading.
			if (!string.IsNullOrEmpty(this.Email))
			{
				// let the view know someone actually tried to reset their password.
				this.AttemptedReset = true;

				// try to find the user by email
				var userToReset = this.DBInstance.Admins.FirstOrDefault(user => user.Email.ToLower() == this.Email.ToLower());

				// reset the user
				if (userToReset != null)
				{
					string sPasswordToSave = System.Web.Security.Membership.GeneratePassword(8, 5);
					userToReset.ForceChangePassword = true;
					userToReset.Password = sPasswordToSave;

					bSuccessfullyReset = EmailHelpers.SendEmail(
						new System.Net.Mail.MailMessage(EmailHelpers.SEND_EMAIL_ADDRESS, userToReset.Email)
						{
							Subject = "Miracles for Mito -- Password Reset",
							Body = @"Dear " + userToReset.FirstName + " " + userToReset.LastName + @",<br/><br/>
								Your password has been reset. Your credentials are as follows:<br/><br/>
								<strong>Username:</strong> <em>[this email]</em><br/>
								<strong>Password:</strong> " + sPasswordToSave + @"<br/><br/>
						
							Sincerely,<br/>
							The Miracles for Mito Dev Team
							",
							IsBodyHtml = true
						}
					);

					// only save the changes if the email was actually sent.
					if (bSuccessfullyReset)
					{
						this.DBInstance.SaveChanges();
					}
					else
					{
						this.ErrorMessage = "Unable to reset password. Please try again.";
					}
				}
				else
				{
					this.ErrorMessage = string.Format("Unable to locate user {0}.", this.Email);
				}
			}

			return bSuccessfullyReset;
		}
	}
}