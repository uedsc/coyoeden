///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
var App = function() {
	//private
	var p = {};
	p.loading = function(f) {
		if (f == 1) {
			if (App.LOADER) { $("#header").before(App.LOADER); };
		} else {
			App.LOADER = $("#web_loading").remove();
		};
	};
	p.onWidgetSort = function(data) {
		if (p.opts.admin != "1") return false;
		p.loading(1);
		$.ajaxJsonPost(LocalApp.Asmx("WidgetService", "Sort"), $.toJSON(data), function(msg) {
			p.loading(0);
		}, null);
	};
	p.onPageOk = function() {
		p.loading(0);
		$("#widgetZones .widget").xwidget({onSort:p.onWidgetSort});
	};

	//public
	var pub = {};
	pub.Init = function(opts) {
		$(document).ready(p.onPageOk);
	};
	return pub;
} ();