/**
 * @author levin
 * @desc	1,�����ռ�����2,���з���domģ��
 */
/*=�����ռ䶨��=*/
//sohu
var sohu={};
//sohu�༭��
sohu.diy={};
//sohu�༭��ģ��
sohu.diyTp={};
/*=/�����ռ䶨��=*/


/*=����ģ������=*/
//�պ���
sohu.diyTp["w0"]='<div class="area area_empty"><div class="vstp_col vstp_w950"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
//2������
sohu.diyTp["w270_670"]='<div class="area area_empty"><div class="vstp_col vstp_w270 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w670 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w430_510"]='<div class="area area_empty"><div class="vstp_col vstp_w430 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w510 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w470_470"]='<div class="area area_empty"><div class="vstp_col vstp_w470 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w470 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w670_270"]='<div class="area area_empty"><div class="vstp_col vstp_w670 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w510_430"]='<div class="area area_empty"><div class="vstp_col vstp_w510 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w430 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
//3������
sohu.diyTp["w190_270_470"]='<div class="area area_empty"><div class="vstp_col vstp_w190 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w470 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w310_390_230"]='<div class="area area_empty"><div class="vstp_col vstp_w310 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w390 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w230 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w470_270_190"]='<div class="area area_empty"><div class="vstp_col vstp_w470 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w190 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';
sohu.diyTp["w270_390_270"]='<div class="area area_empty"><div class="vstp_col vstp_w270 left"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w390 center"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div><div class="vstp_col vstp_w270 right"><div class="vstp_sec"><div class="vstp_secHolder"></div></div></div></div>';

/*=/����ģ������=*/

/*=����ģ������=*/
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

/*=/����ģ������=*/

/*=����ģ������=*/
//����
sohu.diyTp["ctEmptyLine"]='<div class="vspace ct"><hr/></div>';
//����ͼ
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
			/* ��ͼ */
			pics:"vsFocus/images/allfocus4_0.jpg|vsFocus/images/allfocus4_1.jpg|vsFocus/images/allfocus4_2.jpg|vsFocus/images/allfocus4_3.jpg",
			/* ���� */
			links:"http://www.sohu.com|http://www.sogou.com|http://news.sohu.com|http://women.sohu.com|http://it.sohu.com",
			/* ���� */
			texts:"�������� �Ա���Я�Ѻ���ƵάȨ|�������� ١��Ϊ�����P�ݷ���|��������ǩԼ ���ǵ�������|��������Ļ ����ʱ��Ψ���㳡"
		},
		w:opts.w,
		h:opts.h
	};
	var m = {};
	m.Flash01={swf:"vsFocus/swf/allfocus4.swf",h0:320,/*Ĭ�ϸ�*/w0:290/*Ĭ�Ͽ�*/,Var:{pic_width:290,pic_height:300,show_text:1,txtcolor:'000000',bgcolor:'ffffff',button_pos:2,stop_time:5000}};
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
	//����
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
	
	//�߶ȵ�����-���ڷ����Ŀ�����ƣ�����Ҫ��Ը߶Ƚ��б�������
	this.height=(opts.h0*this.width)/opts.w0;
	this.__opts=opts;
	
};
/**
 * ����
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
 * ����flash��������ԡ�֧�ֵ�������id,height,width,swf
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash.prototype.Attr=function(n,v){
	this.FLASH.setAttribute(n,v);
	this[n]=v;
	this.Render(null,true);
};
/**
 * ����flash���������variables�����ֵ�����Զ���variables��������p,l,icon,icon2
 * @param {Object} n
 * @param {Object} v
 */
sohu.diyTp.Flash.prototype.Vari=function(n,v){
	this.FLASH.variables[n]=v;
	this.Var[n]=v;
	this.Render(null,true);
};

/*=/����ģ������=*/

/* ר����ģ������ */
/* ����ͨ����ȡ��̨json���� */
sohu.diyTp.TopicTpl=[
	{id:1,name:'topic_auto',title:'����ר��ģ��1',icon:"images/pic146x151_1.jpg",url:'static/topic_auto/index.html'},
	{id:2,name:'topic_auto1',title:'����ר��ģ��2',icon:"images/pic146x151_1.jpg",url:'static/topic_auto/index.html'},
	{id:3,name:'topic_auto2',title:'����ר��ģ��3',icon:"images/pic146x151_1.jpg",url:'static/topic_auto/index.html'}
];
