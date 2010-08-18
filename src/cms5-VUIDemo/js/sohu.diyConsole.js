/**
 * ���ӻ��༭����̨
 * @author levinhuang
 * @param {Object} opts ѡ��,��clSec:"sec",cssWsp:"#main"��ʾ������css��Ϊsec,���������cssѡ����Ϊ#main
 * @dependency sohu.diyEditor.js;sohu.diyArea.js
 * TODO:�������˵������߼��Ƶ�������js�ļ�diyMenuBar.js
 */
sohu.diyConsole=function(opts){
	//����
	opts=$.extend({},{
		cssWsp:"#main",clSec:"sec",clSec0:"sec0",clSecSub:"subsec",
		clSecRoot:"col",clArea:"area",cssArea:".area",dfTop:100,
		clAreaStatic:"static",
		limitSec:390,
		scrollWrapMainginTop:0
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");

	this.Areas=null;
	
	var p={opts:opts};
	p._$pageTip=$("#pageTip");
	p._$elmTool=$("#elmTool");
	/* �Ի���jq���� */
	p._$wAreaBG=$("#wAreaBG")
	p._$wPageBG=$("#wPageBG");
	p._$wCpkWrap=$("#cpkWrap");
	p._$wAddLink=$("#addLink");
	p._$wSecHead=$("#cfgSecHead");
	p._$wSec=$("#wCfgSec");
	p._$wCode=$("#wCode");
	/* /�Ի���jq���� */

	p._$txtFontColor=$("#txtFontColor");
	p._opts=opts;
	/* =/���������˵�= */
	
	p.getWorkspaceBoundary=function(){
		var lastArea=_this.$Workspace.find(opts.cssArea+":last");
		if (lastArea.size() == 0) {
			return {
				lbleft: -2000,
				ubleft: 2000,
				lbtop: -2000,
				ubtop: 2000
			};
		};
		
		var lbtop=_this.$Workspace.offset().top;
		var ubtop=lastArea.height()+lastArea.offset().top;
		var lbleft=lastArea.offset().left;
		var ubleft=lastArea.width()+lbleft;
		
		return {
			lbleft:lbleft,
			ubleft:ubleft,
			lbtop:lbtop,
			ubtop:ubtop
		};
	};
	//body��ǩ������¼�
	p.onMousemove=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		//if(sohu.diyConsole.EditingSec!=null) return;
		
		var b=p.getWorkspaceBoundary();
		
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//���������
			sohu.diyConsole.CurArea.Deactive();
			//���������
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
				
			//�Ƴ���ק���ֺ������ɲ�
			sohu.diyConsole.Dragger.handle.hide();
			sohu.diyConsole.$EHolder.hide();	
		};
	};
	p.onBodyClick=function(evt){
		//�û��Ƿ���#editMenu
		if($(evt.target).parents(".jqmWindow").length>0) return;
		
		var b=p.getWorkspaceBoundary();
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//sohu.diyDialog.Hide(true);
			//sohu.diyConsole.Preview();			
		};
		return false;
	};
	p.setDocumentDim=function(){
		return;
		var fullheight, height;
		fullheight = sohu.diyConsole.InnerHeight();        
		height = fullheight - p.opts.scrollWrapMainginTop;
		
		sohu.diyConsole.$ScrollWrap.css("height",height);
		_this.$Workspace.css("minHeight",height);
	};
	p.onLoaded=function(){
		//�ĵ��߶���Ӧ����
		p.setDocumentDim();
		//���й�����λ��
		_this.$Layout.css({"top":p.opts.scrollWrapMainginTop+30});
	};
	/**
	 * Save current document.selection to sohu.diyConsole.DocSelection
	 */
	p.saveSelection=function(){
		if(sohu.diyConsole.CurElm.InlineEditable){
			sohu.diySelection.snap(sohu.diyConsole.CurElm.i$frame[0].iDoc());
		}else{
			sohu.diySelection.snap(document,sohu.diyConsole.CurElm.$Layout[0]);
		};
	};
	p.onEHolderClick=function(evt){
		if((!sohu.diyConsole.$EHolder)||(!sohu.diyConsole.$EHolder.t)) return false;
		sohu.diyConsole.$EHolder.t.ForceEdit();
		return false; 
	};
	p.Init=function(){
		//������������
		sohu.diyConsole.$WinSec=p._$wSec;
		sohu.diyConsole.$WinCode=p._$wCode;
		//sohu.diyConsole.$WinPageBG=p._$wPageBG;
		sohu.diyConsole.$SecEditorModel=$("#area_editor");
		sohu.diyConsole.$ScrollWrap=$("#scrollWrap");
		sohu.diyConsole.$BodyBGA=$("#main .bodyBGA");
		sohu.diyConsole.$BodyBGB=$("#main .bodyBGB");
		sohu.diyConsole.$ifEditor=$("#ifEditor").iframeEX();	
		sohu.diyConsole.SecEditor=new sohu.diyEditor({bos:_this});
		sohu.diyConsole.$EHolder=$('#eHolder').click(p.onEHolderClick);	
		sohu.diyConsole.$AreaHolder=$("#areaHolder");
		//���к���
		_this.Areas=_this.AreaList().map(function(i,o){
			var a=new sohu.diyArea({
				isNew:false,
				console:_this,
				onRemove:p.onAreaRemove,
				obj:$(o)
			});
			return a;
		});
		//body����¼�
		sohu.diyConsole.$Body=$("body").mousemove(p.onMousemove).click(p.onBodyClick);
		//window resize�¼�
		sohu.diyConsole.$Window=$(window).resize(function(evt){
			p.setDocumentDim();
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Editor.Reposition();
		});
		new sohu.diyMenuBar({});
		//�������
		sohu.diyDialog.Init({console:_this,cssDragCTM:'window'});
		//on page loaded
		$(document).ready(p.onLoaded);
	};
	this.__p=p;
	//Init
	p.Init();
};
/**
 * �ض�λ
 */
