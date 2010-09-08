/**
 * ��-���ݡ���Ƭ
 * @author levinhuang
 * @param {Object} opts ѡ��{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn",scale:false,clElm:"elm",isNew:true},opts||{});
	var _this=this;
	this.Meta=opts.ct;
	this.IsNew=opts.isNew;
	this.$Layout=null;/*��Validate�����й���*/
	this.Type=opts.ct.type;/* ���ݲ������ */
	this.Sec=opts.sec;//����
	this.Editor=this.Sec.Editor;//�����༭��
	this.MaxWidth=this.Sec.Width;
	this.onDomed=null;/* ����ӵ�dom����Ļص����� */
	this.IsFlash=false;/* �Ƿ�flash����ͼ���� */
	this.FlashObj=null;/* flash���� */
	
	//private property
	var p={opts:opts};
	this.__p=p;
	
	/* ��֤���ݵ���Ч�� ���ҹ���$Layout*/
	this.Validate();
	if(!this.Validation.valid) return;
	
	/* ID */
	if((this.ID=this.$Layout.attr("id"))=="")
		this.ID="ct_"+this.Type+"_"+StringUtils.RdStr(8);
	
	/* Load elements */
	this.LoadElements();
	
	this.BindEvts();
	//�Ƿ�flash
	this.IsFlash=this.$Layout.flash;
	//TODO:��������е�flash��ô����?��Ҫ�޸�sohu.diy.js���������Թ���һ��sohu.diyTp.Flashʵ���������write����
	/*
	if((this.IsFlash=this.$Layout.flash)){
		//��flash������ֳ���
		var fOpt={tplID:this.$Layout.tplID};
		if(opts.scale){fOpt.w=this.MaxWidth;};
		this.onDomed=function(mode){
			this.FlashObj=this.$Layout.flashObj=new sohu.diyTp.Flash(fOpt);
			this.FlashObj.Render(this.$Layout);
		};
	};
	*/
	if(this.IsFlash){
		if(this.IsNew){
			this.onDomed=function(mode){
				this.FlashObj=window["F_"+this.FlashData.pid];
			};	
		}else{
			this.FlashObj=window["F_"+this.FlashData.pid];
		};
		
	};
	this.$Layout.attr("id",this.ID);
};
/**
 * ��ȡ���ݵ�ά����Ϣ
 * @return {Object} {x,y,w,h}
 */
sohu.diyContent.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
/**
 * ����html�༭���༭����
 */
sohu.diyContent.prototype.DoEdit=function(){
	this.Editor.DialogCT("update");
};
/**
 * �༭����ͼ����
 */
sohu.diyContent.prototype.EditFlash=function(){
	sohu.diyDialog.Show('wFlash');
};
/**
 * Notice related objects that current element is being edited.
 * TODO:Use event model to implement this,let related objects register the inline editing event.
 * @param {Object} state
 */
sohu.diyContent.prototype.InlineEdit=function(state){
	if(state=="on"){
		this.Sec.Active();
		this.Sec.InlineEditing=true;
		sohu.diyConsole.EditingSec=this.Sec;
		sohu.diyConsole.EditingCT=this;
	}else{
		this.Sec.Deactive();
		this.Sec.InlineEditing=false;
		//this.$Layout.trigger("mouseleave");
		sohu.diyConsole.EditingSec=null;
		sohu.diyConsole.EditingCT=null;
	}
};
/**
 * ��֤��ǰ�����Ƿ���Ч
 */
