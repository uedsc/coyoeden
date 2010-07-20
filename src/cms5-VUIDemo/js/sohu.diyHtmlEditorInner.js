/**
 * CKEEditor生成的iframe文档的js交互逻辑
 * @author levinhuang
 */
var this$ = function() {
    var p={},pub={};
	p.onCTClick=function(evt){
		alert("hi");
	};
    //private area
    p.initVar = function(opts) { 
	};
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		$(".ct").live("click",p.onCTClick);
		alert("hello");
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 

this$.Init({
	editor:parent.sohu.diyConsole.HtmlEditor
});
