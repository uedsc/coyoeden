/**
 * ���ӻ�ר������������
 * @author levinhuang
 * @dependency sohu.diyDialog.js
 */
/**
 * ��Ӻ��е���
 */
sohu.diyDialog.wArea=function(dlg){
	var p={};
	//˽�к���
	p.afterShow=function(hash,dlg0){
		p._curTpl=null;
	};
	//����ɾ��ʱ�Ļص�����
	p.onAreaRemove=function(area){
		dlg.Console.ActiveArea(null);
		dlg.Console.$Layout.animate({top:dlg.Console.__p.opts.dfTop});	
	};
	//DOM����
	p.$layout=$("#addBlock");
	p.$layout.jqmOpts={title:"��Ӻ���",afterShow:p.afterShow,hideActions:true};
	//�¼�����
	p.selTpl=function(evt){
		p._curTpl=this.id;
		var obj=new sohu.diyArea({
				tplID:p._curTpl,
				console:dlg.Console,
				onRemove:p.onAreaRemove
		});
		dlg.Console.Areas.push(obj);
		dlg.Hide();	
		return false;
	};
	//�¼�ע��
	p.initEvts=function(){
		p.$layout.find("a").click(p.selTpl);
	}();
	
	
	this.$Layout=p.$layout;
};
/**
 * �������õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wCfgArea=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#cfgArea");
	
	//˽�б����º���
	p._fm={
		txtBG:$("#txtAreaBG"),
		txtID:$("#txtAreaID"),
		ddlBGAlign:$("#ddlAreaBGAlign"),
		ddlBGAlignV:$("#ddlAreaBGAlignV"),
		ddlBGRepeat:$("#ddlAreaBGRepeat"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//bg image
		var url=p._fm.txtBG.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBG.addClass("alert").select();
			return false;
		};
		if(url==""){
			sohu.diyConsole.CurArea.$Layout.css("background-image","");//shouldn't use 'none' here
		}else{
			sohu.diyConsole.CurArea.$Layout.css("background-image","url('"+url+"')");
			//bg position
			sohu.diyConsole.CurArea.$Layout.css("background-position",p._fm.ddlBGAlign.curVal+" "+p._fm.ddlBGAlignV.curVal);
			//bg repeat
			sohu.diyConsole.CurArea.$Layout.css("background-repeat",p._fm.ddlBGRepeat.curVal);
		};
		//area id
		var id=p._fm.txtID.val();
		if(!sohu.diyConsole.IsValidID(id)){
			p._fm.txtID.addClass("alert");
			return false;
		};
		var isIDOK=sohu.diyConsole.CurArea.UpdateID(id);
		
		if(cls&&isIDOK)
			dlg0.Hide();
	};
	p.afterShow=function(hash,dlg0){
		//bg img
		var img=sohu.diyConsole.CurArea.$Layout.css("background-image");
		img=img.replace('url("',"").replace('")',"");
		img=img=="none"?"":img;
		p._fm.txtBG.val(img).select();
		//area id
		p._fm.txtID.val(sohu.diyConsole.CurArea.ID);
		//����
		var bg_p=sohu.diyConsole.CurArea.$Layout.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":bg_p;
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlign.val(bg_p[0]);
			p._fm.ddlBGAlign.curVal=bg_p[0];
			p._fm.ddlBGAlignV.val(bg_p[1]);
			p._fm.ddlBGAlignV.curVal=bg_p[1];
		};		
		p._fm.ddlBGAlign.val(p._fm.ddlBGAlign.curVal);
		p._fm.ddlBGAlignV.val(p._fm.ddlBGAlignV.curVal);
		//ƽ��
		var bg_a=sohu.diyConsole.CurArea.$Layout.css("backgroundRepeat");
		p._fm.ddlBGRepeat.val(bg_a);
		p._fm.ddlBGRepeat.curVal=bg_a;		
		
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//�¼�ע��
	p._fm.txtBG.change(function(evt){
		p.preview();
	});
	p._fm.ddlBGAlign.change(function(evt){
		p._fm.ddlBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignV.change(function(evt){
		p._fm.ddlBGAlignV.curVal=this.value;
		p.preview();
	});	
	p._fm.ddlBGRepeat.change(function(evt){
		p._fm.ddlBGRepeat.curVal=this.value;
		p.preview();
	});		
	//�¼�ע��
	
	this.$Layout.jqmOpts={
		title:"��������",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ҳ�����õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wCfgPage=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#cfgPage");
	
	//˽�б����º���
	p._fm={
		txtBGA:$("#txtPageBGA"),
		txtBGB:$("#txtPageBGB"),
		txtBGHA:$("#txtPageBGHA"),
		txtBGHB:$("#txtPageBGHB"),
		ddlBGAlignA:$("#pageBGAlignA"),
		ddlBGAlignAV:$("#pageBGAlignAV"),
		ddlBGAlignB:$("#pageBGAlignB"),
		ddlBGAlignBV:$("#pageBGAlignBV"),
		ddlBGRepeatA:$("#pageBGRepeatA"),
		ddlBGRepeatB:$("#pageBGRepeatB"),
		txtBGC:$("#txtPageBGC"),
		cpk:this.$Layout.find(".cpk"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		//ҳͷ����ͼ
		var url=p._fm.txtBGA.val();
		if((url!="")&&(!StringUtils.isUrl(url))){
			p._fm.txtBGA.addClass("alert").select();
			return false;
		};
		//ҳβ����ͼ
		var url1=p._fm.txtBGB.val();
		if((url1!="")&&(!StringUtils.isUrl(url1))){
			p._fm.txtBGB.addClass("alert").select();
			return false;
		};
		//����ҳͷ����
		var h=p._fm.txtBGHA.val();
		if(!StringUtils.isPlusInt(h)){
			h=0;
		};
		sohu.diyConsole.$BodyBGA.css("height",h+'px');
		if(url==""){
			sohu.diyConsole.$BodyBGA.css("background-image","none");
		}else{
			sohu.diyConsole.$BodyBGA.css("background-image","url('"+url+"')");
		};
		sohu.diyConsole.$BodyBGA.css("background-position",p._fm.ddlBGAlignA.curVal+" "+p._fm.ddlBGAlignAV.curVal);
		sohu.diyConsole.$BodyBGA.css("background-repeat",p._fm.ddlBGRepeatA.curVal);
		
		//����ҳβ����
		h=p._fm.txtBGHB.val();
		if(!StringUtils.isPlusInt(h)){
			h=0;
		};
		sohu.diyConsole.$BodyBGB.css("height",h+'px');
		if(url1==""){
			sohu.diyConsole.$BodyBGB.css("background-image","none");
		}else{
			sohu.diyConsole.$BodyBGB.css("background-image","url('"+url1+"')");
		};
		sohu.diyConsole.$BodyBGB.css("background-position",p._fm.ddlBGAlignB.curVal+" "+p._fm.ddlBGAlignBV.curVal);
		sohu.diyConsole.$BodyBGB.css("background-repeat",p._fm.ddlBGRepeatB.curVal);
		
		if(cls)
			dlg0.Hide();
	};
	p.afterShow=function(hash,dlg0){
		//����ͼ
		var img=sohu.diyConsole.ParseBGImg(sohu.diyConsole.$BodyBGA.css("background-image"));
		p._fm.txtBGA.val(img).select();
		img=sohu.diyConsole.ParseBGImg(sohu.diyConsole.$BodyBGB.css("background-image"));
		p._fm.txtBGB.val(img);
		//�߶�
		var h=parseInt(sohu.diyConsole.$BodyBGA.css("height"));
		h=isNaN(h)?0:h;
		p._fm.txtBGHA.val(h);
		h=parseInt(sohu.diyConsole.$BodyBGB.css("height"));
		h=isNaN(h)?0:h;
		p._fm.txtBGHB.val(h);
		//ҳͷ
		//���뷽ʽ
		var bg_p=sohu.diyConsole.$BodyBGA.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":bg_p;
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlignA.val(bg_p[0]);
			p._fm.ddlBGAlignA.curVal=bg_p[0];
			p._fm.ddlBGAlignAV.val(bg_p[1]);
			p._fm.ddlBGAlignAV.curVal=bg_p[1];
		};
		
		//ƽ�̷�ʽ
		var bg_a=sohu.diyConsole.$BodyBGA.css("backgroundRepeat");
		p._fm.ddlBGRepeatA.val(bg_a);
		p._fm.ddlBGRepeatA.curVal=bg_a;
		//ҳβ
		bg_p=sohu.diyConsole.$BodyBGB.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":bg_p;
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlignB.val(bg_p[0]);
			p._fm.ddlBGAlignB.curVal=bg_p[0];
			p._fm.ddlBGAlignBV.val(bg_p[1]);
			p._fm.ddlBGAlignBV.curVal=bg_p[1];
		};		
		//ƽ�̷�ʽ
		bg_a=sohu.diyConsole.$BodyBGB.css("backgroundRepeat");
		p._fm.ddlBGRepeatB.val(bg_a);
		p._fm.ddlBGRepeatB.curVal=bg_a;
		//����ɫ
		p._fm.txtBGC.css("backgroundColor",sohu.diyConsole.$Body.css("backgroundColor"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	//�¼�ע��
	//TODO:��ȫ�ֵ�ȡɫ��
	p._fm.cpk.icolor({
		flat:true,
		onSelect:function(hex){
			sohu.diyConsole.$Body.css("backgroundColor",hex);
			p._fm.txtBGC.css("backgroundColor",hex);
			p._fm.cpk.hide();
		},
		showInput:true
	});
	p._fm.ddlBGAlignA.change(function(evt){
		p._fm.ddlBGAlignA.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignAV.change(function(evt){
		p._fm.ddlBGAlignAV.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignB.change(function(evt){
		p._fm.ddlBGAlignB.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignBV.change(function(evt){
		p._fm.ddlBGAlignBV.curVal=this.value;
		p.preview();
	});	
	p._fm.ddlBGRepeatA.change(function(evt){
		p._fm.ddlBGRepeatA.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGRepeatB.change(function(evt){
		p._fm.ddlBGRepeatB.curVal=this.value;
		p.preview();
	});		

	p._fm.txtBGHA.change(p.preview);
	p._fm.txtBGHB.change(p.preview);
	p._fm.txtBGA.change(p.preview);
	p._fm.txtBGB.change(p.preview);
	
	//����ɫ
	p._fm.txtBGC
	.css("backgroundColor","transparent")
	.click(function(evt){
		p._fm.cpk.show();
	});
	//�¼�ע��
	
	this.$Layout.jqmOpts={
		title:"ҳ������",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ������ݵ���
 * @param {Object} dlg
 */
