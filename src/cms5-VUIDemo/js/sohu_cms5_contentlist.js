/**
 * �Ѻ�cms5����ģ���ඨ��
 */
var sohu_cms5_ct={
	/**
	 * �ر�����ѡ��Ի���
	 */
	cls:function(){
		parent.bos.CloseCTDialog();
	},
	/**
	 * �ύ����dom����
	 * @param {Object} ct
	 * @param {Boolean} cls�Ƿ�رնԻ���
	 */
	submit:function(ct,cls){
		sohu_cms5_contentlist.SelectedContent=ct;
		parent.bos.Editor.CurSec.AddContent(ct);
		if (cls) {
			sohu_cms5_ct.cls();
		};
	}
};
/**
 * ���е�������
 */
sohu_cms5_ct.Line=function(opts){
	var _this=this;
	//���Զ���-�û�����
	this.layoutID=opts.layoutID||"ctEmptyLine";//ģ����
	this.css_cpk=opts.css_cpk||".cpk";//cpk cssѡ����
	this.css_txtH=opts.css_txtH||".txtH";//�߶�¼��� cssѡ����
	this.css_btnOK=opts.css_btnOK||".btnOK";
	this.css_cbkLine=opts.css_cbkLine||".cbxLine";
	this.color=opts.color||"#808080";
	this.height=opts.height||10;
	this.cLine=opts.cLine||false;//�Ƿ��м���ʾ�ָ���
	//���Զ���-dom����
	this.$layout=null;//ģ��dom
	this.$layoutCfg=null;//���ý���dom
	this.$txtCpk=null;//dom-��ɫѡ����colorpicker
	this.$txtHeight=null;//dom-�߶������
	this.$btnOK=null;//�ύ��ť
	this.$cbkLine=null;//��ʾ���߸�ѡ��
	
	//˽�г�Ա
	var p={};
	p.initLayout=function(){
		_this.$layout=$(sohu.diyTp[_this.layoutID]);
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
			_this.height=parseInt(this.value);
			_this.height=isNaN(_this.height)?10:_this.height;
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
sohu_cms5_ct.Line.prototype.Submit=function(opt){
	this.cLine=this.$cbkLine[0].checked;
	var line=this.$layout.clone(true);
	var hr=line.find("hr");
	if(this.cLine){line.addClass("cline");hr.css("border-color",this.color);}else{
		hr.remove();
	};
	var css={"height":this.height+'px'};
	line.css("height",this.height+'px');
	line.flash=false;
	sohu_cms5_ct.submit(line);
};
/**
 * ����ͼѡ����
 */
sohu_cms5_ct.FocusImg=function(opts){
		//����ͼѡ��
		this.$Layout=$("#cSlide_focusImg").cycleSlide({
			cssBtnPrev:"#ctFocusImg .btnLeft",
			cssBtnNext:"#ctFocusImg .btnRight",
			step:178,
			cloneItem:true
		});
		this.FlashTplID=null;//flash ģ���
		var _this=this;
		var p={};
		p.onAddFlash=function(evt){
			_this.FlashTplID=this.id;
			_this.Submit();
			return false;
		};
		//����¼�
		this.$Layout.find(".item").hover(
			function(evt){$(this).addClass("on");},
			function(evt){$(this).removeClass("on");}
		).click(p.onAddFlash);
	
};
sohu_cms5_ct.FocusImg.prototype.Submit=function(opt){
	var ct=$('<div class="ct flash"></div>');
	ct.flash=true;
	ct.tplID=this.FlashTplID;
	sohu_cms5_ct.submit(ct,true);
};
/**
 * @author levinhuang
 * ����ģ��ѡ��Ի���ͻ����߼�
 */
var sohu_cms5_contentlist = function() {
    var p={},pub={};
	p.selectEmptyLine=function(evt){
		p.setContent(line)
		return false;
	};
    //private area
    p.initVar = function(opts) { 
		p._line=new sohu_cms5_ct.Line({});/*����ģ�����*/
		p._flasher=new sohu_cms5_ct.FocusImg({});/*����ͼ*/
	};
    p.onLoaded = function() { 
		$("#contentAccordion").accordion();
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