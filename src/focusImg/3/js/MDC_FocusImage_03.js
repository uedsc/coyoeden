var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);
	this.id=this.$d[0].id;	
	this.imgW=parseInt(cfg.imgW);
	this.imgH=parseInt(cfg.imgH);
	this.speed=parseInt(cfg.speed)||5000;
	this.focusData=data;
	this.type=cfg.type;							/* 类型1=V首,2=电影,3=电视,4=音乐,5=戏剧 */
	this.autoPlay;
	//私有属性-用于缓存dom对象提升效能
	this._$tabC=null;							/* tab标签容器,init方法中生成 */
	this._$tabs=null;							/* tab标签 */
	this._$titleC=null;							/* 标题容器,init方法中生成 */
	this._$img=null;							/* 图片对象 */
	this._$transparentOvl=null;					/* 透明蒙层 */
	this._$desc=null;							/* 描述文字 */	
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
		//创建框架-注:构建html字符串然后一次性构建dom对象比多次使用$(html)高效
		var html='<div class="fi03_'+this.type+'"'+ 'id="'+this.id+'_'+this.type+'">';
		html+='<div class="content">';
		html+='<img width="'+this.imgW+'" height="'+this.imgH+'"/><div class="transparence"></div><h2></h2><p></p><div class="playButton"></div>';
		html+='</div>';
		html+='<div class="tab"></div>';
		html+='</div>';
		this.$d.css("width",this.imgW).append(html);
		//样式处理及dom缓存
		this.$d.children().css("width",this.imgW);
		this._$transparentOvl=this.$d.find(".transparence").css("opacity",0.5);
		this._$img=this.$d.find("img");
		this._$titleC=this.$d.find("h2");
		this._$desc=this.$d.find("p");
		this._$tabC=this.$d.find(".tab");
		
		//根据数据创建tab标签-注：动态构造大量dom时，先构造完整的html更有效能
		html="";
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
		this.$d.find(".content").click(function(){
			window.open(that._$img.attr("link"));
		})
	},
	/**
		交互(alternation):
			1、根据类型选择对应的过场动画;
			2、数据切换的实现.
	*/
	alternation:function(i){			
		//标签切换交互
		this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");
		
		//透明条动画
		if(this.type==1){			
			this._$transparentOvl.css({
				left:-600
			}).stop().animate({"left":0},200);				
		};
		
		//回填大图路径及绑定链接路径,并给大图加渐显动画
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({"opacity":0}).stop().animate({opacity:1},300);
		
		//数据回填
		this._$titleC.html(this.focusData[i].t);
		this._$desc.html(this.focusData[i].t1);
	}
};