sohu.diyDialog.wAddContent=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#addContent");
	this.$if=$("#ifContentList");
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		sohu.diyConsole.toggleLoading();
		_this.$if.attr("src",_this.$if.attr("rel")+"?t="+new Date().getTime());
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"�������",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		hideActions:true
	};
};
/**
 * �������뵯��
 * @param {Object} dlg
 */
sohu.diyDialog.wCode=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#wCode");
	this.$TextArea=this.$Layout.find("textarea");
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};	
	p.afterShow=function(hash,dlg0){
		_this.$TextArea.val(sohu.diyConsole.CurSec.$Layout.html());
	};
	p.onOK=function(dlg0){
		sohu.diyDialog.doConfirm({
			text:'ȷ�����µ�ǰ������HTML����ô?',
			onOK:function($jqm){
				sohu.diyConsole.CurSec.$Layout.html(_this.$TextArea.val());
				//���¼��ظ÷��������ݣ�����.html(x)��������ʱ��dom�Ѿ�����ԭ����dom
				sohu.diyConsole.CurSec.LoadContents();
				$jqm.jqmHide();
				dlg0.Hide();
			}
		});
	};
	//jqm options
	this.$Layout.jqmOpts={
		title:"����༭",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * �������õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wCfgSec=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#wCfgSec");
	//˽�к��������
	p._fm={
		txtBG:$("#txtSecBG"),
		txtBGC:$("#txtSecBGC"),
		txtBorderC:$("#txtSecBorderColor"),
		cpk:this.$Layout.find(".cpk"),
		ddlBGAlign:$("#ddlSecBGAlign"),
		ddlBGAlignV:$("#ddlSecBGAlignV"),
		ddlBGRepeat:$("#ddlSecBGRepeat"),
		cbxBorder:this.$Layout.find(".cbx"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0,cls){
		var _undefined;
		cls=cls==_undefined?true:cls;
		var url=$.trim(p._fm.txtBG.val());
		if( (url!="") && (!StringUtils.isUrl(url)) ){
			p._fm.txtBG.addClass("alert").select();
			return false;
		};
		//����ͼƬ����
		if (url != "") {
			sohu.diyConsole.CurSec.$Layout.css("background-image", "url('" + url + "')");
		}else{
			sohu.diyConsole.CurSec.$Layout.css("background-image", "none");
		};
		//��������
		sohu.diyConsole.CurSec.$Layout.css("background-position",p._fm.ddlBGAlign.curVal+" "+p._fm.ddlBGAlignV.curVal);
		//����ƽ��
		sohu.diyConsole.CurSec.$Layout.css("background-repeat",p._fm.ddlBGRepeat.curVal);
		//����Ի���
		if(cls)
			dlg0.Hide();			
	};
	p.preview=function(){
		p.onOK(null,false);
	};
	p.afterShow=function(hash,dlg0){
		//����ͼ
		var bgimg=sohu.diyConsole.CurSec.$Layout.css("background-image");
		bgimg=bgimg=="none"?"":bgimg;
		bgimg=bgimg.replace('url("',"").replace('")',"");
		p._fm.txtBG.val(bgimg);
		//����ɫ
		p._fm.txtBGC.val("").css("backgroundColor",sohu.diyConsole.CurSec.$Layout.css("backgroundColor"));
		//�߿�ɫ
		var bdc=sohu.diyConsole.CurSec.$Layout.css("borderColor");
		p._fm.txtBorderC.css("backgroundColor",bdc);
		//���뷽ʽ
		var bg_p=sohu.diyConsole.CurSec.$Layout.css("backgroundPosition");
		bg_p=bg_p=="0% 0%"?"center top":bg_p;
		bg_p=bg_p.split(" ");
		if(bg_p.length==2){
			p._fm.ddlBGAlign.val(bg_p[0]);
			p._fm.ddlBGAlign.curVal=bg_p[0];
			p._fm.ddlBGAlignV.val(bg_p[1]);
			p._fm.ddlBGAlignV.curVal=bg_p[1];
		};
		//ƽ�̷�ʽ
		var bg_a=sohu.diyConsole.CurSec.$Layout.css("backgroundRepeat");
		p._fm.ddlBGRepeat.val(bg_a);
		//�߿�-���ݱ߿�ɫ�жϱ߿������
		bdc=sohu.diyConsole.GetBorderColor(bdc);
		if(!bdc){
			p._fm.cbxBorder.attr("checked",false);
		}else{
			for(var x in bdc){
				if(x)
					p._fm.cbxBorder.filter("[value='"+x+"']")[0].checked=(bdc[x]!="transparent"&&bdc[x]!="none");
			};//for
		};//if			
	};
	//�¼�ע��
	//����ɫ
	p._fm.txtBGC
	.css("backgroundColor","transparent")
	.click(function(evt){
		p._fm.cpk.$t=p._fm.txtBGC;
		p._fm.cpk.flag="bg";
		p._fm.cpk.show();
	});
	//�߿�ɫ
	p._fm.txtBorderC
	.click(function(evt){
		p._fm.cpk.$t=p._fm.txtBorderC;
		p._fm.cpk.flag="bdc";
		p._fm.cpk.show();
	});
	//�߿���-�Զ����¼��ľ���֮��-�ο���722��762
	p._fm.cbxBorder.bind("evtClick",function(evt){
		if(this.checked){
			sohu.diyConsole.CurSec.$Layout.css("border-"+this.value,"1px solid "+p._fm.txtBorderC.attr("title"));
		}else{
			sohu.diyConsole.CurSec.$Layout.css("border-"+this.value,"none");
		};	
	});
	p._fm.cbxBorder.click(function(evt){
		$(this).trigger("evtClick");
	});		
	//����ͼ
	p._fm.txtBG.change(p.preview);
	//����ͼ���뷽ʽ
	p._fm.ddlBGAlign.change(function(evt){
		p._fm.ddlBGAlign.curVal=this.value;
		p.preview();
	});
	p._fm.ddlBGAlignV.change(function(evt){
		p._fm.ddlBGAlignV.curVal=this.value;
		p.preview();
	});	
	//����ͼƽ�̷�ʽ
	p._fm.ddlBGRepeat.click(function(evt){
		p._fm.ddlBGRepeat.curVal=this.value;
		p.preview();
	});
	//��Ŀ����ɫ
	//TODO:��ȫ��ȡɫ��
	p._fm.cpk.icolor({
		flat:true,
		onSelect:function(c){
			p._fm.cpk.$t.css("backgroundColor",c).attr("title",c);
			if(p._fm.cpk.flag=="bg"){
				sohu.diyConsole.CurSec.$Layout.css("backgroundColor",c);
			}else{
				p._fm.cbxBorder.trigger("evtClick");
			};
			p._fm.cpk.hide();
		},
		showInput:true
	});			
	//jqm options
	this.$Layout.jqmOpts={
		title:"��������",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * ��Ŀͷ���õ���
 * @param {Object} dlg
 */
sohu.diyDialog.wSecHead=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#wSecHead");
	p._fm={
		bg:$("#secHDBG"),
		cbxMore:$("#cbxSecHDMore"),
		link:$("#secHDLink"),
		ddlTarget:$("#ddlSecHDTarget"),
		title:$("#secHDTitle"),
		tipLink:this.$Layout.find(".tipSecHDLink"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onIframeSelect=function(evt){
		sohu.diyConsole.SnapSelection();
		var a=sohu.diyConsole.DocSelection.getA();
		if(a.isNull){
			p._fm.isNew=true;
			p.a=null;
			p._fm.title.val(sohu.diyConsole.DocSelection.text);
			return;
		};
		a.href=a.href=="#"?"":a.href;
		p._fm.title.val(a.title);
		p._fm.link.val(a.href);
		p._fm.ddlTarget.val(a.target);
		p._fm.isNew=false;
		p._fm.a=a;	
	};	
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");		
		return true;
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();		
	};
	p.afterShow=function(hash,dlg0){
		//����ͼ
		var bg=sohu.diyConsole.CurElm.CT.$Layout.css("background-image");
		bg=bg=="none"?"":bg;
		bg=bg.replace('url("',"").replace('")',"");
		p._fm.bg.val(bg);
		//�Ƿ���ʾ����
		if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length>0){
			p._fm.cbxMore.attr("checked",true);
		}else{
			p._fm.cbxMore.attr("checked",false);
		};
		//��������
		var $a=sohu.diyConsole.CurElm.CT.$Layout.find("a[href!='']");
		if($a.length>0){
			p._fm.link.val($a[0].href);
		};
		//��׽iframe�༭���û�ѡ��������
		sohu.diyConsole.CurElm.i$frame[0].i$Body().unbind("mouseup").mouseup(p.onIframeSelect);		
	};
	p.onOK=function(dlg0){
		var url=$.trim(p._fm.link.val());
		if((url!="")&&!StringUtils.isUrl(url)){
			p._fm.link.addClass("alert").select();
			p._fm.tipLink.addClass("alert").show();
			return false;
		};
		p._fm.link.removeClass("alert");
		p._fm.tipLink.removeClass("alert").hide();
		//����ͼ
		var img=$.trim(p._fm.bg.val());
		img=img==""?"none":img;
		sohu.diyConsole.CurElm.CT.$Layout.css("background-image",'url("'+img+'")');	
		
		//��ʾ����
		if(p._fm.cbxMore[0].checked){
			if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length==0){
				var $more=$('<strong class="elm more">����&gt;&gt;</strong>');
				sohu.diyConsole.CurElm.CT.$Layout.append($more);
				new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$more});
			};
		}else{
			sohu.diyConsole.CurElm.CT.$Layout.find(".more").remove();
		};
			
		if(p._fm.isNew&&url!=""){
			sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
				sohu.diyConsole.DocSelection.selectAndRelease();
			});
		}else if(p._fm.a){
			p._fm.a.$obj.attr("title",p._fm.title.val())
				.attr("target",p._fm.ddlTarget.curVal);
			if(url!=""){
				p._fm.a.$obj.attr("href",url);
			}else{
				p._fm.a.$obj.attr("href","#");
			};
		};

		dlg0.Hide();
	};	
	//�¼�ע��
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
	});
	//jqm options
	this.$Layout.jqmOpts={
		title:"��Ŀ����",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK		
	};
};
/**
 * ��ӷ�������
 * @param {Object} dlg
 * @param {Object} parentSize �������Ŀ��
 */
