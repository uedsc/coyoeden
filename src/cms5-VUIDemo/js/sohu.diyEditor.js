/**
 * �����༭��-ÿ����������һ�������༭��ʵ��
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip",cssCTSelector:"#content_selector"},opts);
	//����
	//this.$LayoutModel=sohu.diyConsole.$SecEditorModel;/* ��������domģ�� */
	this.$Layout=$("#secEditor");
	this.Console=opts.bos;
	this.CurArea=null;//��ǰ����
	this.CurSec=null;//��ǰ����
	this.CurCT=null;//��ǰ����
	//this.WSecCfg=sohu.diyConsole.$WinSec;
	
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

	this.$CTWrap=$("#ctWrap");
	
	//persist the editor dom
	//this.$Layout.attr("id",this.CurSec.ID+"_t").hide().appendTo(this.CurArea.$Layout);
	_this.$Toolbar.isNew=true;
};
/**
 * ������ӷ���ѡ���
 */
sohu.diyEditor.prototype.DialogSec=function(){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("subSec"+this.CurSec.Width);
};
/**
 * �����������öԻ���
 */
sohu.diyEditor.prototype.DialogSecCfg=function(){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("cfgSec");
};
/**
 * ��������Ի���
 */
sohu.diyEditor.prototype.DialogCode=function(){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("code");
};
/**
 * �����������ѡ���
 * @param {String} mode mode="update"ʱ�༭html����
 */
sohu.diyEditor.prototype.DialogCT=function(mode){
	if(this.CurArea.IsEditing) return;
	sohu.diyDialog.Show("addContent");
};
/**
 * �ر�����ѡ���
 * @param {Object} opts ѡ��
 */
sohu.diyEditor.prototype.CloseCTDialog=function(opts){
	/*
	if(!this.CTDialog) return;
	this.CTDialog.dialog("close");
	*/
	sohu.diyDialog.Hide();
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
	if(this.CurArea.IsEditing) return;
	var _this=this;	
	sohu.diyDialog.doConfirm({
		text:"<p>1,����������ʱ��������ݼ����ӷ�����<br/>2,����������ʱɾ���÷�������ͬ������</p><p>ע�⣺ɾ�����޷��ָ�</p>",
		onOK:function(dlg){
			_this.CurSec.Cls();
			dlg.Hide();
		},
		beforeShow:function(hash,dlg){
			_this.Editing("on");
			return true;
		},
		afterHide:function(hash,dlg){
			_this.Editing("off").CurSec.Deactive();
		}
	});
	
};
sohu.diyEditor.prototype.Reposition=function(){
	//��ȡ��ǰ�ĺ��С�����
	this.CurArea=sohu.diyConsole.CurArea;
	this.CurSec=sohu.diyConsole.CurSec;
	var d=this.CurSec.Dim();
	var st=sohu.diyConsole.$ScrollWrap.scrollTop();/* �Ϲ������� */
	this.$Toolbar.css({width:d.w-11,top:d.y-25,left:d.x-1,opacity:0.9});/*��Ҫ��ȥ11�����ص�����;25�ǹ������߶�*/
	this.$ToolbarTip.css({width:d.w}).html(d.mw+"px");
	//overlay
	this.$Overlay.css({width:d.w+1,top:d.y,left:d.x-1,opacity:0.9,height:d.h+1});
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
	
	//���û�и����������ء��ϼ�����ť
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
