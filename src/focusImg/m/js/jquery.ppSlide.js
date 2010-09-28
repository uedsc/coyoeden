/*
 �Ѻ�����-��Ӱ��ҳ����ͼ���
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
		this.$s=$t.find("."+opts.clSlider);
		this.$sA=$t.find("."+opts.clSliderHandle);
		this.$p=$t.find("."+opts.clPanel);
		this.$ct=$t.find("."+opts.clContent);
		this.$ing=$t.find("."+opts.clLoading);
		this.$boxes=this.$ct.children();
		//this.wCT=this.$ct.width();
		this.wCT=this.$boxes.length*this.$boxes.eq(0).width();
		this.$ct.css("width",this.wCT);
		this.wP=this.$p.width();
		this.wCTGap=this.wCT-this.wP;
		this.wScrollMax=this.$s.width()-this.$sA.width();
		this.factorCT=this.wCTGap/this.wScrollMax;
		this.tm=null;
		
		//init
		this._i();
	};
	p.M.prototype={
		_i:function(){
			var _this=this,step;
			//����ק�Ĺ�����
			this.$sA.draggable({
				axis:'x',
				drag:function(evt,ui){
					if(_this.wCTGap<=0) return;
					step=Math.round(ui.position.left*_this.factorCT);
					_this.$ct.css("margin-left",-step);
				},
				containment:'parent'
			}).click(function(e){return false;});
			//ҳ�����ʱ�������м�λ��
			this.$sA.css({left:this.wScrollMax/2});
			this.$ct.css({'margin-left':-this.wCTGap/2});
			//Сͼ��͸��Ч��
			this.$l.find(this._o.cssThumb).css('opacity',this._o.opacity);
			//Сͼ����
			this.$l.find(this._o.cssItem).mouseenter(function(){
				_this.z($(this));
			});
			//��ͼonload�¼�
			this.$l.find(".lv_zoom img").load(function(){
				_this.$ing.remove();
			});
			//�����ɲ��͸��Ч��
			this.$l.find(".lv_ovl").css("opacity",0.6);
			//��ʼ����ͼ
			this.$l.find(".lv_init").trigger("mouseenter");
		},
		z:function($t){
			if($t.nextAll(".lv_cover").is(":visible")) return;
			var items=this.$l.find(".lv_cover:visible"),_this=this,$cv,d,$img,$z,$n,$ia;
			//stop existing animation
			this.$l.find("."+this._o.clCover).stop(true,true);
			//hide a existing cover
			if(items.length>1){
				if($t.index()%2==0){
					items.eq(1).hide();
				}else{
					items.eq(0).hide();
				};				
			}else{
				items.hide();
			};

			//get image data
			$ia=$t.find("a");
			d=$.evalJSON($ia.attr("rel"));
			//show a cover
			$cv=$t.nextAll("."+_this._o.clCover).animate({height:"hide",width:"hide"},0).animate({height:"show",width:"show"},_this._o.speed,"linear",function(){
				$cv.find(".lv_ovl,.lv_note").fadeIn("normal");
				_this.adjust($cv);
			});
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

		},
		/**
		 * ����������λ�ã��Ա㵱ǰ��ͼʼ����ʾ�ڿ�������
		 * @param {Object} $cv
		 */
		adjust:function($cv){
			var boxIndex=$cv.parent().index(),wCV=$cv.width();
			var ml=parseInt(this.$ct.css("margin-left"));
			var gapRight=(boxIndex+1)*wCV+ml-this.wP;		/* ��ͼ�����ұ߾�Ĵ�С */
			var gapLeft=boxIndex*wCV+ml;					/* ��ͼ������߾�Ĵ�С */
			ml=isNaN(ml)?0:ml;
			if(gapLeft<0){
				//������Ҫ���ұ߹���
				this.s(true,-gapLeft);
				return;
			};
			if(gapRight>0){
				//������Ҫ����߹���
				this.s(false,gapRight);
			};
		},
		/**
		 * ����������������С������
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
			//����
			this.$ct.stop(true,true).animate({"margin-left":val});
			//С������
			this.$sA.stop(true,true).animate({"left":step});
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
		opacity:0.7,
		speed:500
    };
})(jQuery);  