sohu.diyDialog.wSubSec=function(dlg,parentSize){
	var p={};
	//DOM����
	this.$Layout=$("#subSec"+parentSize);
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		return true;
	};
	p.afterHide=function(hash,dlg0){
		//sohu.diyConsole.CurSec.Editor.Editing("off").CurSec.Deactive();
		sohu.diyConsole.Preview();
	};	
	//�¼�ע��
	this.$Layout.find('.item').click(function(evt){
		if(!sohu.diyConsole.CurSec) return;
		//sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
		sohu.diyConsole.CurSec.AddSub($(this).find(".subsec").clone());
		dlg.Hide();
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
	//jqm options
	this.$Layout.jqmOpts={
		title:"��ӷ���",
		beforeShow:p.beforeShow,
		afterHide:p.afterHide,
		hideActions:true
	};
};
/**
 * ���ֱ༭����
 * @param {Object} dlg
 */
sohu.diyDialog.wText=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#txtMsg");
	this.$ElmCAction=this.$Layout.find(".elmcActs");
	p._fm={
		txtATitle:$("#txtATitle"),
		txtAHref:$("#txtAHref"),
		ddlATarget:$("#ddlATarget"),
		tipAHref:this.$Layout.find(".tipAHref"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onIframeSelect=function(evt){
		sohu.diyConsole.SnapSelection();
		var a=sohu.diyConsole.DocSelection.getA();
		if(a.isNull){
			p._fm.isNew=true;
			p.a=null;
			p._fm.txtATitle.val(sohu.diyConsole.DocSelection.text);
			return;
		};
		a.href=a.href=="#"?"":a.href;
		p._fm.txtATitle.val(a.title);
		p._fm.txtAHref.val(a.href);
		p._fm.ddlATarget.val(a.target);
		p._fm.isNew=false;
		p._fm.a=a;	
	};
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");
		p._fm.txtATitle.val("");
		p._fm.txtAHref.val("");
		return true;
	};	
	p.afterShow=function(hash,dlg0){
		//dlg0.$Layout.css("top",25);
		//�ж��Ƿ��ǿɸ���Ԫ��
		if(sohu.diyConsole.CurElm.Copyable){
			_this.$ElmCAction.show();
		}else{
			_this.$ElmCAction.hide();
		};
		//��׽iframe�༭���û�ѡ��������
		sohu.diyConsole.CurElm.i$frame[0].i$Body().unbind("mouseup").mouseup(p.onIframeSelect);
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0){
		var url=p._fm.txtAHref.val();
		if(url!=""&&!StringUtils.isUrl(url)){
			p._fm.txtAHref.addClass("alert").select();
			p._fm.tipAHref.addClass("alert").show();
			return false;
		};
		p._fm.txtAHref.removeClass("alert");
		p._fm.tipAHref.removeClass("alert").hide();	
		if(p._fm.isNew&&url!=""){
			sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
				sohu.diyConsole.DocSelection.selectAndRelease();
			});
		}else{
			if(p._fm.a){
				p._fm.a.$obj.attr("title",p._fm.txtATitle.val())
					.attr("href",url)
					.attr("target",p._fm.ddlATarget.curVal);				
			};
		};
		dlg0.Hide();
	};
	//�¼�ע��
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
	});
	p._fm.ddlATarget.change(function(evt){
		p._fm.ddlATarget.curVal=this.value;
	});
	p._fm.ddlATarget.curVal="_blank";
	//jqm options
	this.$Layout.jqmOpts={
		title:"����",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ͼƬ�༭����
 * @param {Object} dlg
 */
sohu.diyDialog.wImage=function(dlg){
	var _this=this;
	var p={};
	//DOM����
	this.$Layout=$("#picMsg");
	this.$ElmCAction=this.$Layout.find(".elmcActs");
	p._fm={
		h:$("#picH"),
		w:$("#picW"),
		bcolor:$("#picBColor"),
		src:$("#picSrc"),
		btnSwitch:$("#picSwitch"),
		link:$("#picLink"),
		tipLink:this.$Layout.find(".tipPicLink"),
		ddlTarget:$("#picTarget"),
		title:$("#picTitle"),
		alt:$("#picAlt"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.beforeShow=function(hash,dlg0){
		sohu.diyConsole.CurSec.Editor.Editing("on");
		//clear the selected style of the diyMenuBar icons
		_this.$Layout.find('.cmdicon').removeClass("editBtn1");		
		return true;
	};	
	p.afterShow=function(hash,dlg0){
		//dlg0.$Layout.css("top",25);
		//�ж��Ƿ��ǿɸ���Ԫ��
		if(sohu.diyConsole.CurElm.Copyable){
			_this.$ElmCAction.show();
		}else{
			_this.$ElmCAction.hide();
		};
		var $img=sohu.diyConsole.CurElm.$Layout;
		//�߿�
		var h=$img.attr("height");
		var w=$img.attr("width");
		p._fm.h.val(h);
		p._fm.w.val(w);
		//�߿�ɫ
		p._fm.bcolor.css("backgroundColor",$img.css("backgroundColor"));
		//src
		p._fm.src.val($img.attr("src"));
		//����
		var $a=$img.parent("a");
		$a=$a.length>0?$a:$({href:"",title:""});
		var href=$a.attr("href");
		href=href=="#"?"":href;
		p._fm.link.val(href);
		//ͼƬ����
		p._fm.title.val($img.attr("title"));
		//�������
		p._fm.alt.val($img.attr("alt"));
	};
	p.afterHide=function(hash,dlg0){
		p._fm.reset();
		sohu.diyConsole.Preview();
	};
	p.onOK=function(dlg0){
		//ͼƬ��ַ
		var src=p._fm.src.val();
		/*
		if(src!=""&&!StringUtils.isUrl(src)){
			p._fm.src.addClass("alert").select();
			return false;
		};
		p._fm.src.removeClass("alert");
		*/
		//����
		var link=p._fm.link.val();
		if(link!=""&&!StringUtils.isUrl(link)){
			p._fm.link.addClass("alert").select();
			p._fm.tipLink.addClass("alert").show();
			return false;
		};
		p._fm.link.removeClass("alert");
		p._fm.tipLink.removeClass("alert").hide();
		//�߿�
		var fine=true,v=["",""];
		$.each([p._fm.h,p._fm.w],function(i,o){
			var v0=o.val();
			v[i]=v0;
			if((v0!="")&&(!StringUtils.isPlusInt(v0))){
				o.addClass("alert");
				fine=fine&&false;
			};//if
		});//each
		if(!fine){
			return false;
		};
		if(v[0]==""){
			sohu.diyConsole.CurElm.$Layout.removeAttr("height");	
		}else{
			sohu.diyConsole.CurElm.$Layout.attr("height",v[0]);
		};
		if(v[1]==""){
			sohu.diyConsole.CurElm.$Layout.removeAttr("width");	
		}else{
			sohu.diyConsole.CurElm.$Layout.attr("width",v[1]);
		};		
		//�߿�ɫ
		var c=p._fm.bcolor.curVal;
		//����ͼƬ��ʽ
		sohu.diyConsole.CurElm.$Layout.css({
			"border-color":c
		}).attr("src",src).attr("title",p._fm.title.val()).attr("alt",p._fm.alt.val());
		//��������
		var a=sohu.diyConsole.CurElm.$Layout.parent("a");
		if(a.length>0){
			if(link!=""){
				a.attr("target",p._fm.ddlTarget.val()).attr("href",link);
			};	
		};
		dlg0.Hide();
	};
	//�¼�ע��
	this.$Layout.find("input[type='text']").keyup(function(evt){
		this.value=this.value.replace('"',"").replace("'","").replace('��',"").replace('��',"").replace('��',"");
	});
	p._fm.bcolor.click(function(evt){
		var _this=$(this);
		sohu.diyDialog.showColorPicker({
			onSubmit:function(c){
				p._fm.bcolor.css("backgroundColor",c);
				p._fm.bcolor.curVal=c;
			},
			onChange:function(c){
				p._fm.bcolor.css("backgroundColor",c);
				p._fm.bcolor.curVal=c;
			}
		});		
	});
	//jqm options
	this.$Layout.jqmOpts={
		title:"ͼ��",
		modal:false,
		overlay:0,
		beforeShow:p.beforeShow,
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ҳ������
 * @param {Object} dlg
 */
sohu.diyDialog.wPagePro=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wPagePro");
	//�¼�����
	p._fm={
		title:$("#pTitle"),
		kwd:$("#pKeywords"),
		desc:$("#pDesc")
	};
	p.afterShow=function(hash,dlg0){
		p._fm.title.val($("title").html());
	};
	p.onOK=function(dlg0){
		dlg0.Hide();
	};
	//�¼�ע��
	//jqm options
	this.$Layout.jqmOpts={
		title:"ҳ������",
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * ҳ����
 * @param {Object} dlg
 */
sohu.diyDialog.wTheme=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wTheme");
	//�¼�����
	p.afterShow=function(hash,dlg0){
	};
	p.onOK=function(dlg0){
		dlg0.Hide();
	};
	//�¼�ע��
	//jqm options
	this.$Layout.jqmOpts={
		title:"�������",
		afterShow:p.afterShow,
		onOK:p.onOK
	};
};
/**
 * ����������ò˵�
 * @param {Object} dlg
 */
sohu.diyDialog.wSetting1=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wSetting1");
	this.$Body=this.$Layout.children();
	this.IsOpen=true;
	//�¼�����
	p.onTheme=function(evt){
		if(sohu.diyConsole.IsPreview) return false;
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("wTheme");
	};
	p.onPagePro=function(evt){
		if(sohu.diyConsole.IsPreview) return false;
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("wPagePro");
	};
	p.onPageBG=function(evt){
		//if(!sohu.diyConsole.CurArea){alert("δѡ���κκ���!");return false;};
		if(sohu.diyConsole.IsPreview) return false;
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("cfgPage");
	};	
	p.onPreview=function(evt){
		var $b=$("body");
		if (sohu.diyConsole.IsPreview) {
			$(this).find("strong").html("Ԥ��");
			$b.removeClass("preview");
			sohu.diyConsole.UnPreview();
		}else{
			sohu.diyConsole.Preview(true);
			if(sohu.diyConsole.IsPreview){
				$b.addClass("preview");
				$(this).find("strong").html("�ر�Ԥ��");				
			};	
		};
			
		if((!sohu.diyConsole.CurArea)||(!sohu.diyConsole.CurArea.IsEditing))
			return false;
			
		sohu.diyDialog.Hide();
			
		return false;
	};
	p.onPublish=function(evt){alert("publish");};
	p.onHelp=function(evt){alert("help");};
	p.onToggle=function(evt){
		if(_this.IsOpen){
			_this.$Body.addClass("close");
			_this.$Layout.animate({left:-82},"normal");
			_this.IsOpen=false;
		}else{
			_this.$Body.removeClass("close");
			_this.$Layout.animate({left:-1},"normal");
			_this.IsOpen=true;
		};
		return false;
	};
	//�¼�ע��
	this.$Layout.find("a").click(function(evt){return false;});
	$("#wsTheme").click(p.onTheme);
	$("#wsPagePro").click(p.onPagePro);
	$("#wsPageBG").click(p.onPageBG);
	$("#wsPreview").click(p.onPreview);
	$("#wsPublish").click(p.onPublish);
	$("#wsHelp").click(p.onHelp);
	this.$Layout.find(".ctr").click(p.onToggle);
	
};
/**
 * FLASH�༭��
 * @param {Object} dlg
 */
sohu.diyDialog.wFlash=function(dlg){
	var p={},_this=this;
	//DOM����
	this.$Layout=$("#wFlash");
	this.$t=this.$Layout.find('.cfg');
	
	p._fm={
		w:$("#txtWFlash"),
		h:$("#txtHFlash"),
		reset:function(){sohu.diyDialog.resetForm(_this);}
	};
	//�¼�����
	p.onChangeImg=function(evt){
		$(this).parents('.cfg').find('.thumb').attr('src',this.value);
		return false;
	};
	p.onAdd=function(evt){
		var t0=$(this).parents('.cfg');
		var o=t0.clone(true);
		o.find(".thumb").attr("src","").end()
			.find(".flashImg").val("").end()
			.find(".flashTxt").val("").end()
			.find(".flashHref").val("");
		t0.after(o);
		return false;
	};
	p.onDel=function(evt){
		$(this).parents('.cfg').remove();
		return false;
	};
	p.onUp=function(evt){
		var t0=$(this).parents('.cfg');
		var t1=t0.prev(".cfg1");
		if(t1.length==0) return false;
		t1.before(t0);
		return false;
	};
	p.onDown=function(evt){
		var t0=$(this).parents('.cfg');
		var t1=t0.next(".cfg1");
		if(t1.length==0) return false;
		t1.after(t0);
		return false;
	};
	p.afterShow=function(hash,dlg0){
		_this.$Layout.find('.cfg1').remove();
		p._fm.w.val(sohu.diyConsole.CurCT.FlashData.w);
		p._fm.h.val(sohu.diyConsole.CurCT.FlashData.h);
		//ͼƬ�б�
		p._pics=sohu.diyConsole.CurCT.FlashData.v.pics.split('|');
		p._links=sohu.diyConsole.CurCT.FlashData.v.links.split('|');
		p._texts=sohu.diyConsole.CurCT.FlashData.v.texts.split('|');
		$.each(p._pics,function(i,o){
			var $t0=_this.$t.clone(true).addClass('cfg1');
			$t0.find('.flashImg').val(o).end()
				.find('.flashTxt').val(p._texts[i]).end()
				.find('.flashHref').val(p._links[i]).end()
				.find('.thumb').attr('src',o).end()
				.appendTo(_this.$Layout);	
		});
	};
	p.afterHide=function(hash,dlg0){
		sohu.diyConsole.CurCT.InlineEdit('off');
	};
	p.onOK=function(dlg0){
		//��֤
		var h=p._fm.h.val(),w=p._fm.w.val();
		if(!StringUtils.isPlusInt(h)){
			p._fm.h.addClass("alert");
			return false;
		};
		if(!StringUtils.isPlusInt(w)){
			p._fm.w.addClass("alert");
			return false;
		};
		
		p._pics=_this.$Layout.find('.cfg1 .flashImg').map(function(){
			return this.value;
		}).get().join("|");
		p._links=_this.$Layout.find('.cfg1 .flashHref').map(function(){
			return this.value;
		}).get().join("|");
		p._texts=_this.$Layout.find('.cfg1 .flashTxt').map(function(){
			return this.value;
		}).get().join("|");
		
		p._fm.reset();		
		//����flash
		sohu.diyConsole.CurCT.FlashData.v.pic_width=w;
		sohu.diyConsole.CurCT.FlashData.v.pic_height=h;
		sohu.diyConsole.CurCT.FlashData.w=w;
		sohu.diyConsole.CurCT.FlashData.h=h;
		if(sohu.diyConsole.CurCT.FlashData.v.show_text){
			sohu.diyConsole.CurCT.FlashData.h=parseInt(h)+20;	
		};
		
		sohu.diyConsole.CurCT.FlashData.v.pics=p._pics;
		sohu.diyConsole.CurCT.FlashData.v.links=p._links;
		sohu.diyConsole.CurCT.FlashData.v.texts=p._texts;
		
		sohu.diyConsole.CurCT.$FlashData.html($.toJSON(sohu.diyConsole.CurCT.FlashData));
		sohu.diyConsole.CurCT.FlashObj.setAttribute("width",w);
		sohu.diyConsole.CurCT.FlashObj.setAttribute("height",sohu.diyConsole.CurCT.FlashData.h);
		
		sohu.diyConsole.CurCT.FlashObj.variables["pics"]=p._pics;
		sohu.diyConsole.CurCT.FlashObj.variables["links"]=p._links;
		sohu.diyConsole.CurCT.FlashObj.variables["texts"]=p._texts;
		
		if(sohu.diyConsole.CurCT.FlashData.v.pic_width){
			sohu.diyConsole.CurCT.FlashObj.variables["pic_width"]=w;
			sohu.diyConsole.CurCT.FlashObj.variables["pic_height"]=h;
		};
		
		sohu.diyConsole.CurCT.FlashObj.write(sohu.diyConsole.CurCT.FlashData.pid);
		dlg0.Hide();
	};
	//�¼�ע��
	this.$t.find('.flashImg').change(p.onChangeImg);
	this.$t.find('.add').click(p.onAdd);
	this.$t.find('.del').click(p.onDel);
	this.$t.find('.up').click(p.onUp);
	this.$t.find('.down').click(p.onDown);
	//jqm options
	this.$Layout.jqmOpts={
		title:"����ͼ",
		afterShow:p.afterShow,
		afterHide:p.afterHide,
		onOK:p.onOK
	};
};
/**
 * ���й�����
 * @param {Object} dlg
 */
sohu.diyDialog.wAreaTool=function(dlg){
	var p={};
	//DOM����
	p.$layout=$("#areaTools");
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	
	//�¼�����
	p.onReposition=function(evt){
		if(!sohu.diyConsole.CurArea){p.$layout.attr("style","");return;};
		var top=sohu.diyConsole.CurArea.$Layout.position().top-sohu.diyConsole.$Window.scrollTop();
		top=top<0?0:top;
		p.$layout.css("top",top);	
	};
	
	p.onAdd=function(evt){
		if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("addBlock");
	};
	p.onDel=function(evt){
		if(!sohu.diyConsole.CurArea){
			sohu.diyDialog.doAlert({text:'δѡ���κκ���!'});
			return false;
		};
		dlg.Console.Areas=$.grep(dlg.Console.Areas,function(o,i){
			if(o.ID==dlg.Console.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
	};
	p.onCfgBG=function(evt){
		if(!sohu.diyConsole.CurArea){sohu.diyDialog.doAlert({text:'δѡ���κκ���!'});return false;};
		if(sohu.diyConsole.CurArea.IsEditing) return false;
		sohu.diyDialog.Show("cfgArea");
	};
	p.onMove=function(evt){
		if(!sohu.diyConsole.CurArea){sohu.diyDialog.doAlert({text:'δѡ���κκ���!'});return false;};
		if(sohu.diyConsole.CurArea.IsEditing) return false;	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		p.onReposition();
	};
	//�¼�ע��
	p.initEvts=function(){
		p.$layout.bind("evtReposition",p.onReposition).find("a").click(function(evt){return false;});
		p._$btnAdd.click(p.onAdd);
		p._$btnDel.click(p.onDel);
		p._$btnBG.click(p.onCfgBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
	}();
	
	this.$Layout=p.$layout;
	this.RePosition=p.onReposition;	
};
/**
 * ���������ض�λ
 */
sohu.diyDialog.wAreaTool.Reposition=function(){
	var dlg=sohu.diyDialog.Get("areaTools");
	if(!dlg) return;
	dlg.RePosition();
};
/**
 * ע�ᵯ��ʵ��
 * @param {Object} dlg
 */
sohu.diyDialog.onInit=function(dlg){
	new sohu.diyDialog.wSetting1(dlg);
	sohu.diyDialog.Register("addBlock",new sohu.diyDialog.wArea(dlg));
	sohu.diyDialog.Register("areaTools",new sohu.diyDialog.wAreaTool(dlg));
	sohu.diyDialog.Register("cfgArea",new sohu.diyDialog.wCfgArea(dlg));
	sohu.diyDialog.Register("cfgPage",new sohu.diyDialog.wCfgPage(dlg));
	sohu.diyDialog.Register("addContent",new sohu.diyDialog.wAddContent(dlg));
	sohu.diyDialog.Register("code",new sohu.diyDialog.wCode(dlg));
	sohu.diyDialog.Register("cfgSec",new sohu.diyDialog.wCfgSec(dlg));
	sohu.diyDialog.Register("subSec190",new sohu.diyDialog.wSubSec(dlg,190));
	sohu.diyDialog.Register("subSec230",new sohu.diyDialog.wSubSec(dlg,230));
	sohu.diyDialog.Register("subSec270",new sohu.diyDialog.wSubSec(dlg,270));
	sohu.diyDialog.Register("subSec310",new sohu.diyDialog.wSubSec(dlg,310));
	sohu.diyDialog.Register("subSec390",new sohu.diyDialog.wSubSec(dlg,390));
	sohu.diyDialog.Register("subSec430",new sohu.diyDialog.wSubSec(dlg,430));
	sohu.diyDialog.Register("subSec470",new sohu.diyDialog.wSubSec(dlg,470));
	sohu.diyDialog.Register("subSec510",new sohu.diyDialog.wSubSec(dlg,510));
	sohu.diyDialog.Register("subSec670",new sohu.diyDialog.wSubSec(dlg,670));
	sohu.diyDialog.Register("subSec950",new sohu.diyDialog.wSubSec(dlg,950));
	sohu.diyDialog.Register("wText",new sohu.diyDialog.wText(dlg));
	sohu.diyDialog.Register("wImage",new sohu.diyDialog.wImage(dlg));
	sohu.diyDialog.Register("wSecHead",new sohu.diyDialog.wSecHead(dlg));	
	sohu.diyDialog.Register("wPagePro",new sohu.diyDialog.wPagePro(dlg));
	sohu.diyDialog.Register("wTheme",new sohu.diyDialog.wTheme(dlg));
	sohu.diyDialog.Register("wFlash",new sohu.diyDialog.wFlash(dlg));
};
