var this$ = (function($) {
    var p={},pub={};
    p.loadMCE=function(){
		$("#edt_hoder").tinymce({
			script_url:'assets/javascripts/tiny_mce/tiny_mce.js',
			theme:'advanced',
			skin:'lvLuna',
			width:"100%",
			plugins : "safari,inlinepopups,spellchecker,paste,media,fullscreen,tabfocus",
			// Theme options
			theme_advanced_buttons1 : "bold,italic,strikethrough,|,bullist,numlist,blockquote,|,justifyleft,justifycenter,justifyright,|,link,unlink,|,fullscreen",
			theme_advanced_buttons2 : "styleselect,formatselect,underline,justifyfull,forecolor,|,pastetext,pasteword,removeformat,|,media,charmap,|,outdent,indent,|,undo,redo",
			theme_advanced_buttons3 : "",
			theme_advanced_buttons4 : "",
			theme_advanced_toolbar_location : "top",
			theme_advanced_toolbar_align : "left",
			theme_advanced_statusbar_location : "bottom",
			theme_advanced_resizing : true,
			content_css:'css/editor.css',
			theme_advanced_resize_horizontal:"",
			dialog_type:"modal"		
		});
	};
	//private area
    p.initVar = function(opts) { };
    p.onLoaded = function() { 
		p.loadMCE();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
}) (jQuery); 