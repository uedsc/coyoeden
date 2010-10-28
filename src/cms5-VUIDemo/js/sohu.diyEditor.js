/**
 * 分栏编辑器-每个分栏具有一个分栏编辑器实体
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".vstp_secTip",cssCTSelector:"#vstp_content_selector"},opts);
	//属性
	//this.$LayoutModel=sohu.diyConsole.$SecEditorModel;/* 工具条的dom模型 */
	this.$Layout=$("#vstp_secEditor");
	this.Console=opts.bos;
	this.CurArea=null;//当前横切
	this.CurSec=null;//当前分栏
	this.CurCT=null;//当前内容
	//this.WSecCfg=sohu.diyConsole.$WinSec;
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Toolbar=this.$Layout.find(".vstp_actions");	/* editor actions */
	this.$Overlay=this.$Layout.find(".vstp_overlay");
	this.$ToolbarTip=this.$Toolbar.find(opts.cssSecHelper);	/* sec tip */
	//按钮事件注册
	this.$Toolbar.btn={
		addContent:this.$Toolbar.find(".vstp_a_content"),
		addSec:this.$Toolbar.find(".vstp_a_sec"),
		clear:this.$Toolbar.find(".vstp_a_clear"),
		prevLevel:this.$Toolbar.find(".vstp_a_ret"),
		cfg:this.$Toolbar.find(".vstp_a_cfg")
	};
	this.$Toolbar.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Toolbar.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$Toolbar.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Toolbar.btn.prevLevel.click(function(evt){_this.CurSec.ActiveParent();return false;});
	this.$Toolbar.btn.cfg.click(function(evt){_this.DialogSecCfg();return false;});

	this.$CTWrap=$("#vstp_ctWrap");
	
	//persist the editor dom
	//this.$Layout.attr("id",this.CurSec.ID+"_t").hide().appendTo(this.CurArea.$Layout);
	this.$Toolbar.isNew=true;
};
/**
 * 弹出添加分栏选择框
 */
sohu.diyEditor.prototype.DialogSec=function(){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("subSec"+this.CurSec.Width);
};
/**
 * 弹出分栏设置对话框
 */
sohu.diyEditor.prototype.DialogSecCfg=function(){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("cfgSec");
};
/**
 * 弹出添加内容选择框
 * @param {String} mode mode="update"时编辑html内容
 */
sohu.diyEditor.prototype.DialogCT=function(mode){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("addContent");
};
/**
 * 关闭内容选择框
 * @param {Object} opts 选项
 */
sohu.diyEditor.prototype.CloseCTDialog=function(opts){
	sohu.diyDialog.Hide();
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
	if(this.CurArea.IsEditing) return;
	var _this=this;	
	sohu.diyDialog.doConfirm({
		text:"1,分栏有内容时将清除内容及其子分栏。<br/>2,分栏无内容时删除该分栏及其同级分栏<br/>注意：删除后无法恢复！",
		onOK:function($jqm){
			_this.CurSec.Cls();
			$jqm.jqmHide();
		},
		beforeShow:function(hash){
			_this.Editing("on");
			return true;
		},
		afterShow:function(hash){
			//显示红色蒙层
			_this.CurSec.Overlay("on");
		},
		afterHide:function(hash){
			_this.Editing("off").CurSec.Deactive();
			_this.CurSec.Overlay("off");
		}
	});
	
};
sohu.diyEditor.prototype.Reposition=function(){
	//获取当前的横切、分栏
	this.CurArea=sohu.diyConsole.CurArea;
	this.CurSec=sohu.diyConsole.CurSec;
	//编辑器置入当前分栏
	this.CurSec.$Layout.append(this.$Layout);
	var d=this.CurSec.Dim();
	//this.$Toolbar.css({width:d.w-11,top:d.y-25,left:d.x-1,opacity:0.9});/*宽要减去11个像素的留白;25是工具条高度*/
	this.$Toolbar.css({opacity:0.9});
	//this.$ToolbarTip.css({width:d.w}).html(d.mw);
	this.$ToolbarTip.html(d.mw);
	//overlay
	//this.$Overlay.css({width:d.w+1,top:d.y,left:d.x-1,opacity:0.9,height:d.h+1});
	this.$Overlay.css({width:d.w1+4,top:-2,left:-2,opacity:0.9,height:d.h1+4});
};
/**
 * 显示编辑器-即激活编辑器
 * @param {Object} opts
 */
sohu.diyEditor.prototype.Show=function(){
	if(this.$Toolbar.isNew){
		this.Reposition();
	}
	this.$Layout.show();
	
	//如果没有父分栏，隐藏“上级”按钮
	if(!this.CurSec.HasParent()){
		this.$Toolbar.btn.prevLevel.hide();
	}else{
		this.$Toolbar.btn.prevLevel.show();
	};
		
	if(!this.CurSec.Divisible){
		this.$Toolbar.btn.addSec.hide();	
	}else{
		this.$Toolbar.btn.addSec.show();
	};
};
/**
 * 触发编辑事件。
 * @param {Object} mode "off"或"on"
 */
sohu.diyEditor.prototype.Editing=function(mode){
	if(mode=="on"){
		this.CurSec.IsAddingContent=true;
		this.CurArea.IsEditing=true;
		this.CurSec.$Layout.addClass("vstp_ing");
	}else{
		this.CurSec.IsAddingContent=false;
		this.CurArea.IsEditing=false;
		this.CurSec.$Layout.removeClass("vstp_ing");
	};
	return this;
};
/**
 * 隐藏
 */
sohu.diyEditor.prototype.Hide=function(){
	//return;
	if(!this.CurSec) return;
	if(this.CurSec.IsAddingContent) return;
	this.$Layout.hide();
};
