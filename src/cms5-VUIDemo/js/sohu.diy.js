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
	var opts0={
		interval:5,
		bg:"#ffffff",
		Param:{
			wmode:"opaque",//zindex issue:slightlymore.co.uk/flash-and-the-z-index-problem-solved/
			quality:"high",
			salign:"t"
		},
		Var:{
			b:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
			p:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
			p_s:"vsFocus/images/01.jpg|vsFocus/images/02.jpg|vsFocus/images/03.jpg|vsFocus/images/04.jpg|vsFocus/images/05.jpg",
			l:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
			icon:"标题1|标题2|标题3|标题4|标题5",
			icon_2:"内容1|内容2|内容3|内容4|内容5",
			icon2:"内容1|内容2|内容3|内容4|内容5"
		},
		w:opts.w,
		h:opts.h
	};
	var m = {};
	m.Flash01={swf:"vsFocus/swf/0501.swf",h0:359,/*默认高*/w0:662/*默认宽*/,Var:{b:"NULL",p_s:"NULL",icon_2:"NULL"}};
	m.Flash02={swf:"vsFocus/swf/0502.swf",h0:440,w0:780,interval:6,Param:{salign:"NULL"},Var:{b:"NULL",p_s:"NULL",l:"NULL",icon:"NULL",icon2:"NULL",icon_2:"NULL"}};
	m.Flash03={swf:"vsFocus/swf/0601.swf",h0:270,w0:320,Var:{b:"NULL",p_s:"NULL",icon2:"NULL",icon_2:"NULL"}};
	m.Flash04={swf:"vsFocus/swf/0602.swf",h0:340,w0:520,Var:{b:"NULL",p_s:"NULL",icon_2:"NULL"}};
	m.Flash05={swf:"vsFocus/swf/0603.swf",h0:388,w0:950,bg:"#000000",interval:8,Var:{b:"NULL"}};
	m.Flash06={swf:"vsFocus/swf/1001.swf",h0:360,w0:616,Var:{p_s:"NULL",icon2:"NULL",icon_2:"NULL"}};
	m.Flash07={swf:"vsFocus/swf/1102.swf",h0:330,w0:685,Var:{p:"NULL",p_s:"NULL",icon:"NULL",icon2:"NULL",icon_2:"NULL"}};
	m.Flash08={swf:"vsFocus/swf/1103.swf",h0:295,w0:635,Var:{b:"NULL",p_s:"NULL",l:"NULL",icon:"NULL",icon_2:"NULL",icon2:"NULL"}};
	m.Flash08.Var.p="vsFocus/images/c4.jpg|vsFocus/images/a.jpg|vsFocus/images/b1.jpg|vsFocus/images/c3.jpg|vsFocus/images/1.png|vsFocus/images/a.jpg|vsFocus/images/b1.jpg|images/c3.jpg";
	m.Flash09={swf:"vsFocus/swf/1105.swf",h0:340,w0:520,bg:"#000000",Var:{b:"NULL",icon_2:"NULL",icon2:"NULL"}};
	m.Flash10={swf:"vsFocus/swf/1203.swf",h0:375,w0:620,bg:"#000000",interval:8,Var:{b:"NULL",icon2:"NULL"}};
	m.Flash11={swf:"vsFocus/swf/1204.swf",h0:210,w0:948,bg:"#000000",interval:8,Var:{b:"NULL",p:"NULL",icon:"NULL",icon2:"NULL"}};
	m.Flash12={swf:"vsFocus/swf/1205.swf",h0:290,w0:395,Var:{b:"NULL",p:"NULL",p_s:"NULL",l:"NULL",icon2:"NULL",icon_2:"NULL",pics:opts0.Var.p,links:opts0.Var.l}};
	m.Flash13={swf:"vsFocus/swf/1207.swf",h0:340,w0:590,bg:"#000000",Var:{b:"NULL",p_s:"NULL",l:"NULL",icon:"NULL",icon_2:"NULL",icon2:"NULL",mylinks:opts0.Var.l,texts:opts0.Var.icon}};
	m.Flash14={swf:"vsFocus/swf/0203.swf",h0:230,w0:690,interval:7,Var:{b:"NULL",p_s:"NULL"}};
	m.Flash15={swf:"vsFocus/swf/0205.swf",h0:299,w0:298,Var:{b:"NULL",p:"NULL",p_s:"NULL",l:"NULL",icon2:"NULL",icon_2:"NULL",pics:opts0.Var.p,links:opts0.Var.l,text:opts0.Var.icon}};
	m.Flash16={swf:"vsFocus/swf/0207.swf",h0:469,w0:980,Var:{p_s:"NULL",icon2:"NULL",icon_2:"NULL"}};
	//属性
	this.tid=new Date().getTime();
	this.tplID=opts.tplID;
	opts=$.extend(true,{},opts0,m[opts.tplID]);
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
/**
 * 呈现
 * @param {Object} $t
 * @param {Object} reRender
 */
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
/*=/内容模板区域=*/

