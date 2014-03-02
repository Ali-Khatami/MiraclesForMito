using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Models
{
	public class PaginationModel
	{
		/// <summary>
		/// How many items you want to see per page.
		/// </summary>
		public int? PageSize { get; set; }
		/// <summary>
		/// What page we are currently on.
		/// </summary>
		public int? PageIndex {get;set; }
		/// <summary>
		/// The total amount of items to paginate through.
		/// </summary>
		public int? TotalCount {get;set; }
		/// <summary>
		/// The url to find the pagination information from.
		/// </summary>
		public string AJAXUrl { get; set; }
		/// <summary>
		/// Additional information that will be passed to the ajax url.
		/// </summary>
		public string AdditionalData { get; set; }
	}
}