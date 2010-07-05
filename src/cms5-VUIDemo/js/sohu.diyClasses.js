/**
 * @author levinhuang
 * @desc	CMS5可视化编辑器-js交互类定义
 * @dependence 1,sohu.diy.js
 */

/**
 * @class SubArea manager
 * @opts 选项
 */
sohu.diy.SubAreaManager=function(opts){
	var _this=this;
	/*BEGIN属性定义*/
	//TODO:可配置
	this.cfg={
		colClass:"col",/*空分栏css class*/
		colClassActive:"subarea-active",
		areaWidth:950,
		editorBodyClass:"area_body"
	};
	
	this.area=null;/*目标横切-横切的jq对象*/
	this.areaIsEmpty=false;/*目标横切是空横切-即950横切*/
	this.columns=null;
	this.columnsX=null;
	this.canSubArea=true;
	this.target=null;
	this.curTemplate=null;/*当前选中的分栏模板*/
	
	/*END属性定义*/
	
	//分栏选择器事件注册
	$('#hiddenTemplate .subarea_selector li').click(function(evt){
		_this.curTemplate=this.id;
		_this.areaSelector.dialog("close");
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
	
};
sohu.diy.SubAreaManager.prototype.SetArea=function(area){
	//横切变化后，重设各属性
	this.area=area;
	this.target=null;
	this.columnsX=[];
	this.columns=null;
	this.curTemplate=null;
	//读取分栏
	this.LoadColumns();
};
/**
 * 读取并更新当前分栏的子分栏
 */
sohu.diy.SubAreaManager.prototype.LoadColumns=function(){	
	var _this=this;
	this.columnsX=[];
	this.columns=this.area.find("."+this.cfg.colClass);
	this.areaIsEmpty=(this.columns.length==0);
	//空横切的特殊处理
	if(this.areaIsEmpty){
		this.columns=this.area.addClass(this.cfg.colClass);
	};
	//分析各列看是否可以分栏
	this.columns.each(function(i,obj){
		var width=_this.getColSize($(obj));
		obj.canSubArea=false;
		if(width>=390){obj.canSubArea=true;obj._width=width;_this.columnsX.push(obj);};
	});//each
	this.columns.click(function(evt){
		_this.SetTarget($(this));
	});
	
	this.canSubArea=this.columnsX.length>0;
};
sohu.diy.SubAreaManager.prototype.SetTarget=function(colObj){
	$(this.columns).removeClass(this.cfg.colClassActive);
	if (colObj) {
		this.target = colObj.addClass(this.cfg.colClassActive);
	};
	return this;
};
sohu.diy.SubAreaManager.prototype.Dialog=function(){
	var _this=this;
	this.CheckTarget();
	//var width=this.getColSize(this.target);
	//if(width==390) return;
	if (!this.target[0].canSubArea) {
		alert("尺寸小于390，无法再进行拆分");
		return;
	};
	var templateID="#subarea_selector_"+this.target[0]._width;
	var _onClose=function(evt,ui){
		if(!_this.curTemplate) return;
		if(_this.areaIsEmpty){
			_this.target.find("."+_this.cfg.editorBodyClass).html(sohu.diyTp[_this.curTemplate]);
		}else{
			_this.target.html(sohu.diyTp[_this.curTemplate]);
		};
		_this.SetTarget(null);
		//该分栏不再是空分栏
		_this.target.removeClass(_this.cfg.colClass).unbind("click");
		//刷新子分栏
		_this.LoadColumns();
	};
	this.areaSelector=$(templateID).dialog({
		title:"添加分栏",
		resizable:false,
		modal:true,
		width:430,
		height:250,
		close:_onClose
	});
	
};
sohu.diy.SubAreaManager.prototype.getColSize=function(colObj){
	//空横切特殊情况
	if(this.areaIsEmpty){
		return this.cfg.areaWidth;	
	};
	//其他带分栏的横切
	var width=0;
	var classes=colObj.attr("class").split(" ");
	$.each(classes,function(i1,o1){
		if(o1.indexOf("w")==0){
			width=parseInt(o1.substr(1));
			return false;
		};
	});//each
	return width;
};
/**
 * 检查当前分栏的目标栏是否存在，如果不存在则默认指定第一栏
 */
sohu.diy.SubAreaManager.prototype.CheckTarget=function(){
	if(!this.target){this.SetTarget(this.columns.eq(0));};
};
sohu.diy.SubAreaManager.prototype.AddContent=function(ct){
	this.CheckTarget();
	if (this.areaIsEmpty) {
		this.target.find("." + this.cfg.editorBodyClass).append(ct);
	}else{
		this.target.append(ct);
	};
};

/**
 * @class AreaEditor
 */
sohu.diy.AreaEditor=function(opts){
	//属性
	this.layoutModel=opts.layoutModel;
	this.target=null;//当前横切的dom
	this.area=null;//当前横切对象
	this.subAreaManager=new sohu.diy.SubAreaManager({});
	this.contentDialog=null;
};
sohu.diy.AreaEditor.prototype.AddSubArea=function(){
	if(!this.target) return;
	this.subAreaManager.Dialog();
};
sohu.diy.AreaEditor.prototype.Show=function(opts){
	var _this=this;
	if (!opts.target) {
		alert("AreaEditor调用Show方法失败-参数target无效");return false;
	};
	//prepare ui
	this.layout=this.layoutModel.clone();
	this.layout_body=this.layout.find(".area_body");
	this.layout_actions=this.layout.find(".actions").hide();
	this.btn={
		addContent:this.layout.find(".a_content"),
		addSubArea:this.layout.find(".a_subArea"),
		clear:this.layout.find(".a_clear"),
		editCode:this.layout.find(".a_code")
	};

	this.target=opts.target;
	this.area=opts.area;
	this.layout_body.append(this.target.children());
	this.layout.appendTo(this.target);
	this.layout_actions.show();
	
	//subAreaManager
	this.subAreaManager.SetArea(this.target);
	
	//按钮事件注册
	this.btn.addContent.click(function(evt){_this.contentDialog=$("#content_selector").dialog({title:"添加内容",width:600,height:420,modal:true});return false;});
	this.btn.addSubArea.click(function(evt){_this.AddSubArea();return false;});
	this.btn.clear.click(function(evt){_this.area.Remove();return false;});
	this.btn.editCode.click(function(evt){alert("代码");return false;});
};
sohu.diy.AreaEditor.prototype.Remove=function(){
	if(!this.target) return;
	if(!this.layout_body) return;
	this.subAreaManager.SetTarget(null);
	this.target.html(this.layout_body.html());
	this.layout_body=null;
	this.layout=null;this.layout_actions=null;
	if(this.area.hasContent){
		this.target.removeClass("area-empty");
	};
};
sohu.diy.AreaEditor.prototype.AddContent=function(ct){
	this.subAreaManager.AddContent(ct);
	this.area.hasContent=true;
};
/**
 * 关闭添加内容的对话框
 * @param {Object} opt 选项
 */
sohu.diy.AreaEditor.prototype.CloseContentDialog=function(opt){
	if(!this.contentDialog) return;
	this.contentDialog.dialog("close");
};
