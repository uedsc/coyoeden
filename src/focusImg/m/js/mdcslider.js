/*
 �Ѻ�����-��Ӱ��ҳ����ͼ���
 1,l=layout
 2,s=slider,scroll bar
 3,p=panel,scroll panel
 4,w=width
 5,ct=content,scroll content
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
		this.wCT=this.$ct.width();
		this.wP=this.$p.width();
		this.wScrollMax=this.$s.width()-this.$sA.width();
		
		//init
		this._i();
	};
	p.M.prototype={
		_i:function(){
			var _this=this;
			var gap=this.wCT-this.wP,step;
			//����ק�Ĺ�����
			this.$sA.draggable({
				axis:'x',
				drag:function(evt,ui){
					if(gap<=0) return;
					step=Math.round(ui.position.left/_this.wScrollMax*gap);
					_this.$ct.css("margin-left",-step);
				},
				containment:'parent'
			}).click(function(e){return false;});
			//ҳ�����ʱ�������м�λ��
			this.$sA.animate({left:this.wScrollMax/2},1000);
			this.$ct.animate({'margin-left':-gap/2},1000);
			//Сͼ��͸��Ч��
			this.$l.find(this._o.cssThumb).css('opacity',this._o.opacity);
			//Сͼ����
			this.$l.find(this._o.cssItem).mouseenter(function(){
				
			});
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
		opacity:0.7
    };
    // Public functions.
    $.fn.ppSlide.method1 = function(skinName) {
        return;
    };
})(jQuery);  