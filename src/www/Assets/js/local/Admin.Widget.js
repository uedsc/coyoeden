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
        var w = p.listWidget.val();
        var isOk = true;

        if (w == "" && showError) {
            alert("Please select a widget!");
            p.listWidget.focus();
            isOk = false;
        };
        return { isOk: isOk, widget: w };
    };
    p.onWidgetAdd = function(evt) {
        var z = $(this).attr("rel");
        var info = p.getAddWidgetInfo(true);
        if (info.isOk) {
            LocalApp.Loading(1);
            $.ajaxJsonPost(LocalApp.Asmx("WidgetService", "Add"), $.toJSON({ data: { Name: info.widget, Zone: z} }), function(msg) {
                p.appTip = msg = msg.d || msg;
                LocalApp.Loading(0);
                p.showAppTip();
            }, LocalApp.Loading);
        };
        return false;
    };
    p.initObjs = function(opts) {
        p.listWidget = $("#listWidget");
        p.lnkSave = $("#lnkSave");
    };
    var pub = {};
    pub.Init = function(opts) {
        p.initObjs(opts);
        if (opts.appTip.length > 0) { p.appTip = $.evalJSON(opts.appTip); p.showAppTip(); };
        p.lnkSave.click(p.onWidgetAdd);
    };
    return pub;
} ();