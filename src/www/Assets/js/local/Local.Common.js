///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.validate.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
/*
* Common js for the LocalApp web Project
* Requirements:jquery.js
* lastUpdate:20090701
*/

$.blockUI.defaults.overlayCSS = { backgroundColor: '#6AA5D7', opacity: .5 };

var LocalApp = {};
LocalApp.jqMTipX = function(msg, jqmSelector, tipType) {
	///<summary>Pop up a tipbox using jqModal</summary>
	$.jqMExt.jqm(jqmSelector || "#jqModal1", { title: msg, flag: tipType || 'alert_error' });
}; //endof TipX
LocalApp.PopUpload = function(opts) {
	///<summary>Init a Uploader</summary>
	///<param name="opts">E.g:{rel:"category",uploader:"SWF"}</param>
	window.scrollTo(0, 0);
	//opts.t = Math.PI * Math.random();
	var uploadSRC = LocalApp.WebRoot + 'aspx/upload.aspx?' + $.param(opts);

	var items = $("#PortalUploader");
	if (items.size() > 0) {
		$("#jqModal1").jqmShow();
		$("#PortalUploader").attr("src", uploadSRC); //can't use items.attr here...firefox don't work
		return;
	};

	var size = { 'height': 200, 'width': 'auto' };
	size = $.extend(size, opts.size || {});
	var w = size.width == "auto" ? "99%" : (size.width + 'px');
	var iframe = document.createElement('iframe');
	iframe.name = 'PortalUploader';
	iframe.id = 'PortalUploader';
	iframe.style.height = size.height + 'px';
	iframe.style.width = w;
	iframe.style.backgroundColor = 'white';
	iframe.style.border = '1px solid #888';
	iframe.frameborder = '0';
	iframe.scrolling = 'no';
	$(iframe).attr("title", "文件上传部件");

	$("<div id='PortalUploaderWrapper' style='display:none;'></div>").append(iframe).appendTo("body");

	$.jqMExt.jqm("#jqModal1", { title: "文件上传部件", content: $("#PortalUploaderWrapper").html(), mode: '1' });
	$("#PortalUploaderWrapper").empty();
	$("#PortalUploader").attr("src", uploadSRC);
	//ie6 sitll css fix
	if ($.browser.msie) {
		var p = $("#PortalUploader").parent(); p.width(p.width()).css("overflow", "hidden"); p = null;
	};
};               //endof LocalApp.PopUpload
LocalApp.InitUpload = function(container, opts) {
	var uploadSRC = LocalApp.WebRoot + 'aspx/upload.aspx?' + $.param(opts);

	var items = $("#PortalUploader");
	if (items.size() > 0) {
		items.attr("src", uploadSRC);
		return;
	};

	var size = { 'height': 200, 'width': 'auto' };
	size = $.extend(size, opts.size || {});
	var iframe = document.createElement('iframe');
	iframe.name = 'PortalUploader';
	iframe.id = 'PortalUploader';
	iframe.style.height = size.height + 'px';
	iframe.style.width = size.width == "auto" ? "99%" : (size.width + 'px');
	iframe.style.backgroundColor = 'white';
	iframe.style.border = '1px solid #888';
	iframe.frameborder = '0';
	iframe.scrolling = 'no';

	$(iframe).attr("title", "文件上传部件");
	iframe.src = uploadSRC;

	$(container).append(iframe);
};      //endof InitUpload
LocalApp.Asmx = function(serviceName,actionName) {
	return LocalApp.WebRoot + "Services/" + serviceName + ".asmx/"+actionName;
}; //endof Asmx
LocalApp.SetData = function(name, val, key) {
///<summary>cache data for document</summary>
	key = key || "cfg";
	var _data = $(document).data(key) || {};
	_data[name] = val;
	$(document).data(key, _data);
}; //endof setdata
LocalApp.GetData = function(cachedKey) {
///<summary>get cached data of document</summary>
	return $(document).data(cachedKey);
}; //endof getdata
LocalApp.SetDataF = function(name, val, key) {
///<summary>cache data for #aspnetForm</summary>
	key = key || "cfg";
	var _data = $("#aspnetForm").data(key) || {};
	_data[name] = val;
	$("#aspnetForm").data(key,_data);
}; //endof setdataf
LocalApp.GetDataF = function(cachedKey) {
	///<summary>get cached data of document</summary>
	return $("#aspnetForm").data(cachedKey);
}; //endof getdata
$(document).ready(function() {
	//ie6 bng fix.fuck ie6
	$.ifixpng(LocalApp.WebRoot + "assets/img/pixel.gif");
	$(".ipng").ifixpng();
});

/*=====================jQuery.Validate ext============================*/
jQuery.validator.messages.required = "不能为空";
jQuery.validator.messages.number = "请输合法的数字";
jQuery.validator.messages.digits = "请输数字";
jQuery.validator.messages.min = jQuery.validator.format("请输>={0}的值");
jQuery.validator.messages.max = jQuery.validator.format("请输<={0}的值");
jQuery.validator.messages.dateISO = "日期格式应为yyyy-MM-dd";

/*====================swfupload ext==================================*/