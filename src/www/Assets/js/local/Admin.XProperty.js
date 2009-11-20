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
var App = function() {
	//private
	var p = {};
	p.initShowTypeEvent = function() {

		$("#showList").click(function() {
			$(".ilist_w1").hide();
			$(".ilist_w0").show();
			$("#showList").attr("class", "byBlock_active");
			$("#showBlock").attr("class", "byList");
			//$.cookie('usr_collect_owner', 'List');
		});
		$("#showBlock").click(function() {
			$(".ilist_w0").hide();
			$(".ilist_w1").show();
			$("#showBlock").attr("class", "byList_active");
			$("#showList").attr("class", "byBlock");
			//$.cookie('usr_collect_owner', 'Block');
		});
	};
	p.setOrder = function() {
		var _value = $("select[@name=order] option[@selected]").val();
		alert(_value);
	};
	//public
	var pub = {};
	pub.Init = function() {
		$("#listOrderType").change(p.setOrder);
		p.initShowTypeEvent();
	};

	return pub;
} ();

