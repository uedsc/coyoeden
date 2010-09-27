;(function($) {
    // Private functions.
    var p = {};
    p.M = function($t,opts){
		this._o=opts;
		this.$l=$t;										/* �����dom���� */
		this.$items=$t.find(".lv_item");				/* ������Ԫ�� */
		this.imiddle=this.$items.length/2;				/* �м�Ԫ������ */
		//init
		this._i(); 
	};
	p.M.prototype={
		_i:function(){
			var _this=this;
			//init layout
			this.$items.each(function(i,o){
				if(i%2==0){
					//��һ��
					$(o).css({left:(i/2)*_this._o.w,top:0});
				}else{
					//�ڶ���
					$(o).css({
						top: _this._o.h,
						left: (i - 1) / 2 * _this._o.w
					});
				};
			});
			//init events
			this.$items.mouseenter(function(e){
				_this.z($(this));
			});
		},
		/**
		 * zoom effect
		 * @param {Object} $t
		 */
		z:function($t){
			var _this=this;
			if($t.hasClass("lv_zoom")) return;
			var i0=$t.index(),even=(i0%2==0),toLeft=(i0>=_this.imiddle);
			this._zIn($t.addClass("lv_zoom"),even,toLeft);
			this.$items.each(function(i,o){
				if(i==i0) return true;
				if(toLeft){
					//���Ԫ�ؾ��ң�������Ŵ�
					_this._goL($(o),even,i,i0);
					
				}else{
					//���Ԫ�ؾ��������ҷŴ�
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
			if (even) {
				if(toLeft){
					$t.stop(true, true).animate({
						width: this._o.w * 2,
						height: this._o.h * 2,
						left:'-='+this._o.w
					});
					$t.find("*").stop(true, true).animate({
						width: this._o.w * 2 - this._o.gapH * 2,
						height: this._o.h * 2 - this._o.gapV * 2
					});					
				}else{
					$t.stop(true, true).animate({
						width: this._o.w * 2,
						height: this._o.h * 2
					});
					$t.find("*").stop(true, true).animate({
						width: this._o.w * 2 - this._o.gapH * 2,
						height: this._o.h * 2 - this._o.gapV * 2
					});						
				};

			}else {
				if(toLeft){
					$t.stop(true, true).animate({
						width: this._o.w * 2,
						height: this._o.h * 2,
						top:0,
						left:"-="+this._o.w
					});
					$t.find("*").stop(true, true).animate({
						width: this._o.w * 2 - this._o.gapH * 2,
						height: this._o.h * 2 - this._o.gapV * 2
					});					
				}else{
					$t.stop(true, true).animate({
						width: this._o.w * 2,
						height: this._o.h * 2,
						top:"-="+this._o.h
					});
					$t.find("*").stop(true, true).animate({
						width: this._o.w * 2 - this._o.gapH * 2,
						height: this._o.h * 2 - this._o.gapV * 2
					});					
				};

			};
		},
		/**
		 * reset
		 * @param {Object} $t
		 */
		r:function($t){
			$t.stop(true,true).animate({width:this._o.w,height:this._o.h});
		},
		_goL:function($t,even,i,i0){
			if(even&&i>(i0+1)) return;
			if((!even)&&i>i0) return;
			if(i%2==0){
				$t.stop(true,true).animate({left:"-="+this._o.w*(even?1:2)},600);
			}else{
				$t.stop(true,true).animate({left:"-="+this._o.w*(even?2:1)},600);
			};

		},
		_goR:function($t,even,i,i0){
			if(even&&i<i0) return;
			if((!even)&&i<(i0-1)) return;
			if(i%2==0){
				$t.stop(true,true).animate({left:"+="+this._o.w*(even?1:2)},600);
			}else{
				$t.stop(true,true).animate({left:"+="+this._o.w*(even?2:1)},600);
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
        w: 130,
		h:175,
		gapV:5,				/* ͼƬ��ֱ�������� */
		gapH:5				/* ͼƬˮƽ�������� */
    };
    // Public functions.
    $.fn.btslide.method1 = function(skinName) {
        return;
    };
})(jQuery); 