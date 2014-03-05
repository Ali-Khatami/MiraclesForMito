using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiraclesForMito.Models
{
	public class SiteDB : DbContext
	{
		public SiteDB() : base("DefaultConnection") { }

		public DbSet<AdminUser> Admins { get; set; }

		public DbSet<Event> Events { get; set; }

		public DbSet<BlogPost> BlogPosts { get; set; }
	}
}