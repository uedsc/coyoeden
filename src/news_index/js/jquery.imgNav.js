;(function($) {
	// Private functions.
	var p = {};
	p.showC = function(opts) {
		///<summary>show content of a specified navigation</summary>
		p._clist.hide().filter(opts.filter).show();
	}; //showNav
	p.onNav = function(evt) {
		p._i=$(this);
		p._alist.removeClass(p._opts.on);
		p._i.addClass(p._opts.on);
		p.showC({ filter:p._i.attr("href") });
		return false;
	}; //onClick
	//main plugin body
	$.fn.imgNav = function(options) {
		// Set the options.
		options = $.extend({}, $.fn.imgNav.defaults, options);
		p._opts = options;
		// Go through the matched elements and return the jQuery object.
		return this.each(function() {
			p._alist = $("a", this);
			p._clist = $(p._opts.navc);
			p._alist.click(p.onNav);
			if(p._opts.mode=="hover"){
				p._alist.mouseover(p.onNav);
			};
		});
	};
	// Public defaults.
	$.fn.imgNav.defaults = {
		mode:'click',
		on: 'on',
		off: 'off',
		navc: '.navc'//nav content selector
	};
})(jQuery);