using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Models
{
	public class NotifiedListItem
	{
		/// <summary>
		/// The ID of the item
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Is required and must be a valid email.
		/// </summary>
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}