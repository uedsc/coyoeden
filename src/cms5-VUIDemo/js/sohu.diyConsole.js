/**
 * ���ӻ��༭����̨
 * @author levinhuang
 * @param {Object} opts ѡ��,��clSec:"sec",cssWsp:"#main"��ʾ������css��Ϊsec,���������cssѡ����Ϊ#main
 * @dependency sohu.diyEditor.js;sohu.diyArea.js
 */
sohu.diyConsole=function(opts){
	//����
	opts=$.extend({},{
		cssWsp:"#main",clSec:"sec",clSec0:"sec0",clSecSub:"subsec",
		clSecRoot:"col",clArea:"area",cssArea:".area",dfTop:100,
		limitSec:390
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");
	this.CurArea=null;//��ǰ����ĺ��ж���
	this.Editor=new sohu.diyEditor({
		$layoutModel:$("#hiddenTemplate .area_editor"),
		console:_this
	});
	this.Areas=null;
	
	var p={opts:opts};
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	p._$areaSelector=$("#area_selector");
	p._$secHelper=$("#hiddenTemplate .secTip");
	p._$pageTip=$("#pageTip");
	p._opts=opts;
	
	//����ɾ��ʱ�Ļص�����
	p.onAreaRemove=function(area){
		_this.ActiveArea(null);
		_this.$Layout.animate({top:opts.dfTop});	
	};
	
	/**
	 * ��ʾ����ѡ����
	 */
	p.showSelector=function(){
		var _onClose=function(evt,ui){
			if(!p._curAreaTpl) return false;//δѡ���κη���
			var obj=new sohu.diyArea({
				tplID:p._curAreaTpl,
				console:_this,
				onRemove:p.onAreaRemove
			});
			_this.Areas.push(obj);
		};
		//��ʾѡ���
		p._$areaSelector.dialog({
			title:"��Ӻ���",
			resizable:false,
			modal:true,
			width:430,
			height:250,
			position:[700,50],
			close:_onClose
			}
		);
	};
	p.onAdd=function(evt){
	///<summary>��Ӻ���</summary>
		p.showSelector();	
		return false;
	};
	p.onSelectAreaTpl=function(evt){
	///<summary>ѡ��ĳ������ģ���</summary>
		p._curAreaTpl=this.id;p._$areaSelector.dialog("close");	return false;
	};
	p.onRemove=function(evt){
	///<summary>ɾ������</summary>	
		if(!_this.CurArea){alert("δѡ���κκ���!");return false;};
		_this.Areas=$.grep(_this.Areas,function(o,i){
			if(o.ID==_this.CurArea.ID) return false;
			return true;
		});
		_this.CurArea.Remove();
		return false;	
	};
	p.onAddBG=function(evt){
	///<summary>��Ӻ��б���</summary>	
		if(!_this.CurArea){alert("δѡ���κκ���!");return false;};
		alert("����");
		return false;
	};
	p.onMove=function(evt){
	///<summary>�ƶ�����</summary>
		if(!_this.CurArea){alert("δѡ���κκ���!");return false;};	
		var isUp=evt.data.up;
		_this.CurArea.Move(isUp);
		_this.RePosition();
		return false;	
	};
	//body��ǩ������¼�
	p.onMousemove=function(evt){
		if(!_this.CurArea) return;
		if(!_this.CurArea.IsActive) return;
		var lastArea=_this.$Workspace.find(opts.cssArea+":last");
		if(lastArea.size()==0) return;
		var lbtop=_this.$Workspace.offset().top;
		var ubtop=lastArea.height()+lastArea.offset().top;
		var lbleft=lastArea.offset().left;
		var ubleft=lastArea.width()+lbleft;
		
		if(evt.pageX<lbleft||evt.pageX>ubleft||evt.pageY<lbtop||evt.pageY>ubtop){
			_this.CurArea.Deactive();
		};
	};
	p.Init=function(){
		//����ѡ����
		$("li",p._$areaSelector).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");})
			.click(p.onSelectAreaTpl);
		
		p._$btnAdd.bind("click",p.onAdd);
		p._$btnDel.click(p.onRemove);
		//����ɾ����ť
		//p._$btnDel.parent().hide();
		p._$btnBG.click(p.onAddBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
		
		//���к���
		_this.Areas=_this.AreaList().map(function(i,o){
			o=new sohu.diyArea({
				isNew:false,
				console:_this,
				onRemove:p.onAreaRemove
			});
			return o;
		});
		//body����¼�
		$("body").mousemove(p.onMousemove);
	};
	this.__p=p;
	//Init
	p.Init();
};
/**
 * �ض�λ
 */
sohu.diyConsole.prototype.RePosition=function(){
	if(!this.CurArea){this.$Layout.attr("style","");return;};
	this.$Layout.css("top",this.CurArea.$Layout.offset().top);
};
/**
 * ����ָ�����ж���
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//����һ�����з�����
	if(this.CurArea){
		this.CurArea.Deactive();
	};
	//���ǰ�ĺ���
	this.CurArea=target;
	this.Editor.CurArea=target;
	return this;
};
/**
 * �ر�����ѡ��Ի���
 */
sohu.diyConsole.prototype.CloseCTDialog=function(){
	this.Editor.CloseCTDialog();
};
/**
 * ��ȡ���к���jquery����
 */
sohu.diyConsole.prototype.AreaList=function(){
	var items= this.$Workspace.find(this.__p.opts.cssArea);
	return items;
};
/**
 * ����һ��ȷ�϶Ի���
 * @param {Object} opts
 */
sohu.diyConsole.prototype.Confirm=function(opts){
	var _this=this;
	opts=$.extend({},{
		title:"ȷ�ϲ���?",
		ct:"",
		height:140,
		width:"",
		position:"center",
		resizable:false,
		modal:true,
		yes:null,
		no:null,
		close:null
	},opts);
	
	var dlOpt={
		title:opts.title,
		resizable:opts.resizable,
		height:opts.height,
		width:opts.width,
		modal:opts.modal,
		position:opts.position,
		buttons:{
			"ȡ��":function(){
				if(opts.no){opts.no(this);};
				$(this).dialog("close");
			},
			"ȷ��":function(){
				if(opts.yes){opts.yes(this);};
				$(this).dialog("close");
			}
		},
		close:function(evt,ui){
			_this.__p._$pageTip.removeClass("confirm");
			if(opts.close){
				opts.close(evt,ui);
			};
		}
	};
	this.__p._$pageTip.addClass("confirm").html(opts.ct).dialog(dlOpt);
};
/**
 * �Ƴ�.txtLoading
 */
sohu.diyConsole.toggleLoading=function(){
	$(".txtLoading").toggle();
};
/*��̬����*/
sohu.diyConsole.Dragger={ing:false,obj:null};
//TODO:�Ƶ�sohu.stringUtils.js��
/**
 * ��ȡָ�����ȵ�����ַ�����ע�⣺���������ֺ���ĸ���
 * @param {Object} size ����ַ����ĳ���
 * @param {Boolean} plusTimeStamp �Ƿ���ϵ�ǰʱ���
 */
sohu.diyConsole.RdStr=function(size,plusTimeStamp){
	var size0=8;
	var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
	size=size||size0;size=size<1?size0:size;size=size>chars.length?size0:size;
	var s = '';
	for (var i=0; i<size; i++) {
		var rnum = Math.floor(Math.random() * chars.length);
		s += chars.substring(rnum,rnum+1);
	};
	if(plusTimeStamp){
		s+=new Date().getTime();
	};
	return s;
};
