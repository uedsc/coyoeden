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
		this.$s=$t.find("."+opts.clSlider);						/* ������ */
		this.$sA=$t.find("."+opts.clSliderHandle);				/* �������ϵ���ק���� */
		this.$p=$t.find("."+opts.clPanel);						/* ��������jq���� */
		this.$ct=$t.find("."+opts.clContent);					/* ���� jq����*/
		this.$ing=$t.find("."+opts.clLoading);					/* Loadingͼ�� */
		this.$boxes=this.$ct.children();						/* ���ݿ�=ÿ�麬��4��С�� */
		this.wB=this.$boxes.eq(0).width();						/* ��ͼ�� */
		this.wB0=this.wB-2*this._o.gapH;						/* ��ͼʵ�ʿ� */
		this.hB=this.$boxes.eq(0).height();						/* ��ͼ�ܸ� */
		this.hB0=this.hB-2*this._o.gapV;						/* ��ͼʵ�ʸ� */
		this.wS0=(this.wB0-2*this._o.gapH)/2;					/* Сͼʵ�ʿ� */
		this.hS0=(this.hB0-2*this._o.gapV)/2;					/* Сͼʵ�ʸ� */
		this.wCT=this.$boxes.length*this.wB;					/* ���������ܿ� */
		this.$ct.css("width",this.wCT);
		this.wP=this.$p.width();								/* ��������(��Ļ)��� */
		this.wCTGap=this.wCT-this.wP;							/* �ܿ�-���ֿ� */
		this.wScrollMax=this.$s.width()-this.$sA.width();		/* ��������Ч��� */
		this.factorCT=this.wCTGap/this.wScrollMax;				/* ����ϵ�� */
		this.tm=null;											/* ��ʱ�� */
		this.$cur=null;											/* ��ǰ���Ŵ��Сͼ */
		this.$curCv=null;										/* ��ǰ��ͼ ����*/
		this.$curImgB=null;										/* ��ǰ��ͼimg���� */
		this.$curNote=null;
		this.curPos=null;										/* ��ǰСͼλ�� */
		//init
		this._i();
	};
	p.M.prototype={
		_i:function(){
			var _this=this,step;
			//����ק�Ĺ�����-init scroll bar
			this._isb();
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
		/**
		 * �Ŵ�ָ��������ͼ
		 * @param {Object} $t
		 */
		z:function($t){
			if(this.isHot($t)) return;
			var _this=this,d,$img,$z,$ia;
			//������Сǰһ�Ŵ�ͼ
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
		 * ��ͼ����Ч��-��ͬСͼλ���в�ͬ�Ľ���
		 * �����߼����������ҷŴ�;ż������Ŵ��������Сͼʱ����ͼ��Сͼ�ĳߴ縲��Сͼ��Ȼ��Ŵ�
		 */
		_zIn:function(){
			var _this=this;
			//��ͼ������Сͼ��λ��
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
			//ͼƬ�Ľ���
			this.$curImgB.stop(true,true).css({
				width:this.wS0,
				height:this.hS0
			}).animate({
				width:this.wB0,
				height:this.hB0
			},this._o.speed,"linear");			
		},
		/**
		 * ����������λ�ã��Ա㵱ǰ��ͼʼ����ʾ�ڿ�������
		 */
		adjust:function(){
			var boxIndex=this.$curCv.parent().index();
			var ml=parseInt(this.$ct.css("margin-left"));
			var gapRight=(boxIndex+1)*this.wB+ml-this.wP;		/* ��ͼ�����ұ߾�Ĵ�С */
			var gapLeft=boxIndex*this.wB+ml;					/* ��ͼ������߾�Ĵ�С */
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
			//ҳ�����ʱ�������м�λ��
			this.$sA.css({left:this.wScrollMax/2});
			this.$ct.css({'margin-left':-this.wCTGap/2});			
		},
		/**
		 * ָ����ͼ�Ƿ������ڷŴ�״̬
		 */
		isHot:function($t){
			if($t.nextAll(".lv_cover").is(":visible")) return true;
			return false;
		},
		/**
		 * ��С��ǰ��ͼ-r=reset
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
		scrollBar:true,					/* �Ƿ���ʾ������-�����ʾ����Ҫjquery-ui-ppSlide.min.js */
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
		gapV:5,							/* ͼƬ�Ĵ�ֱ���� */
		gapH:5							/* ͼƬ��ˮƽ���� */
    };
})(jQuery);  