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
		new sohu.diyDialog.wSetting1(p.dlg);
		sohu.diyDialog.Register("addBlock",new sohu.diyDialog.wArea(p.dlg));
		sohu.diyDialog.Register("areaTools",new sohu.diyDialog.wAreaTool(p.dlg));
		sohu.diyDialog.Register("cfgArea",new sohu.diyDialog.wCfgArea(p.dlg));
		sohu.diyDialog.Register("cfgPage",new sohu.diyDialog.wCfgPage(p.dlg));
		sohu.diyDialog.Register("addContent",new sohu.diyDialog.wAddContent(p.dlg));
		sohu.diyDialog.Register("code",new sohu.diyDialog.wCode(p.dlg));
		sohu.diyDialog.Register("cfgSec",new sohu.diyDialog.wCfgSec(p.dlg));
		sohu.diyDialog.Register("subSec390",new sohu.diyDialog.wSubSec(p.dlg,390));
		sohu.diyDialog.Register("subSec430",new sohu.diyDialog.wSubSec(p.dlg,430));
		sohu.diyDialog.Register("subSec470",new sohu.diyDialog.wSubSec(p.dlg,470));
		sohu.diyDialog.Register("subSec510",new sohu.diyDialog.wSubSec(p.dlg,510));
		sohu.diyDialog.Register("subSec670",new sohu.diyDialog.wSubSec(p.dlg,670));
		sohu.diyDialog.Register("subSec950",new sohu.diyDialog.wSubSec(p.dlg,950));
		sohu.diyDialog.Register("wText",new sohu.diyDialog.wText(p.dlg));
		sohu.diyDialog.Register("wImage",new sohu.diyDialog.wImage(p.dlg));
		sohu.diyDialog.Register("wSecHead",new sohu.diyDialog.wSecHead(p.dlg));	
		sohu.diyDialog.Register("wPagePro",new sohu.diyDialog.wPagePro(p.dlg));
		sohu.diyDialog.Register("wTheme",new sohu.diyDialog.wTheme(p.dlg));
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
		
		sohu.diyDialog.$jqmCpk.find(".cpk").ColorPicker({
			flat:true,
			color:"#000000",
			onSubmit:function(hsb,hex,rgb){
				if(opts.onSubmit){
					opts.onSubmit("#"+hex);
				};
				sohu.diyDialog.$jqmCpk.jqmHide();
			},
			onChange:function(hsb,hex,rgb){
				if(opts.onChange){
					opts.onChange("#"+hex);
				};
			}
		});
		//margin-left
		var ml=-(sohu.diyDialog.$jqmCpk.width()/2);
		sohu.diyDialog.$jqmCpk.css("margin-left",ml);
	};
	
	sohu.diyDialog.$jqmCpk.jqmShow();
};
/* /��̬���� */


/* ����Ϊ����������� */

/**
 * ��Ӻ��е���
 */
