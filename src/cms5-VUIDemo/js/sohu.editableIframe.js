/**
 * @author levinhuang
 */
;(function($) {
    // Public functions.
    var pub = {
		setMode:function(mode){
			var _i=this[0]||this; /* Resume that the method caller 'this' reference to a jquery object firstly */
			var _this=this;
			if(_i.contentWindow.document.body)/* null if an iframe with empty src property in IE */
				_i.contentWindow.document.body.contentEditable=(mode=="on");
			
			try{
				_i.contentWindow.document.designMode=mode;
			}catch(e){
				setTimeout(function(){
					_this.setMode(mode);
				},250);
			}
		}//setMode
	};
    //main plugin body
    $.fn.editableIframe = function(opts) {
        // Set the options.
		opts = $.extend({}, $.fn.editableIframe.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			if(!$this.is("iframe")) return;
			//register methods
			$.extend(this,pub);	/* register to the iframe dom object!So we call use iframe.setMode method */
			$.extend($this,pub);/* register to the iframe jquery object */
			//init mode
			this.setMode("on");
			//$this.setMode("on");/* is also ok! */
        });
    };
    // Public defaults.
    $.fn.editableIframe.defaults = {
        on: true
    };
    // Public functions.
    $.fn.editableIframe.method1 = function(skinName) {
        return;
    };
})(jQuery);  