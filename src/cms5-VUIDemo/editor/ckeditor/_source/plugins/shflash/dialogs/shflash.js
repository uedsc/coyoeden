/**
 * @author levinhuang
 */
(function(){
	//Internal methods used by the dialog definition
	//0,utils functions
	var dlgInfo={
		ifDoc:null /* reference to the iframe document */
	};
	//1,dialog callback functions
	var dlgcbk={};
	dlgcbk.show=function(editor,dlg){
		/* 定义editor目前的插件的元信息 Meta*/
		editor.M={
			type:'shflash',
			dom:null,
			html:null,
			flashID:null
		};
	};
	dlgcbk.hide=function(editor,dlg){
		//use dlgInfo.ifDoc to reference the iframe document
		if(editor.M.html){
			//editor.shFlash=new sohu.diyTp.Flash({tplID:editor.shFlashID});
			editor.setData(editor.html,function(){
				var $d=$(editor.document.$).find(".shflash");
				//editor.shFlash.Render($d);
			});

		}		
	};
	//2,dialog content definition
	var dlgct=function(editor,src){
		var src0=CKEDITOR.getUrl(
			'_source/' + // @Packager.RemoveLine
			'plugins/shflash/file/data.html' );
			src=src||src0;
		var ct=[];
		var p1={
			id:'info',
			label:editor.lang.shflash.title,
			expand:true,
			elements:
			[
				{
					type: 'iframe',
					src: src,
					width: '100%',
					height: '100%',
					onContentLoad:function(){
						//iframe is loaded
						dlgInfo.iframe=document.getElementById(this._.frameId);
						dlgInfo.ifDoc=dlgInfo.iframe.contentWindow;
						//dlgInfo.iframe.width=dlgInfo.ifDoc.innerWidth;
						//dlgInfo.iframe.height=dlgInfo.ifDoc.innerHeight;	
					}
				}
			]
		};
		ct.push(p1);
		return ct;
	};
	//3,dialog definition
	var dlgDef=function(editor,src){
		return{
			title : editor.lang.shflash.title/* title in string*/,
			minWidth : 600 /*number of pixels*/,
			minHeight : 300 /*number of pixels*/,
			onHide: function(){dlgcbk.hide(editor,this);}/*function*/ ,
			onShow: function(){ dlgcbk.show(editor,this); }/*function*/,
			resizable: 'none' /* none,width,height or both  */,
			buttons:[],
			contents: dlgct(editor, src) /*content definition, basically the UI of the dialog*/		
		};
	};
	//4,add the dialog
	CKEDITOR.dialog.add('shflash',function(editor){return dlgDef(editor);});
})(); 