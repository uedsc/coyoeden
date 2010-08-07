/**
 * Dialog manager
 * @author levinhuang
 * @dependency jqModal.js
 */
if(!sohu){var sohu={};};
sohu.diyDialog=function(){
	var p={},pub={};
	//private
	p.dialog=function(){
		var _this=this;
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
		this.$Body.empty().append($dom);
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
		p.dlg=new p.dialog();
	};
	/**
	 * Show a dialog.
	 * @param {Object} dom Dom selector or jquery object of the dialog 
	 * @param {Object} opts options
	 */
	pub.Show=function(dom,opts){
		if(!dom.jquery){dom=$(dom);};
		opts.title=opts.title||"提示信息";
		p.dlg.SetBody(dom).SetTitle(opts.title).Show(opts);
	};
	return pub;
}();
