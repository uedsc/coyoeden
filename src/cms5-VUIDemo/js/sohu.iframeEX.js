/**
 * A jquery plugin which will do some extensions to the iframe element!
 * @author levinhuang
 */
;(function($) {
	/* Private helper methods */
	var p={};
	p.pxToFontSize=function(curSize){
		var newSize = 3;
		if (curSize < 13) {
	          newSize = 1;
	        } else if (curSize >= 13 && curSize < 16) {
	          newSize = 2;
	        } else if (curSize >= 16 && curSize < 18) {
	          newSize = 3;
	        } else if (curSize >= 18 && curSize < 24) {
	          newSize = 4;
	        } else if (curSize >= 24 && curSize < 32) {
	          newSize = 5;
	        } else if (curSize >= 32) {
	          newSize = 6;
	        }
		return newSize;
	};
	p.cmdFontSizeUp=function(ifDoc,opts){
		try {
          	var currentSize = ifDoc.queryCommandValue("FontSize");
	      	if (!currentSize) 
	      	{
              ifDoc.execCommand("FontSize", false, 3);
	        } 
	        else if (typeof(currentSize) == "string") 
	        {
	          if( currentSize.match("px") )
	          {
                var newSize = p.pxToFontSize(parseInt(currentSize.replace("px", "")));
	          }
	          else
	          {
	            var newSize = parseInt(currentSize);
	          }
	          newSize = newSize > 6 ? 7 : newSize + 1;
              ifDoc.execCommand("FontSize", false, newSize);
	        } 
	        else 
	        {
              ifDoc.execCommand("FontSize", false, currentSize > 6 ? 7 : currentSize + 1);
	        }
		 } catch(e) { ifDoc.execCommand(command, false, options); }
	};
   	p.cmdFontSizeDwn=function(ifDoc,opts){
      try {
          var currentSize = ifDoc.queryCommandValue("FontSize");
          if (!currentSize) 
          {
            ifDoc.execCommand("FontSize", false, 1);
          } 
          else if (typeof(currentSize) == "string")
          { 
              if( currentSize.match("px")) 
              {
            	var newSize = p.pxToFontSize(parseInt(currentSize.replace("px", "")));
              }
              else
              {
                var newSize = parseInt(currentSize);
              }
              newSize = newSize > 1 ? newSize - 1 : 1;
              ifDoc.execCommand("FontSize", false, newSize);
          } 
          else 
          {
            ifDoc.execCommand("FontSize", false, currentSize < 2 ? 1 : currentSize - 1);
          }
      } catch(e) { ifDoc.execCommand(command, false, opts); }		
	}; 
   /* Public functions that will be merged into the raw iframe element. */
	/* Add the prefix 'i' to all the functions to avoid name conflicts with other jquery plugins... */
    var pub = {
		/**
		 * Reference to the iframe dom object itself.
		 * @param {int} idx index for the seleted iframe
		 */
		i:function(idx){
			/* Resume that the method caller 'this' reference to a jquery object firstly */
			idx=idx||0;
			return (this[idx]||this);
		},
		iCnt:0,/* Design mode trying counters */
		iCurCss:{"text-align":"left"},/* Current font align for the iframe */
		/**
		 * Focus the specified iframe
		 * @param {Object} idx index
		 */
		iFocus:function(idx){
			var _i=this.i(idx);
			_i.contentWindow.focus();
			return this;
		},
		/**
		 * Select all the contents of the specified iframe
		 * @param {Object} idx index
		 */
		iSelect:function(idx){
			var _i=this.i(idx);
			var _this=this;
			this.iDoCommand("selectAll",false,null,idx);
			setTimeout(function(){
				_this.iDoCommand("selectAll",false,null,idx);
			},500);	
			return this;
		},
		/**
		 * invoke specified execCommand method.
		 */
		iDoCommand:function(cmd,opts,cbk,idx){
			idx=idx||0;
			try {
				switch (cmd) {
					case "fontsizeup":
						p.cmdFontSizeUp(this.i$Doc()[0], opts);
						break;
					case "fontsizedwn":
						p.cmdFontSizeDwn(this.i$Doc()[0], opts);
						break;
					case "justifyleft":
						this.iCurCss["text-align"] = "left";
						this.i$Doc(idx).css("text-align", "left");
						break;
					case "justifycenter":
						this.iCurCss["text-align"] = "center";
						this.i$Doc(idx).css("text-align", "center");
						break;
					case "justifyright":
						this.iCurCss["text-align"] = "right";
						this.i$Doc(idx).css("text-align", "right");
						break;
					case "justifyfull":
						this.iCurCss["text-align"] = "right";
						this.i$Doc(idx).css("text-align", "right");
						break;
					default:
						this.i$Doc(idx)[0].execCommand(cmd, false, opts);
						break;
				};//switch
				if(cbk){cbk(this);};
			}catch(e){
				//do nothing
			};
			return this;
		},
		/**
		 * Toggle the designMode of the iframe
		 * @param {String} mode Possible values may be 'on' or 'off'
		 * @param {int} idx index for the seleted iframe
		 * @param {Function} cbk callback handler when the mode has been changed.
		 */
		iDesignMode:function(mode,idx,cbk){
			var _i=this.i(idx); 
			//Old mode,no need to change
			if(_i.contentWindow.document.designMode.toLowerCase()==mode) 
				return this;
			//Change the designMode 				
			var _this=this;
			if(_i.contentWindow.document.body)/* null if an iframe with empty src property in IE */
				_i.contentWindow.document.body.contentEditable=(mode=="on");
			
			try{
				_i.contentWindow.document.designMode=mode;
				if(cbk){
					cbk(this,{mode:mode,idx:idx});
				}//if
			}catch(e){
				if((this.iCnt++)>3)
					return this;
				
				//try again
				setTimeout(function(){
					_this.iDesignMode(mode,idx);
				},350);
			}	
			
			return this;
		},//disignMode
		/**
		 * Get the data contained in the body tag of the iframe
		 * @param {int} idx index for the seleted iframe
		 */
		iGetData:function(idx){
			return this.i$Body(idx).html();
		},
		/**
		 * @param {int} idx index for the seleted iframe
		 */
		i$Body:function(idx){
			var _i=this.i(idx);
			if(this._opts.bodyTag){/* init body tab only once */
				_i.contentWindow.document.open();
				_i.contentWindow.document.write(this._opts.bodyTag);
				_i.contentWindow.document.close();
				this._opts.bodyTag=null;
			}
			
			return $(_i.contentWindow.document.body);
		},
		/**
		 * Get the document jquery object of the iframe
		 * @param {Object} idx index for the seleted iframe
		 */
		i$Doc:function(idx){
			var _i=this.i(idx);
			return $(_i.contentWindow.document);
		},
		/**
		 * set data to the body tag of the iframe
		 * @param {String} html 
		 * @param {int} idx index for the seleted iframe
		 */
		iSetData:function(html,idx){
			this.i$Body(idx).html(html);
			return this;
		},
		/**
		 * 
		 * @param {Object} css
		 * @param {Object} idx index for the seleted iframe
		 */
		iSetBodyCss:function(css,idx){
			var _this=this;
			if(css){
				this.i$Body(idx).css(css);
			}else{
				if(pub._opts.bodyCss.toLowerCase().indexOf(".css")>0){
					//load external css file
					$.get(pub._opts.bodyCss,function(css1,txtStatus){
						pub._opts.bodyCss=css1||"";
						_this.i$Body(idx).attr("style",pub._opts.bodyCss);
					});
				}
			}//if
			return this;
		}
	};
    /* main plugin body */
    $.fn.iframeEX = function(opts) {
        // Set the options.
		opts = $.extend({}, $.fn.iframeEX.defaults, opts);
        // Go through the matched elements and return the jQuery object.
        this.each(function() {
			var $this=$(this);
			if(!$this.is("iframe")) return;
			//register methods
			pub._opts=opts;
			$.extend(this,pub);	/* register to the iframe dom object!So we call use iframe.setMode method */
			//init mode
			if(opts.designMode)
				this.iDesignMode("on");
			//load css
			this.iSetBodyCss();
        });
		/* Register to the iframe jquery object.*/
		$.extend(this,pub);
		/* return the extended one */
		return this;
		
    };
    /* Public defaults. */
    $.fn.iframeEX.defaults = {
        designMode: true,
		bodyTag		:  "<html><head><meta http-equiv='content-type' content='text/html;charset=uft-8' /></head><body id='icontent'></body></html>" /* default html tag for the iframe */,
		bodyCss		: '' /* css applied to the body tab of the iframe */
    };
    /* Public static functions. */
    $.fn.iframeEX.method1 = function(skinName) {
        return;
    };
})(jQuery);  