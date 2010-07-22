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
	}
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
	this.width=opts.width||100;/* �߶ȣ� */
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
			_this.height=(_this.height>100||_this.height<1)?10:_this.height;
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
		type:'shline'
	};
	
	sohu.diyTplFactory.submit(ct);
};
/**
 * ����ͼѡ����
 */
sohu.diyTplFactory.FocusImg=function(opts){
		//����ͼѡ��
		this.$Layout=$("#cSlide_focusImg").cycleSlide({
			cssBtnPrev:"#ctFocusImg .btnLeft",
			cssBtnNext:"#ctFocusImg .btnRight",
			step:178,
			cloneItem:true
		});
		this.tplID=null;//flash ģ���
		var _this=this;
		var p={};
		p.onAddFlash=function(evt){
			_this.tplID=this.id;
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
	ct.html0='<div class="ct shflash"></div>';
	ct.flash=true;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type='shflash';
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * ͼƬѡ����
 */
sohu.diyTplFactory.Image=function(opts){
		//����ͼѡ��
		this.$Layout=$("#ctImg");
		this.tplID=null;//ģ���
		var _this=this;
		var p={};
		p.onAddImg=function(evt){
			_this.tplID=this.id;
			_this.tplObj=$(this).find(".ctWrap");
			_this.Submit();
			return false;
		};
		//����¼�
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddImg);
	
};
sohu.diyTplFactory.Image.prototype.Submit=function(opt){
	var ct={};
	ct.html0=this.tplObj.html();
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type="shimage";
	sohu.diyTplFactory.submit(ct,true);
};
/**
 * �ı�ѡ����
 */
sohu.diyTplFactory.Text=function(opts){
		//�ı�ѡ��
		this.$Layout=$("#cSlide_text").cycleSlide({
			cssBtnPrev:"#ctText .btnLeft",
			cssBtnNext:"#ctText .btnRight",
			step:178,
			cloneItem:true
		});
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
	
};
sohu.diyTplFactory.Text.prototype.Submit=function(opt){
	var ct={html0:this.tplObj.html()};
	ct.flash=false;
	ct.tplID=this.tplID;
	ct.isNew=true;
	ct.type="shtext";
	sohu.diyTplFactory.submit(ct,true);
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
    //private area
    p.initVar = function(opts) { 
		p._line=new sohu.diyTplFactory.Line({});/*����ģ�����*/
		p._flasher=new sohu.diyTplFactory.FocusImg({});/*����ͼ*/
		p._img=new sohu.diyTplFactory.Image({});/*ͼƬ*/
		p._txt=new sohu.diyTplFactory.Text({});/*�ı�*/
		pub.$ctWrap=$("#ctWrap");
	};
    p.onLoaded = function() { 
		$("#contentAccordion").accordion();
		$("#web_loading").remove();
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