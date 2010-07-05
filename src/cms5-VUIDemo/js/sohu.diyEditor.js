/**
 * 分栏编辑器
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip"},opts);
	//属性
	this.$Layout=null;
	this.$LayoutModel=opts.$layoutModel;
	this.Console=opts.console;
	this.CurArea=opts.curArea;//当前横切
	this.CurSec=opts.curSec;//当前分栏
	
	var p={opts:opts};
	this.__p=p;
	
	//按钮事件注册
	this.$LayoutModel.btn={
		addContent:this.$LayoutModel.find(".a_content"),
		addSec:this.$LayoutModel.find(".a_sec"),
		clear:this.$LayoutModel.find(".a_clear"),
		editCode:this.$LayoutModel.find(".a_code")
	};
	this.$LayoutModel.btn.addContent.click(function(evt){_this.CTDialog=$("#content_selector").dialog({title:"添加内容",width:600,height:420,modal:true});return false;});
	this.$LayoutModel.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$LayoutModel.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$LayoutModel.btn.editCode.click(function(evt){alert("代码");return false;});
	
	//this.$Layout=this.$LayoutModel.clone(true);
	this.$Layout=this.$LayoutModel;
	this.$Parent=this.$LayoutModel.parent();
	//分栏选择器事件注册
	$('#hiddenTemplate .sec_selector li').click(function(evt){
		_this.CurTpl=this.id;
		//_this.SecSelector.dialog("close");
		_this.CurSec.AddSub($(sohu.diyTp[_this.CurTpl]));
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
};
/**
 * 弹出添加分栏选择框
 */
sohu.diyEditor.prototype.DialogSec=function(){
	var _this=this;
	var templateID="#sec_selector_"+this.CurSec.Width;
	this.CurTpl=null;
	this.CurArea.IsEditing=true;
	this.CurSec.IsAddingContent=true;
	var _onClose=function(evt,ui){
		if(!_this.CurTpl) return;
		_this.CurArea.IsEditing=false;
		_this.CurSec.IsAddingContent=false;
		_this.CurSec.Deactive();
	};
	this.SecSelector=$(templateID).dialog({
		title:"添加分栏",
		resizable:false,
		modal:true,
		width:430,
		height:250,
		close:_onClose
	});
};
/**
 * 更新diyEditor的内容
 * @param {int} mode 更新内容的类型 1标识末追加；0表示替换；-1表示首追加
 * @param {Object} ct 内容
 */
sohu.diyEditor.prototype.UpdateCT=function(mode,ct){
	var body=this.$Layout_body;//this.$Layout.find(".area_body");
	switch(mode){
		case 0:
			body.empty().append(ct);
		break;
		case 1:
			body.append(ct);
		break;
		case -1:
			body.prepend(ct);
		break;
		default:
			body.append(ct);
		break;
	};
};
/**
 * 清空内容
 */
sohu.diyEditor.prototype.Cls=function(){
	if(!window.confirm("确定清空当前分栏的内容?")) return;
	this.$Layout_body.children().not(this.__p.opts.cssSecHelper).remove();
};
/**
 * 附加到指定的分栏上面
 * @param {Object} sec 分栏对象
 */
sohu.diyEditor.prototype.AttachTo=function(sec){
	//先从旧的编辑对象上移除编辑器
	if(this.CurSec){
		this.CurSec.Deactive();
	};
	this.CurSec=sec;
	return this;
};
/**
 * 显示编辑器-即激活编辑器
 * @param {Object} opts
 */
sohu.diyEditor.prototype.Show=function(){
	var _this=this;
	//prepare ui
	this.$Layout_body=this.$Layout.find(".area_body");
	this.$Layout_actions=this.$Layout.find(".actions").hide();

	this.$Layout_body.append(this.CurSec.$Layout.children());
	this.$Layout.appendTo(this.CurSec.$Layout);
	this.$Layout_actions.slideDown();
	
	if(!this.CurSec.Divisible){
		this.$Layout.find(".a_sec").hide();	
	}else{
		this.$Layout.find(".a_sec").show();
	};
};
/**
 * 移除
 */
sohu.diyEditor.prototype.Remove=function(){
	if(!this.CurSec) return;
	if(!this.$Layout_body) return;
	if(this.CurSec.IsAddingContent) return;
	this.$Layout_body.children().appendTo(this.CurSec.$Layout);
	this.$Layout.prependTo(this.$Parent);
};
