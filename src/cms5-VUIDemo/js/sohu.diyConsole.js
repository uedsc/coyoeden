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
		cssWsp:"#vstp_main",clSec:"vstp_sec",clSec0:"vstp_sec0",clSecSub:"vstp_subsec",
		clSecRoot:"vstp_col",clArea:"area",cssArea:".area",dfTop:100,
		clAreaStatic:"vstp_static",
		limitSec:390,
		scrollWrapMainginTop:0
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#vstp_areaTools");
	

	this.Areas=null;
	
	var p={opts:opts};
	p._$pageTip=$("#vstp_pageTip");
	p._$elmTool=$("#vstp_elmTool");
	/* �Ի���jq���� */
	p._$wSec=$("#vstp_wCfgSec");
	p._$wCode=$("#vstp_wCode");
	/* /�Ի���jq���� */
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
		//if(sohu.diyConsole.Dragger.ing) return;
		//if(sohu.diyConsole.EditingSec!=null) return;
		
		var b=p.getWorkspaceBoundary();
		
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			/*
			//���������
			sohu.diyConsole.CurArea.Deactive();
			//���������
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
				
			//�Ƴ���ק���ֺ������ɲ�
			sohu.diyConsole.Dragger.handle.hide();	
			*/
		};
	};
	p.onBodyClick=function(evt){
		//�û��Ƿ���#editMenu
		if($(evt.target).parents(".jqmWindow").length>0) return;
		
		var b=p.getWorkspaceBoundary();
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//sohu.diyDialog.Hide(true);
			//sohu.diyConsole.Preview();
			//���������
			if(sohu.diyConsole.CurArea)
				sohu.diyConsole.CurArea.Deactive();
			//���������
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
				
			//�Ƴ���ק���ֺ������ɲ�
			sohu.diyConsole.Dragger.handle.hide();				
		};
		return false;
	};
	p.setDocumentDim=function(){
		return;
		var fullheight, height;
		fullheight = sohu.diyConsole.InnerHeight();        
		height = fullheight - p.opts.scrollWrapMainginTop;
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
	p.Init=function(){
		//������������
		sohu.diyConsole.URL_TopicTpl=opts.urlTopicTpl||'static/';
		sohu.diyConsole.$WinSec=p._$wSec;
		sohu.diyConsole.$WinCode=p._$wCode;
		//sohu.diyConsole.$WinPageBG=p._$wPageBG;
		sohu.diyConsole.$SecEditorModel=$("#vstp_area_editor");		
		sohu.diyConsole.$BodyBGA=$("#vstp_main .bodyBGA");
		sohu.diyConsole.$BodyBGB=$("#vstp_main .bodyBGB");	
		sohu.diyConsole.SecEditor=new sohu.diyEditor({bos:_this});	
		sohu.diyConsole.$AreaHolder=$("#vstp_areaHolder");
		sohu.diyConsole.$FlashHolder=$("#vstp_flashHolder").mouseleave(function(evt){$(this).hide();});
		sohu.diyConsole.$CTHelper=$("#vstp_ctHelper");
		sohu.diyConsole.IsPreview=false;
		sohu.diyConsole.$Workspace=_this.$Workspace;
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
		sohu.diyDialog.Init({console:_this,cssDragCTM:'document',onInit:sohu.diyDialog.onInit});
		//��Ƭ�༭���
		sohu.diyChipEditor.Init({singleton:true});
		//on page loaded
		$(document).ready(p.onLoaded);
		//�Զ����¼�
		$(_this).bind("evtPreview",function(e){});
	};
	this.__p=p;
	//Init
	p.Init();
	this.Fire();
};
/**
 * ���༭����ע�������ĵ�
 */
sohu.diyConsole.prototype.Fire=function(){
	var _this=this;
	//�������еĽ���ͼflash
	this.loadFlash();
	//���к���
	this.Areas=this.AreaList().map(function(i,o){
		var a=new sohu.diyArea({
			isNew:false,
			console:_this,
			onRemove:null,
			obj:$(o)
		});
		return a;
	});
};
/**
 * ��������flash
 */
sohu.diyConsole.prototype.loadFlash=function(){
	$(".vstp_flashWrap").each(function(i,o){
		var $o=$(o);
		var d=$.evalJSON($o.next().html());
		if(d.dummy) return;
		var f=new sohuFlash(d.swf,d.id,d.w,d.h,d.interval);
		f.addParam("quality", "high");
		f.addParam("wmode", "opaque");
		for(var n in d.v){
			f.addVariable(n,d.v[n]);
		};
		f.write(d.pid);
		f.data=d;
		window['F_'+d.pid]=f;
	});
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
	$(".vstp_txtLoading").toggle();
};
/*��̬����������*/
sohu.diyConsole.Dragger={
	obj:null,
	handle:$("#vstp_ctHandle"),
	cssHandle:'.vstp_dragHandle'
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
sohu.diyConsole.Preview=function(flag){
	if(flag&&bos&&bos.Areas&&bos.Areas.length>0){
		$(bos.Areas).each(function(i,o){
			o.UnbindEvts();
		});
		sohu.diyConsole.IsPreview=true;
		//���غ��й�����
		bos.$Layout.hide();
		//�����Զ����¼�evtPreview�Ա�֪ͨ������
		$(bos).trigger("evtPreview");
	};
	
	if(!sohu.diyConsole.CurArea) return;
	/*
	if(!sohu.diyConsole.CurArea.IsActive) return;
	if(sohu.diyConsole.CurArea.IsEditing) return;
	*/
	//if(sohu.diyConsole.Dragger.ing) return;
	//�Ƴ������༭��
	if(sohu.diyConsole.CurSec)
		sohu.diyConsole.CurSec.Editor.Editing("off");

	//���������
	sohu.diyConsole.CurArea.Deactive();
	//���������
	if(sohu.diyConsole.CurSec)
		sohu.diyConsole.CurSec.Deactive();
	
};
/**
 * ���ر༭״̬
 */
sohu.diyConsole.UnPreview=function(){
	if(bos&&bos.Areas&&bos.Areas.length>0){
		$(bos.Areas).each(function(i,o){
			o.BindEvts();
		});
		//��ʾ���й�����
		bos.$Layout.show();	
		sohu.diyConsole.IsPreview=false;	
	};	
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
/**
 * Stop the default navigation behavior of the A tag.
 * @param {Object} evt
 */
sohu.diyConsole.OnStopNav=function(evt){
	//alert("onStopNav");
	evt.preventDefault();
	return true;
};
/**
 * ��ָ������ק������window.scroll�¼�,������ק������֮����޷�����������
 * @param {Object} $t ��ק�����jquery dom.
 */
sohu.diyConsole.FixDraggable=function($t){
	//����window��scroll�¼�
	$(window).scroll(function(){
		var of;
		if((of=$t.offset()).top<0)
			$t.css("top",0);
		
		if(of.top>(of.t1=$(document).height()))
			$t.css("top",$(this).height()-$t.height());
			
	});
};
