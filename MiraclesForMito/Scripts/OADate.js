Number.prototype.fromOADate = function ()
{
	var jO = new Date(((this - 25569) * 86400000));
	var tz = jO.getTimezoneOffset();
	return new Date(((this - 25569 + (tz / (60 * 24))) * 86400000));

};

Date.prototype.toOADate = function ()
{
	var jsDate = this || new Date();
	var timezoneOffset = jsDate.getTimezoneOffset() / (60 * 24);
	var msDateObj = (jsDate.getTime() / 86400000) + (25569 - timezoneOffset);
	return msDateObj;
}