/**
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
	p.daddy=function(evt){return false;};
	p.navL=function(evt,noAminate){
		var $dom=evt.data.$t,opts=evt.data.opts;
		if(noAminate){
			var l=$dom._p.panel.css("left");
			$dom._p.panel.css({"left": l+ opts.step});
		}else{
			$dom._p.panel.animate({left:'+='+opts.step,opacity:opts.opacity},opts.duration,function(){
				$dom._p.panel.css({opacity:1});
			});			
		};

		$dom._p.slideNum--;
		
		//�ұ߱�����ͬʱ���¼�
		if($dom._p.btnR.hasClass(opts.clNoNav))
			$dom._p.btnR.removeClass(opts.clNoNav).unbind("click.nav").bind("click.nav",{$t:$dom,opts:opts},p.navR);
		
		if($dom._p.slideNum<=0){
			//ZUO�߰�ť��ң�ͬʱ�Ƴ��󶨵��¼�
			$dom._p.btnL.addClass(opts.clNoNav).unbind("click.nav");
		};
		$dom._p.dir="-1";
		if(opts.autoSlide&&$dom._p.t==null){
			p.autoSlide($dom,opts);
		};
		if(opts.onSlide)
			opts.onSlide($dom,"-1");			
	};
	p.navR=function(evt,noAminate){
		var $dom=evt.data.$t,opts=evt.data.opts;
		if(noAminate){
			var l=$dom._p.panel.css("left");
			$dom._p.panel.css({"left": l-opts.step});
		}else{
			$dom._p.panel.animate({left:'-='+opts.step,opacity:opts.opacity},opts.duration,function(){
				$dom._p.panel.css({opacity:1});
			});			
		};

		$dom._p.slideNum++;
		
		//��߱�����ͬʱ���¼�
		if($dom._p.btnL.hasClass(opts.clNoNav))
			$dom._p.btnL.removeClass(opts.clNoNav).unbind("click.nav").bind("click.nav",{$t:$dom,opts:opts},p.navL);
		
		if($dom._p.slideNum>=($dom._p.total-opts.showNum)){
			//�ұ߰�ť��ң�ͬʱ�Ƴ��󶨵��¼�
			$dom._p.btnR.addClass(opts.clNoNav).unbind("click.nav");
		};
		
		$dom._p.dir="1";
		if(opts.autoSlide&&$dom._p.t==null){
			p.autoSlide($dom,opts);
		};
		
		if(opts.onSlide)
			opts.onSlide($dom,"1");			
	};
	p.autoSlide=function($dom,opts){
		if(!opts.autoSlide) return;
		if($dom._p.total<=opts.showNum) return;
		$dom._p.t=window.setInterval(function(){
			if($dom._p.btnR.hasClass(opts.clNoNav)){
				$dom._p.btnL.trigger("click");
			}else if($dom._p.btnL.hasClass(opts.clNoNav)){
				$dom._p.btnR.trigger("click");
			}else{
				if((!$dom._p.dir)||$dom._p.dir=="1"){
					$dom._p.btnR.trigger("click");
				}else{
					$dom._p.btnL.trigger("click");
				};//if
			};//if
		},opts.autoSlide);
	};
    p.goSlide = function($dom,opts) {
		$dom._p.btnR.bind("click",p.daddy);
		$dom._p.btnL.bind("click",p.daddy);
		
		if($dom._p.total<=opts.showNum){
			$dom._p.btnL.addClass(opts.clNoNav);
			$dom._p.btnR.addClass(opts.clNoNav);
			return;
		}; 
		$dom._p.btnR.bind("click.nav",{$t:$dom,opts:opts},p.navR);
		//$dom._p.btnL.bind("click.nav",{$t:$dom,opts:opts},p.navL);
		$dom._p.items.click(function(evt){
			if(opts.autoSlide){
				window.clearInterval($dom._p.t);
				$dom._p.t=null;
			};
			if(opts.onClick){
				opts.onClick($dom,opts);
			};
		});
	};
    //main plugin body
    $.fn.abcdSlider = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.abcdSlider.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			$this._p={
				btnL:opts.externalBtn?$(opts.cssBtnL):$this.find(opts.cssBtnL),
				btnR:opts.externalBtn?$(opts.cssBtnR):$this.find(opts.cssBtnR),
				items:$this.find(opts.cssItem),
				total:opts.total?opts.total:$this.find(opts.cssItem).length,
				panel:$this.find(opts.cssPanel),
				slideNum:0,
				t:null,
				dir:null
			};
			p.goSlide($this,opts);
			p.autoSlide($this,opts);
        });
    };
    // Public defaults.
    $.fn.abcdSlider.defaults = {
        showNum: 5,			/* ������������������ʾ��Ԫ���� */
		step:135,			/* �������� */
		clNoNav:'noNav',	/* ���ɻ���ʱ��ť���ӵ�css�� */
		duration:'normal',	/* �����ٶ� */
		opacity:0.25,		/* ����ʱ����������͸���� */
		cssBtnL:'.btnL',	/* ��/�ϰ�ť */
		cssBtnR:'.btnR',	/* ��/�°�ť  */
		externalBtn:false,	/* �Ƿ�ť���ⲿ */
		total:null,			/* ����Ԫ���� */
		cssItem:'.item',	/* ����Ԫ��cssѡ����,����趨��total��Ԫ������totalΪ׼ */
		cssPanel:'.slider',	/* �������� */
		onSlide:null,		/* ������Ļص����� */
		onClick:null,		/* Ԫ�ص���¼��Ļص� */
		autoSlide:false		/* �Զ����� */
    };
})(jQuery);  