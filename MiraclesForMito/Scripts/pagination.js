/*

Example

<div class="pagination-container" data-pageSize="5" data-totalItems="100" data-pageUrl="~/">
	<div class="pagination-body">
		// content to paginate goes here
	</div>
	<div class="pagination-footer clear-fix">
		<a class="pagination-link pagination-link-next pull-right" href="javascript:void(0);">
			<span class="glyphicon glyphicon-chevron-right"></span>
			Next
		</a>
		<a class="pagination-link pagination-link-previous pull-right" href="javascript:void(0);">
			<span class="glyphicon glyphicon-chevron-left"></span>
			Previous
		</a>
	</div>
</div>

*/

(function ($)
{
	// this will handle all the pagination
	var paginationClass = function($paginationContainer, oSettings)
	{
		this.$PaginationContainer = $paginationContainer;
		this._Settings = oSettings;
	};

	paginationClass.fn = paginationClass.prototype;

	paginationClass.fn.Init = function ()
	{
		this._StoreSettings();
		this._CacheElements();
		this._BindEvents();
		this.RefreshView();
	};

	paginationClass.fn.Reset = function ()
	{
		this._StoreSettings();

		// go back 1 page
		if (this._Settings.pageIndex > 0 && this._Settings.pageIndex * this._Settings.pageSize >= this._Settings.totalCount)
		{
			this._Settings.pageIndex = this._Settings.pageIndex - 1;
		}

		this.RefreshView();
		this._RequestNewPage();
	};

	paginationClass.fn.RefreshView = function ()
	{
		// hide the footer to start
		this.$Footer.hide();
		// hide the links next
		this.$NextLink.hide();
		this.$PreviousLink.hide();

		// hide the footer as pagination is not needed.
		if (this._Settings.totalCount > this._Settings.pageIndex * this._Settings.pageSize)
		{
			this.$Footer.show();
		}

		// is there a previous page?
		if (this._Settings.pageIndex > 0)
		{
			this.$PreviousLink.show();
		}

		// is there a next page?
		if ((this._Settings.pageIndex + 1) * this._Settings.pageSize < this._Settings.totalCount)
		{
			this.$NextLink.show();
		}
	};

	paginationClass.fn._StoreSettings = function ()
	{
		this._Settings = {
			pageSize: Number(this.$PaginationContainer.attr("data-pageSize")),
			pageIndex: Number(this.$PaginationContainer.attr("data-pageIndex")),
			totalCount: Number(this.$PaginationContainer.attr("data-totalCount")),
			ajaxUrl: this.$PaginationContainer.attr("data-ajaxUrl"),
			additionalData: this.$PaginationContainer.attr("data-additionalData")
		};
	};

	paginationClass.fn._CacheElements = function ()
	{
		this.$PaginationBody = this.$PaginationContainer.children("div.pagination-body:first");
		this.$Footer = this.$PaginationContainer.children("div.pagination-footer:first");
		this.$NextLink = this.$Footer.children("a.pagination-link-next:first");
		this.$PreviousLink = this.$Footer.children("a.pagination-link-previous:first");
	};

	paginationClass.fn._BindEvents = function ()
	{
		this.$Footer.on(
			"click.pagination-instance",
			"a.pagination-link",
			$.proxy(this._HandlePaginationLinkClick, this)
		);

		// when pagination returns always scroll the page to the top of the container
		this.$PaginationContainer.on("paginateAlways.defaultScrollTo", $.proxy(this._ScrollToTop, this));
	};

	paginationClass.fn._ScrollToTop = function (e)
	{
		var iTop = this.$PaginationContainer.offset().top - $("#MainNav").outerHeight(true);

		$("body").animate({ scrollTop: iTop }, 400);
	};

	paginationClass.fn._HandlePaginationLinkClick = function (e)
	{
		// 1 means next, -1 means previous
		var iDirection = ($(e.currentTarget).hasClass("pagination-link-next")) ? 1 : -1;

		// either go to next page or go back to previous page
		this._Settings.pageIndex += iDirection;

		// TODO show loading graphic

		this._RequestNewPage();
	};

	paginationClass.fn._RequestNewPage = function ()
	{
		this.$PaginationContainer.trigger("beforePaginate");

		// kick off the ajax call to get the new data
		$.ajax({
			type: "GET",
			url: this._Settings.ajaxUrl.ResolveUrl(),
			data: {
				pageSize: this._Settings.pageSize,
				pageIndex: this._Settings.pageIndex,
				additionalData: this._Settings.additionalData
			},
			dataType: "html"
		})
		.always($.proxy(this._PaginationAlways, this))
		.done($.proxy(this._PaginationSuccess, this))
		.fail($.proxy(this._PaginationFail, this));
	};

	paginationClass.fn._PaginationAlways = function ()
	{
		this.$PaginationContainer.trigger("paginateAlways");
	};

	paginationClass.fn._PaginationSuccess = function (html)
	{
		this.$PaginationContainer.trigger("paginateSuccess");

		// TODO hide loading graphic

		// replace the body content with the results
		this.$PaginationBody.html(html);

		// call was successful, change the page index.
		this.$PaginationContainer.attr("data-pageIndex", this._Settings.pageIndex);

		// refresh everything
		this.RefreshView();
	};

	paginationClass.fn._PaginationFail = function (jqxhr)
	{
		this.$PaginationContainer.trigger("paginateFail");
		// TODO hide loading graphic
	};

	$.fn.paginate = function (sAction)
	{
		return this.each(function ()
		{
			var $this = $(this);

			if (!$this.data("paginate"))
			{
				var paginationInstance = new paginationClass($this);
				paginationInstance.Init();

				// create the pagination instance and save it in the data of the container
				$this.data("paginate", paginationInstance);
			}
			// call the action on the paginate instance
			else if (typeof ($this.data("paginate")[sAction]) === "function")
			{
				$this.data("paginate")[sAction]();
			}
		});
	};

})(jQuery);

$(function ()
{
	// automatically init pagination on page load
	$("div.pagination-container").paginate();
});