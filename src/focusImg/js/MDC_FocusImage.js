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
		this.focusData=data;						/* 图片json数据 */
		this.clickTabToNav=cfg.clickTabToNav||false;/* 点击tab标签是否导航 */
		this.autoPlay;								/* 自动播放计时器id */
		this.autoPlay1;								/* 鼠标从tab上移走时进行下一次播放的计时器id */
		
		//私有属性-_$开头的变量用于缓存dom对象提升效能
		this._fiObj;								/* 焦点图定制对象 */
		this._$tabC=null;							/* tab标签容器,init方法中生成 */
		this._$tabs=null;							/* tab标签 */
		this._$titleC=null;							/* 标题容器,init方法中生成 */
		this._$img=null;							/* 图片对象 */
		this._$transparentOvl=null;					/* 透明蒙层 */
		this._$desc=null;							/* 描述文字 */
		this._$pointer=null;						/* 缩略图指针 */
		this._curData=null;							/* 当前json数据项 */
		this._cfg=cfg;								/* 引用用户的配置项 */	
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
			//afterInit回调
			if(this._cfg.afterInit){
				this._cfg.afterInit(this);
			};
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
					that.alternation((++_index)==that.focusData.length?_index=0:_index);
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
							window.open(that.focusData[i].l);
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
							var j=(i+1)==that.focusData.length?0:(i+1);
							that.autoPlay1=window.setTimeout(function(){gogo(j);},that.speed);
						});
						
					};
				})(i,this)
			});
						
			if(this._fiObj.initEvts){
				this._fiObj.initEvts(this,gogo);
				//afterInitEvts回调
				if(this._cfg.afterInitEvts){
					this._cfg.afterInitEvts(this);
				};
			};
		},
		/**
			交互(alternation):
				1、根据类型选择对应的过场动画;
				2、数据切换的实现.
		*/
		alternation:function(i){			
			var continueCommon=true;
			this._curData=this.focusData[i];
			this.$d.data("curData",this._curData);
			//定制的交互效果
			if(this._fiObj.alt)
				continueCommon=this._fiObj.alt(this,i);
				
			if(!continueCommon){
				//afterAlt回调
				if(this._cfg.afterAlt){
					this._cfg.afterAlt(this);
				};				
				return;
			}; 
			//回填大图路径及绑定链接路径,并给大图加渐显动画
			this._$img.attr({"src":this._curData.p,"link":this._curData.l}).css({opacity:0}).stop().animate({opacity:1},500);		
			//标签切换交互
			this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");	
			//数据回填
			this._$titleC.html(this._curData.t);
			if(this._$desc)
				this._$desc.html(this._curData.t1);
				
			//afterAlt回调
			if(this._cfg.afterAlt){
				this._cfg.afterAlt(this);
			};
			
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
	/**
	 * 获取指定焦点图定制对象
	 * @param {Object} key
	 */
	$.fn.focusImg.Get=function(key){
		return p.cache[key];
	};
})(jQuery);

