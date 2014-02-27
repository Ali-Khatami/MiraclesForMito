using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MiraclesForMito.Utilities
{
	public class EmailHelpers
	{
		/// <summary>
		/// The email we are sending from.
		/// </summary>
		public const string SEND_EMAIL_ADDRESS = "MiraclesForMitoDevTeam@outlook.com";
		private const string SEND_PASSWORD = "$ervic3d@y$";

		public static bool SendEmail(MailMessage msg)
		{
			try
			{
				SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
				{
					UseDefaultCredentials = false,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					Credentials = new NetworkCredential(SEND_EMAIL_ADDRESS, SEND_PASSWORD),
					 
					Port = 587,
					EnableSsl = true
				};
				smtpClient.Send(msg);
				msg.Dispose();
				smtpClient.Dispose();
				return true;
			}
			catch (Exception e)
			{
				string sMessage = e.Message;
				return false;
			}
		}
	}
}