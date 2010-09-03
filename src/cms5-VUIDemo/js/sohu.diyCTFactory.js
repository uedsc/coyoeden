/**
 * �Ѻ�cms5����ģ���ඨ��
 */
sohu.diyTplFactory={
	/**
	 * �ر�����ѡ��Ի���
	 */
	cls:function(){
		parent.bos.CloseCTDialog();
	},
	/**
	 * �ύ���ݶ���
	 * @param {Object} ct ���ݶ���,��{isNew:true,flash:false,html0:''}
	 * @param {Boolean} cls�Ƿ�رնԻ���
	 */
	submit:function(ct,cls){
		sohu.diyCTFactory.CurCT=ct;
		parent.sohu.diyConsole.CurSec.AddContent(ct);
		if (cls) {
			sohu.diyTplFactory.cls();
		};
	},
	r:function(evt){return false;},
	/**
	 * ��ȡclass���Ե�ĳ��ֵ��ע������ģ���class���Եĵ�һ��ֵ��ʾ��ģ�������
	 * @param {Object} $dom ����ģ��
	 */
	getClass:function($dom,idx){
		idx=idx||0;
		var items=$dom.attr("class").split(" ");
		items=$.grep(items,function(o,i){
			if($.trim(o)=="") return false;
			return true;
		});
		if(idx<0) return items;
		idx=idx>=items.length?(items.length-1):idx;
		return items[idx];
	}
};
/**
 * ������������
 * @param {Object} opts
 */
