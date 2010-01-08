///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="Local.Common.js"/>
///<reference path="../date.js"/>
var WidgetApp = function() {
	var p = {};
	p.showAppTip = function() {
		if (p.appTip&&p.appTip.Body.length>0) {
			LocalApp.jqMTipX(p.appTip.Body);
		};
	};
	var pub = {};
	pub.Init = function(opts) {
		if (opts.appTip.length>0) { p.appTip = $.evalJSON(opts.appTip); p.showAppTip(); };

	};
	return pub;
} ();