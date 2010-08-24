/**
 * textareaSearch插件-查询textarea的内容
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {},pub={};
    p.onFocus=function(evt){
		$(this).select();
	};
	p.onKeyup=function(evt){
		if(evt.which==13){
			this.Search(this.value);
		};
	};
	p.onSearch=function(str){
		str=$.trim(str);
		if(str=="") return;
		var num = 0;
	    if (document.selection)　　
			num = document.selection.createRange().text.length;
	    this.txtRange.moveStart("character", num);
	    this.txtRange.moveEnd("character", str.length);
	    if (this.txtRange.findText(str))
			this.txtRange.select();
	    
		if (this.txtRange.text != str) {　　
			this.txtRange = this.$t[0].createTextRange();
	    };

	};
	p.initEvts = function(i) { 
		$(i).focus(p.onFocus).keyup(p.onKeyup);
		i.$btn.click(function(evt){
			p.onSearch(i.value);
			return false;
		});
	};
    //main plugin body
    $.fn.textareaSearch = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.textareaSearch.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			pub.$t=$(opts.cssTextArea);
			pub.$btn=$(opts.cssBtn);
			pub.Search=p.onSearch;
			pub.txtRange=document.selection.createRange();
			$.extend(this,pub);
			p.initEvts(this);
        });
    };
    // Public defaults.
    $.fn.textareaSearch.defaults = {
        cssTextArea: '.textarea',
		cssBtn:'.btnSearch'
    };
    // Public functions.
})(jQuery);