var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);						/* dom jquery对象 */
	this.id=this.$d[0].id;						/* dom编号 */	
	this.imgW=parseInt(cfg.imgW);
	this.imgH=parseInt(cfg.imgH);
	this.speed=parseInt(cfg.speed)||5000;
	this.place=cfg.place;
	this.text=cfg.text;
	this.txtColor=cfg.txtColor;
	this.bgColor=cfg.bgColor;
	this.focusData=data;
	this.autoPlay;
	//私有属性-缓存dom对象
	this._$tabC=null;							/* tab标签容器,init方法中生成 */
	this._$tabs=null;							/* tab标签 */
	this._$titleC=null;							/* 标题容器,init方法中生成 */
	this._$img=null;							/* 图片对象 */
	//缓存数据
	this.$d.data("apple",{a:cfg,b:data});
	//init
	this.init();
};
MDC_FocusImage.prototype={
	/**
		初始化(init):
			1、根据类型创建焦点图框架;
			2、并数据回填到结构中.
	*/
	init:function(){
		var that=this;
		//创建框架
		var html='<img height="'+this.imgH+'"/>';
		html+='<div class="tab '+this.place+'"></div>';
		html+='<div class="tt"></div>';
		this.$d.append(html);
		//样式处理及dom缓存
		this._$tabC=this.$d.find(".tab");
		this._$titleC=this.$d.find(".tt").css({background:this.bgColor,color:this.txtColor});
		this._$img=this.$d.find("img");
		
		if(this.text){
			var h0=this._$titleC.height();
			this._$titleC.show();
			this.$d.css({width:this.imgW,height:this.imgH+h0});
			this._$tabC.css("bottom",h0);
		}else{
			this._$titleC.hide();
			this.$d.css({width:this.imgW,height:this.imgH});
		};
		
		//根据数据创建tab标签
		html="";
		for(var i=0;i<that.focusData.length;i++){
			html+='<span>'+(i+1)+'</span>';
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
		//回填大图路径及绑定链接路径,并给大图加渐显动画
		this.$d.attr("href",this.focusData[i].l);
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({"opacity":0}).stop().animate({opacity:1},1000);
		
		//数据回填
		this._$titleC.html(this.focusData[i].t);
		//标签切换交互
		this._$tabs.filter(".now").removeClass().end()
			.eq(i).addClass("now");		
	}
};
