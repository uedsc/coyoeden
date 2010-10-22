/**
 * JS interactive logic for editing fragments
 * 碎片编辑器
 * @author levinhuang
 * @version 2010.10.13
 * 1,更新一些编辑器专属class防止冲突，class前缀为cedt_
 * 2,加粗加红利用font标签.(为了兼容老代码)
 */

var chipEditor = function() {
    var p={},pub={};
	p.editors={};
	/* 事件处理 */	
	/**
	 * 获取碎片编号的逻辑
	 * @param {Object} $chip 碎片的jq dom对象
	 */
	p.getChipID=function($chip){
		var id=$chip.attr("cid");
		if((!id)||(id=="")){
			id="flag"+StringUtils.RdStr(8);
			$chip.attr("cid",id);
		};
		return id;	
	};
	/**
	 * 禁用a标签的默认导航行为
	 * @param {Object} evt
	 */
	p.stopNav=function(evt){
		evt.preventDefault();
		return true;		
	};
	p.initTinyMCE=function(){
		/*这种异步加载tiny_mce.js的方式在ie下有问题，需要移到chipEditor.js外面。否则需要用原生的tinymce.init方法。
		p._$mce=$("#tinymce1").tinymce({
			script_url:'editor/tiny_mce/tiny_mce.js',
			theme:'simple'
		}).hide();
		*/
		if(chipEditor.NoTinyMCE) return;
		tinymce.init({
			mode:'specific_textareas',
			editor_selector:'cedt_mceEditor',
			theme:'simple',
			language: "en",
			convert_urls : false,
			remove_trailing_nbsp: false,
			extended_valid_elements: "link[id|rel|type|href],style[id|type],align[left|center|right|middle]",
			entities : '160,nbsp,161,iexcl,162,cent,163,pound,164,curren,165,yen,166,brvbar,167,sect,168,uml,169,copy,170,ordf,171,laquo,172,not,173,shy,174,reg,175,macr,176,deg,177,plusmn,178,sup2,179,sup3,180,acute,181,micro,182,para,184,cedil,185,sup1,186,ordm,187,raquo,188,frac14,189,frac12,190,frac34,191,iquest,192,Agrave,193,Aacute,194,Acirc,195,Atilde,196,Auml,197,Aring,198,AElig,199,Ccedil,200,Egrave,201,Eacute,202,Ecirc,203,Euml,204,Igrave,205,Iacute,206,Icirc,207,Iuml,208,ETH,209,Ntilde,210,Ograve,211,Oacute,212,Ocirc,213,Otilde,214,Ouml,215,times,216,Oslash,217,Ugrave,218,Uacute,219,Ucirc,220,Uuml,221,Yacute,222,THORN,223,szlig,224,agrave,225,aacute,226,acirc,227,atilde,228,auml,229,aring,230,aelig,231,ccedil,232,egrave,233,eacute,234,ecirc,235,euml,236,igrave,237,iacute,238,icirc,239,iuml,240,eth,241,ntilde,242,ograve,243,oacute,244,ocirc,245,otilde,246,ouml,247,divide,248,oslash,249,ugrave,250,uacute,251,ucirc,252,uuml,253,yacute,254,thorn,255,yuml,402,fnof,913,Alpha,914,Beta,915,Gamma,916,Delta,917,Epsilon,918,Zeta,919,Eta,920,Theta,921,Iota,922,Kappa,923,Lambda,924,Mu,925,Nu,926,Xi,927,Omicron,928,Pi,929,Rho,931,Sigma,932,Tau,933,Upsilon,934,Phi,935,Chi,936,Psi,937,Omega,945,alpha,946,beta,947,gamma,948,delta,949,epsilon,950,zeta,951,eta,952,theta,953,iota,954,kappa,955,lambda,956,mu,957,nu,958,xi,959,omicron,960,pi,961,rho,962,sigmaf,963,sigma,964,tau,965,upsilon,966,phi,967,chi,968,psi,969,omega,977,thetasym,978,upsih,982,piv,8226,bull,8230,hellip,8242,prime,8243,Prime,8254,oline,8260,frasl,8472,weierp,8465,image,8476,real,8482,trade,8501,alefsym,8592,larr,8593,uarr,8594,rarr,8595,darr,8596,harr,8629,crarr,8656,lArr,8657,uArr,8658,rArr,8659,dArr,8660,hArr,8704,forall,8706,part,8707,exist,8709,empty,8711,nabla,8712,isin,8713,notin,8715,ni,8719,prod,8721,sum,8722,minus,8727,lowast,8730,radic,8733,prop,8734,infin,8736,ang,8743,and,8744,or,8745,cap,8746,cup,8747,int,8756,there4,8764,sim,8773,cong,8776,asymp,8800,ne,8801,equiv,8804,le,8805,ge,8834,sub,8835,sup,8836,nsub,8838,sube,8839,supe,8853,oplus,8855,otimes,8869,perp,8901,sdot,8968,lceil,8969,rceil,8970,lfloor,8971,rfloor,9001,lang,9002,rang,9674,loz,9824,spades,9827,clubs,9829,hearts,9830,diams,338,OElig,339,oelig,352,Scaron,353,scaron,376,Yuml,710,circ,732,tilde,8194,ensp,8195,emsp,8201,thinsp,8204,zwnj,8205,zwj,8206,lrm,8207,rlm,8211,ndash,8212,mdash,8216,lsquo,8217,rsquo,8218,sbquo,8220,ldquo,8221,rdquo,8222,bdquo,8224,dagger,8225,Dagger,8240,permil,8249,lsaquo,8250,rsaquo,8364,euro',
			oninit:function(){
				tinymce.activeEditor.hide();
			}
		});
	};
    //private area
    p.initVar = function(opts) {
		p._cssBold=opts.cssBold||'cedt_bb';
		p._cssColor=opts.cssColor||'cedt_cc';
		chipEditor.aTplStr=opts.aTplStr||'<a href="http://sohu.com" title="">sohu</a>';
		chipEditor.vdTplStr=opts.vdTplStr||'<a href="http://sohu.com" title=""><img src="http://images.sohu.com/uiue/vd.gif" alt="Video"/></a>';
		p._dlgModel=jQuery(".cedt_layout");
		p._$body=jQuery("body");
		chipEditor.NoTinyMCE=opts.noTinyMCE||false;
		/* 用户的回调函数 */
		p.onUpPic=opts.onUpPic;
		p.onTest=opts.onTest;
		p.onSave=opts.onSave;
		p.onCancel=opts.onCancel;
		p.onGlobalRes=opts.onGlobalRes;
		p.onExternal=opts.onExternal;
		p.onFlashEdit=opts.onFlashEdit;
		p.onLoadHis=opts.onLoadHis;
		/* /用户的回调函数 */
	};
    p.onLoaded = function() { 
		p.initTinyMCE();
	};
    p.initEvents = function(opts) {
        jQuery(document).ready(p.onLoaded);
		jQuery(".cedt_acts a,.cedt_layout button").button();
		jQuery(".cedt_jqmCT a").bind('click.noNav',p.stopNav);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	/**
	 * 显示碎片编辑器
	 * @param {Object} chip 碎片的dom对象
	 * @param {Object} opts 其他选项
	 */
	pub.Show=function(chip,opts){
		opts=jQuery.extend({tabs:[0,1,2]},opts||{});
		var dlg=null,$chip=jQuery(chip),id=p.getChipID($chip);
		opts.isNew=false;
		if(!(dlg=p.editors[id])){
			opts.isNew=true;
			opts=jQuery.extend({
				dlgModel:p._dlgModel,
				$body:p._$body,
				$chip:$chip,
				onUpPic:p.onUpPic,
				onTest:p.onTest,
				onSave:p.onSave,
				onCancel:p.onCancel,
				onGlobalRes:p.onGlobalRes,
				onExternal:p.onExternal,
				onFlashEdit:p.onFlashEdit,
				onLoadHis:p.onLoadHis								
			},opts);
			dlg=new chipEditor.Dialog(opts);
			p.editors[id]=dlg;
			
		};
		dlg.Show(opts);
	};
	pub.MCE=function(){
		//return p._$mce.tinymce();
		if(chipEditor.NoTinyMCE){
			//不用tinymce,为了减少改动代码,伪造一个tinyMCE
			if(!p._fakeMCE){
				p._fakeMCE={
					html:"",
					setContent:function(html){
						this.html=html;
					},
					getContent:function(){
						return this.html;
					}
				};
			};
			return p._fakeMCE; 
		};
		return tinymce.activeEditor;
	};
	pub.StopNav=p.stopNav;
	pub.$RowTxtA=jQuery("#cedt_elmATpl .cedt_rowTxt");
	pub.$RowTitleA=jQuery("#cedt_elmATpl .cedt_rowTitle");
	pub.$RowTxtA0=jQuery("#cedt_elmATpl .cedt_rowTxt0");
	pub.$RowTitleA0=jQuery("#cedt_elmATpl .cedt_rowTitle0");		
	pub.$RowLnkA=jQuery("#cedt_elmATpl .cedt_rowLnk");
	pub.$RowImg=jQuery("#cedt_elmImgTpl .cedt_rowImg");
	pub.$RowALT=jQuery("#cedt_elmImgTpl .cedt_rowALT");
	pub.$RowLnkImg=jQuery("#cedt_elmImgTpl .cedt_rowLnk");
    return pub;
} ();
/**
 * 碎片编辑器弹框类
 * @param {Object} opts
 */
chipEditor.Dialog=function(opts){
	opts=jQuery.extend({},{
		tabIDTpl:'wVsp_tab',
		lblSort:'排序状态',
		lblSort1:'结束排序',
		lblTab0:'<strong class="cedt_alert">行修改状态</strong><em>请选择行</em>',
		clBold:'cedt_bb',
		clColor:'cedt_cc',
		clSort:'cedt_sort',
		clIng:'cedt_ing',
		onUpPic:null,
		onTest:null,
		onSave:null,
		onCancel:null,
		onGlobalRes:null,
		onExternal:null,
		onFlashEdit:null,
		onLoadHis:null
		},opts||{});
	var _this=this;
	this.Sorting=false;
	this._opts=opts;
	this.$Elm=null;//当前元素
	this.$Chip=opts.$chip;//当前碎片
	this.SortElm=null;//当前排序元素
	
	//DOM引用
	this.$Layout=opts.dlgModel.clone().appendTo(opts.$body);
	chipEditor.MCE().setContent(this.$Chip.html());
	this.$Code=this.$Layout.find('.cedt_txtVspC').val(chipEditor.MCE().getContent());

	this.$Layout.find('.cedt_txtVspCS').textareaSearch({
		cssTextArea:_this.$Code,
		cssBtn:_this.$Layout.find('.cedt_btnVspCS')
	});
	this.$btnCode=this.$Layout.find(".cedt_external");//.globalRes,
	this.$ElmA=this.$Layout.find(".cedt_elmA");
	this.$ElmImg=this.$Layout.find(".cedt_elmImg");
	//事件处理--20101018
	this.InitEdit();
	//订阅window.onscroll事件-20100926
	jQuery(window).scroll(function(e){
		if(_this.$Layout.position().top<0)
			_this.$Layout.css("top",0);
	});
	// /20100926
	this.$Backup=this.$Chip.clone(true);
	//整体测试
	this.$BtnTest=this.$Layout.find(".cedt_test");
	//保存碎片
	this.$BtnSave=this.$Layout.find(".cedt_save");
	//取消
	this.$BtnCancel=this.$Layout.find(".cedt_cancel").click(function(evt){
		/*20101018
		var c=_this.$Backup.clone(true);
		_this.$Chip.replaceWith(c);
		_this.$Chip=c;
		*/
		_this.$Chip.html(_this.$Backup.html());
		_this.InitEdit();
		
		_this.$Layout.jqmHide();
		_this.UpdateCode();
		return false;
	});
	//统一资源库
	this.$BtnGlobalRes=this.$Layout.find(".cedt_globalRes");
	//外包
	this.$BtnExternal=this.$Layout.find(".cedt_external");
	//flash编辑
	this.$BtnFlashEdit=this.$Layout.find(".cedt_btnFlashEdit");
	//tab菜单
	var tabID="tab"+StringUtils.RdStr(8);
	this.$TabM=this.$Layout.find(".cedt_tabM").each(function(i,o){
		var $o=jQuery(o);
		$o.attr("href",$o.attr("href").replace(opts.tabIDTpl,tabID));
	});
	//tab内容
	this.$TabC=this.$Layout.find(".cedt_tabC").each(function(i,o){
		var $o=jQuery(o);
		$o.attr("id",$o.attr("id").replace(opts.tabIDTpl,tabID));
	});
	//排序按钮
	this.$BtnSort=this.$Layout.find('.cedt_btnSort').click(function(evt){
		_this.ToggleSorting(jQuery(this));return false;
	});
	//tab
	this.$CT=this.$Layout.find(".cedt_jqmCT").tabs({
		select:function(evt,ui){
			//按钮的显隐
			if(ui.index==1){
				_this.$btnCode.removeClass("cedt_hide");
			}else{
				_this.$btnCode.addClass("cedt_hide");
			};
			//修改记录
			if(ui.index==2){
				if(_this.onLoadHis){
					_this.onLoadHis(_this);
				};
			};
		}
	});
	//上传浮层
	this.$UpPic=this.$CT.find(".cedt_uppic").draggable({handle:".cedt_upp_t",containment:'parent',axis:'y'});
	this.$BtnUpPic=this.$UpPic.find(".cedt_up");
	this.$UpPic.find(".cedt_cls").click(function(evt){
		_this.$UpPic.slideUp("fast");
	});
	//jqm options
	this.jqmOpts={trigger:false,modal:false,overlay:false,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null,overlayClass:"cedt_jqmovl",closeClass:'cedt_jqmcls'};
	this.jqmOpts.onShow=function(hash){
		var doShow=true;
		if(_this.jqmOpts.beforeShow){
			doShow=_this.jqmOpts.beforeShow(hash,_this);
		};
		if(doShow){
			hash.w.show();
			_this.jqmHash=hash;
			//清除排序状态
			if(_this.Sorting){
				_this.$BtnSort.trigger("click");
			};
			//显示后的回调函数					
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
			//afterHide callback
			_this.$Chip.removeClass("cedt_on");
			//重置第一个tab的内容
			_this.$ElmA.empty().html(_this._opts.lblTab0);
			_this.$ElmImg.empty().html(_this._opts.lblTab0);
			
			if(_this.jqmOpts.afterHide)
			{
				_this.jqmOpts.afterHide(hash,_this);
			};
		};	
	};
	
	//draggable
	this.$Layout.draggable({handle:".cedt_hd",containment:'body'});
	//margin-left
	this.$Layout.css("margin-left",-(this.$Layout.width()/2));
}; 
chipEditor.Dialog.prototype.Show=function(opts){
	var _this=this;
	opts=jQuery.extend({tabs:[0,1,2]},opts||{});
	jQuery.extend(this.jqmOpts,{trigger:false,modal:false,overlay:false,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null});//reset
	jQuery.extend(this.jqmOpts,opts);
	this.IsNew=opts.isNew;
	if(!opts.isNew)
		this.Hide(opts);
	//tabs
	if((!opts.tabs)||opts.tabs.length==0){
		this.$CT.hide();
	}else{
		this.$TabM.hide();
		jQuery.each(opts.tabs,function(i,o){
			_this.$TabM.eq(o).show();
		});
		this.$CT.tabs("select",opts.tabs[0]);
	};
	//按钮回调
	if(opts.onTest){
		this.$BtnTest.unbind("click.a").bind("click.a",function(evt){opts.onTest(_this);return false;});
	};
	if(opts.onSave){
		this.$BtnSave.unbind("click.a").bind("click.a",function(evt){opts.onSave(_this);return false;});
	};
	if(opts.onCancel){
		this.$BtnCancel.unbind("click.a").bind("click.a",function(evt){opts.onCancel(_this);return false;});
	};
	if(opts.onGlobalRes){
		this.$BtnGlobalRes.unbind("click.a").bind("click.a",function(evt){opts.onGlobalRes(_this);return false;});
	};
	if(opts.onExternal){
		this.$BtnExternal.unbind("click.a").bind("click.a",function(evt){opts.onExternal(_this);return false;});
	};
	if(opts.onFlashEdit){
		this.$BtnFlashEdit.unbind("click.a").bind("click.a",function(evt){opts.onFlashEdit(_this);return false;});
	};
	if(opts.onUpPic){
		this.$BtnUpPic.unbind("click.a").bind("click.a",function(evt){opts.onUpPic(_this);return false;});
	};
	if(opts.onLoadHis){
		this.onLoadHis=opts.onLoadHis;
	};
	this.$Chip.addClass("cedt_on");
		
	this.$Layout.jqm(this.jqmOpts).jqmShow();
};
chipEditor.Dialog.prototype.Hide=function(opts){
	jQuery.extend(this.jqmOpts,opts||{});
	this.$Layout.jqm(this.jqmOpts).jqmHide();
	return this;
};
chipEditor.Dialog.prototype.ToggleSorting=function($i){
	var _this=this;
	if(this.Sorting){
		$i.removeClass("cedt_btnSort1").find("span").html(this._opts.lblSort);
		this.$CT.tabs("option","disabled",[]);
		this.$CT.tabs("option","selected",0);
		
		this.Sorting=false;
		//绑定click.edit事件
		/* 20101018 */
		this.InitEdit();
		/* /20101018 */
		//重置"可视化修改"和"代码修改"(由于元素发生了变化)
		this.$ElmA.empty().html(this._opts.lblTab0);
		this.$ElmImg.empty().html(this._opts.lblTab0);
	}else{
		$i.prev().trigger("click");
		$i.addClass("cedt_btnSort1").find("span").html(this._opts.lblSort1);
		this.$CT.tabs("option","disabled",[0,1,2]);
		this.Sorting=true;
		//移除碎片a标签的click.edit事件处理
		/* 20101018 */
		/*
		this.$Chip.find("a").unbind("click").bind("click.sort",function(evt){
			_this.Sort(jQuery(this));return false;
		});
		*/
		this.InitSort();
		/* /20101018 */
	};
};
/**
 * 编辑某个元素
 * @param {Object} $elm
 */
chipEditor.Dialog.prototype.Edit=function($elm){
	var _t=0,_this=this;
	//改元素是否在编辑中
	if($elm.hasClass(this._opts.clIng)) return;
	
	$elm.addClass(this._opts.clIng);
	
	//检查元素类型
	if($elm.find("img").length>0){
		_t=1;//图片
	}else{
		_t=0;//非图片
	};
	
	var $elmList=$elm.parent().contents();
	
	if(this.$Elm) this.$Elm.removeClass(this._opts.clIng);
	this.$Elm=$elm;
	this.$Chip.addClass("cedt_on");
	
	var onShow=function(hash,dlg){	
		//第一个tab
		dlg.$ElmA.hide().empty();
		dlg.$ElmImg.hide().empty();
		switch(_t){
			case 0:
				$elmList.each(function(i,o){
					var $o=jQuery(o);
					var tpl=null;
					if(o.nodeType!=1){
						//文本
						tpl=chipEditor.$RowTxtA.clone();
						dlg.$ElmA.append(tpl);
						var data={$obj:$o,$tpl:tpl};
						tpl.find("input").val($o.text()).bind("keyup",function(evt){
							/*20101018
							var obj0=jQuery(document.createTextNode(this.value));
							data.$obj.replaceWith(obj0);
							data.$obj=obj0;
							*/
							data.$obj[0].nodeValue=this.value;
							dlg.UpdateCode();
						}).focus(_this.focusSelect);
						//删除按钮
						tpl.find(".cedt_btnDel").bind("click",function(evt){
							data.$obj.remove();
							tpl.remove();
							dlg.UpdateCode();
							return false;
						});
					}else if($o.is("a")){
						var $img=$o.find("img");
						if($img.length>0){
							//图片链接
							tpl=jQuery([chipEditor.$RowImg.clone()[0],chipEditor.$RowALT.clone()[0],chipEditor.$RowLnkImg.clone()[0]]);
							tpl.eq(0).find("input").val($img.attr("src"));
							tpl.eq(1).find("input").val($img.attr("alt"));
							tpl.eq(2).find("input").val($o.attr("href"));
							_this.$ElmImg.append(tpl);
							_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t,$img:$img});								
						}else{
							tpl=jQuery([chipEditor.$RowTitleA.clone()[0],chipEditor.$RowLnkA.clone()[0]]);
							tpl.eq(0).find("input").val($o.html());//attr("title")
							tpl.eq(1).find("input").val($o.attr("href"));
							dlg.$ElmA.append(tpl);
							_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t});							
						};
					};

				});
				_this.$ElmA.show();
			break;
			case 1:
				$elmList.each(function(i,o){
					var $o=jQuery(o),tpl=null;
					if(o.nodeType!=1){
						//文本
						tpl=chipEditor.$RowTxtA.clone();
						_this.$ElmImg.append(tpl);
						var data={$obj:$o,$tpl:tpl,t:_t};
						tpl.find("input").val($o.text()).bind("keyup",function(evt){
							/* 20101018 */
							/*
							var obj0=jQuery(document.createTextNode(this.value));
							data.$obj.replaceWith(obj0);
							data.$obj=obj0;
							*/
							data.$obj[0].nodeValue=this.value;
							/* /20101018 */
							dlg.UpdateCode();
						}).focus(_this.focusSelect);
						//删除按钮
						tpl.find(".cedt_btnDel").bind("click",function(evt){
							data.$obj.remove();
							tpl.remove();
							dlg.UpdateCode();
							return false;
						});								
					}else if($o.is("a")){
						var $img=$o.find("img");
						if($img.length>0){
							//图片链接
							tpl=jQuery([chipEditor.$RowImg.clone()[0],chipEditor.$RowALT.clone()[0],chipEditor.$RowLnkImg.clone()[0]]);
							tpl.eq(0).find("input").val($img.attr("src"));
							tpl.eq(1).find("input").val($img.attr("alt"));
							tpl.eq(2).find("input").val($o.attr("href"));
							_this.$ElmImg.append(tpl);
							_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t,$img:$img});								
						}else{
							//文字链接
							tpl=jQuery([chipEditor.$RowTitleA.clone()[0],chipEditor.$RowLnkA.clone()[0]]);
							tpl.eq(0).find("input").val($o.html());
							tpl.eq(1).find("input").val($o.attr("href"));
							_this.$ElmImg.append(tpl);
							_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:0});								
						};
					};
				});//each
				_this.$ElmImg.show();
			break;
		};//switch
		//第二个tab
		/*
		chipEditor.MCE().setContent(dlg.$Chip.html());
		dlg.$Code.val(chipEditor.MCE().getContent());
		*/
		dlg.UpdateCode();
		//上传按钮
		dlg.$Layout.find(".cedt_btnUpl").click(function(evt){
			dlg.$UpPic.show().effect("highlight");
			//设定当前上传的图片对象2010.10.15
			dlg.$UpPic.data=jQuery(this).data("data");
			return false;
		});
					
	};//onShow
	var onHide=function(hash,dlg){
		if(dlg.$Elm){
			dlg.$Elm.removeClass(_this._opts.clIng);
			dlg.$Elm=null;			
		};
		dlg.$Chip.removeClass("cedt_on");
	};//onHide
	this.Show({
		afterShow:onShow,
		afterHide:onHide
	});
};
/**
 * 排序
 * @param {Object} $i
 */
