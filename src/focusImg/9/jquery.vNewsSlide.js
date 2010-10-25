/**
 * @author levinhuang
 * @搜狐高清-视频新闻首页焦点图
 * @version 2010.10.26
 * @dependency jquery.SOHUFocusImageCore.js 
 */
;(function($){
	/* 焦点图09 */
	$.fn.focusImg.Register("fi09", {
	    init: function (fi) {
	        //dom references and cache
	        fi._$tabC = fi.$d.find(".fi_tab");
	        //tab列表
	        fi._$tabs = fi._$tabC.find("li");
	        fi._tabNum = fi._$tabs.length;		
			//左侧展示元素
			fi._$ctItems=fi.$d.find(".fi_ct_");
	    },
	    initEvts: function (fi, gogo) {
	        var _this = this;
	        //绑定各种元素的事件
	        fi._$tabC.find("a").click(function (e) { e.preventDefault(); return true; });
	    },
		/**
		 * 选中指定tab
		 * @param {Object} i
		 */
		setTab:function(fi,i,cbk){
	        fi._$curTab = fi._$tabs.removeClass("fi_now").eq(i).addClass("fi_now");
	        fi._$curItem = fi._$ctItems.hide().eq(i).fadeIn();
			fi._curLink=fi._$curTab.find("a:first").attr("href");	
		},
	    alt: function (fi, i, cbk) {
			//选中指定tab
			this.setTab(fi,i,cbk);
			
	        //返回false，不执行jquery.SOHUFocusImageCore.js中定义的通用的交互流程
	        return false;
	    }
	});	
})(jQuery);
