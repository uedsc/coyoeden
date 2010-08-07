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
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnPageBG=$("#lnkPageBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	p._$pageTip=$("#pageTip");
	p._$elmTool=$("#elmTool");
	/* =���������˵�= */
	p._$editMenu=$("#editMenu");
	p._$menus=$("#editMenuChild .imenu");
	p._$menuTxt=p._$editMenu.find(".mtxt");
	p._$menuImg=p._$editMenu.find(".mimg");
	p._$menuSecHead=p._$editMenu.find(".msecHead");
	p._$menuElmc=p._$editMenu.find(".melmc");
	p._$popWins=p._$editMenu.find(".win");
	/* �Ի���jq���� */
	p._$wAreaBG=$("#wAreaBG")
	p._$wPageBG=$("#wPageBG");
	p._$wCpkWrap=$("#cpkWrap");
	p._$wAddLink=$("#addLink");
	p._$wImgPro=$("#imgPro");
	p._$wImgSwitch=$("#imgSwitch");
	p._$wSecHead=$("#cfgSecHead");
	p._$wSec=$("#wCfgSec");
	p._$wCode=$("#wCode");
	/* /�Ի���jq���� */
	
	/* �Ի����Ԫ�� */
	p._fmAddLink={
		txtATitle:$("#txtATitle"),
		txtAHref:$("#txtAHref"),
		txtATarget:$("input[name='atarget']"),
		tipAHref:p._$wAddLink.find(".tipAHref"),
		reset:function(){p._$wAddLink.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	p._fmImgPro={
		txtImgH:$("#txtImgH"),
		txtImgW:$("#txtImgW"),
		ddlImgBStyle:$("#ddlImgBStyle"),
		txtImgBColor:$('#txtImgBColor'),
		txtImgPadding:$("#txtImgPadding"),
		cpk:p._$wImgPro.find(".cpk"),
		reset:function(){
			p._$wImgPro.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");
		}
	};
	p._fmImgSwitch={
		txtImgAlt:$("#txtImgAlt"),
		txtImgSrc:$("#txtImgSrc"),
		tipImgSrc:p._$wImgSwitch.find(".tipImgSrc"),
		btnOK:p._$wImgSwitch.find(".btnMWinOK"),
		btnNO:p._$wImgSwitch.find(".btnMWinNO"),
		reset:function(){p._$wImgSwitch.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	p._fmAreaBG={
		txtAreaBG:$("#txtAreaBG"),
		txtAreaID:$("#txtAreaID"),
		rbtnAreaBGAlign:p._$wAreaBG.find("input[name='areaBGAlign']"),
		rbtnAreaBGRepeat:p._$wAreaBG.find("input[name='areaBGRepeat']"),
		tipAreaBG:p._$wAreaBG.find(".tipAreaBG"),
		tipAreaID:p._$wAreaBG.find("tipAreaID"),
		reset:function(){p._$wAreaBG.find("*").removeClass("alert").end().find(":text").val("");}
	};
	p._fmPageBG={
		txtPageBG:$("#txtPageBG"),
		txtPageBGH:$("#txtPageBGH"),
		rbtnPageBGAlign:p._$wPageBG.find("input[name='pageBGAlign']"),
		rbtnPageBGRepeat:p._$wPageBG.find("input[name='pageBGRepeat']"),
		tipPageBG:p._$wPageBG.find(".tipPageBG"),
		tipPageBGH:p._$wPageBG.find(".tipPageBGH"),
		txtPageBGC:$("#txtPageBGC"),
		cpk:p._$wPageBG.find(".cpk"),
		reset:function(){p._$wPageBG.find("*").removeClass("alert").end().find(":text").val("");}
	};
	p._fmSecHead={
		txtBG:$("#txtSecHeadBGC"),
		txtBGImg:$("#txtSecHeadImg"),
		tipBGImg:p._$wSecHead.find(".tipSecHeadImg"),
		btnOK:p._$wSecHead.find(".btnMWinOK"),
		btnNO:p._$wSecHead.find(".btnMWinNO"),
		btnDel:p._$wSecHead.find(".btnMWinDel"),
		cpk:p._$wSecHead.find(".cpk"),
		reset:function(){p._$wSecHead.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	p._fmSec={
		txtBG:$("#txtSecBG"),
		tipBG:p._$wSec.find(".tipSecBG"),
		txtBGC:$("#txtSecBGC"),
		txtBorderC:$("#txtSecBorderColor"),
		btnOK:p._$wSec.find(".btnWinOK"),
		btnNO:p._$wSec.find(".btnWinNO"),
		cpk:p._$wSec.find(".cpk"),
		rbtnSecBGAlign:p._$wSec.find("input[name='secBGAlign']"),
		rbtnSecBGRepeat:p._$wSec.find("input[name='secBGRepeat']"),
		cbxSecBorder:p._$wSec.find("input[name='SecBorderDir']"),
		reset:function(){p._$wSecHead.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	/* /�Ի����Ԫ�� */
	p._$txtFontColor=$("#txtFontColor");
	p._opts=opts;
	/* =/���������˵�= */
	
	//����ɾ��ʱ�Ļص�����
	p.onAreaRemove=function(area){
		_this.ActiveArea(null);
		_this.$Layout.animate({top:opts.dfTop});	
	};
	p.onAdd=function(evt){
	///<summary>��Ӻ���</summary>
		sohu.diyDialog.Show("addBlock");	
		return false;
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
		p._$wAreaBG.dialog("open");
		return false;
	};
	p.onPageBG=function(evt){
	///<summary>����ҳ�汳��</summary>	
		p._$wPageBG.dialog("open");
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
			if(sohu.diyConsole.EditingSec!=null&&sohu.diyConsole.CurElm!=null){
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
		
		sohu.diyConsole.$ScrollWrap.css("height",height);
		_this.$Workspace.css("minHeight",height);
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
		//������ɫʰɫ��
		var cbk=p._$wCpkWrap.ColorPicker({
			flat:true,
			color:"#000000",
			onSubmit:function(hsb,hex,rgb){
				var c="#"+hex;
				p._$txtFontColor.val(c);
				p._$popWins.slideUp();	
				sohu.diyConsole.CurElm.i$frame[0].iDoCommand("foreColor",c,null,function($iframe){
					sohu.diyConsole.DocSelection.selectAndRelease();
				});
			},
			onChange:function(hsb,hex,rgb){
				sohu.diyConsole.DocSelection.select();
			}
		});
		//ͼƬ�߿�ʰɫ��
		p._fmImgPro.cpk.ColorPicker({
			flat:true,
			color:"#cccccc",
			onChange:function(hsb,hex,rgb){
				p._fmImgPro.txtImgBColor.css("backgroundColor","#"+hex).val("#"+hex);
			},
			onSubmit:function(hsb,hex,rgb){
				p._fmImgPro.cpk.hide();
			}
		});
		//��Ŀͷ����ɫ
		p._fmSecHead.cpk.ColorPicker({
			flat:true,
			color:"#cccccc",
			onChange:function(hsb,hex,rgb){
				p._fmSecHead.txtBG.css("backgroundColor","#"+hex).val("#"+hex);
				sohu.diyConsole.CurElm.$Layout.parent().css("backgroundColor","#"+hex).val("#"+hex);
			},
			onSubmit:function(hsb,hex,rgb){
				p._fmSecHead.cpk.hide();
			}
		});
		//��Ŀ����ɫ
		p._fmSec.cpk.ColorPicker({
			flat:true,
			color:"#eeeeee",
			onChange:function(hsb,hex,rgb){
				var c="#"+hex;
				p._fmSec.cpk.$t.css("backgroundColor",c).attr("title",c);
				if(p._fmSec.cpk.flag=="bg"){
					sohu.diyConsole.CurSec.$Layout.css("backgroundColor",c);
				}else{
					sohu.diyConsole.CurSec.$Layout.css("borderColor",c);
				};
			},
			onSubmit:function(hsb,hex,rgb){
				p._fmSec.cpk.hide();
			}
		});
		//ҳ�汳��ɫ
		p._fmPageBG.cpk.ColorPicker({
			flat:true,
			color:"#ffffff",
			onChange:function(hsb,hex,rgb){
				var c="#"+hex;
				p._fmPageBG.txtPageBGC.css("backgroundColor",c).attr("title",c);
				$("body").css("backgroundColor",c);
			},
			onSubmit:function(hsb,hex,rgb){
				$("body").css("backgroundColor","#"+hex);
				p._fmPageBG.cpk.hide();
			}
		});
	};
	p.initAddLink=function(){
		//���ӶԻ����ȷ�ϰ�ť�߼�
		p._$wAddLink.find("#btnAddLink").click(function(evt){
			var url=p._fmAddLink.txtAHref.val();
			if(!StringUtils.isUrl(url)){
				p._fmAddLink.txtAHref.addClass("alert").select();
				p._fmAddLink.tipAHref.addClass("alert");
				return false;
			};
			p._$popWins.slideUp();
			if(p._fmAddLink.isNew){
				sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
					sohu.diyConsole.DocSelection.selectAndRelease();
				});
			}else{
				p._fmAddLink.a.$obj.attr("title",p._fmAddLink.txtATitle.val())
					.attr("href",p._fmAddLink.txtAHref.val())
					.attr("target",p._fmAddLink.txtATarget.curVal);
			}

			p._fmAddLink.reset();
		});
		//�رհ�ť
		p._$wAddLink.find("#btnCloseLink").click(function(evt){
			p._$popWins.slideUp();
		});
		p._$wAddLink.find("input[type='text']").keyup(function(evt){
			this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
		});
		//��λ�õ�ѡ��
		p._fmAddLink.txtATarget.click(function(evt){
			p._fmAddLink.txtATarget.curVal=this.value;
		});
		p._fmAddLink.txtATarget.curVal="_blank";
	};
	p.initImgPro=function(){
		//ͼƬ���ԶԻ����ȷ�ϰ�ť�߼�
		p._$wImgPro.find("#btnAddLink1").click(function(evt){
			var isOk=true,i=[null,null,null];
			$.each([p._fmImgPro.txtImgH,p._fmImgPro.txtImgW,p._fmImgPro.txtImgPadding],function(j,o){
				var _v=o.val();
				i[j]=(_v==""?"auto":_v);
				if((_v!="")&&(!StringUtils.isPlusInt(_v))){
					$(o).addClass("alert");
					isOk=isOk&&false;
				}
			});
			if(!isOk) return;
			p._$popWins.slideUp();

			i[2]=i[2]=="auto"?"0":i[2];
			var $img=sohu.diyConsole.CurElm.$Layout;
			$img.attr("width",i[1]).attr("height",i[0]).css("padding",i[2]+'px');
			if(p._fmImgPro.ddlImgBStyle.val()!="none"){
				var b="1px "+p._fmImgPro.ddlImgBStyle.val()+" "+p._fmImgPro.txtImgBColor.val();
				$img.css("border",b);
			}else{
				$img.css("border","none");
			}

			p._fmImgPro.reset();
		});
		//�رհ�ť
		p._$wImgPro.find("#btnCloseLink1").click(function(evt){
			p._$popWins.slideUp();
		});
		p._$wImgPro.find("input[type='text']").keyup(function(evt){
			this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
		});
		//�߿���ʽ
		p._fmImgPro.ddlImgBStyle.change(function(evt){
			p._fmImgPro.ddlImgBStyle.curVal=this.value;
		});
		p._fmImgPro.ddlImgBStyle.curVal="none";
		//�߿���ɫʰɫ��

		p._fmImgPro.txtImgBColor
			.attr("readonly",true)
			.css("backgroundColor","#cccccc")
			.click(function(evt){
				p._fmImgPro.cpk.show();
			});
	};
	p.initImgSwitch=function(){
		//��ť��Ϊ
		p._fmImgSwitch.btnOK.click(function(evt){
			var url=p._fmImgSwitch.txtImgSrc.val();
			if(!StringUtils.isUrl(url)){
				p._fmImgSwitch.txtImgSrc.addClass("alert").select();
				p._fmImgSwitch.tipImgSrc.addClass("alert");
				return false;
			};
			//����ͼƬ����
			sohu.diyConsole.CurElm.$Layout.attr("src",url).attr("alt",p._fmImgSwitch.txtImgAlt.val());
			//����Ի���
			p._$popWins.slideUp();
			//���ñ�
			p._fmImgSwitch.reset();	
		});
		p._fmImgSwitch.btnNO.click(function(evt){p._$popWins.slideUp();});
		
	};	
	p.initAreaBG=function(){
		//�Ի���ע��
		//�Ի�����ػص�������
		var _onOK=function(evt,cls){
			cls=cls||true;
			//bg image
			var url=p._fmAreaBG.txtAreaBG.val();
			if((url!="")&&(!StringUtils.isUrl(url))){
				p._fmAreaBG.txtAreaBG.addClass("alert").select();
				p._fmAreaBG.tipAreaBG.addClass("alert");
				return false;
			};
			if(url==""){
				//sohu.diyConsole.CurArea.$Layout.css("background-image","none");
			}else{
				sohu.diyConsole.CurArea.$Layout.css("background-image","url('"+url+"')");
				//bg position
				var al=p._fmAreaBG.rbtnAreaBGAlign.curVal;
				al=al=="center"?al:al+" top";
				sohu.diyConsole.CurArea.$Layout.css("background-position",al);
				//bg repeat
				var rp=p._fmAreaBG.rbtnAreaBGRepeat.curVal;
				sohu.diyConsole.CurArea.$Layout.css("background-repeat",rp);
			};
			//area id
			var id=p._fmAreaBG.txtAreaID.val();
			if(!sohu.diyConsole.IsValidID(id)){
				p._fmAreaBG.txtAreaID.addClass("alert");
				p._fmAreaBG.tipAreaID.addClass("alert");
				return false;
			};
			var isIDOK=sohu.diyConsole.CurArea.UpdateID(id);
			
			if(cls&&isIDOK)
				$(this).dialog("close");
		};
		var _onNO=function(evt){
			$(this).dialog("close");
		};
		//��ʾ�Ի���
		p._$wAreaBG=p._$wAreaBG.dialog(
		{
			title:"���б�������",
			resizable:false,
			modal:true,
			width:430,
			autoOpen:false,
			position:[700,50],
			buttons:{
				"�ر�":_onNO,
				"ȷ��":_onOK,
			},
			open:function(evt,ui){
				//bg img
				var img=sohu.diyConsole.CurArea.$Layout.css("background-image");
				img=img.replace('url("',"").replace('")',"");
				img=img=="none"?"":img;
				p._fmAreaBG.txtAreaBG.val(img).select();
				//area id
				p._fmAreaBG.txtAreaID.val(sohu.diyConsole.CurArea.ID);
			},
			close:function(evt,ui){p._fmAreaBG.reset();}
		}
		);
		p._$wAreaBG.preview=function(){
			_onOK(null,false);
		};
		//�¼�ע��
		p._fmAreaBG.rbtnAreaBGAlign.click(function(evt){
			p._fmAreaBG.rbtnAreaBGAlign.curVal=this.value;
			p._$wAreaBG.preview();
		});
		p._fmAreaBG.rbtnAreaBGRepeat.click(function(evt){
			p._fmAreaBG.rbtnAreaBGRepeat.curVal=this.value;
			p._$wAreaBG.preview();
		});		
		p._fmAreaBG.rbtnAreaBGAlign.curVal="center";
		p._fmAreaBG.rbtnAreaBGRepeat.curVal="no-repeat";
	};
	p.initPageBG=function(){
		//�Ի�����ػص�������
		var _onOK=function(evt,cls){
			cls=cls||true;
			var url=p._fmPageBG.txtPageBG.val();
			if((url!="")&&(!StringUtils.isUrl(url))){
				p._fmPageBG.txtPageBG.addClass("alert").select();
				p._fmPageBG.tipPageBG.addClass("alert");
				return false;
			};
			var h=p._fmPageBG.txtPageBGH.val();
			if((!StringUtils.isPlusInt(h))||(h=parseInt(h))<1){
				p._fmPageBG.txtPageBGH.addClass("alert").select();
				p._fmPageBG.tipPageBGH.addClass("alert");
				return false;
			};
			
			if(url==""){
				sohu.diyConsole.$BodyBGA.css("background-image","none");
			}else{
				sohu.diyConsole.$BodyBGA.css("background-image","url('"+url+"')");
			};
			var al=p._fmPageBG.rbtnPageBGAlign.curVal;
			sohu.diyConsole.$BodyBGA.css("background-position",al);
			
			var rp=p._fmPageBG.rbtnPageBGRepeat.curVal;
			sohu.diyConsole.$BodyBGA.css("background-repeat",rp);
			
			sohu.diyConsole.$BodyBGA.css("height",h);
			
			if(cls)
				$(this).dialog("close");
		};
		var _onNO=function(evt){
			$(this).dialog("close");
		};
		//��ʾ�Ի���
		p._$wPageBG=p._$wPageBG.dialog(
		{
			title:"ҳ�汳������",
			resizable:false,
			modal:true,
			width:430,
			position:[700,50],
			autoOpen:false,
			buttons:{
				"�ر�":_onNO,
				"ȷ��":_onOK,
			},
			open:function(evt,ui){
				//����ͼ
				var img=sohu.diyConsole.$BodyBGA.css("background-image");
				img=img=="none"?"":img;
				img=img.replace('url("',"").replace('")',"");
				p._fmPageBG.txtPageBG.val(img).select();
				//�߶�
				var h=sohu.diyConsole.$BodyBGA.css("height");
				h=parseInt(h);
				p._fmPageBG.txtPageBGH.val(h);
				//���뷽ʽ
				var bg_p=sohu.diyConsole.$BodyBGA.css("backgroundPosition");
				bg_p=bg_p=="0% 0%"?"center center":bg_p;
				var bg_a=sohu.diyConsole.$BodyBGA.css("backgroundRepeat");
				p._fmPageBG.rbtnPageBGAlign.filter("[value='"+bg_p+"']").trigger("click");
				//ƽ�̷�ʽ
				p._fmPageBG.rbtnPageBGRepeat.filter("[value='"+bg_a+"']").trigger("click");
				//����ɫ
				p._fmPageBG.txtPageBGC.css("backgroundColor",$("body").css("backgroundColor"));
			},
			close:function(evt,ui){p._fmPageBG.reset();}
		}
		);
		p._$wPageBG.preview=function(){
			_onOK(null,false);
		};
		//�¼�ע��
		p._fmPageBG.rbtnPageBGAlign.click(function(evt){
			p._fmPageBG.rbtnPageBGAlign.curVal=this.value;
			p._$wPageBG.preview();
		});
		p._fmPageBG.rbtnPageBGRepeat.click(function(evt){
			p._fmPageBG.rbtnPageBGRepeat.curVal=this.value;
			p._$wPageBG.preview();
		});		
		p._fmPageBG.rbtnPageBGAlign.curVal="center center";
		p._fmPageBG.rbtnPageBGRepeat.curVal="no-repeat";
		p._fmPageBG.txtPageBGH.change(function(evt){
			p._$wPageBG.preview();
		});
		p._fmPageBG.txtPageBG.change(function(evt){p._$wPageBG.preview();});
		
		//����ɫ
		p._fmPageBG.txtPageBGC
		.css("backgroundColor","transparent")
		.click(function(evt){
			p._fmPageBG.cpk.show();
		});
	};
	p.initSecHead=function(){
		//��ť��Ϊ
		p._fmSecHead.btnOK.click(function(evt){
			var url=$.trim(p._fmSecHead.txtBGImg.val());
			if( (url!="") && (!StringUtils.isUrl(url)) ){
				p._fmSecHead.txtBGImg.addClass("alert").select();
				p._fmSecHead.tipBGImg.addClass("alert");
				return false;
			};
			//����ͼƬ����
			if(url!="")
				sohu.diyConsole.CurElm.$Layout.parent().css("background-image","url('"+url+"')");
			//����Ի���
			p._$popWins.slideUp();
			//���ñ�
			p._fmSecHead.reset();	
		});
		p._fmSecHead.btnNO.click(function(evt){p._$popWins.slideUp();});
		p._fmSecHead.btnDel.click(function(evt){
			if(!window.confirm("ȷ��ɾ������Ŀ����ô")) return;
			p._$popWins.slideUp();
			var ct=sohu.diyConsole.CurElm.CT;
			var sec=sohu.diyConsole.EditingSec;
			sohu.diyConsole.CurElm.HideEditor(false);
			ct.$Layout.remove();
			sec.RemoveCTByID(ct.ID);
			
		});
		//����ɫ
		p._fmSecHead.txtBG
		.attr("readonly",true)
		.css("backgroundColor","#cccccc")
		.click(function(evt){
			p._fmSecHead.cpk.show();
		});
		//����ͼ
		p._fmSecHead.txtBGImg.change(function(evt){
			var url=p._fmSecHead.txtBGImg.val();
			if(!StringUtils.isUrl(url)){
				p._fmSecHead.txtBGImg.addClass("alert").select();
				p._fmSecHead.tipBGImg.addClass("alert");
				return false;
			};
			//����ͼƬ����
			sohu.diyConsole.CurElm.$Layout.parent().css("background-image","url('"+url+"')");
		});
	};
	p.initSec=function(){
		//��ť��Ϊ
		p._fmSec.btnOK.click(function(evt){
			var url=$.trim(p._fmSec.txtBG.val());
			if( (url!="") && (!StringUtils.isUrl(url)) ){
				p._fmSec.txtBG.addClass("alert").select();
				p._fmSec.tipBG.addClass("alert");
				return false;
			};
			//����ͼƬ����
			if(url!="")
				sohu.diyConsole.CurSec.$Layout.css("background-image","url('"+url+"')");
			
			
			//����Ի���
			p._$wSec.dialog("close");
			//���ñ�
			p._fmSec.reset();	
		});
		p._fmSec.btnNO.click(function(evt){p._$wSec.dialog("close");});
		//����ɫ
		p._fmSec.txtBGC
		.css("backgroundColor","transparent")
		.click(function(evt){
			p._fmSec.cpk.$t=p._fmSec.txtBGC;
			p._fmSec.cpk.flag="bg"
			p._fmSec.cpk.show();
		});
		//�߿�ɫ
		p._fmSec.txtBorderC
		.css("borderColor","#eeeeee")
		.click(function(evt){
			p._fmSec.cpk.$t=p._fmSec.txtBorderC;
			p._fmSec.cpk.flag="bdc"
			p._fmSec.cpk.show();
		});
		//�߿���
		p._fmSec.cbxSecBorder.click(function(evt){
			if(this.checked){
				sohu.diyConsole.CurSec.$Layout.css("border-"+this.value,"1px solid "+p._fmSec.txtBorderC.attr("title"));
			}else{
				sohu.diyConsole.CurSec.$Layout.css("border-"+this.value,"1px solid transparent");
			};
		});		
		//����ͼ
		p._fmSec.txtBG.change(function(evt){
			var url=p._fmSec.txtBG.val();
			if(!StringUtils.isUrl(url)){
				p._fmSec.txtBG.addClass("alert").select();
				p._fmSec.tipBG.addClass("alert");
				return false;
			};
			p._fmSec.txtBG.removeClass("alert");
			p._fmSec.tipBG.removeClass("alert");
			//����ͼƬ����
			sohu.diyConsole.CurSec.$Layout.css("background-image","url('"+url+"')");
		});
		//����ͼ���뷽ʽ
		p._fmSec.rbtnSecBGAlign.click(function(evt){
			p._fmSec.rbtnSecBGAlign.curVal=this.value;
			sohu.diyConsole.CurSec.$Layout.css("background-position",this.value);
		});
		//����ͼƽ�̷�ʽ
		p._fmSec.rbtnSecBGRepeat.click(function(evt){
			p._fmSec.rbtnSecBGRepeat.curVal=this.value;
			sohu.diyConsole.CurSec.$Layout.css("background-repeat",this.value);
		});		
		p._fmSec.rbtnSecBGAlign.curVal="center";
		p._fmSec.rbtnSecBGRepeat.curVal="repeat";
		
		//��ʼ���������öԻ���
		var _onOpen=function(){
			//����ͼ
			var bgimg=sohu.diyConsole.CurSec.$Layout.css("background-image");
			bgimg=bgimg=="none"?"":bgimg;
			bgimg=bgimg.replace('url("',"").replace('")',"");
			p._fmSec.txtBG.val(bgimg);
			//����ɫ
			p._fmSec.txtBGC.css("backgroundColor",sohu.diyConsole.CurSec.$Layout.css("backgroundColor"));
			//�߿�ɫ
			var bdc=sohu.diyConsole.CurSec.$Layout.css("borderColor");
			bdc=bdc==""?"#eeeeee":bdc;
			p._fmSec.txtBorderC.css("backgroundColor",bdc);
			//���뷽ʽ
			var bg_p=sohu.diyConsole.CurSec.$Layout.css("backgroundPosition");
			bg_p=bg_p=="0% 0%"?"center center":bg_p;
			var bg_a=sohu.diyConsole.CurSec.$Layout.css("backgroundRepeat");
			p._fmSec.rbtnSecBGAlign.filter("[value='"+bg_p+"']").trigger("click");
			//ƽ�̷�ʽ
			p._fmSec.rbtnSecBGRepeat.filter("[value='"+bg_a+"']").trigger("click");
			//�߿�-���ݱ߿�ɫ�жϱ߿������
			bdc=sohu.diyConsole.GetBorderColor(bdc);
			if(!bdc){
				p._fmSec.cbxSecBorder.each(function(i,o){
					o.checked=false;
				});
			}else{
				for(var x in bdc){
					if(x)
						p._fmSec.cbxSecBorder.filter("[value='"+x+"']")[0].checked=(bdc[x].indexOf("transparent")<0);
				};//for
			};//if	
			
		};
		p._$wSec=p._$wSec.dialog({
			title:"��������",
			resizable:false,
			modal:true,
			width:540,
			open:_onOpen,
			autoOpen:false
		});
		
	};
	p.initCode=function(){
		var txtarea=p._$wCode.find("textarea");
		var btnOK=p._$wCode.find(".btnOK");
		var _onOpen=function(evt,ui){
			txtarea.val(sohu.diyConsole.CurSec.$Layout.html());
		};
		//�¼�ע��
		p._$wCode=p._$wCode.dialog({
			title:"HTML����-�༭",
			resizable:false,
			modal:true,
			width:540,
			open:_onOpen,
			autoOpen:false
		});
		btnOK.click(function(evt){
			//���·�����html����ͬʱ���¼��ظ÷���������
			if(!window.confirm("ȷ�����µ�ǰ������HTML����ô?")) return false;
			//���¼��ظ÷��������ݣ�����.html(x)��������ʱ��dom�Ѿ�����ԭ����dom
			sohu.diyConsole.CurSec.$Layout.html(txtarea.val());
			sohu.diyConsole.CurSec.LoadContents();
			p._$wCode.dialog("close");
		});
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
	p.initEditMenu=function(){
		var fontStyleCbk=function(iframeEditor){
			sohu.diyConsole.CurElm.$Layout.css(iframeEditor.iCurCss);
		};
		p._$menus.click(function(evt){
			p._$menus.removeClass("on");
			var _this=$(this);
			_this.addClass("on");
			if(!_this.is(".popwin"))
				p._$popWins.hide();	
				
			return false;
		});
		$("#imenu-b").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Bold",null);});
		$("#imenu-i").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Italic",null);});
		$("#imenu-u").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Underline",null);});
		$("#imenu-if").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("increasefontsize",null);});
		$("#imenu-df").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("decreasefontsize",null);});
		$("#imenu-al").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyleft",null,fontStyleCbk);});
		$("#imenu-ac").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifycenter",null,fontStyleCbk);});
		$("#imenu-ar").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyright",null,fontStyleCbk);});
		$("#imenu-af").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyfull",null,fontStyleCbk);});
		$("#imenu-ul").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertunorderedlist",null);});
		$("#imenu-ol").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertorderedlist",null);});
		$("#imenu-z").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("undo",null);});
		$("#imenu-r").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("redo",null);});
		//colorpicker
		$("#imenu-cc").mousedown(function(evt){
			p.saveSelection();
			p._$popWins.hide();	
			p._$wCpkWrap.slideDown();
		});
		//create link
		$("#imenu-cl").mousedown(function(evt){
			p.saveSelection();
			p._$popWins.hide();
			p._$wAddLink.slideDown();
			p._fmAddLink.txtATitle.focus();
			//��ǰѡ���Ƿ���a��ǩ
			var a=sohu.diyConsole.DocSelection.getA();
			/*
			if(sohu.diyConsole.CurElm.InlineEditable){
				a=
			}else{
				if(sohu.diyConsole.CurElm.tagName=="img"){
					var $a=sohu.diyConsole.CurElm.$Layout.parents("a");
					a={
						isNull:false,
						$obj:$a,
						title:$a.attr()
					};	
				};
			};
			*/
			if(a.isNull){
				p._fmAddLink.isNew=true;
				p.a=null;
				return;
			};
			p._fmAddLink.txtATitle.val(a.title);
			p._fmAddLink.txtAHref.val(a.href);
			p._fmAddLink.txtATarget.filter("input[value='"+a.target+"']").trigger("click");
			p._fmAddLink.isNew=false;
			p._fmAddLink.a=a;
		});
		//ͼƬ����
		$("#imenu-imgp").mousedown(function(evt){
			p._$popWins.hide();
			p._$wImgPro.slideDown();
			var $img=sohu.diyConsole.CurElm.$Layout;
			p._fmImgPro.txtImgH.val(parseInt($img.css("height")));
			p._fmImgPro.txtImgW.val(parseInt($img.css("width")));
			p._fmImgPro.ddlImgBStyle.val($img.css("borderStyle"));
			p._fmImgPro.txtImgBColor.val($img.css("borderColor"));
			p._fmImgPro.txtImgPadding.val(parseInt($img.css("padding"))||"0");
		});
		//����ͼƬ
		$("#imenu-img").mousedown(function(evt){
			p._$popWins.hide();
			p._$wImgSwitch.slideDown();
			p._fmImgSwitch.txtImgAlt.val(sohu.diyConsole.CurElm.$Layout.attr("alt"));
			p._fmImgSwitch.txtImgSrc.val("");
		});
		//��������
		$("#imenu-secHead").mousedown(function(evt){
			p._$popWins.hide();
			p._$wSecHead.slideDown();
			var $img=sohu.diyConsole.CurElm.$Layout;
			p._fmSecHead.txtBG.val($img.css("backgroundColor"));
			var img=$img.css("background-image");
			img=img.replace('url("',"").replace('")',"");
			img=img=="none"?"":img;
			p._fmSecHead.txtBGImg.val(img);
		});
	};
	p.Init=function(){
		//������������
		sohu.diyConsole.$EditMenu=p._$editMenu;
		sohu.diyConsole.$MenuTxt=p._$menuTxt;
		sohu.diyConsole.$MenuImg=p._$menuImg;
		sohu.diyConsole.$MenuSecHead=p._$menuSecHead;
		sohu.diyConsole.$MenuElmc=p._$menuElmc;
		sohu.diyConsole.$CPKWrap=p._$wCpkWrap;
		sohu.diyConsole.$PopWins=p._$popWins;
		sohu.diyConsole.$Menu=p._$menus;
		sohu.diyConsole.$ElmTool=p._$elmTool;
		sohu.diyConsole.$WinSec=p._$wSec;
		sohu.diyConsole.$WinCode=p._$wCode;
		//sohu.diyConsole.$WinPageBG=p._$wPageBG;
		sohu.diyConsole.$SecEditorModel=$("#area_editor");
		sohu.diyConsole.$ScrollWrap=$("#scrollWrap");
		sohu.diyConsole.$BodyBGA=$("#main .bodyBGA");		
		//����ѡ����
		$('#hiddenTemplate .sec_selector li').click(function(evt){
			if(!sohu.diyConsole.CurSec) return;
			sohu.diyConsole.CurSec.Editor.SecSelector.Cur=this.id;
			sohu.diyConsole.CurSec.Editor.SecSelector.dialog("close");
			sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
			return false;
		}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
		
		//���ܰ�ť
		p._$btnAdd.bind("click",p.onAdd);
		p._$btnDel.click(p.onRemove);
		p._$btnBG.click(p.onAddBG);
		p._$btnPageBG.click(p.onPageBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
		
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
		//ͼƬ���ԶԻ���
		p.initImgPro();
		//����ͼƬ�Ի���
		p.initImgSwitch();
		//���б����Ի���
		p.initAreaBG();
		//��������Ի���
		p.initSecHead();
		//�������öԻ���
		p.initSec();
		//html����Ի���
		p.initCode();
		//editMenu���¼�ע��
		p.initEditMenu();
		//ҳ�汳������
		p.initPageBG();
		//Ԫ�ع�����
		sohu.diyElementTool.Init({});
		//�������
		sohu.diyDialog.Init({console:_this});
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
 * ��ʾ�����༭�˵�
 * @param {Object} group �˵�����
 * @param {Boolean} showOthers ��ʾ������˵�
 */
sohu.diyConsole.ShowEditMenu=function(group,showOthers){
	showOthers=showOthers||false;
	sohu.diyConsole.$EditMenu.show();
	if(!showOthers)
		sohu.diyConsole.$Menu.not(".mcom").hide();
	
	sohu.diyConsole[group].show();
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