sohu.diyContent.prototype.Validate=function(){
	var _this=this;
	this.SetValidation(true);
	
	var commonValidate=function(ct,msg){
		if(_this.IsNew){
			ct.$Layout=$(ct.Meta.html0).filter("."+ct.Type);
		}else{
			ct.$Layout=ct.Meta.$dom;
		};
		
		if(ct.$Layout.length==0||(!ct.$Layout.is("."+ct.__p.opts.cl))){
			ct.SetValidation(false,msg);
		};
	};
	
	switch(this.Type){
		case "ohmygod":
			this.SetValidation(false,"Html���ݲ����Ͽ��ӻ�ר��ģ��淶");
		break;
		case "shtable":
			this.$Layout=$(this.Meta.html0).filter("table");
			if(this.$Layout.length==0){
				this.SetValidation(false,"Html�����ޱ���ǩ");
			}else{
				//��$Layout�滻�ɷ���diy����ģ��淶�Ķ���
				this.$Layout=$("<div/>").addClass(this.__p.opts.cl+" "+this.Type+" clear").append(this.$Layout);
			}
		break;
		case "flash":
			this.$Layout=this.IsNew?$(this.Meta.html0):this.Meta.$dom;
			this.$FlashData=this.$Layout.find(".flashData");
			this.FlashData=$.evalJSON(this.$FlashData.html());
		break;
		default:
			commonValidate(this,"Html���ݲ�����ģ��淶");
		break;
	};
	if(this.Validation.valid){
		$.extend(this.$Layout,this.Meta);
	};
	return this;
};
/**
 * ������֤��Ϣ
 * @param {Boolean} isValid �Ƿ���Ч
 * @param {String} ��֤�����ʾ��Ϣ
 */
sohu.diyContent.prototype.SetValidation=function(isValid,msg){
	this.Validation={
		valid:isValid,
		msg:msg||null
	};
	return this;
};
sohu.diyContent.prototype.LoadElements=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clElm);
	items.each(function(i,o){
		new sohu.diyElement({
			$dom:$(o),
			ct:_this
		});
	});
};
/**
 * �Ƴ����ӻ��༭ע����¼�
 */
sohu.diyContent.prototype.UnbindEvts=function(){
	//�Ƴ������¼�
	this.$Layout.unbind(".edit");
	//�Ƴ�Ԫ���¼�
	//this.$Layout.find("."+this.__p.opts.clElm).trigger("evtUnbindEvt");
};
/**
 * �󶨿��ӻ��༭���¼�
 */
sohu.diyContent.prototype.BindEvts=function(){
	var p={},_this=this;
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(_this.__p.opts.clOn);
		_this.Editor.CurCT=_this;
		sohu.diyConsole.CurCT=_this;
		//��ק�����¼�
		sohu.diyConsole.Dragger.handle.show().appendTo(_this.$Layout);	
			
		sohu.diyConsole.Dragger.handle
		.unbind()
		.bind("mousedown",function(evt){
			sohu.diyConsole.Dragger.ing=true;
			sohu.diyConsole.Dragger.obj=_this;
			_this.Sec.Deactive();	
		}).bind("mouseup",function(evt){
			sohu.diyConsole.Dragger.ing=false;
		});
		
		if(_this.IsFlash){
			var d=_this.Dim();
			sohu.diyConsole.$FlashHolder.css({
				top:d.y-1+17,
				left:d.x-1,
				height:d.h-17,
				width:d.w,
				opacity:0.5,
				display:'block'
			}).unbind("click").bind("click",function(evt){
				_this.InlineEdit("on");
				sohu.diyConsole.CurCT=_this;
				_this.EditFlash();
			});
		};
	};
	p.mouseLeave=function(evt){
		if(_this.Editor.CurArea.IsEditing) return false;
		_this.$Layout.removeClass(_this.__p.opts.clOn);
		sohu.diyConsole.Dragger.handle.hide();
		sohu.diyConsole.CurCT=null;
	};
	
	//���ݵ�����¼�
	this.$Layout.bind("mouseenter.edit",p.mouseEnter).bind("mouseleave.edit",p.mouseLeave);
	//�Զ����¼�
	this.$Layout.unbind("evtBindEvt").bind("evtBindEvt",function(e){
		_this.BindEvts();
		//_this.$Layout.find("."+_this.__p.opts.clElm).trigger("evtBindEvt");
		return false;//ֹͣð��
	});
	this.$Layout.bind("evtUnbindEvt.edit",function(e){
		_this.UnbindEvts();
		return false;//ֹͣð��
	});
};
/*��̬����*/
/**
 * �����е�domԪ�ع���һ��diyContent ����
 * @param {Object} opts ѡ�� {sec,ct}
 */
sohu.diyContent.New=function(opts){
	return new sohu.diyContent(opts);
};
