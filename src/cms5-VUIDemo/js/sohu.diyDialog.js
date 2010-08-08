/**
 * Dialog manager
 * @author levinhuang
 * @dependency jqModal.js
 */
if(!sohu){var sohu={};};
/**
 * 可视化专题弹框控件
 */
sohu.diyDialog=function(){
	var p={},pub={};
	//private
	p.dialog=function(){
		var _this=this;
		this.Console=p.console;
		this.$Layout=$(p.opts.cssLayout);
		this.$Title=this.$Layout.find(p.opts.cssTitle);
		this.$Acts=this.$Layout.find(p.opts.cssActs);
		this.$BtnOK=this.$Layout.find(p.opts.cssOK);
		this.$Body=this.$Layout.find(p.opts.cssCT);
		this.$CTWrap=$(p.opts.cssCTWrap);/* Wrapper for all the dialog contents */
		
		//default options for jqModal
		this.jqmOpts={trigger:false,modal:true,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null,hideActions:false};
		this.jqmOpts.onShow=function(hash){
			var doShow=true;
			if(_this.jqmOpts.beforeShow){
				doShow=_this.jqmOpts.beforeShow(hash,_this);
			};
			if(doShow){
				hash.w.show();
				if(_this.jqmOpts.afterShow){
					_this.jqmOpts.afterShow(hash,_this);
				};
			};
				
		};
		this.jqmOpts.onHide=function(hash){
			var doHide=true;
			if(_this.jqmOpts.beforeHide){
				doHide=_this.jqmOpts.beforeHide(hash,_this);				
			};
			if(doHide){
				hash.w.fadeOut('500',function(){ 
					hash.o.remove(); 
					//reset the dialog content
					_this.$CTWrap.append(_this.$Body.children());
				}); 
				//afterHide callback
				if(_this.jqmOpts.afterHide)
				{
					_this.jqmOpts.afterHide(hash,_this);
				};
			};	
		};
		//draggable?
		if(p.opts.draggable)
			this.$Layout.draggable({handle:p.opts.cssDragHandle,containment:p.opts.cssDragCTM});
	};
	/**
	 * Update the dialog's content
	 * @param {Object} $dom
	 */
	p.dialog.prototype.SetBody=function($dom){
		this.$Body.append($dom);
		//adjust margin-left
		this.$Layout.css("margin-left",-(this.$Layout.width()/2));
		return this;
	};
	p.dialog.prototype.SetTitle=function(txt){
		this.$Title.html(txt);
		return this;
	};
	/**
	 * Show the dialog
	 * @param {Object} opts
	 */
	p.dialog.prototype.Show=function(opts){
		var _this=this;
		this.ResetOptions();
		$.extend(this.jqmOpts,opts);
		
		if(this.jqmOpts.hideActions){
			this.$Acts.hide();
		}else{
			this.$Acts.show();
		};
		
		this.$Layout.jqm(this.jqmOpts).jqmShow();
		//button events
		if(this.jqmOpts.onOK){
			this.$BtnOK.unbind().bind("click",function(evt){
				_this.jqmOpts.onOK(_this);
				return false;
			});
		};
	};
	/**
	 * Hide the dialog
	 */
	p.dialog.prototype.Hide=function(){
		this.$Layout.jqmHide();
	};
	/**
	 * Load default options for the jqModal
	 */
	p.dialog.prototype.ResetOptions=function(){
		//default options for jqModal
		$.extend(this.jqmOpts,{trigger:false,modal:true,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null,hideActions:false});
	};
	//public 
	pub.Init=function(opts){
		p.opts=$.extend({cssLayout:".jqmWindow",cssTitle:".title",cssCT:".ct",cssOK:".jqmOk",draggable:true,cssDragHandle:".hd",cssDragCTM:"#main",cssCTWrap:"#dialogList",cssActs:".acts"},opts);
		p.console=opts.console;/* diyConsole instance */
		p.dlg=new p.dialog();
		
		//注册弹框实体
		sohu.diyDialog.Register("addBlock",new sohu.diyDialog.Area(p.dlg));
		sohu.diyDialog.Register("areaTools",new sohu.diyDialog.AreaTool(p.dlg));
		sohu.diyDialog.Register("cfgArea",new sohu.diyDialog.CfgArea(p.dlg));
		sohu.diyDialog.Register("cfgPage",new sohu.diyDialog.CfgPage(p.dlg));
		sohu.diyDialog.Register("addContent",new sohu.diyDialog.AddContent(p.dlg));
	};
	/**
	 * Show a dialog.
	 * @param {Object} dom Dom selector or jquery object of the dialog 
	 * @param {Object} opts options
	 */
	pub.Show=function(dom,opts){
		//fetch from cache firstly
		var $dom=null;
		if(typeof(dom)=="string"){
			var dlg=p[dom];
			if (dlg) {
				$dom=dlg.$Layout;
				opts = $dom.jqmOpts;
			};
		};
		//no cache,fetch from dom
		if(!$dom){
			if(!dom.jquery){
				$dom=$(dom);
			}else{
				$dom=dom;
			};
		};
		opts=opts||{};
		opts.title=opts.title||"提示信息";
		p.dlg.SetBody($dom).SetTitle(opts.title).Show(opts);
	};
	/**
	 * Register a dialog content object,for the sake of cache
	 * @param {Object} key key of the dialog
	 * @param {Object} $dom a jquery object  
	 */
	pub.Register=function(key,$dom){
		p[key]=$dom;
	};
	return pub;
}();

