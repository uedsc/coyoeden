/**
 * ��-���ݡ���Ƭ
 * @author levinhuang
 * @param {Object} opts ѡ��{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"vstp_ct",clOn:"vstp_ctOn",scale:false,clElm:"vstp_elm",clElmOn:"vstp_elmOn",isNew:true,addingMode:0},opts||{});
	var _this=this;
	this.Meta=opts.ct;
	this.IsNew=opts.isNew;
	this.$Layout=null;							/*��Validate�����й���*/
	this.Type=opts.ct.type;						/* ���ݲ������ */
	this.Sec=opts.sec;							/* ���� */
	this.Editor=this.Sec.Editor;				/* �����༭�� */
	this.CTEditor=sohu.diyConsole.CTEditor;		/* ���ݱ༭�� */
	this.MaxWidth=this.Sec.Width;				/* ����� */
	this.onDomed=null;							/* ����ӵ�dom����Ļص����� */
	this.IsFlash=false;							/* �Ƿ�flash����ͼ���� */
	this.FlashObj=null;							/* flash���� */
	this.IsEditing=false;						/* �Ƿ��ڱ༭״̬ */
	this.IsActive=false;						/* �Ƿ���ѡ��״̬ */
	this._timerBlink=null;						/* ��˸��ʱ�� */
	this.AddingMode=opts.addingMode;			/* 0Ϊ��ǰ������ӵ����ݣ�1Ϊ��ǰ��Ƭ�·���ӵ����� */							
	
	//private property
	var p={opts:opts};
	this.__p=p;
	
	/* ��֤���ݵ���Ч�� ���ҹ���$Layout*/
	this.Validate();
	if(!this.Validation.valid) return;
	
	/* ID */
	if ((this.ID = this.$Layout.attr("id")) == "") {
		this.ID = "ct_" + this.Type + "_" + StringUtils.RdStr(8);
		this.$Layout.attr("id",this.ID);
	};
	
	//�Ƿ�flash
	this.IsFlash=this.$Layout.flash;
	if(this.IsFlash){
		if(this.IsNew){
			this.onDomed=function(mode){
				this.FlashObj=window["F_"+this.FlashData.pid];
			};	
		}else{
			this.FlashObj=window["F_"+this.FlashData.pid];
		};
		
	};
	/* Persistence to the dom tree */
	if (this.IsNew) {
		if(this.AddingMode==0)
			this.Editor.UpdateCT(this, 1);
		else
			this.CTEditor.UpdateCT(this,1);
	};
	/* Load elements */
	this.LoadElements();
	
	this.BindEvts();
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
		h:this.$Layout.height(),
		w1:this.$Layout.outerWidth(),
		h1:this.$Layout.outerHeight()
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
		this.IsEditing=true;
		sohu.diyConsole.EditingSec=this.Sec;
		sohu.diyConsole.EditingCT=this;
	}else{
		this.Sec.Deactive();
		this.Sec.InlineEditing=false;
		this.IsEditing=false;
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
		case "vstp_flash":
			this.$Layout=this.IsNew?$(this.Meta.html0):this.Meta.$dom;
			this.$FlashData=this.$Layout.find(".vstp_flashData");
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
/**
 * ���ص�ǰ���ݵ�Ԫ��
 */
sohu.diyContent.prototype.LoadElements=function(){
	var _this=this;
	//a��ǩ���¼�ע��
	this.$Layout.find("a").bind("click.edit",function(e){
		//��ʾ�༭��
		_this.ShowChipEditor($(this));
		return false;
	}).end()
		.find("."+this.__p.opts.clElm).unbind(".edit")//�����ֹ���vstp_elm��Ԫ��
		.bind("click.edit",function(e){
			_this.ShowChipEditor($(this));
			return false;
	});	
	return this;
	/*
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clElm);
	items.each(function(i,o){
		new sohu.diyElement({
			$dom:$(o),
			ct:_this
		});
	});
	return this;
	*/
};
/**
 * unload event handlers for the elements
 */
sohu.diyContent.prototype.UnloadElements=function(){
	this.$Layout.find("a").unbind(".edit").end()
		.find("."+this.__p.opts.clElm).unbind(".edit");
		
	return this;
};
/**
 * �Ƴ����ӻ��༭ע����¼�
 */
sohu.diyContent.prototype.UnbindEvts=function(){
	//�Ƴ������¼�
	this.$Layout.unbind(".edit");
	//�Ƴ�Ԫ���¼�
	//this.$Layout.find("."+this.__p.opts.clElm).trigger("evtUnbindEvt");
	return this;
};
/**
 * �󶨿��ӻ��༭���¼�
 */
sohu.diyContent.prototype.BindEvts=function(){
	var p={},_this=this;
	p.mouseEnter=function(evt){
		_this.Active();
		return false;//stop bubbling
	};
	p.mouseLeave=function(evt){
		_this.Deactive();
	};
	
	//���ݵ�����¼�
	this.$Layout.bind("mouseenter.edit",p.mouseEnter);//.bind("mouseleave.edit",p.mouseLeave);
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
	
	return this;
};
/**
 * ɾ����Ƭ����
 */
sohu.diyContent.prototype.Cls=function(){
	this.Sec.RemoveCTByID(this.ID);
	this.$Layout.remove();
};
/**
 * highlight current content
 * @param {Object} opts {color:'red',speed:2000}
 */
sohu.diyContent.prototype.Blink=function(opts){
	clearInterval(this._timerBlink);
	if(arguments.length==1&&opts==false){return this;};
	opts=$.extend({color:'red',speed:2000},opts||{});
	var _i=this,b=function(){
		_i.$Layout.effect("highlight",{color:opts.color},opts.speed/2);
	};
	this._timerBlink=window.setInterval(b,opts.speed);
	b();
	return this;
};
/**
 * active current content
 */
sohu.diyContent.prototype.Active=function(force){
	force=force||false;
	var _i=this;
	//�Ѿ�ѡ��
	if(this.IsActive) return this;
	//����Լ����ڱ༭����״̬������ʾ�༭��
	if(this.IsEditing) return this;
	//���������ק״̬ ������ʾ�༭��
	if(sohu.diyConsole.Dragger.ing) return this;
	//���˴��ڱ༭״̬������ʾ�༭��
	if(sohu.diyConsole.CurCT&&sohu.diyConsole.CurCT.IsEditing&&(!force)) return this;
	//��������һ�����ݶ���
	if(sohu.diyConsole.CurCT){
		sohu.diyConsole.CurCT.Deactive();
	};
	
	//������ط�������
	if(force)
		this.Sec.Active(true);
	
	this.IsActive=true;
	this.Editor.CurCT=this;
	sohu.diyConsole.CurCT=this;
	//_this.ToggleDragger("on");
	this.CTEditor.AttachTo(this).Show();
	//Flash����ͼ
	if(this.IsFlash){
		var d=this.Dim();
		sohu.diyConsole.$FlashHolder.CT=this;
		sohu.diyConsole.$FlashHolder.css({
			top:d.y-1,
			left:d.x-1,
			height:d.h+1,
			width:d.w+1,
			opacity:0.5,
			display:'block'
		}).unbind("click").bind("click",function(evt){
			_i.InlineEdit("on");
			sohu.diyConsole.CurCT=_i;
			_i.EditFlash();
		});
	};
	return this;	
};
/**
 * Deactive current content obj
 */
sohu.diyContent.prototype.Deactive=function(){
	this.Blink(false);
	this.IsActive=false;
	sohu.diyConsole.CTEditor.Detach();
	sohu.diyConsole.CurCT=null;
	return this;
};
/**
 * �������-�����ݵ�ĩβ�������
 * @param {Object} ct ��������ݶ�����{html0:'xx',flash:false,type:'pp'}
 */
sohu.diyContent.prototype.AddContent=function(ct){
	var _this=this;
	if(ct.isNew){
		/* ���� */
		//������Ӧ��diyContentʵ��
		ct=sohu.diyContent.New({sec:this.Sec,ct:ct,addingMode:1});
		if(!ct.Validation.valid){
			alert(ct.Validation.msg);
			return;
		};
		this.Sec.Contents.push(ct);
	}else{
		/* ���� */
		var ct0=this.Sec.GetCTByID(ct.attr("id"));
		if(!ct0) return;
		if(ct.html0==""){
			if (window.confirm("HTML����Ϊ�ջ��߲�����ģ��淶,�Ƿ�ȷ��ɾ��ԭ����?")) {
				ct0.$Layout.remove();
			};
		}else{
			ct0.$Layout.html(ct.html0);
		};//if1	
	};//if0
};
/**
 * ��ʾ��Ƭ�༭��
 * @param {Object} $t ��ǰa��ǩ���߾���vstp_elm���Ԫ��
 */
sohu.diyContent.prototype.ShowChipEditor=function($t){
	if($t.hasClass(this.__p.opts.clElmOn)) return this;
	
	var _this=this;
	sohu.diyChipEditor.Show({
		ct:this,
		tabs:[0],
		$elm:$t,
		onSave:function(dlg){
			dlg.Hide();
		},
		afterShow:function(hash,dlg){
			//�Ƿ�����Ԫ�صġ����ӡ�ɾ�������ơ����ơ���ť
			dlg.$ElmcActs.show();
			dlg.$Elm.addClass(_this.__p.opts.clElmOn);
			//ȷ�ϼ������ݶ���
			dlg.CT.Active(true);
			dlg.CT.InlineEdit("on");			
		},
		afterHide:function(hash,dlg){		
			dlg.CT.InlineEdit("off");			
		}
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
