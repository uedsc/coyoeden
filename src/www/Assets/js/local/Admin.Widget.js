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
    p.checkAppTip = function(opts) {
        if (p._appTip.Body && p._appTip.Body.length > 0) {
            LocalApp.jqMTipX(p._appTip.Body, null, p._appTip.IsError ? "alert_error" : "alert_ok", 2000);
        };
    };
    p.getAddWidgetInfo = function(showError) {
        var w = p._listWidget.val();
        var isOk = true;

        if (w == "" && showError) {
            alert("Please select a widget!");
            p._listWidget.focus();
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
                p._appTip = msg = msg.d || msg;
                LocalApp.Loading(0);
                p.checkAppTip();
            }, LocalApp.Loading);
        };
        return false;
    };
    p.initVar = function(opts) {
        p._listWidget = $("#listWidget");
        p._lnkSave = $("#lnkSave");
        p._listOrderType = $("#listOrderType");
        p._appTip = opts.appTip;
        p._qData = opts.qData;
        //restore obj states
        
    };
    p.initEvents = function(opts) {
        //event registers
        p._lnkSave.click(p.onWidgetAdd);

    }; //endof initEvents
    var pub = {};
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
        p.checkAppTip(opts);
    };
    return pub;
} ();