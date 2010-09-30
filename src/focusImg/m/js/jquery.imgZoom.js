/**
 * A jquery gallery zoom effect plugin
 * @author levinhuang
 * @site	Http://vivasky.com
 */
;(function($) {
    // Private functions.
    var p = {};
	/**
	 * 类Cell-存放BB的箱子的单元格
	 */
	p.C=function(opts){
		$.extend(this,{
			dim:{
				top:opts.t,
				left:opts.l,
				width:opts.w,
				height:opts.h
			},
			wBox:opts.wbox,			/* width of the box */
			hBox:opts.hbox,			/* height of the box */
			Full:false,				/* empty or not */
			$Baby:null,				/* baby in the cell */
			nT:opts.t<opts.hbox/2,	/* the cell is near the top part of the box */
			nL:opts.l<opts.wbox/2,	/* the cell is near the left part of the box */
			id:opts.id
		});
	};
	p.C.prototype={
		/**
		 * put a baby in the cell
		 * @param {Object} t
		 */
		add:function(t){
			this.$Baby=t;
			this.Full=true;
			
			t.css(this.dim);
			return this;
		},
		/**
		 * clear the cell
		 */
		clear:function(){
			this.Full=false;
			this.$Baby.hide();
			return this;
		}
	};
    p.M = function(t,opts) {
		//properties
		$.extend(this,opts);
		$.extend(this,{
			$l:t,
			$items:t.find(".lv_item"),
			gapW:opts.wItem1-opts.wItem0,
			gapH:opts.hItem1-opts.hItem0,
			h:t.height(),
			w:t.width(),
			$cur:null,
			$curImg:null,
			cells:[]
		});
		//init
		this._i();
	};
	p.M.prototype={
		/**
		 * init 
		 */
		_i:function(){
			var i=this,col=0;
			//position for items
			i.$items.each(function(j,o){
				if((j>0)&&(j%i.rowNum==0)){
					col++;	
				};				
				$(o).css({left:col*i.wItem0,top:j*i.hItem0});			
			}).mouseenter(function(){
				i.z($(this));
			}).filter(".lv_zoom").trigger("mouseenter");	
					
			//build cells
			//每行要增加的格子数=zoom-1
			var id=0;
			for(var k=0;k<(col+i.zoom-1);k++){
				for (var j = 0; j < i.rowNum; j++) {
					i.cells.add(new p.C({
						l: k * i.wItem0,
						t: j * i.hItem0,
						w: i.wItem0,
						h: i.hItem0,
						id:id++
					}));
				};
			};
		},
		/**
		 * meta info for the specified element
		 */
		meta:function(t){
			var i=this,pos=t.position(), d={
				L:pos.left,					/* left */
				T:pos.top,					/* top */
				Z:t.hasClass("lv_zoom"),	/* is zoomed */
				nT:pos.top<i.h/2,			/* near the top part */
				nL:pos.left<i.w/2			/* near the left part */
			};
			return d;
		},
		z:function(t){
			var i=this,m=i.meta(t),m1,l;
			if(m.Z) return;
			//zoom out the last zoomed item
			i.r(t.addClass("lv_zoom"));
			//zoom in
			i.$curImg.animate({width:i.wImg1,height:i.hImg1},i.speed,function(){
						
			});
			var css={width:i.wItem1,height:i.hItem1};
			if(m.nT){
				if(!m.nL){
					css.left="-="+i.gapW;	
				};
			}else{
				css.top="-="+i.gapH;
				if(!m.nL){
					css.left="-="+i.gapW;
				};
			};
			t.animate(css,i.speed);
			i.$items.not(t[0]).each(function(j,o){
				o=$(o);
				m1=i.meta(o);
				o.stop(true,true);
				if(m.nL&&m1.L<m.L&&m1.T==m.T) return;
				if((!m.nL)&&m1.L>m.L&&m1.T==m.T) return;
				o.animate({left:(m.L<m1.L?"+":"-")+"="+i.wItem0},i.speed,function(){
					l=o.position().left;
					if(l>=0&&l<i.w) return;
					o.css("left",(l>0?0:(i.w-i.wItem0)));
				});					
			});
		},
		r:function(t){
			var i=this;
			if(i.$cur){
				//zoom out
				i.$curImg.stop(true,true).animate({width:i.wImg0,height:i.hImg0},i.speed,function(){
							
				});
				var css={width:i.wItem0,height:i.hItem0},m=i.meta(i.$cur);
				if(m.nT){
					if(!m.nL){
						css.left="+="+i.gapW;	
					};
				}else{
					css.top="+="+i.gapH;
					if(!m.nL){
						css.left="+="+i.gapW;
					};
				};
				i.$cur.stop(true,true).animate(css,i.speed,function(){
					$(this).removeClass("lv_zoom");
				});				
			};
			i.$cur=t.stop(true,true);
			i.$curImg=t.find("img").stop(true,true);
		}
	};
    //main plugin body
    $.fn.imgZoom = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.imgZoom.defaults, opts);
		$.extend(opts,{
			wItem1:opts.wItem0*opts.zoom,
			hItem1:opts.hItem0*opts.zoom
		});

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			new p.M($(this),opts);
        });
    };
    // Public defaults.
    $.fn.imgZoom.defaults = {
        speed: 500,
		wImg0:130,	/* 小图宽 */
		hImg0:180,	/* 小图高 */
		wImg1:270,
		hImg1:370,
		wItem0:140,
		hItem0:190,
		rowNum:3,
		zoom:2		/* 200% zoom */
    };
})(jQuery);