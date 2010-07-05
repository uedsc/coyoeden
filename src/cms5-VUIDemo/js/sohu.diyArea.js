/**
 * 类-横切
 * @author levin
 * @desc	横切相关交互
 * @param {Object} opts 选项
 * @dependency sohu.diy.js
 */
sohu.diyArea=function(opts){
///<summary>横切对象</summary>
	//属性
	opts=$.extend({},{clArea:"area",clActive:"area_active",isNew:true,clSec:"sec",clHelper:opts.clHelper||"areaTip"},opts);
	var _this=this;
	this.TemplateID=null;//横切模板id
	this.Console=opts.console;
	this.$Workspace=this.Console.$Workspace;
	this.$Helper=this.Console.__p._$areaHelper.clone();
	this.IsEmpty=true;
	this.IsEditing=false;//是否处于编辑状态:1,添加分栏时为true
	this.IsActive=false;//横切是否激活
	this.HasContent=false;//是否添加了内容
	
	var p={
		opts:opts
	};
	p.addNew=function(){
		_this.$Layout=function(){
			return $(sohu.diyTp[opts.tplID]);
		}();
		_this.$Layout.attr("id",_this.ID);
		if(!_this.Console.CurArea){
			_this.$Workspace.append(_this.$Layout);
		}else{
			_this.Console.CurArea.$Layout.after(_this.$Layout);
		};		
	};
	//绑定事件
	p.bindEvts=function(){
		_this.$Layout.mouseenter(function(evt){_this.Active();})
			.mouseleave(function(evt){_this.Deactive();});		
	};
	//移除绑定的事件处理函数
	p.unbindEvts=function(){
		_this.$Layout.unbind("mouseenter mouseleave");
	};
	//行为
	if(opts.isNew){
		this.TemplateID=opts.tplID;
		this.ID=this.TemplateID+"_"+sohu.diyConsole.RdStr(8);
		p.addNew();
		this.$Layout.prepend(this.$Helper.css("opacity",0.7));
	}else{
		this.$Layout=opts.obj;
		this.IsEmpty=false;
		this.ID=this.$Layout.attr("id");
		this.TemplateID=this.ID.substr(0,this.ID.lastIndexOf("_"));
	};
	//横切事件-注：当横切具有分栏时，由于分栏的鼠标事件函数返回false，停止了事件的冒泡，横切的鼠标事件将不被触发，
	//故这里不适合用鼠标事件
	this.$Layout.effect("highlight",{easing:'easeInElastic'},'slow');
	p.bindEvts();
	
	this.__p=p;
	
	//已有分栏处理
	this.LoadSections();
};
/**
 * 激活
 */
sohu.diyArea.prototype.Active=function(){
	var _this=this;
	if(this.IsActive) return;
	if(this.$Layout.hasClass(this.__p.opts.clActive)) return;
	this.Console.AreaList().removeClass(this.__p.opts.clActive);
	this.$Layout.addClass(this.__p.opts.clActive);
	this.Console.ActiveArea(this).RePosition();
	this.IsActive=true;
	//隐藏助手dom
	this.__p.unbindEvts();
	_this.$Helper.slideUp(400,this.__p.bindEvts);
};
/**
 * 移除激活状态
 */
sohu.diyArea.prototype.Deactive=function(){
	if(this.IsEditing||(!this.IsActive)) return;
	var _this=this;
	_this.$Layout.removeClass(_this.__p.opts.clActive);
	
	this.IsActive=false;
	
	this.__p.unbindEvts();
	//显示助手dom
	if(!_this.IsEmpty){
		_this.$Helper.hide().remove();
	}else{
		_this.$Helper.slideDown(500,_this.__p.bindEvts);
	};

};
/**
 * 移动
 * @param {Object} isUp 上移true，下移false
 */
sohu.diyArea.prototype.Move=function(isUp){
	var sibling=isUp?this.$Layout.prev():this.$Layout.next();
	var tip0=isUp? "不能再往上移动了...":"不能再往下移动了...";
	if (sibling.size() == 0) {
		alert(tip0);return false;
	};	
	if(isUp){sibling.before(this.$Layout);}else{sibling.after(this.$Layout)};
	return false;	
};
/**
 * 移除
 */
sohu.diyArea.prototype.Remove=function(){
	if(!window.confirm("确定删除选中的横切么?")) return false;
	this.$Layout.remove();
	if(this.__p.opts.onRemove){
		this.__p.opts.onRemove(this);
	};
	return false;	
};
/**
 * 加载当前横切内的所有分栏
 */
sohu.diyArea.prototype.LoadSections=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clSec);
	items=items.map(function(i,sec){
		return sohu.diySection.New({$obj:$(sec),editor:_this.Console.Editor,secHelper:_this.Console.__p._$secHelper});
	});
	return items;
};
