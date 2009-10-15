///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="../jquery/jquery.validate.js"/>
///<reference path="Local.Common.js"/>
///<reference path="../date.js"/>
var MSetting = function() {
	//private
	var pr = {};
	pr.ddlTheme = null;
	pr.txtGeocodingLatitude = null;
	pr.txtGeocodingLongitude = null;
	pr.geoFound = function(pos) {
		pr.txtGeocodingLatitude.val(pos.latitude);
		pr.txtGeocodingLongitude.val(pos.longtitude);
	};
	pr.geoNotFound = function() {
		alert("You must be on a wifi network for us to determine your location");
	};
	//public
	var pu = {};
	pu.PreviewTheme = function() {
		var url = LocalApp.WebRoot + "default.aspx?theme=" + pr.ddlTheme.val();
		window.open(url);
		return false;
	};
	pu.Init = function(opts) {
		pr.ddlTheme = $("#" + opts.ddlThemeID);
		pr.txtGeocodingLatitude = $("#" + opts.txtGeocodingLatitudeID);
		pr.txtGeocodingLongitude = $("#" + opts.txtGeocodingLongitudeID);
		$("#btnPreviewTheme").click(pu.PreviewTheme);
	};
	pu.GeodeAsk = function() {
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(pr.geoFound, pr.geoNotFound);
		};
	};
	return pu;
} ();