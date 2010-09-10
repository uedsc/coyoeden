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
		this.text==1?this.$d.css({"height":parseInt(that.imgH)+20+"px"}):this.$d.css({"height":that.imgH+"px"});
		this.$d.css({"width":that.imgW+"px","position":"relative","overflow":"hidden","cursor":"pointer"});
		var _imgBg,_place;
		switch(that.place){
			case "1":
				that.text==1?_place="left:2px;bottom:20px":_place="left:2px;bottom:0";
				_imgBg="height:13px;padding-top:3px;background:url(images/MDC_Focus_bg01.gif) no-repeat bottom";
				break;
			case "2":
				that.text==1?_place="right:2px;bottom:20px":_place="right:2px;bottom:0";
				_imgBg="height:13px;padding-top:3px;background:url(images/MDC_Focus_bg01.gif) no-repeat bottom";
				break;
			case "3":
				_place="left:2px;top:0";
				_imgBg="height:16px;padding-top:0;background:url(images/MDC_Focus_bg02.gif) no-repeat bottom";
				break;
			case "4":
				_place="right:2px;top:0";
				_imgBg="height:16px;padding-top:0;background:url(images/MDC_Focus_bg02.gif) no-repeat bottom";
				break;
		}		
		this._$tabC=jQuery('<div style="height:16px;display:inline;position:absolute;'+_place+'"></div>');
		this._$titleC=jQuery("<div style='height:20px;width:100%;position:absolute;bottom:0;left:0;background:"+that.bgColor+";color:"+that.txtColor+";font:12px/20px \"宋体\"'></div>");
		this._$img=jQuery('<img style="position:absolute;top:0;left:0;display:block;width:100%;height:'+that.imgH+'px" />');
		this.$d.append(this._$img).append(this._$tabC).append(this._$titleC);			

		//是否显示文本
		this.text==1?this._$titleC.show():this._$titleC.hide();
		
		//根据数据创建tab标签
		for(var i=0;i<that.focusData.length;i++){
			this._$tabC.append('<span style="'+_imgBg+';width:23px;font:12px/12px \'arial\';color:#fff;float:left;margin-left:-1px">'+(i+1)+'</span>');
		};
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
		this._$tabs.filter(".now").removeClass().css("backgroundPosition","bottom").end()
			.eq(i).addClass("now").css("backgroundPosition","top");
	}
};