sohu.diyDialog.wArea=function(dlg){
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
sohu.diyDialog.wCfgArea=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#cfgArea");
	
	//˽�б����º���
	p._fm={
		txtBG:$("#txtAreaBG"),
		txtID:$("#txtAreaID"),
		ddlBGAlign:$("#ddlAreaBGAlign"),
		ddlBGRepeat:$("#ddlAreaBGRepeat"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//bg image
		var url=p._fm.txtBG.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBG.addClass("alert").select();
			return false;
		};
		if(url==""){
			sohu.diyConsole.CurArea.$Layout.css("background-image","");//shouldn't use 'none' here
		}else{
			sohu.diyConsole.CurArea.$Layout.css("background-image","url('"+url+"')");
			//bg position
			var al=p._fm.ddlBGAlign.curVal;
			al=al=="center"?al:al+" top";
			sohu.diyConsole.CurArea.$Layout.css("background-position",al);
			//bg repeat
			var rp=p._fm.ddlBGRepeat.curVal;
			sohu.diyConsole.CurArea.$Layout.css("background-repeat",rp);
		};
		//area id
		var id=p._fm.txtID.val();
		if(!sohu.diyConsole.IsValidID(id)){
			p._fm.txtID.addClass("alert");
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
		//����
		p._fm.ddlBGAlign.val(p._fm.ddlBGAlign.curVal);
		//ƽ��
		p._fm.ddlBGRepeat.val(p._fm.ddlBGRepeat.curVal);
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//�¼�ע��
	p._fm.txtBG.change(function(evt){
		p.preview();
	});
	p._fm.ddlBGAlign.change(function(evt){
		p._fm.ddlBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGRepeat.change(function(evt){
		p._fm.ddlBGRepeat.curVal=this.value;
		p.preview();
	});		
	p._fm.ddlBGAlign.curVal="center";
	p._fm.ddlBGRepeat.curVal="no-repeat";
	//�¼�ע��
	
	this.$Layout.jqmOpts={
		title:"��������",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ҳ�����õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wCfgPage=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#cfgPage");
	
	//˽�б����º���
	p._fm={
		txtBGA:$("#txtPageBGA"),
		txtBGB:$("#txtPageBGB"),
		txtBGHA:$("#txtPageBGHA"),
		txtBGHB:$("#txtPageBGHB"),
		ddlBGAlignA:$("#pageBGAlignA"),
		ddlBGAlignB:$("#pageBGAlignB"),
		ddlBGRepeatA:$("#pageBGRepeatA"),
		ddlBGRepeatB:$("#pageBGRepeatB"),
		txtBGC:$("#txtPageBGC"),
		cpk:this.$Layout.find(".cpk"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//ҳͷ����ͼ
		var url=p._fm.txtBGA.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBGA.addClass("alert").select();
			return false;
		};
		//ҳβ����ͼ
		var url1=p._fm.txtBGB.val();
		if((url1!="")&&(!StringUtils.isUrl(url1))){
			p._fm.txtBGB.addClass("alert").select();
			return false;
		};
		//����ҳͷ����
		var h=p._fm.txtBGHA.val();
		if(!StringUtils.isPlusInt(h)){
			h=0;
		};
		sohu.diyConsole.$BodyBGA.css("height",h+'px');
		if(url==""){
			sohu.diyConsole.$BodyBGA.css("background-image","none");
		}else{
			sohu.diyConsole.$BodyBGA.css("background-image","url('"+url+"')");
		};
		sohu.diyConsole.$BodyBGA.css("background-position",p._fm.ddlBGAlignA.curVal);
		sohu.diyConsole.$BodyBGA.css("background-repeat",p._fm.ddlBGRepeatA.curVal);
		
		//����ҳβ����
		h=p._fm.txtBGHB.val();
		if(!StringUtils.isPlusInt(h)){
			h=0;
		};
		sohu.diyConsole.$BodyBGB.css("height",h+'px');
		if(url1==""){
			sohu.diyConsole.$BodyBGB.css("background-image","none");
		}else{
			sohu.diyConsole.$BodyBGB.css("background-image","url('"+url1+"')");
		};
		sohu.diyConsole.$BodyBGB.css("background-position",p._fm.ddlBGAlignB.curVal);
		sohu.diyConsole.$BodyBGB.css("background-repeat",p._fm.ddlBGRepeatB.curVal);
		
		if(cls)
			dlg0.Hide();
	};
	p.afterShow=function(hash,dlg0){
		//����ͼ
		var img=sohu.diyConsole.ParseBGImg(sohu.diyConsole.$BodyBGA.css("background-image"));
		p._fm.txtBGA.val(img).select();
		img=sohu.diyConsole.ParseBGImg(sohu.diyConsole.$BodyBGB.css("background-image"));
		p._fm.txtBGB.val(img);
		//�߶�
		var h=parseInt(sohu.diyConsole.$BodyBGA.css("height"));
		h=isNaN(h)?0:h;
		p._fm.txtBGHA.val(h);
		h=parseInt(sohu.diyConsole.$BodyBGB.css("height"));
		h=isNaN(h)?0:h;
		p._fm.txtBGHB.val(h);
		//ҳͷ
		//���뷽ʽ
		var bg_p=sohu.diyConsole.$BodyBGA.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center center":bg_p;
		var bg_a=sohu.diyConsole.$BodyBGA.css("backgroundRepeat");
		p._fm.ddlBGAlignA.val(bg_p);
		//ƽ�̷�ʽ
		p._fm.ddlBGRepeatA.val(bg_a);
		p._fm.ddlBGAlignA.curVal=bg_p;
		p._fm.ddlBGRepeatA.curVal=bg_a;
		//ҳβ
		bg_p=sohu.diyConsole.$BodyBGB.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center center":bg_p;
		bg_a=sohu.diyConsole.$BodyBGB.css("backgroundRepeat");
		p._fm.ddlBGAlignB.val(bg_p);
		//ƽ�̷�ʽ
		p._fm.ddlBGRepeatB.val(bg_a);
		p._fm.ddlBGAlignB.curVal=bg_p;
		p._fm.ddlBGRepeatB.curVal=bg_a;
		//����ɫ
		p._fm.txtBGC.css("backgroundColor",sohu.diyConsole.$Body.css("backgroundColor"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//�¼�ע��
	p._fm.cpk.ColorPicker({
		flat:true,
		color:"#ffffff",
		onChange:function(hsb,hex,rgb){
			var c="#"+hex;
			p._fm.txtBGC.css("backgroundColor",c).attr("title",c);
			sohu.diyConsole.$Body.css("backgroundColor",c);
		},
		onSubmit:function(hsb,hex,rgb){
			sohu.diyConsole.$Body.css("backgroundColor","#"+hex);
			p._fm.cpk.hide();
		}
	});
	p._fm.ddlBGAlignA.click(function(evt){
		p._fm.ddlBGAlignA.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignB.click(function(evt){
		p._fm.ddlBGAlignB.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGRepeatA.click(function(evt){
		p._fm.ddlBGRepeatA.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGRepeatB.click(function(evt){
		p._fm.ddlBGRepeatB.curVal=this.value;
		p.preview();
	});		

	p._fm.txtBGHA.change(p.preview);
	p._fm.txtBGHB.change(p.preview);
	p._fm.txtBGA.change(p.preview);
	p._fm.txtBGB.change(p.preview);
	
	//����ɫ
	p._fm.txtBGC
	.css("backgroundColor","transparent")
	.click(function(evt){
		p._fm.cpk.show();
	});
	//�¼�ע��
	
	this.$Layout.jqmOpts={
		title:"ҳ������",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ������ݵ���
 * @param {Object} dlg
 */
sohu.diyDialog.wAddContent=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#addContent");
	this.$if=$("#ifContentList");
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		sohu.diyConsole.toggleLoading();
		_this.$if.attr("src",_this.$if.attr("rel")+"?t="+new Date().getTime());
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off");
		//sohu.diyConsole.CurSec.Deactive();
		sohu.diyConsole.Preview();
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"�������",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		hideActions:true
	};
};
/**
 * �������뵯��
 * @param {Object} dlg
 */
sohu.diyDialog.wCode=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#wCode");
	this.$TextArea=this.$Layout.find("textarea");
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off");
		sohu.diyConsole.Preview();
	};	
	p.afterShow=function(hash,dlg0){
		_this.$TextArea.val(sohu.diyConsole.CurSec.$Layout.html());
	};
	p.onOK=function(dlg0){
		sohu.diyDialog.doConfirm({
			text:'ȷ�����µ�ǰ������HTML����ô?',
			onOK:function($jqm){
				sohu.diyConsole.CurSec.$Layout.html(_this.$TextArea.val());
				//���¼��ظ÷��������ݣ�����.html(x)��������ʱ��dom�Ѿ�����ԭ����dom
				sohu.diyConsole.CurSec.LoadContents();
				$jqm.jqmHide();
				dlg0.Hide();
			}
		});
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"����༭",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * �������õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wCfgSec=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#wCfgSec");
	//˽�к��������
	p._fm={
		txtBG:$("#txtSecBG"),
		txtBGC:$("#txtSecBGC"),
		txtBorderC:$("#txtSecBorderColor"),
		cpk:this.$Layout.find(".cpk"),
		rbtnBGAlign:this.$Layout.find("input[name='secBGAlign']"),
		rbtnBGRepeat:this.$Layout.find("input[name='secBGRepeat']"),
		cbxBorder:this.$Layout.find("input[name='SecBorderDir']"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		var url=$.trim(p._fm.txtBG.val());
		if( (url!="") && (!StringUtils.isUrl(url)) ){
			p._fm.txtBG.addClass("alert").select();
			return false;
		};
		//����ͼƬ����
		if (url != "") {
			sohu.diyConsole.CurSec.$Layout.css("background-image", "url('" + url + "')");
		}else{
			sohu.diyConsole.CurSec.$Layout.css("background-image", "none");
		};
		//����Ի���
		dlg0.Hide();
		//���ñ�
		p._fm.reset();			
	};
	p.afterShow=function(hash,dlg0){
		//����ͼ
		var bgimg=sohu.diyConsole.CurSec.$Layout.css("background-image");
		bgimg=bgimg=="none"?"":bgimg;
		bgimg=bgimg.replace('url("',"").replace('")',"");
		p._fm.txtBG.val(bgimg);
		//����ɫ
		p._fm.txtBGC.val("").css("backgroundColor",sohu.diyConsole.CurSec.$Layout.css("backgroundColor"));
		//�߿�ɫ
		var bdc=sohu.diyConsole.CurSec.$Layout.css("borderColor");
		p._fm.txtBorderC.css("backgroundColor",bdc);
		//���뷽ʽ
		var bg_p=sohu.diyConsole.CurSec.$Layout.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center center":bg_p;
		var bg_a=sohu.diyConsole.CurSec.$Layout.css("backgroundRepeat");
		p._fm.rbtnBGAlign.filter("[value='"+bg_p+"']").trigger("click");
		//ƽ�̷�ʽ
		p._fm.rbtnBGRepeat.filter("[value='"+bg_a+"']").trigger("click");
		//�߿�-���ݱ߿�ɫ�жϱ߿������
		bdc=sohu.diyConsole.GetBorderColor(bdc);
		if(!bdc){
			/*
			p._fm.cbxBorder.each(function(i,o){
				o.checked=false;
			});
			*/
			p._fm.cbxBorder.attr("checked",false);
		}else{
			for(var x in bdc){
				if(x)
					p._fm.cbxBorder.filter("[value='"+x+"']")[0].checked=(bdc[x]!="transparent"&&bdc[x]!="none");
			};//for
		};//if			
	};
	//�¼�ע��
	//����ɫ
	p._fm.txtBGC
	.css("backgroundColor","transparent")
	.click(function(evt){
		p._fm.cpk.$t=p._fm.txtBGC;
		p._fm.cpk.flag="bg";
		p._fm.cpk.show();
	});
	//�߿�ɫ
	p._fm.txtBorderC
	.css("borderColor","#eeeeee")
	.click(function(evt){
		p._fm.cpk.$t=p._fm.txtBorderC;
		p._fm.cpk.flag="bdc";
		p._fm.cpk.show();
	});
	//�߿���-�Զ����¼��ľ���֮��-�ο���722��762
	p._fm.cbxBorder.bind("evtClick",function(evt){
		if(this.checked){
			sohu.diyConsole.CurSec.$Layout.css("border-"+this.value,"1px solid "+p._fm.txtBorderC.attr("title"));
		}else{
			sohu.diyConsole.CurSec.$Layout.css("border-"+this.value,"none");
		};	
	});
	p._fm.cbxBorder.click(function(evt){
		$(this).trigger("evtClick");
	});		
	//����ͼ
	p._fm.txtBG.change(function(evt){
		var url=$.trim(p._fm.txtBG.val());
		if((url!="")&&!StringUtils.isUrl(url)){
			p._fm.txtBG.addClass("alert").select();
			p._fm.tipBG.addClass("alert");
			return false;
		};
		p._fm.txtBG.removeClass("alert");
		p._fm.tipBG.removeClass("alert");
		//����ͼƬ����
		if(url!=""){
			sohu.diyConsole.CurSec.$Layout.css("background-image","url('"+url+"')");
		}else{
			sohu.diyConsole.CurSec.$Layout.css("background-image","none");
		};
		
	});
	//����ͼ���뷽ʽ
	p._fm.rbtnBGAlign.click(function(evt){
		p._fm.rbtnBGAlign.curVal=this.value;
		sohu.diyConsole.CurSec.$Layout.css("background-position",this.value);
	});
	//����ͼƽ�̷�ʽ
	p._fm.rbtnBGRepeat.click(function(evt){
		p._fm.rbtnBGRepeat.curVal=this.value;
		sohu.diyConsole.CurSec.$Layout.css("background-repeat",this.value);
	});
	//��Ŀ����ɫ
	p._fm.cpk.ColorPicker({
		flat:true,
		color:"#eeeeee",
		onChange:function(hsb,hex,rgb){
			var c="#"+hex;
			p._fm.cpk.$t.css("backgroundColor",c).attr("title",c);
			if(p._fm.cpk.flag=="bg"){
				sohu.diyConsole.CurSec.$Layout.css("backgroundColor",c);
			}else{
				p._fm.cbxBorder.trigger("evtClick");
			};
		},
		onSubmit:function(hsb,hex,rgb){
			this.onChange(hsb,hex,rgb);
			p._fm.cpk.hide();
		}
	});			
	p._fm.rbtnBGAlign.curVal="center";
	p._fm.rbtnBGRepeat.curVal="repeat";	
	//jqm options
	this.$Layout.jqmOpts={
		title:"��������",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * ��Ŀͷ���õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wSecHead=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#wSecHead");
	p._fm={
		bg:$("#secHDBG"),
		cbxMore:$("#cbxSecHDMore"),
		link:$("#secHDLink"),
		ddlTarget:$("#ddlSecHDTarget"),
		title:$("#secHDTitle"),
		tipLink:this.$Layout.find(".tipSecHDLink"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onIframeSelect=function(evt){
		sohu.diyConsole.SnapSelection();
		var a=sohu.diyConsole.DocSelection.getA();
		if(a.isNull){
			p._fm.isNew=true;
			p.a=null;
			p._fm.title.val(sohu.diyConsole.DocSelection.text);
			return;
		};
		a.href=a.href=="#"?"":a.href;
		p._fm.title.val(a.title);
		p._fm.link.val(a.href);
		p._fm.ddlTarget.val(a.target);
		p._fm.isNew=false;
		p._fm.a=a;	
	};	
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");		
		return true;
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();		
	};
	p.afterShow=function(hash,dlg0){
		//����ɫ
		var bg=sohu.diyConsole.CurElm.CT.$Layout.css("background-image");
		bg=bg=="none"?"":bg;
		bg=bg.replace('url("',"").replace('")',"");
		p._fm.bg.val(bg);
		//�Ƿ���ʾ����
		if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length>0){
			p._fm.cbxMore.attr("checked",true);
		}else{
			p._fm.cbxMore.attr("checked",false);
		};
		//��������
		var $a=sohu.diyConsole.CurElm.CT.$Layout.find("a[href!='']");
		if($a.length>0){
			p._fm.link.val($a[0].href);
		};
		//��׽iframe�༭���û�ѡ��������
		sohu.diyConsole.CurElm.i$frame[0].i$Body().unbind("mouseup").mouseup(p.onIframeSelect);		
	};
	p.onOK=function(dlg0){
		var url=$.trim(p._fm.link.val());
		if((url!="")&&!StringUtils.isUrl(url)){
			p._fm.link.addClass("alert").select();
			p._fm.tipLink.addClass("alert").show();
			return false;
		};
		p._fm.link.removeClass("alert");
		p._fm.tipLink.removeClass("alert").hide();
		//����ͼ
		var img=$.trim(p._fm.bg.val());
		img=img==""?"none":img;
		sohu.diyConsole.CurElm.CT.$Layout.css("background-image",img);	
		
		//��ʾ����
		if(p._fm.cbxMore[0].checked){
			if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length==0){
				var $more=$('<strong class="more elm">����>></strong>');
				sohu.diyConsole.CurElm.CT.$Layout.append($more);
				new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$more});
			};
		}else{
			sohu.diyConsole.CurElm.CT.$Layout.find(".more").remove();
		};
			
		if(p._fm.isNew&&url!=""){
			sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
				sohu.diyConsole.DocSelection.selectAndRelease();
			});
		}else if(p._fm.a){
			p._fm.a.$obj.attr("title",p._fm.title.val())
				.attr("target",p._fm.ddlTarget.curVal);
			if(url!=""){p._fm.a.$obj.attr("href",url);};
		};

		dlg0.Hide();
	};	
	//�¼�ע��
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
	});
	//jqm options
	this.$Layout.jqmOpts={
		title:"��Ŀͷ",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK		
	};
};
/**
 * ��ӷ�������
 * @param {Object} dlg
 * @param {Object} parentSize �������Ŀ��
 */
sohu.diyDialog.wSubSec=function(dlg,parentSize){
	var p={};
	//DOM����
	this.$Layout=$("#subSec"+parentSize);
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
	};	
	//�¼�ע��
	this.$Layout.find('.item').click(function(evt){
		if(!sohu.diyConsole.CurSec) return;
		sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
		dlg.Hide();
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
	//jqm options
	this.$Layout.jqmOpts={
		title:"��ӷ���",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		hideActions:true
	};
};
/**
 * ���ֱ༭����
 * @param {Object} dlg
 */
sohu.diyDialog.wText=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#txtMsg");
	this.$ElmCAction=this.$Layout.find(".elmcActs");
	p._fm={
		txtATitle:$("#txtATitle"),
		txtAHref:$("#txtAHref"),
		ddlATarget:$("#ddlATarget"),
		tipAHref:this.$Layout.find(".tipAHref"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onIframeSelect=function(evt){
		sohu.diyConsole.SnapSelection();
		var a=sohu.diyConsole.DocSelection.getA();
		if(a.isNull){
			p._fm.isNew=true;
			p.a=null;
			p._fm.txtATitle.val(sohu.diyConsole.DocSelection.text);
			return;
		};
		a.href=a.href=="#"?"":a.href;
		p._fm.txtATitle.val(a.title);
		p._fm.txtAHref.val(a.href);
		p._fm.ddlATarget.val(a.target);
		p._fm.isNew=false;
		p._fm.a=a;	
	};
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");
		p._fm.txtATitle.val("");
		p._fm.txtAHref.val("");
		return true;
	};	
	p.afterShow=function(hash,dlg0){
		//dlg0.$Layout.css("top",25);
		//�ж��Ƿ��ǿɸ���Ԫ��
		if(sohu.diyConsole.CurElm.Copyable){
			_this.$ElmCAction.show();
		}else{
			_this.$ElmCAction.hide();
		};
		//��׽iframe�༭���û�ѡ��������
		sohu.diyConsole.CurElm.i$frame[0].i$Body().unbind("mouseup").mouseup(p.onIframeSelect);
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0){
		var url=p._fm.txtAHref.val();
		if(url!=""&&!StringUtils.isUrl(url)){
			p._fm.txtAHref.addClass("alert").select();
			p._fm.tipAHref.addClass("alert").show();
			return false;
		};
		p._fm.txtAHref.removeClass("alert");
		p._fm.tipAHref.removeClass("alert").hide();	
		if(p._fm.isNew&&url!=""){
			sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
				sohu.diyConsole.DocSelection.selectAndRelease();
			});
		}else{
			if(p._fm.a){
				p._fm.a.$obj.attr("title",p._fm.txtATitle.val())
					.attr("href",url)
					.attr("target",p._fm.ddlATarget.curVal);				
			};
		};
		dlg0.Hide();
	};
	//�¼�ע��
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
	});
	p._fm.ddlATarget.change(function(evt){
		p._fm.ddlATarget.curVal=this.value;
	});
	p._fm.ddlATarget.curVal="_blank";
	//jqm options
	this.$Layout.jqmOpts={
		title:"����",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ͼƬ�༭����
 * @param {Object} dlg
 */
sohu.diyDialog.wImage=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#picMsg");
	this.$ElmCAction=this.$Layout.find(".elmcActs");
	p._fm={
		h:$("#picH"),
		w:$("#picW"),
		bcolor:$("#picBColor"),
		src:$("#picSrc"),
		btnSwitch:$("#picSwitch"),
		link:$("#picLink"),
		tipLink:this.$Layout.find(".tipPicLink"),
		ddlTarget:$("#picTarget"),
		title:$("#picTitle"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");		
		return true;
	};	
	p.afterShow=function(hash,dlg0){
		//dlg0.$Layout.css("top",25);
		//�ж��Ƿ��ǿɸ���Ԫ��
		if(sohu.diyConsole.CurElm.Copyable){
			_this.$ElmCAction.show();
		}else{
			_this.$ElmCAction.hide();
		};
		var $img=sohu.diyConsole.CurElm.$Layout;
		//�߿�
		var h=$img.attr("height");
		h=h==""?"auto":h;
		var w=$img.attr("width");
		w=h==""?"auto":w;
		p._fm.h.val(h);
		p._fm.w.val(w);
		//�߿�ɫ
		p._fm.bcolor.css("backgroundColor",$img.css("backgroundColor"));
		//src
		p._fm.src.val($img.attr("src"));
		//����
		var $a=$img.parent("a");
		$a=$a.length>0?$a:$({href:"",title:""});
		var href=$a.attr("href");
		href=href=="#"?"":href;
		p._fm.link.val(href);
		//ͼƬ����
		p._fm.title.val($img.attr("title"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0){
		//ͼƬ��ַ
		var src=p._fm.src.val();
		/*
		if(src!=""&&!StringUtils.isUrl(src)){
			p._fm.src.addClass("alert").select();
			return false;
		};
		p._fm.src.removeClass("alert");
		*/
		//����
		var link=p._fm.link.val();
		if(link!=""&&!StringUtils.isUrl(link)){
			p._fm.link.addClass("alert").select();
			p._fm.tipLink.addClass("alert").show();
			return false;
		};
		p._fm.link.removeClass("alert");
		p._fm.tipLink.removeClass("alert").hide();
		//�߿�
		var fine=true,v=["auto","auto"];
		$.each([p._fm.h,p._fm.w],function(i,o){
			var v0=o.val();
			v[i]=v0;
			v[i]=v0==""?"auto":v[i];
			if((v0!="")&&(!StringUtils.isPlusInt(v0))){
				o.addClass("alert");
				fine=fine&&false;
			};//if
		});//each
		if(!fine){
			return false;
		};
		//�߿�ɫ
		var c=p._fm.bcolor.curVal;
		//����ͼƬ��ʽ
		sohu.diyConsole.CurElm.$Layout.css({
			"border-color":c
		}).attr("src",src).attr("height",v[0]).attr("width",v[1])
		.attr("title",p._fm.title.val()).attr("alt",p._fm.title.val());
		//��������
		var a=sohu.diyConsole.CurElm.$Layout.parent("a");
		if(a.length>0){
			if(link!=""){
				a.attr("target",p._fm.ddlTarget.val()).attr("href",link);
			};	
		};
		dlg0.Hide();
	};
	//�¼�ע��
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
	});
	p._fm.bcolor.click(function(evt){
		var _this=$(this);
		sohu.diyDialog.showColorPicker({
			onSubmit:function(c){
				p._fm.bcolor.css("backgroundColor",c);
				p._fm.bcolor.curVal=c;
			},
			onChange:function(c){
				p._fm.bcolor.css("backgroundColor",c);
				p._fm.bcolor.curVal=c;
			}
		});		
	});
	//jqm options
	this.$Layout.jqmOpts={
		title:"ͼ��",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ҳ������
 * @param {Object} dlg
 */
sohu.diyDialog.wPagePro=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wPagePro");
	//�¼�����
	p._fm={
		title:$("#pTitle"),
		kwd:$("#pKeywords"),
		desc:$("#pDesc")
	};
	p.afterShow=function(hash,dlg0){
		p._fm.title.val($("title").html());
	};
	p.onOK=function(dlg0){
		dlg0.Hide();
	};
	//�¼�ע��
	//jqm options
	this.$Layout.jqmOpts={
		title:"ҳ������",
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * ҳ����
 * @param {Object} dlg
 */
sohu.diyDialog.wTheme=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wTheme");
	//�¼�����
	p.afterShow=function(hash,dlg0){
	};
	p.onOK=function(dlg0){
		dlg0.Hide();
	};
	//�¼�ע��
	//jqm options
	this.$Layout.jqmOpts={
		title:"�������",
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * ����������ò˵�
 * @param {Object} dlg
 */
sohu.diyDialog.wSetting1=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wSetting1");
	this.$Body=this.$Layout.children();
	this.IsOpen=true;
	//�¼�����
	p.onTheme=function(evt){
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("wTheme");
	};
	p.onPagePro=function(evt){
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("wPagePro");
	};
	p.onPageBG=function(evt){
		//if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("cfgPage");
	};	
	p.onPreview=function(evt){
		var $b=$("body");
		if ($b.hasClass("preview")) {
			$(this).find("strong").html("Ԥ��");
			$b.removeClass("preview");
		}else{
			$b.addClass("preview");
			$(this).find("strong").html("�ر�Ԥ��");
		};
			
		if((!sohu.diyConsole.CurArea)||(!sohu.diyConsole.CurArea.IsEditing))
			return false;
			
		sohu.diyDialog.Hide();
			
		return false;
	};
	p.onPublish=function(evt){alert("publish");};
	p.onHelp=function(evt){alert("help");};
	p.onToggle=function(evt){
		if(_this.IsOpen){
			_this.$Body.addClass("close");
			_this.$Layout.animate({left:-82},"normal");
			_this.IsOpen=false;
		}else{
			_this.$Body.removeClass("close");
			_this.$Layout.animate({left:-1},"normal");
			_this.IsOpen=true;
		};
		return false;
	};
	//�¼�ע��
	this.$Layout.find("a").click(function(evt){return false;});
	$("#wsTheme").click(p.onTheme);
	$("#wsPagePro").click(p.onPagePro);
	$("#wsPageBG").click(p.onPageBG);
	$("#wsPreview").click(p.onPreview);
	$("#wsPublish").click(p.onPublish);
	$("#wsHelp").click(p.onHelp);
	this.$Layout.find(".ctr").click(p.onToggle);
	
};
/**
 * ���й�����
 * @param {Object} dlg
 */
sohu.diyDialog.wAreaTool=function(dlg){
	var p={};
	//DOM����
	p.$layout=$("#areaTools");
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	
	//�¼�����
	p.onReposition=function(evt){
		if(!sohu.diyConsole.CurArea){p.$layout.attr("style","");return;};
		var top=sohu.diyConsole.CurArea.$Layout.position().top-sohu.diyConsole.$Window.scrollTop();
		top=top<0?0:top;
		p.$layout.css("top",top);	
	};
	
	p.onAdd=function(evt){
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("addBlock");
	};
	p.onDel=function(evt){
		if(!sohu.diyConsole.CurArea){
			sohu.diyDialog.doAlert({text:'δѡ���κκ���!'});
			return false;
		};
		dlg.Console.Areas=$.grep(dlg.Console.Areas,function(o,i){
			if(o.ID==dlg.Console.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
	};
	p.onCfgBG=function(evt){
		if(!sohu.diyConsole.CurArea){sohu.diyDialog.doAlert({text:'δѡ���κκ���!'});return false;};
		if(sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("cfgArea");
	};
	p.onMove=function(evt){
		if(!sohu.diyConsole.CurArea){sohu.diyDialog.doAlert({text:'δѡ���κκ���!'});return false;};
		if(sohu.diyConsole.CurArea.IsEditing) return false;	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		p.onReposition();
	};
	//�¼�ע��
	p.initEvts=function(){
		p.$layout.bind("evtReposition",p.onReposition).find("a").click(function(evt){return false;});
		p._$btnAdd.click(p.onAdd);
		p._$btnDel.click(p.onDel);
		p._$btnBG.click(p.onCfgBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
	}();
	
	this.$Layout=p.$layout;
	this.RePosition=p.onReposition;	
};
/**
 * ���������ض�λ
 */
sohu.diyDialog.wAreaTool.Reposition=function(){
	var dlg=sohu.diyDialog.Get("areaTools");
	if(!dlg) return;
	dlg.RePosition();
};
