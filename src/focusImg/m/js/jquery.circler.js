/**
 * @author levinhuang
 */
;(function($) {
    // Private functions.
    var p = {};
    p.M = function($t,opts) { 
		this._opts=opts;
		this.$t=$t;
		this.iDia=opts.iDia;
		this.iRadius=this.iDia/2;
		this.iDivide=opts.iDivide;
		
		this.init();
	};
	p.M.prototype={
		init:function(){
			this.angle=360/this.iDivide;
			this.offsetX=this.offsetY=this.iRadius;
			//draw the circle
			this.draw();
			this.rock();
		},
		draw:function(){
			//sinA=a/r;cosA=b/r;
			var a=0,x,y,i=0;
			if(!this._opts.iHot){
				this._opts.iHot=Math.floor(this.iDivide/4);
			};
			while(a<360){
				x=this.offsetX+Math.cos(a*Math.PI/180)*this.iRadius;
				y=this.offsetY-Math.sin(a*Math.PI/180)*this.iRadius;
				
				this.$t.append($("<div/>",{
					className:this._opts.clItem + " ccl_A"+a+(this._opts.iHot==i?" ccl_hot":""),
					css:{
						left:x,
						top:y
					},
					html:'<img src="'+this._opts.data[i].img+'" alt=""/>'
				}));
				
				if(this._opts.iHot==i){
					this.star={
						x:x,
						y:y,
						a:a
					};
					this.curHot=i;
				};
				
				a+=this.angle;
				i++;
			};
			this.$items=this.$t.find("."+this._opts.clItem);
		},
		rock:function(){
			var _i=this;
			
			this._tm=window.setInterval(function(){
				_i.curHot=_i.curHot+1>=_i.iDivide?0:(_i.curHot+1);
				_i.move(_i.curHot);
			},this._opts.speed);
			
			this.$curHot=this.$items.eq(this.curHot);
			this.curHot=this.curHot+1>=this.iDivide?0:(this.curHot+1);
			this.move(this.curHot);			
		},
		move:function(i){
			var prev=null,_i=this,fired=false;
			
			this.$curHot=this.$items.removeClass("ccl_hot").eq(i).addClass("ccl_hot");
				
			this.$items.each(function(j,o){
				o=$(o),prev=o.prev();
				if(prev.length==0){
					prev=_i.$items.filter(':last');
				};
				prev=prev.position();
				o.stop(true,true).animate({
					left:prev.left,
					top:prev.top
				},200);
			}).mouseenter(function(){
				_i.$items.stop(true,true);			
				clearInterval(_i._tm);
				clearTimeout(_i._tm1);
				
			}).mouseleave(function(){
				clearInterval(_i._tm);
				clearTimeout(_i._tm1);
				_i._tm1=window.setTimeout(function(){
					_i.rock();
				},_i._opts.speed);
			});
		}
	};
    //main plugin body
    $.fn.circler = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.circler.defaults, opts);

        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
        	new p.M($(this),opts);
		});
    };
    // Public defaults.
    $.fn.circler.defaults = {
        iDia: 200,
		iDivide:12,
		speed:3000,
		clItem:'ccl_item',
		iHot:null,
		onMove:null
    };
})(jQuery);