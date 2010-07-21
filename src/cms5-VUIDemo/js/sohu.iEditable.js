/**
 * Make an element to be inline editable
 * @dependency sohu.iframeEX.js
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
    p.onClick = function(evt) { 
		this.iEdit("on");
	};
	p.initIFrame=function(dom){
		  var $dom=$(dom);
		  dom.id=dom.id==""?"ET_"+new Date().getTime():dom.id;
		  var if0=document.createElement('iframe');
		  if0.id = dom.id+'Edit';
		  if0.className = 'editable';
		  if0.style.display = 'block';
		  if0.style.cssFloat = 'none';
		  if0.style.styleFloat = 'none';
		  if0.style.clear = 'both';
	      if0.allowTransparency = true ;
	      if0.frameBorder = '0' ;
	      if0.scrolling = 'no' ;
		  if0.src= "";
		  
		  dom.i$frame=$(if0);
		  //insert the iframe and hide it
		  $dom.after(dom.i$frame);
		  dom.i$frame.iframeEX().hide();
		  //event handlers
		  dom.i$frame.keyup(function(evt){
			 //reset the iframe high
			 var $body=dom.i$frame.i$Body();
			 var h=$body.css("scrollHeight");
			 h=h<25?25:h;
			 dom.i$frame.css("height",h);
			 //refresh text
			 $dom.html($body.html());  	
		  });
		  
	};
	/* Public functions that will be merged into the raw element. */
	/* Add the prefix 'i' to all the functions to avoid name conflicts with other jquery plugins... */
	var pub={
		iEditing:false,
		i$frame:null,
		iEdit:function(mode){
			var $this=$(this);
			if(!this.i$frame)
				p.initIFrame(this);
				
			if(mode=="on"){
				if(this.iEditing) return;
				
				this.iEditing=true;
				$this.hide();
				//show the editbox
				this.i$frame
					.iDesignMode("on")
					.iSetData($this.html())
					.iFocus()
					.iSelect();
				//copy styles to the iframe document
				$this.copyCss(this.i$frame.i$Body());
				this.i$frame.show();
				//focus and select the text in the iframe
			
			}else{
				//hide the editbox
				if(!this.iEditing) return;
				
				this.iEditing=false;
				$this.show();
				this.i$frame.hide();
			}
		}
		
	};
	
    /* main plugin body */
    $.fn.iEditable = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.iEditable.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			/* 1,invalid tag detect */
			if($this.is(opts.excludeTags)) 
				return;
			/* 2,click handler register */
			$this.click(p.onClick);	
			/* 3,Extent the element */
			$.extend(this,pub);
			
				
        });
    };
    // Public defaults.
    $.fn.iEditable.defaults = {
        excludeTags: 'table,input,select,button,tr,iframe' /* tags that don't support inline editable */
    };
    // Public functions.
    $.fn.iEditable.method1 = function(skinName) {
        return;
    };
})(jQuery);  