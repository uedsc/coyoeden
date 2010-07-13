/**
 * 搜狐cms5内容模板类定义
 */
var sohu_cms5_ct={
	/**
	 * 关闭内容选择对话框
	 */
	cls:function(){
		parent.bos.CloseCTDialog();
	},
	/**
	 * 提交内容对象
	 * @param {Object} ct 内容对象
	 * @param {Boolean} cls是否关闭对话框
	 */
	submit:function(ct,cls){
		sohu_cms5_contentlist.SelectedContent=ct;
		parent.bos.Editor.CurSec.AddContent(ct);
		if (cls) {
			sohu_cms5_ct.cls();
		};
	}
};
/**
 * 空行的设置类
 */
sohu_cms5_ct.Line=function(opts){
	var _this=this;
	//属性定义-用户设置
	this.layoutID=opts.layoutID||"ctEmptyLine";//模板编号
	this.css_cpk=opts.css_cpk||".cpk";//cpk css选择器
	this.css_txtH=opts.css_txtH||".txtH";//高度录入框 css选择器
	this.css_btnOK=opts.css_btnOK||".btnOK";
	this.css_cbkLine=opts.css_cbkLine||".cbxLine";
	this.color=opts.color||"#808080";
	this.height=opts.height||10;
	this.cLine=opts.cLine||false;//是否中间显示分割线
	//属性定义-dom引用
	this.html=null;//模板html
	this.$layoutCfg=null;//设置界面dom
	this.$txtCpk=null;//dom-颜色选择器colorpicker
	this.$txtHeight=null;//dom-高度输入框
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
sohu_cms5_ct.Line.prototype.Submit=function(opt){
	this.cLine=this.$cbkLine[0].checked;
	var lineWrap=$("<div/>").html(this.html);
	var hr=lineWrap.find("hr");
	var line=lineWrap.find(".vspace");
	if(this.cLine){line.addClass("cline");hr.css("border-color",this.color);}else{
		hr.remove();
	};
	var css={"height":this.height+'px'};
	line.css("height",this.height+'px');
	
	var ct={
		flash:false,
		html:lineWrap.html()
	};
	
	sohu_cms5_ct.submit(ct);
};
/**
 * 焦点图选择类
 */
sohu_cms5_ct.FocusImg=function(opts){
		//焦点图选择
		this.$Layout=$("#cSlide_focusImg").cycleSlide({
			cssBtnPrev:"#ctFocusImg .btnLeft",
			cssBtnNext:"#ctFocusImg .btnRight",
			step:178,
			cloneItem:true
		});
		this.FlashTplID=null;//flash 模板号
		var _this=this;
		var p={};
		p.onAddFlash=function(evt){
			_this.FlashTplID=this.id;
			_this.Submit();
			return false;
		};
		//鼠标事件
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddFlash);
	
};
sohu_cms5_ct.FocusImg.prototype.Submit=function(opt){
	var ct={};
	ct.html='<div class="ct flash"></div>';
	ct.flash=true;
	ct.tplID=this.FlashTplID;
	sohu_cms5_ct.submit(ct,true);
};
/**
 * 图片选择类
 */
sohu_cms5_ct.Image=function(opts){
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
sohu_cms5_ct.Image.prototype.Submit=function(opt){
	var ct={};
	ct.html=this.tplObj.html();
	ct.flash=false;
	ct.tplID=this.tplID;
	sohu_cms5_ct.submit(ct,true);
};
/**
 * 文本选择类
 */
sohu_cms5_ct.Text=function(opts){
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
sohu_cms5_ct.Text.prototype.Submit=function(opt){
	var ct={html:this.tplObj.html()};
	ct.flash=false;
	ct.tplID=this.tplID;
	sohu_cms5_ct.submit(ct,true);
};
/**
 * @author levinhuang
 * 内容模板选择对话框客户端逻辑
 */
var sohu_cms5_contentlist = function() {
    var p={},pub={};
	p.selectEmptyLine=function(evt){
		p.setContent(line);
		return false;
	};
    //private area
    p.initVar = function(opts) { 
		p._line=new sohu_cms5_ct.Line({});/*空行模板对象*/
		p._flasher=new sohu_cms5_ct.FocusImg({});/*焦点图*/
		p._img=new sohu_cms5_ct.Image({});/*图片*/
		p._txt=new sohu_cms5_ct.Text({});/*文本*/
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