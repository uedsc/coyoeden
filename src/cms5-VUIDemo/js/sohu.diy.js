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
sohu.diyTp["w0"]='<div class="area area_empty"><div class="vstp_col vstp_w950"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
//2栏横切
sohu.diyTp["w270_670"]='<div class="area area_empty"><div class="vstp_col vstp_w270 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w670 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w430_510"]='<div class="area area_empty"><div class="vstp_col vstp_w430 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w510 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w470_470"]='<div class="area area_empty"><div class="vstp_col vstp_w470 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w470 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w670_270"]='<div class="area area_empty"><div class="vstp_col vstp_w670 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w510_430"]='<div class="area area_empty"><div class="vstp_col vstp_w510 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w430 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
//3栏横切
sohu.diyTp["w190_270_470"]='<div class="area area_empty"><div class="vstp_col vstp_w190 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w470 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w310_390_230"]='<div class="area area_empty"><div class="vstp_col vstp_w310 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w390 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w470_270_190"]='<div class="area area_empty"><div class="vstp_col vstp_w470 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w270_390_270"]='<div class="area area_empty"><div class="vstp_col vstp_w270 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w390 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';

/*=/横切模板区域=*/

/*=分栏模板区域=*/
//390
sohu.diyTp["sw390"]='<div class="vstp_subsec"><div class="vstp_col vstp_w390"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw190_190"]='<div class="vstp_subsec"><div class="vstp_col vstp_w190 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
//430
sohu.diyTp["sw210_210"]='<div class="vstp_subsec"><div class="vstp_col vstp_w210 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w210 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw190_230"]='<div class="vstp_subsec"><div class="vstp_col vstp_w190 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw230_190"]='<div class="vstp_subsec"><div class="vstp_col vstp_w230 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
//470
sohu.diyTp["sw230_230"]='<div class="vstp_subsec"><div class="vstp_col vstp_w230 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw270_190"]='<div class="vstp_subsec"><div class="vstp_col vstp_w270 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw190_270"]='<div class="vstp_subsec"><div class="vstp_col vstp_w190 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';

//510
sohu.diyTp["sw250_250"]='<div class="vstp_subsec"><div class="vstp_col vstp_w250 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w250 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw190_310"]='<div class="vstp_subsec"><div class="vstp_col vstp_w190 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w310 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw310_190"]='<div class="vstp_subsec"><div class="vstp_col vstp_w310 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';

//670
sohu.diyTp["sw330_330"]='<div class="vstp_subsec"><div class="vstp_col vstp_w330 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w330 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw190_470"]='<div class="vstp_subsec"><div class="vstp_col vstp_w190 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w470 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw470_190"]='<div class="vstp_subsec"><div class="vstp_col vstp_w470 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw230_190_230"]='<div class="vstp_subsec"><div class="vstp_col w230 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';

//950
sohu.diyTp["sw310_310_310"]='<div class="vstp_subsec"><div class="vstp_col vstp_w310 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w310 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w310 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw230_230_230_230"]='<div class="vstp_subsec"><div class="vstp_col vstp_w230 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["sw182_182_182_182_182"]='<div class="vstp_subsec"><div class="vstp_col vstp_w182 l"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w182 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w182 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w182 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w182 r"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';

/*=/分栏模板区域=*/

/*=内容模板区域=*/
//空行
sohu.diyTp["ctEmptyLine"]='<div class="vspace ct"><hr/></div>';
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
			/* 大图 */
			pics:"vsFocus/images/allfocus4_0.jpg|vsFocus/images/allfocus4_1.jpg|vsFocus/images/allfocus4_2.jpg|vsFocus/images/allfocus4_3.jpg",
			/* 链接 */
			links:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
			/* 标题 */
			texts:"官网启动 赵宝刚携搜狐视频维权|即将打响 佟大为马伊琍演夫妻|首轮卫视签约 众星到场助阵|超豪华落幕 打造时尚唯美秀场"
		},
		w:opts.w,
		h:opts.h
	};
	var m = {};
	m.Flash01={swf:"vsFocus/swf/allfocus4.swf",h0:320,/*默认高*/w0:290/*默认宽*/,Var:{pic_width:290,pic_height:300,show_text:1,txtcolor:'000000',bgcolor:'ffffff',button_pos:2,stop_time:5000}};
	m.Flash02=m.Flash01;
	m.Flash03=m.Flash01;
	m.Flash04=m.Flash01;
	m.Flash05=m.Flash01;
	m.Flash06=m.Flash01;
	m.Flash07=m.Flash01;
	m.Flash08=m.Flash01;
	m.Flash09=m.Flash01;
	m.Flash10=m.Flash01;
	m.Flash11=m.Flash01;
	m.Flash12=m.Flash01;
	m.Flash13=m.Flash01;
	m.Flash14=m.Flash01;
	m.Flash15=m.Flash01;
	m.Flash16=m.Flash01;
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
		//$t.css({width:this.width,height:this.height});
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
sohu.diyTp.Flash.prototype.Vari=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};

/*=/内容模板区域=*/

/* 专题风格模板配置 */
/* 将来通过获取后台json数据 */
sohu.diyTp.TopicTpl=[
	{id:1,name:'topic_auto',title:'汽车专题模板1',icon:"images/pic146x151_1.jpg",url:'static/topic_auto/index.html'},
	{id:2,name:'topic_auto1',title:'汽车专题模板2',icon:"images/pic146x151_1.jpg",url:'static/topic_auto/index.html'},
	{id:3,name:'topic_auto2',title:'汽车专题模板3',icon:"images/pic146x151_1.jpg",url:'static/topic_auto/index.html'}
];
