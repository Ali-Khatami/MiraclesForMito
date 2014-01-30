using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Models
{
	public class BlogPost
	{
		public int ID { get; set; }

		/// <summary>
		/// The title for the blog post
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The content of the blog post
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// The author of the post.
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// The date the post was created.
		/// </summary>
		public DateTime? InsertDate { get; set; }

		/// <summary>
		/// Last time the post was updated.
		/// </summary>
		public DateTime? UpdatedDate { get; set; }

		/// <summary>
		/// Allows users to link to the post specfically.
		/// </summary>
		public string SEOLink { get; set; }

		/// <summary>
		/// Determines whether or not the blog post is published.
		/// </summary>
		public bool? Published { get; set; }

		/// <summary>
		/// An event if you want to link to one.
		/// </summary>
		public virtual Event LinkedEvent { get; set; }

		/// <summary>
		/// The ID for the linked event
		/// </summary>
		public int? LinkedEventID { get; set; }
	}
}