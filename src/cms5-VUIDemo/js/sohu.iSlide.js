/**
 * ���ָ��Ԫ�صļ򵥻���(ǰ����,�󻬶�)���
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
    //ΪԪ�ؼӷ�ҳ����
	p.make = function($t) {
		var cfg=$t.data("cfg");
		var items=cfg.$items;
		if(items.length<1) return;
		var slide=function(){
			if(cfg.opts.vertical){
				//��ֱ
				
			}else{
				//ˮƽ
				
			};
		};
	};
    //main plugin body
    $.fn.iSlide = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.iSlide.defaults, opts);
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			var $this=$(this);
			//����û���ʵ�������
			var data={
				opts:opts,
				$btnPrev:$this.find(opts.cssBtnPrev),
				$btnNext:$this.find(opts.cssBtnNext),
				$items:$this.find(opts.cssItem)
			};
			$this.data("cfg",data);//should i do it via $this.cfg=data?
			p.make($this);
        });
    };
    // Public defaults.
    $.fn.iSlide.defaults = {
		cssItem:'>*',/*��������Ԫ��*/
        cssBtnPrev: '.btn0',/*��ǰ������ť*/
		cssBtnNext:'.btn1',/*��󵼺���ť*/
		alwaysNav:false,/*�����Ƿ��з�ҳ����ʾ������ť*/
		step:1,/*ÿ����һ�εĲ���.��λ����*/
		cycle:true,
		vertical:false,
		auto:false,
		interval:2000
    };
})(jQuery);  