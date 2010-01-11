///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.validate.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
/*
* Common js for the LocalApp web Project
* Requirements:jquery.js
* lastUpdate:20090701
*/
if ($.blockUI) {
	$.blockUI.defaults.overlayCSS = { backgroundColor: '#6AA5D7', opacity: .5 };
};
var LocalApp = {};
LocalApp.Loading = function(f) {
    if (!f) {
        if (LocalApp.LOADER) {
            $("#header").before(LocalApp.LOADER.html("Server error...")); return false;
        };
    };
    if (f == 1) {//loading...
        if (LocalApp.LOADER) { $("#header").before(LocalApp.LOADER.html("Loading...")); };
    } else if (f == 0) {//hide
        LocalApp.LOADER = $("#web_loading").remove();
    };
};
LocalApp.jqMTipX = function(msg, jqmSelector, tipType, timeout) {
    ///<summary>Pop up a tipbox using jqModal</summary>
    var onshow = null;
    if (timeout && timeout > 0) {
        onshow = function(jqmApi) {
            window.setTimeout(function() {jqmApi.w.jqmHide(); }, timeout);
        };
    };
    $.jqMExt.jqm(jqmSelector || "#jqModal1", { title: msg, flag: tipType || 'alert_error', onShow: onshow });
};          //endof TipX
LocalApp.IFrame = function(container, src, opts) {
	///<summary>create a iframe</summary>
	///<param name="container">where the iframe should be put.If equals 'pop',a popup iframe will be created using jqModal.</param>
	///<param name="src">src of the iframe.</param>
	///<param name="opts">additional parameters.opts.data will be converted as the query string of the iframe.</param>
	if (src.indexOf("http://") < 0) {
		src = LocalApp.WebRoot + src;
	};
	if (opts && opts.data) {
		src = src + "?" + $.param(opts.data);
	};

	var id = opts.id || "LocalAppIFrame";
	var items = $("#" + id);
	if (items.size() > 0) {
		if (container == "pop") { $("#jqModal1").jqmShow(); };
		$("#" + id).attr("src", src);//can't use items here...firefox doesn't work.
		return;
	};

	var size = { 'height':200, 'width': 'auto' };
	size = $.extend(size, opts.size || {});
	var iframe = document.createElement('iframe');
	iframe.name = id;
	iframe.id = id;
	iframe.style.height = size.height == 'auto' ? "61.8%" : (size.height + 'px');
	iframe.style.width = size.width == "auto" ? "99%" : (size.width + 'px');
	iframe.style.backgroundColor = 'white';
	iframe.style.border = '1px solid #888';
	iframe.frameborder = '0';
	iframe.scrolling = 'no';

	iframe.src = src;
	if (container == "pop") {
		if (LocalApp.LocalAppIFrameWrapper) {
			LocalApp.LocalAppIFrameWrapper.append(iframe).appendTo("body");
		} else {
			$("<div id='LocalAppIFrameWrapper' style='display:none;'></div>").append(iframe).appendTo("body");
			LocalApp.LocalAppIFrameWrapper = $("#LocalAppIFrameWrapper").remove();
		};
		$.jqMExt.jqm("#jqModal1", { title: opts.title || "LocalAppIFrame", content: LocalApp.LocalAppIFrameWrapper.html(), mode:opts.mode||'-2',okClick:opts.okClick||null});
		//ie6 sitll css fix
		if ($.browser.msie) {
			var p = $("#"+id).parent(); p.width(p.width()).css("overflow", "hidden"); p = null;
		};
	} else {
		$(container).append(iframe);
	};
};                     //endof InitUpload
LocalApp.InitUpload = function(container, opts) {
	var uploadSRC = LocalApp.WebRoot + 'aspx/upload.aspx';
	LocalApp.IFrame(container, uploadSRC, { title: '文件上传部件', id: 'PortalUploader', data: opts });
};        //endof InitUpload
LocalApp.PopUpload = function(opts) {
	///<summary>Init a Uploader</summary>
	///<param name="opts">E.g:{rel:"category",uploader:"SWF"}</param>
	window.scrollTo(0, 0);
	//opts.t = Math.PI * Math.random();
	var uploadSRC = LocalApp.WebRoot + 'aspx/upload.aspx';
	LocalApp.IFrame("pop", uploadSRC, {title:'文件上传部件',id:'PortalUploader',data:opts});
};                //endof LocalApp.PopUpload
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
	if ($.ifixpng) {
		$.ifixpng(LocalApp.WebRoot + "assets/img/pixel.gif");
		$(".ipng").ifixpng();
	};
});

/*=====================jQuery.Validate ext============================*/
if (jQuery.validator) {
	jQuery.validator.messages.required = "不能为空";
	jQuery.validator.messages.number = "请输合法的数字";
	jQuery.validator.messages.digits = "请输数字";
	jQuery.validator.messages.min = jQuery.validator.format("请输>={0}的值");
	jQuery.validator.messages.max = jQuery.validator.format("请输<={0}的值");
	jQuery.validator.messages.dateISO = "日期格式应为yyyy-MM-dd";
};
/*====================swfupload ext==================================*/