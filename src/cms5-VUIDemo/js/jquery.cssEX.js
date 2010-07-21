/**
 * @author levinhuang
 */
;(function($) {
    //main plugin body
    $.fn.copyCss = function(toNode,styleList) {
        var _this=this;
		// Set the options.
        styleList=styleList|| $.fn.copyCss.defaults.style;
		if(!$.isArray(styleList)) styleList=styleList.split(" ");
		//copy css
		$.each(styleList,function(i,name){
			toNode.css(name,_this.css(name));
		});
		return this;
    };
    // Public defaults.
    $.fn.copyCss.defaults = {
        style:['font-family','font-size','font-weight','font-style','color',
	        'text-transform','text-decoration','letter-spacing','word-spacing',
	        'line-height','text-align','vertical-align','direction','background-color',
	        'background-image','background-repeat','background-position',
	        'background-attachment','opacity','width','height','top','right','bottom',
	        'left','margin-top','margin-right','margin-bottom','margin-left',
	        'padding-top','padding-right','padding-bottom','padding-left',
	        'border-top-width','border-right-width','border-bottom-width',
	        'border-left-width','border-top-color','border-right-color',
	        'border-bottom-color','border-left-color','border-top-style',
	        'border-right-style','border-bottom-style','border-left-style','position',
	        'display','visibility','z-index','overflow-x','overflow-y','white-space',
	        'clip','float','clear','cursor','list-style-image','list-style-position',
	        'list-style-type','marker-offset']
    };
})(jQuery);  

