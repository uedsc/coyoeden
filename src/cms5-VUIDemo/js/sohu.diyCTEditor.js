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
	this.CT=null;						/* ��ǰ�༭����Ŀ������ */
	
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
		cfgCT:this.$Toolbar.find(".vstp_a_cfgCT")
	};
	
	this.$Toolbar.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Toolbar.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Toolbar.btn.editCode.click(function(evt){_this.DialogCode();return false;});
	this.$Toolbar.btn.cfgCT.click(function(evt){_this.DialogCTCfg();return false;});
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
 * �������öԻ���
 */
sohu.diyCTEditor.prototype.DialogCTCfg=function(){
	if(sohu.diyConsole.CurArea.IsEditing) return;
	sohu.diyDialog.Show("cfgCT");
};
/**
 * �����������ѡ���
 * @param {String} mode mode="update"ʱ�༭html����
 */
sohu.diyCTEditor.prototype.DialogCT=function(mode){
	if(sohu.diyConsole.CurArea.IsEditing) return;
	sohu.diyDialog.Get("addContent").$Layout.data("mode",1);//0��ʾ������������ݣ�1��ʾ����Ƭ�·��������
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
			_this.Detach();
			sohu.diyConsole.CurCT.Cls();
			$jqm.jqmHide();
		},
		beforeShow:function(hash){
			_this.Editing("on");
			return true;
		},
		afterShow:function(hash){
			//��ʾ��ɫ�ɲ�
			//_this.CurSec.Overlay("on");
			//_this.CT.Blink({speed:3000}).Blink(false);
			_this.Highlight();
		},
		afterHide:function(hash){
			_this.Editing("off");
			sohu.diyConsole.CurSec.Deactive();
			//_this.CurSec.Overlay("off");
			_this.Highlight('off');
		}
	});
	
};
/**
 * highlight the editor background
 * @param {Object} mode 'on' or 'off'
 */
sohu.diyCTEditor.prototype.Highlight=function(mode){
	mode=mode||'on';
	if(mode=='on'){
		this.$Overlay.addClass('vstp_overlay_hot');
	}else{
		this.$Overlay.removeClass("vstp_overlay_hot");
	};
	return this;
};
/**
 * �༭���������ݶ���
 * @param {Object} ct
 */
sohu.diyCTEditor.prototype.AttachTo=function(ct){
	ct.$Layout.append(this.$Layout);
	this.CT=ct;
	return this;
};
sohu.diyCTEditor.prototype.Detach=function(){
	this.$Layout.detach();
	this.CT=null;
	return this;
};
/**
 * ���¶�λ��Ƭ�༭������
 */
sohu.diyCTEditor.prototype.Reposition=function(){		
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
