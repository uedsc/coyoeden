/**
 * ���ݱ༭��
 * @author levinhuang
 */
sohu.diyCTEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssCTSelector:"#vstp_content_selector"},opts);
	//����
	this.$Layout=$("#vstp_ctEditor");
	this.Console=opts.bos;
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Toolbar=this.$Layout.find(".vstp_actions");	/* editor actions */
	this.$Overlay=this.$Layout.find(".vstp_overlay");	
	this.$menuOthers=this.$Layout.find(".vstp_others");
	//��ť�¼�ע��
	this.$Toolbar.btn={
		addContent:this.$Toolbar.find(".vstp_a_content"),
		clear:this.$Toolbar.find(".vstp_a_clear"),
		editCode:this.$Toolbar.find(".vstp_a_code"),
		cfg:this.$Toolbar.find(".vstp_a_cfg"),
		cfgSec:this.$Toolbar.find(".vstp_a_cfgSec")
	};
	
	this.$Toolbar.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Toolbar.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Toolbar.btn.editCode.click(function(evt){_this.DialogCode();return false;});
	this.$Toolbar.btn.cfg.mouseenter(function(evt){_this.$menuOthers.show();return false;}).click(sohu.diyConsole.OnStopNav);
	
	//�����¼�ע��
	//this.$menuOthers.mouseleave(function(){_this.$menuOthers.hide();});
	
	//��ק
	this.$Toolbar.find(".vstp_dragHandle").mousedown(function(){
		sohu.diyConsole.Dragger.obj=sohu.diyConsole.CurCT;
		sohu.diyConsole.Dragger.ing=true;
	}).mouseup(function(){
		sohu.diyConsole.Dragger.ing=false;
	});

	this.$CTWrap=$("#vstp_ctWrap");
	
	this.$Toolbar.isNew=true;
};
/**
 * ��������Ի���
 */
sohu.diyCTEditor.prototype.DialogCode=function(){
	if(sohu.diyConsole.CurArea.IsEditing) return;
	sohu.diyDialog.Show("code");
};
/**
 * �����������ѡ���
 * @param {String} mode mode="update"ʱ�༭html����
 */
sohu.diyCTEditor.prototype.DialogCT=function(mode){
	if(sohu.diyConsole.CurArea.IsEditing) return;
	sohu.diyDialog.Show("addContent");
};
/**
 * �ر�����ѡ���
 * @param {Object} opts ѡ��
 */
sohu.diyCTEditor.prototype.CloseCTDialog=function(opts){
	sohu.diyDialog.Hide();
};
/**
 * ����diyEditor������-������Ƭ
 * @param {diyContent} ct ���ݶ���
 * @param {int} mode �������ݵ����� -1��ʾ��׷��
 */
sohu.diyCTEditor.prototype.UpdateCT=function(ct,mode){
	switch(mode){
		case -1:
			sohu.diyConsole.CurCT.$Layout.before(ct.$Layout);
		break;
		default:
			sohu.diyConsole.CurCT.$Layout.after(ct.$Layout);
		break;
	};
	if(ct.onDomed){
		ct.onDomed(mode);
	};
};
/**
 * �������
 */
sohu.diyCTEditor.prototype.Cls=function(){
	if(sohu.diyConsole.CurArea.IsEditing) return;
	var _this=this;	
	sohu.diyDialog.doConfirm({
		text:"ȷ��ɾ������Ƭ����<br/>ע�⣺ɾ�����޷��ָ���",
		onOK:function($jqm){
			sohu.diyConsole.CurCT.Cls();
			$jqm.jqmHide();
		},
		beforeShow:function(hash){
			sohu.diyConsole.Editing("on");
			return true;
		},
		afterShow:function(hash){
			//��ʾ��ɫ�ɲ�
			//_this.CurSec.Overlay("on");
		},
		afterHide:function(hash){
			//_this.Editing("off").CurSec.Deactive();
			//_this.CurSec.Overlay("off");
		}
	});
	
};
sohu.diyCTEditor.prototype.Reposition=function(){
	//���ݱ༭������
	sohu.diyConsole.CurCT.$Layout.append(this.$Layout);
	//��ȡ��ǰ�ĺ��С�����
	var d=sohu.diyConsole.CurCT.Dim();
	//this.$Toolbar.css({width:d.w1-11,top:d.y-25,left:d.x-1,opacity:0.9});/*��Ҫ��ȥ11�����ص�����;25�ǹ������߶�*/
	this.$Toolbar.css({width:d.w1-11,top:-25,left:-1,opacity:0.9});
	//overlay
	//this.$Overlay.css({width:d.w1+1,top:d.y,left:d.x-1,opacity:0.9,height:d.h1+1});
	this.$Overlay.css({width:d.w1+1,top:0,left:-1,opacity:0.9,height:d.h1+1});
};
/**
 * ��ʾ�༭��-������༭��
 * @param {Object} opts
 */
sohu.diyCTEditor.prototype.Show=function(){
	if(this.$Toolbar.isNew){
		this.Reposition();
	}
	this.$Layout.show();
};
/**
 * �����༭�¼���
 * @param {Object} mode "off"��"on"
 */
sohu.diyCTEditor.prototype.Editing=function(mode){
	if(mode=="on"){
		sohu.diyConsole.CurSec.IsAddingContent=true;
		sohu.diyConsole.CurArea.IsEditing=true;
		sohu.diyConsole.CurSec.$Layout.addClass("vstp_ing");
	}else{
		sohu.diyConsole.CurSec.IsAddingContent=false;
		sohu.diyConsole.CurArea.IsEditing=false;
		sohu.diyConsole.CurSec.$Layout.removeClass("vstp_ing");
	};
	return this;
};
/**
 * ����
 */
sohu.diyCTEditor.prototype.Hide=function(){
	//return;
	if(!sohu.diyConsole.CurCT) return;
	if(sohu.diyConsole.CurSec.IsAddingContent) return;
	this.$Layout.hide();
};
