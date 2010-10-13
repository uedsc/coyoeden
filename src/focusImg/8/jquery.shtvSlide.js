/**
 * @author levinhuang
 * @�Ѻ�����-���Ӿ���ҳ����ͼ
 * @version 2010.10.12
 * @dependency jquery.SOHUFocusImageCore.js
 */
/* ����ͼ8 */
$.fn.focusImg.Register("fi08", {
    init: function (fi) {
		fi.ptStepY_=fi.ptStepY_||50;
		fi.ptStepY=fi.ptStepY||180;
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
        //����tab�������趨��ť�Ƿ�ɵ��
        fi._cfg.displayNum = fi._cfg.displayNum || 4;
        if (fi._tabNum <= fi._cfg.displayNum) {
            fi._$btnR.addClass("fi_btn_off");
        } else {
            fi._$btnL.addClass("fi_btn_on");
        };
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
            if ((s==fi._tabNum-1)||fi._$list.is(":animated") || $(this).hasClass("fi_btn_off")) return;
            clearInterval(fi.autoPlay);
            clearTimeout(fi.autoPlay1);

			s=(s+1==fi._tabNum)?0:(s+1);
			_this.alt(fi,s,function(){
				fi.autoPlay1=window.setTimeout(function(){
					gogo(s-1);
				},fi.speed);				
			});	

        });
        //�Ұ�ť
        fi._$btnR.mousedown(function () {
            var s = fi._$tabs.index(fi._$tabs.filter(".now")[0]);
            if ((s ==0) || fi._$list.is(":animated") || $(this).hasClass("fi_btn_off")) return;
            clearInterval(fi.autoPlay);
            clearTimeout(fi.autoPlay1);
			
			s=(s-1<0)?0:(s-1);
			_this.alt(fi,s,function(){
				fi.autoPlay1=window.setTimeout(function(){
					gogo(s+1);
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
		var step=fi.ptStepY+(i1-1)*fi.ptStepY_;
		fi._$list1.stop(true,true).animate({top:-step},200,function(){
			fi._$list1
				.append($(liNow).prevAll().removeClass("fi_note_on").stop(true,true).height(fi.ptStepY_))
				.css("top",0)
				.find("li:first").stop(true,true)
				.addClass("fi_note_on")
				.css("height",fi.ptStepY_)
				.animate({height:fi.ptStepY},300);
		});		
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
            //�ұ߰���߰�
            fi._$btnR.removeClass("fi_btn_on").addClass("fi_btn_off");
            fi._$btnL.removeClass("fi_btn_off").addClass("fi_btn_on");
        } else if (i == (fi._tabNum - 1)) {
            //��߰��ұ���
            fi._$btnL.removeClass("fi_btn_on").addClass("fi_btn_off");
            fi._$btnR.removeClass("fi_btn_off").addClass("fi_btn_on");
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
        fi._curLink = fi._$curTab.find("a").attr("href");
        //��������
        fi._$img.attr("src", fi._$curImg.attr("src")).css("opacity", 0).stop(true, true).animate({ opacity: 1 }, 200);		
		
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