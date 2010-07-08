/**
 * �����༭��
 * @author levinhuang
 */
sohu.diyEditor=function(opts){
	var _this=this;
	opts=$.extend({},{cssSecHelper:".secTip"},opts);
	//����
	this.$Layout=null;
	this.$LayoutModel=opts.$layoutModel;
	this.Console=opts.console;
	this.CurArea=opts.curArea;//��ǰ����
	this.CurSec=opts.curSec;//��ǰ����
	
	var p={opts:opts};
	this.__p=p;
	
	//��ť�¼�ע��
	this.$LayoutModel.btn={
		addContent:this.$LayoutModel.find(".a_content"),
		addSec:this.$LayoutModel.find(".a_sec"),
		clear:this.$LayoutModel.find(".a_clear"),
		editCode:this.$LayoutModel.find(".a_code")
	};
	this.$LayoutModel.btn.addContent.click(function(evt){_this.DialogCT();return false;});
	this.$LayoutModel.btn.addSec.click(function(evt){_this.DialogSec();return false;});
	this.$LayoutModel.btn.clear.click(function(evt){_this.Cls();return false;});
	this.$LayoutModel.btn.editCode.click(function(evt){alert("����");return false;});
	
	//this.$Layout=this.$LayoutModel.clone(true);
	this.$Layout=this.$LayoutModel;
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
 */
sohu.diyEditor.prototype.DialogCT=function(){
	var _this=this;
	this.CurArea.IsEditing=true;
	this.CurSec.IsAddingContent=true;
	//�رջص�
	var _onClose=function(evt,ui){
		_this.CurArea.IsEditing=false;
		_this.CurSec.IsAddingContent=false;
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
	this.CTDialog=$("#content_selector").dialog({
		title:"�������",
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
 * �������
 */
sohu.diyEditor.prototype.Cls=function(){
	var _this=this;
	var pos=[this.CurSec.Dim().x+10,this.CurSec.Dim().y+10];
	this.CurSec.IsAddingContent=true;
	this.CurArea.IsEditing=true;
	this.Console.Confirm({
		title:"ȷ�ϲ���",
		ct:"ȷ���������������ô?",
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
 * �Ƴ�
 */
sohu.diyEditor.prototype.Remove=function(){
	if(!this.CurSec) return;
	if(!this.$Layout_body) return;
	if(this.CurSec.IsAddingContent) return;
	this.$Layout_body.children().appendTo(this.CurSec.$Layout);
	this.$Layout.prependTo(this.$Parent);
};
