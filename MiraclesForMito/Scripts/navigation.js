var Navigation = function () { }

Navigation.prototype.init = function () {
    this._bindEvents();
};

Navigation.prototype._bindEvents = function () {
    $('.scroller').on(
        'click',
        'a',
        $.proxy(function (e) {
            this._animateAnchor(e);
        }, this)
    );
};

Navigation.prototype._animateAnchor = function (e) {
    var $this = $(e.currentTarget);
    var destinationId = $this.attr("href");
    var $destination = $(destinationId);
    var offset;

    $this.closest("ul").find("li.active").removeClass("active");
    $this.closest("li").addClass("active");

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