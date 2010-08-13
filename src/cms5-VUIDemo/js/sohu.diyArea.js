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
	opts=$.extend({},{clArea:"area",clActive:"area_active",clEmpty:"area_empty",isNew:true,clSec:"sec",clHelper:"areaTip",clContent:"ct"},opts);
	var _this=this;
	this.TemplateID=null;//横切模板id
	this.Console=opts.console;
	this.$Workspace=this.Console.$Workspace;
	this.IsEditing=false;//是否处于编辑状态:1,添加分栏时为true
	this.IsActive=false;//横切是否激活
	
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
	/*
	 横切的激活由分栏控制
	//绑定事件
	p.bindEvts=function(){
		_this.$Layout.mouseenter(function(evt){_this.Active();});
			//.mouseleave(function(evt){_this.Deactive();});		
	};
	//移除绑定的事件处理函数
	p.unbindEvts=function(){
		//_this.$Layout.unbind("mouseenter mouseleave");
		_this.$Layout.unbind("mouseenter");
	};
	*/
	//行为
	if(opts.isNew){
		this.TemplateID=opts.tplID;
		this.ID="content_"+StringUtils.RdStr(8);
		p.addNew();
	}else{
		this.$Layout=opts.obj;
		this.ID=this.$Layout.attr("id");
		this.TemplateID=this.ID.substr(0,this.ID.lastIndexOf("_"));
	};
	//this.$Layout.effect("highlight",null,"fast");
	//p.bindEvts();
	
	this.__p=p;
	
	//已有分栏处理
	this.LoadSections();
};
/**
 * 激活
 */
sohu.diyArea.prototype.Active=function(){
	if(this.IsActive) return;
	if(sohu.diyConsole.CurSec&&(sohu.diyConsole.CurSec.IsAddingContent||sohu.diyConsole.CurSec.InlineEditing)) return;
	var _this=this;
	//if(this.IsActive) return;
	//if(this.$Layout.hasClass(this.__p.opts.clActive)) return;
	//this.Console.AreaList().removeClass(this.__p.opts.clActive);
	this.$Layout.addClass(this.__p.opts.clActive);
	this.Console.ActiveArea(this).RePosition();
	this.IsActive=true;
	//暂时移除事件处理函数
	//this.__p.unbindEvts();
};
/**
 * 移除激活状态
 */
sohu.diyArea.prototype.Deactive=function(){
	if(this.IsEditing||(!this.IsActive)||(sohu.diyConsole.EditingSec!=null)) return;
	this.$Layout.removeClass(this.__p.opts.clActive);
	this.IsActive=false;
	
	//this.__p.unbindEvts();
	//内容是否为空
	if(!this.IsEmpty()){
		this.$Layout.removeClass(this.__p.opts.clEmpty);
	}else{
		//没有内容的话继续显示横切的框架以便用户查看
		this.$Layout.addClass(this.__p.opts.clEmpty);
	};
	//this.__p.bindEvts();

};
/**
 * 更新横切的ID，同时遍历sohu.diyConsole实体的Areas属性
 * @param {Object} newID
 */
sohu.diyArea.prototype.UpdateID=function(newID){
	if((!newID)||(newID==this.ID)||(!sohu.diyConsole.IsValidID(newID))) return;
	//遍历判断该ID是否已经存在
	var isOk=true;
	$.each(this.Console.Areas,function(i,o){
		if(o.ID==newID){
			return (isOk=false);
		};
		return true;
	});
	if (!isOk) {
		alert("编号" + newID + "已经被占用！");
		return false;
	};
	
	this.ID=newID;
	this.$Layout.attr("id",this.ID);
	
	return true;
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
	if(isUp){sibling.before(this.$Layout);}else{sibling.after(this.$Layout);};
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
		return sohu.diySection.New({
			$obj:$(sec),
			curArea:_this
		});
	});
	return items;
};
/**
 * 当前横切是否有内容
 */
sohu.diyArea.prototype.IsEmpty=function(){
	var ct=this.$Layout.find("."+this.__p.opts.clContent);
	return (ct.length==0);
};
/**
 * 返回当前横切相当于window的x和y值,以及自身的高和宽
 * @return {Object} {x,y,w,h}
 */
sohu.diyArea.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
