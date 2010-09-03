/**
 * 搜狐cms5内容模板类定义
 */
sohu.diyTplFactory={
	/**
	 * 关闭内容选择对话框
	 */
	cls:function(){
		parent.bos.CloseCTDialog();
	},
	/**
	 * 提交内容对象
	 * @param {Object} ct 内容对象,如{isNew:true,flash:false,html0:''}
	 * @param {Boolean} cls是否关闭对话框
	 */
	submit:function(ct,cls){
		sohu.diyCTFactory.CurCT=ct;
		parent.sohu.diyConsole.CurSec.AddContent(ct);
		if (cls) {
			sohu.diyTplFactory.cls();
		};
	},
	r:function(evt){return false;},
	/**
	 * 获取class属性的某个值。注意内容模板的class属性的第一个值表示该模板的类型
	 * @param {Object} $dom 内容模板
	 */
	getClass:function($dom,idx){
		idx=idx||0;
		var items=$dom.attr("class").split(" ");
		items=$.grep(items,function(o,i){
			if($.trim(o)=="") return false;
			return true;
		});
		if(idx<0) return items;
		idx=idx>=items.length?(items.length-1):idx;
		return items[idx];
	}
};
/**
 * 导航条设置类
 * @param {Object} opts
 */
sohu.diyTplFactory.Nav=function(opts){
	var _this=this;
	this.$Layout=$("#ctNav");
	this.CurNav=null;
	//事件注册
	$("#navList li").click(function(evt){
		_this.CurNav=$(this).find(".ctWrap").html();
		_this.Submit();
	});
};
sohu.diyTplFactory.Nav.prototype.Submit=function(){
	var ct={};
	ct.html0=this.CurNav;
	ct.flash=false;
	ct.isNew=true;
	ct.type="nav";
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 栏目标题设置类
 * @param {Object} opts
 */
sohu.diyTplFactory.SecHead=function(opts){
	var _this=this;
	//属性-dom引用
	this.$Layout=$("#ctSecHead");
	//私有成员
	//事件注册
	$("#secHeadList li").click(function(evt){
		_this.CurHtml=$(this).find(".ctWrap").html();
		_this.Submit();
	});
};
sohu.diyTplFactory.SecHead.prototype.Submit=function(opts){
	var ct={
		flash:false,
		html0:this.CurHtml,
		isNew:true,
		type:"sec_hd"
	};
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 空行的设置类
 */
sohu.diyTplFactory.Line=function(opts){
	var _this=this;
	//属性定义-用户设置
	this.layoutID=opts.layoutID||"ctEmptyLine";//模板编号
	this.css_cpk=opts.css_cpk||".cpk";//cpk css选择器
	this.css_txtH=opts.css_txtH||".txtH";//高度录入框 css选择器
	this.css_txtW=opts.css_txtW||".txtW";//宽度录入框
	this.css_btnOK=opts.css_btnOK||".btnOK";
	this.css_cbkLine=opts.css_cbkLine||".cbxLine";
	this.color=opts.color||"#808080";
	this.height=opts.height||10;
	this.width=opts.width||96;/* 高度％ */
	this.cLine=opts.cLine||false;//是否中间显示分割线
	//属性定义-dom引用
	this.html=null;//模板html
	this.$layoutCfg=null;//设置界面dom
	this.$txtCpk=null;//dom-颜色选择器colorpicker
	this.$txtHeight=null;//dom-高度输入框
	this.$txtWidth=null;/* input $obj for width */
	this.$btnOK=null;//提交按钮
	this.$cbkLine=null;//显示虚线复选框
	
	//私有成员
	var p={};
	p.initLayout=function(){
		_this.html=sohu.diyTp[_this.layoutID];
		_this.$layoutCfg=$("#"+_this.layoutID);
		//颜色录入框
		_this.$txtCpk=_this.$layoutCfg.find(_this.css_cpk).ColorPicker({
			color:_this.color,
			onShow:function(cpk){
				$(cpk).fadeIn(500);return false;
			},
			onHide:function(cpk){
				$(cpk).fadeOut(500); return false;
			},
			onChange:function(hsb,hex,rgb){
				_this.color="#"+hex;
				_this.$txtCpk.css("backgroundColor",_this.color).val(_this.color);
			}
		});
		//高度录入框
		_this.$txtHeight=_this.$layoutCfg.find(_this.css_txtH).keyup(function(evt){
			if(!StringUtils.isPlusInt(this.value)){
				this.value="10";
				this.select();
			};

		}).blur(function(evt){
			_this.height=parseInt(this.value);
			_this.height=isNaN(_this.height)?10:_this.height;
			_this.height=(_this.height>1000||_this.height<1)?10:_this.height;
			this.value=_this.height;
		});
		/* input for width */
		_this.$txtWidth=_this.$layoutCfg.find(_this.css_txtW).keyup(function(evt){
			/* handler for keyup evt */
			if(!StringUtils.isPlusInt(this.value)){
				this.value="100";
				this.select();
			}
			
		}).blur(function(evt){
			/* handler for blur evt */
			//_this.width=parseInt(this.value);
			//_this.width=isNaN(_this.width)?100:_this.width;
			_this.width=isNaN(_this.width=parseInt(this.value))?100:_this.width;
			_this.width=(_this.width>100||_this.width<10)?100:_this.width;
			this.value=_this.width;
		});
		//确定按钮
		_this.$btnOK=_this.$layoutCfg.find(_this.css_btnOK).click(function(evt){
			_this.Submit({});return false;
		});
		//虚线复选框
		_this.$cbkLine=_this.$layoutCfg.find(_this.css_cbkLine);
	};
	p.initLayout();
};
sohu.diyTplFactory.Line.prototype.Submit=function(opt){
	this.cLine=this.$cbkLine[0].checked;
	sohu.diyCTFactory.$ctWrap.html(this.html);
	var hr=sohu.diyCTFactory.$ctWrap.find("hr");
	var line=sohu.diyCTFactory.$ctWrap.find(".vspace");
	if(this.cLine){
		var css={
			"border-color":this.color,
			"width":this.width+"%",
			"left":(100-this.width)/2+"%"
		};
		hr.css(css);
		
	}else{
		hr.remove();
	};
	var css={"height":this.height+'px'};
	line.css("height",this.height+'px');
	
	var ct={
		flash:false,
		html0:sohu.diyCTFactory.$ctWrap.html(),
		isNew:true,
		type:'vspace'
	};
	
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 焦点图选择类
 */
sohu.diyTplFactory.FocusImg=function(opts){
		var _this=this;
		//焦点图选择
		this.$Layout=$("#focusImgSlide").abcdSlider({
			showNum:1,
			total:2,
			externalBtn:true,
			cssBtnL:'#ctFocusImg .btnL',
			cssBtnR:'#ctFocusImg .btnR',
			cssPanel:'#cSlide_focusImg',
			step:540,
			onSlide:function($dom,mode){
				_this.$btnDots.removeClass("currA").eq($dom._p.slideNum).addClass("currA");
			}
		});
		this.tplID=null;//flash 模板号
		this.$btnDots=$("#ctImg .btnDot").click(function(evt){return false;});
	
		var p={};
		p.onAddFlash=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find('.ctWrap').clone();
			_this.$jsonData=_this.tplObj.find('.flashData');
			_this.flashData=$.evalJSON(_this.$jsonData.html());
			_this.flashData.dummy=false;
			_this.$jsonData.html($.toJSON(_this.flashData));
			_this.Submit();
			return false;
		};
		//鼠标事件
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddFlash);
	
};
sohu.diyTplFactory.FocusImg.prototype.Submit=function(opt){
	var ct={};
	var id='flash'+StringUtils.RdStr(8);
	ct.html0=this.tplObj.html().replace(/FLASHID/g,id);
	ct.flash=true;
	/*
	ct.tplID=this.tplID;
	*/
	ct.isNew=true;
	ct.type='flash';
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 图片选择类
 */
sohu.diyTplFactory.Image=function(opts){
		var _this=this;
		//焦点图选择
		this.$Layout=$("#ctImgSlide").abcdSlider({
			step:488,
			showNum:1,
			total:2,
			externalBtn:true,
			cssBtnL:'#ctImg .btnL',
			cssBtnR:'#ctImg .btnR',
			cssPanel:'#ctImgSlider',
			onSlide:function($dom,mode){
				_this.$btnDots.removeClass("currA").eq($dom._p.slideNum).addClass("currA");
			}
		});
				
		//组图设置弹框
		this.$btnDots=$("#ctImg .btnDot").click(function(evt){return false;});
		this.$WinCfg=$("#cfgImgList");
		this.$WinCfg.ovl=$("#cfgImgList_ovl");
		this.$WinCfg.$imgWidth=$("#txtImgWidth");
		this.$WinCfg.$imgCol=$("#ddlImgListCol");
		this.$WinCfg.$tip=this.$WinCfg.find(".tip");
		this.$WinCfg.$tip1=this.$WinCfg.find(".tip1");
		this.tplID=null;//模板号
		this.IsImgList=false;//是否组图
		
		var p={};
		p.onAddImg=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.CurColSize=parseInt(parent.sohu.diyConsole.CurSec.Width);
			_this.CurSecSize=parent.sohu.diyConsole.CurSec.Dim().w;
			//如果是组图，弹出组图设置框
			_this.IsImgList=_this.tplObj.find(".ct").hasClass("pp");
			if(_this.IsImgList){
				_this.$WinCfg.ovl.css("opacity","0.7").show();
				_this.$WinCfg.$tip.html(_this.CurColSize);
				_this.$WinCfg.$tip1.html(_this.CurSecSize-10-6);
				_this.$WinCfg.slideDown();
				_this.$WinCfg.$imgCol.trigger("change");
				_this.$WinCfg.$imgWidth.removeClass("alert");
				_this.$WinCfg.$tip1.removeClass("alert");
				return false;
			};
			_this.Submit();
			return false;
		};
		p.closePPCfg=function(evt){
			_this.$WinCfg.hide();
			_this.$WinCfg.ovl.hide();
			return false;
		};
		p.buildPP=function(evt){
			if(_this.$WinCfg.$imgWidth.hasClass("alert")){
				_this.$WinCfg.$imgWidth.select();
				return false;
			};
			var col=_this.$WinCfg.$imgCol.val();
			var w=_this.$WinCfg.$imgWidth.val();
			
			var ct0=$(_this.tplObj.html());
			var li0=ct0.find("li:first"),li1=null;
			var ul0=ct0.find("ul");
			ul0.addClass("c"+col).empty();
			for(var i=0;i<col;i++){
				li1=li0.clone();
				li1.find("img").attr("width",w);
				ul0.append(li1);
			};
			_this.tplObj=$("<div/>").append(ct0);
			
			_this.Submit();
			
			return false;
		};
		p.setPPCol=function(evt){
			_this.$WinCfg.$imgCol.curVal=this.value;
			var w=Math.floor(_this.CurSecSize/this.value-10-6);
			_this.$WinCfg.$imgWidth.removeClass("alert").val(w).effect("highlight");
		};
		p.setPPWidth=function(evt){
			var v=this.value;
			if(!StringUtils.isPlusInt(v)){
				_this.$WinCfg.$imgWidth.addClass("alert");
				_this.$WinCfg.$tip1.addClass("alert");
				//_this.$WinCfg.$imgCol.trigger("change");
				_this.$WinCfg.$imgWidth.select();
				return false;
			};
			//根据图片宽度推算列数
			v=parseInt(v);
			var col=Math.floor(_this.CurSecSize/(v+10+6));
			if(col==0){
				_this.$WinCfg.$imgWidth.addClass("alert");
				_this.$WinCfg.$tip1.addClass("alert").effect("highlight");				
				return false;
			};
			_this.$WinCfg.$imgWidth.removeClass("alert");
			_this.$WinCfg.$tip1.removeClass("alert");
			
			col=col>6?6:col;
			if(_this.$WinCfg.$imgCol.curVal>col){
				_this.$WinCfg.$imgCol.val(col);
				_this.$WinCfg.$imgCol.curVal=col;
			};
			
		};
		//鼠标事件
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddImg);
		this.$WinCfg.find(".btnOK").click(p.buildPP);
		this.$WinCfg.find(".btnNO").click(p.closePPCfg);
		this.$WinCfg.$imgCol.change(p.setPPCol);
		this.$WinCfg.$imgWidth.change(p.setPPWidth);
		//a标签
		//this.$Layout.find("a").click(sohu.diyTplFactory.r);
	
};
sohu.diyTplFactory.Image.prototype.Submit=function(opt){
	var ct={};
	ct.html0=this.tplObj.html();
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type=sohu.diyTplFactory.getClass(this.tplObj.children());
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 文本选择类
 */
sohu.diyTplFactory.Text=function(opts){
		//文本选择
		this.$Layout=$("#cSlide_text");
		this.tplID=null;//模板号
		var _this=this;
		var p={};
		p.onAddText=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.Submit();
			return false;
		};
		//鼠标事件
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddText);
		//a标签
		//this.$Layout.find("a").click(sohu.diyTplFactory.r);
		$("#ctText .navBtn").click(sohu.diyTplFactory.r);	
};
sohu.diyTplFactory.Text.prototype.Submit=function(opt){
	var ct={html0:this.tplObj.html()};
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type=sohu.diyTplFactory.getClass(this.tplObj.children());
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 自动列表类
 * @param {Object} opts
 */
sohu.diyTplFactory.DynamicList=function(opts){
	var _this=this;
	var p={};
	p.scriptItems=[{"categroyName":"txt","desc":"默认文字列表","name":"std_list","categoryDesc":"文字列表","type":"groovy"},{"categroyName":"txt","desc":"默认无点文字列表","name":"std_list_nodot","categoryDesc":"文字列表","type":"groovy"},{"categroyName":"txt","desc":"时间右对齐","name":"std_list_timera","categoryDesc":"文字列表","type":"groovy"},{"categroyName":"txt","desc":"10条输出一空行,带类别","name":"std_list_b10s","categoryDesc":"文字列表","type":"groovy"},{"categroyName":"txt","desc":"5条一块,带副标题","name":"std_subhead","categoryDesc":"文字列表","type":"groovy"},{"categroyName":"txt","desc":"第一块4条，其它的5条一块,带副标题","name":"four_std_subhead","categoryDesc":"文字列表","type":"groovy"},{"categroyName":"pic","desc":"默认图片列表","name":"std_list_pic","categoryDesc":"图片列表","type":"groovy"},{"categroyName":"pic","desc":"组图列表","name":"std_list_zutu","categoryDesc":"图片列表","type":"groovy"},{"categroyName":"pic","desc":"新版图片列表","name":"std_list_pic_custom","categoryDesc":"图片列表","type":"groovy"},{"categroyName":"pic","desc":"新版组图列表","name":"std_list_zutu_custom","categoryDesc":"图片列表","type":"groovy"},{"categroyName":"video","desc":"默认视频列表","name":"std_list_video","categoryDesc":"视频列表","type":"groovy"},{"categroyName":"video","desc":"新版视频列表","name":"std_list_video_custom","categoryDesc":"视频列表","type":"groovy"},{"categroyName":"video","desc":"大图视频列表","name":"std_list_video_bpic_custom","categoryDesc":"视频列表","type":"groovy"}];
	this.$dlContent=$("#dlContent");
	//表单
	p.form={
		dlFlagName:$("#dlFlagName"),				/* 碎片名称 */
		txtDLEntity:$("#txtDLEntity"),				/* 引用实体 */
		txtDLShowNum:$("#txtDLShowNum"),			/* 显示条数 */
		ddlDLSort:$("#ddlDLSort"),					/* 排序类型 */
		ddlDLPriorityMin:$("#ddlDLPriorityMin"),	/* 权重min */
		ddlDLPriorityMax:$("#ddlDLPriorityMax"),	/* 权重max */
		ddlDLSubType:$("#ddlDLSubType"),			/* 新闻类型 */
		ddlDLScriptType:$("#ddlDLScriptType"),		/* 显示类型 */
		ddlDLScriptName:$("#ddlDLScriptName"),		/* 输出格式 */
		ddlDLTimeType:$("#ddlDLTimeType"),			/* 时间格式 */
		btnPreview:$("#ctDynamicList .btnPreview"),	/* 预览按钮 */
		btnOK:$("#ctDynamicList .btnOK"),			/* 确定按钮 */
		btnNO:$("#ctDynamicList .btnNO")			/* 关闭按钮 */
	};
	p.form.validate=function(){
		p.form.isValid=true;
		p.form.txtDLEntity.curVal=$.trim(p.form.txtDLEntity.val());
		p.form.txtDLShowNum.curVal=$.trim(p.form.txtDLShowNum.val());
		p.form.ddlDLSort.curVal=p.form.ddlDLSort.val();
		p.form.ddlDLPriorityMin.curVal=p.form.ddlDLPriorityMin.val();
		p.form.ddlDLPriorityMax.curVal=p.form.ddlDLPriorityMax.val();
		p.form.ddlDLSubType.curVal=p.form.ddlDLSubType.val();
		p.form.ddlDLScriptType.curVal=p.form.ddlDLScriptType.val();
		p.form.ddlDLScriptName.curVal=p.form.ddlDLScriptName.val();
		p.form.ddlDLTimeType.curVal=p.form.ddlDLTimeType.val();
		
		if(p.form.txtDLEntity.curVal=="")
		{
			alert("引用实体不能为空");
			p.form.txtDLEntity.focus();
			p.form.isValid=false;
			return;
		};
		if(!StringUtils.isPlusInt(p.form.txtDLShowNum.curVal))
		{
			alert("显示条数必须是1到100间的整数");
			p.form.txtDLShowNum.focus();
			p.form.isValid=false;
			return;
		};
		p.form.txtDLShowNum.curVal=parseInt(p.form.txtDLShowNum.curVal);
		if(p.form.txtDLShowNum.curVal<1||p.form.txtDLShowNum.curVal>100){
			alert("显示条数必须是1到100间的整数");
			p.form.isValid=false;
			return;			
		};
		
		if(p.form.ddlDLPriorityMin.curVal>p.form.ddlDLPriorityMax.curVal){
			alert("权重下限必须小于等于权重上限");
			p.form.isValid=false;
			return;
		};
		
	};
	//表单事件注册
	p.form.ddlDLPriorityMin.change(function(evt){
		var v=$(this).val();
		if(v>p.form.ddlDLPriorityMax.val()){
			alert("权重下限必须小于等于权重上限");
			p.form.ddlDLPriorityMax.focus();
		};
	});
	p.form.ddlDLPriorityMin.change(function(evt){
		var v=$(this).val();
		if(v<p.form.ddlDLPriorityMin.val()){
			alert("权重下限必须小于等于权重上限");
			p.form.ddlDLPriorityMin.focus();
		};
	});
	
	//显示类型联动输出格式
	p.form.ddlDLScriptType.change(function(evt){
		var v=$(this).val();
		p.form.ddlDLScriptName.empty();
		$.each(p.scriptItems,function(i,o){
			if(o.categroyName==v){
				p.form.ddlDLScriptName.append('<option value="'+o.name+'">'+o.desc+'</option>');
			};
		});//each
	});
	
	p.form.ddlDLScriptType.trigger("change");
	
	p.form.btnPreview.click(function(evt){
		_this.Preview();
	});
	
	p.form.btnOK.click(function(evt){
		_this.Submit();
	});
	p.form.btnNO.click(function(evt){
		sohu.diyTplFactory.cls();
	});
	
	this.$dlContent.click(function(evt){
		_this.$dlContent.slideUp();
	});
	
	this._p=p;
};
sohu.diyTplFactory.DynamicList.prototype.Submit=function(){
	this._p.form.validate();
	if(!this._p.form.isValid) return false;
	
	this.Preview(-1);
	
	var tempDiv=this.$dlContent.clone().find(".overlay").remove().end();
	var ct={html0:$.trim(tempDiv.html())};
	ct.flash=false;
	ct.tplID="dynamicList";
	ct.isNew=true;
	ct.type="dlist";
	
	sohu.diyTplFactory.submit(ct,true);
};
sohu.diyTplFactory.DynamicList.prototype.Preview=function(popup){
	popup=popup||true;
	this._p.form.validate();
	if(!this._p.form.isValid) return false;
	
	this.BuildList();
	if(popup){
		this.$dlContent.slideDown("fast");
		this.$dlContent.find(".overlay").css("opacity","0.8");
	};

};
sohu.diyTplFactory.DynamicList.prototype.BuildList=function(){
	//生成相应的dom
	if(!this._p.form.isValid) return false;
	var ctWrap=$('<div><div class="dlist ct"><ul/></div></div>');
	var ul0=ctWrap.find("ul");
	var ct0=ctWrap.find(".ct");
	var li0=$('<li><a href="http://testcms.sohu.com/20090602/n264287979.shtml" target="_blank">视频：医院丧道德 致新生儿失治疗机会终生残疾</a><span>(13:00)</span></li>');
	var li1=$('<li><a href="http://testcms.sohu.com/20091010/n267260341.shtml" target="_blank"><img alt="beijing news" src="http://photocdn.sohu.com/20091010/Img267260342_ss.jpg" width="120" height="90"/></a><span><a href="http://testcms.sohu.com/20091010/n267260341.shtml" target="_blank">beijing news</a><br/>组图1张</span></li>');
	var li=null;
	if(this._p.form.ddlDLScriptType.curVal=="txt"){
		for(var i=0;i<this._p.form.txtDLShowNum.curVal;i++){
			li=li0.clone();
			if(this._p.form.ddlDLTimeType.curVal=="-1"){
				li.find("span").remove();
			};
			ul0.append(li);
		};
		ct0.addClass("list");
		
	}else if(this._p.form.ddlDLScriptType.curVal=="pic"){
		ct0.addClass("pp");
		for(var i=0;i<this._p.form.txtDLShowNum.curVal;i++){
			li=li1.clone();
			ul0.append(li);
		};
	}else{
		ct0.addClass("pp");
		for(var i=0;i<this._p.form.txtDLShowNum.curVal;i++){
			li=li1.clone();
			ul0.append(li);
		};
	};
	
	this.$dlContent.find(".ct").remove().end().append(ctWrap.html());
};
/**
 * @author levinhuang
 * 内容模板选择对话框客户端逻辑
 */
sohu.diyCTFactory = function() {
    var p={},pub={};
	p.selectEmptyLine=function(evt){
		p.setContent(line);
		return false;
	};
	p.getAccordionCfg=function(){
		var ckName="sohu.diyCTFactory.tabIndx";
		//看cookie中上次打开的tab
		var tabIdx=$.cookie(ckName);
		tabIdx=tabIdx||0;
		//每次tab改变时更新cookie
		var _onChange=function(evt,ui){
			var idx=ui.newHeader[0].id.split("_")[1];
			$.setCookie(ckName,idx);
		};
		return {
			active:tabIdx,
			change:_onChange
		};
	};
    //private area
    p.initVar = function(opts) { 
		p._line=new sohu.diyTplFactory.Line({});/*空行模板对象*/
		p._flasher=new sohu.diyTplFactory.FocusImg({});/*焦点图*/
		p._img=new sohu.diyTplFactory.Image({});/*图片*/
		p._txt=new sohu.diyTplFactory.Text({});/*文本*/
		new sohu.diyTplFactory.SecHead({});/* 栏目标题 */
		new sohu.diyTplFactory.DynamicList({});/* 自动列表 */
		new sohu.diyTplFactory.Nav({}); /* 导航 */
		pub.$ctWrap=$("#ctWrap");
	};
    p.onLoaded = function() { 
		var cfg=p.getAccordionCfg();
		var $main=$("#addContent").accordion(cfg);
		$("#web_loading").remove();
		if(parent.sohu)
			parent.sohu.diyConsole.toggleLoading();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 