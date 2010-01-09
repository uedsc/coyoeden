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
var HtmlEditor = function() {
    var p = {};
    p.tinymceOpts = {
        script_url: 'assets/editors/tiny_mce/tiny_mce.js',
        // General options
        mode: "exact",
        theme: "advanced",
        plugins: "inlinepopups,fullscreen,contextmenu,emotions,table,iespell,advlink",
        convert_urls: false,
        // Theme options
        theme_advanced_buttons1: "fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,outdent,indent,|,iespell,link,unlink,sub,sup,removeformat,cleanup,charmap,emotions,|,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,

        tab_focus: ":prev,:next"
    };
    p.initEditor = function() {
        if (p.isTinyMce) {
            $(document).ready(function() {
                $('#' + p.hostID).tinymce(p.tinymceOpts);
            });
        };
    };
    var pub = {};
    pub.Init = function(opts) {
        p.hostID = opts.hostID;
        p.webRoot = opts.webRoot;
        p.isTinyMce = opts.isTinyMce || true;
        p.tinymceOpts.script_url = p.webRoot + p.tinymceOpts.script_url;
        $.extend(p.tinymceOpts, opts.tinymceOpts || {});
        p.initEditor();
    };
    return pub;
} ();