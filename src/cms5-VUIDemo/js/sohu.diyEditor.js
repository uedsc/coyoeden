/**
 * 分栏编辑器
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip"},opts);
	//属性
	this.$LayoutModel=opts.$layoutModel;
	this.Console=opts.console;
	this.CurArea=opts.curArea;//当前横切
	this.CurSec=opts.curSec;//当前分栏
	this.CurCT=null;//当前内容
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Layout=this.$LayoutModel;
	this.$LayoutA=this.$Layout.find(".actions");/*editor actions*/
	this.$LayoutF=this.$Layout.find(".footer");/*editor footer*/
	this.$LayoutT=this.$Layout.find(opts.cssSecHelper);/*sec tip*/
		//按钮事件注册
	this.$Layout.btn={
		addContent:this.$Layout.find(".a_content"),
		addSec:this.$Layout.find(".a_sec"),
		clear:this.$Layout.find(".a_clear"),
		editCode:this.$Layout.find(".a_code")
	};
	this.$Layout.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Layout.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$Layout.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Layout.btn.editCode.click(function(evt){alert("代码");return false;});
	// sec tip事件注册
	this.$LayoutT.click(function(evt){$(this).hide();});
	//actions事件注册-双击返回父级分栏
	this.$LayoutA.dblclick(function(evt){
		_this.CurSec.ActiveParent();return false;
	});
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
 * @param {String} mode mode="update"时编辑html内容
 */
sohu.diyEditor.prototype.DialogCT=function(mode){
	var _this=this;
	this.CurArea.IsEditing=true;
	this.CurSec.IsAddingContent=true;
	//关闭回调
	var _onClose=function(evt,ui){
		_this.CurArea.IsEditing=false;
		_this.CurSec.IsAddingContent=false;
		_this.RePosition();
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
	if(!mode){
		this.CTDialog=$("#content_selector").dialog({
			title:"添加内容",
			width:660,
			height:430,
			modal:true,
			close:_onClose,
			open:_onOpen,
			autoOpen:false
		});
	}else{
		if(!sohu.diyConsole.CurCT) return;
		sohu.diyConsole.CurCT.isNew=false;
	}
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
	switch(mode){
		case 0:
			this.CurSec.Cls();
			this.CurSec.$Layout.append($ct);
		break;
		case 1:
			this.CurSec.$Layout.append($ct);
		break;
		case -1:
			this.CurSec.$Layout.prepend($ct);
		break;
		default:
			this.CurSec.$Layout.append($ct);
		break;
	};
};
/**
 * 清空内容
 */
sohu.diyEditor.prototype.Cls=function(){
	var _this=this;
	var d=this.CurSec.Dim();
	var pos=[d.x+10,d.y+10];//对话框位置
	this.CurSec.IsAddingContent=true;
	this.CurArea.IsEditing=true;
	this.Console.Confirm({
		title:"确认操作",
		ct:"确定清除分栏的内容么?",
		yes:function(ui){
			_this.CurSec.Cls();
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
	this.RePosition();
	this.CurArea.$Layout.append(this.$LayoutA);
	this.CurArea.$Layout.append(this.$LayoutF);		
	
	if(!this.CurSec.Divisible){
		this.$Layout.btn.addSec.hide();	
	}else{
		this.$Layout.btn.addSec.show();
	};
};
/**
 * 移除
 */
sohu.diyEditor.prototype.Remove=function(){
	if(!this.CurSec) return;
	if(this.CurSec.IsAddingContent) return;
	this.$LayoutA.appendTo(this.$Layout);
	this.$LayoutF.appendTo(this.$Layout);
};
/**
 * 更新编辑器的位置.一般在添加分栏或者内容后调用此方法
 */
sohu.diyEditor.prototype.RePosition=function(){
	if(!this.CurSec) return;
	var d=this.CurSec.Dim();
	this.$LayoutA.css({width:d.w-12,top:d.y-31,left:d.x,opacity:0.8});/*宽要减去12个像素的留白;31是高度*/
	this.$LayoutF.css({width:d.w-12,top:d.y+d.h-8,left:d.x});/*8是footer的高度*/
	this.$LayoutT.show().css({width:d.w}).html("w:"+d.w+"px");
	return this;
};
