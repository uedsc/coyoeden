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
			p.onSearch(this);
		};
	};
	p.onSearch=function(i){
		var str=$.trim(i.value);
		if(str=="") return;
		
		var v=i.$t.val();
		if(v.indexOf(str)==-1) return;
		
		var num = 0;
		if($.browser.msie){
		    if (document.selection)　　
				num = document.selection.createRange().text.length;
		    i._rng.moveStart("character", num);
		    i._rng.moveEnd("character", str.length);
		    if (i._rng.findText(str))
				i._rng.select();
		    
			if (i._rng.text != str) {　　
				i._rng = i.$t[0].createTextRange();
		    };
		}else{
			i.$t.focus();
			num=v.indexOf(str,i.$t[0].selectionEnd);
			num=num<0?v.indexOf(str):num;
			i.$t[0].setSelectionRange(num,num+str.length);
		};

	};
	p.initEvts = function(i) { 
		$(i).focus(p.onFocus).keyup(p.onKeyup);
		i.$btn.click(function(evt){
			p.onSearch(i);
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
			if($.browser.msie)
				pub._rng=pub.$t[0].createTextRange();
			
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