/* 焦点图1 */
$.fn.focusImg.Register("fi01",{
	init:function(fi){
		//创建框架
		var html='<img class="fi_player"/>';
		html+='<div class="fi_tab '+fi.place+'"></div>';
		html+='<div class="fi_tt"></div>';
		fi.$d.append(html);
		//样式处理及dom缓存
		fi._$tabC=fi.$d.find(".fi_tab");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$img=fi.$d.find(".fi_player");
		
		if(fi.text){
			fi._$titleC.show();
		}else{
			fi._$titleC.hide();
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
		html+='<img class="fi_player"/>';
		html+='<div class="fi_ovl"></div>';
		html+='<div class="fi_tt"></div>';
		html+='<div class="fi_bg"></div>';
		html+='<div class="fi_link"></div>';
		html+='<div class="fi_tab"></div>';
		html+='</div>';
		fi.$d.append(html);
		//dom缓存
		fi._$tabC=fi.$d.find(".fi_tab");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$img=fi.$d.find(".fi_player");
		fi._$transparentOvl=fi.$d.find(".fi_ovl").css("opacity",0.5);
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
		html+='<div class="fi_ct">';
		html+='<img class="fi_player"/><div class="fi_ovl"></div><div class="fi_tt"></div><div class="fi_desc"></div>';
		html+='</div>';
		html+='<div class="fi_tab"></div>';
		html+='</div>';
		fi.$d.append(html);
		//样式处理及dom缓存
		fi._$transparentOvl=fi.$d.find(".fi_ovl").css("opacity",0.5);
		fi._$img=fi.$d.find(".fi_player");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$desc=fi.$d.find(".fi_desc");
		fi._$tabC=fi.$d.find(".fi_tab");
		
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
		fi.$d.find(".fi_ct").click(function(){window.open(fi._$img.attr("link"));});
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
		html+='<div class="fi_ct">';
		html+='<img class="fi_player"/><div class="fi_ovl"></div><div class="fi_tt"></div><div class="fi_desc"></div>';
		html+='</div>';
		html+='<div class="fi_tab"><span class="fi_pointer"></span><dl></dl></div>';
		html+='</div>';
		fi.$d.append(html);
		
		//样式处理及dom缓存
		fi._$transparentOvl=fi.$d.find(".fi_ovl").css("opacity",0.5);
		fi._$img=fi.$d.find(".fi_player");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$desc=fi.$d.find(".fi_desc");
		fi._$tabC=fi.$d.find("dl");
		fi._$pointer=fi.$d.find(".fi_pointer");
		
		//根据数据创建tab标签
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;
			html+='<dd><img src="'+fi.focusData[i].p1+'"/></dd>';
		};
		fi._$tabC.append(html).find("img").css("opacity",0.5);
		fi._$tabs=fi._$tabC.find("dd");
	},
	initEvts:function(fi){
		//给焦点区绑定Click事件,用于弹出链接
		fi.$d.find(".fi_ct").click(function(){window.open(fi._$img.attr("link"));});
		fi._$pointer.click(function(){window.open(fi._$img.attr("link"));});
	},
	alt:function(fi,i){
		fi._$tabs.filter(".now").children().css("opacity",.5);
		fi._$tabs.eq(i).children().css("opacity",1);
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
/* 焦点图5 */
$.fn.focusImg.Register("fi05",{
	init:function(fi){
		//创建框架-先构建html再利用$(html)构建dom对象,避免多次使用$(html)
		var html='<div class="fi05_'+fi.type+'"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="fi_ct">';
		html+='<img class="fi_player"/><div class="fi_ovl"></div><div class="fi_tt"></div>';
		html+='</div>';
		html+='<div class="fi_tab"><dl></dl></div>';
		html+='</div>';
		fi.$d.append(html);
		
		//样式处理及dom缓存
		fi._$transparentOvl=fi.$d.find(".fi_ovl").css("opacity",0.5);
		fi._$img=fi.$d.find(".fi_player");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$tabC=fi.$d.find("dl");		
		
		//根据数据创建tab标签		
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;
			html+='<dd><img src="'+fi.focusData[i].p1+'"/><div>'+fi.focusData[i].t+'</div></dd>';
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("dd");		
	},
	initEvts:function(fi){
		fi._$tabs.mouseenter(function(){
			jQuery(this).addClass("hover");
		});
		fi._$tabs.mouseleave(function(){
			jQuery(this).removeClass("hover");
		});		
		//给焦点区绑定Click事件,用于弹出链接
		fi.$d.find(".fi_ct").click(function(){window.open(fi._$img.attr("link"));});
	},
	alt:function(fi,i){
		//透明条动画,标题同步骤
		fi._$transparentOvl.css({
			bottom:-40
		}).stop().animate({"bottom":0},200);	
					
		fi._$titleC.css({
			bottom:-24
		}).stop().animate({"bottom":16},200);		
		return true;
	}
});
/* 焦点图6 */
$.fn.focusImg.Register("fi06",{
	init:function(fi){
		//创建框架
		var html='<div class="fi06_'+fi.type+' clear"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="fi_ct l">';
		html+='<a target="_blank"><img class="fi_player"/></a><div class="fi_ovl"></div>';
		html+='<div class="fi_note"><div class="fi_tt"></div><div class="fi_desc"></div></div>';
		html+='<span class="fi_btnplay"></span>';
		html+='</div>';//l
		html+='<div class="fi_tab r">';
		html+='<span class="fi_up"><a href="#" class="fi_btn"></a></span><span class="fi_down"><a href="#" class="fi_btn"></a></span>';
		html+='<div class="fi_tab_"><dl></dl></div>';
		html+='</div>';//r
		html+='</div>';
		fi.$d.html(html);
		//dom references and cache
		fi._$img=fi.$d.find(".fi_player");
		fi._$tabC=fi.$d.find(".fi_tab dl");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$desc=fi.$d.find(".fi_desc");
		fi._$transparentOvl=fi.$d.find(".fi_ovl");
		fi._$note=fi.$d.find(".fi_note");
		fi._$tabOvl=fi.$d.find(".fi_tab_");
		//create tabs for navigation
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;				
			html+='<dd><img src="'+fi.focusData[i].p1+'"/><div class="fi_tt0"><span class="fi_icon"></span>'+fi.focusData[i].t_+'</div><div class="fi_desc0">'+fi.focusData[i].t1_+'</div></dd>';
		};
		fi._$tabC.html(html);
		fi._$tabs=fi._$tabC.find("dd");	
	},
	initEvts:function(fi,gogo){
		var _num=fi._$tabC.height()-fi._$tabOvl.height();
		//绑定各种元素的事件
		fi.$d.find(".fi_btn").click(function(e){/*失焦以便移除虚线*/this.blur();return false;});
		fi.$d.find(".r > .fi_up").mousedown(function(){
			if(fi._$tabC.position().top<0){
				if(!fi._$tabC.is(":animated")){
					clearInterval(fi.autoPlay);
					clearTimeout(fi.autoPlay1);
					fi._$tabC.stop().animate({top:"+="+fi.ptStepY},200,function(){
						fi.autoPlay1=window.setTimeout(function(){
							var s=fi._$tabs.filter(".now").index();
							s=(s-1)==-1?0:(s-1);
							gogo(s);
						},fi.speed);
					});
				};
			};			
		});
		fi.$d.find(".r > .fi_down").mousedown(function(){
			if(fi._$tabC.position().top>-_num){
				if(!fi._$tabC.is(":animated")){
					clearInterval(fi.autoPlay);
					clearTimeout(fi.autoPlay1);
					fi._$tabC.stop().animate({top:"-="+fi.ptStepY},200,function(){
						fi.autoPlay1=window.setTimeout(function(){
							var s=fi._$tabs.filter(".now").index();
							s=(s+1)==fi._$tabs.length?0:(s+1);
							gogo(s);
						},fi.speed);						
					});
				};
			};	
		});
		fi.$d.find(".fi_ct").hover(function(){
			$(this).find(".fi_btnplay").addClass("now");
		},function(){
			$(this).find(".fi_btnplay").removeClass("now");
		}).click(function(){
			window.open($(this).find("a:eq(0)").attr("href"));
			//return false;
		});
		
		//fi_player外面的a标签
		fi._$img.parent("a").click(function(e){
			e.preventDefault();
			return true;
		});
	},
	alt:function(fi,i){
		fi._$img.attr("src",fi.focusData[i].p).parent().attr("href",fi.focusData[i].l);
		fi._$tabs.removeClass("now").eq(i).addClass("now");
		//回填数据
		fi._$titleC.html(fi.focusData[i].t);
		fi._$desc.html(fi.focusData[i].t1);
		
		var _w=fi._$note.width(),_h=fi._$tabOvl.height();/* _h:tab显示区域的高度 */
		fi._$transparentOvl.css("width",0).stop().animate({width:_w},200);
		var _num1=fi._$tabs.filter(".now").prevAll().length*fi.ptStepY;
		var _num2=fi._$tabC.height()-_h;
		var _num3=_num1-(_h-fi.ptStepY);
		if(!fi._$tabC.is(":animated")){
			if(fi._$tabC.position().top > -_num3){
				fi._$tabC.stop().animate({top:-_num3},200);
			};
			if(fi._$tabC.position().top < (-_num1)){				
				fi._$tabC.stop().animate({top:-_num1},200);
			}
		}
		if(fi._$tabs[0].className=="now"){			
			if(!fi._$tabC.is(":animated")){				
				fi._$tabC.stop().animate({top:0},200);
			}
		};
		return false;
	}
});
/* 焦点图7 */
$.fn.focusImg.Register("fi07",{
	init:function(fi){
		//创建框架
		var html='<div class="fi07_'+fi.type+'" id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="fi_ct">';
		html+='<div class="fi_list"></div><div class="fi_ovl"></div><div class="fi_tt"></div>';
		html+='</div>';//content
		html+='<div class="fi_tab">';
		html+='<em class="fi_btn l"><a href="#"></a></em><em class="fi_btn r"><a href="#"></a></em>';
		html+='</div>';//tab
		html+='</div>';
		fi.$d.html(html);
		//dom references and cache
		fi._$tabC=fi.$d.find(".fi_tab");
		fi._$titleC=fi.$d.find(".fi_tt");
		fi._$transparentOvl=fi.$d.find(".fi_ovl").css("opacity",.5);
		fi._$list=fi.$d.find(".fi_list");
		fi._$btnL=fi._$tabC.find(".l");
		fi._$btnR=fi._$tabC.find(".r");
		//create tabs for navigation & image list
		html="";
		var html1="";
		for(var i=0;i<fi.focusData.length;i++){
			html+='<span><a href="'+fi.focusData[i].l+'"></a></span>';
			html1+='<img src="'+fi.focusData[i].p+'"/>';
		};
		fi._$btnL.after(html);
		fi._$list.html(html1);
		fi._$tabs=fi._$tabC.find("span");
		
		//水平方向步长
		fi.ptStepX=fi.$d.find(".fi_ct").width();	
	},
	initEvts:function(fi,gogo){
		var l=function(){return parseInt(fi._$list.css("left"));};
		var _this=this;
		//绑定各种元素的事件
		fi._$tabC.find("a").click(function(e){/*失焦以便移除虚线*/this.blur();return false;});
		//左按钮
		fi._$btnL.mousedown(function(){
			var s=fi._$tabs.index(fi._$tabs.filter(".now")[0]);
			if(s==0||fi._$list.is(":animated")) return;
			clearInterval(fi.autoPlay);
			clearTimeout(fi.autoPlay1);
			_this.alt(fi,s-1,function(){
				fi.autoPlay1=window.setTimeout(function(){
					gogo(s);
				},fi.speed);				
			});		
		});
		//右按钮
		fi._$btnR.mousedown(function(){
			var s=fi._$tabs.index(fi._$tabs.filter(".now")[0]);
			if((s==fi._$tabs.length-1)||fi._$list.is(":animated")) return;
			clearInterval(fi.autoPlay);
			clearTimeout(fi.autoPlay1);
			_this.alt(fi,s+1,function(){
				fi.autoPlay1=window.setTimeout(function(){
					s=(s+2)==fi._$tabs.length?0:(s+2);
					gogo(s);
				},fi.speed);				
			});
		});
		
		fi.$d.find(".fi_ct").click(function(e){
			window.open(fi._$tabs.filter(".now").find("a").attr("href"));
		});
	},
	alt:function(fi,i,cbk){
		fi._$tabs.removeClass("now").eq(i).addClass("now");
		//回填数据
		fi._$titleC.html(fi.focusData[i].t);
		//透明层交互
		fi._$transparentOvl.css("width",0).stop().animate({width:fi.ptStepX},200);
		//图片列表交互
		fi._$list.stop().animate({left:-i*fi.ptStepX},200,cbk);
		return false;
	}
});
