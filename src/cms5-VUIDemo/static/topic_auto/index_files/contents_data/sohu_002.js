/**
 * 针对指定元素的简单滑动(前滑动,后滑动)插件
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
	//获取边界元素
	p.brimItem=function($t,cfg,first){
		var obj=null;
		if(first){
			obj=$t.find(cfg.opts.cssItem).filter(":first");
		}else{
			obj=$t.find(cfg.opts.cssItem).filter(":last");
		};
		return obj;
	};
    //为元素加滑动功能
	p.slide=function($t,cfg,$items){
		var tag=cfg.opts.vertical?"top":"left";
		var marginTag="margin-"+tag;
		var s=function(){
			var bItem=p.brimItem($t,cfg,(cfg.opts.step<0));//边界元素
			var bItemCloned=null;
			if(cfg.opts.cloneItem){
				bItemCloned=bItem.clone(true);
				bItemCloned.mTag=parseInt(bItem.css(marginTag));
				if(cfg.opts.step<0){
					$t.append(bItemCloned);
				}else{
					$t.prepend(bItemCloned.css(marginTag,-cfg.opts.step+bItemCloned.mTag));
				};
			};
			var e={};
			e[tag]=cfg.opts.step;
			$t.animate(e,"slow",function(){
				bItem.remove();
				if(cfg.opts.step<0){
					if(!cfg.opts.cloneItem){
						$t.append(bItem);
					};
				}else{
					if(!cfg.opts.cloneItem){
						$t.prepend(bItem);
					}else{
						bItemCloned.css(marginTag,bItemCloned.mTag);
					};
				};
				e[tag]=0;
				$t.css(e);
			});	
		};
		var loop=function(){
			if (cfg.opts.auto) {
				$t.intervalID = window.setInterval(s, cfg.opts.interval);
			};
		};
		loop();
		//按钮事件
		cfg.$btnPrev.click(function(evt){
			if($t.intervalID){
				clearInterval($t.intervalID);
			};
			cfg.opts.step=-Math.abs(cfg.opts.step);
			if(cfg.opts.onPrePrev){
				if(!cfg.opts.onPrePrev($t,cfg)) return false;
			};
			s();
			return false;
		}).mouseout(function(evt){
			loop();
		});
		cfg.$btnNext.click(function(evt){
			if($t.intervalID){
				clearInterval($t.intervalID);
			};
			cfg.opts.step=Math.abs(cfg.opts.step);
			if(cfg.opts.onPreNext){
				if(!cfg.opts.onPreNext($t,cfg)) return false;
			};
			s();
			return false;
		}).mouseout(function(evt){
			loop();
		});
		//滑动元素的鼠标事件
		if(cfg.opts.iMouseOver){
			$items.mouseover(cfg.opts.iMouseOver);
		};
		if(cfg.opts.iMouseOut){
			$items.mouseout(cfg.opts.iMouseOut);
		};
		if(cfg.opts.iClick){
			$items.click(cfg.opts.iClick);
		};
	};
	p.make = function($t) {
		var cfg=$t.data("cfg");
		var items=cfg.$items;
		if(items.length<1) return;
		
		p.slide($t,cfg,items);
	};
    //main plugin body
    $.fn.cycleSlide = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.cycleSlide.defaults, opts);
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			//缓存该滑动实体的数据
			var data={
				opts:opts,
				$btnPrev:$(opts.cssBtnPrev),
				$btnNext:$(opts.cssBtnNext),
				$items:$this.find(opts.cssItem)
			};
			$this.data("cfg",data);//should i do it via $this.cfg=data?
			p.make($this);
        });
    };
    // Public defaults.
    $.fn.cycleSlide.defaults = {
		cssItem:'>*',/*待滑动的元素*/
        cssBtnPrev: '.btnSlidePrev',/*向前导航按钮*/
		cssBtnNext:'.btnSlideNext',/*向后导航按钮*/
		step:50,/*每滑动一次的步长.单位像素*/
		vertical:false,
		auto:false,
		interval:2000,
		cloneItem:false,
		onPrePrev:null,/*向前滑动前的回调函数，如果返回false则终止本次滑动*/
		onPreNext:null,/*向后滑动前的回调函数,如果返回false则终止本次滑动*/
		iMouseOver:null,
		iMouseOut:null,
		iClick:null
    };
})(jQuery);  