chipEditor.Dialog.prototype.Sort=function($i){
	if($i.hasClass(this._opts.clSort)) return;
	if(!this.SortElm){
		this.SortElm=$i.addClass(this._opts.clSort);
		return;
	};
	//调换内容
	/* 20101018 */
	var temp=this.SortElm.html(),_class=this.SortElm.removeClass(this._opts.clSort).attr("class"),_tl=this.SortElm.attr("title"),_href=this.SortElm.attr("href");
	this.SortElm.html($i.html()).attr("class",$i.attr("class")).attr("title",$i.attr("title")).attr("href",$i.attr("href"));
	$i.html(temp).attr("class",_class).attr("title",_tl).attr("href",_href);
	/* /20101018 */
	
	//结束本次操作
	this.SortElm=null;
	if(this.$Elm)
		this.$Elm.removeClass(this._opts.clIng);
	//更新代码textarea
	/*
	chipEditor.MCE().setContent(this.$Chip.html());
	this.$Code.val(chipEditor.MCE().getContent());
	*/
	this.UpdateCode();
};
/**
 * 图标事件处理
 * @param {Object} $tpl
 * @param {Object} data
 */
chipEditor.Dialog.prototype.BindIconEvts=function($tpl,data){
	var _this=this;
	//删除按钮
	$tpl.find(".cedt_btnDel").bind("click",function(evt){
		_this.DelElm(data);return false;
	});
	//复制按钮
	$tpl.find(".cedt_btnAdd").bind("click",function(evt){
		_this.AddElm(data);return false;
	});
	//加粗
	$tpl.find(".cedt_bold").bind("click",function(evt){
		_this.Bold(data);return false;
	});
	//颜色
	$tpl.find(".cedt_color").bind("click",function(evt){
		_this.Color(data);return false;
	});
	//检查连接按钮
	$tpl.find(".cedt_checklink").click(function(evt){
		var lnk=jQuery.trim(jQuery(this).prev().val());
		if(lnk!="")
			window.open(lnk);
	});
	
	if(data.t==0){
		//标题输入框
		var ipt0=$tpl.eq(0).find("input").keyup(function(evt){
			data.$obj.html(this.value);
			_this.UpdateCode();
		}).focus(this.focusSelect);
		if(data.$obj.hasClass(_this._opts.clBold)){
			ipt0.addClass(_this._opts.clBold);
		};
		if(data.$obj.hasClass(_this._opts.clColor)){
			ipt0.addClass(_this._opts.clColor);
		};
		//链接输入框
		$tpl.eq(1).find("input").keyup(function(evt){
			data.$obj.attr("href",this.value);
			_this.UpdateCode();
		}).focus(this.focusSelect);
		//其他图标
		$tpl.eq(0).find('.cedt_icon').click(function(evt){
			jQuery(this).next().toggle();
			return false;
		});
		//视频前
		$tpl.eq(0).find(".cedt_videoL").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:true});
			jQuery(this).parents("ul.cedt_others").hide();
			return false;
		});
		//视频后
		$tpl.eq(0).find(".cedt_videoR").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:false});
			jQuery(this).parents("ul.cedt_others").hide();
			return false;
		});			
	}else{
		//图片地址输入框
		$tpl.eq(0).find("input").change(function(evt){
			data.$img.attr("src",this.value);
			_this.UpdateCode();
		}).focus(this.focusSelect);
		//ALT输入框
		$tpl.eq(1).find("input").change(function(evt){
			data.$img.attr("alt",this.value);
			_this.UpdateCode();
		}).focus(this.focusSelect);
		//链接输入框
		$tpl.eq(2).find("input").change(function(evt){
			data.$obj.attr("href",this.value);
			_this.UpdateCode();
		}).focus(this.focusSelect);
		//为上传按钮设定图片对象-2010.10.15
		$tpl.find(".btnUpl").data("data",data);
	};

};
/**
 * 删除元素
 * @param {Object} data
 */
