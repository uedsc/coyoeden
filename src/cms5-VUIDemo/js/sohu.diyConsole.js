/**
 * 可视化编辑控制台
 * @author levinhuang
 * @param {Object} opts 选项,如clSec:"sec",cssWsp:"#main"表示分栏的css类为sec,工作区域的css选择器为#main
 * @dependency sohu.diyEditor.js;sohu.diyArea.js
 */
sohu.diyConsole=function(opts){
	//属性
	opts=$.extend({},{
		cssWsp:"#main",clSec:"sec",clSec0:"sec0",clSecSub:"subsec",
		clSecRoot:"col",clArea:"area",cssArea:".area",dfTop:100,
		limitSec:390,
		scrollWrapMainginTop:0
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");

	sohu.diyConsole.$SecEditorModel=$("#area_editor");
	this.Areas=null;
	
	var p={opts:opts};
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	p._$areaSelector=$("#area_selector");
	p._$pageTip=$("#pageTip");
	p._$editMenu=$("#editMenu");
	p._opts=opts;
	
	//横切删除时的回调函数
	p.onAreaRemove=function(area){
		_this.ActiveArea(null);
		_this.$Layout.animate({top:opts.dfTop});	
	};
	
	/**
	 * 显示横切选择器
	 */
	p.showSelector=function(){
		var _onClose=function(evt,ui){
			if(!p._curAreaTpl) return false;//未选中任何分栏
			var obj=new sohu.diyArea({
				tplID:p._curAreaTpl,
				console:_this,
				onRemove:p.onAreaRemove
			});
			_this.Areas.push(obj);
		};
		//显示选择框
		p._$areaSelector.dialog({
			title:"添加横切",
			resizable:false,
			modal:true,
			width:430,
			height:250,
			position:[700,50],
			close:_onClose
			}
		);
	};
	p.onAdd=function(evt){
	///<summary>添加横切</summary>
		p.showSelector();	
		return false;
	};
	p.onSelectAreaTpl=function(evt){
	///<summary>选择某个横切模板后</summary>
		p._curAreaTpl=this.id;p._$areaSelector.dialog("close");	return false;
	};
	p.onRemove=function(evt){
	///<summary>删除横切</summary>	
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		_this.Areas=$.grep(_this.Areas,function(o,i){
			if(o.ID==_this.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
		return false;	
	};
	p.onAddBG=function(evt){
	///<summary>添加横切背景</summary>	
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		alert("背景");
		return false;
	};
	p.onMove=function(evt){
	///<summary>移动横切</summary>
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		_this.RePosition();
		return false;	
	};
	p.getWorkspaceBoundary=function(){
		var lastArea=_this.$Workspace.find(opts.cssArea+":last");
		if (lastArea.size() == 0) {
			return {
				lbleft: -2000,
				ubleft: 2000,
				lbtop: -2000,
				ubtop: 2000
			};
		};
		
		var lbtop=_this.$Workspace.offset().top;
		var ubtop=lastArea.height()+lastArea.offset().top;
		var lbleft=lastArea.offset().left;
		var ubleft=lastArea.width()+lbleft;
		
		return {
			lbleft:lbleft,
			ubleft:ubleft,
			lbtop:lbtop,
			ubtop:ubtop
		};
	};
	//body标签的鼠标事件
	p.onMousemove=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		//if(sohu.diyConsole.EditingSec!=null) return;
		
		var b=p.getWorkspaceBoundary();
		
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//反激活横切
			sohu.diyConsole.CurArea.Deactive();
			//反激活分栏
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
		};
	};
	p.onBodyClick=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		
		var b=p.getWorkspaceBoundary();
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//强制移除内联编辑器
			if(sohu.diyConsole.EditingSec!=null){
				sohu.diyConsole.CurElm.HideEditor(false);
			};
			//反激活横切
			sohu.diyConsole.CurArea.Deactive();
			//反激活分栏
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
		};
		return false;
	};
	p.setDocumentDim=function(){
		var fullheight, height;
		fullheight = sohu.diyConsole.InnerHeight();        
		height = fullheight - p.opts.scrollWrapMainginTop;
		
		$("#scrollWrap").css("height",height);
		$("#main").css("minHeight",height);
	};
	p.onLoaded=function(){
		//文档高度适应处理
		p.setDocumentDim();
		//编辑工具栏位置处理
		p._$editMenu.css("top",p.opts.scrollWrapMainginTop).hide().find("#editMenuChild").draggable({containment:'#mainWrap',scroll:false});
		//横切工具条位置
		_this.$Layout.css({"top":p.opts.scrollWrapMainginTop+30});
	};
	p.Init=function(){
		//公有属性引用
		sohu.diyConsole.$EditMenu=p._$editMenu;
		//横切选择器
		$("li",p._$areaSelector).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");})
			.click(p.onSelectAreaTpl);
		
		//分栏选择器
		$('#hiddenTemplate .sec_selector li').click(function(evt){
			if(!sohu.diyConsole.CurSec) return;
			sohu.diyConsole.CurSec.Editor.SecSelector.Cur=this.id;
			//_this.SecSelector.dialog("close");
			sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
			return false;
		}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
		
		p._$btnAdd.bind("click",p.onAdd);
		p._$btnDel.click(p.onRemove);
		//隐藏删除按钮
		//p._$btnDel.parent().hide();
		p._$btnBG.click(p.onAddBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
		
		//已有横切
		_this.Areas=_this.AreaList().map(function(i,o){
			o=new sohu.diyArea({
				isNew:false,
				console:_this,
				onRemove:p.onAreaRemove
			});
			return o;
		});
		//body鼠标事件
		$("body").mousemove(p.onMousemove).click(p.onBodyClick);
		//window resize事件
		$(window).resize(function(evt){
			p.setDocumentDim();
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Editor.Reposition();
		});
		//on page loaded
		$(document).ready(p.onLoaded);
	};
	this.__p=p;
	//Init
	p.Init();
};
/**
 * 重定位
 */
sohu.diyConsole.prototype.RePosition=function(){
	if(!this.CurArea){this.$Layout.attr("style","");return;};
	this.$Layout.css("top",this.CurArea.$Layout.offset().top);
};
/**
 * 设定激活的横切对象
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//将上一个横切反激活
	if(sohu.diyConsole.CurArea){
		if(target.ID==sohu.diyConsole.CurArea.ID) 
			return this;
		
		sohu.diyConsole.CurArea.Deactive();
	};
	//激活当前的横切
	sohu.diyConsole.CurArea=target;
	return this;
};
/**
 * 关闭内容选择对话框
 */
sohu.diyConsole.prototype.CloseCTDialog=function(){
	sohu.diyConsole.CurSec.Editor.CloseCTDialog();
};
/**
 * 获取所有横切jquery对象
 */
sohu.diyConsole.prototype.AreaList=function(){
	var items= this.$Workspace.find(this.__p.opts.cssArea);
	return items;
};
/**
 * 弹出一个确认对话框
 * @param {Object} opts
 */
sohu.diyConsole.prototype.Confirm=function(opts){
	var _this=this;
	opts=$.extend({},{
		title:"确认操作?",
		ct:"",
		height:140,
		width:"",
		position:"center",
		resizable:false,
		modal:true,
		yes:null,
		no:null,
		close:null
	},opts);
	
	var dlOpt={
		title:opts.title,
		resizable:opts.resizable,
		height:opts.height,
		width:opts.width,
		modal:opts.modal,
		position:opts.position,
		buttons:{
			"取消":function(){
				if(opts.no){opts.no(this);};
				$(this).dialog("close");
			},
			"确认":function(){
				if(opts.yes){opts.yes(this);};
				$(this).dialog("close");
			}
		},
		close:function(evt,ui){
			_this.__p._$pageTip.removeClass("confirm");
			if(opts.close){
				opts.close(evt,ui);
			};
		}
	};
	this.__p._$pageTip.addClass("confirm").html(opts.ct).dialog(dlOpt);
};
/**
 * 移除.txtLoading
 */
sohu.diyConsole.toggleLoading=function(){
	$(".txtLoading").toggle();
};
/*静态方法、对象*/
sohu.diyConsole.Dragger={
	ing:false,
	obj:null,
	handle:$('<div class="dragHandle"></div>'),
	cssHandle:'.dragHandle'
};
sohu.diyConsole.CurArea=null;
sohu.diyConsole.CurSec=null;		/* 当前鼠标所在的分栏 */
sohu.diyConsole.EditingSec=null;	/* 当前内联编辑的分栏 */
sohu.diyConsole.CurCT=null;
sohu.diyConsole.EditingCT=null;
sohu.diyConsole.CurElm=null;/* current editing element */
sohu.diyConsole.$SecEditorModel=null; /* 分栏编辑器dom模型 */
sohu.diyConsole.InnerHeight=function() {
    var x,y;
    if (self.innerHeight) // all except Explorer
    {
		return self.innerHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight)         
    {
		// Explorer 6 Strict Mode
        return document.documentElement.clientHeight;
    }
    else if (document.body) // other Explorers
    {
        return document.body.clientHeight;
    };

};
//TODO:移到sohu.stringUtils.js中
/**
 * 获取指定长度的随机字符串。注意：仅仅由数字和字母组成
 * @param {Object} size 随机字符串的长度
 * @param {Boolean} plusTimeStamp 是否加上当前时间戳
 */
sohu.diyConsole.RdStr=function(size,plusTimeStamp){
	var size0=8;
	var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
	size=size||size0;size=size<1?size0:size;size=size>chars.length?size0:size;
	var s = '';
	for (var i=0; i<size; i++) {
		var rnum = Math.floor(Math.random() * chars.length);
		s += chars.substring(rnum,rnum+1);
	};
	if(plusTimeStamp){
		s+=new Date().getTime();
	};
	return s;
};
