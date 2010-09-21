/*
 搜狐高清-电影首页焦点图插件
 1,l=layout
 2,s=slider
 */

;(function($) {
    // Private functions.
    var p = {};
    p.M = function($t,opts){
		this._o=opts; 
		this.$l=$t;
		this.$s=$t.find("."+opts.clSlider);
		//init
		this._i();
	};
	p.M.prototype={
		_i:function(){
			this.$s.slider();
		}
	};
    //main plugin body
    $.fn.ppSlide = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.ppSlide.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			new p.M($(this),opts);
        });
    };
    // Public defaults.
    $.fn.ppSlide.defaults = {
        clSlider: 'lv_slider_'
    };
    // Public functions.
    $.fn.ppSlide.method1 = function(skinName) {
        return;
    };
})(jQuery);  