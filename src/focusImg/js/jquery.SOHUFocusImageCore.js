/**
 * 搜狐焦点图插件核心插件
 * @author levinhuang
 * @version 2010.10.12
 * @desc 搜狐常用7种焦点图请使用MDC_FocusImage.js.
 * 新增的焦点图，考虑到搜索引擎对动态生成的dom内容不可见，请采用第8种焦点图的方式,不采用json数据动态生成
 */
;(function($) {
    // Private functions.
    var p = {cache:{}};
	p.M=function(cfg){
		this.$d=cfg.$d;								/* JQuery对象 */
		this.flag=cfg.flag;							/* 焦点图类型标识。如fi01表示类型1的焦点图 */	
		this.id=this.$d[0].id;						/* dom编号 */
		this.text=cfg.text||true;					/* 是否显示标题 */
		this.speed=parseInt(cfg.speed)||5000;		/* 轮刷速度 */
		this.hoverStop=cfg.hoverStop;				/* 鼠标移到tab上时是否停止播放 */
		this.type=cfg.type||1;						/* 子类型 */
		this.place=cfg.place||'fi_tabRB';				/* tab标签的位置 */
		this.myHtml=cfg.myHtml||"";					/* 用户额外的html.填充到class='fi_ct'的容器中 */
		this.ptStepX=cfg.ptStepX||92;				/* tab指针水平位移步长 */
		this.ptStepY=cfg.ptStepY||81;				/* tab指针垂直位移步长 */
		this.ptStepX_=cfg.ptStepX_||4;				/* tab指针水平位移步长偏移值 */
		this.ptStepY_=cfg.ptStepY_||-1;				/* tab指针垂直位移步长 偏移值*/			
		this.clickTabToNav=cfg.clickTabToNav||false;/* 点击tab标签是否导航 */
		this.autoPlay=null;							/* 自动播放计时器id */
		this.autoPlay1=null;						/* 鼠标从tab上移走时进行下一次播放的计时器id */
		
		//私有属性-_$开头的变量用于缓存dom对象提升效能
		this._fiObj=null;							/* 焦点图定制对象 */
		this._$tabC=null;							/* tab标签容器,init方法中生成 */
		this._$tabs=null;							/* tab标签 */
		this._$curTab=null;							/* 当前tab标签 */
		this._$titleC=null;							/* 标题容器,init方法中生成 */
		this._$img=null;							/* 图片对象 */
		this._$transparentOvl=null;					/* 透明蒙层 */
		this._$desc=null;							/* 描述文字 */
		this._$pointer=null;						/* 缩略图指针 */
		this._cfg=cfg;
		this._curLink=null;
		this._$curImg=null;							/* 当前小图 */
        this._tabNum=0;                             /* tab总数 */	
		//缓存数据
		this.$d.data("apple",{a:cfg});	
		//初始化
		this.init();
			
	};
	p.M.prototype={
		/**
			初始化(init):
				1、根据类型创建焦点图框架;
				2、并数据回填到结构中.
		*/
		init:function(){
			//获取焦点图定制对象
			this._fiObj=p.cache[this.flag];
			if(!this._fiObj) return;
			//创建框架-并缓存dom
			this._fiObj.init(this);
			//额外的html内容
			this.$d.find(".fi_ct").append(this.myHtml);
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
					that.alternation((++_index)==that._tabNum?_index=0:_index);
				},that.speed);
			};
			gogo();
			
			//给tab标签绑定Click事件,用于焦点图切换
			this._$tabs.each(function(i){
				(function(i,o){
					o=jQuery(o).click(function(){
						if(this.className!="now"){
							clearTimeout(that.autoPlay1);
							clearInterval(that.autoPlay);
							gogo(i);
						};
						if(that.clickTabToNav){
							window.open(that._curLink);
						};
						return false;
					});
					if(that.hoverStop){
						o.mouseenter(function(){
							clearInterval(that.autoPlay);
							clearTimeout(that.autoPlay1);
							if (this.className != "now") {that.alternation(i);};
							return false;
						}).mouseleave(function(){
							clearInterval(that.autoPlay);
							clearTimeout(that.autoPlay1);
							var j=(i+1)==that._tabNum?0:(i+1);
							that.autoPlay1=window.setTimeout(function(){gogo(j);},that.speed);
						});
						
					};
				})(i,this)
			});
						
			if(this._fiObj.initEvts){
				this._fiObj.initEvts(this,gogo);
			};
		},
		/**
			交互(alternation):
				1、根据类型选择对应的过场动画;
				2、数据切换的实现.
		*/
		alternation:function(i){			
			var continueCommon=true;
			//定制的交互效果
			if(this._fiObj.alt)
				continueCommon=this._fiObj.alt(this,i);
				
			if(!continueCommon) return;
			//回填大图路径及绑定链接路径,并给大图加渐显动画
			this._$img.attr("src",this._$curTab.find("img").attr("src")).css({opacity:0}).stop().animate({opacity:1},500);		
			//标签切换交互
			this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");	
			
		}
	};
    //main plugin body
	/**
	 * focusImg插件
	 * @param {Object} cfg 焦点图的配置数据
	 */
    $.fn.focusImg = function(cfg) {
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			cfg.$d=$(this);
        	new p.M(cfg);
		});
    };
    // Public functions.
	/**
	 * 注册焦点图定制对象
	 * @param {Object} key 键，如fi01表示焦点图类型1
	 * @param {Object} fiObj 如{init,initEvts,alt}
	 */
    $.fn.focusImg.Register = function(key,fiObj) {
        p.cache[key]=fiObj;
    };
})(jQuery);