sohu.diyTplFactory.Nav=function(opts){
	var _this=this;
	this.$Layout=$("#ctNav");
	this.CurNav=null;
	//�¼�ע��
	$("#navList li").click(function(evt){
		_this.CurNav=$(this).find(".ctWrap").html();
		_this.Submit();
	});
};
sohu.diyTplFactory.Nav.prototype.Submit=function(){
	var ct={};
	ct.html0=this.CurNav;
	ct.flash=false;
	ct.isNew=true;
	ct.type="nav";
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * ��Ŀ����������
 * @param {Object} opts
 */
sohu.diyTplFactory.SecHead=function(opts){
	var _this=this;
	//����-dom����
	this.$Layout=$("#ctSecHead");
	//˽�г�Ա
	//�¼�ע��
	$("#secHeadList li").click(function(evt){
		_this.CurHtml=$(this).find(".ctWrap").html();
		_this.Submit();
	});
};
sohu.diyTplFactory.SecHead.prototype.Submit=function(opts){
	var ct={
		flash:false,
		html0:this.CurHtml,
		isNew:true,
		type:"sec_hd"
	};
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * ���е�������
 */
sohu.diyTplFactory.Line=function(opts){
	var _this=this;
	//���Զ���-�û�����
	this.layoutID=opts.layoutID||"ctEmptyLine";//ģ����
	this.css_cpk=opts.css_cpk||".cpk";//cpk cssѡ����
	this.css_txtH=opts.css_txtH||".txtH";//�߶�¼��� cssѡ����
	this.css_txtW=opts.css_txtW||".txtW";//���¼���
	this.css_btnOK=opts.css_btnOK||".btnOK";
	this.css_cbkLine=opts.css_cbkLine||".cbxLine";
	this.color=opts.color||"#808080";
	this.height=opts.height||10;
	this.width=opts.width||96;/* �߶ȣ� */
	this.cLine=opts.cLine||false;//�Ƿ��м���ʾ�ָ���
	//���Զ���-dom����
	this.html=null;//ģ��html
	this.$layoutCfg=null;//���ý���dom
	this.$txtCpk=null;//dom-��ɫѡ����colorpicker
	this.$txtHeight=null;//dom-�߶������
	this.$txtWidth=null;/* input $obj for width */
	this.$btnOK=null;//�ύ��ť
	this.$cbkLine=null;//��ʾ���߸�ѡ��
	
	//˽�г�Ա
	var p={};
	p.initLayout=function(){
		_this.html=sohu.diyTp[_this.layoutID];
		_this.$layoutCfg=$("#"+_this.layoutID);
		//��ɫ¼���
		_this.$txtCpk=_this.$layoutCfg.find(_this.css_cpk).ColorPicker({
			color:_this.color,
			onShow:function(cpk){
				$(cpk).fadeIn(500);return false;
			},
			onHide:function(cpk){
				$(cpk).fadeOut(500); return false;
			},
			onChange:function(hsb,hex,rgb){
				_this.color="#"+hex;
				_this.$txtCpk.css("backgroundColor",_this.color).val(_this.color);
			}
		});
		//�߶�¼���
		_this.$txtHeight=_this.$layoutCfg.find(_this.css_txtH).keyup(function(evt){
			if(!StringUtils.isPlusInt(this.value)){
				this.value="10";
				this.select();
			};

		}).blur(function(evt){
			_this.height=parseInt(this.value);
			_this.height=isNaN(_this.height)?10:_this.height;
			_this.height=(_this.height>1000||_this.height<1)?10:_this.height;
			this.value=_this.height;
		});
		/* input for width */
		_this.$txtWidth=_this.$layoutCfg.find(_this.css_txtW).keyup(function(evt){
			/* handler for keyup evt */
			if(!StringUtils.isPlusInt(this.value)){
				this.value="100";
				this.select();
			}
			
		}).blur(function(evt){
			/* handler for blur evt */
			//_this.width=parseInt(this.value);
			//_this.width=isNaN(_this.width)?100:_this.width;
			_this.width=isNaN(_this.width=parseInt(this.value))?100:_this.width;
			_this.width=(_this.width>100||_this.width<10)?100:_this.width;
			this.value=_this.width;
		});
		//ȷ����ť
		_this.$btnOK=_this.$layoutCfg.find(_this.css_btnOK).click(function(evt){
			_this.Submit({});return false;
		});
		//���߸�ѡ��
		_this.$cbkLine=_this.$layoutCfg.find(_this.css_cbkLine);
	};
	p.initLayout();
};
sohu.diyTplFactory.Line.prototype.Submit=function(opt){
	this.cLine=this.$cbkLine[0].checked;
	sohu.diyCTFactory.$ctWrap.html(this.html);
	var hr=sohu.diyCTFactory.$ctWrap.find("hr");
	var line=sohu.diyCTFactory.$ctWrap.find(".vspace");
	if(this.cLine){
		var css={
			"border-color":this.color,
			"width":this.width+"%",
			"left":(100-this.width)/2+"%"
		};
		hr.css(css);
		
	}else{
		hr.remove();
	};
	var css={"height":this.height+'px'};
	line.css("height",this.height+'px');
	
	var ct={
		flash:false,
		html0:sohu.diyCTFactory.$ctWrap.html(),
		isNew:true,
		type:'vspace'
	};
	
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * ����ͼѡ����
 */
sohu.diyTplFactory.FocusImg=function(opts){
		var _this=this;
		//����ͼѡ��
		this.$Layout=$("#focusImgSlide").abcdSlider({
			showNum:1,
			total:2,
			externalBtn:true,
			cssBtnL:'#ctFocusImg .btnL',
			cssBtnR:'#ctFocusImg .btnR',
			cssPanel:'#cSlide_focusImg',
			step:540,
			onSlide:function($dom,mode){
				_this.$btnDots.removeClass("currA").eq($dom._p.slideNum).addClass("currA");
			}
		});
		this.tplID=null;//flash ģ���
		this.$btnDots=$("#ctImg .btnDot").click(function(evt){return false;});
	
		var p={};
		p.onAddFlash=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find('.ctWrap').clone();
			_this.$jsonData=_this.tplObj.find('.flashData');
			_this.flashData=$.evalJSON(_this.$jsonData.html());
			_this.flashData.dummy=false;
			_this.$jsonData.html($.toJSON(_this.flashData));
			_this.Submit();
			return false;
		};
		//����¼�
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddFlash);
	
};
sohu.diyTplFactory.FocusImg.prototype.Submit=function(opt){
	var ct={};
	var id='flash'+StringUtils.RdStr(8);
	ct.html0=this.tplObj.html().replace(/FLASHID/g,id);
	ct.flash=true;
	/*
	ct.tplID=this.tplID;
	*/
	ct.isNew=true;
	ct.type='flash';
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * ͼƬѡ����
 */
sohu.diyTplFactory.Image=function(opts){
		var _this=this;
		//����ͼѡ��
		this.$Layout=$("#ctImgSlide").abcdSlider({
			step:488,
			showNum:1,
			total:2,
			externalBtn:true,
			cssBtnL:'#ctImg .btnL',
			cssBtnR:'#ctImg .btnR',
			cssPanel:'#ctImgSlider',
			onSlide:function($dom,mode){
				_this.$btnDots.removeClass("currA").eq($dom._p.slideNum).addClass("currA");
			}
		});
				
		//��ͼ���õ���
		this.$btnDots=$("#ctImg .btnDot").click(function(evt){return false;});
		this.$WinCfg=$("#cfgImgList");
		this.$WinCfg.ovl=$("#cfgImgList_ovl");
		this.$WinCfg.$imgWidth=$("#txtImgWidth");
		this.$WinCfg.$imgCol=$("#ddlImgListCol");
		this.$WinCfg.$tip=this.$WinCfg.find(".tip");
		this.$WinCfg.$tip1=this.$WinCfg.find(".tip1");
		this.tplID=null;//ģ���
		this.IsImgList=false;//�Ƿ���ͼ
		
		var p={};
		p.onAddImg=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.CurColSize=parseInt(parent.sohu.diyConsole.CurSec.Width);
			_this.CurSecSize=parent.sohu.diyConsole.CurSec.Dim().w;
			//�������ͼ��������ͼ���ÿ�
			_this.IsImgList=_this.tplObj.find(".ct").hasClass("pp");
			if(_this.IsImgList){
				_this.$WinCfg.ovl.css("opacity","0.7").show();
				_this.$WinCfg.$tip.html(_this.CurColSize);
				_this.$WinCfg.$tip1.html(_this.CurSecSize-10-6);
				_this.$WinCfg.slideDown();
				_this.$WinCfg.$imgCol.trigger("change");
				_this.$WinCfg.$imgWidth.removeClass("alert");
				_this.$WinCfg.$tip1.removeClass("alert");
				return false;
			};
			_this.Submit();
			return false;
		};
		p.closePPCfg=function(evt){
			_this.$WinCfg.hide();
			_this.$WinCfg.ovl.hide();
			return false;
		};
		p.buildPP=function(evt){
			if(_this.$WinCfg.$imgWidth.hasClass("alert")){
				_this.$WinCfg.$imgWidth.select();
				return false;
			};
			var col=_this.$WinCfg.$imgCol.val();
			var w=_this.$WinCfg.$imgWidth.val();
			
			var ct0=$(_this.tplObj.html());
			var li0=ct0.find("li:first"),li1=null;
			var ul0=ct0.find("ul");
			ul0.addClass("c"+col).empty();
			for(var i=0;i<col;i++){
				li1=li0.clone();
				li1.find("img").attr("width",w);
				ul0.append(li1);
			};
			_this.tplObj=$("<div/>").append(ct0);
			
			_this.Submit();
			
			return false;
		};
		p.setPPCol=function(evt){
			_this.$WinCfg.$imgCol.curVal=this.value;
			var w=Math.floor(_this.CurSecSize/this.value-10-6);
			_this.$WinCfg.$imgWidth.removeClass("alert").val(w).effect("highlight");
		};
		p.setPPWidth=function(evt){
			var v=this.value;
			if(!StringUtils.isPlusInt(v)){
				_this.$WinCfg.$imgWidth.addClass("alert");
				_this.$WinCfg.$tip1.addClass("alert");
				//_this.$WinCfg.$imgCol.trigger("change");
				_this.$WinCfg.$imgWidth.select();
				return false;
			};
			//����ͼƬ�����������
			v=parseInt(v);
			var col=Math.floor(_this.CurSecSize/(v+10+6));
			if(col==0){
				_this.$WinCfg.$imgWidth.addClass("alert");
				_this.$WinCfg.$tip1.addClass("alert").effect("highlight");				
				return false;
			};
			_this.$WinCfg.$imgWidth.removeClass("alert");
			_this.$WinCfg.$tip1.removeClass("alert");
			
			col=col>6?6:col;
			if(_this.$WinCfg.$imgCol.curVal>col){
				_this.$WinCfg.$imgCol.val(col);
				_this.$WinCfg.$imgCol.curVal=col;
			};
			
		};
		//����¼�
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddImg);
		this.$WinCfg.find(".btnOK").click(p.buildPP);
		this.$WinCfg.find(".btnNO").click(p.closePPCfg);
		this.$WinCfg.$imgCol.change(p.setPPCol);
		this.$WinCfg.$imgWidth.change(p.setPPWidth);
		//a��ǩ
		//this.$Layout.find("a").click(sohu.diyTplFactory.r);
	
};
sohu.diyTplFactory.Image.prototype.Submit=function(opt){
	var ct={};
	ct.html0=this.tplObj.html();
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type=sohu.diyTplFactory.getClass(this.tplObj.children());
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * �ı�ѡ����
 */
sohu.diyTplFactory.Text=function(opts){
		//�ı�ѡ��
		this.$Layout=$("#cSlide_text");
		this.tplID=null;//ģ���
		var _this=this;
		var p={};
		p.onAddText=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.Submit();
			return false;
		};
		//����¼�
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddText);
		//a��ǩ
		//this.$Layout.find("a").click(sohu.diyTplFactory.r);
		$("#ctText .navBtn").click(sohu.diyTplFactory.r);	
};
sohu.diyTplFactory.Text.prototype.Submit=function(opt){
	var ct={html0:this.tplObj.html()};
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type=sohu.diyTplFactory.getClass(this.tplObj.children());
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * �Զ��б���
 * @param {Object} opts
 */
sohu.diyTplFactory.DynamicList=function(opts){
	var _this=this;
	var p={};
	p.scriptItems=[{"categroyName":"txt","desc":"Ĭ�������б�","name":"std_list","categoryDesc":"�����б�","type":"groovy"},{"categroyName":"txt","desc":"Ĭ���޵������б�","name":"std_list_nodot","categoryDesc":"�����б�","type":"groovy"},{"categroyName":"txt","desc":"ʱ���Ҷ���","name":"std_list_timera","categoryDesc":"�����б�","type":"groovy"},{"categroyName":"txt","desc":"10�����һ����,�����","name":"std_list_b10s","categoryDesc":"�����б�","type":"groovy"},{"categroyName":"txt","desc":"5��һ��,��������","name":"std_subhead","categoryDesc":"�����б�","type":"groovy"},{"categroyName":"txt","desc":"��һ��4����������5��һ��,��������","name":"four_std_subhead","categoryDesc":"�����б�","type":"groovy"},{"categroyName":"pic","desc":"Ĭ��ͼƬ�б�","name":"std_list_pic","categoryDesc":"ͼƬ�б�","type":"groovy"},{"categroyName":"pic","desc":"��ͼ�б�","name":"std_list_zutu","categoryDesc":"ͼƬ�б�","type":"groovy"},{"categroyName":"pic","desc":"�°�ͼƬ�б�","name":"std_list_pic_custom","categoryDesc":"ͼƬ�б�","type":"groovy"},{"categroyName":"pic","desc":"�°���ͼ�б�","name":"std_list_zutu_custom","categoryDesc":"ͼƬ�б�","type":"groovy"},{"categroyName":"video","desc":"Ĭ����Ƶ�б�","name":"std_list_video","categoryDesc":"��Ƶ�б�","type":"groovy"},{"categroyName":"video","desc":"�°���Ƶ�б�","name":"std_list_video_custom","categoryDesc":"��Ƶ�б�","type":"groovy"},{"categroyName":"video","desc":"��ͼ��Ƶ�б�","name":"std_list_video_bpic_custom","categoryDesc":"��Ƶ�б�","type":"groovy"}];
	this.$dlContent=$("#dlContent");
	//��
	p.form={
		dlFlagName:$("#dlFlagName"),				/* ��Ƭ���� */
		txtDLEntity:$("#txtDLEntity"),				/* ����ʵ�� */
		txtDLShowNum:$("#txtDLShowNum"),			/* ��ʾ���� */
		ddlDLSort:$("#ddlDLSort"),					/* �������� */
		ddlDLPriorityMin:$("#ddlDLPriorityMin"),	/* Ȩ��min */
		ddlDLPriorityMax:$("#ddlDLPriorityMax"),	/* Ȩ��max */
		ddlDLSubType:$("#ddlDLSubType"),			/* �������� */
		ddlDLScriptType:$("#ddlDLScriptType"),		/* ��ʾ���� */
		ddlDLScriptName:$("#ddlDLScriptName"),		/* �����ʽ */
		ddlDLTimeType:$("#ddlDLTimeType"),			/* ʱ���ʽ */
		btnPreview:$("#ctDynamicList .btnPreview"),	/* Ԥ����ť */
		btnOK:$("#ctDynamicList .btnOK"),			/* ȷ����ť */
		btnNO:$("#ctDynamicList .btnNO")			/* �رհ�ť */
	};
	p.form.validate=function(){
		p.form.isValid=true;
		p.form.txtDLEntity.curVal=$.trim(p.form.txtDLEntity.val());
		p.form.txtDLShowNum.curVal=$.trim(p.form.txtDLShowNum.val());
		p.form.ddlDLSort.curVal=p.form.ddlDLSort.val();
		p.form.ddlDLPriorityMin.curVal=p.form.ddlDLPriorityMin.val();
		p.form.ddlDLPriorityMax.curVal=p.form.ddlDLPriorityMax.val();
		p.form.ddlDLSubType.curVal=p.form.ddlDLSubType.val();
		p.form.ddlDLScriptType.curVal=p.form.ddlDLScriptType.val();
		p.form.ddlDLScriptName.curVal=p.form.ddlDLScriptName.val();
		p.form.ddlDLTimeType.curVal=p.form.ddlDLTimeType.val();
		
		if(p.form.txtDLEntity.curVal=="")
		{
			alert("����ʵ�岻��Ϊ��");
			p.form.txtDLEntity.focus();
			p.form.isValid=false;
			return;
		};
		if(!StringUtils.isPlusInt(p.form.txtDLShowNum.curVal))
		{
			alert("��ʾ����������1��100�������");
			p.form.txtDLShowNum.focus();
			p.form.isValid=false;
			return;
		};
		p.form.txtDLShowNum.curVal=parseInt(p.form.txtDLShowNum.curVal);
		if(p.form.txtDLShowNum.curVal<1||p.form.txtDLShowNum.curVal>100){
			alert("��ʾ����������1��100�������");
			p.form.isValid=false;
			return;			
		};
		
		if(p.form.ddlDLPriorityMin.curVal>p.form.ddlDLPriorityMax.curVal){
			alert("Ȩ�����ޱ���С�ڵ���Ȩ������");
			p.form.isValid=false;
			return;
		};
		
	};
	//���¼�ע��
	p.form.ddlDLPriorityMin.change(function(evt){
		var v=$(this).val();
		if(v>p.form.ddlDLPriorityMax.val()){
			alert("Ȩ�����ޱ���С�ڵ���Ȩ������");
			p.form.ddlDLPriorityMax.focus();
		};
	});
	p.form.ddlDLPriorityMin.change(function(evt){
		var v=$(this).val();
		if(v<p.form.ddlDLPriorityMin.val()){
			alert("Ȩ�����ޱ���С�ڵ���Ȩ������");
			p.form.ddlDLPriorityMin.focus();
		};
	});
	
	//��ʾ�������������ʽ
	p.form.ddlDLScriptType.change(function(evt){
		var v=$(this).val();
		p.form.ddlDLScriptName.empty();
		$.each(p.scriptItems,function(i,o){
			if(o.categroyName==v){
				p.form.ddlDLScriptName.append('<option value="'+o.name+'">'+o.desc+'</option>');
			};
		});//each
	});
	
	p.form.ddlDLScriptType.trigger("change");
	
	p.form.btnPreview.click(function(evt){
		_this.Preview();
	});
	
	p.form.btnOK.click(function(evt){
		_this.Submit();
	});
	p.form.btnNO.click(function(evt){
		sohu.diyTplFactory.cls();
	});
	
	this.$dlContent.click(function(evt){
		_this.$dlContent.slideUp();
	});
	
	this._p=p;
};
sohu.diyTplFactory.DynamicList.prototype.Submit=function(){
	this._p.form.validate();
	if(!this._p.form.isValid) return false;
	
	this.Preview(-1);
	
	var tempDiv=this.$dlContent.clone().find(".overlay").remove().end();
	var ct={html0:$.trim(tempDiv.html())};
	ct.flash=false;
	ct.tplID="dynamicList";
	ct.isNew=true;
	ct.type="dlist";
	
	sohu.diyTplFactory.submit(ct,true);
};
sohu.diyTplFactory.DynamicList.prototype.Preview=function(popup){
	popup=popup||true;
	this._p.form.validate();
	if(!this._p.form.isValid) return false;
	
	this.BuildList();
	if(popup){
		this.$dlContent.slideDown("fast");
		this.$dlContent.find(".overlay").css("opacity","0.8");
	};

};
sohu.diyTplFactory.DynamicList.prototype.BuildList=function(){
	//������Ӧ��dom
	if(!this._p.form.isValid) return false;
	var ctWrap=$('<div><div class="dlist ct"><ul/></div></div>');
	var ul0=ctWrap.find("ul");
	var ct0=ctWrap.find(".ct");
	var li0=$('<li><a href="http://testcms.sohu.com/20090602/n264287979.shtml" target="_blank">��Ƶ��ҽԺɥ���� ��������ʧ���ƻ��������м�</a><span>(13:00)</span></li>');
	var li1=$('<li><a href="http://testcms.sohu.com/20091010/n267260341.shtml" target="_blank"><img alt="beijing news" src="http://photocdn.sohu.com/20091010/Img267260342_ss.jpg" width="120" height="90"/></a><span><a href="http://testcms.sohu.com/20091010/n267260341.shtml" target="_blank">beijing news</a><br/>��ͼ1��</span></li>');
	var li=null;
	if(this._p.form.ddlDLScriptType.curVal=="txt"){
		for(var i=0;i<this._p.form.txtDLShowNum.curVal;i++){
			li=li0.clone();
			if(this._p.form.ddlDLTimeType.curVal=="-1"){
				li.find("span").remove();
			};
			ul0.append(li);
		};
		ct0.addClass("list");
		
	}else if(this._p.form.ddlDLScriptType.curVal=="pic"){
		ct0.addClass("pp");
		for(var i=0;i<this._p.form.txtDLShowNum.curVal;i++){
			li=li1.clone();
			ul0.append(li);
		};
	}else{
		ct0.addClass("pp");
		for(var i=0;i<this._p.form.txtDLShowNum.curVal;i++){
			li=li1.clone();
			ul0.append(li);
		};
	};
	
	this.$dlContent.find(".ct").remove().end().append(ctWrap.html());
};
/**
 * @author levinhuang
 * ����ģ��ѡ��Ի���ͻ����߼�
 */
sohu.diyCTFactory = function() {
    var p={},pub={};
	p.selectEmptyLine=function(evt){
		p.setContent(line);
		return false;
	};
	p.getAccordionCfg=function(){
		var ckName="sohu.diyCTFactory.tabIndx";
		//��cookie���ϴδ򿪵�tab
		var tabIdx=$.cookie(ckName);
		tabIdx=tabIdx||0;
		//ÿ��tab�ı�ʱ����cookie
		var _onChange=function(evt,ui){
			var idx=ui.newHeader[0].id.split("_")[1];
			$.setCookie(ckName,idx);
		};
		return {
			active:tabIdx,
			change:_onChange
		};
	};
    //private area
    p.initVar = function(opts) { 
		p._line=new sohu.diyTplFactory.Line({});/*����ģ�����*/
		p._flasher=new sohu.diyTplFactory.FocusImg({});/*����ͼ*/
		p._img=new sohu.diyTplFactory.Image({});/*ͼƬ*/
		p._txt=new sohu.diyTplFactory.Text({});/*�ı�*/
		new sohu.diyTplFactory.SecHead({});/* ��Ŀ���� */
		new sohu.diyTplFactory.DynamicList({});/* �Զ��б� */
		new sohu.diyTplFactory.Nav({}); /* ���� */
		pub.$ctWrap=$("#ctWrap");
	};
    p.onLoaded = function() { 
		var cfg=p.getAccordionCfg();
		var $main=$("#addContent").accordion(cfg);
		$("#web_loading").remove();
		if(parent.sohu)
			parent.sohu.diyConsole.toggleLoading();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 