/**
 * Dialog manager
 * @author levinhuang
 * @dependency jqModal.js
 */
if(!sohu){var sohu={};};
/**
 * ���ӻ�ר�ⵯ��ؼ�
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
		
		//ע�ᵯ��ʵ��
		sohu.diyDialog.Register("addBlock",new sohu.diyDialog.Area(p.dlg));
		sohu.diyDialog.Register("areaTools",new sohu.diyDialog.AreaTool(p.dlg));
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
		opts.title=opts.title||"��ʾ��Ϣ";
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
 * ��Ӻ��е���
 */
sohu.diyDialog.Area=function(dlg){
	var p={};
	//˽�к���
	p.afterShow=function(hash,dlg0){
		p._curTpl=null;
	};
	//����ɾ��ʱ�Ļص�����
	p.onAreaRemove=function(area){
		dlg.Console.ActiveArea(null);
		dlg.Console.$Layout.animate({top:dlg.Console.__p.opts.dfTop});	
	};
	//DOM����
	p.$layout=$("#addBlock");
	p.$layout.jqmOpts={title:"��Ӻ���",afterShow:p.afterShow,hideActions:true};
	//�¼�����
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
	//�¼�ע��
	p.initEvts=function(){
		p.$layout.find("a").click(p.selTpl);
	}();
	
	
	this.$Layout=p.$layout;
};
/**
 * �������õ���
 * @param {Object} dlg
 */
sohu.diyDialog.CfgArea=function(dlg){
	var p={},pub={};
	//DOM����
	
	return pub;
};
/**
 * ���й�����
 * @param {Object} dlg
 */
sohu.diyDialog.AreaTool=function(dlg){
	var p={};
	//DOM����
	p.$layout=$("#areaTools");
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnPageBG=$("#lnkPageBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	
	//˽�к���
	p.rePosition=function(){
		if(!dlg.Console.CurArea){p.$layout.attr("style","");return;};
		p.$layout.css("top",dlg.Console.CurArea.$Layout.offset().top);
	};
	
	//�¼�����
	p.onAdd=function(evt){
		sohu.diyDialog.Show("addBlock");
	};
	p.onDel=function(evt){
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		dlg.Console.Areas=$.grep(dlg.Console.Areas,function(o,i){
			if(o.ID==dlg.Console.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
	};
	p.onCfgBG=function(evt){
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		sohu.diyDialog.Show("cfgAreaBG");
	};
	p.onCfgPageBG=function(evt){
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		sohu.diyDialog.Show("cfgPageBG");
	};
	p.onMove=function(evt){
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		p.rePosition();
	};
	//�¼�ע��
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