chipEditor.Dialog.prototype.DelElm=function(data){
	if(data.$obj){
		data.$obj.remove();
		data.$tpl.remove();
		this.UpdateCode();
	};
};
/**
 * 加粗字体
 * @param {Object} data
 */
chipEditor.Dialog.prototype.Bold=function(data){
	if(!data.$obj) return false;
	var $font=data.$obj.children();
	//a内部没有font标签,加一个
	if(!$font.is("font")){
		data.$obj.wrapInner('<font style="font-weight:bold;"/>');
		data.$tpl.eq(0).find("input").addClass(this._opts.clBold);
		this.UpdateCode();
		return;	
	};
	//已有font标签，则改样式
	if($font.css("font-weight")=="bold"){
		$font.css("font-weight","normal");
		data.$tpl.eq(0).find("input").removeClass(this._opts.clBold);
	}else{
		$font.css("font-weight","bold");
		data.$tpl.eq(0).find("input").addClass(this._opts.clBold);
	};
	this.UpdateCode();
};
/**
 * 着色
 * @param {Object} data
 */
chipEditor.Dialog.prototype.Color=function(data){
	if(!data.$obj) return false;
	var $font=data.$obj.children();
	//a内部没有font标签,加一个
	if(!$font.is("font")){
		data.$obj.wrapInner('<font color="red"/>');
		data.$tpl.eq(0).find("input").addClass(this._opts.clColor);
		this.UpdateCode();
		return;	
	};
	//已有font标签，则改样式
	if($font.attr("color")=="red"){
		if(this._rawColor){
			$font.attr("color",this._rawColor);
		}else{
			$font.removeAttr("color");	
		};
		data.$tpl.eq(0).find("input").removeClass(this._opts.clColor);
	}else{
		if(!this._rawColor)
			this._rawColor=$font.attr("color");
		
		$font.attr("color","red");
		data.$tpl.eq(0).find("input").addClass(this._opts.clColor);
	};
	this.UpdateCode();
};
/**
 * 添加元素
 * @param {Object} data
 */
