var Navigation = function () { }

Navigation.prototype.init = function ()
{
	if (!$('#MainNav').hasClass("admin"))
	{
		this._cacheElements();
		this._bindEvents();
	}
};

Navigation.prototype._cacheElements = function () {
    this.$navItems = $('.nav').find("li > a");
};

Navigation.prototype._bindEvents = function () {
    $('#MainNav').on(
        'click',
        'a',
        $.proxy(function (e) {
            this._animateAnchor(e);
        }, this)
    );

    var context = this;
    $(document).scroll(
       $.proxy(function () {
           var $ancActive;

           // clear the active nav and get the correct nav to highlight
           for (var i = 0; i < context.$navItems.length; i++) {
               var $this = $(context.$navItems[i]);
               var sHref = $this.attr("href");

               if (sHref.indexOf("#") == 0) {
                   $this.closest("li").removeClass("active");

                   if ($(window).scrollTop() >= $(sHref).position().top) {
                       $ancActive = $this;
                   }
               }
           };

           // set the active nav item
           $ancActive.closest("li").addClass("active");
       }, context)
   );
};

Navigation.prototype._animateAnchor = function (e) {
    var $this = $(e.currentTarget);
    var destinationId = $this.attr("href");
    var $destination = $(destinationId);
    var offset;

    if (destinationId.indexOf("#") > -1) {
        e.preventDefault();

        offset = $destination.offset();
        $('html,body').animate({ scrollTop: offset.top }, function () {
            location.hash = destinationId;
        });

        return false;
    }
};

$(document).ready(function () {
    var nav = new Navigation();
    nav.init();
});