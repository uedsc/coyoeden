/**
 * �����༭��
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip"},opts);
	//����
	this.$LayoutModel=opts.$layoutModel;
	this.Console=opts.console;
	this.CurArea=opts.curArea;//��ǰ����
	this.CurSec=opts.curSec;//��ǰ����
	this.CurCT=null;//��ǰ����
	
	var p={opts:opts};
	this.__p=p;
	
	this.$Layout=this.$LayoutModel;
	this.$LayoutA=this.$Layout.find(".actions");/*editor actions*/
	this.$LayoutF=this.$Layout.find(".footer");/*editor footer*/
	this.$LayoutT=this.$Layout.find(opts.cssSecHelper);/*sec tip*/
		//��ť�¼�ע��
	this.$Layout.btn={
		addContent:this.$Layout.find(".a_content"),
		addSec:this.$Layout.find(".a_sec"),
		clear:this.$Layout.find(".a_clear"),
		editCode:this.$Layout.find(".a_code")
	};
	this.$Layout.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$Layout.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$Layout.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$Layout.btn.editCode.click(function(evt){alert("����");return false;});
	// sec tip�¼�ע��
	this.$LayoutT.click(function(evt){$(this).hide();});
	//actions�¼�ע��-˫�����ظ�������
	this.$LayoutA.dblclick(function(evt){
		_this.CurSec.ActiveParent();return false;
	});
	this.$Parent=this.$LayoutModel.parent();
	//����ѡ�����¼�ע��
	$('#hiddenTemplate .sec_selector li').click(function(evt){
		_this.CurTpl=this.id;
		//_this.SecSelector.dialog("close");
		_this.CurSec.AddSub($(sohu.diyTp[_this.CurTpl]));
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
};
/**
 * ������ӷ���ѡ���
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
 * �����������ѡ���
 * @param {String} mode mode="update"ʱ�༭html����
 */
sohu.diyEditor.prototype.DialogCT=function(mode){
	var _this=this;
	this.CurArea.IsEditing=true;
	this.CurSec.IsAddingContent=true;
	//�رջص�
	var _onClose=function(evt,ui){
		_this.CurArea.IsEditing=false;
		_this.CurSec.IsAddingContent=false;
		_this.RePosition();
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
		this.CTDialog=$("#content_selector").dialog({
			title:"�������",
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
 * �ر�����ѡ���
 * @param {Object} opts ѡ��
 */
sohu.diyEditor.prototype.CloseCTDialog=function(opts){
	if(!this.CTDialog) return;
	this.CTDialog.dialog("close");
};
/**
 * ����diyEditor������-���ڷ���
 * @param {Object} $ct ����(jq dom)
 * @param {int} mode �������ݵ����� 1��ʶĩ׷�ӣ�0��ʾ�滻��-1��ʾ��׷��
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
 * �������
 */
sohu.diyEditor.prototype.Cls=function(){
	var _this=this;
	var d=this.CurSec.Dim();
	var pos=[d.x+10,d.y+10];//�Ի���λ��
	this.CurSec.IsAddingContent=true;
	this.CurArea.IsEditing=true;
	this.Console.Confirm({
		title:"ȷ�ϲ���",
		ct:"ȷ���������������ô?",
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
 * ���ӵ�ָ���ķ�������
 * @param {Object} sec ��������
 */
sohu.diyEditor.prototype.AttachTo=function(sec){
	//�ȴӾɵı༭�������Ƴ��༭��
	if(this.CurSec){
		this.CurSec.Deactive();
	};
	this.CurSec=sec;
	return this;
};
/**
 * ��ʾ�༭��-������༭��
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
 * �Ƴ�
 */
sohu.diyEditor.prototype.Remove=function(){
	if(!this.CurSec) return;
	if(this.CurSec.IsAddingContent) return;
	this.$LayoutA.appendTo(this.$Layout);
	this.$LayoutF.appendTo(this.$Layout);
};
/**
 * ���±༭����λ��.һ������ӷ����������ݺ���ô˷���
 */
sohu.diyEditor.prototype.RePosition=function(){
	if(!this.CurSec) return;
	var d=this.CurSec.Dim();
	this.$LayoutA.css({width:d.w-12,top:d.y-31,left:d.x,opacity:0.8});/*��Ҫ��ȥ12�����ص�����;31�Ǹ߶�*/
	this.$LayoutF.css({width:d.w-12,top:d.y+d.h-8,left:d.x});/*8��footer�ĸ߶�*/
	this.$LayoutT.show().css({width:d.w}).html("w:"+d.w+"px");
	return this;
};
