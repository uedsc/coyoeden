/**
 * �����༭��-ÿ����������һ�������༭��ʵ��
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".vstp_secTip",cssCTSelector:"#vstp_content_selector"},opts);
	//����
	//this.$LayoutModel=sohu.diyConsole.$SecEditorModel;/* ��������domģ�� */
	this.$Layout=$("#vstp_secEditor");
	this.Console=opts.bos;
	this.CurArea=null;//��ǰ����
	this.CurSec=null;//��ǰ����
	this.CurCT=null;//��ǰ����
	//this.WSecCfg=sohu.diyConsole.$WinSec;
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Toolbar=this.$Layout.find(".vstp_actions");	/* editor actions */
	this.$Overlay=this.$Layout.find(".vstp_overlay");
	this.$ToolbarTip=this.$Toolbar.find(opts.cssSecHelper);	/* sec tip */
	//��ť�¼�ע��
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
		text:"1,����������ʱ��������ݼ����ӷ�����<br/>2,����������ʱɾ���÷�������ͬ������<br/>ע�⣺ɾ�����޷��ָ���",
		onOK:function($jqm){
			_this.CurSec.Cls();
			$jqm.jqmHide();
		},
		beforeShow:function(hash){
			_this.Editing("on");
			return true;
		},
		afterShow:function(hash){
			//��ʾ��ɫ�ɲ�
			_this.CurSec.Overlay("on");
		},
		afterHide:function(hash){
			_this.Editing("off").CurSec.Deactive();
			_this.CurSec.Overlay("off");
		}
	});
	
};
sohu.diyEditor.prototype.Reposition=function(){
	//��ȡ��ǰ�ĺ��С�����
	this.CurArea=sohu.diyConsole.CurArea;
	this.CurSec=sohu.diyConsole.CurSec;
	//�༭�����뵱ǰ����
	this.CurSec.$Layout.append(this.$Layout);
	var d=this.CurSec.Dim();
	//this.$Toolbar.css({width:d.w-11,top:d.y-25,left:d.x-1,opacity:0.9});/*��Ҫ��ȥ11�����ص�����;25�ǹ������߶�*/
	this.$Toolbar.css({opacity:0.9});
	//this.$ToolbarTip.css({width:d.w}).html(d.mw);
	this.$ToolbarTip.html(d.mw);
	//overlay
	//this.$Overlay.css({width:d.w+1,top:d.y,left:d.x-1,opacity:0.9,height:d.h+1});
	this.$Overlay.css({width:d.w1+4,top:-2,left:-2,opacity:0.9,height:d.h1+4});
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
		this.CurSec.$Layout.addClass("vstp_ing");
	}else{
		this.CurSec.IsAddingContent=false;
		this.CurArea.IsEditing=false;
		this.CurSec.$Layout.removeClass("vstp_ing");
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
