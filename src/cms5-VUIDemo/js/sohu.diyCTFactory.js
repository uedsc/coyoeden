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
 * 栏目标题设置类
 * @param {Object} opts
 */
sohu.diyTplFactory.SecHead=function(opts){
	var _this=this;
	//属性-用户变量
	this.Style="sec_hd";
	this.ShowMore=true;
	this.ShowMoreTpl='<em class="elm">更多>></em>';
	//属性-dom引用
	this.$Layout=$("#ctSecHead");
	this.$ddlSecHeadCss=$("#ddlSecHeadCss");
	this.$tplObj=this.$Layout.find(".ctWrap");
	//私有成员
	var p={};
	p.defaultStyle="sec_hd ct";
	p.initLayout=function(){
		_this.$ddlSecHeadCss.change(function(evt){
			_this.Style=this.value;
			_this.$tplObj.find("h2").attr("class","").addClass(p.defaultStyle).addClass(_this.Style);
		});
		
		_this.$Layout.find(".btnOK").click(function(evt){
			_this.Submit({});
			return false;
		});
		_this.$Layout.find("#cbxSecHeadMore").click(function(evt){
			_this.ShowMore=this.checked;
			var items=_this.$tplObj.find("em");
			if(!_this.ShowMore){
				items.remove();
			}else{
				if(items.length==0)
					_this.$tplObj.find("h2").append(_this.ShowMoreTpl);
			}
		});
	};
	
	p.initLayout();
};
sohu.diyTplFactory.SecHead.prototype.Submit=function(opts){
	var ct={
		flash:false,
		html0:this.$tplObj.html(),
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
	this.width=opts.width||100;/* 高度％ */
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
			_this.height=(_this.height>100||_this.height<1)?10:_this.height;
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
		type:'shline'
	};
	
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 焦点图选择类
 */
sohu.diyTplFactory.FocusImg=function(opts){
		//焦点图选择
		this.$Layout=$("#cSlide_focusImg").cycleSlide({
			cssBtnPrev:"#ctFocusImg .btnLeft",
			cssBtnNext:"#ctFocusImg .btnRight",
			step:178,
			cloneItem:true
		});
		this.tplID=null;//flash 模板号
		var _this=this;
		var p={};
		p.onAddFlash=function(evt){
			_this.tplID=this.id;
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
	ct.html0='<div class="ct shflash"></div>';
	ct.flash=true;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type='shflash';
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * 图片选择类
 */
sohu.diyTplFactory.Image=function(opts){
		//焦点图选择
		this.$Layout=$("#ctImg");
		//组图设置弹框
		this.$WinCfg=$("#cfgImgList");
		this.$WinCfg.ovl=$("#cfgImgList_ovl");
		this.$WinCfg.$imgWidth=$("#txtImgWidth");
		this.$WinCfg.$imgCol=$("#ddlImgListCol");
		this.$WinCfg.$tip=this.$WinCfg.find(".tip");
		this.tplID=null;//模板号
		this.IsImgList=false;//是否组图
		var _this=this;
		var p={};
		p.onAddImg=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.CurColSize=parseInt(parent.sohu.diyConsole.CurSec.Width);
			_this.CurSecSize=parent.sohu.diyConsole.CurSec.Dim().w;
			//如果是组图，弹出组图设置框
			_this.IsImgList=_this.tplObj.find(".ct").hasClass("pp");
			if(_this.IsImgList){
				_this.$WinCfg.ovl.css("opacity","0.8").show();
				_this.$WinCfg.$tip.html(_this.CurColSize);
				_this.$WinCfg.slideDown();
				_this.$WinCfg.$imgCol.trigger("change");
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
			var v=_this.$WinCfg.$imgCol.val();
			var w=Math.floor(_this.CurSecSize/v-10-6);
			_this.$WinCfg.$imgWidth.removeClass("alert").val(w).effect("highlight");
		};
		p.setPPWidth=function(evt){
			var v=this.value;
			if(!StringUtils.isPlusInt(v)){
				_this.$WinCfg.$imgWidth.addClass("alert");
				//_this.$WinCfg.$imgCol.trigger("change");
				_this.$WinCfg.$imgWidth.select();
				return false;
			};
			_this.$WinCfg.$imgWidth.removeClass("alert");
			//根据图片宽度推算列数
			v=parseInt(v);
			var col=Math.floor(_this.CurSecSize/(v+10+6));
			col=col==0?1:col;
			if(col>6){
				alert("图片宽度"+v+"px对应的图片列数是"+col+"列,超过了组图列数上限6！");
				_this.$WinCfg.$imgWidth.addClass("alert");
				//_this.$WinCfg.$imgCol.trigger("change");
				_this.$WinCfg.$imgWidth.select();
				return false;
			};
			_this.$WinCfg.$imgCol.val(col);
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
		this.$Layout=$("#cSlide_text").cycleSlide({
			cssBtnPrev:"#ctText .btnLeft",
			cssBtnNext:"#ctText .btnRight",
			step:187,
			cloneItem:true
		});
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
		this.$Layout.find("a").click(sohu.diyTplFactory.r);	
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
 * @author levinhuang
 * 内容模板选择对话框客户端逻辑
 */
sohu.diyCTFactory = function() {
    var p={},pub={};
	p.selectEmptyLine=function(evt){
		p.setContent(line);
		return false;
	};
    //private area
    p.initVar = function(opts) { 
		p._line=new sohu.diyTplFactory.Line({});/*空行模板对象*/
		p._flasher=new sohu.diyTplFactory.FocusImg({});/*焦点图*/
		p._img=new sohu.diyTplFactory.Image({});/*图片*/
		p._txt=new sohu.diyTplFactory.Text({});/*文本*/
		new sohu.diyTplFactory.SecHead({});/* 栏目标题 */
		pub.$ctWrap=$("#ctWrap");
	};
    p.onLoaded = function() { 
		$("#contentAccordion").accordion();
		$("#web_loading").remove();
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