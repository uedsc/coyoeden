///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
var App = function() {
	//private
	var p = {};
	p.ajaxError
	p.isAdmin = function() {
		if (p.opts.admin != "1") return false;
		return true;
	};
	p.loading = function(f) {
		if (!f) {
			if (App.LOADER) {
				$("#header").before(App.LOADER.html("Server error...")); return false;
			};
		};
		if (f == 1) {//loading...
			if (App.LOADER) { $("#header").before(App.LOADER.html("Loading...")); };
		} else if (f == 0) {//hide
			App.LOADER = $("#web_loading").remove();
		};
	};
	p.onWidgetSort = function(data) {
		if (!p.isAdmin()) return false;
		p.loading(1);
		$.ajaxJsonPost(LocalApp.Asmx("WidgetService", "Sort"), $.toJSON({ data: data.sdata }), function(msg) {
			p.loading(0);
		}, p.loading);
	};
	p.onWidgetEditting = function(o) {
		if (p.isAdmin()) {
			p.loading(1);
			$.ajaxJsonPost(LocalApp.Asmx("WidgetService", "Edit"), $.toJSON({ data: o }), function(msg) {
				p.loading(0); msg = msg.d || msg;
				if (!msg.IsError) {
					$.navTo();
				} else { alert(msg.Body); };
			}, p.loading);
		};
	};
	p.onPageOk = function() {
		p.loading(0);
		$(".widget").xwidget({ onSort: p.onWidgetSort, onEdit: p.onWidgetEditting });
	};

	//public
	var pub = {};
	pub.Init = function(opts) {
		p.opts = opts;
		$(document).ready(p.onPageOk);
	};
	return pub;
} ();