/**
 * ����ͼ���
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {cache:{}};
	p.M=function(cfg,data){
		this.$d=cfg.$d;								/* JQuery���� */
		this.flag=cfg.flag;							/* ����ͼ���ͱ�ʶ����fi01��ʾ����1�Ľ���ͼ */	
		this.id=this.$d[0].id;						/* dom��� */
		this.text=cfg.text||true;					/* �Ƿ���ʾ���� */
		this.speed=parseInt(cfg.speed)||5000;		/* ��ˢ�ٶ� */
		this.hoverStop=cfg.hoverStop;				/* ����Ƶ�tab��ʱ�Ƿ�ֹͣ���� */
		this.type=cfg.type;							/* ������ */
		this.place=cfg.place||'tabRB';				/* tab��ǩ��λ�� */
		this.myHtml=cfg.myHtml||"";					/* �û������html.��䵽class='content'�������� */
		this.ptStepX=cfg.ptStepX||92;				/* tabָ��ˮƽλ�Ʋ��� */
		this.ptStepY=cfg.ptStepY||81;				/* tabָ�봹ֱλ�Ʋ��� */
		this.ptStepX_=cfg.ptStepX_||4;				/* tabָ��ˮƽλ�Ʋ���ƫ��ֵ */
		this.ptStepY_=cfg.ptStepY_||-1;				/* tabָ�봹ֱλ�Ʋ��� ƫ��ֵ*/	
		this.focusData=data;						/* ͼƬjson���� */
		this.clickTabToNav=cfg.clickTabToNav||false;/* ���tab��ǩ�Ƿ񵼺� */
		this.autoPlay;								/* �Զ����ż�ʱ��id */
		this.autoPlay1;								/* ����tab������ʱ������һ�β��ŵļ�ʱ��id */
		
		//˽������-_$��ͷ�ı������ڻ���dom��������Ч��
		this._fiObj;								/* ����ͼ���ƶ��� */
		this._$tabC=null;							/* tab��ǩ����,init���������� */
		this._$tabs=null;							/* tab��ǩ */
		this._$titleC=null;							/* ��������,init���������� */
		this._$img=null;							/* ͼƬ���� */
		this._$transparentOvl=null;					/* ͸���ɲ� */
		this._$desc=null;							/* �������� */
		this._$pointer=null;						/* ����ͼָ�� */	
		//��������
		this.$d.data("apple",{a:cfg,b:data});	
		//��ʼ��
		this.init();
			
	};
	p.M.prototype={
		/**
			��ʼ��(init):
				1���������ʹ�������ͼ���;
				2�������ݻ���ṹ��.
		*/
		init:function(){
			//��ȡ����ͼ���ƶ���
			this._fiObj=p.cache[this.flag];
			if(!this._fiObj) return;
			//�������-������dom
			this._fiObj.init(this);
			//�����html����
			this.$d.find(".content").append(this.myHtml);
			//ִ�д���
			this.spring();
		},
		/**
			����(spring):
				1��tab�л��¼��İ�;
				2����ʱ����ˢ��ʵ��.
		*/
		spring:function(){
			var that=this;
			//ִ�н�������ˢ
			var gogo=function(i){
				var _index=i||0;
				that.alternation(_index);
				that.autoPlay=setInterval(function(){
					that.alternation((++_index)==that.focusData.length?_index=0:_index);
				},that.speed);
			};
			gogo();
			
			//��tab��ǩ��Click�¼�,���ڽ���ͼ�л�
			this._$tabs.each(function(i){
				(function(i,o){
					o=jQuery(o).click(function(){
						if(this.className!="now"){
							//clearTimeout(that.autoPlay);
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
			};
		},
		/**
			����(alternation):
				1����������ѡ���Ӧ�Ĺ�������;
				2�������л���ʵ��.
		*/
		alternation:function(i){			
			var continueCommon=true;
			//���ƵĽ���Ч��
			if(this._fiObj.alt)
				continueCommon=this._fiObj.alt(this,i);
				
			if(!continueCommon) return;
			//�����ͼ·����������·��,������ͼ�ӽ��Զ���
			this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({opacity:0}).stop().animate({opacity:1},500);		
			//��ǩ�л�����
			this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");	
			//���ݻ���
			this._$titleC.html(this.focusData[i].t);
			if(this._$desc)
				this._$desc.html(this.focusData[i].t1);
			
		}
	};
    //main plugin body
	/**
	 * focusImg���
	 * @param {Object} cfg ����ͼ����������
	 * @param {Object} data ����ͼ��ͼƬ����
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
	 * ע�ό��ͼ���ƶ���
	 * @param {Object} key ������fi01��ʾ����ͼ����1
	 * @param {Object} fiObj ��{init,initEvts,alt}
	 */
    $.fn.focusImg.Register = function(key,fiObj) {
        p.cache[key]=fiObj;
    };
})(jQuery);

/* ����ͼ1 */
$.fn.focusImg.Register("fi01",{
	init:function(fi){
		//�������
		var html='<img class="iplayer"/>';
		html+='<div class="tab '+fi.place+'"></div>';
		html+='<h2 class="tt"></h2>';
		fi.$d.append(html);
		//��ʽ����dom����
		fi._$tabC=fi.$d.find(".tab");
		fi._$titleC=fi.$d.find(".tt");
		fi._$img=fi.$d.find("img");
		
		if(fi.text){
			fi._$titleC.show();
		}else{
			fi._$titleC.hide();
		};
		
		//�������ݴ���tab��ǩ
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			html+='<span>'+(i+1)+'</span>';
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("span");		
	},
	initEvts:function(fi){
		//����������Click�¼�,���ڵ�������
		fi.$d.click(function(){window.open(fi._$img.attr("link"));});
	}
});
/* ����ͼ2 */
$.fn.focusImg.Register("fi02",{
	init:function(fi){
		//�������
		var html='<div id="'+fi.id+'_'+fi.type+'" class="fi02_'+fi.type+'">';
		html+='<img class="iplayer"/>';
		html+='<div class="transparence"></div>';
		html+='<h2 class="tt"></h2>';
		html+='<div class="focusImageBg"></div>';
		html+='<div class="link"></div>';
		html+='<div class="tab"></div>';
		html+='</div>';
		fi.$d.append(html);
		//dom����
		fi._$tabC=fi.$d.find(".tab");
		fi._$titleC=fi.$d.find(".tt");
		fi._$img=fi.$d.find("img");
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		//�������ݴ���tab��ǩ-ע����̬�������domʱ���ȹ���������html����Ч��
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			html+="<span>"+(i+1)+"</span>";
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("span");
	},
	initEvts:function(fi){
		//����������Click�¼�,���ڵ�������
		fi.$d.click(function(){window.open(fi._$img.attr("link"));});
	}
});
/* ����ͼ3 */
$.fn.focusImg.Register("fi03",{
	init:function(fi){
		//�������-ע:����html�ַ���Ȼ��һ���Թ���dom����ȶ��ʹ��$(html)��Ч
		var html='<div class="fi03_'+fi.type+'"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="content">';
		html+='<img class="iplayer"/><div class="transparence"></div><h2></h2><p></p>';
		html+='</div>';
		html+='<div class="tab"></div>';
		html+='</div>';
		fi.$d.append(html);
		//��ʽ����dom����
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		fi._$img=fi.$d.find("img");
		fi._$titleC=fi.$d.find("h2");
		fi._$desc=fi.$d.find("p");
		fi._$tabC=fi.$d.find(".tab");
		
		//�������ݴ���tab��ǩ-ע����̬�������domʱ���ȹ���������html����Ч��
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			html+="<span>"+(i+1)+"</span>";
		};
		fi._$tabC.append(html);
		fi._$tabs=fi._$tabC.find("span");
	},
	initEvts:function(fi){
		//����������Click�¼�,���ڵ�������
		fi.$d.find(".content").click(function(){window.open(fi._$img.attr("link"));});
	},
	alt:function(fi,i){
		//͸��������
		if(fi.type==1){			
			fi._$transparentOvl.css({
				left:-600
			}).stop().animate({"left":0},200);				
		};
		return true;
	}
});
/* ����ͼ4 */
$.fn.focusImg.Register("fi04",{
	init:function(fi){
		//�������-�ȹ���html������$(html)����dom����,������ʹ��$(html)
		var html='<div class="fi04_'+fi.type+'"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="content">';
		html+='<img class="iplayer"/><div class="transparence"></div><h2></h2><p></p>';
		html+='</div>';
		html+='<div class="tab"><span class="pointer"></span><dl></dl></div>';
		html+='</div>';
		fi.$d.append(html);
		
		//��ʽ����dom����
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		fi._$img=fi.$d.find("img");
		fi._$titleC=fi.$d.find("h2");
		fi._$desc=fi.$d.find("p");
		fi._$tabC=fi.$d.find("dl");
		fi._$pointer=fi.$d.find(".pointer");
		
		//�������ݴ���tab��ǩ
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;
			html+='<dd><img src="'+fi.focusData[i].p1+'"/></dd>';
		};
		fi._$tabC.append(html).find("img").css("opacity",0.5);
		fi._$tabs=fi._$tabC.find("dd");
	},
	initEvts:function(fi){
		//����������Click�¼�,���ڵ�������
		fi.$d.find(".content").click(function(){window.open(fi._$img.attr("link"));});
		fi._$pointer.click(function(){window.open(fi._$img.attr("link"));});
	},
	alt:function(fi,i){
		fi._$tabs.filter(".now").children().animate({"opacity":.5},200);
		fi._$tabs.eq(i).children().animate({"opacity":1},200);
		//ָ���������				
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
/* ����ͼ5 */
$.fn.focusImg.Register("fi05",{
	init:function(fi){
		//�������-�ȹ���html������$(html)����dom����,������ʹ��$(html)
		var html='<div class="fi05_'+fi.type+'"'+ 'id="'+fi.id+'_'+fi.type+'">';
		html+='<div class="content">';
		html+='<img class="iplayer"/><div class="transparence"></div><h2></h2>';
		html+='</div>';
		html+='<div class="tab"><dl></dl></div>';
		html+='</div>';
		fi.$d.append(html);
		
		//��ʽ����dom����
		fi._$transparentOvl=fi.$d.find(".transparence").css("opacity",0.5);
		fi._$img=fi.$d.find("img");
		fi._$titleC=fi.$d.find("h2");
		fi._$tabC=fi.$d.find("dl");		
		
		//�������ݴ���tab��ǩ		
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;
			html+='<dd><img src="'+fi.focusData[i].p1+'"/><p>'+fi.focusData[i].t+'</p></dd>';
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
		})		
		//����������Click�¼�,���ڵ�������
		fi.$d.find(".content").click(function(){window.open(fi._$img.attr("link"));});
	},
	alt:function(fi,i){
		//͸��������,����ͬ����
		fi._$transparentOvl.css({
			bottom:-40
		}).stop().animate({"bottom":0},200);	
					
		fi._$titleC.css({
			bottom:-24
		}).stop().animate({"bottom":16},200);		
		return true;
	}
});
/* ����ͼ6 */
$.fn.focusImg.Register("fi06",{
	init:function(fi){
		//�������
		var html='<div class="fi06_'+fi.type+' clear"'+ 'id="'+fi.id+'_'+fi.type+'">';
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
		fi.$d.html(html);
		//dom references and cache
		fi._$img=fi.$d.find(".player");
		fi._$tabC=fi.$d.find(".tab dl");
		fi._$titleC=fi.$d.find(".tt");
		fi._$desc=fi.$d.find(".desc");
		fi._$transparentOvl=fi.$d.find(".transparence");
		fi._$note=fi.$d.find(".note");
		fi._$tabOvl=fi.$d.find(".tab_");
		//create tabs for navigation
		html="";
		for(var i=0;i<fi.focusData.length;i++){
			fi.focusData[i].p1=((!fi.focusData[i].p1)||(fi.focusData[i].p1==""))?fi.focusData[i].p:fi.focusData[i].p1;				
			html+='<dd><img src="'+fi.focusData[i].p1+'"/><h2 class="tt0"><span class="icon"></span>'+fi.focusData[i].t_+'</h2><p class="desc0">'+fi.focusData[i].t1_+'</p></dd>';
		};
		fi._$tabC.html(html);
		fi._$tabs=fi._$tabC.find("dd");	
	},
	initEvts:function(fi,gogo){
		var _num=fi._$tabC.height()-fi._$tabOvl.height();
		//�󶨸���Ԫ�ص��¼�
		fi.$d.find(".btn").click(function(e){return false;});
		fi.$d.find(".r > .up").mousedown(function(){
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
		fi.$d.find(".r > .down").mousedown(function(){
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
		fi.$d.find(".content").hover(function(){
			$(this).find(".playButton").addClass("now");
		},function(){
			$(this).find(".playButton").removeClass("now");
		}).click(function(){
			window.open($(this).find("a:eq(0)").attr("href"));
			return false;
		});
	},
	alt:function(fi,i){
		fi._$img.attr("src",fi.focusData[i].p).parent().attr("href",fi.focusData[i].l);
		fi._$tabs.removeClass("now").eq(i).addClass("now");
		//��������
		fi._$titleC.html(fi.focusData[i].t);
		fi._$desc.html(fi.focusData[i].t1);
		
		var _w=fi._$note.width();
		fi._$transparentOvl.css("width",0).stop().animate({width:_w},200);
		var _num1=fi._$tabs.filter(".now").prevAll().length*fi.ptStepY;
		var _num2=fi._$tabC.height()-fi._$tabOvl.height();
		var _num3=_num1-270;//TODO:270���߼���ô��
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
