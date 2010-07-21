/**
 * 类-内容、碎片
 * @author levinhuang
 * @param {Object} opts 选项{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn",scale:true,clElm:"elm"},opts||{});
	var _this=this;
	this.Meta=opts.ct;
	this.$Layout=null;/*在Validate方法中构建*/
	this.Type=opts.ct.type;/* 内容插件类型 */
	this.Sec=opts.sec;//分栏
	this.Editor=this.Sec.Editor;//分栏编辑器
	this.MaxWidth=this.Sec.Width;
	this.ID="ct_"+this.Type+"_"+sohu.diyConsole.RdStr(8);
	this.onDomed=null;/* 被添加到dom树后的回调函数 */
	
	//private property
	var p={opts:opts};
	this.__p=p;
	
	/* 验证内容的有效性 并且构建$Layout*/
	this.Validate();
	if(!this.Validation.valid) return;
	
	/* Load elements */
	this.LoadElements();
	
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(opts.clOn);
		_this.Editor.CurCT=_this;
		sohu.diyConsole.CurCT=_this;
		//拖拽助手事件
		
		var dim=_this.Dim();
		/*v20100722
		sohu.diyConsole.Dragger.handle.show()
		.css({width:dim.w,height:dim.h,opacity:0.3})
		.unbind()
		.bind("mousedown",function(evt){
			sohu.diyConsole.Dragger.ing=true;
			sohu.diyConsole.Dragger.obj=_this;
			_this.Sec.Deactive();
			
		}).bind("mouseup",function(evt){
			sohu.diyConsole.Dragger.ing=false;
		}).bind("dblclick",function(evt){
			_this.DoEdit();return false;
		});
		_this.$Layout.find(".dragHandle").remove().end().prepend(sohu.diyConsole.Dragger.handle);
		*/
	};
	p.mouseLeave=function(evt){
		if(_this.Editor.CurArea.IsEditing) return false;
		_this.$Layout.removeClass(opts.clOn);
		/*sohu.diyConsole.Dragger.handle.remove();v20100722*/
		sohu.diyConsole.CurCT=null;
	};
	
	//内容的鼠标事件
	this.$Layout.mouseenter(p.mouseEnter).mouseleave(p.mouseLeave);
	//是否flash
	if(this.$Layout.flash){
		this.ID+="_fl";
		//将flash对象呈现出来
		var fOpt={tplID:this.$Layout.tplID};
		if(opts.scale){fOpt.w=this.MaxWidth;};
		this.onDomed=function(mode){
			this.$Layout.flashObj=new sohu.diyTp.Flash(fOpt);
			this.$Layout.flashObj.Render(this.$Layout);
		};
	};
	this.$Layout.attr("id",this.ID);
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
/**
 * 利用html编辑器编辑内容
 */
sohu.diyContent.prototype.DoEdit=function(){
	this.Editor.DialogCT("update");
};
/**
 * 验证当前内容是否有效
 */
sohu.diyContent.prototype.Validate=function(){
	var _this=this;
	this.SetValidation(true);
	
	var commonValidate=function(ct,msg){
		ct.$Layout=$(ct.Meta.html0).filter("."+ct.Type);
		if(ct.$Layout.length==0||(!ct.$Layout.is("."+ct.__p.opts.cl))){
			ct.SetValidation(false,msg);
		};
	};
	
	switch(this.Type){
		case "ohmygod":
			this.SetValidation(false,"Html内容不符合可视化专题模板规范");
		break;
		case "shtable":
			this.$Layout=$(this.Meta.html0).filter("table");
			if(this.$Layout.length==0){
				this.SetValidation(false,"Html内容无表格标签");
			}else{
				//将$Layout替换成符合diy内容模板规范的对象
				this.$Layout=$("<div/>").addClass(this.__p.opts.cl+" "+this.Type+" clear").append(this.$Layout);
			}
		break;
		case "shflash":
			this.$Layout=$(this.Meta.html0);
		break;
		case "shline":
			commonValidate(this,"Html内容不符合{空行}模板规范");
		break;
		case "shimage":
			commonValidate(this,"Html内容不符合{图文}模板规范");
		break;
		case "shtext":
			commonValidate(this,"Html内容不符合{文本}模板规范");
		break;
		default:
			this.SetValidation(false,"Html内容不符合可视化专题模板规范");
		break;
	};
	if(this.Validation.valid){
		$.extend(this.$Layout,this.Meta);
	};
	return this;
};
/**
 * 设置验证信息
 * @param {Boolean} isValid 是否有效
 * @param {String} 验证结果提示信息
 */
sohu.diyContent.prototype.SetValidation=function(isValid,msg){
	this.Validation={
		valid:isValid,
		msg:msg||null
	};
	return this;
};
sohu.diyContent.prototype.LoadElements=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clElm);
	items.each(function(i,o){
		new sohu.diyElement({
			$dom:$(o),
			ct:_this
		});
	});
};
/*静态方法*/
/**
 * 从现有的dom元素构建一个diyContent 对象
 * @param {Object} opts 选项
 */
sohu.diyContent.New=function(opts){
	return new sohu.diyContent(opts);
};
