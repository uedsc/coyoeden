/**
 * @author levin
 * @desc	1,命名空间声明2,横切分栏dom模板
 */
/*=命名空间定义=*/
//sohu
var sohu={};
//sohu编辑器
sohu.diy={};
//sohu编辑器模板
sohu.diyTp={};
/*=/命名空间定义=*/


/*=横切模板区域=*/
//空横切
sohu.diyTp["w0"]='<div class="area"><div class="col w950"><div class="sec"></div></div></div>';
//2栏横切
sohu.diyTp["w270_670"]='<div class="area clear"><div class="col w270 left"><div class="sec"></div></div><div class="col w670 right"><div class="sec"></div></div></div>';
sohu.diyTp["w430_510"]='<div class="area clear"><div class="col w430 left"><div class="sec"></div></div><div class="col w510 right"><div class="sec"></div></div></div>';
sohu.diyTp["w470_470"]='<div class="area clear"><div class="col w470 left"><div class="sec"></div></div><div class="col w470 right"><div class="sec"></div></div></div>';
sohu.diyTp["w670_270"]='<div class="area clear"><div class="col w670 left"><div class="sec"></div></div><div class="col w270 right"><div class="sec"></div></div></div>';
sohu.diyTp["w510_430"]='<div class="area clear"><div class="col w510 left"><div class="sec"></div></div><div class="col w430 right"><div class="sec"></div></div></div>';
//3栏横切
sohu.diyTp["w190_270_470"]='<div class="area clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w270 center"><div class="sec"></div></div><div class="col w470 right"><div class="sec"></div></div></div>';
sohu.diyTp["w310_390_230"]='<div class="area clear"><div class="col w310 left"><div class="sec"></div></div><div class="col w390 center"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["w470_270_190"]='<div class="area clear"><div class="col w470 left"><div class="sec"></div></div><div class="col w270 center"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
sohu.diyTp["w270_390_270"]='<div class="area clear"><div class="col w270 left"><div class="sec"></div></div><div class="col w390 center"><div class="sec"></div></div><div class="col w270 right"><div class="sec"></div></div></div>';

/*=/横切模板区域=*/

