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
	}
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
	
	sohu.diyTplFactory.submit(ct);
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
		this.tplID=null;//模板号
		var _this=this;
		var p={};
		p.onAddImg=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.Submit();
			return false;
		};
		//鼠标事件
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddImg);
	
};
sohu.diyTplFactory.Image.prototype.Submit=function(opt){
	var ct={};
	ct.html0=this.tplObj.html();
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type="shimage";
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
			step:178,
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
	
};
sohu.diyTplFactory.Text.prototype.Submit=function(opt){
	var ct={html0:this.tplObj.html()};
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type="shtext";
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