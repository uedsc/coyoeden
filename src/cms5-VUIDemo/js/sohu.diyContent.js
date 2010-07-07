/**
 * 类-内容、碎片
 * @author levinhuang
 * @param {Object} opts 选项{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn"},opts||{});
	var _this=this;
	this.$Layout=opts.$obj;
	this.Sec=opts.sec;//分栏
	this.MaxWidth=this.Sec.Width;
	this.ID="ct_"+sohu.diyConsole.RdStr(8);
	//private property
	var p={opts:opts};
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(opts.clOn).css("opacity",0.6);
	};
	p.mouseLeave=function(evt){
		_this.$Layout.removeClass(opts.clOn).css("opacity",1);
	};
	this.__p=p;
	
	//内容的鼠标事件
	this.$Layout.mouseenter(p.mouseEnter).mouseleave(p.mouseLeave);
	//是否flash
	if(this.$Layout.flash){
		this.ID+="_fl";
		this.$Layout.attr("id",this.ID);
		//将flash对象呈现出来
		this.$Layout.flashObj=new sohu.diyTp[this.$Layout.tplID]({tplID:this.$Layout.tplID,w:this.MaxWidth});
		this.$Layout.flashObj.Render(this.$Layout);
	};
};
