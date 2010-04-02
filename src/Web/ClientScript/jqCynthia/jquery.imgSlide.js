; (function($) {
	// Private functions.
	var p = {};
	p.stop = function() { window.clearInterval(p._intervalID); };
	p.slide=function(opts) {
		 if (opts) {
			p._cur = opts.cur || 0;
		 } else {
			p._cur = (p._cur >= (p._cnt - 1)) ? 0 : (++p._cur);
		 };
		 p._i.filter(":visible").fadeOut(p._opts.fadeout, function() {
			p._i.eq(p._cur).fadeIn(p._opts.fadein);
			p._i.removeClass(p._opts.on).eq(p._cur).addClass(p._opts.on);
		 });
		 p._t.removeClass(p._opts.on).eq(p._cur).addClass(p._opts.on);
		 p._tbg.removeClass(p._opts.on).eq(p._cur).addClass(p._opts.on);
	}; //slide 
	p.go=function() {
		p.stop();
		p._intervalID = window.setInterval(function() { p.slide(); }, p._opts.interval);
	}; //go
	p.iMouseover = function(target, items) {
		p.stop();
		var i = $.inArray(target, items);
		p.slide({ cur: i });
	}; //iMouseover

	//main plugin body
	$.fn.imgSlide = function(options) {
		// Set the options.
		options = $.extend({}, $.fn.imgSlide.defaults, options);
		p._opts = options;
		// Go through the matched elements and return the jQuery object.
		return this.each(function() {
			//title
			p._t = $(p._opts.t, this);
			//title masks
			p._tbg = $(p._opts.tbg, this);
			//silde items
			p._i = $(p._opts.i, this);
			//count
			p._cnt = p._t.size();
			p._cur = 0;
			p._intervalID = null;
			p._t.hover(function(evt) { if ($(this).attr('class') !=p._opts.on) { p.iMouseover(this,p._t); } else { stop(); } }, p.go);
			p._i.hover(p.stop, p.go);
			//trigger the slidebox
			p.go();

		});
	};
	// Public defaults.
	$.fn.imgSlide.defaults = {
		on: 'cur',
		t: 'ul.slide_txt>li', //title css selector
		tbg: 'ul.op>li', //title backgrounds css selector
		i: 'ul.slide_pic li', //slide items  css selector
		interval: 5000,
		fadein: 300,
		fadeout: 200
	};
})(jQuery); 