/*
 搜狐高清-电影首页焦点图插件
 1,l=layout
 2,s=slider,scroll bar
 3,p=panel,scroll panel
 4,w=width
 5,ct=content,scroll content
 6,tm=timer
 */

;(function($) {
    // Private functions.
    var p = {};
    p.M = function($t,opts){
		this._o=opts; 
		this.$l=$t;
		this.$s=$t.find("."+opts.clSlider);
		this.$sA=$t.find("."+opts.clSliderHandle);
		this.$p=$t.find("."+opts.clPanel);
		this.$ct=$t.find("."+opts.clContent);
		this.$ing=$t.find("."+opts.clLoading);
		this.wCT=this.$ct.width();
		this.wP=this.$p.width();
		this.wScrollMax=this.$s.width()-this.$sA.width();
		this.tm=null;
		
		//init
		this._i();
	};
	p.M.prototype={
		_i:function(){
			var _this=this;
			var gap=this.wCT-this.wP,step;
			//可拖拽的滚动条
			this.$sA.draggable({
				axis:'x',
				drag:function(evt,ui){
					if(gap<=0) return;
					step=Math.round(ui.position.left/_this.wScrollMax*gap);
					_this.$ct.css("margin-left",-step);
				},
				containment:'parent'
			}).click(function(e){return false;});
			//页面加载时滚动到中间位置
			this.$sA.animate({left:this.wScrollMax/2},1000);
			this.$ct.animate({'margin-left':-gap/2},1000);
			//小图的透明效果
			this.$l.find(this._o.cssThumb).css('opacity',this._o.opacity);
			//小图交互
			this.$l.find(this._o.cssItem).mouseenter(function(){
				_this.z($(this));
			});
			//大图onload事件
			this.$l.find(".lv_zoom img").load(function(){
				$(this).parent().nextAll().slideDown(100);
				_this.$ing.remove();
			});
			//文字蒙层的透明效果
			this.$l.find(".lv_ovl").css("opacity",0.6);
			//初始化大图
			this.$l.find(".lv_init").trigger("mouseenter");
		},
		z:function($t){
			if($t.nextAll(".lv_cover").is(":visible")) return;
			var items=this.$l.find(".lv_cover:visible"),_this=this,$cv,d,$img,$z,$n,$ia;
			//hide a existing cover
			if(items.length>1){
				if($t.index()%2==0){
					items.eq(1).hide();
				}else{
					items.eq(0).hide();
				};				
			};

			//get image data
			$ia=$t.find("a");
			d=$.evalJSON($ia.attr("rel"));
			//show a cover
			$cv=$t.nextAll("."+_this._o.clCover).stop(true,true).fadeIn("normal");
			//loading
			$cv.append(_this.$ing.show().remove());			
			//get the zoomed image
			$img=$t.find("img");
			d.p=$img.attr("alt");
			d.p=d.p==""?$img.attr("src"):d.p;
			//hide the note
			$n=$cv.find(".lv_ovl,.lv_note").hide().filter(".lv_note").find(".lv_t").html("").end();
			//update data
			$z=$cv.find(".lv_zoom").attr("href",$ia[0].href);
			$cv.find(".lv_t0").html(d.t0);
			$cv.find(".lv_t1").html(d.t1);			
			//load the big image
			$z.find("img").attr("src",d.p);		

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
        clSlider: 'lv_slider_',
		clSliderHandle:"lv_slider_hdl",
		clPanel:'lv_main',
		clContent:'lv_main_',
		cssItem:'.lv_item',
		cssThumb:'.lv_item img',
		clCover:'lv_cover',
		clLoading:'lv_loading',
		opacity:0.7
    };
    // Public functions.
    $.fn.ppSlide.method1 = function(skinName) {
        return;
    };
})(jQuery);  