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
	this.$LayoutModel.btn.addContent.click(function(evt){_this.DialogCT();return false;});
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
	//close callback
	var _onClose=function(evt,ui){
		if(!_this.CurTpl) return;
		_this.CurArea.IsEditing=false;
		_this.CurSec.IsAddingContent=false;
		_this.CurSec.Deactive();
	};
	//Open callback
	var _onOpen=function(evt,ui){
		var dim=_this.CurSec.Dim();
		var pos=[dim.x-100,dim.y+dim.h/3];
		_this.SecSelector.dialog("option","position",pos);
	};
	//open the dialog
	this.SecSelector=$(templateID).dialog({
		title:"添加分栏",
		resizable:false,
		modal:true,
		width:430,
		height:250,
		close:_onClose,
		open:_onOpen,
		autoOpen:false//to reuse the dialog,we have to set autoOpen to false!
	});
	this.SecSelector.dialog("open");
};
/**
 * 弹出添加内容选择框
 */
sohu.diyEditor.prototype.DialogCT=function(){
	var _this=this;
	this.CurArea.IsEditing=true;
	this.CurSec.IsAddingContent=true;
	//关闭回调
	var _onClose=function(evt,ui){
		_this.CurArea.IsEditing=false;
		_this.CurSec.IsAddingContent=false;
	};
	//打开回调
	var _onOpen=function(evt,ui){
		var areaDim=_this.CurArea.Dim();
		var pos=[areaDim.x+areaDim.w/2-300,areaDim.y+areaDim.h/3];
		_this.CTDialog.dialog("option","position",pos);
		var ifr=_this.CTDialog.find("#ifContentList");
		ifr.attr("src",ifr.attr("rel")+"?t="+new Date().getTime());
	};
	sohu.diyConsole.toggleLoading();
	this.CTDialog=$("#content_selector").dialog({
		title:"添加内容",
		width:620,
		height:430,
		modal:true,
		close:_onClose,
		open:_onOpen,
		autoOpen:false
	});
	this.CTDialog.dialog("open");
};
/**
 * 关闭内容选择框
 * @param {Object} opts 选项
 */
sohu.diyEditor.prototype.CloseCTDialog=function(opts){
	if(!this.CTDialog) return;
	this.CTDialog.dialog("close");
};
/**
 * 更新diyEditor的内容-用于分栏
 * @param {Object} $ct 内容(jq dom)
 * @param {int} mode 更新内容的类型 1标识末追加；0表示替换；-1表示首追加
 */
sohu.diyEditor.prototype.UpdateCT=function($ct,mode){
	var body=this.$Layout_body;//this.$Layout.find(".area_body");
	switch(mode){
		case 0:
			body.empty().append($ct);
		break;
		case 1:
			body.append($ct);
		break;
		case -1:
			body.prepend($ct);
		break;
		default:
			body.append($ct);
		break;
	};
};
/**
 * 清空内容
 */
sohu.diyEditor.prototype.Cls=function(){
	var _this=this;
	var pos=[this.CurSec.Dim().x+10,this.CurSec.Dim().y+10];
	this.CurSec.IsAddingContent=true;
	this.CurArea.IsEditing=true;
	this.Console.Confirm({
		title:"确认操作",
		ct:"确定清除分栏的内容么?",
		yes:function(ui){
			_this.$Layout_body.children().not(_this.__p.opts.cssSecHelper).remove();	
		},
		position:pos,
		close:function(evt,ui){
			_this.CurSec.IsAddingContent=false;
			_this.CurArea.IsEditing=false;
		}
	});
	
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
	this.$Layout_actions=this.$Layout.find(".actions");

	this.$Layout_body.append(this.CurSec.$Layout.children());
	this.$Layout.appendTo(this.CurSec.$Layout);
	//this.$Layout_actions.slideDown();
	
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
