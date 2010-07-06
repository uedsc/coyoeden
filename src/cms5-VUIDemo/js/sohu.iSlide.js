/**
 * 针对指定元素的简单滑动(前滑动,后滑动)插件
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
    //为元素加分页功能
	p.make = function($t) {
		var cfg=$t.data("cfg");
		var items=cfg.$items;
		if(items.length<1) return;
		var slide=function(){
			if(cfg.opts.vertical){
				//垂直
				
			}else{
				//水平
				
			};
		};
	};
    //main plugin body
    $.fn.iSlide = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.iSlide.defaults, opts);
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			//缓存该滑动实体的数据
			var data={
				opts:opts,
				$btnPrev:$this.find(opts.cssBtnPrev),
				$btnNext:$this.find(opts.cssBtnNext),
				$items:$this.find(opts.cssItem)
			};
			$this.data("cfg",data);//should i do it via $this.cfg=data?
			p.make($this);
        });
    };
    // Public defaults.
    $.fn.iSlide.defaults = {
		cssItem:'>*',/*待滑动的元素*/
        cssBtnPrev: '.btn0',/*向前导航按钮*/
		cssBtnNext:'.btn1',/*向后导航按钮*/
		alwaysNav:false,/*不管是否有分页都显示导航按钮*/
		step:1,/*每滑动一次的步长.单位像素*/
		cycle:true,
		vertical:false,
		auto:false,
		interval:2000
    };
})(jQuery);  