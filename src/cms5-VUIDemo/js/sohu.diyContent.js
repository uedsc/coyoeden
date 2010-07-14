/**
 * 类-内容、碎片
 * @author levinhuang
 * @param {Object} opts 选项{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn",scale:true},opts||{});
	var _this=this;
	this.$Layout=opts.$obj;
	this.Sec=opts.sec;//分栏
	this.Editor=this.Sec.Editor;//分栏编辑器
	this.MaxWidth=this.Sec.Width;
	this.ID="ct_"+sohu.diyConsole.RdStr(8);
	//private property
	var p={opts:opts};
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(opts.clOn);
		_this.Editor.CurCT=_this;
		
		//拖拽助手事件
		
		var dim=_this.Dim();
		sohu.diyConsole.Dragger.handle.show()
		.css({width:dim.w,height:dim.h,opacity:0.3})
		.unbind()
		.bind("mousedown",function(evt){
			sohu.diyConsole.Dragger.ing=true;
			sohu.diyConsole.Dragger.obj=_this;
			_this.Sec.Deactive();
			
		}).bind("mouseup",function(evt){
			sohu.diyConsole.Dragger.ing=false;
		});
		_this.$Layout.prepend(sohu.diyConsole.Dragger.handle);
		
	};
	p.mouseLeave=function(evt){
		_this.$Layout.removeClass(opts.clOn);
		sohu.diyConsole.Dragger.handle.remove();
	};
	this.__p=p;
	
	//内容的鼠标事件
	this.$Layout.mouseenter(p.mouseEnter).mouseleave(p.mouseLeave);
	//是否flash
	if(this.$Layout.flash){
		this.ID+="_fl";
		this.$Layout.attr("id",this.ID);
		//将flash对象呈现出来
		var fOpt={tplID:this.$Layout.tplID};
		if(opts.scale){fOpt.w=this.MaxWidth;};
		this.$Layout.flashObj=new sohu.diyTp.Flash(fOpt);
		this.$Layout.flashObj.Render(this.$Layout);
	};
};
/**
 * 获取内容的维度信息
 * @return {Object} {x,y,w,h}
 */
sohu.diyContent.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
/*静态方法*/
/**
 * 从现有的dom元素构建一个diyContent 对象
 * @param {Object} opts 选项
 */
sohu.diyContent.New=function(opts){
	return new sohu.diyContent(opts);
};
