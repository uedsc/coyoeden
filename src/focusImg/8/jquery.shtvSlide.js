/**
 * @author levinhuang
 * @�Ѻ�����-���Ӿ���ҳ����ͼ
 * @version 2010.10.12
 * @dependency jquery.SOHUFocusImageCore.js
 */
/* ����ͼ8 */
$.fn.focusImg.Register("fi08",{
	init:function(fi){
		//dom references and cache
		fi._$tabC=fi.$d.find(".fi_tab");
		//fi._$titleC=fi.$d.find(".fi_tt");
		fi._$transparentOvl=fi.$d.find(".fi_ovl").css("opacity",fi._cfg.opacity||0.5);
		fi._$list=fi._$tabC.find("ul");
		fi._$btnL=fi._$tabC.find(".fi_left a");
		fi._$btnR=fi._$tabC.find(".fi_right a");
		fi._$tabs=fi._$list.find("li");
		fi._$img=fi.$d.find(".fi_player");
		//ˮƽ���򲽳�
		fi.ptStepX=fi._$tabs.eq(0).width();
		//����tab���ܳ���
		fi._$list.css("width",fi.ptStepX*fi._$tabs.length);
		//����tab�������趨��ť�Ƿ�ɵ��
		fi._cfg.displayNum=fi._cfg.displayNum||4;
		if(fi._$tabs.length<=fi._cfg.displayNum){
			fi._$btnR.addClass("fi_btn_off");
		}else{
			fi._$btnL.addClass("fi_btn_on");
		};
		//����tabͼƬ��͸��
		fi._$tabC.find("img").css("opacity",fi._cfg.opacity||0.5);	
	},
	initEvts:function(fi,gogo){
		var _this=this;
		//�󶨸���Ԫ�ص��¼�
		fi._$tabC.find("a").click(function(e){/*ʧ���Ա��Ƴ�����*/this.blur();return false;});
		//��ť
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
		//�Ұ�ť
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
			window.open(fi._curLink);
		});
	},
	alt:function(fi,i,cbk){
		if(fi._$curImg){
			fi._$curImg.stop(true,true).animate({opacity:fi._cfg.opacity||0.5},200);
		};
		fi._$curTab=fi._$tabs.removeClass("now").eq(i).addClass("now");
		fi._$curImg=fi._$curTab.find("img").stop(true,true).animate({opacity:1},200);
		fi._curLink=fi._$curTab.find("a").attr("href");
		//��������
		fi._$img.attr("src",fi._$curImg.attr("src")).css("opacity",0).stop(true,true).animate({opacity:1},200);
		//ͼƬ�б���
		fi._$list.stop(true,true).animate({left:-i*fi.ptStepX},200,cbk);
		return false;
	}
});