/*=分栏模板区域=*/
//390
sohu.diyTp["sw190_190"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
//430
sohu.diyTp["sw210_210"]='<div class="subsec clear"><div class="col w210 left"><div class="sec"></div></div><div class="col w210 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_230"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw230_190"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
//470
sohu.diyTp["sw230_230"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw270_190"]='<div class="subsec clear"><div class="col w270 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_270"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w270 right"><div class="sec"></div></div></div>';

//510
sohu.diyTp["sw250_250"]='<div class="subsec clear"><div class="col w250 left"><div class="sec"></div></div><div class="col w250 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_310"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w310 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw310_190"]='<div class="subsec clear"><div class="col w310 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';

//670
sohu.diyTp["sw330_330"]='<div class="subsec clear"><div class="col w330 left"><div class="sec"></div></div><div class="col w330 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_470"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w470 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw470_190"]='<div class="subsec clear"><div class="col w470 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw230_190_230"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w190 center"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';

//950
sohu.diyTp["sw310_310_310"]='<div class="subsec clear"><div class="col w310 left"><div class="sec"></div></div><div class="col w310 center"><div class="sec"></div></div><div class="col w310 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw230_230_230_230"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w230 center"><div class="sec"></div></div><div class="col w230 center"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw182_182_182_182_182"]='<div class="subsec clear"><div class="col w182 left"><div class="sec"></div></div><div class="col w182 center"><div class="sec"></div></div><div class="col w182 center"><div class="sec"></div></div><div class="col w182 center"><div class="sec"></div></div><div class="col w182 right"><div class="sec"></div></div></div>';

/*=/分栏模板区域=*/

/*=内容模板区域=*/
//空行
sohu.diyTp["ctEmptyLine"]='<div class="ct vspace"><hr/></div>';
//焦点图
sohu.diyTp.Flash=function(opts){
	var defaultOpts={
		interval:5,
		bg:"#ffffff",
		Param:{
			wmode:"opaque",//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			quality:"high",
			salign:"t"
		},
		Var:{
			p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
			l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
			icon:"标题1|标题2|标题3|标题4|标题5",
			icon2:"内容1|内容2|内容3|内容4|内容5"
		}
	};
	var models = {};
	models.Flash01={
			h0:359,//默认高
			w0:662,//默认宽
			swf:"vsFocus/swf/0501.swf"
	};
	
	
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	
	opts=$.extend(true,{},defaultOpts,models[opts.tplID]);
	//Params
	this.Param=opts.Param;
	for(var p in this.Param){
		if(this.Param[p]=="NULL"){delete this.Param[p];};//special NULL
	};
	//Variables
	this.Var=opts.Var;
	for(var v in this.Var){
		if(this.Var[v]=="NULL"){delete this.Var[v];};
	};
	//attributes
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
	this.__opts=opts;
	
};

sohu.diyTp.Flash.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			//params
			for(var p in this.Param){
				this.FLASH.addParam(p,this.Param[p]);
			};
			//variables
			for(var v in this.Var){
				this.FLASH.addVariable(v,this.Var[v]);
			};
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图1-焦点图元素有 图片，标题，内容，连接，序号，图片5秒轮刷，图片可加减

sohu.diyTp.Flash01=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:5,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		icon:"标题1|标题2|标题3|标题4|标题5",
		icon2:"内容1|内容2|内容3|内容4|内容5",
		swf:"vsFocus/swf/0501.swf",
		h0:359,//默认高
		w0:662,//默认宽
		bg:"#ffffff"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p,
		l:opts.l,
		icon:opts.icon,
		icon2:opts.icon2
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash01.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
			this.FLASH.addVariable("icon2", this.Var.icon2);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash01.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash01.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图2-大图缓冲出现，其他图片依次遮挡，留左侧一小部分
sohu.diyTp.Flash02=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:6,
		swf:"vsFocus/swf/0502.swf",
		h0:440,//默认高
		w0:780//默认宽
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.Var={
		p:opts.p
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash02.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash02.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash02.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图3-普通焦点图，图片轮刷，点击图片打开连接
sohu.diyTp.Flash03=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg",
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		texts:"标题1|标题2|标题3|标题4|标题5",
		interval:5,
		swf:"vsFocus/swf/0601.swf",
		h0:270,//默认高
		w0:320,//默认宽
		bg:"#ffffff"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.bg=opts.bg;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.Var={
		p:opts.p,
		l:opts.l,
		icon:opts.texts
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash03.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval,this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign",this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash03.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash03.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图4-文字在图片下方，焦点图元素有 图片，标题，内容，连接，序号，图片5秒轮刷，图片可加减
sohu.diyTp.Flash04=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:5,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		icon:"标题1|标题2|标题3|标题4|标题5",
		icon2:"内容1|内容2|内容3|内容4|内容5",
		swf:"vsFocus/swf/0602.swf",
		h0:340,//默认高
		w0:520,//默认宽
		bg:"#ffffff"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p,
		l:opts.l,
		icon:opts.icon,
		icon2:opts.icon2
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash04.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
			this.FLASH.addVariable("icon2", this.Var.icon2);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash04.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash04.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图5-右侧缩略图，点击右侧左侧变大图，图片轮刷，点击图片打开连接。焦点图有详细介绍，和缩略图和简单介绍
sohu.diyTp.Flash05=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		p_s:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:8,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		icon:"标题1|标题2|标题3|标题4|标题5",
		icon_2:"内容1|内容2|内容3|内容4|内容5",
		swf:"vsFocus/swf/0603.swf",
		h0:388,//默认高
		w0:950,//默认宽
		bg:"#000000"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p,
		p_s:opts.p_s,
		l:opts.l,
		icon:opts.icon,
		icon_2:opts.icon2
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash05.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("p_s", this.Var.p);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
			this.FLASH.addVariable("icon_2", this.Var.icon2);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash05.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash05.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图6-下方有缩略图，点击小图，变大图，大图加有标题。可通过按钮左右选择图片
sohu.diyTp.Flash06=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		b:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:5,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		icon:"标题1|标题2|标题3|标题4|标题5",
		swf:"vsFocus/swf/1001.swf",
		h0:369,//默认高
		w0:616,//默认宽
		bg:"#ffffff"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p,
		b:opts.b,
		l:opts.l,
		icon:opts.icon
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash06.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("b", this.Var.b);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash06.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash06.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图7-左右按钮翻大图
sohu.diyTp.Flash07=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		b:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:5,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		swf:"vsFocus/swf/1102.swf",
		h0:330,//默认高
		w0:685,//默认宽
		bg:"#ffffff"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		b:opts.b,
		l:opts.l
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash07.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("b", this.Var.b);
			this.FLASH.addVariable("l", this.Var.l);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash07.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash07.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图8-左右按钮翻大图，图片组呈弧形透视.图片大小 438x536
sohu.diyTp.Flash08=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:5,
		swf:"vsFocus/swf/1103.swf",
		h0:295,//默认高
		w0:635,//默认宽
		bg:"#ffffff"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash08.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash08.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash08.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图9-焦点图有大图，标题，内容。不用的传统的序号，改为图形
sohu.diyTp.Flash09=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		p_s:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:8,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		icon:"标题1|标题2|标题3|标题4|标题5",
		swf:"vsFocus/swf/1105.swf",
		h0:390,//默认高
		w0:680,//默认宽
		bg:"#000000"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p,
		p_s:opts.p_s,
		l:opts.l,
		icon:opts.icon
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash09.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("p_s", this.Var.p);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash09.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash09.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
//焦点图10-焦点图左侧小图，通过上下按钮控制小图位置，点小图，变大图和连接
sohu.diyTp.Flash10=function(opts){
	//选项
	opts=$.extend({},{
		quality:"high",
		salign:"t",
		p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		p_s:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
		interval:8,
		l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
		icon:"标题1|标题2|标题3|标题4|标题5",
		icon_2:"内容1|内容2|内容3|内容4|内容5",
		swf:"vsFocus/swf/1203.swf",
		h0:375,//默认高
		w0:620,//默认宽
		bg:"#000000"
	},opts);
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	this.quality=opts.quality;
	this.salign=opts.salign;
	this.interval=opts.interval;
	this.swf=opts.swf;
	this.FLASH=null;
	this.height=opts.h||opts.h0;
	this.width=opts.w||opts.w0;
	this.bg=opts.bg;
	this.Var={
		p:opts.p,
		p_s:opts.p_s,
		l:opts.l,
		icon:opts.icon,
		icon_2:opts.icon2
	};
	
	//高度的修正-由于分栏的宽度限制，所以要针对高度进行比例缩放
	this.height=(opts.h0*this.width)/opts.w0;
};

sohu.diyTp.Flash10.prototype.Render=function($t,reRender){
	if (!reRender) {
		this.DomID = $t.attr("id");
		try{
			this.FLASH = new sohuFlash(this.swf, this.DomID+"_"+this.tid, this.width, this.height, this.interval, this.bg);
			this.FLASH.addParam("quality", this.quality);
			this.FLASH.addParam("salign", this.salign);
			this.FLASH.addParam("wmode","opaque");//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			this.FLASH.addVariable("p", this.Var.p);
			this.FLASH.addVariable("p_s", this.Var.p);
			this.FLASH.addVariable("l", this.Var.l);
			this.FLASH.addVariable("icon", this.Var.icon);
			this.FLASH.addVariable("icon_2", this.Var.icon2);
		}catch(e){
			$t.html(e.message);
		}
	};
	if(!this.FLASH) return;
	try{
		this.FLASH.write(this.DomID);	
		$t.css({width:this.width,height:this.height});
	}catch(e){
		$t.html(e.message);
	};
};
/**
 * 更改flash对象的属性。支持的属性有id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash10.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * 更改flash对象的属性variables对象的值。属性对象variables的属性有p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash10.prototype.Var=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};
/*=/内容模板区域=*/

