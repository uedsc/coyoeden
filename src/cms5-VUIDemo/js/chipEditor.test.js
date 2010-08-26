/**
 * @author levin
 */
var test = function() {
	var p={},pub={};
	p.getDim=function($i){
		var of=$i.offset();
		var pt=parseInt($i.css("padding-top"));
		pt=isNaN(pt)?0:pt;
		var pb=parseInt($i.css("padding-bottom"));
		pb=isNaN(pb)?0:pb;
		var pl=parseInt($i.css("padding-left"));
		pl=isNaN(pl)?0:pl;
		var pr=parseInt($i.css("padding-right"));
		pr=isNaN(pr)?0:pr;
		
		var dim={
			y:of.top,
			x:of.left,
			h:$i.height(),
			w:$i.width(),
			pt:pt,
			pb:pb,
			pl:pl,
			pr:pr
		};
		
		return dim;
	};
	p.onFlagMEnter=function(evt){	
		var $i=$(this);
		if($i.hasClass("on")) return;/* 在编辑某个元素时碎片会加上on */
		var d=p.getDim($i);
		p._$chipCover.css({
			opacity:0.5,
			top:d.y-1,
			left:d.x-1,
			height:d.h+d.pt+d.pb,
			width:d.w+d.pl+d.pr,
			display:'block'
		}).one("mouseleave",function(evt){
			p._$chipCover.hide();
			//p._$chipCover_t.removeClass("on");
			//$i.bind("mouseenter",p.onFlagMEnter);
		});
		
		
		p._$chipCover_t=$i;
		//p._$chipCover_t.addClass("on");
	};	
	//private area
	p.initVar = function(opts) { 
		p._$chipCover=$("#chipCover");
		p._cssFlash=".flash";
	};
	p.onLoaded = function() { };
	p.initEvents = function(opts) {
		$(document).ready(p.onLoaded);
		$(".chip").bind("mouseenter",p.onFlagMEnter).mouseleave(function(evt){
			p._$chipCover_t.unbind("mouseenter").bind("mouseenter",p.onFlagMEnter);
		});
		//蒙层的单击事件
		p._$chipCover.click(function(evt){
			if(p._$chipCover_t.is(p._cssFlash)){
				//进入焦点图编辑界面
				chipEditor.Show(p._$chipCover_t,{
					tabs:[1,2],
					afterShow:function(hash,dlg){
						//show the flash tip
						dlg.$TabC.eq(1).find(".row").hide().filter(".flashTip").show();
						//content of tab no2
						dlg.$Code.val(dlg.$Chip.html());
						dlg.$CT.addClass("jqmFlash");/* 控制排序按钮的显示 */
					}
				});
				return;
			};
			
			p._$chipCover.trigger("mouseleave");
			p._$chipCover_t.unbind("mouseenter");
			
			//显示碎片编辑器
			chipEditor.Show(p._$chipCover_t);
		});
	};
	//public area
	pub.Init = function(opts) {
		p.initVar(opts);
		p.initEvents(opts);
	};
	return pub;
} (); 

test.Init({});
