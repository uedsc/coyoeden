/**
 * @author levin
 */
var this$ = function() {
	var p={},pub={};
	p.loadCss=function(){
		var h=$("head");
		$(p._externalCss).each(function(i,o){
			h.append('<link href="'+p._cke.siteRoot+o+'" rel="stylesheet" type="text/css"></link>');
		});
	};
	p.mouseover=function(evt){
		var $item=$(this);
		p._itemCover.css({
			width:$item.width(),
			height:$item.height(),
			opacity:0.5
		}).show().prependTo($item);
	};
	p.select=function(evt){
		 var _this=$(this);
		 var $ctWrap=$("#ctWrap").find(".ct").attr("id","ct_"+new Date().getTime()).end();
		 p._cke.M.flashID=_this.parent().attr("id");
		 p._cke.M.flash=new parent.sohu.diyTp.Flash({tplID:p._cke.shFlashID});	
		 p._cke.M.flash.Render($ctWrap.find(".ct"));	 
		 p._cke.M.html=$ctWrap.html();
		 //p._cke.shFlashID=_this.parent().attr("id");
		 //p._cke.shFlash=new parent.sohu.diyTp.Flash({tplID:p._cke.shFlashID});
		 p._ckeDlg.hide();
	};
	//private area
	p.initVar = function(opts) {
		p._itemCover=$("#itemCover");
		p._cke=parent.sohu.diyHtmlEditor.Editor();
		p._ckeDlg=parent.CKEDITOR.dialog.getCurrent();
		p._externalCss=['css/global1.3.css','css/content.css','css/content.shplugins.css'];
	};
	p.onLoaded = function() { };
	p.initEvents = function(opts) {
		$(document).ready(p.onLoaded);
		$("li.item").mouseover(p.mouseover);
		p._itemCover.click(p.select);
		p.loadCss();
	};
	//public area
	pub.Init = function(opts) {
		p.initVar(opts);
		p.initEvents(opts);
	};
	return pub;
} ();