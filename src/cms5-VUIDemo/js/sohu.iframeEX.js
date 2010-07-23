/**
 * A jquery plugin which will do some extensions to the iframe element!
 * @author levinhuang
 * @version 1.2010.0723
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
		iCnt:0,/* Design mode trying counters */
		iCurCss:{"text-align":"left"},/* Current font align for the iframe */
		/**
		 * Focus the specified iframe
		 */
		iFocus:function(){
			this.contentWindow.focus();
			return this;
		},
		/**
		 * Select all the contents of the specified iframe
		 */
		iSelect:function(){
			this.iDoCommand("selectall",false);
			return this;
		},
		/**
		 * invoke specified execCommand method.
		 * @param {Function} preCbk callback handler before running command
		 */
		iDoCommand:function(cmd,opts,cbk,preCbk){
			try {
				//callback handler before running command
				if(preCbk){preCbk(this);};
				//execute the command
				switch (cmd) {
					case "fontsizeup":
						p.cmdFontSizeUp(this.contentWindow.document, opts);
						break;
					case "fontsizedwn":
						p.cmdFontSizeDwn(this.contentWindow.document, opts);
						break;
					case "justifyleft":
						this.iCurCss["text-align"] = "left";
						this.i$Body().css("text-align", "left");
						break;
					case "justifycenter":
						this.iCurCss["text-align"] = "center";
						this.i$Body().css("text-align", "center");
						break;
					case "justifyright":
						this.iCurCss["text-align"] = "right";
						this.i$Body().css("text-align", "right");
						break;
					case "justifyfull":
						this.iCurCss["text-align"] = "right";
						this.i$Body().css("text-align", "right");
						break;
					default:
						this.contentWindow.document.execCommand(cmd, false, opts||null);
						break;
				};//switch
				if(cbk){cbk(this);};
			}catch(e){
				//do nothing
				this.contentWindow.document.write("ERROR:"+(e.description||e.message));
			};
			return this;
		},
		/**
		 * Toggle the designMode of the iframe
		 * @param {String} mode Possible values may be 'on' or 'off'
		 * @param {Function} cbk callback handler when the mode has been changed.
		 */
		iDesignMode:function(mode,cbk){
			//Old mode,no need to change
			if(this.contentWindow.document.designMode.toLowerCase()==mode) 
				return this;
			//Change the designMode 				
			var _this=this;
			
			try{
				//this.iBody.contentEditable=(mode=="on");	
				//set the design mode
				this.contentWindow.document.designMode=mode;
				
				if(cbk){
					cbk(this,{mode:mode});
				}//if
			}catch(e){
				if((this.iCnt++)>3)
					return this;
				alert("Design mode error,try it again!");
				//try again
				setTimeout(function(){
					_this.iDesignMode(mode,cbk);
				},350);
			}//try
			
			return this;
		},//disignMode
		/**
		 * Get the data contained in the body tag of the iframe
		 */
		iGetData:function(){
			return this.contentWindow.document.body.innerHTML;
		},
		/**
		 * set data to the body tag of the iframe
		 * @param {String} html 
		 */
		iSetData:function(html){
			this.contentWindow.document.body.innerHTML=html;
			return this;
		},
		/**
		 * 
		 * @param {Object} css
		 */
		iSetBodyCss:function(css){
			var _this=this;
			if(css){
				this.i$Body().css(css);
			}else{
				if(this._opts.bodyCss.toLowerCase().indexOf(".css")>0){
					//load external css file
					$.get(this._opts.bodyCss,function(css1,txtStatus){
						this._opts.bodyCss=css1||"";
						_this.i$Body().attr("style",this._opts.bodyCss);
					});
				}
			}//if
			return this;
		},
		/**
		 * Get iframe body jquery object.
		 * Can't use i$Body as  property because the order in which things are done varies for IE and FF 
		 */
		i$Body:function(){
			if(this._opts.bodyTag){/* init body tab only once */
				this.contentWindow.document.open();
				this.contentWindow.document.write(this._opts.bodyTag);
				this.contentWindow.document.close();
				this._opts.bodyTag=null;
			}
			return $(this.contentWindow.document.body);
		},
		/**
		 * Get the iframe document jq obj
		 * Can't use i$Body as  property because the order in which things are done varies for IE and FF
		 */
		i$Doc:function(){
			return $(this.contentWindow.document);
		},
		/**
		 * Get the iframe body
		 */
		iBody:function(){
			return this.contentWindow.document.body;
		},
		/**
		 * Get the iframe document
		 */
		iDoc:function(){
			return this.contentWindow.document;
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
			var _this=this;
			/* register public methods to the iframe dom object!So we call use iframe.setMode method */
			pub._opts=opts;
			$.extend(this,pub);
			//init body tag
			var $body=this.i$Body();
			//load the external css for once
			this.iSetBodyCss();
			//init mode
			if(opts.designMode){
				this.iDesignMode("on");
			}				
			
        });
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