/**
 * 添加横切弹框
 */
sohu.diyDialog.Area=function(dlg){
	var p={};
	//私有函数
	p.afterShow=function(hash,dlg0){
		p._curTpl=null;
	};
	//横切删除时的回调函数
	p.onAreaRemove=function(area){
		dlg.Console.ActiveArea(null);
		dlg.Console.$Layout.animate({top:dlg.Console.__p.opts.dfTop});	
	};
	//DOM引用
	p.$layout=$("#addBlock");
	p.$layout.jqmOpts={title:"添加横切",afterShow:p.afterShow,hideActions:true};
	//事件处理
	p.selTpl=function(evt){
		p._curTpl=this.id;
		var obj=new sohu.diyArea({
				tplID:p._curTpl,
				console:dlg.Console,
				onRemove:p.onAreaRemove
		});
		dlg.Console.Areas.push(obj);
		dlg.Hide();	
		return false;
	};
	//事件注册
	p.initEvts=function(){
		p.$layout.find("a").click(p.selTpl);
	}();
	
	
	this.$Layout=p.$layout;
};
/**
 * 横切设置弹框
 * @param {Object} dlg
 */
sohu.diyDialog.CfgArea=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#cfgArea");
	
	//私有变量÷函数
	p._fm={
		txtBG:$("#txtAreaBG"),
		txtID:$("#txtAreaID"),
		rbtnBGAlign:this.$Layout.find("input[name='areaBGAlign']"),
		rbtnBGRepeat:this.$Layout.find("input[name='areaBGRepeat']"),
		tipBG:this.$Layout.find(".tipAreaBG"),
		tipID:this.$Layout.find("tipAreaID"),
		reset:function(){_this.$Layout.find("*").removeClass("alert").end().find(":text").val("");}
	};
	//事件处理
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//bg image
		var url=p._fm.txtBG.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBG.addClass("alert").select();
			p._fm.tipBG.addClass("alert");
			return false;
		};
		if(url==""){
			//sohu.diyConsole.CurArea.$Layout.css("background-image","none");
		}else{
			sohu.diyConsole.CurArea.$Layout.css("background-image","url('"+url+"')");
			//bg position
			var al=p._fm.rbtnBGAlign.curVal;
			al=al=="center"?al:al+" top";
			sohu.diyConsole.CurArea.$Layout.css("background-position",al);
			//bg repeat
			var rp=p._fm.rbtnBGRepeat.curVal;
			sohu.diyConsole.CurArea.$Layout.css("background-repeat",rp);
		};
		//area id
		var id=p._fm.txtID.val();
		if(!sohu.diyConsole.IsValidID(id)){
			p._fm.txtID.addClass("alert");
			p._fm.tipID.addClass("alert");
			return false;
		};
		var isIDOK=sohu.diyConsole.CurArea.UpdateID(id);
		
		if(cls&&isIDOK)
			dlg0.Hide();
	};
	p.afterShow=function(hash,dlg0){
		//bg img
		var img=sohu.diyConsole.CurArea.$Layout.css("background-image");
		img=img.replace('url("',"").replace('")',"");
		img=img=="none"?"":img;
		p._fm.txtBG.val(img).select();
		//area id
		p._fm.txtID.val(sohu.diyConsole.CurArea.ID);
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//事件注册
	p._fm.rbtnBGAlign.click(function(evt){
		p._fm.rbtnBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.rbtnBGRepeat.click(function(evt){
		p._fm.rbtnBGRepeat.curVal=this.value;
		p.preview();
	});		
	p._fm.rbtnBGAlign.curVal="center";
	p._fm.rbtnBGRepeat.curVal="no-repeat";
	//事件注册
	
	this.$Layout.jqmOpts={
		title:"横切设置",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * 页面设置弹框
 * @param {Object} dlg
 */
sohu.diyDialog.CfgPage=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#cfgPage");
	
	//私有变量÷函数
	p._fm={
		txtBG:$("#txtPageBG"),
		txtBGH:$("#txtPageBGH"),
		rbtnBGAlign:this.$Layout.find("input[name='pageBGAlign']"),
		rbtnBGRepeat:this.$Layout.find("input[name='pageBGRepeat']"),
		tipBG:this.$Layout.find(".tipPageBG"),
		tipBGH:this.$Layout.find(".tipPageBGH"),
		txtBGC:$("#txtPageBGC"),
		cpk:this.$Layout.find(".cpk"),
		reset:function(){_this.$Layout.find("*").removeClass("alert").end().find(":text").val("");}
	};
	//事件处理
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		var url=p._fm.txtBG.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBG.addClass("alert").select();
			p._fm.tipBG.addClass("alert");
			return false;
		};
		var h=p._fm.txtBGH.val();
		if((!StringUtils.isPlusInt(h))||(h=parseInt(h))<1){
			p._fm.txtBGH.addClass("alert").select();
			p._fm.tipBGH.addClass("alert");
			return false;
		};
		
		if(url==""){
			sohu.diyConsole.$BodyBGA.css("background-image","none");
		}else{
			sohu.diyConsole.$BodyBGA.css("background-image","url('"+url+"')");
		};
		var al=p._fm.rbtnBGAlign.curVal;
		sohu.diyConsole.$BodyBGA.css("background-position",al);
		
		var rp=p._fm.rbtnBGRepeat.curVal;
		sohu.diyConsole.$BodyBGA.css("background-repeat",rp);
		
		sohu.diyConsole.$BodyBGA.css("height",h);
		
		if(cls)
			dlg0.Hide();
	};
	p.afterShow=function(hash,dlg0){
		//背景图
		var img=sohu.diyConsole.$BodyBGA.css("background-image");
		img=img=="none"?"":img;
		img=img.replace('url("',"").replace('")',"");
		p._fm.txtBG.val(img).select();
		//高度
		var h=sohu.diyConsole.$BodyBGA.css("height");
		h=parseInt(h);
		p._fm.txtBGH.val(h);
		//对齐方式
		var bg_p=sohu.diyConsole.$BodyBGA.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center center":bg_p;
		var bg_a=sohu.diyConsole.$BodyBGA.css("backgroundRepeat");
		p._fm.rbtnBGAlign.filter("[value='"+bg_p+"']").trigger("click");
		//平铺方式
		p._fm.rbtnBGRepeat.filter("[value='"+bg_a+"']").trigger("click");
		//背景色
		p._fm.txtBGC.css("backgroundColor",$("body").css("backgroundColor"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//事件注册
	p._fm.cpk.ColorPicker({
		flat:true,
		color:"#ffffff",
		onChange:function(hsb,hex,rgb){
			var c="#"+hex;
			p._fm.txtBGC.css("backgroundColor",c).attr("title",c);
			$("body").css("backgroundColor",c);
		},
		onSubmit:function(hsb,hex,rgb){
			$("body").css("backgroundColor","#"+hex);
			p._fm.cpk.hide();
		}
	});
	p._fm.rbtnBGAlign.click(function(evt){
		p._fm.rbtnBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.rbtnBGRepeat.click(function(evt){
		p._fm.rbtnBGRepeat.curVal=this.value;
		p.preview();
	});		
	p._fm.rbtnBGAlign.curVal="center center";
	p._fm.rbtnBGRepeat.curVal="no-repeat";
	p._fm.txtBGH.change(function(evt){
		p.preview();
	});
	p._fm.txtBG.change(function(evt){p.preview();});
	
	//背景色
	p._fm.txtBGC
	.css("backgroundColor","transparent")
	.click(function(evt){
		p._fm.cpk.show();
	});
	//事件注册
	
	this.$Layout.jqmOpts={
		title:"页面设置",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * 添加内容弹框
 * @param {Object} dlg
 */
sohu.diyDialog.AddContent=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#addContent");
	//事件处理
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		sohu.diyConsole.toggleLoading();
		return true;
	};
	p.afterHide=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("off");
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"添加内容",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide
	};
};
/**
 * 横切工具条
 * @param {Object} dlg
 */
sohu.diyDialog.AreaTool=function(dlg){
	var p={};
	//DOM引用
	p.$layout=$("#areaTools");
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnPageBG=$("#lnkPageBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	
	//私有函数
	p.rePosition=function(){
		if(!dlg.Console.CurArea){p.$layout.attr("style","");return;};
		p.$layout.css("top",dlg.Console.CurArea.$Layout.offset().top);
	};
	
	//事件处理
	p.onAdd=function(evt){
		sohu.diyDialog.Show("addBlock");
	};
	p.onDel=function(evt){
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		dlg.Console.Areas=$.grep(dlg.Console.Areas,function(o,i){
			if(o.ID==dlg.Console.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
	};
	p.onCfgBG=function(evt){
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		sohu.diyDialog.Show("cfgArea");
	};
	p.onCfgPageBG=function(evt){
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		sohu.diyDialog.Show("cfgPage");
	};
	p.onMove=function(evt){
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		p.rePosition();
	};
	//事件注册
	p.initEvts=function(){
		p.$layout.find("a").click(function(evt){return false;});
		p._$btnAdd.click(p.onAdd);
		p._$btnDel.click(p.onDel);
		p._$btnBG.click(p.onCfgBG);
		p._$btnPageBG.click(p.onCfgPageBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
	}();
	
	this.$Layout=p.$layout;
	this.RePosition=p.rePosition;	
};