chipEditor.Dialog.prototype.AddElm=function(data){
	if(!data.$obj) return false;
	var txt0=jQuery(document.createTextNode(" "));
	var a0=jQuery(chipEditor.aTplStr);
	var _this=this;
	data.$obj.after(a0).after(txt0);
	a0.bind('click.noNav',chipEditor.StopNav).bind("click.edit",function(evt){_this.Edit(jQuery(this));});
	
	var tpl=chipEditor.$RowTxtA.clone();
	var data1={$obj:txt0,$tpl:tpl};
	//文字输入框事件(闭包的应用)
	tpl.find("input").val(" ").bind("keyup",function(evt){
		/* 20101018 */
		/*
		var obj0=jQuery(document.createTextNode(this.value));
		data1.$obj.replaceWith(obj0);
		data1.$obj=obj0;
		*/
		data1.$obj[0].nodeValue=this.value;
		/* /20101018 */
		_this.UpdateCode();
	}).focus(this.focusSelect);
	tpl.find(".cedt_btnDel").bind("click",function(evt){
		data1.$obj.remove();
		tpl.remove();
		_this.UpdateCode();
	});
	var tpl1=jQuery([chipEditor.$RowTitleA.clone()[0],chipEditor.$RowLnkA.clone()[0]]);
	//输入框回填内容
	tpl1.eq(0).find("input").val(a0.html());
	tpl1.eq(1).find("input").val(a0.attr("href"));
	var data2={$obj:a0,$tpl:tpl1,t:0};
	//图标事件处理
	this.BindIconEvts(tpl1,data2);
	
	data.$tpl.eq(data.$tpl.length-1).after(tpl1).after(tpl);
	this.UpdateCode();
	return false;
};
chipEditor.Dialog.prototype.focusSelect=function(evt){
	jQuery(this).select();
};
/**
 * 插入视频小图片
 * @param {Object} data
 */
