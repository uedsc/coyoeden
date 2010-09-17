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
		this.$BtnNO=this.$Layout.find(".jqmClose");
		this.$Body=this.$Layout.find(p.opts.cssCT);
		this.$CTWrap=$(p.opts.cssCTWrap);/* Wrapper for all the dialog contents */
		this.jqmHash=null;
		//default options for jqModal
		this.jqmOpts={trigger:false,modal:true,overlay:50,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null,hideActions:false,txtBtnOK:'ȷ��',txtBtnNO:'ȡ��',btnNO:true};
		this.jqmOpts.onShow=function(hash){
			var doShow=true;
			if(_this.jqmOpts.beforeShow){
				doShow=_this.jqmOpts.beforeShow(hash,_this);
			};
			if(doShow){
				hash.w.show();
				_this.jqmHash=hash;
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
				hash.w.hide();				
				if(_this.jqmOpts.modal){hash.o.remove();}; 
				//reset the dialog content
				_this.$CTWrap.append(_this.$Body.children());	
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
		
		//Set buttons' texts
		this.$BtnOK.attr("title",this.jqmOpts.txtBtnOK).attr("style",'').find("span").html(this.jqmOpts.txtBtnOK);
		this.$BtnNO.attr("title",this.jqmOpts.txtBtnNO).attr("style",'').find("span").html(this.jqmOpts.txtBtnNO);
		
		//Hide the close button
		if(!this.jqmOpts.btnNO){
			this.$BtnNO.hide();
			this.$BtnOK.css("float","none");
		};
		
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
		$.extend(this.jqmOpts,{trigger:false,modal:true,overlay:50,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null,hideActions:false,txtBtnOK:'ȷ��',txtBtnNO:'ȡ��',btnNO:true});
	};
	//public 
	pub.Init=function(opts){
		p.opts=$.extend({cssLayout:"#jqmWin1",cssTitle:".title",cssCT:".ct",cssOK:".jqmOk",draggable:true,cssDragHandle:".hd",cssDragCTM:"#main",cssCTWrap:"#dialogList",cssActs:".acts"},opts);
		p.console=opts.console;/* diyConsole instance */
		p.dlg=new p.dialog();
		
		//ע�ᵯ��ʵ��
		if(opts.onInit){
			opts.onInit(p.dlg);
		};
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
	 * Hide the dialog
	 */
	pub.Hide=function(ignoreCbk){
		if (ignoreCbk) {
			//hide without executing callbacks
			p.dlg.$Layout.hide();
			p.dlg.$CTWrap.append(p.dlg.$Body.children());
			if(p.dlg.jqmHash){
				p.dlg.jqmHash.a = false;
			};	
		}
		else 
			p.dlg.Hide();
	};
	/**
	 * Register a dialog content object,for the sake of cache
	 * @param {Object} key key of the dialog
	 * @param {Object} dlg a dialog instance object  
	 */
	pub.Register=function(key,dlg){
		p[key]=dlg;
	};
	/**
	 * Update the jqm options of the specified dialog instance
	 * @param {Object} key
	 * @param {Object} jqmOpts
	 */
	pub.Update=function(key,jqmOpts){
		var dlg=p[key];
		if(!dlg) return;
		if(!dlg.jqmOpts0)
			dlg.jqmOpts0=dlg.$Layout.jqmOpts;
		
		if(jqmOpts){
			$.extend(dlg.$Layout.jqmOpts,jqmOpts);
		}else{
			dlg.$Layout.jqmOpts=dlg.jqmOpts0;
		};
	};
	/**
	 * Get a specified dialog instance object
	 * @param {Object} key
	 */
	pub.Get=function(key){
		var dlg=p[key];
		return dlg;
	};
	return pub;
}();
/* ��̬���� */
/**
 * ����ĳ������ı�
 * @param {Object} dlg
 */
sohu.diyDialog.resetForm=function(dlg,callback){
	dlg.$Layout.find("*").removeClass("alert").end().find(":text").val("");
	dlg.$Layout.find(".tip").hide();
	if(callback)
		callback(dlg);
};
/**
 * ����һ��ȷ�϶Ի���
 * @param {Object} opts
 */
sohu.diyDialog.doConfirm=function(opts){
	opts=$.extend({},{
		title:"��ʾ��Ϣ",
		text:"",
		modal:true,
		trigger:false,
		overlay:50,
		autoFocus:false,
		onOK:null,
		afterHide:null,
		beforeShow:null,
		type:"caution",
		btnNO:true
	},opts);
	
	var jqmOpts={trigger:opts.trigger,modal:opts.modal,overlay:opts.overlay,autoFocus:opts.autoFocus};
	jqmOpts.onShow=function(hash){
		var doShow=true;
		if(opts.beforeShow){
			doShow=opts.beforeShow(hash);
		};
		if(doShow){
			hash.w.show();
			
			sohu.diyDialog.$jqmConfirm.$txt.empty().append(opts.text);
			sohu.diyDialog.$jqmConfirm.$title.html(opts.title);
			sohu.diyDialog.$jqmConfirm.$icon.attr("class","").addClass("icon "+opts.type);
			sohu.diyDialog.$jqmConfirm.$btnOK.attr("style",'');
			sohu.diyDialog.$jqmConfirm.$btnNO.attr("style",'');
			if(!opts.btnNO){
				sohu.diyDialog.$jqmConfirm.$btnNO.hide();
				sohu.diyDialog.$jqmConfirm.$btnOK.css("float","none");
			};
			
			sohu.diyDialog.$jqmConfirm.css("margin-left",-(sohu.diyDialog.$jqmConfirm.width()/2));
			
			if(opts.afterShow){
				opts.afterShow(hash);
			};
		};
	};
	jqmOpts.onHide=function(hash){
		var doHide=true;
		if(opts.beforeHide){
			doHide=opts.beforeHide(hash);				
		};
		if(doHide){
			hash.w.hide();				
			if(opts.modal){hash.o.remove();}; 
			//afterHide callback
			if(opts.afterHide)
			{
				opts.afterHide(hash);
			};
		};	
	};
	
	
	if(!sohu.diyDialog.$jqmConfirm){
		sohu.diyDialog.$jqmConfirm=$("#jqmConfirm").jqm(jqmOpts).draggable({handle:".hd",containment:'window'});
		sohu.diyDialog.$jqmConfirm.$txt=sohu.diyDialog.$jqmConfirm.find(".txt");
		sohu.diyDialog.$jqmConfirm.$icon=sohu.diyDialog.$jqmConfirm.find(".icon");
		sohu.diyDialog.$jqmConfirm.$title=sohu.diyDialog.$jqmConfirm.find(".title");
		sohu.diyDialog.$jqmConfirm.$acts=sohu.diyDialog.$jqmConfirm.find(".acts");
		sohu.diyDialog.$jqmConfirm.$btnOK=sohu.diyDialog.$jqmConfirm.find(".jqmOk");
		sohu.diyDialog.$jqmConfirm.$btnNO=sohu.diyDialog.$jqmConfirm.find(".jqmClose");
	};
	
	sohu.diyDialog.$jqmConfirm.$btnOK.unbind("click");
	if(opts.onOK){
		sohu.diyDialog.$jqmConfirm.$btnOK.click(function(evt){
			opts.onOK(sohu.diyDialog.$jqmConfirm,opts);
			return false;
		});
	};
	
	sohu.diyDialog.$jqmConfirm.jqm(jqmOpts).jqmShow();
	
};
/**
 * ����һ����ʾ��.����windows.alert
 * @param {Object} opts
 */
sohu.diyDialog.doAlert=function(opts){
	opts=$.extend({},{
		btnNO:false,
		onOK:function($jqm){
			$jqm.jqmHide();
		},
	},opts);
	opts.btnNO=false;
	sohu.diyDialog.doConfirm(opts);
};
/**
 * ��ʾ��ɫ����
 * @param {Object} opts
 */
sohu.diyDialog.showColorPicker=function(opts){
	if(!sohu.diyDialog.$jqmCpk){
		sohu.diyDialog.$jqmCpk=$("#jqmCpk").jqm({
			title:"��ɫ",
			modal:true
		}).draggable({handle:".hd",containment:'window'});
				
		sohu.diyDialog.$jqmCpk.find(".cpk").icolor({
			flat:true,
			onSelect:function(hex){
				if(sohu.diyDialog.$jqmCpk._onSubmit){
					sohu.diyDialog.$jqmCpk._onSubmit(hex);
				};
				sohu.diyDialog.$jqmCpk.jqmHide();
			}
		});
		
		//margin-left
		var ml=-(sohu.diyDialog.$jqmCpk.width()/2);
		sohu.diyDialog.$jqmCpk.css("margin-left",ml);
	};
	//���漰���»ص�
	sohu.diyDialog.$jqmCpk._onSubmit=opts.onSubmit;
	sohu.diyDialog.$jqmCpk._onChange=opts.onChange;
	sohu.diyDialog.$jqmCpk.jqmShow();
};
/* /��̬���� */