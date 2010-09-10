var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);							/* 框架dom */
	this.id=this.$d[0].id;							/* dom编号 */	
	this.type=cfg.type;								/* 类型1=视频新闻 */
	this.W=parseInt(cfg.W);							/* 焦点图宽 */
	this.H=parseInt(cfg.H);							/* 焦点图高 */
	this.imgW=parseInt(cfg.imgW);					/* 图片宽 */
	this.imgH=parseInt(cfg.imgH);					/* 图片高 */
	this.speed=parseInt(cfg.speed)||5000;			/* 轮换时间 */
	this.focusData=data;							/* 图片数据 */
	this.autoPlay;
	
	//私有属性-用于缓存dom对象提升效能
	this._$tabC=null;							/* tab标签容器,init方法中生成 */
	this._$tabs=null;							/* tab标签 */
	this._$titleC=null;							/* 标题容器,init方法中生成 */
	this._$img=null;							/* 图片对象 */
	//缓存数据
	this.$d.data("apple",{a:cfg,b:data});	
	//初始化
	this.init();
};
MDC_FocusImage.prototype={
	/**
		初始化(init):
			1、根据类型创建焦点图框架;
			2、并数据回填到结构中.
	*/
	init:function(){
		//创建框架
		this._$tabC=jQuery("<div>",{className:"tab"});
		this._$titleC=jQuery("<h2>");
		this._$img=jQuery("<img>",{
			css:{						
				width:this.imgW,
				height:this.imgH,
				display:'block'
			}
		});
		
		this.$d.css({"width":this.W,"height":this.H}).append(
			jQuery("<div>",{
				id:this.id+"_"+this.type,
				className:'fi02_'+this.type,
				css:{
					width:this.W,
					height:this.H
				}
			}).append(this._$img).append(
				jQuery("<div>",{
					className:"transparence",
					css:{						
						opacity:.5
					}
				})
			).append(this._$titleC).append(
				jQuery("<div>",{
					className:"focusImageBg"
				})
			).append(
				jQuery("<div>",{
					className:"link"
				})
			).append(this._$tabC)
		);
		
		//根据数据创建tab标签-注：动态构造大量dom时，先构造完整的html更有效能
		var html="";
		for(var i=0;i<this.focusData.length;i++){
			html+="<span>"+(i+1)+"</span>";
		};
		this._$tabC.append(html);
		this._$tabs=this._$tabC.find("span");
		//执行触发
		this.spring();
	},
	/**
		触发(spring):
			1、tab切换事件的绑定;
			2、计时器轮刷的实现.
	*/
	spring:function(){
		var that=this;
		
		//执行交互并轮刷
		var gogo=function(i){
			var _index=i||0;
			that.alternation(_index);
			that.autoPlay=setInterval(function(){
				that.alternation((++_index)==that.focusData.length?_index=0:_index);
			},that.speed);
		};
		gogo();
		
		//给tab标签绑定Click事件,用于焦点图切换
		this._$tabs.each(function(i){
			(function(i,o){
				jQuery(o).click(function(){
					if(this.className!="now"){
						clearTimeout(that.autoPlay);
						gogo(i);
					};
					return false;
				});
			})(i,this)
		});
		
		//给焦点区绑定Click事件,用于弹出链接
		this.$d.click(function(){
			window.open(that._$img.attr("link"));
		});
	},
	/**
		交互(alternation):
			1、根据类型选择对应的过场动画;
			2、数据切换的实现.
	*/
	alternation:function(i){
		//标签切换交互
		this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");
		//回填大图路径及绑定链接路径,并给大图加渐显动画
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({"opacity":0}).stop().animate({opacity:1},300);
		//数据回填
		this._$titleC.html(this.focusData[i].t);
	}
};
