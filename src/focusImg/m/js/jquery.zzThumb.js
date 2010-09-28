;(function($) {
    // Private functions.
    var p = {};
    p.M = function($t,opts){
		this._o=opts;
		this.$l=$t;										/* 插件的dom布局 */
		this.$items=$t.find(".lv_item");				/* 交互的元素 */
		this.$oddItems=this.$items.filter(":odd");
		this.$evenItems=this.$items.filter(":even");
		this.imiddle=this.$items.length/2;				/* 中间元素索引 */
		this.$hot=null;
		//init
		this._i(); 
	};
	p.M.prototype={
		_i:function(){
			var _this=this;
			//init layout
			this.$items.each(function(i,o){
				if(i%2==0){
					//第一排
					$(o).css({left:(i/2)*_this._o.w,top:0});
				}else{
					//第二排
					$(o).css({
						top: _this._o.h,
						left: (i - 1) / 2 * _this._o.w
					});
				};
			});
			//init events
			this.b();
			//init the zoomed items
			this.$items.filter(".lv_init").trigger("mouseenter");
			
			//opacity effect for overlay
			this.$l.find(".lv_ovl").css("opacity",this._o.opacityOvl);
			//opacity effect for thumb
			this.$l.find("img").css("opacity",this._o.opacityImg);
		},
		/**
		 * bind events
		 */
		b:function(){
			var _this=this;
			this.$items.bind("mouseenter",function(e){
				_this.z($(this));
			}).bind("evtReset",function(e){
				_this.r($(this));
			});			
		},
		/**
		 * unbind events
		 */
		ub:function(){
			this.$items.unbind("mouseenter").unbind("evtReset");
		},
		/**
		 * zoom effect
		 * @param {Object} $t
		 */
		z:function($t){
			if($t.hasClass("lv_zoom")) return;
			var _this=this,$hot=this.$hot;
			
			if($hot){
				$hot.trigger("evtReset");
			};
			
			this.$hot=$t;
			var i0=$t.index(),even=(i0%2==0),toLeft=(i0>=_this.imiddle);
			//this.ub();
			this._zIn($t.addClass("lv_zoom"),even,toLeft);
		},
		/**
		 * zoom in
		 * @param {Object} $t
		 * @param {Object} even
		 * @param {Object} toLeft
		 */
		_zIn:function($t,even,toLeft){
			var _this=this;
			$t.stop(true, true);
			$t.find("img").stop(true, true).animate({
				width: this._o.w * 2 - this._o.gapH * 2,
				height: this._o.h * 2 - this._o.gapV * 2,
				opacity:1
			},this._o.speed,function(){
				$t.find(".lv_cover").slideDown();
				//_this.b();
			});	
			if (even) {
				if(toLeft){	
					$t.animate({
						left:'-='+this._o.w
					},this._o.speed);
				};
			}else {
				if(toLeft){
					$t.animate({
						top:0,
						left:"-="+this._o.w
					},this._o.speed);										
				}else{
					$t.animate({
						top:"-="+this._o.h
					},this._o.speed);				
				};

			};
		},
		/**
		 * reset the zoomed item
		 * @param {Object} $t
		 */
		r:function($t){
			var _this=this,sp=this._o.speed;
			var i0=$t.index(),even=(i0%2==0),toLeft=(i0>=this.imiddle),tempObj;
			$t.removeClass("lv_zoom").find("img").stop(true,true).animate({width:this._o.w-this._o.gapH*2,height:this._o.h-this._o.gapV*2,opacity:this._o.opacityImg},sp);
			$t.find(".lv_cover").stop(true,true).hide();
			$t.stop(true,true);
			if(even){
				if(toLeft){
					$t.animate({left:"+="+this._o.w},sp);	
				};
			}else{
				if(toLeft){
					$t.animate({left:"+="+_this._o.w,top:"+="+_this._o.h},sp);					
				}else{
					$t.animate({top:"+="+this._o.h},sp);
				};//if toLeft
			};//end of if even
		}
	};
    //main plugin body
    $.fn.zzThumb = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.zzThumb.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
        	new p.M($(this),opts);
		});
    };
    // Public defaults.
    $.fn.zzThumb.defaults = {
        w: 140,
		h:190,
		gapV:5,				/* 图片垂直方向留白 */
		gapH:5,				/* 图片水平方向留白 */
    	opacityOvl:0.7,
		opacityImg:0.6,
		speed:400
	};
    // Public functions.
    $.fn.zzThumb.method1 = function(skinName) {
        return;
    };
})(jQuery); 