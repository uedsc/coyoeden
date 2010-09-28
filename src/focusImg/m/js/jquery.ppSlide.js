/*
 搜狐高清-电影首页焦点图插件
 1,l=layout
 2,s=slider,scroll bar
 3,p=panel,scroll panel
 4,w=width
 5,ct=content,scroll content
 6,tm=timer
 7,cv=cover
 */

;(function($) {
    // Private functions.
    var p = {};
    p.M = function($t,opts){
		this._o=opts; 
		this.$l=$t;
		this.$s=$t.find("."+opts.clSlider);						/* 滚动条 */
		this.$sA=$t.find("."+opts.clSliderHandle);				/* 滚动条上的拖拽助手 */
		this.$p=$t.find("."+opts.clPanel);						/* 内容遮罩jq对象 */
		this.$ct=$t.find("."+opts.clContent);					/* 内容 jq对象*/
		this.$ing=$t.find("."+opts.clLoading);					/* Loading图标 */
		this.$boxes=this.$ct.children();						/* 内容块=每块含有4个小块 */
		this.wB=this.$boxes.eq(0).width();						/* 大图宽 */
		this.wB0=this.wB-2*this._o.gapH;						/* 大图实际宽 */
		this.hB=this.$boxes.eq(0).height();						/* 大图总高 */
		this.hB0=this.hB-2*this._o.gapV;						/* 大图实际高 */
		this.wS0=(this.wB0-2*this._o.gapH)/2;					/* 小图实际宽 */
		this.hS0=(this.hB0-2*this._o.gapV)/2;					/* 小图实际高 */
		this.wCT=this.$boxes.length*this.wB;					/* 内容区域总宽 */
		this.$ct.css("width",this.wCT);
		this.wP=this.$p.width();								/* 内容遮罩(屏幕)宽度 */
		this.wCTGap=this.wCT-this.wP;							/* 总宽-遮罩宽 */
		this.wScrollMax=this.$s.width()-this.$sA.width();		/* 滚动条有效宽度 */
		this.factorCT=this.wCTGap/this.wScrollMax;				/* 滚动系数 */
		this.tm=null;											/* 计时器 */
		this.$cur=null;											/* 当前被放大的小图 */
		this.$curCv=null;										/* 当前大图 容器*/
		this.$curImgB=null;										/* 当前大图img对象 */
		this.$curNote=null;
		this.curPos=null;										/* 当前小图位置 */
		//init
		this._i();
	};
	p.M.prototype={
		_i:function(){
			var _this=this,step;
			//可拖拽的滚动条-init scroll bar
			this._isb();
			//小图的透明效果
			this.$l.find(this._o.cssThumb).css('opacity',this._o.opacity);
			//小图交互
			this.$l.find(this._o.cssItem).mouseenter(function(){
				_this.z($(this));
			});
			//大图onload事件
			this.$l.find(".lv_zoom img").load(function(){
				_this.$ing.remove();
			});
			//文字蒙层的透明效果
			this.$l.find(".lv_ovl").css("opacity",0.6);
			//初始化大图
			this.$l.find(".lv_init").trigger("mouseenter");
		},
		/**
		 * 放大指定的缩略图
		 * @param {Object} $t
		 */
		z:function($t){
			if(this.isHot($t)) return;
			var _this=this,d,$img,$z,$ia;
			//重置缩小前一张大图
			this.r($t);
			//get image data
			$ia=$t.find("a");
			d=$.evalJSON($ia.attr("rel"));
			//hide the note
			this.$curNote.hide().filter(".lv_note").find(".lv_t").html("").end();
			//zoom in
			this._zIn();
			//loading
			this.$curCv.append(this.$ing.show().remove());			
			//get the zoomed image
			$img=$t.find("img");
			d.p=$img.attr("alt");
			d.p=d.p==""?$img.attr("src"):d.p;
			//update data
			$z=this.$curCv.find(".lv_zoom").attr("href",$ia[0].href);
			this.$curCv.find(".lv_t0").html(d.t0);
			this.$curCv.find(".lv_t1").html(d.t1);			
			//load the big image
			$z.find("img").attr("src",d.p);		

		},
		/**
		 * 大图交互效果-不同小图位置有不同的交互
		 * 交互逻辑：奇数向右放大;偶数向左放大；鼠标移至小图时，大图以小图的尺寸覆盖小图，然后放大
		 */
		_zIn:function(){
			var _this=this;
			//大图覆盖着小图的位置
			this.$curCv.stop(true,true).css({
				left:this.curPos.left,
				top:this.curPos.top,
				display:"block"
			}).animate({
				left:0,
				top:0,
				height:this.hB,
				width:this.wB
			},this._o.speed,"linear",function(){
				_this.$curNote.fadeIn("normal");
				_this.adjust();
			});
			//图片的交互
			this.$curImgB.stop(true,true).css({
				width:this.wS0,
				height:this.hS0
			}).animate({
				width:this.wB0,
				height:this.hB0
			},this._o.speed,"linear");			
		},
		/**
		 * 调整滚动条位置，以便当前大图始终显示在可视区域
		 */
		adjust:function(){
			var boxIndex=this.$curCv.parent().index();
			var ml=parseInt(this.$ct.css("margin-left"));
			var gapRight=(boxIndex+1)*this.wB+ml-this.wP;		/* 大图距离右边距的大小 */
			var gapLeft=boxIndex*this.wB+ml;					/* 大图距离左边距的大小 */
			ml=isNaN(ml)?0:ml;
			if(gapLeft<0){
				//内容需要往右边滚动
				this.s(true,-gapLeft);
				return;
			};
			if(gapRight>0){
				//内容需要往左边滚动
				this.s(false,gapRight);
			};
		},
		/**
		 * 滚动内容区域，联动小滚动条
		 */
		s:function(toRight,val){
			var step;
			var val0=val/this.factorCT;
			if(toRight){
				val="+="+val;
				step="-="+val0;
			}else{
				val="-="+val;
				step="+="+val0;
			};
			//内容
			this.$ct.stop(true,true).animate({"margin-left":val});
			//小滚动条
			if(this._o.scrollBar)
				this.$sA.stop(true,true).animate({"left":step});
		},
		/**
		 * init scroll bar
		 */
		_isb:function(){
			var _this=this;
			if(!this._o.scrollBar) return;
			this.$sA.draggable({
				axis:'x',
				drag:function(evt,ui){
					if(_this.wCTGap<=0) return;
					step=Math.round(ui.position.left*_this.factorCT);
					_this.$ct.css("margin-left",-step);
				},
				containment:'parent'
			}).click(function(e){return false;});
			//页面加载时滚动到中间位置
			this.$sA.css({left:this.wScrollMax/2});
			this.$ct.css({'margin-left':-this.wCTGap/2});			
		},
		/**
		 * 指定缩图是否正处于放大状态
		 */
		isHot:function($t){
			if($t.nextAll(".lv_cover").is(":visible")) return true;
			return false;
		},
		/**
		 * 缩小当前大图-r=reset
		 */
		r:function($t){
			if(this.$cur){
				this.$curCv.stop(true,true).animate({
					left:this.curPos.left,
					top:this.curPos.top,
					width:this.wB/2,
					height:this.hB/2
				},this._o.speed,function(){
					$(this).hide();
				});
				this.$curImgB.stop(true,true).animate({width:this.wS0,height:this.hS0},this._o.speed);
				this.$curNote.hide();
			};
			
			this.$cur=$t;
			this.$curCv=$t.nextAll("."+this._o.clCover);
			this.$curImgB=this.$curCv.find(".lv_zoom img");
			this.curPos=this.$cur.position();
			this.$curNote=this.$curCv.find(".lv_ovl,.lv_note");	
		}
	};
    //main plugin body
    $.fn.ppSlide = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.ppSlide.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			new p.M($(this),opts);
        });
    };
    // Public defaults.
    $.fn.ppSlide.defaults = {
		scrollBar:true,					/* 是否显示滚动条-如果显示则需要jquery-ui-ppSlide.min.js */
        clSlider: 'lv_slider_',
		clSliderHandle:"lv_slider_hdl",
		clPanel:'lv_main',
		clContent:'lv_main_',
		cssItem:'.lv_item',
		cssThumb:'.lv_item img',
		clCover:'lv_cover',
		clLoading:'lv_loading',
		opacity:0.7,
		speed:500,
		gapV:5,							/* 图片的垂直留白 */
		gapH:5							/* 图片的水平留白 */
    };
})(jQuery);  