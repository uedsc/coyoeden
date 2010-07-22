/**
 * 分栏编辑器-每个分栏具有一个分栏编辑器实体
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip",cssCTSelector:"#content_selector"},opts);
	//属性
	this.$LayoutModel=sohu.diyConsole.$SecEditorModel;/* 工具条的dom模型 */
	this.Console=window.bos;
	this.CurArea=opts.curArea;//当前横切
	this.CurSec=opts.curSec;//当前分栏
	this.CurCT=null;//当前内容
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Toolbar=this.$LayoutModel.find(".actions").clone();	/* editor actions */
	this.$ToolbarF=this.$LayoutModel.find(".footer").clone();	/* editor footer */
	this.$ToolbarTip=this.$Toolbar.find(opts.cssSecHelper);	/* sec tip */
	//按钮事件注册
	this.$Toolbar.btn={
		addContent:this.$Toolbar.find(".a_content"),
		addSec:this.$Toolbar.find(".a_sec"),
		clear:this.$Toolbar.find(".a_clear"),
		editCode:this.$Toolbar.find(".a_code")
	};
	this.$Toolbar.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Toolbar.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$Toolbar.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Toolbar.btn.editCode.click(function(evt){alert("代码");return false;});
	// sec tip事件注册
	this.$ToolbarTip.click(function(evt){$(this).hide();});
	//actions事件注册-双击返回父级分栏
	this.$Toolbar.dblclick(function(evt){
		_this.CurSec.ActiveParent();return false;
	});
	this.$CTWrap=$("#ctWrap");
	
	//persist the toolbar dom
	this.$Toolbar.attr("id",this.CurSec.ID+"_t").hide().appendTo(this.CurArea.$Layout);
	_this.$Toolbar.isNew=true;
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
		if(!_this.SecSelector.Cur) return; /* _this.SecSelector.Cur赋值逻辑参考sohu.diyConsole.js */
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
		this.CTDialog=$(this.__p.opts.cssCTSelector).dialog({
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
 * @param {diyContent} ct 内容对象
 * @param {int} mode 更新内容的类型 1标识末追加；0表示替换；-1表示首追加
 */
sohu.diyEditor.prototype.UpdateCT=function(ct,mode){
	switch(mode){
		case 0:
			this.CurSec.Cls();
			this.CurSec.$Layout.append(ct.$Layout);
		break;
		case 1:
			this.CurSec.$Layout.append(ct.$Layout);
		break;
		case -1:
			this.CurSec.$Layout.prepend(ct.$Layout);
		break;
		default:
			this.CurSec.$Layout.append(ct.$Layout);
		break;
	};
	if(ct.onDomed){
		ct.onDomed(mode);
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
sohu.diyEditor.prototype.Reposition=function(){
		var d=this.CurSec.Dim();
		this.$Toolbar.css({width:d.w-12,top:d.y-31,left:d.x,opacity:0.8});/*宽要减去12个像素的留白;31是高度*/
		this.$ToolbarTip.css({width:d.w}).html("w:"+d.mw+"px");
};
/**
 * 显示编辑器-即激活编辑器
 * @param {Object} opts
 */
sohu.diyEditor.prototype.Show=function(){
	if(this.$Toolbar.isNew){
		this.Reposition();
	}
	this.$Toolbar.show();
		
	if(!this.CurSec.Divisible){
		this.$Toolbar.btn.addSec.hide();	
	}else{
		this.$Toolbar.btn.addSec.show();
	};
};
/**
 * 隐藏
 */
sohu.diyEditor.prototype.Hide=function(){
	if(!this.CurSec) return;
	if(this.CurSec.IsAddingContent) return;
	this.$Toolbar.hide();
};
