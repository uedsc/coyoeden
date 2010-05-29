;(function($) {
	// Private functions.
	var p = {};
	p.showC = function(opts) {
		///<summary>show content of a specified navigation</summary>
		opts._clist.hide().filter(opts._i.attr("href")).show();
	}; //showNav
	p.onNav = function(evt) {
		var t=evt.data;
		t._i=$(this);
		t._alist.removeClass(t._opts.on);
		t._i.addClass(t._opts.on);
		t._index=$.inArray(this,t._alist);
		p.showC(t);
		if(t._opts.callback){
			t._opts.callback(t);	
		};
		return false;
	}; //onClick
	p.initNav=function(t){
		t._alist.bind("click",t,p.onNav);
		if(t._opts.mode=="hover"){
			t._alist.bind("mouseover",t,p.onNav);
		};
	};
	//main plugin body
	$.fn.imgNav = function(options) {
		// Set the options.
		options = $.extend({}, $.fn.imgNav.defaults, options);
		// Go through the matched elements and return the jQuery object.
		return this.each(function() {
			var _p={};
			_p._opts=options;
			_p._alist = $("a", this);
			_p._clist = $(_p._opts.navc);
			p.initNav(_p);
		});
	};
	// Public defaults.
	$.fn.imgNav.defaults = {
		mode:'click',
		on: 'on',
		off: 'off',
		navc: '.navc',//nav content selector
		callback:null
	};
})(jQuery);