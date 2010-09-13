/**
 * 焦点图插件
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {cache:{}};
	p.M=function(cfg,data){
		this.$d=cfg.$d;								/* JQuery对象 */
		this.flag=cfg.flag;							/* 焦点图类型标识。如fi01表示类型1的焦点图 */	
		this.id=this.$d[0].id;						/* dom编号 */
		this.imgW=parseInt(cfg.imgW);				/* 大图宽 */
		this.imgH=parseInt(cfg.imgH);				/* 大图高 */
		this.imgW1=parseInt(cfg.imgW1);				/* 小图宽 */
		this.imgH1=parseInt(cfg.imgH1);				/* 小图高 */
		this.W=parseInt(cfg.W);						/* 宽 */
		this.H=parseInt(cfg.H);						/* 高 */
		this.text=cfg.text||true;					/* 是否显示标题 */
		this.speed=parseInt(cfg.speed)||5000;		/* 轮刷速度 */
		this.hoverStop=cfg.hoverStop;				/* 鼠标移到tab上时是否停止播放 */
		this.type=cfg.type;							/* 子类型 */
		this.place=cfg.place||'tabRB';				/* tab标签的位置 */
		this.txtColor=cfg.txtColor;					/* 标题文本颜色 */
		this.bgColor=cfg.bgColor;					/* 标题背景色 */
		this.myHtml=cfg.myHtml||"";					/* 用户额外的html.填充到class='content'的容器中 */
		this.ptStepX=cfg.ptStepX||92;				/* tab指针水平位移步长 */
		this.ptStepY=cfg.ptStepY||81;				/* tab指针垂直位移步长 */
		this.ptStepX_=cfg.ptStepX_||4;				/* tab指针水平位移步长偏移值 */
		this.ptStepY_=cfg.ptStepY_||-1;				/* tab指针垂直位移步长 偏移值*/	
		this.focusData=data;						/* 图片json数据 */
		this.autoPlay;								/* 自动播放 */
		
		//私有属性-_$开头的变量用于缓存dom对象提升效能
		this._fiObj;								/* 焦点图定制对象 */
		this._$tabC=null;							/* tab标签容器,init方法中生成 */
		this._$tabs=null;							/* tab标签 */
		this._$titleC=null;							/* 标题容器,init方法中生成 */
		this._$img=null;							/* 图片对象 */
		this._$transparentOvl=null;					/* 透明蒙层 */
		this._$desc=null;							/* 描述文字 */
		this._$pointer=null;						/* 缩略图指针 */	
		//缓存数据
		this.$d.data("apple",{a:cfg,b:data});	
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
			this.$d.find(".content").append(this.myHtml);
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
					o=jQuery(o).click(function(){
						if(this.className!="now"){
							//clearTimeout(that.autoPlay);
							clearInterval(that.autoPlay);
							gogo(i);
						};
						return false;
					});
					if(that.hoverStop){
						o.mouseenter(function(){
							clearInterval(that.autoPlay);
							if (this.className != "now") {that.alternation(i);};
							return false;
						});
						//TODO:
						that._$tabC.mouseleave(function(){
							if((i+1)==that.focusData.length)
								gogo(0);
							else
								gogo(i+1);
						});
						
					};
				})(i,this)
			});
						
			if(this._fiObj.initEvts){
				this._fiObj.initEvts(this);
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
			this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({opacity:0}).stop().animate({opacity:1},500);		
			//标签切换交互
			this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");	
			//数据回填
			this._$titleC.html(this.focusData[i].t);
			if(this._$desc)
				this._$desc.html(this.focusData[i].t1);
			
		}
	};
    //main plugin body
	/**
	 * focusImg插件
	 * @param {Object} cfg 焦点图的配置数据
	 * @param {Object} data 焦点图的图片数据
	 */
    $.fn.focusImg = function(cfg,data) {
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			cfg.$d=$(this);
        	new p.M(cfg,data);
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

/* 焦点图1 */
$.fn.focusImg.Register("fi01",{
	init:function(fi){
		//创建框架
		var html='<img height="'+fi.imgH+'"/>';
		html+='<div class="tab '+fi.place+'"></div>';
		html+='<h2 class="tt"></h2>';
		fi.$d.append(html);
		//样式处理及dom缓存
		fi._$tabC=fi.$d.find(".tab");
		fi._$titleC=fi.$d.find(".tt").css({background:fi.bgColor,color:fi.txtColor});
		fi._$img=fi.$d.find("img");
		
		if(fi.text){
			var h0=fi._$titleC.height();
			fi._$titleC.show();
			fi.$d.css({width:fi.imgW,height:fi.imgH+h0});
			fi._$tabC.css("bottom",h0);
		}else{
			fi._$titleC.hide();
			fi.$d.css({width:fi.imgW,height:fi.imgH});
		};
		
		//根据数据创建tab标签
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			html+='<span>'+(i+1)+'</span>';
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("span");		
	},
	initEvts:function(fi){
		//给焦点区绑定Click事件,用于弹出链接
		fi.$d.click(function(){window.open(fi._$img.attr("link"));});
	}
});
/* 焦点图2 */
$.fn.focusImg.Register("fi02",{
	init:function(fi){
		//创建框架
		var html='<div id="'+fi.id+'_'+fi.type+'" class="fi02_'+fi.type+'">';
		html+='<img width="'+fi.imgW+'" height="'+fi.imgH+'"/>';
		html+='<div class="transparence"></div>';
		html+='<h2 class="tt"></h2>';
		html+='<div class="focusImageBg"></div>';
		html+='<div class="link"></div>';
		html+='<div class="tab"></div>';
		html+='</div>';
		fi.$d.css({"width":fi.W,"height":fi.H}).append(html);
		//dom缓存
		fi.$d.children().css({width:fi.W,height:fi.H});
		fi._$tabC=fi.$d.find(".tab");
		fi._$titleC=fi.$d.find(".tt");
		fi._$img=fi.$d.find("img");
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		//根据数据创建tab标签-注：动态构造大量dom时，先构造完整的html更有效能
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			html+="<span>"+(i+1)+"</span>";
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("span");
	},
	initEvts:function(fi){
		//给焦点区绑定Click事件,用于弹出链接
		fi.$d.click(function(){window.open(fi._$img.attr("link"));});
	}
});
/* 焦点图3 */
$.fn.focusImg.Register("fi03",{
	init:function(fi){
		//创建框架-注:构建html字符串然后一次性构建dom对象比多次使用$(html)高效
		var html='<div class="fi03_'+fi.type+'"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="content">';
		html+='<img width="'+fi.imgW+'" height="'+fi.imgH+'"/><div class="transparence"></div><h2></h2><p></p><div class="playButton"></div>';
		html+='</div>';
		html+='<div class="tab"></div>';
		html+='</div>';
		fi.$d.css("width",fi.imgW).append(html);
		//样式处理及dom缓存
		fi.$d.children().css("width",fi.imgW);
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		fi._$img=fi.$d.find("img");
		fi._$titleC=fi.$d.find("h2");
		fi._$desc=fi.$d.find("p");
		fi._$tabC=fi.$d.find(".tab");
		
		//根据数据创建tab标签-注：动态构造大量dom时，先构造完整的html更有效能
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			html+="<span>"+(i+1)+"</span>";
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("span");
	},
	initEvts:function(fi){
		//给焦点区绑定Click事件,用于弹出链接
		fi.$d.find(".content").click(function(){window.open(that._$img.attr("link"));});
	},
	alt:function(fi,i){
		//透明条动画
		if(fi.type==1){			
			fi._$transparentOvl.css({
				left:-600
			}).stop().animate({"left":0},200);				
		};
		return true;
	}
});
/* 焦点图4 */
$.fn.focusImg.Register("fi04",{
	init:function(fi){
		//创建框架-先构建html再利用$(html)构建dom对象,避免多次使用$(html)
		var html='<div class="fi04_'+fi.type+'"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="content">';
		html+='<img width="'+fi.imgW+'" height="'+fi.imgH+'"/><div class="transparence"></div><h2></h2><p></p>';
		html+='</div>';
		html+='<div class="tab"><span class="pointer"></span><dl></dl></div>';
		html+='</div>';
		fi.$d.css("width",fi.W).append(html);
		
		//样式处理及dom缓存
		fi.$d.children().css({width:fi.W,height:fi.H});
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		fi._$img=fi.$d.find("img");
		fi._$titleC=fi.$d.find("h2");
		fi._$desc=fi.$d.find("p");
		fi._$tabC=fi.$d.find("dl");
		fi._$pointer=fi.$d.find(".pointer");
		
		//根据数据创建tab标签
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;
			html+='<dd><img src="'+fi.focusData[i].p1+'" width="'+fi.imgW1+'" height="'+fi.imgH1+'"/></dd>';
		};
		fi._$tabC.append(html).find("img").css("opacity",0.5);
		fi._$tabs=fi._$tabC.find("dd");
	},
	initEvts:function(fi){
		//给焦点区绑定Click事件,用于弹出链接
		fi.$d.find(".content").click(function(){window.open(that._$img.attr("link"));});
		fi._$pointer.click(function(){window.open(that._$img.attr("link"));});
	},
	alt:function(fi,i){
		fi._$tabs.filter(".now").children().animate({"opacity":.5},200);
		fi._$tabs.eq(i).children().animate({"opacity":1},200);
		//指针滚动动画				
		switch(fi.type){
			case "1":
				fi._$pointer.stop().animate({"left":i*fi.ptStepX+fi.ptStepX_},200);
				break;
			case "2":
				fi._$pointer.stop().animate({"top":i*fi.ptStepY+fi.ptStepY_},200);
				break;
		};		
		return true;
	}
});
