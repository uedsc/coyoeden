/**
 * @author levinhuang
 * @desc �Ѻ�������ҳ2010��10�°潹��ͼ
 * @version 2010.10.29
 * @dependency jquery.SOHUFocusImageCore.js 
 */
;(function($){
	/* ����ͼ10 */
	$.fn.focusImg.Register("fi10", {
	    init: function (fi) {
	        //dom references and cache
	        fi._$tabC = fi.$d.find(".fi_tab");
	        //tab�б�
	        fi._$tabs = fi._$tabC.find("li");
	        fi._tabNum = fi._$tabs.length;
			var w=fi._$tabs.outerWidth(true)*fi._tabNum;
			if($.browser.msie&&$.browser.version<7){w+=5;};
			fi._$tabC.width(w);
			//��ͼ
			fi._$ctItems=fi.$d.find(".fi_ct li");
			
			//ͼƬ�Ƿ�lazyload
			if(fi._cfg.lazyload){
				fi._$loading=$('<div class="fi_lazyload"></div>').appendTo(fi.$d);
				$(document).ready(function(){
					fi.$d.find("[data-src]").each(function(i,o){
						o=$(o);
						(function($t){
							var src=$t.attr("data-src"),tempImg=$("<img/>");
							tempImg.load(function(){
								$t.removeAttr("data-src").attr("src",src);
								tempImg=null;
							}).attr("src",src);					
						})(o);
					});					
				});
			};
			fi._cfg.lazyload_t=fi._cfg.lazyload_t||450;//�����setTab��fadeInʱ�������fadeInĬ��400��
	    },
	    initEvts: function (fi, gogo) {
	        var _this = this;
	        //�󶨸���Ԫ�ص��¼�
	        fi._$tabC.find("a").click(function (e) { e.preventDefault(); return true; }).end().parent().click(function(){return false;});
	    },
		/**
		 * ѡ��ָ��tab
		 * @param {Object} i
		 */
		setTab:function(fi,i,cbk){
	        fi._$curTab = fi._$tabs.removeClass("fi_now").eq(i).addClass("fi_now");
	        fi._$curItem = fi._$ctItems.hide().eq(i).fadeIn();//fadeInĬ����450��,fadeIn('fast')��200��
			fi._$curImg=fi._$curItem.find("img");
			fi._curLink=fi._$curTab.find("a:first").attr("href");	
		},
	    alt: function (fi, i, cbk) {	
			//ѡ��ָ��tab
			this.setTab(fi,i,cbk);
			//lazyload�Ĵ���
			var src=null;
			window.clearTimeout(fi._timer3);
			if((src=fi._$curImg.attr("data-src"))){
				if(fi._$loading){
					fi._timer3=window.setTimeout(function(){fi._$loading.show();},fi._cfg.lazyload_t);//�����setTab��fadeInʱ�����
				}else{
					fi._$curImg.attr("src",src);
				};
			}else{
				if(fi._$loading){fi._$loading.hide();};
			};			
	        //����false����ִ��jquery.SOHUFocusImageCore.js�ж����ͨ�õĽ�������
	        return false;
	    }
	});	
})(jQuery);