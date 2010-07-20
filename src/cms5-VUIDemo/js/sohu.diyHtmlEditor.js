/**
 * 集成tinyMCE编辑器\ckeditor编辑器
 * 注意:内容和碎片需要写成tinyMCE的插件\ckeditor插件
 * @author levinhuang
 */
sohu.diyHtmlEditor = function() {
    var p={},pub={};
	/**
	 * 往编辑器的iframe文档中注入交互脚本
	 */
	p.injectJS=function(){
		/* 此方法有bug，每次编辑器内容改变后，cke会自动重新生成一个iframe... */
		var js1='<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>';
		var js2='<script type="text/javascript" src="js/sohu.diyHtmlEditorInner.js"></script>';
		var e1=new CKEDITOR.dom.element.createFromHtml(js1,p.editor.document);
		var e2=new CKEDITOR.dom.element.createFromHtml(js2,p.editor.document);
		p.editor.document.getHead().append(e1);
		p.editor.document.getBody().getParent().append(e2);
	};
	p.onEditorInit=function(){
		if(p._editorType=="ckeditor"){
			p.editor=p.$editor.ckeditorGet();
			p.editor.siteRoot=CKEDITOR.basePath.replace("editor/ckeditor/","");
			p.editor.getContent=p.editor.getData;
		}else{
			p.editor=p.$editor.tinymce();//tinymce.activeEditor;
		};
		//scripts used by the document
		//$(p.editor.document.$).find("head").append('<script src="'+p.editor.siteRoot+"js/sohu.diy_utf8.js"+'" type="text/javascript"></script>');
		
		//load editing content if exists
		if(sohu.diyConsole.CurCT){
			//p.editor.execCommand('mceInsertContent',false,sohu.diyConsole.CurCT.$Layout.html);
			p.editor.setData(sohu.diyConsole.CurCT.$Layout.html);
		};
		
		//用户是否提供了oninit回调
		if(p._opts.onHtmlEditorInit){
			p._opts.onHtmlEditorInit(p.editor);
		};
		
		/* 编辑器iframe文档内部js交互 */
		//p.injectJS();
	};
	p.cls=function(){
		//parent.bos.CloseCTDialog();
		bos.CloseCTDialog();
		return false;
	};
	p.submit=function(evt){
		var html=p.editor.getContent();
		if(!p.editor.shmode){
			/* 新增 */
			p.ct={
				flash:false,
				html0:html,
				isNew:true,
				type:p.editor.M.type||'ohmygod'
			};
		}else{
			/* 更新 */
			var ct=$(html).filter(".ct");
			p.ct=sohu.diyConsole.CurCT.$Layout;
			p.ct.isNew=false;
			p.ct.html0=ct.length>0?ct.html():ct.find(".ct").html();
			p.ct.html0=p.ct.html0||"";	
		}
		bos.Editor.CurSec.AddContent(p.ct);
		return p.cls();
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
		p.$editor=$('.htmlEditor').ckeditor(p.onEditorInit,p._CKEditorCfg);
	};
    //private area
    p.initVar = function(opts) { 
		p._opts=opts||{};
		p._editorType=opts.editor||"ckeditor";
		//p._btnOk=$("#btnOK");
		//p._btnClose=$("#btnClose");
		p._btnOk=$("#content_selector1 .jqmOk");
		p._btnClose=$("#content_selector1 .jqmClose");
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
			oninit:p.onEditorInit
		};
		//cfg for CKE
		p._CKEditorCfg= {
			language:'zh-cn',
			toolbar:
			[
				['Source','Maximize','-','Undo','Redo','-','SHTable','SHLine','SHText','-','SHImage','SHFlash']
			],
			extraPlugins:'shtable,shline,shtext,shimage,shflash',
			removePlugins:'uicolor,table,scayt',
			menu_groups:'clipboard,form,tablecell,tablecellproperties,tablerow,tablecolumn,shtable,anchor,link,image,flash,checkbox,radio,textfield,hiddenfield,imagebutton,button,select,textarea',
			contentsCss:[CKEDITOR.basePath+'contents.css','css/global1.3.css','css/content.css'],
			others:''
		};
	};
	p.initEditor=function(){
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
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        //$(document).ready(p.onLoaded);
		//初始化htmlEditor
		p.initEditor();
		//按钮事件注册
		//p._btnClose.click(p.cls);
		p._btnOk.click(p.submit);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	pub.Editor=function(){
		return p.editor;
	};
    return pub;
} (); 