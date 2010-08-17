/**
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
	p.daddy=function(evt){return false;};
	p.navL=function(evt){
		var $dom=evt.data.$t,opts=evt.data.opts;
		$dom._p.panel.animate({left:'+='+opts.step,opacity:opts.opacity},opts.duration,function(){
			$dom._p.panel.css({opacity:1});
		});
		$dom._p.slideNum--;
		
		//右边变亮，同时绑定事件
		$dom._p.btnR.removeClass("noNav").unbind("click.nav").bind("click.nav",{$t:$dom,opts:opts},p.navR);
		
		if($dom._p.slideNum<=0){
			//ZUO边按钮变灰，同时移除绑定的事件
			$dom._p.btnL.addClass("noNav").unbind("click.nav");
			//计数器清零
			//p.slideNum=0;
		};			
	};
	p.navR=function(evt){
		var $dom=evt.data.$t,opts=evt.data.opts;
		$dom._p.panel.animate({left:'-='+opts.step,opacity:opts.opacity},opts.duration,function(){
			$dom._p.panel.css({opacity:1});
		});
		$dom._p.slideNum++;
		
		//左边变亮，同时绑定事件
		$dom._p.btnL.removeClass("noNav").unbind("click.nav").bind("click.nav",{$t:$dom,opts:opts},p.navL);
		
		if($dom._p.slideNum>=($dom._p.total-opts.showNum)){
			//右边按钮变灰，同时移除绑定的事件
			$dom._p.btnR.addClass("noNav").unbind("click.nav");
			//计数器清零
			//p.slideNum=0;
		};		
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
		$dom._p.btnL.bind("click.nav",{$t:$dom,opts:opts},p.navL);
	};
    //main plugin body
    $.fn.abcdSlider = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.abcdSlider.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			$this._p={
				btnL:$this.find(opts.cssBtnL),
				btnR:$this.find(opts.cssBtnR),
				total:$this.find(opts.cssItem).length,
				panel:$this.find(opts.cssPanel),
				slideNum:0
			};
			p.goSlide($this,opts);
        });
    };
    // Public defaults.
    $.fn.abcdSlider.defaults = {
        showNum: 5,
		step:135,
		clNoNav:'noNav',
		duration:'normal',
		opacity:0.25,
		cssBtnL:'.btnL',
		cssBtnR:'.btnR',
		cssItem:'.item',
		cssPanel:'.slider'
    };
})(jQuery);  