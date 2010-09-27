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
			this.$items.mouseenter(function(e){
				_this.z($(this));
			}).bind("evtReset",function(e){
				_this.r($(this));
			});
			//init the zoomed items
			this.$items.filter(".lv_init").trigger("mouseenter");
			
			//opacity effect for overlay
			this.$l.find(".lv_ovl").css("opacity",this._o.opacityOvl);
		},
		/**
		 * zoom effect
		 * @param {Object} $t
		 */
		z:function($t){
			var _this=this;
			if($t.hasClass("lv_zoom")) return;
			if(this.$hot){
				this.$hot.trigger("evtReset");
			};
			this.$hot=$t;
			var i0=$t.index(),even=(i0%2==0),toLeft=(i0>=_this.imiddle);
			this._zIn($t.addClass("lv_zoom"),even,toLeft);
			this.$items.each(function(i,o){
				if(i==i0) return true;
				if(toLeft){
					//如果元素居右，则向左放大
					_this._goL($(o),even,i,i0);
					
				}else{
					//如果元素居左，则向右放大
					_this._goR($(o),even,i,i0);
				};//if
			});//each
		},
		/**
		 * zoom in
		 * @param {Object} $t
		 * @param {Object} even
		 * @param {Object} toLeft
		 */
		_zIn:function($t,even,toLeft){
			$t.stop(true, true);
			$t.find("img").stop(true, true).animate({
				width: this._o.w * 2 - this._o.gapH * 2,
				height: this._o.h * 2 - this._o.gapV * 2
			},this._o.speed,function(){
				$t.find(".lv_cover").show();
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
		 * reset
		 * @param {Object} $t
		 */
		r:function($t){
			var _this=this,sp=0;
			var i0=$t.index(),even=(i0%2==0),toLeft=(i0>=this.imiddle),tempObj;
			$t.removeClass("lv_zoom").find("img").stop(true,true).animate({width:this._o.w-this._o.gapH*2,height:this._o.h-this._o.gapV*2},sp);
			$t.find(".lv_cover").hide();
			this.$items.stop(true,true);
			if(even){
				if(toLeft){
					this.$evenItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i>=i0) return true;
						tempObj.animate({left:"+="+_this._o.w},sp);
					});
					this.$oddItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i>(i0+1)) return true;
						tempObj.animate({left:"+="+_this._o.w*2},sp);
					});
					$t.animate({left:"+="+this._o.w},sp);	
				}else{
					this.$evenItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i<=i0) return true;
						tempObj.animate({left:"-="+_this._o.w},sp);
					});
					this.$oddItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i<=i0) return true;
						tempObj.animate({left:"-="+_this._o.w*2},sp);
					});
				};
			}else{
				if(toLeft){
					this.$evenItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i>=i0) return true;
						tempObj.animate({left:"+="+_this._o.w*2},sp);
					});
					this.$oddItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i>=i0) return true;
						tempObj.animate({left:"+="+_this._o.w},sp);
					});
					$t.animate({left:"+="+_this._o.w,top:"+="+_this._o.h},sp);					
				}else{
					this.$evenItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i<(i0-1)) return true;
						tempObj.animate({left:"-="+_this._o.w*2},sp);
					});
					this.$oddItems.each(function(i,o){
						i=(tempObj=$(o)).index();
						if(i<=i0) return true;
						tempObj.animate({left:"-="+_this._o.w},sp);
					});
					$t.animate({top:"+="+this._o.h},sp);
				};//if toLeft
			};//end of if even
		},
		_goL:function($t,even,i,i0){
			if(even&&i>(i0+1)) return;
			if((!even)&&i>i0) return;
			$t.stop(true,true);
			if(i%2==0){
				$t.animate({left:"-="+this._o.w*(even?1:2)},this._o.speed);
			}else{
				$t.animate({left:"-="+this._o.w*(even?2:1)},this._o.speed);
			};

		},
		_goR:function($t,even,i,i0){
			if(even&&i<i0) return;
			if((!even)&&i<(i0-1)) return;
			$t.stop(true,true);
			if(i%2==0){
				$t.animate({left:"+="+this._o.w*(even?1:2)},this._o.speed);
			}else{
				$t.animate({left:"+="+this._o.w*(even?2:1)},this._o.speed);
			};	
		}
	};
    //main plugin body
    $.fn.btslide = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.btslide.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
        	new p.M($(this),opts);
		});
    };
    // Public defaults.
    $.fn.btslide.defaults = {
        w: 140,
		h:190,
		gapV:5,				/* 图片垂直方向留白 */
		gapH:5,				/* 图片水平方向留白 */
    	opacityOvl:0.7,
		speed:400
	};
    // Public functions.
    $.fn.btslide.method1 = function(skinName) {
        return;
    };
})(jQuery); 