sohu.diyConsole.prototype.RePosition=function(){
	sohu.diyDialog.wAreaTool.Reposition();
};
/**
 * �趨����ĺ��ж���
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//����һ�����з�����
	if(sohu.diyConsole.CurArea){
		if(target&&(target.ID==sohu.diyConsole.CurArea.ID)) 
			return this;
		
		sohu.diyConsole.CurArea.Deactive();
	};
	//���ǰ�ĺ���
	sohu.diyConsole.CurArea=target;
	this.CurArea=target;
	return this;
};
/**
 * �ر�����ѡ��Ի���
 */
sohu.diyConsole.prototype.CloseCTDialog=function(){
	sohu.diyConsole.CurSec.Editor.CloseCTDialog();
};
/**
 * ��ȡ���к���jquery����
 */
sohu.diyConsole.prototype.AreaList=function(){
	var _this=this;
	var items= this.$Workspace.find(this.__p.opts.cssArea);
	//�޳�channelNav��indexNav�Ⱥ���static��ĺ���
	items=$.grep(items,function(o,i){
		if($(o).hasClass(_this.__p.opts.clAreaStatic)) return false;
		return true;
	});
	
	return $(items);
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
/*��̬����������*/
sohu.diyConsole.Dragger={
	ing:false,
	obj:null,
	handle:$("#ctHandle"),
	cssHandle:'.dragHandle'
};
sohu.diyConsole.CurArea=null;
sohu.diyConsole.CurSec=null;		/* ��ǰ������ڵķ��� */
sohu.diyConsole.EditingSec=null;	/* ��ǰ�����༭�ķ��� */
sohu.diyConsole.CurCT=null;
sohu.diyConsole.EditingCT=null;
sohu.diyConsole.CurElm=null;/* current editing element */
sohu.diyConsole.$SecEditorModel=null; /* �����༭��domģ�� */
sohu.diyConsole.DocSelection='';
sohu.diyConsole.InnerHeight=function() {
    var x,y;
    if (self.innerHeight) // all except Explorer
    {
		return self.innerHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight)         
    {
		// Explorer 6 Strict Mode
        return document.documentElement.clientHeight;
    }
    else if (document.body) // other Explorers
    {
        return document.body.clientHeight;
    };

};
/**
 * ������ɫֵ��ȡ�������ϡ��ҡ��¡����ĸ��������ɫֵ
 * @param {Object} c
 */
sohu.diyConsole.GetBorderColor=function(c){
	c=$.trim(c);
	if(c=="") return null;
	//Ĭ���������jquery��css������ȡ��rgb��ɫֵrgb(9, 168, 139)�к��пո����Ƚ���Щ�ո�ȥ��
	var reg=/\b,\s\b/g;
	c=c.replace(reg,",");//��", "�滻Ϊ","
	var cList=c.split(" "),retVal={};
	$.each(cList,function(i,o){
		if(o.indexOf("#")!=0&&o.indexOf("rgb")!=0)
			cList[i]="none";
			
	});
	switch(cList.length){
		case 0:
			retVal= null;
		break;
		case 1:
			retVal.top=retVal.right=retVal.bottom=retVal.left=cList[0];
		break;
		case 2:
			retVal.top=retVal.bottom=cList[0];
			retVal.left=retVal.right=cList[1];
		break;
		case 3:
			retVal.top=cList[0];
			retVal.left=retVal.right=cList[1];
			retVal.bottom=cList[2];	
		break;
		case 4:
			retVal.top=cList[0];
			retVal.right=cList[1];
			retVal.bottom=cList[2];
			retVal.left=cList[3];
		break;
		default:
			retVal=null;
		break;
	};//switch
	return retVal;
};
/**
 * ��ȡָ��jq dom����ĵ�idx��css class
 * @param {Object} $dom
 * @param {Object} idx
 */
sohu.diyConsole.GetClassName=function($dom,idx){
	var cl=$.trim($dom.attr("class"));
	if(cl=="") return "";
	
	cl=cl.split(" ");
	cl=$.grep(cl,function(o,i){
		if(o=="") return false;
		return true;
	});
	idx=idx||0;
	if(idx<0) idx=0;
	if(idx>=cl.length) idx-=1;
	return cl[idx];
};
/**
 * ����ַ����Ƿ����ID������ĸ�����֡��»���
 * @param {Object} str
 */
sohu.diyConsole.IsValidID=function(str){
	if(!StringUtils.isAlphanumeric(str.replace("_",""))) return false;
	return true;
};
/**
 * Ԥ��-�˳��༭״̬
 */
sohu.diyConsole.Preview=function(){
	if(!sohu.diyConsole.CurArea) return;
	/*
	if(!sohu.diyConsole.CurArea.IsActive) return;
	if(sohu.diyConsole.CurArea.IsEditing) return;
	*/
	if(sohu.diyConsole.Dragger.ing) return;
	//�Ƴ������༭��
	if(sohu.diyConsole.CurSec)
		sohu.diyConsole.CurSec.Editor.Editing("off");
	//ǿ���Ƴ������༭��
	if(sohu.diyConsole.EditingSec!=null&&sohu.diyConsole.CurElm!=null){
		sohu.diyConsole.CurElm.HideEditor(false);
	};
	//���������
	sohu.diyConsole.CurArea.Deactive();
	//���������
	if(sohu.diyConsole.CurSec)
		sohu.diyConsole.CurSec.Deactive();
};
/**
 * Ԫ��ѡ�����
 */
sohu.diyConsole.SnapSelection=function(){
	if(sohu.diyConsole.CurElm.InlineEditable){
		sohu.diySelection.snap(sohu.diyConsole.CurElm.i$frame[0].iDoc());
	}else{
		sohu.diySelection.snap(document,sohu.diyConsole.CurElm.$Layout[0]);
	};
};
/**
 * ��������ͼ��ַ
 * @param {Object} img
 */
sohu.diyConsole.ParseBGImg=function(img){
	img=img=="none"?"":img;
	img=img.replace('url("',"").replace('")',"");
	return img;
};
