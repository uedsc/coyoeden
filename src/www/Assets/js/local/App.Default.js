///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="Local.Common.js"/>
var this$ = function() {
    //private
    var p = {};
    p.isAdmin = function() {
        if (p.opts.admin != "1") return false;
        return true;
    };
    p.onWidgetSort = function(data) {
        if (!p.isAdmin()) return false;
        LocalApp.Loading(1);
        $.ajaxJsonPost(LocalApp.Asmx("WidgetService", "Sort"), $.toJSON({ data: data.sdata }), function(msg) {
            LocalApp.Loading(0);
        }, LocalApp.Loading);
    };
    p.onWidgetEditting = function(o) {
        if (p.isAdmin()) {
            LocalApp.IFrame("pop", "admin/widgets/popshow.aspx", { title: 'Widget Editor', id: 'widgetEditorFrame', data: { id: o.Id }, size: { height: 450, width: 700} });
        };
    };
    p.onWidgetRemoving = function(o) {
        if (!p.isAdmin()) return false;
        LocalApp.Loading(1);
        $.ajaxJsonPost(
            LocalApp.Asmx("WidgetService", "Delete"), $.toJSON({ data: o.sdata }), function(msg) {
                LocalApp.Loading(0);
                if (!msg.IsError) {
                    o.Target.hide().remove();
                } else {
                    alert(msg.Body);
                };
            }, LocalApp.Loading);
    };
    p.onLoaded = function() {
        LocalApp.Loading(0);
        $(".widget").xwidget({ onSort: p.onWidgetSort, onEdit: p.onWidgetEditting, onRemove: p.onWidgetRemoving });
    };
    p.initVar = function(opts) {
        p._opts = opts;
    }; //initVar
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
    }

    //public
    var pub = {};
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    pub.CloseWidgetEditor = function() {
        $("#jqModal1").jqmHide();
        parent.window.location.reload();
    };
    return pub;
} ();