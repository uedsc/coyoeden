/**
 * Make an element to be inline editable
 * @dependency sohu.iframeEX.js
 * @author levinhuang
 * @version 1.2010.0722
 */
;(function($) {
    // Private functions.
    var p = {};
    p.onClick = function(evt) {
		if(this.iGlobalIF&&this.i$frame.t){
			this.i$frame.t.iEdit("off");
			this.i$frame.t=null;
		};
		if(this.i$frame)
			this.i$frame.t=this;
			
		this.iEdit("on");
	};
	p.bindEvents=function(dom){
		dom.i$frame[0].i$Doc().keyup(function(evt){
			 var $body=dom.i$frame[0].i$Body();
			 //reset the iframe high
			 var h=$body[0].scrollHeight;
			 h=h<25?25:h;
			 dom.i$frame.css("height",h);
			 //refresh text
			 $(dom).html($body.html());  	
	  	}).keydown(function(evt){
			//fix newline entering in IE
			if($.browser.msie&& evt.which == 13 && document.selection){
				var sel = document.selection.createRange();
		        sel.pasteHTML('<br /><span></span>');  //empty span needed to advance cursor to next line
				return false;
			}
		});
	};
	p.unbindEvents=function(dom){
		dom.i$frame[0].i$Doc().unbind();
	};
	p.initIframe=function(dom,callback){
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
		  //if0.src= "";
		  //insert the iframe and hide it
		  $if=$(if0)
		  $dom.after($if);
		  dom.i$frame=$if.iframeEX().hide();
		  if(callback&&$.browser.msie){
		  	 //IE like a shit
		  	 setTimeout(function(){
			 	callback(dom);
			 },400);
		  }else{
		  	 callback(dom);
		  };
	};
	/**
	 * ¶¨Î»iframe±à¼­Æ÷
	 * @param {Object} dom
	 */
	p.seatIframe=function(dom){
		var $this=$(dom);
		dom.i$frame.css("position","absolute");
		var pos=null;
		if(dom.iGlobalIF){
			pos=$this.offset();
		}else{
			pos=$this.position();
		};
		dom.i$frame.css({
			"top":pos.top,
			"left":pos.left,
			"z-index":50
		});
	};
	p.cssToIframe=function(dom){
		var $this=$(dom);
		var $body=dom.i$frame[0].i$Body();
		//copy styles to the iframe document
		$this.copyCss($body).copyCss(dom.i$frame);
		//we don't need any marign and border styles inside the iframe body tag 
		var ovrCss0={"margin":"0","border":"none","text-align":"left","padding":"0","background":"none"},ovrCss1={margin:0};
		//override height or width process
		if($this.css("height")=="auto"){
			ovrCss0.height=ovrCss1.height=$this.height();
		};
		ovrCss0.width=ovrCss1.width=$this.width();
		//update iframe lineheight
		if(!$.browser.msie){
			if($this.css("display")!="block"){
				ovrCss0.lineHeight=ovrCss1.lineHeight="normal";
			}else{
				var fs = $this.css('fontSize').replace('px', '');
	            var lh = $this.css('lineHeight').replace('px', '');
	            if(fs && lh){
	               ovrCss0.lineHeight=ovrCss1.lineHeight= ''+(lh/fs);
	            };				
			};
		};
		//maxWidth
		if(!$.browser.msie)
			ovrCss1.maxWidth="inherit";
		//overflow
		ovrCss1.overflow="hidden";
		//overide css
		$body.css(ovrCss0);
		dom.i$frame.css(ovrCss1);
		//show the original dom as the place holder!
		$this.show().css("visibility","hidden");
		//iframe position
		p.seatIframe(dom);
		//visibility
		$body.show();
		dom.i$frame.show();
		
	};
	p.fixPosition=function(dom){
		var $this=$(dom),isAutoWidth=($this.css("width")=="auto");
		if($this.css("position")=="absolute") return;
		//fix the iframe when it has floating elements arround
		var objs=$.grep($this.prevAll(),function(o,i){
			o._fl=$(o).css("float");
			return (o._fl=="left"||o._fl=="right");
		});
		if(objs.length==0) return;
		objs=$(objs[0]);
		objs._d={
			w:objs.width(),
			h:objs.height(),
			ml:parseInt(objs.css("marginLeft")),
			mr:parseInt(objs.css("marginRight")),
			y:objs.offset().top
		};
		var wFrame=dom.i$frame.width();
		if($this.offset().top<(objs._d.y+objs._d.h)&&isAutoWidth){
			wFrame=wFrame-objs._d.w-objs._d.ml-objs._d.mr;
		};
		var css={
			width:wFrame
		};
		if($this.css("display")=="block"&&objs[0]._fl=="left"){
			css.left=objs.offset().left+objs._d.w+objs._d.ml+objs._d.mr;
		};
		dom.i$frame.css(css);
		dom.i$frame[0].i$Body().css("width",wFrame);
		
	};
	p.doEditing=function(opts){
		var $this=$(opts.dom);
		if(opts.mode=="on"){
			if(opts.dom.iEditing) return;
			
			opts.dom.iEditing=true;
			
			p.cssToIframe(opts.dom);

			p.fixPosition(opts.dom);
			//show the editbox
			opts.dom.i$frame[0]
				.iSetData($this.html())
				.iFocus();
			
			p.bindEvents(opts.dom);
			
		}else{
			//hide the editbox
			if(!opts.dom.iEditing) return;
			
			opts.dom.iEditing=false;
			$this.show().css("visibility","visible").html(opts.dom.i$frame[0].iGetData());
			p.unbindEvents(opts.dom);
			opts.dom.i$frame.hide();
		};
		if(opts.dom._opts.onModeChange&&(!opts.ignoreCbk)){
			opts.dom._opts.onModeChange(opts.dom);
		};
	};
	/**
	 * Listen to window's resize event when using a global absolute position iframe as editor!
	 */
	p.fixResize=function(i$frame){
		if(!i$frame) return;
		if(i$frame.onWindowResize) return;
		$(window).resize(function(evt){
			if(i$frame&&i$frame.t)
				p.seatIframe(i$frame.t);
		});
		i$frame.onWindowResize=true;
	};
	/* Public functions that will be merged into the raw element. */
	/* Add the prefix 'i' to all the functions to avoid name conflicts with other jquery plugins... */
	var pub={
		iEditing:false,
		i$frame:null,		/* Cache the iframe editor */
		iGlobalIF:false,	/* whether use a global iframe as the editor */
		iEdit:function(mode,ignoreCbk){
			ignoreCbk=ignoreCbk||false;
			var cbkOpt={
				dom:this,
				mode:mode,
				ignoreCbk:ignoreCbk
			};
			if (!this.i$frame) {
				var _this=this;
				p.initIframe(this, function(){
					p.doEditing(cbkOpt);
				});
				return this;
			};
			p.doEditing(cbkOpt);
			return this;
		}//iEdit
		
	};
	
    /* main plugin body */
    $.fn.iEditable = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.iEditable.defaults, opts);
		// Listen to window's resize event when using a global absolute position iframe as editor!
		p.fixResize(opts.i$frame);
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			/* 1,invalid tag detect */
			if($this.is(opts.excludeTags)) 
				return;
			/* 2,click handler register */
			$this.click(p.onClick);	
			/* 3,Extent the element */
			pub._opts=opts;
			/* 4,whether use a global iframe editor */
			if(opts.i$frame){
				pub.i$frame=opts.i$frame;
				pub.iGlobalIF=true;
			};
			$.extend(this,pub);
        });
    };
    // Public defaults.
    $.fn.iEditable.defaults = {
        excludeTags: 'table,input,select,button,tr,iframe' /* tags that don't support inline editable */,
		onModeChange:null 				/* callback handler when the iEdit function was invoked */,
		accurateWidth:true 				/* convert auto with to an accurate value when in editing mode */,
		cssFloat:'.l,.r,.left,.right',	/* floating css selectors */
		i$frame:null 					/* specify an iframe editor to the element */
    };
})(jQuery);  