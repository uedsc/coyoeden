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
        if (p.appTip && p.appTip.Body.length > 0) {
            LocalApp.jqMTipX(p.appTip.Body, null, p.appTip.IsError ? "alert_error" : "alert_ok", 2000);
        };
    };
    p.getAddWidgetInfo = function(showError) {
        var info = $("#widgetAdder select").serializeX();
        var isOk = true;
        alert($.toJSON(info));
        if (showError) {
            //
        };
        return { isOk: isOk, data: info };
    };
    p.onWidgetAdd = function(evt) {
        var info = p.getAddWidgetInfo(true);
        if (info.isOk) {
            LocalApp.Loading(1);
            $.ajaxJsonPost(LocalApp.Asmx("WidgetService", "Add"), $.toJSON({ data: info.data }), function(msg) {
                LocalApp.Loading(0);
            }, LocalApp.Loading);
        };
        return false;
    };
    var pub = {};
    pub.Init = function(opts) {
        if (opts.appTip.length > 0) { p.appTip = $.evalJSON(opts.appTip); p.showAppTip(); };
        $("#lnkSave").click(p.onWidgetAdd);
    };
    return pub;
} ();