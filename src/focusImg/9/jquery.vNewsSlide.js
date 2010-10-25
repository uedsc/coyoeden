/**
 * @author levinhuang
 * @�Ѻ�����-��Ƶ������ҳ����ͼ
 * @version 2010.10.26
 * @dependency jquery.SOHUFocusImageCore.js 
 */
;(function($){
	/* ����ͼ09 */
	$.fn.focusImg.Register("fi09", {
	    init: function (fi) {
	        //dom references and cache
	        fi._$tabC = fi.$d.find(".fi_tab");
	        //tab�б�
	        fi._$tabs = fi._$tabC.find("li");
	        fi._tabNum = fi._$tabs.length;		
			//���չʾԪ��
			fi._$ctItems=fi.$d.find(".fi_ct_");
	    },
	    initEvts: function (fi, gogo) {
	        var _this = this;
	        //�󶨸���Ԫ�ص��¼�
	        fi._$tabC.find("a").click(function (e) { e.preventDefault(); return true; });
	    },
		/**
		 * ѡ��ָ��tab
		 * @param {Object} i
		 */
		setTab:function(fi,i,cbk){
	        fi._$curTab = fi._$tabs.removeClass("fi_now").eq(i).addClass("fi_now");
	        fi._$curItem = fi._$ctItems.hide().eq(i).fadeIn();
			fi._curLink=fi._$curTab.find("a:first").attr("href");	
		},
	    alt: function (fi, i, cbk) {
			//ѡ��ָ��tab
			this.setTab(fi,i,cbk);
			
	        //����false����ִ��jquery.SOHUFocusImageCore.js�ж����ͨ�õĽ�������
	        return false;
	    }
	});	
})(jQuery);
