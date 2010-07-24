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
		limitSec:390,
		scrollWrapMainginTop:0
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");

	sohu.diyConsole.$SecEditorModel=$("#area_editor");
	this.Areas=null;
	
	var p={opts:opts};
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	p._$areaSelector=$("#area_selector");
	p._$pageTip=$("#pageTip");
	p._$editMenu=$("#editMenu");
	p._$popWins=p._$editMenu.find(".win");
	p._$cpkWrap=$("#cpkWrap");
	p._$addLink=$("#addLink");
	p._fmAddLink={
		txtATitle:$("#txtATitle"),
		txtAHref:$("#txtAHref"),
		txtATarget:$("input[name='atarget']"),
		tipAHref:p._$addLink.find(".tipAHref"),
		reset:function(){p._$addLink.find(".tip").removeClass("alert").end().find("input[type='text']").val("");}
	};
	p._$txtFontColor=$("#txtFontColor");
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
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		_this.Areas=$.grep(_this.Areas,function(o,i){
			if(o.ID==_this.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
		return false;	
	};
	p.onAddBG=function(evt){
	///<summary>��Ӻ��б���</summary>	
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		alert("����");
		return false;
	};
	p.onMove=function(evt){
	///<summary>�ƶ�����</summary>
		if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		_this.RePosition();
		return false;	
	};
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
		};
	};
	p.onBodyClick=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		//�û��Ƿ���#editMenu
		if($(evt.target).parents("#editMenu").length>0) return;
		
		var b=p.getWorkspaceBoundary();
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//ǿ���Ƴ������༭��
			if(sohu.diyConsole.EditingSec!=null){
				sohu.diyConsole.CurElm.HideEditor(false);
			};
			//���������
			sohu.diyConsole.CurArea.Deactive();
			//���������
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
			//����������ӶԻ����
			p._fmAddLink.reset();
		};
		return false;
	};
	p.setDocumentDim=function(){
		var fullheight, height;
		fullheight = sohu.diyConsole.InnerHeight();        
		height = fullheight - p.opts.scrollWrapMainginTop;
		
		$("#scrollWrap").css("height",height);
		$("#main").css("minHeight",height);
	};
	p.onLoaded=function(){
		//�ĵ��߶���Ӧ����
		p.setDocumentDim();
		//�༭������λ�ô���
		p._$editMenu.css("top",p.opts.scrollWrapMainginTop)
			.hide().find("#editMenuIn")
			.draggable({containment:'#mainWrap',scroll:false,handle:"#editMenuChild"});
		//���й�����λ��
		_this.$Layout.css({"top":p.opts.scrollWrapMainginTop+30});
	};
	p.initColorPicker=function(){
		var cbk=p._$cpkWrap.ColorPicker({
			flat:true,
			color:"#000000",
			onSubmit:function(hsb,hex,rgb){
				var c="#"+hex;
				p._$txtFontColor.val(c);
				p._$popWins.slideUp();	
				sohu.diyConsole.CurElm.i$frame[0].iDoCommand("foreColor",c,null,function($iframe){
					if(sohu.diyConsole.DocSelection.text){
						sohu.diyConsole.DocSelection.select();
						sohu.diyConsole.DocSelection='';
					}
				});
			},
			onChange:function(hsb,hex,rgb){
				//IE like a bullshit
				if($.browser.msie&&sohu.diyConsole.DocSelection.text){
					sohu.diyConsole.DocSelection.select();
				};
			}
		});
	};
	p.initAddLink=function(){
		p._$addLink.find("#btnAddLink").click(function(evt){
			var url=p._fmAddLink.txtAHref.val();
			if(!StringUtils.isUrl(url)){
				p._fmAddLink.txtAHref.css("color","red").select();
				p._fmAddLink.tipAHref.addClass("alert");
				return false;
			};
			p._$popWins.slideUp();
			sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
				if(sohu.diyConsole.DocSelection.text){
					sohu.diyConsole.DocSelection.select();
					sohu.diyConsole.DocSelection='';
				}
			});
			p._fmAddLink.reset();
		});
		p._$addLink.find("#btnCloseLink").click(function(evt){
			p._$popWins.slideUp();
		});
		p._$addLink.find("input[type='text']").keyup(function(evt){
			this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
		});
		p._fmAddLink.txtATarget.click(function(evt){
			p._fmAddLink.txtATarget.curVal=this.value;
		});
		p._fmAddLink.txtATarget.curVal="_blank";
	};
	/**
	 * Save current document.selection to sohu.diyConsole.DocSelection
	 */
	p.saveSelection=function(){
		if(document.selection)
				sohu.diyConsole.DocSelection=sohu.diyConsole.CurElm.i$frame[0].iDoc().selection.createRange();
	};
	p.initEditMenu=function(){
		var fontStyleCbk=function(iframeEditor){
			sohu.diyConsole.CurElm.$Layout.css(iframeEditor.iCurCss);
		};
		$("#editMenuChild .menuitem").click(function(evt){return false;});
		$("#menuitem-b").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Bold",null);});
		$("#menuitem-i").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Italic",null);});
		$("#menuitem-u").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Underline",null);});
		$("#menuitem-if").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("increasefontsize",null);});
		$("#menuitem-df").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("decreasefontsize",null);});
		$("#menuitem-al").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyleft",null,fontStyleCbk);});
		$("#menuitem-ac").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifycenter",null,fontStyleCbk);});
		$("#menuitem-ar").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyright",null,fontStyleCbk);});
		$("#menuitem-af").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyfull",null,fontStyleCbk);});
		$("#menuitem-ul").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertunorderedlist",null);});
		$("#menuitem-ol").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertorderedlist",null);});
		$("#menuitem-z").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("undo",null);});
		$("#menuitem-r").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("redo",null);});
		//colorpicker
		$("#menuitem-cc").mousedown(function(evt){
			p.saveSelection();
			p._$popWins.hide();	
			p._$cpkWrap.slideDown();
		});
		//create link
		$("#menuitem-cl").mousedown(function(evt){
			p.saveSelection();
			p._$popWins.hide();
			p._$addLink.slideDown();
			p._fmAddLink.txtATitle.focus();
		});
	};
	p.Init=function(){
		//������������
		sohu.diyConsole.$EditMenu=p._$editMenu;
		sohu.diyConsole.$CPKWrap=p._$cpkWrap;
		//����ѡ����
		$("li",p._$areaSelector).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");})
			.click(p.onSelectAreaTpl);
		
		//����ѡ����
		$('#hiddenTemplate .sec_selector li').click(function(evt){
			if(!sohu.diyConsole.CurSec) return;
			sohu.diyConsole.CurSec.Editor.SecSelector.Cur=this.id;
			//_this.SecSelector.dialog("close");
			sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
			return false;
		}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
		
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
		$("body").mousemove(p.onMousemove).click(p.onBodyClick);
		//window resize�¼�
		$(window).resize(function(evt){
			p.setDocumentDim();
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Editor.Reposition();
		});
		//color picker
		p.initColorPicker();
		//create link popup window
		p.initAddLink();
		//editMenu���¼�ע��
		p.initEditMenu();
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
	if(!this.CurArea){this.$Layout.attr("style","");return;};
	this.$Layout.css("top",this.CurArea.$Layout.offset().top);
};
/**
 * �趨����ĺ��ж���
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//����һ�����з�����
	if(sohu.diyConsole.CurArea){
		if(target.ID==sohu.diyConsole.CurArea.ID) 
			return this;
		
		sohu.diyConsole.CurArea.Deactive();
	};
	//���ǰ�ĺ���
	sohu.diyConsole.CurArea=target;
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
/*��̬����������*/
sohu.diyConsole.Dragger={
	ing:false,
	obj:null,
	handle:$('<div class="dragHandle"></div>'),
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
