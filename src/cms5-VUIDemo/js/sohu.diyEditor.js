/**
 * �����༭��-ÿ����������һ�������༭��ʵ��
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip",cssCTSelector:"#content_selector"},opts);
	//����
	this.$LayoutModel=sohu.diyConsole.$SecEditorModel;/* ��������domģ�� */
	this.$Layout=$(this.$LayoutModel.html());
	this.Console=window.bos;
	this.CurArea=opts.curArea;//��ǰ����
	this.CurSec=opts.curSec;//��ǰ����
	this.CurCT=null;//��ǰ����
	this.WSecCfg=sohu.diyConsole.$WinSec;
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Toolbar=this.$Layout.find(".actions");	/* editor actions */
	this.$Overlay=this.$Layout.find(".overlay")
	this.$ToolbarTip=this.$Toolbar.find(opts.cssSecHelper);	/* sec tip */
	//��ť�¼�ע��
	this.$Toolbar.btn={
		addContent:this.$Toolbar.find(".a_content"),
		addSec:this.$Toolbar.find(".a_sec"),
		clear:this.$Toolbar.find(".a_clear"),
		editCode:this.$Toolbar.find(".a_code"),
		prevLevel:this.$Toolbar.find(".a_ret"),
		cfg:this.$Toolbar.find(".a_cfg")
	};
	this.$Toolbar.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Toolbar.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$Toolbar.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Toolbar.btn.editCode.click(function(evt){_this.DialogCode();return false;});
	this.$Toolbar.btn.prevLevel.click(function(evt){_this.CurSec.ActiveParent();return false;});
	this.$Toolbar.btn.cfg.click(function(evt){_this.DialogSecCfg();return false;});
	//���û�и����������ء��ϼ�����ť
	if(!this.CurSec.HasParent()){
		this.$Toolbar.btn.prevLevel.hide();
	};
	// sec tip�¼�ע��
	this.$ToolbarTip.click(function(evt){$(this).hide();});

	this.$CTWrap=$("#ctWrap");
	
	//persist the editor dom
	this.$Layout.attr("id",this.CurSec.ID+"_t").hide().appendTo(this.CurArea.$Layout);
	_this.$Toolbar.isNew=true;
};
/**
 * ������ӷ���ѡ���
 */
sohu.diyEditor.prototype.DialogSec=function(){
	var _this=this;
	var templateID="#sec_selector_"+this.CurSec.Width;
	this.CurTpl=null;
	this.Editing("on");
	//close callback
	var _onClose=function(evt,ui){
		if(!_this.SecSelector.Cur) return; /* _this.SecSelector.Cur��ֵ�߼��ο�sohu.diyConsole.js */
		_this.Editing("off");
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
		title:"��ӷ���",
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
 * �����������öԻ���
 */
sohu.diyEditor.prototype.DialogSecCfg=function(){
	var _this=this;
	var _onClose=function(evt,ui){
		_this.Editing("off").CurSec.Deactive();
	};
	this.Editing("on");
	this.WSecCfg.dialog("option",{close:_onClose});
	this.WSecCfg.dialog("open");
};
/**
 * ��������Ի���
 */
sohu.diyEditor.prototype.DialogCode=function(){
	var _this=this;
	var _onClose=function(evt,ui){
		_this.Editing("off").CurSec.Deactive();
	};
	this.Editing("on");
	sohu.diyConsole.$WinCode.dialog("option",{close:_onClose});
	sohu.diyConsole.$WinCode.dialog("open");
};
/**
 * �����������ѡ���
 * @param {String} mode mode="update"ʱ�༭html����
 */
sohu.diyEditor.prototype.DialogCT=function(mode){
	var _this=this;
	this.Editing("on");
	
	//�رջص�
	var _onClose=function(evt,ui){
		_this.Editing("off");
	};
	//�򿪻ص�
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
			title:"�������",
			width:680,
			height:500,
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
 * �ر�����ѡ���
 * @param {Object} opts ѡ��
 */
sohu.diyEditor.prototype.CloseCTDialog=function(opts){

	if(!this.CTDialog) return;
	this.CTDialog.dialog("close");
	
};
/**
 * ����diyEditor������-���ڷ���
 * @param {diyContent} ct ���ݶ���
 * @param {int} mode �������ݵ����� 1��ʶĩ׷�ӣ�0��ʾ�滻��-1��ʾ��׷��
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
 * �������
 */
sohu.diyEditor.prototype.Cls=function(){
	var _this=this;
	var d=this.CurSec.Dim();
	var pos=[d.x+10,d.y+10];//�Ի���λ��
	this.Editing("on");
	this.Console.Confirm({
		title:"ȷ�ϲ���",
		ct:"1,����������ʱ��������ݼ����ӷ�����<br/>2,����������ʱɾ���÷�������ͬ������",
		yes:function(ui){
			_this.CurSec.Cls();
		},
		position:pos,
		close:function(evt,ui){
			_this.Editing("off");
		}
	});
	
};
sohu.diyEditor.prototype.Reposition=function(){
		var d=this.CurSec.Dim();
		var st=sohu.diyConsole.$ScrollWrap.scrollTop();/* �Ϲ������� */
		this.$Toolbar.css({width:d.w-11,top:d.y+st-25,left:d.x-1,opacity:0.9});/*��Ҫ��ȥ11�����ص�����;25�ǹ������߶�*/
		this.$ToolbarTip.css({width:d.w}).html(d.mw+"px");
		//overlay
		this.$Overlay.css({width:d.w+1,top:d.y+st,left:d.x-1,opacity:0.9,height:d.h+1});
};
/**
 * ��ʾ�༭��-������༭��
 * @param {Object} opts
 */
sohu.diyEditor.prototype.Show=function(){
	if(this.$Toolbar.isNew){
		this.Reposition();
	}
	this.$Layout.show();
		
	if(!this.CurSec.Divisible){
		this.$Toolbar.btn.addSec.hide();	
	}else{
		this.$Toolbar.btn.addSec.show();
	};
};
/**
 * �����༭�¼���
 * @param {Object} mode "off"��"on"
 */
sohu.diyEditor.prototype.Editing=function(mode){
	if(mode=="on"){
		this.CurSec.IsAddingContent=true;
		this.CurArea.IsEditing=true;
		this.CurSec.$Layout.addClass("ing");
	}else{
		this.CurSec.IsAddingContent=false;
		this.CurArea.IsEditing=false;
		this.CurSec.$Layout.removeClass("ing");
	};
	return this;
};
/**
 * ����
 */
sohu.diyEditor.prototype.Hide=function(){
	//return;
	if(!this.CurSec) return;
	if(this.CurSec.IsAddingContent) return;
	this.$Layout.hide();
};
