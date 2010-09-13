var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);
	this.id=this.$d.attr("id");
	this.speed=cfg.speed||5000;
	this.type=cfg.type||"1";
	this.ptStepY=cfg.ptStepY||90;
	this.focusData=data;
	this.autoPlay;
	this.autoPlay1;
	//私有属性-用于缓存dom对象提升效能
	this._$tabC=null;							/* tab标签容器,init方法中生成 */
	this._$tabs=null;							/* tab标签 */
	this._$titleC=null;							/* 标题容器,init方法中生成 */
	this._$desc=null;
	this._$img=null;							/* 图片对象 */
	this._$transparentOvl=null;					/* 透明蒙层 */
	//缓存数据
	this.$d.data("apple",{a:cfg,b:data});	
	//初始化
	this.init();			
};
MDC_FocusImage.prototype={
	init:function(){
		//创建框架
		var html='<div class="fi06_'+this.type+' clear"'+ 'id="'+this.id+'_'+this.type+'">';
		html+='<div class="content l">';
		html+='<a target="_blank"><img class="player"/></a><div class="bg transparence"></div>';
		html+='<div class="note"><h1 class="tt"></h1><p class="desc"></p></div>';
		html+='<span class="playButton"></span>';
		html+='</div>';//l
		html+='<div class="tab r">';
		html+='<span class="up"><a href="#" class="btn"></a></span><span class="down"><a href="#" class="btn"></a></span>';
		html+='<div class="tab_"><dl></dl></div>';
		html+='</div>';//r
		html+='</div>';
		this.$d.html(html);
		//dom references and cache
		this._$img=this.$d.find(".player");
		this._$tabC=this.$d.find(".tab dl");
		this._$titleC=this.$d.find(".tt");
		this._$desc=this.$d.find(".desc");
		this._$transparentOvl=this.$d.find(".transparence");
		this._$note=this.$d.find(".note");
		this._$tabOvl=this.$d.find(".tab_");
		//create tabs for navigation
		html="";
		for(var i=0;i<this.focusData.length;i++){
			this.focusData[i].p1=((!this.focusData[i].p1)||(this.focusData[i].p1==""))?this.focusData[i].p:this.focusData[i].p1;				
			html+='<dd><img src="'+this.focusData[i].p1+'"/><h2 class="tt0"><span class="icon"></span>'+this.focusData[i].t_+'</h2><p class="desc0">'+this.focusData[i].t1_+'</p></dd>';
		};
		this._$tabC.html(html);
		this._$tabs=this._$tabC.find("dd");
		
		//触发及注册事件
		this.spring();
	},
	spring:function(){
		var that=this;
		var _num=this._$tabC.height()-this._$tabOvl.height();
		//执行交互并轮刷
		var gogo=function(i){
			var _index=i||0;
			that.alternation(_index);
			that.autoPlay=setInterval(function(){
				that.alternation((++_index)==that.focusData.length?_index=0:_index);
			},that.speed);
		};
		gogo();
		//绑定各种元素的事件
		this.$d.find(".btn").click(function(e){return false;});
		this.$d.find(".r > .up").mousedown(function(){
			if(that._$tabC.position().top<0){
				if(!that._$tabC.is(":animated")){
					clearInterval(that.autoPlay);
					clearTimeout(that.autoPlay1);
					that._$tabC.stop().animate({top:"+="+that.ptStepY},200,function(){
						that.autoPlay1=window.setTimeout(function(){
							var s=that._$tabs.filter(".now").index();
							s=(s-1)==-1?0:(s-1);
							gogo(s);
						},that.speed);
					});
				};
			};			
		});
		this.$d.find(".r > .down").mousedown(function(){
			if(that._$tabC.position().top>-_num){
				if(!that._$tabC.is(":animated")){
					clearInterval(that.autoPlay);
					clearTimeout(that.autoPlay1);
					that._$tabC.stop().animate({top:"-="+that.ptStepY},200,function(){
						that.autoPlay1=window.setTimeout(function(){
							var s=that._$tabs.filter(".now").index();
							s=(s+1)==that._$tabs.length?0:(s+1);
							gogo(s);
						},that.speed);						
					});
				};
			};	
		});
		this.$d.find(".content").hover(function(){
			$(this).find(".playButton").addClass("now");
		},function(){
			$(this).find(".playButton").removeClass("now");
		}).click(function(){
			window.open($(this).find("a:eq(0)").attr("href"));
			return false;
		});
		//给tab标签绑定事件
		this._$tabs.each(function(i){
			(function(i,o){
				jQuery(o).bind("mouseenter",function(){
					clearInterval(that.autoPlay);
					clearTimeout(that.autoPlay1);	
					if(this.className!="now"){
						gogo(i);
						clearInterval(that.autoPlay);
						clearTimeout(that.autoPlay1);
					};
				}).bind("mouseleave",function(){
					clearInterval(that.autoPlay);					
					that.autoPlay1=setTimeout(function(){
						var s=(i+1)==that.focusData.length?0:(i+1);
						gogo(s);
					},that.speed);
				}).click(function(){
					if(this.className!="now"){
						//clearTimeout(that.autoPlay);
						clearInterval(that.autoPlay);
						gogo(i);
					};
					window.open(that.focusData[i].l)
					return false;					
				});
			})(i,this)
		});//each
	},
	alternation:function(i){
		this._$img.attr("src",this.focusData[i].p).parent().attr("href",this.focusData[i].l);
		this._$tabs.removeClass("now").eq(i).addClass("now");
		//回填数据
		this._$titleC.html(this.focusData[i].t);
		this._$desc.html(this.focusData[i].t1);
		
		var _w=this._$note.width();
		this._$transparentOvl.css("width",0).stop().animate({width:_w},200);
		var _num1=this._$tabs.filter(".now").prevAll().length*this.ptStepY;
		var _num2=this._$tabC.height()-this._$tabOvl.height();
		var _num3=_num1-270;//TODO:270的逻辑怎么来
		if(!this._$tabC.is(":animated")){
			if(this._$tabC.position().top > -_num3){
				this._$tabC.stop().animate({top:-_num3},200);
			};
			if(this._$tabC.position().top < (-_num1)){				
				this._$tabC.stop().animate({top:-_num1},200);
			}
		}
		if(this._$tabs[0].className=="now"){			
			if(!this._$tabC.is(":animated")){				
				this._$tabC.stop().animate({top:0},200);
			}
		};
	}
};

