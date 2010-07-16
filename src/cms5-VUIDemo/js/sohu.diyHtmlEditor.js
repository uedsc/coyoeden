if(!sohu){
	var sohu={};
};
/**
 * 集成tinyMCE编辑器\ckeditor编辑器
 * 注意:内容和碎片需要写成tinyMCE的插件\ckeditor插件
 * @author levinhuang
 */
sohu.diyHtmlEditor = function() {
    var p={},pub={};
	p.loadContent=function(){
		if(p._editorType=="ckeditor"){
			p.editor=p.$editor.ckeditorGet();
			p.editor.getContent=p.editor.getData;
			//p.editor.setData($("#test").html());
		}else{
			p.editor=p.$editor.tinymce();//tinymce.activeEditor;
			//p.editor.execCommand('mceInsertContent',false,$("#test").html());
		}

		return;
		//load editing content if exists
		if(parent.sohu.diyConsole.CurCT){
			p.isNew=false;
			p.editor.execCommand('mceInsertContent',false,parent.sohu.diyConsole.CurCT.$Layout.html);
		}
	};
	p.cls=function(){
		parent.bos.CloseCTDialog();
	};
	p.isNew=true;
	p.submit=function(evt){
		var html=p.editor.getContent();
		alert(html);return;
		if(p.isNew){
			p.ct={
				flash:false,
				html:html,
				isNew:true,
				type:"diy_tbl"
			};
		}else{
			p.ct=parent.sohu.diyConsole.CurCT.$Layout;
			p.ct.isNew=false;
			p.ct.html=html;	
		}
		parent.bos.Editor.CurSec.AddContent(p.ct);
		p.cls();
	};
	p.initTinyMCE=function(){
		p.$editor=$('textarea.htmlEditor').tinymce(p._tinyMCECfg);
	};
	p.initTinyMCE1=function(){
		tinyMCE.init(p._tinyMCECfg);
	};
	p.initCKEditor=function(){
		// Initialize the editor.
		// Callback function can be passed and executed after full instance creation.
		p.$editor=$('.htmlEditor').ckeditor(p.loadContent,p._CKEditorCfg);
	};
    //private area
    p.initVar = function(opts) { 
		p._editorType=opts.editor||"ckeditor";
		p._btnOk=$("#btnOK");
		p._btnClose=$("#btnClose");
		//CFG for tinymce
		p._tinyMCECfg={
			// Location of TinyMCE script
			script_url : 'editor/tiny_mce/tiny_mce.js',
			// General options
			mode : "textareas",
			theme : "advanced",
			
			plugins:"fullscreen,inlinepopups,diyTable",
			//plugins : "pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave",
	
			// Theme options
			theme_advanced_buttons1:"fullscreen,diyTable",
			theme_advanced_buttons2:"",
			theme_advanced_buttons3:"",
			//theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
			//theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
			//theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
			//theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
			theme_advanced_toolbar_location : "top",
			theme_advanced_toolbar_align : "left",
			theme_advanced_statusbar_location : "bottom",
			theme_advanced_resizing : true,
	
			// Example content CSS (should be your site CSS)
			content_css : "css/content.css",
	
			// Drop lists for link/image/media/template dialogs
			template_external_list_url : "lists/template_list.js",
			external_link_list_url : "lists/link_list.js",
			external_image_list_url : "lists/image_list.js",
			media_external_list_url : "lists/media_list.js",
	
			// Style formats
			style_formats : [
				{title : 'Bold text', inline : 'b'},
				{title : 'Red text', inline : 'span', styles : {color : '#ff0000'}},
				{title : 'Red header', block : 'h1', styles : {color : '#ff0000'}},
				{title : 'Example 1', inline : 'span', classes : 'example1'},
				{title : 'Example 2', inline : 'span', classes : 'example2'},
				{title : 'Table styles'},
				{title : 'Table row 1', selector : 'tr', classes : 'tablerow1'}
			],
	
			// Replace values for the template plugin
			template_replace_values : {
				username : "Some User",
				staffid : "991234"
			},
			oninit:p.loadContent
		};
		//cfg for CKE
		p._CKEditorCfg= {
			language:'zh-cn',
			toolbar:
			[
				['Source','Maximize','-','Undo','Redo','-','SHTable','SHLine','SHText','-','SHImage','SHFlash']
			],
			extraPlugins:'shtable,shline,shtext,shimage,shflash',
			removePlugins:'uicolor,table',
			menu_groups:'clipboard,form,tablecell,tablecellproperties,tablerow,tablecolumn,shtable,anchor,link,image,flash,checkbox,radio,textfield,hiddenfield,imagebutton,button,select,textarea',
			contentsCss:[CKEDITOR.basePath+'contents.css','css/content.css'],
			others:''
		};
	};
    p.onLoaded = function() { 
		$("#web_loading").remove();
		//parent.sohu.diyConsole.toggleLoading();
		//init htmlEditor instance
		if (p._editorType == "ckeditor") {
			p.initCKEditor();
		}
		else {
			p.initTinyMCE();
		//p.initTinyMCE1();
		}
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		p._btnClose.click(p.cls);
		p._btnOk.click(p.submit);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 