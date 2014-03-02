using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MiraclesForMito.Models
{
	public enum BlogFilterType
	{
		None,
		Author,
		SEOTitle
	}

	public class BlogPaginationModel : PaginationModel
	{
		private const int DEFAULT_PAGE_SIZE = 5;

		/// <summary>
		/// States whether this is an archive or upcoming view.
		/// </summary>
		public BlogFilterType FilterType;

		/// <summary>
		/// The value you want to filter the posts with.
		/// </summary>
		public string FilterValue;

		/// <summary>
		/// The instance of the site's database.
		/// </summary>
		private SiteDB _DBInstance;

		/// <summary>
		/// The message that will display when there are no items from the filters.
		/// </summary>
		public string NoPostsMessage
		{
			get
			{
				string sMessage = null;

				switch (this.FilterType)
				{
					default:
					case BlogFilterType.None:
						sMessage = "No blog posts.";
						break;
					case BlogFilterType.Author:
						sMessage = "No posts available for this author.";
						break;
					case BlogFilterType.SEOTitle:
						sMessage = "Unable to find blog post.";
						break;
				}

				return sMessage;
			}
		}
		
		/// <summary>
		/// The filtered and sorted events to display.
		/// </summary>
		public IQueryable<BlogPost> SortedAndFilteredPosts;

		public BlogPaginationModel() : this(new SiteDB(), BlogFilterType.None, null, null, null) { }

		public BlogPaginationModel(SiteDB dbInstance) : this(dbInstance, BlogFilterType.None, null, null, null) { }

		public BlogPaginationModel(SiteDB dbInstance, int? pageIndex, int? pageSize) : this(dbInstance, BlogFilterType.None, null, pageIndex, pageSize) { }

		public BlogPaginationModel(SiteDB dbInstance, BlogFilterType filterType, string filterValue) : this(dbInstance, filterType, filterValue, null, null) { }

		public BlogPaginationModel(SiteDB dbInstance, BlogFilterType filterType, string filterValue, int? pageIndex, int? pageSize)
		{
			base.PageIndex = pageIndex.GetValueOrDefault(0);
			base.PageSize = pageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE);
			base.AdditionalData = JsonConvert.SerializeObject(new { FilterType = (int)filterType, FilterValue = filterValue });
			this.FilterType = filterType;
			this.FilterValue = filterValue;
			this._DBInstance = dbInstance;
			this.SortedAndFilteredPosts = this._SortAndFilterPosts();
		}

		private IQueryable<BlogPost> _SortAndFilterPosts()
		{
			IQueryable<BlogPost> posts = null;

			switch (this.FilterType)
			{
				case BlogFilterType.None:
					// set the total count
					base.TotalCount = _DBInstance.BlogPosts.Count();

					posts = _DBInstance.BlogPosts.OrderByDescending(post => post.UpdatedDate)
									.Skip(base.PageIndex.GetValueOrDefault(0) * base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE))
									.Take(base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE));
					break;
				case BlogFilterType.Author:
					// find the upcoming events
					posts = _DBInstance.BlogPosts.Where(post => !string.IsNullOrEmpty(post.Author) && post.Author.ToLower() == this.FilterValue.ToLower());

					// set the total count
					base.TotalCount = posts.Count();

					posts = posts.OrderByDescending(post => post.UpdatedDate)
									.Skip(base.PageIndex.GetValueOrDefault(0) * base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE))
									.Take(base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE));
					break;
				case BlogFilterType.SEOTitle:
					// find the upcoming events
					posts = _DBInstance.BlogPosts.Where(post => !string.IsNullOrEmpty(post.SEOLink) && post.SEOLink.ToLower() == this.FilterValue.ToLower());

					// set the total count
					base.TotalCount = posts.Count();

					posts = posts.OrderByDescending(post => post.UpdatedDate)
									.Skip(base.PageIndex.GetValueOrDefault(0) * base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE))
									.Take(base.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE));
					break;
			}

			return posts;
		}
	}
}