chipEditor.Dialog.prototype.InsertVDIcon=function(data){
	var _this=this;
	var a=jQuery(chipEditor.vdTplStr).bind("click.noNav",chipEditor.StopNav).bind('click.edit',function(evt){
		_this.Edit(jQuery(this));
	});
	if(data.before){
		data.$obj.before(a);
	}else{
		data.$obj.after(a);
	};
	this.UpdateCode();
	return false;
};
/**
 * 更新代码编辑文本框
 */
chipEditor.Dialog.prototype.UpdateCode=function(){
	chipEditor.MCE().setContent(this.$Chip.html());
	this.$Code.val(chipEditor.MCE().getContent());
};
/**
 * 初始化碎片的事件处理20101018
 */
chipEditor.Dialog.prototype.InitEdit=function(){
	var _this=this;
	if(this.SortElm){
		this.SortElm.removeClass(this._opts.clSort);
		this.SortElm=null;
	};
	this.$Chip.find("a").unbind("click").bind("click.edit",function(evt){
		_this.Edit(jQuery(this));
		return false;
	});
	return this;
};
/**
 * 初始化碎片的排序事件20101018
 */
chipEditor.Dialog.prototype.InitSort=function(){
	var _this=this;
	this.$Chip.find("a").unbind("click").bind("click.sort",function(evt){
		_this.Sort(jQuery(this));return false;
	});
	return this;
};
