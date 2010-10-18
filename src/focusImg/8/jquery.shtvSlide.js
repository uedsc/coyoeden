/**
 * @author levinhuang
 * @�Ѻ�����-���Ӿ���ҳ����ͼ
 * @version 2010.10.12
 * @dependency jquery.SOHUFocusImageCore.js 
 */
;(function($){
	/* ����ͼ8 */
	$.fn.focusImg.Register("fi08", {
	    init: function (fi) {
			//�ұ�������-�����ƶ�ƫ��ֵ����padding+margin
			fi.ptStepY_=fi.ptStepY_||20;
			//�ұ�������-ѡ����߶�
			fi.ptStepY=fi.ptStepY||160;
			//�ұ�������-Ĭ�ϸ߶�
			fi.ptStepY1=fi._cfg.ptStepY1||50;
			//�ұ����ֹ����ٶ�
			fi.speed1=fi._cfg.speed1||200;
			//�ұ��������Ƿ����
			fi.yRolling=fi._cfg.yRolling||false;
	        //dom references and cache
	        fi._$tabC = fi.$d.find(".fi_tab");
			fi._$transparentOvl = fi.$d.find(".fi_ovl").css("opacity", fi._cfg.opacity || 0.5);
	        //tab�б�
			fi._$list = fi._$tabC.find("ul");
	        fi._$tabs = fi._$list.find("li");
	        fi._tabNum = fi._$tabs.length;		
			//�Ҳ������б�
			fi._$list1=fi.$d.find(".fi_note ul");
			//���������б�li������
			fi._$list1.find("li").each(function(i,o){
				$(o).data("id",i);
			});
			//tab��ť
	        fi._$btnL = fi._$tabC.find(".fi_left a");
	        fi._$btnR = fi._$tabC.find(".fi_right a");
			//��ͼ
	        fi._$img = fi.$d.find(".fi_player");
	        //ˮƽ���򲽳�
	        fi.ptStepX = fi._$tabs.eq(0).width();
	        //����tab���ܳ���
	        fi._$list.css("width", fi.ptStepX * fi._tabNum);
	        //չʾ��tab�������趨��ť��ʼ״̬
	        fi._cfg.displayNum = fi._cfg.displayNum || 4;
	        fi._$btnR.addClass("fi_btn_on");
			fi._$btnL.addClass("fi_btn_off");
	        //����tabͼƬ��͸��
	        fi._$tabC.find("img").css("opacity", fi._cfg.opacity || 0.5);
	    },
	    initEvts: function (fi, gogo) {
	        var _this = this;
	        //�󶨸���Ԫ�ص��¼�
	        fi._$tabC.find("a").click(function (e) { /*ʧ���Ա��Ƴ�����*/this.blur(); e.preventDefault(); return true; });
	        //��ť
	        fi._$btnL.mousedown(function () {
	            var s = fi._$tabs.index(fi._$tabs.filter(".now")[0]);
	            if ((s==0)||fi._$list.is(":animated") || $(this).hasClass("fi_btn_off")) return;
	            clearInterval(fi.autoPlay);
	            clearTimeout(fi.autoPlay1);
				
				_this.alt(fi,s-1,function(){
					fi.autoPlay1=window.setTimeout(function(){
						gogo(s);
					},fi.speed);				
				});	
	
	        });
	        //�Ұ�ť
	        fi._$btnR.mousedown(function () {
	            var s = fi._$tabs.index(fi._$tabs.filter(".now")[0]);
	            if ((s ==fi._tabNum-1) || fi._$list.is(":animated") || $(this).hasClass("fi_btn_off")) return;
	            clearInterval(fi.autoPlay);
	            clearTimeout(fi.autoPlay1);
				
				_this.alt(fi,s+1,function(){
					fi.autoPlay1=window.setTimeout(function(){
						s=(s+2)==fi._tabNum?0:(s+2);
						gogo(s);
					},fi.speed);				
				});
	        });
	
	        fi.$d.find(".fi_player_c").click(function (e) {
	            window.open(fi._curLink);
	        });
	    },
		/**
		 * �Ҳ������б���𲽹���
		 */
		roll:function(fi,i){
			var i1=0;
			var liNow=$.grep(fi._$list1.find("li"),function(o,j){
				if($(o).data("id")==i){
					i1=j;
					return true;
				};
				return false;
			});		
			if(i1==0) return;
			var step=fi.ptStepY+(i1-1)*fi.ptStepY1+i1*fi.ptStepY_;
			liNow=$(liNow);
			var $prev=liNow.prevAll();
			if(fi.yRolling){
				//����Ч��
				//TODO:�����ƶ����ʱ���ұ������б���ܳ�����������ѡ��״̬
				//���ƶ���Ԫ�ص�ĩβ
				for(var k=$prev.length-1;k>=0;k--){
					fi._$list1.append($prev.eq(k).clone(true).removeClass("fi_note_on").height(fi.ptStepY1));
				};			
				fi._$list1.stop(true,true).animate({top:-step},fi.speed1,function(){
					//�Ƴ�����Ԫ��
					$prev.remove();
					fi._$list1.css("top",0);
					liNow.addClass("fi_note_on").css("height",fi.ptStepY);
					
				});
				
				/*
				liNow.stop(true,false).animate({height:fi.ptStepY},fi.speed1,function(){
					liNow.addClass("fi_note_on");
				});
				var $prev=liNow.prevAll().stop(true,true).filter(".fi_note_on").animate({height:fi.ptStepY1},fi.speed1,function(){
					$(this).removeClass("fi_note_on");
				}).end();
				fi._$list1.stop(true,false).animate({top:-step},fi.speed1,function(){
					fi._$list1.css("top",0);
					for(var k=$prev.length-1;k>=0;k--){
						fi._$list1.append($prev[k]);
					};
				});
				*/				
			}else{
				//��������Ч��
				fi._$list1.css("top",-step);
				for(var k=$prev.length-1;k>=0;k--){
					fi._$list1.append($prev.eq(k).removeClass("fi_note_on").height(fi.ptStepY1));
				};
				fi._$list1.css("top",0);
				liNow.addClass("fi_note_on").height(fi.ptStepY);
			};			
		},
		tabOffset:function(fi,i){
			var r={visible:false};
			//���㵱ǰtab�Ƿ�����ʾ����
	        r.val = (i + 1) * fi.ptStepX + fi._$list.position().left;
	        //��ǰtab����ʾ��
	        if (r.val > 0 && r.val <= fi._cfg.displayNum * fi.ptStepX){
				r.visible=true;
			};
			return r;
		},
		/**
		 * ��ť״̬����
		 * @param {Object} fi
		 * @param {Object} i
		 */
		btnTrack:function(fi,i){
	        //���Ұ�ť����
	        if (i == 0) {
	            //������
	            fi._$btnR.removeClass("fi_btn_off").addClass("fi_btn_on");
	            fi._$btnL.removeClass("fi_btn_on").addClass("fi_btn_off");
	        } else if (i == (fi._tabNum - 1)) {
	            //�����Ұ�
	            fi._$btnL.removeClass("fi_btn_off").addClass("fi_btn_on");
	            fi._$btnR.removeClass("fi_btn_on").addClass("fi_btn_off");
	        } else {
	            //����
	            fi._$btnL.removeClass("fi_btn_off").addClass("fi_btn_on");
	            fi._$btnR.removeClass("fi_btn_off").addClass("fi_btn_on");
	        };		
		},
		/**
		 * ѡ��ָ��tab
		 * @param {Object} i
		 */
		setTab:function(fi,i,cbk){
			if (fi._$curImg) {
	            fi._$curImg.stop(true, true).animate({ opacity: fi._cfg.opacity || 0.5 }, 200);
	        };
	        fi._$curTab = fi._$tabs.removeClass("now").eq(i).addClass("now");
	        fi._$curImg = fi._$curTab.find("img").stop(true, true).animate({ opacity: 1 }, 200,cbk);
			/* �Ҳ������б�������� */
			this.roll(fi,i);
			//��ǰ����
			fi._$curLink=fi._$curTab.find("a");		
	        fi._curLink = fi._$curLink.attr("href");
	        //��������
			fi._curBImg=fi._$curLink.attr("rel")||"";
			fi._curBImg=fi._curBImg==""?fi._$curImg.attr("src"):fi._curBImg;
	        fi._$img.attr("src", fi._curBImg).css("opacity", 0).stop(true, true).animate({ opacity: 1 }, 200);		
			
		},
	    alt: function (fi, i, cbk) {
			//ѡ��ָ��tab
			this.setTab(fi,i,cbk);
			
	        /* ͼƬtab�б��� */
	        //tab����С�ڵ�����ʾ��
	        if (fi._tabNum <= fi._cfg.displayNum) return false;
			//��ť״̬
			this.btnTrack(fi,i);
	        //���㵱ǰtab�Ƿ�����ʾ����
	        var offset =this.tabOffset(fi,i);
	        //��ǰtab����ʾ��
	        if (offset.visible) return false;
	        //��ǰtab������ʾ������Ҫ�ƶ�tab�б�
	        fi._$list.stop(true, true).animate({ left: (offset.val <= 0 ? "+=" : "-=") + fi.ptStepX }, 200);
	        //����false����ִ��jquery.SOHUFocusImageCore.js�ж����ͨ�õĽ�������
	        return false;
	    }
	});	
})(jQuery);
