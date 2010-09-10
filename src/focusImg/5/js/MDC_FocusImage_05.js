var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);
	this.id=this.$d[0].id;	
	this.imgW=parseInt(cfg.imgW);				/* 大图宽 */
	this.imgH=parseInt(cfg.imgH);				/* 大图高 */
	this.imgW1=parseInt(cfg.imgW1);				/* 小图宽 */
	this.imgH1=parseInt(cfg.imgH1);				/* 小图高 */
	this.W=parseInt(cfg.W);						/* 宽 */
	this.H=parseInt(cfg.H);						/* 高 */
	this.speed=parseInt(cfg.speed)||5000;
	this.type=cfg.type;							/* 类型1=公益 */
	this.focusData=data;
	this.autoPlay;
	//私有属性-用于缓存dom对象提升效能
	this._$tabC=null;							/* tab标签容器,init方法中生成 */
	this._$tabs=null;							/* tab标签 */
	this._$titleC=null;							/* 标题容器,init方法中生成 */
	this._$img=null;							/* 图片对象 */
	this._$transparentOvl=null;					/* 透明蒙层 */
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
		//创建框架-先构建html再利用$(html)构建dom对象,避免多次使用$(html)
		var html='<div class="fi05_'+this.type+'"'+ 'id="'+this.id+'_'+this.type+'">';
		html+='<div class="content">';
		html+='<img style="display:block;"/><div class="transparence"></div><h2></h2>';
		html+='</div>';
		html+='<div class="tab"><dl></dl></div>';
		html+='</div>';
		this.$d.css("width",this.W).append(html);
		
		//样式处理及dom缓存
		this.$d.children().css({width:this.W,height:this.H});
		this._$transparentOvl=this.$d.find(".transparence").css("opacity",0.5);
		this._$img=this.$d.find("img").css({width:this.imgW,height:this.imgH});
		this._$titleC=this.$d.find("h2");
		this._$tabC=this.$d.find("dl");		
		
		//根据数据创建tab标签		
		html="";
		for(var i=0;i<this.focusData.length;i++){
			this.focusData[i].p1=((!this.focusData[i].p1)||(this.focusData[i].p1==""))?this.focusData[i].p:this.focusData[i].p1;
			html+='<dd><img src="'+this.focusData[i].p1+'" width="'+this.imgW1+'" height="'+this.imgH1+'" style="display:block;"/><p>'+this.focusData[i].t+'</p></dd>';
		};
		this._$tabC.append(html);
		this._$tabs=this._$tabC.find("dd");				
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
		this._$tabs.mouseenter(function(){
			jQuery(this).addClass("hover")
		});
		this._$tabs.mouseleave(function(){
			jQuery(this).removeClass("hover")
		})
		
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
		
		//透明条动画,标题同步骤
		this._$transparentOvl.css({
			bottom:-40
		}).stop().animate({"bottom":0},200);	
					
		this._$titleC.css({
			bottom:-24
		}).stop().animate({"bottom":16},200);	
		
		//回填大图路径及绑定链接路径,并给大图加渐显动画
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({"opacity":0}).stop().animate({opacity:1},300);
		
		//数据回填
		this._$titleC.html(this.focusData[i].t);
	}
};