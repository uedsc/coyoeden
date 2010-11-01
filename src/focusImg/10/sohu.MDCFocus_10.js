/**
 * @author levinhuang
 * @desc 搜狐新闻首页2010年10月版焦点图
 * @version 2010.10.29
 * @dependency jquery.SOHUFocusImageCore.js 
 */
;(function($){
	/* 焦点图10 */
	$.fn.focusImg.Register("fi10", {
	    init: function (fi) {
	        //dom references and cache
	        fi._$tabC = fi.$d.find(".fi_tab");
	        //tab列表
	        fi._$tabs = fi._$tabC.find("li");
	        fi._tabNum = fi._$tabs.length;
			var w=fi._$tabs.outerWidth(true)*fi._tabNum;
			if($.browser.msie&&$.browser.version<7){w+=5;};
			fi._$tabC.width(w);
			//大图
			fi._$ctItems=fi.$d.find(".fi_ct li");
			
			//图片是否lazyload
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
			fi._cfg.lazyload_t=fi._cfg.lazyload_t||450;//请根据setTab的fadeIn时间调整，fadeIn默认400秒
	    },
	    initEvts: function (fi, gogo) {
	        var _this = this;
	        //绑定各种元素的事件
	        fi._$tabC.find("a").click(function (e) { e.preventDefault(); return true; }).end().parent().click(function(){return false;});
	    },
		/**
		 * 选中指定tab
		 * @param {Object} i
		 */
		setTab:function(fi,i,cbk){
	        fi._$curTab = fi._$tabs.removeClass("fi_now").eq(i).addClass("fi_now");
	        fi._$curItem = fi._$ctItems.hide().eq(i).fadeIn();//fadeIn默认是450秒,fadeIn('fast')是200秒
			fi._$curImg=fi._$curItem.find("img");
			fi._curLink=fi._$curTab.find("a:first").attr("href");	
		},
	    alt: function (fi, i, cbk) {	
			//选中指定tab
			this.setTab(fi,i,cbk);
			//lazyload的处理
			var src=null;
			window.clearTimeout(fi._timer3);
			if((src=fi._$curImg.attr("data-src"))){
				if(fi._$loading){
					fi._timer3=window.setTimeout(function(){fi._$loading.show();},fi._cfg.lazyload_t);//请根据setTab的fadeIn时间调整
				}else{
					fi._$curImg.attr("src",src);
				};
			}else{
				if(fi._$loading){fi._$loading.hide();};
			};			
	        //返回false，不执行jquery.SOHUFocusImageCore.js中定义的通用的交互流程
	        return false;
	    }
	});	
})(jQuery);