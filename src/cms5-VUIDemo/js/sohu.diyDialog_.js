/**
 * 可视化专题各个弹框的类
 * @author levinhuang
 * @dependency sohu.diyDialog.js
 */
/**
 * 添加横切弹框
 */
sohu.diyDialog.wArea=function(dlg){
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
	p.$layout=$("#vstp_addBlock");
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
sohu.diyDialog.wCfgArea=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_cfgArea");
	
	//私有变量÷函数
	p._fm={
		txtBG:$("#vstp_txtAreaBG"),
		txtID:$("#vstp_txtAreaID"),
		ddlBGAlign:$("#vstp_ddlAreaBGAlign"),
		ddlBGAlignV:$("#vstp_ddlAreaBGAlignV"),
		ddlBGRepeat:$("#vstp_ddlAreaBGRepeat"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//bg image
		var url=p._fm.txtBG.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBG.addClass("vstp_alert").select();
			return false;
		};
		if(url==""){
			sohu.diyConsole.CurArea.$Layout.css("background-image","");//shouldn't use 'none' here
		}else{
			sohu.diyConsole.CurArea.$Layout.css("background-image","url('"+url+"')");
			//bg position
			sohu.diyConsole.CurArea.$Layout.css("background-position",p._fm.ddlBGAlign.curVal+" "+p._fm.ddlBGAlignV.curVal);
			//bg repeat
			sohu.diyConsole.CurArea.$Layout.css("background-repeat",p._fm.ddlBGRepeat.curVal);
		};
		//area id
		var id=p._fm.txtID.val();
		if(!sohu.diyConsole.IsValidID(id)){
			p._fm.txtID.addClass("vstp_alert");
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
		//对齐
		var bg_p=sohu.diyConsole.CurArea.$Layout.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":(bg_p||"center top");
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlign.val(bg_p[0]);
			p._fm.ddlBGAlign.curVal=bg_p[0];
			p._fm.ddlBGAlignV.val(bg_p[1]);
			p._fm.ddlBGAlignV.curVal=bg_p[1];
		};		
		p._fm.ddlBGAlign.val(p._fm.ddlBGAlign.curVal);
		p._fm.ddlBGAlignV.val(p._fm.ddlBGAlignV.curVal);
		//平铺
		var bg_a=sohu.diyConsole.CurArea.$Layout.css("backgroundRepeat");
		p._fm.ddlBGRepeat.val(bg_a);
		p._fm.ddlBGRepeat.curVal=bg_a;		
		
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//事件注册
	p._fm.txtBG.change(function(evt){
		p.preview();
	});
	p._fm.ddlBGAlign.change(function(evt){
		p._fm.ddlBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignV.change(function(evt){
		p._fm.ddlBGAlignV.curVal=this.value;
		p.preview();
	});	
	p._fm.ddlBGRepeat.change(function(evt){
		p._fm.ddlBGRepeat.curVal=this.value;
		p.preview();
	});		
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
sohu.diyDialog.wCfgPage=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_cfgPage");
	
	//私有变量÷函数
	p._fm={
		txtBGA:$("#vstp_txtPageBGA"),
		txtBGB:$("#vstp_txtPageBGB"),
		txtBGHA:$("#vstp_txtPageBGHA"),
		txtBGHB:$("#vstp_txtPageBGHB"),
		ddlBGAlignA:$("#vstp_pageBGAlignA"),
		ddlBGAlignAV:$("#vstp_pageBGAlignAV"),
		ddlBGAlignB:$("#vstp_pageBGAlignB"),
		ddlBGAlignBV:$("#vstp_pageBGAlignBV"),
		ddlBGRepeatA:$("#vstp_pageBGRepeatA"),
		ddlBGRepeatB:$("#vstp_pageBGRepeatB"),
		txtBGC:$("#vstp_txtPageBGC"),
		cpk:this.$Layout.find(".vstp_cpk"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//页头背景图
		var url=p._fm.txtBGA.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBGA.addClass("vstp_alert").select();
			return false;
		};
		//页尾背景图
		var url1=p._fm.txtBGB.val();
		if((url1!="")&&(!StringUtils.isUrl(url1))){
			p._fm.txtBGB.addClass("vstp_alert").select();
			return false;
		};
		//设置页头背景
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
		sohu.diyConsole.$BodyBGA.css("background-position",p._fm.ddlBGAlignA.curVal+" "+p._fm.ddlBGAlignAV.curVal);
		sohu.diyConsole.$BodyBGA.css("background-repeat",p._fm.ddlBGRepeatA.curVal);
		
		//设置页尾背景
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
		sohu.diyConsole.$BodyBGB.css("background-position",p._fm.ddlBGAlignB.curVal+" "+p._fm.ddlBGAlignBV.curVal);
		sohu.diyConsole.$BodyBGB.css("background-repeat",p._fm.ddlBGRepeatB.curVal);
		
		if(cls)
			dlg0.Hide();
	};
	p.afterShow=function(hash,dlg0){
		//背景图
		var img=sohu.diyConsole.ParseBGImg(sohu.diyConsole.$BodyBGA.css("background-image"));
		p._fm.txtBGA.val(img).select();
		img=sohu.diyConsole.ParseBGImg(sohu.diyConsole.$BodyBGB.css("background-image"));
		p._fm.txtBGB.val(img);
		//高度
		var h=parseInt(sohu.diyConsole.$BodyBGA.css("height"));
		h=isNaN(h)?0:h;
		p._fm.txtBGHA.val(h);
		h=parseInt(sohu.diyConsole.$BodyBGB.css("height"));
		h=isNaN(h)?0:h;
		p._fm.txtBGHB.val(h);
		//页头
		//对齐方式
		var bg_p=sohu.diyConsole.$BodyBGA.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":(bg_p||"center top");
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlignA.val(bg_p[0]);
			p._fm.ddlBGAlignA.curVal=bg_p[0];
			p._fm.ddlBGAlignAV.val(bg_p[1]);
			p._fm.ddlBGAlignAV.curVal=bg_p[1];
		};
		
		//平铺方式
		var bg_a=sohu.diyConsole.$BodyBGA.css("backgroundRepeat");
		p._fm.ddlBGRepeatA.val(bg_a);
		p._fm.ddlBGRepeatA.curVal=bg_a;
		//页尾
		bg_p=sohu.diyConsole.$BodyBGB.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":(bg_p||"center top");
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlignB.val(bg_p[0]);
			p._fm.ddlBGAlignB.curVal=bg_p[0];
			p._fm.ddlBGAlignBV.val(bg_p[1]);
			p._fm.ddlBGAlignBV.curVal=bg_p[1];
		};		
		//平铺方式
		bg_a=sohu.diyConsole.$BodyBGB.css("backgroundRepeat");
		p._fm.ddlBGRepeatB.val(bg_a);
		p._fm.ddlBGRepeatB.curVal=bg_a;
		//背景色
		p._fm.txtBGC.css("backgroundColor",sohu.diyConsole.$Body.css("backgroundColor"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//事件注册
	//TODO:用全局的取色器
	p._fm.cpk.icolor({
		flat:true,
		onSelect:function(hex){
			sohu.diyConsole.$Body.css("backgroundColor",hex);
			p._fm.txtBGC.css("backgroundColor",hex);
			p._fm.cpk.hide();
		},
		showInput:true
	});
	p._fm.ddlBGAlignA.change(function(evt){
		p._fm.ddlBGAlignA.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignAV.change(function(evt){
		p._fm.ddlBGAlignAV.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignB.change(function(evt){
		p._fm.ddlBGAlignB.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignBV.change(function(evt){
		p._fm.ddlBGAlignBV.curVal=this.value;
		p.preview();
	});	
	p._fm.ddlBGRepeatA.change(function(evt){
		p._fm.ddlBGRepeatA.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGRepeatB.change(function(evt){
		p._fm.ddlBGRepeatB.curVal=this.value;
		p.preview();
	});		

	p._fm.txtBGHA.change(p.preview);
	p._fm.txtBGHB.change(p.preview);
	p._fm.txtBGA.change(p.preview);
	p._fm.txtBGB.change(p.preview);
	
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
sohu.diyDialog.wAddContent=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_addContent");
	this.$if=$("#vstp_ifContentList");
	//事件处理
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		sohu.diyConsole.toggleLoading();
		_this.$if.attr("src",_this.$if.attr("rel")+"?t="+new Date().getTime());
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"添加内容",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		hideActions:true
	};
};
/**
 * 分栏代码弹框
 * @param {Object} dlg
 */
sohu.diyDialog.wCode=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_wCode");
	this.$TextArea=this.$Layout.find("textarea");
	this.$tempWrap=$("<div/>");
	//事件处理
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};	
	p.afterShow=function(hash,dlg0){
		_this.$tempWrap.html(sohu.diyConsole.CurSec.$Layout.html()).find(".vstp_secHolder").remove();
		_this.$TextArea.val(_this.$tempWrap.html());
	};
	p.onOK=function(dlg0){
		sohu.diyDialog.doConfirm({
			text:'确定更新当前分栏的HTML代码么?',
			onOK:function($jqm){
				sohu.diyConsole.CurSec.$Layout.children().filter(":not(.vstp_secHolder)").remove();
				sohu.diyConsole.CurSec.$Layout.append(_this.$TextArea.val());
				//重新加载该分栏的内容，利用.html(x)更新内容时，dom已经不是原来的dom
				sohu.diyConsole.CurSec.LoadContents();
				$jqm.jqmHide();
				dlg0.Hide();
			}
		});
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"代码编辑",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * 分栏设置弹框
 * @param {Object} dlg
 */
sohu.diyDialog.wCfgSec=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_wCfgSec");
	//私有函数或变量
	p._fm={
		txtBG:$("#vstp_txtSecBG"),
		txtBGC:$("#vstp_txtSecBGC"),
		txtBorderC:$("#vstp_txtSecBorderColor"),
		cpk:this.$Layout.find(".vstp_cpk"),
		ddlBGAlign:$("#vstp_ddlSecBGAlign"),
		ddlBGAlignV:$("#vstp_ddlSecBGAlignV"),
		ddlBGRepeat:$("#vstp_ddlSecBGRepeat"),
		cbxBorder:this.$Layout.find(".cbx"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		var url=$.trim(p._fm.txtBG.val());
		if( (url!="") && (!StringUtils.isUrl(url)) ){
			p._fm.txtBG.addClass("vstp_alert").select();
			return false;
		};
		//更改图片属性
		if (url != "") {
			sohu.diyConsole.CurSec.$Layout.css("background-image", "url('" + url + "')");
		}else{
			sohu.diyConsole.CurSec.$Layout.css("background-image", "none");
		};
		//背景对齐
		sohu.diyConsole.CurSec.$Layout.css("background-position",p._fm.ddlBGAlign.curVal+" "+p._fm.ddlBGAlignV.curVal);
		//背景平铺
		sohu.diyConsole.CurSec.$Layout.css("background-repeat",p._fm.ddlBGRepeat.curVal);
		//收起对话框
		if(cls)
			dlg0.Hide();			
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	p.afterShow=function(hash,dlg0){
		//背景图
		var bgimg=sohu.diyConsole.CurSec.$Layout.css("background-image");
		bgimg=bgimg=="none"?"":bgimg;
		bgimg=bgimg.replace('url("',"").replace('")',"");
		p._fm.txtBG.val(bgimg);
		//背景色
		p._fm.txtBGC.val("").css("backgroundColor",sohu.diyConsole.CurSec.$Layout.css("backgroundColor"));
		//边框色
		var bdc=sohu.diyConsole.CurSec.$Layout.css("borderColor");
		p._fm.txtBorderC.css("backgroundColor",bdc);
		//对齐方式
		var bg_p=sohu.diyConsole.CurSec.$Layout.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":(bg_p||"center top");
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlign.val(bg_p[0]);
			p._fm.ddlBGAlign.curVal=bg_p[0];
			p._fm.ddlBGAlignV.val(bg_p[1]);
			p._fm.ddlBGAlignV.curVal=bg_p[1];
		};
		//平铺方式
		var bg_a=sohu.diyConsole.CurSec.$Layout.css("backgroundRepeat");
		p._fm.ddlBGRepeat.val(bg_a);
		//边框-根据边框色判断边框的有无
		bdc=sohu.diyConsole.GetBorderColor(bdc);
		if(!bdc){
			p._fm.cbxBorder.attr("checked",false);
		}else{
			for(var x in bdc){
				if(x)
					p._fm.cbxBorder.filter("[value='"+x+"']")[0].checked=(bdc[x]!="transparent"&&bdc[x]!="none");
			};//for
		};//if			
	};
	//事件注册
	//背景色
	p._fm.txtBGC
	.css("backgroundColor","transparent")
	.click(function(evt){
		p._fm.cpk.$t=p._fm.txtBGC;
		p._fm.cpk.flag="bg";
		p._fm.cpk.show();
	});
	//边框色
	p._fm.txtBorderC
	.click(function(evt){
		p._fm.cpk.$t=p._fm.txtBorderC;
		p._fm.cpk.flag="bdc";
		p._fm.cpk.show();
	});
	//边框方向-自定义事件的精妙之处-参考行722和762
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
	//背景图
	p._fm.txtBG.change(p.preview);
	//背景图对齐方式
	p._fm.ddlBGAlign.change(function(evt){
		p._fm.ddlBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignV.change(function(evt){
		p._fm.ddlBGAlignV.curVal=this.value;
		p.preview();
	});	
	//背景图平铺方式
	p._fm.ddlBGRepeat.click(function(evt){
		p._fm.ddlBGRepeat.curVal=this.value;
		p.preview();
	});
	//栏目背景色
	//TODO:用全局取色器
	p._fm.cpk.icolor({
		flat:true,
		onSelect:function(c){
			p._fm.cpk.$t.css("backgroundColor",c).attr("title",c);
			if(p._fm.cpk.flag=="bg"){
				sohu.diyConsole.CurSec.$Layout.css("backgroundColor",c);
			}else{
				p._fm.cbxBorder.trigger("evtClick");
			};
			p._fm.cpk.hide();
		},
		showInput:true
	});			
	//jqm options
	this.$Layout.jqmOpts={
		title:"分栏设置",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * 栏目头设置弹框
 * @param {Object} dlg
 */
sohu.diyDialog.wSecHead=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_wSecHead");
	p._fm={
		bg:$("#vstp_secHDBG"),
		cbxMore:$("#vstp_cbxSecHDMore"),
		link:$("#vstp_secHDLink"),
		ddlTarget:$("#vstp_ddlSecHDTarget"),
		title:$("#vstp_secHDTitle"),
		tipLink:this.$Layout.find(".vstp_tipSecHDLink"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
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
		//背景图
		var bg=sohu.diyConsole.CurElm.CT.$Layout.css("background-image");
		bg=bg=="none"?"":bg;
		bg=bg.replace('url("',"").replace('")',"");
		p._fm.bg.val(bg);
		//是否显示更多
		if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length>0){
			p._fm.cbxMore.attr("checked",true);
		}else{
			p._fm.cbxMore.attr("checked",false);
		};
		//标题连接
		var $a=sohu.diyConsole.CurElm.CT.$Layout.find("a[href!='']");
		if($a.length>0){
			p._fm.link.val($a[0].href);
		};
		//捕捉iframe编辑器用户选定的内容
		sohu.diyConsole.CurElm.i$frame[0].i$Body().unbind("mouseup").mouseup(p.onIframeSelect);		
	};
	p.onOK=function(dlg0){
		var url=$.trim(p._fm.link.val());
		if((url!="")&&!StringUtils.isUrl(url)){
			p._fm.link.addClass("vstp_alert").select();
			p._fm.tipLink.addClass("vstp_alert").show();
			return false;
		};
		p._fm.link.removeClass("vstp_alert");
		p._fm.tipLink.removeClass("vstp_alert").hide();
		//背景图
		var img=$.trim(p._fm.bg.val());
		img=img==""?"none":img;
		sohu.diyConsole.CurElm.CT.$Layout.css("background-image",'url("'+img+'")');	
		
		//显示更多
		if(p._fm.cbxMore[0].checked){
			if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length==0){
				var $more=$('<strong class="vstp_elm more">更多&gt;&gt;</strong>');
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
			if(url!=""){
				p._fm.a.$obj.attr("href",url);
			}else{
				p._fm.a.$obj.attr("href","#");
			};
		};

		dlg0.Hide();
	};	
	//事件注册
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('‘',"").replace('“',"").replace('”',"");
	});
	//jqm options
	this.$Layout.jqmOpts={
		title:"栏目标题",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK		
	};
};
/**
 * 添加分栏弹框
 * @param {Object} dlg
 * @param {Object} parentSize 父分栏的宽度
 */
sohu.diyDialog.wSubSec=function(dlg,parentSize){
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_subSec"+parentSize);
	//事件处理
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};	
	//事件注册
	this.$Layout.find('.vstp_item').click(function(evt){
		if(!sohu.diyConsole.CurSec) return;
		//sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
		sohu.diyConsole.CurSec.AddSub($(this).find(".vstp_subsec").clone());
		dlg.Hide();
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
	//jqm options
	this.$Layout.jqmOpts={
		title:"添加分栏",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		hideActions:true
	};
};
/**
 * 文字编辑弹框
 * @param {Object} dlg
 */
sohu.diyDialog.wText=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_txtMsg");
	this.$ElmCAction=this.$Layout.find(".vstp_elmcActs");
	p._fm={
		txtATitle:$("#vstp_txtATitle"),
		txtAHref:$("#vstp_txtAHref"),
		ddlATarget:$("#vstp_ddlATarget"),
		tipAHref:this.$Layout.find(".vstp_tipAHref"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
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
		//判断是否是可复制元素
		if(sohu.diyConsole.CurElm.Copyable){
			_this.$ElmCAction.show();
		}else{
			_this.$ElmCAction.hide();
		};
		//捕捉iframe编辑器用户选定的内容
		sohu.diyConsole.CurElm.i$frame[0].i$Body().unbind("mouseup").mouseup(p.onIframeSelect);
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0){
		var url=p._fm.txtAHref.val();
		if(url!=""&&!StringUtils.isUrl(url)){
			p._fm.txtAHref.addClass("vstp_alert").select();
			p._fm.tipAHref.addClass("vstp_alert").show();
			return false;
		};
		p._fm.txtAHref.removeClass("vstp_alert");
		p._fm.tipAHref.removeClass("vstp_alert").hide();	
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
	//事件注册
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('‘',"").replace('“',"").replace('”',"");
	});
	p._fm.ddlATarget.change(function(evt){
		p._fm.ddlATarget.curVal=this.value;
	});
	p._fm.ddlATarget.curVal="_blank";
	//jqm options
	this.$Layout.jqmOpts={
		title:"文字",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * 图片编辑弹框
 * @param {Object} dlg
 */
sohu.diyDialog.wImage=function(dlg){
	var _this=this;
	var p={};
	//DOM引用
	this.$Layout=$("#vstp_picMsg");
	this.$ElmCAction=this.$Layout.find(".vstp_elmcActs");
	p._fm={
		h:$("#vstp_picH"),
		w:$("#vstp_picW"),
		bcolor:$("#vstp_picBColor"),
		src:$("#vstp_picSrc"),
		btnSwitch:$("#vstp_picSwitch"),
		link:$("#vstp_picLink"),
		tipLink:this.$Layout.find(".vstp_tipPicLink"),
		ddlTarget:$("#vstp_picTarget"),
		title:$("#vstp_picTitle"),
		alt:$("#vstp_picAlt"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");		
		return true;
	};	
	p.afterShow=function(hash,dlg0){
		//dlg0.$Layout.css("top",25);
		//判断是否是可复制元素
		if(sohu.diyConsole.CurElm.Copyable){
			_this.$ElmCAction.show();
		}else{
			_this.$ElmCAction.hide();
		};
		var $img=sohu.diyConsole.CurElm.$Layout;
		//高宽
		var h=$img.attr("height");
		var w=$img.attr("width");
		p._fm.h.val(h);
		p._fm.w.val(w);
		//边框色
		p._fm.bcolor.css("backgroundColor",$img.css("backgroundColor"));
		//src
		p._fm.src.val($img.attr("src"));
		//链接
		var $a=$img.parent("a");
		$a=$a.length>0?$a:$({href:"",title:""});
		var href=$a.attr("href");
		href=href=="#"?"":href;
		p._fm.link.val(href);
		//图片标题
		p._fm.title.val($img.attr("title"));
		//替代文字
		p._fm.alt.val($img.attr("alt"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0){
		//图片地址
		var src=p._fm.src.val();
		/*
		if(src!=""&&!StringUtils.isUrl(src)){
			p._fm.src.addClass("alert").select();
			return false;
		};
		p._fm.src.removeClass("alert");
		*/
		//链接
		var link=p._fm.link.val();
		if(link!=""&&!StringUtils.isUrl(link)){
			p._fm.link.addClass("vstp_alert").select();
			p._fm.tipLink.addClass("vstp_alert").show();
			return false;
		};
		p._fm.link.removeClass("vstp_alert");
		p._fm.tipLink.removeClass("vstp_alert").hide();
		//高宽
		var fine=true,v=["",""];
		$.each([p._fm.h,p._fm.w],function(i,o){
			var v0=o.val();
			v[i]=v0;
			if((v0!="")&&(!StringUtils.isPlusInt(v0))){
				o.addClass("vstp_alert");
				fine=fine&&false;
			};//if
		});//each
		if(!fine){
			return false;
		};
		if(v[0]==""){
			sohu.diyConsole.CurElm.$Layout.removeAttr("height");	
		}else{
			sohu.diyConsole.CurElm.$Layout.attr("height",v[0]);
		};
		if(v[1]==""){
			sohu.diyConsole.CurElm.$Layout.removeAttr("width");	
		}else{
			sohu.diyConsole.CurElm.$Layout.attr("width",v[1]);
		};		
		//边框色
		var c=p._fm.bcolor.curVal;
		//设置图片样式
		sohu.diyConsole.CurElm.$Layout.css({
			"border-color":c
		}).attr("src",src).attr("title",p._fm.title.val()).attr("alt",p._fm.alt.val());
		//设置链接
		var a=sohu.diyConsole.CurElm.$Layout.parent("a");
		if(a.length>0){
			if(link!=""){
				a.attr("target",p._fm.ddlTarget.val()).attr("href",link);
			};	
		};
		dlg0.Hide();
	};
	//事件注册
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('‘',"").replace('“',"").replace('”',"");
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
		title:"图像",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * 页面属性
 * @param {Object} dlg
 */
sohu.diyDialog.wPagePro=function(dlg){
	var p={},_this=this;
	//DOM引用
	this.$Layout=$("#vstp_wPagePro");
	//事件处理
	p._fm={
		title:$("#vstp_pTitle"),
		kwd:$("#vstp_pKeywords"),
		desc:$("#vstp_pDesc")
	};
	p.afterShow=function(hash,dlg0){
		p._fm.title.val($("title").html());
	};
	p.onOK=function(dlg0){
		dlg0.Hide();
	};
	//事件注册
	//jqm options
	this.$Layout.jqmOpts={
		title:"页面属性",
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * 页面风格
 * @param {Object} dlg
 */
sohu.diyDialog.wTheme=function(dlg){
	var p={},_this=this;
	//DOM引用
	this.$Layout=$("#vstp_wTheme");
	this.$list=this.$Layout.find(".vstp_listTopicTpl");
	//初始化
	$("#vstp_topicTpl").tmpl(sohu.diyTp.TopicTpl).appendTo(this.$list);
	this.$Layout.find("a").click(sohu.diyConsole.OnStopNav);
	this.$list.find("a").click(function(){
		var n=this.name;
		//注入当前模板
		sohu.diyConsole.$Workspace.load(this.rel,function(d){
			sohu.diyConsole.$Workspace.empty().append($(d).filter(":not(.vstp_exclude,title,meta)"));
			sohu.diyConsole.$Workspace.find("link,style").appendTo("head");
			//初始化编辑器
			//$("#vstp_jsInit").remove().appendTo("body");
			bos.Fire();
			dlg.Hide();
		});
	});
	//事件处理
	p.afterShow=function(hash,dlg0){
		dlg0.$Acts.hide();
	};
	p.onOK=function(dlg0){
		//注入当前模板
		dlg0.Hide();
	};
	//事件注册
	//jqm options
	this.$Layout.jqmOpts={
		title:"更换风格",
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * 左侧隐藏设置菜单
 * @param {Object} dlg
 */
sohu.diyDialog.wSetting1=function(dlg){
	var p={},_this=this;
	//DOM引用
	this.$Layout=$("#vstp_wSetting1");
	this.$Body=this.$Layout.children();
	this.IsOpen=true;
	//事件处理
	p.onTheme=function(evt){
		if(sohu.diyConsole.IsPreview) return false;
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("wTheme");
	};
	p.onPagePro=function(evt){
		if(sohu.diyConsole.IsPreview) return false;
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("wPagePro");
	};
	p.onPageBG=function(evt){
		//if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		if(sohu.diyConsole.IsPreview) return false;
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("cfgPage");
	};	
	p.onPreview=function(evt){
		var $b=$("body");
		if (sohu.diyConsole.IsPreview) {
			$(this).find("strong").html("预览");
			$b.removeClass("vstp_preview");
			sohu.diyConsole.UnPreview();
		}else{
			sohu.diyConsole.Preview(true);
			if(sohu.diyConsole.IsPreview){
				$b.addClass("vstp_preview");
				$(this).find("strong").html("关闭预览");				
			};	
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
			_this.$Body.addClass("vstp_close");
			_this.$Layout.animate({left:-82},"normal");
			_this.IsOpen=false;
		}else{
			_this.$Body.removeClass("vstp_close");
			_this.$Layout.animate({left:-1},"normal");
			_this.IsOpen=true;
		};
		return false;
	};
	//事件注册
	this.$Layout.find("a").click(function(evt){return false;});
	$("#vstp_wsTheme").click(p.onTheme);
	$("#vstp_wsPagePro").click(p.onPagePro);
	$("#vstp_wsPageBG").click(p.onPageBG);
	$("#vstp_wsPreview").click(p.onPreview);
	$("#vstp_wsPublish").click(p.onPublish);
	$("#vstp_wsHelp").click(p.onHelp);
	this.$Layout.find(".ctr").click(p.onToggle);
	
};
/**
 * FLASH编辑框
 * @param {Object} dlg
 */
sohu.diyDialog.wFlash=function(dlg){
	var p={},_this=this;
	//DOM引用
	this.$Layout=$("#vstp_wFlash");
	this.$t=this.$Layout.find('.vstp_cfg');
	
	p._fm={
		w:$("#vstp_txtWFlash"),
		h:$("#vstp_txtHFlash"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//事件处理
	p.onChangeImg=function(evt){
		$(this).parents('.vstp_cfg').find('.vstp_thumb').attr('src',this.value);
		return false;
	};
	p.onAdd=function(evt){
		var t0=$(this).parents('.vstp_cfg');
		var o=t0.clone(true);
		o.find(".vstp_thumb").attr("src","").end()
			.find(".vstp_flashImg").val("").end()
			.find(".vstp_flashTxt").val("").end()
			.find(".vstp_flashHref").val("");
		t0.after(o);
		return false;
	};
	p.onDel=function(evt){
		$(this).parents('.vstp_cfg').remove();
		return false;
	};
	p.onUp=function(evt){
		var t0=$(this).parents('.vstp_cfg');
		var t1=t0.prev(".vstp_cfg1");
		if(t1.length==0) return false;
		t1.before(t0);
		return false;
	};
	p.onDown=function(evt){
		var t0=$(this).parents('.vstp_cfg');
		var t1=t0.next(".vstp_cfg1");
		if(t1.length==0) return false;
		t1.after(t0);
		return false;
	};
	p.afterShow=function(hash,dlg0){
		_this.$Layout.find('.vstp_cfg1').remove();
		p._fm.w.val(sohu.diyConsole.CurCT.FlashData.w);
		p._fm.h.val(sohu.diyConsole.CurCT.FlashData.h);
		//图片列表
		p._pics=sohu.diyConsole.CurCT.FlashData.v.pics.split('|');
		p._links=sohu.diyConsole.CurCT.FlashData.v.links.split('|');
		p._texts=sohu.diyConsole.CurCT.FlashData.v.texts.split('|');
		$.each(p._pics,function(i,o){
			var $t0=_this.$t.clone(true).addClass('vstp_cfg1');
			$t0.find('.vstp_flashImg').val(o).end()
				.find('.vstp_flashTxt').val(p._texts[i]).end()
				.find('.vstp_flashHref').val(p._links[i]).end()
				.find('.vstp_thumb').attr('src',o).end()
				.appendTo(_this.$Layout);	
		});
	};
	p.afterHide=function(hash,dlg0){
		sohu.diyConsole.CurCT.InlineEdit('off');
	};
	p.onOK=function(dlg0){
		//验证
		var h=p._fm.h.val(),w=p._fm.w.val();
		if(!StringUtils.isPlusInt(h)){
			p._fm.h.addClass("vstp_alert");
			return false;
		};
		if(!StringUtils.isPlusInt(w)){
			p._fm.w.addClass("vstp_alert");
			return false;
		};
		
		p._pics=_this.$Layout.find('.vstp_cfg1 .vstp_flashImg').map(function(){
			return this.value;
		}).get().join("|");
		p._links=_this.$Layout.find('.vstp_cfg1 .vstp_flashHref').map(function(){
			return this.value;
		}).get().join("|");
		p._texts=_this.$Layout.find('.vstp_cfg1 .vstp_flashTxt').map(function(){
			return this.value;
		}).get().join("|");
		
		p._fm.reset();		
		//更新flash
		sohu.diyConsole.CurCT.FlashData.v.pic_width=w;
		sohu.diyConsole.CurCT.FlashData.v.pic_height=h;
		sohu.diyConsole.CurCT.FlashData.w=w;
		sohu.diyConsole.CurCT.FlashData.h=h;
		if(sohu.diyConsole.CurCT.FlashData.v.show_text){
			sohu.diyConsole.CurCT.FlashData.h=parseInt(h)+20;	
		};
		
		sohu.diyConsole.CurCT.FlashData.v.pics=p._pics;
		sohu.diyConsole.CurCT.FlashData.v.links=p._links;
		sohu.diyConsole.CurCT.FlashData.v.texts=p._texts;
		
		sohu.diyConsole.CurCT.$FlashData.html($.toJSON(sohu.diyConsole.CurCT.FlashData));
		sohu.diyConsole.CurCT.FlashObj.setAttribute("width",w);
		sohu.diyConsole.CurCT.FlashObj.setAttribute("height",sohu.diyConsole.CurCT.FlashData.h);
		
		sohu.diyConsole.CurCT.FlashObj.variables["pics"]=p._pics;
		sohu.diyConsole.CurCT.FlashObj.variables["links"]=p._links;
		sohu.diyConsole.CurCT.FlashObj.variables["texts"]=p._texts;
		
		if(sohu.diyConsole.CurCT.FlashData.v.pic_width){
			sohu.diyConsole.CurCT.FlashObj.variables["pic_width"]=w;
			sohu.diyConsole.CurCT.FlashObj.variables["pic_height"]=h;
		};
		
		sohu.diyConsole.CurCT.FlashObj.write(sohu.diyConsole.CurCT.FlashData.pid);
		dlg0.Hide();
	};
	//事件注册
	this.$t.find('.vstp_flashImg').change(p.onChangeImg);
	this.$t.find('.vstp_add').click(p.onAdd);
	this.$t.find('.vstp_del').click(p.onDel);
	this.$t.find('.vstp_up').click(p.onUp);
	this.$t.find('.vstp_down').click(p.onDown);
	//jqm options
	this.$Layout.jqmOpts={
		title:"焦点图",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * 横切工具条
 * @param {Object} dlg
 */
sohu.diyDialog.wAreaTool=function(dlg){
	var p={};
	//DOM引用
	p.$layout=$("#vstp_areaTools");
	p._$btnAdd=$("#vstp_lnkAreaAdd");
	p._$btnDel=$("#vstp_lnkAreaDel");
	p._$btnBG=$("#vstp_lnkAreaBG");
	p._$btnDown=$("#vstp_lnkAreaDown");
	p._$btnUp=$("#vstp_lnkAreaUp");
	
	//事件处理
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
			sohu.diyDialog.doAlert({text:'未选中任何横切!'});
			return false;
		};
		dlg.Console.Areas=$.grep(dlg.Console.Areas,function(o,i){
			if(o.ID==dlg.Console.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
	};
	p.onCfgBG=function(evt){
		if(!sohu.diyConsole.CurArea){sohu.diyDialog.doAlert({text:'未选中任何横切!'});return false;};
		if(sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("cfgArea");
	};
	p.onMove=function(evt){
		if(!sohu.diyConsole.CurArea){sohu.diyDialog.doAlert({text:'未选中任何横切!'});return false;};
		if(sohu.diyConsole.CurArea.IsEditing) return false;	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		p.onReposition();
	};
	//事件注册
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
 * 横切助手重定位
 */
sohu.diyDialog.wAreaTool.Reposition=function(){
	var dlg=sohu.diyDialog.Get("areaTools");
	if(!dlg) return;
	dlg.RePosition();
};
/**
 * 注册弹框实体
 * @param {Object} dlg
 */
sohu.diyDialog.onInit=function(dlg){
	new sohu.diyDialog.wSetting1(dlg);
	sohu.diyDialog.Register("addBlock",new sohu.diyDialog.wArea(dlg));
	sohu.diyDialog.Register("areaTools",new sohu.diyDialog.wAreaTool(dlg));
	sohu.diyDialog.Register("cfgArea",new sohu.diyDialog.wCfgArea(dlg));
	sohu.diyDialog.Register("cfgPage",new sohu.diyDialog.wCfgPage(dlg));
	sohu.diyDialog.Register("addContent",new sohu.diyDialog.wAddContent(dlg));
	sohu.diyDialog.Register("code",new sohu.diyDialog.wCode(dlg));
	sohu.diyDialog.Register("cfgSec",new sohu.diyDialog.wCfgSec(dlg));
	sohu.diyDialog.Register("subSec190",new sohu.diyDialog.wSubSec(dlg,190));
	sohu.diyDialog.Register("subSec230",new sohu.diyDialog.wSubSec(dlg,230));
	sohu.diyDialog.Register("subSec270",new sohu.diyDialog.wSubSec(dlg,270));
	sohu.diyDialog.Register("subSec310",new sohu.diyDialog.wSubSec(dlg,310));
	sohu.diyDialog.Register("subSec390",new sohu.diyDialog.wSubSec(dlg,390));
	sohu.diyDialog.Register("subSec430",new sohu.diyDialog.wSubSec(dlg,430));
	sohu.diyDialog.Register("subSec470",new sohu.diyDialog.wSubSec(dlg,470));
	sohu.diyDialog.Register("subSec510",new sohu.diyDialog.wSubSec(dlg,510));
	sohu.diyDialog.Register("subSec670",new sohu.diyDialog.wSubSec(dlg,670));
	sohu.diyDialog.Register("subSec950",new sohu.diyDialog.wSubSec(dlg,950));
	sohu.diyDialog.Register("wText",new sohu.diyDialog.wText(dlg));
	sohu.diyDialog.Register("wImage",new sohu.diyDialog.wImage(dlg));
	sohu.diyDialog.Register("wSecHead",new sohu.diyDialog.wSecHead(dlg));	
	sohu.diyDialog.Register("wPagePro",new sohu.diyDialog.wPagePro(dlg));
	sohu.diyDialog.Register("wTheme",new sohu.diyDialog.wTheme(dlg));
	sohu.diyDialog.Register("wFlash",new sohu.diyDialog.wFlash(dlg));
};
