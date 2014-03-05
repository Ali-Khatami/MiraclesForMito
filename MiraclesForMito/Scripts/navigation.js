var Navigation = function () { }

Navigation.prototype.init = function ()
{
	this.$MainNav = $('#MainNav');
	if (this.$MainNav.length && this.$MainNav.hasClass("scrollspy"))
	{
		this._cacheElements();
		this._bindEvents();
	}
};

Navigation.prototype._cacheElements = function () {
	this.$navItems = this.$MainNav.find('ul.nav:first').find("li > a");
};

Navigation.prototype._bindEvents = function () {
	this.$MainNav.on(
        'click',
        'a',
        $.proxy(this._animateAnchor,this)
    );
    
	$(window).on(
		"scroll.customScrollSpy",
       $.proxy(this._handleScroll, this)
   );
};

Navigation.prototype._handleScroll = function (e) {
	this.$navItems.closest("li").removeClass("active");	

	var iScrollTop = $(window).scrollTop();
	var iNavHeight = this.$MainNav.height();

	var $ancActive;

	// clear the active nav and get the correct nav to highlight
	this.$navItems.each(function(index, element)
	{
		var $this = $(element);
		// sHref == the id of the anchor
		var sHref = $this.attr("href");

		if (sHref.indexOf("#") == 0)
		{	
			if (iScrollTop + iNavHeight >= $(sHref).offset().top - 50) // creating a 50 px buffer 
			{
				$ancActive = $this;
			}
		}
	});

	// set the active nav item
	$ancActive.closest("li").addClass("active");
};

Navigation.prototype._animateAnchor = function (e) {	
	var $this = $(e.currentTarget);
    var destinationId = $this.attr("href");
    var $destination = $(destinationId);
    var offset;

    if (destinationId.indexOf("#") > -1) {
        e.preventDefault();        
        offset = $destination.offset();
        $('html,body').animate(
			{ scrollTop: offset.top },
			$.proxy(
				this._animateAnchor_Complete,
				this,
				$this,
				$destination,
				destinationId
			)
		);

        return false;
    }
};

Navigation.prototype._animateAnchor_Complete = function ($el, $destination, destinationId)
{
	location.hash = destinationId;
	this.$navItems.closest("li").removeClass("active");
	$el.closest("li").addClass("active");	
	if (destinationId == "#footer")
	{		
		this._blinkContainer($("div.main-page-footer:first"), "#555");
	}
};

Navigation.prototype._blinkContainer = function ($el, sColor)
{
	var sOriginalBGColor = $el.css("background-color");

	// start the animate
	$el.animate(
		{
			backgroundColor: sColor
		},
		500,
		function ()
		{
			// animate it back
			$(this).animate(
				{
					backgroundColor: sOriginalBGColor
				},
				250
			);
		}
	);
};

$(document).ready(function () {
    var nav = new Navigation();
    nav.init();
});